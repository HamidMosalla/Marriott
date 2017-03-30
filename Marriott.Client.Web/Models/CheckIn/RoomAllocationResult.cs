using System.ComponentModel;

namespace Marriott.Client.Web.Models.CheckIn
{
    public class RoomAllocationResult
    {
        public int RoomNumber { get; set; }
        public Result Result { get; set; }
    }

    public enum Result
    {
        Succeeded,
        [Description("In Progress")]
        InProgress,
        Failed
    }
}