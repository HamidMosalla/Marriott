using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marriott.Business.Pricing
{
    public class RoomRate
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoomTypeId { get; set; }
        public double Rate { get; set; }
    }
}
