using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using NServiceBus;
using Autofac;
using Autofac.Integration.Mvc;
using Marriott.Conventions;
using System.Reflection;
using Marriott.ITOps.PaymentGateway;

namespace Marriott.Client.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static IEndpointInstance Endpoint;

        protected void Application_Start()
        {
            var containerBuilder = new ContainerBuilder();

            // Register the MVC controllers.
            containerBuilder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Set the dependency resolver to be Autofac.
            var container = containerBuilder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            var endpointConfiguration = new EndpointConfiguration("Marriott.Client.Web");
            endpointConfiguration.MakeInstanceUniquelyAddressable("Marriott.Client.Web");
            endpointConfiguration.ConfigureCommonConfigurations();
            endpointConfiguration.UseContainer<AutofacBuilder>(customizations: customizations => { customizations.ExistingLifetimeScope(container); });
            endpointConfiguration.EnableInstallers();

            Endpoint = NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();

            var updater = new ContainerBuilder();
            updater.RegisterInstance(Endpoint);

            //auto-register composers
            //http://docs.autofac.org/en/latest/register/scanning.html
            var assembly = typeof(MvcApplication).GetTypeInfo().Assembly;
            updater
                .RegisterAssemblyTypes(assembly)
                .Where(t => t.Namespace.StartsWith("Marriott.Client.Web.Composers") && t.GetTypeInfo().IsClass)
                .AsSelf();

            updater.RegisterType<CreditCardService>().As<ICreditCardService>().InstancePerDependency();

            updater.RegisterControllers(typeof(MvcApplication).Assembly);
            var updated = updater.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(updated));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_End()
        {
            Endpoint?.Stop().GetAwaiter().GetResult();
        }
    }
}