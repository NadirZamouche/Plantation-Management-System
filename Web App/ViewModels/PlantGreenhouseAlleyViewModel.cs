using Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Identity.ViewModels
{
    public class PlantGreenhouseAlleyViewModel
    {
        public int PlantID { get; set; }

        [Required]
        [MinLength(1)]
        [Display(Name = "Plant")]
        public string Name { get; set; } = null!;

        public float Weight { get; set; }
        public DateTime HarvestDate { get; set; }
        public bool State { get; set; }
        public bool Verification { get; set; }
        public string? Remarks { get; set; }
        public int GreenhouseID { get; set; }
        public List<Greenhouse> Greenhouses { get; set; }
        public int AlleyID { get; set; }
        public List<Alley> Alleys { get; set; }
    }
}