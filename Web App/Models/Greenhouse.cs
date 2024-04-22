using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Identity.Models
{
    public class Greenhouse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Greenhouse")]
        public string Name { get; set; }
    }
}
