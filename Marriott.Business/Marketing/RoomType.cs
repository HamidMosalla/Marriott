using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Marriott.Business.Marketing
{
    public class RoomType
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int RoomTypeId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DisplayName("Image URL")]
        public string ImageUrl { get; set; }
    }
}
