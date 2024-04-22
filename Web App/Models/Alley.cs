using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Identity.Models
{
    public class Alley
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Alley")]
        public string Name { get; set; }
    }
}