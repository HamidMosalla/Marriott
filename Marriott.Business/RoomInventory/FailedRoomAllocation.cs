using System;
using System.ComponentModel.DataAnnotations;

namespace Marriott.Business.RoomInventory
{
    //TODO
    //in theory, althougth it's an edge case, a room allocation could fail for the same reservation id for more than one room type
    //I would need more feedback for a domain expoert as to whether this should be modeled explicitly in the system
    public class FailedRoomAllocation
    {
        [Key]
        public Guid ReservationId { get; set; }
    }
}
