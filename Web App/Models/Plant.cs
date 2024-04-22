using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Identity.Models
{
    public class Plant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Plant")]
        public string Name { get; set; }
        public float Weight { get; set; }
        public DateTime HarvestDate { get; set; }
        public bool State { get; set; }
        public bool Verification { get; set; }
        public string? Remarks { get; set; }
        public Greenhouse Greenhouse { get; set; }
        public Alley Alley { get; set; }
    }
}
