using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marriott.Business.RoomInventory
{
    public class RoomNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Number { get; set; }
        public int RoomTypeId { get; set; }
    }
}
