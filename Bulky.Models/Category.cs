using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Bulky.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(30)]
        [Required]
        [DisplayName("Category Names")]
        public string Name { get; set; }

        [Required]
        [DisplayName("Display Order")]
        [Range(1, 100000, ErrorMessage = "Display order must be within 1 to 100")]
        public int DisplayOrder { get; set; }
    }
}
