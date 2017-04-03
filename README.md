# Marriott

My attempt at building a hotel registration system like Marriott.com using SOA and NServiceBus 6. This project is a result of Udi Dahan's Advanced Distributed System Design with SOA & DDD exercise where attendees are asked to design a hotel reservation system.

## Technologies Used
- NServiceBus 6 for the message bus
- EF6/Sql Server for the database. You'll need to have some version of Sql Server installed locally in order for the project to run correctly
- SignalR 
  - SignalR hubs are used to invoke synchronous services directly from the UI/controller layer.
  - this is not a complete implementation, as exception handling/cancellation tokens have not been coded for any SignalR invocations
  - the handlers for the SignalR-dispatched commands using `context.Reply` to send a message back to the "UI"
  - the handlers for the messages sent via `context.Reply` get the HubContext and the specific client that sent the message to becone with, and invokes some javascript on that razor view/page.
  
## My Approach

### High Cohesion
Cohesion is the hardest thing to get right. IMHO, this is what separates "good" SOA-based systems from "bad" ones. Cohesion is the act of identifying and grouping things that change together. Doing this correctly is part of the business boundary discovery process and identifying what data a given boundary should own.

### Transactional Consistency
Identify data that needs to be transactionally consistent on a use case by use case basis. Finding this transactional consistency in data allows us to group data together that changes together (high cohesion) and then implement code that enforces that transactional consistency. This can also be considered part of the boundary identification process. More often than not, the UI is a great tool to discover what data needs to change together transactionally. Combine the UI with the use cases to really get down to the metal.

### Collaborative Data
Identify any data that is collaborative and try to determine an SLA for that data. Since searching for and reserving a room is not only collaborative, but could also have contention, I used the [Reservation Pattern](http://freecontent.manning.com/wp-content/uploads/reservation-pattern.pdf) for these two processes in order to address the collobration and contention around a fixed set of resources (the hotel's rooms)

### Guiding Principles
- rely on asynchronous messaging (fire and forget) to "do things" in the system and publish events when "things are done". 
- validate commands up front before dispatching to give the command the best chance of succeeding. 
- avoid request/response  
- identify the minimal amount of data that needs to be on published events, especially if they are external events that can be subscribed to across boundaries.
- only share stable business abstractions across boundaries
- rely on NServiceBus Sagas to coordinate long running business processes (no polling!!!), handle messages to control the workflow and dispatch messages to "do things" in the system. Sagas are coordinators, not implementors.
- look for compensating actions when commands fail and/or exceptions are thrown. Most of the time, throwing an exception doens't really do much for the end user as they don't know what they did to cause the exception. If they do know what they did to cause the exception, they usually don't know how to fix it. **Compensating business actions are usually at the crux of how a business handles money and resources.** More often that not, we'll see compensating actions come into play with what developers initially mistake as [race conditions](http://udidahan.com/2010/08/31/race-conditions-dont-exist/)

### Boundary Data Isolation
- one boundary cannot talk to or access another boundary's data. Only messaging is allowed.
- instead of going with a database per boundary, I opted instead to use a single physical database with a schema per boundary. This means table names are prefixed with the boundary name (`Reservation.ConfirmedReservation`, `Marketing.RoomType`, etc...)
- all tables are in the Marriott database
- I'm still logically separating the data by boundary, but physically, there is only one database to work with  

## Solution/Project Structure
A couple of notes on the physical structure of the solution and the projects in the solution.

### Business\Marriott.Business
- this is where all boundaries are identified.
- each folder is a business boundary. In a "real" system, these boundaries would likely be broken out into separate projects instead of all within one project to help with separation and not accidently taking a dependency on another boundary's internals
- under each boundary folder (Billing, Guest, Housekeeping, etc...), you'll see folders for Commands, Events (these are internal events: they're only subscribed to by handlers in the same boundary), Data (the DbContext file per boundary) and maybe a Messages folder if the boundary is using request/reply
- any class(es) not under a folder under each boundary-defined folder is most likely an EF controlled "entity" class. These classes end up as tables in the database
- the `SeedingContext` class is a separatre DbContext class that basically populates the SiteSeeded table with whether or not the site has already seeded itself.

### Business\Marriott.Business.Endpoint
- this is the NServiceBus endpoint for all business boundaries
- in a real production system, you'd separate each boundary's endpoint into its own project, and most likely, the endpoint would run as a separate windows service. NServiceBus 6 allows you to run multiple endpoints in the same windows service, so this is not a hard and fast rule. It really depends on the amount of traffic, contention and collaboration you expect per endpoint.
- note the folder structure is the same as the Business\Marriott.Business project
- all boundary folders have a Handler folder which contains the command, event and message handlers
- the `EnsureTablesAreCreatedWhenConfiguringEndpoint` class is kind of a "cheat" in the fact that it coordinates the intial seeding of certain contexts if the site has not been seeded yet. Why is it a cheat? Because it directly talks to multiple boundaries DbContexts directly. If this solution were to have it's boundarys and their respective endpoints broken out into separate projects, there would need to be a more robust seeding solution.

### Business\Marriott.External.Events
- events that are consumed cross-boundary are in this folder. We want a separate project for these events because they're different than "internal" the internal events I mentioned earlier.
- internal events (events published and consumed by the same endpoint) can carry extra boundary-specific data on them
- external events should only carry a very minimal amount of data on them and the data that is carried should only be stable business abstractions.

### ITOps

What goes into ITOps? Things like logging and authorization. This project has no implemenmtation of these cross-cutting concerns as the purpose was to use SOA and NServiceBus to build a distributed hotel reservation system. But ITOps is a place for more than just cross cutting concerns. 

**Any type of functionality that does not lend itself to a specific buiness function goes in ITOps.** So basically, any type of functionality "left over" after all the boundaries have been identified and all the business use cases fulfilled will most likley find their way into ITOps.

For example, in this project, the `PaymentGateway` is an ITOps function because it's being used across mutliple boundaries and handles some of the messages being dispatched from the business boundaries. It's purpose is to invoke a RPC to a fake credit card processing web service. I do this in a transactionally "safe" way by using NServiceBus to handles the "read" side (build up the data I need to send), then the "send" side (this invocation of the web service). In a real life situation, each of these ITOps functions could have their own endpoints, each with their own SLA, SLR, FLR, and retry/exponential back off policies to suit whatever is being invoked by the service.

> note: ITOps is the only other service(s) that is allowed to take a direct dependency on the business-specific boundaries (the other are Composers) to get the job done. ITOps has access to each boundaries data store as well as the ability to consume internal events from boundaries.

### ITOps\Conventions\Marriott.Conventions
- `EndpointConfigurationExtensions` contains any re-used/repeated endpoint setup/configuration
- `MessageConventions` contains conventions for finding classes that are commands/events/messages based on folder structure. This convention based messages definition is much better than having to implement ICommand, IEvent, IMessage on every command, event on message you want NServiceBus to work with.

### ITOps\Web\Marriott.Client.Web
contains the asp.net MVC website

#### Composers
Disclaimer: I totally stole this idea of "composers" from my day job. 

The idea behind composers are they're the only ones (besides ITOps) allowed to reach into different boundaries DbContexts to query for data in order to build a composite UI. For anyone not familiar with what a composite UI or what you'd want to use one, [please read this](http://udidahan.com/2012/06/23/ui-composition-techniques-for-correct-service-boundaries/) and [this](http://udidahan.com/2012/07/09/ui-composition-vs-server-side-orchestration/).

Composers are read-only (never used for writing, use commands for that) and do represnet a point on contention as they can query and work with data from multiple boundaries. What is returned from a composer should be a model that can be used by the MVC razor view to show a UI.

Sometimes I do not compose a ViewModel to return to the UI at all, I just use the raw entity. In theory, if your UI is a functional/use-case-driven UI (not editiable grids) aligned properly with your boundaries and changing data in a transactionally consistent manner, you should not have to map or build ViewModels for your UI.

### ITOps\Marriott.ITOps
- contains individual folders for each process delegated to ITOps.
- in these folders are implementations for Notifications/Email, Invoicing and a PaymentGateway for fake credit card functionality

### ITOps\Marriott.ITOps.Endpoint
- contains the handlers for handling messages dispatched from ITOps\Marriott.ITOps

## Use Cases

The following use cases are part of this project. Some of the sub-items of the use cases have not been built purely at out of me being lazy, or the fact that I do not have any access to to any domain experts, so making the whole thing up seemed like a waste of time.

### Use Case: Search for an available room.

- user will provide you with a check-in and check-out date
- assume one room one guest
- they can see what types of rooms are available (queen, king) and what they cost
- things to consider as part of this UC:
  - how does the system know the rooms are available?
  - take into account the capacity of the hotel, how many of each type of rooms
  - current reservations for those dates
  - extra credit: overbooking strategies/policies

### Use Case: Make a Reservation

- provide first, last name, address, credit card information
- show summary page of reservation
- the dates, room info, billing info, total cost
- then Complete Reservation button
- what happens if there is only one room left and two people are on the site and they click the complete button at the same time?
  - the official "business requirement" is: while we are willing to accept overbooking, we are only willing to accept that up to a point.
  - aka, if we have a 10% overbooking strategy, if we're already at that number when I complete reservation, want to make sure only one person gets the actual room
  - this should happen only occasionally, and should not happen to a large amount of people at the same time
- things to consider:
  - a hold goes on the card for a certain amount of money (the cancellation charge)

### Use Case: Guest Check In
- front desk agent (fda) handles the check-in process: search for the reservation by the guest's email
- need to pull up all information on guest
- extra credit: Additional credit card for incidentals
- allocate a room to the guest based on the reservation
- they are now checked in
- things to consider:
  - make sure when checking in, the cancellation fee is not charged, and the hold on the card goes away
  - now we need a hold on their card for all the night's they'll be staying (new hold or change existing hold?)
  - what happens if putting a hold on the card fails? bad card, etc... how will that affect two people trying to get the same room when there is only one left

### Use Case: Check Out Process
- night before check out, print up bill and slide under door
- if the bill is correct, leave the key in the room, and go
- the guest has left the room
- have they used anything in the mini-bar?

## In Lieu of Domain Experts
Without access to a true domain experts in the realms of hotel reservations, housekeeping or credit card processing, I had to do a good amount of research on my own. As a result, I have a list of URL's I relied on to read/research what information was available about the specific subject I felt I needed a domain expert on. These questions/links can be found in the QuestionsForDomainExperts.txt file

## The ToDo.txt file
Some of the stuff is done, some of the stuff is not done, some of it will probably never be done. In the file are notes that I took about decisions made, reasons for things I did, and overall questions I still have about some of the items I'd like to add.

## How to Run the Solution
Make sure you have a version of Sql Server installed locally on your machine. In Marriott.Client.Web\Web.config and Marriott.Business.Endpoint\App.config change all the `<connectionStrings>`'s DataSource to the name of your local Sql Server instance.

After that, just hit F5, the solution should seed and you can use the user to run through the use cases.
