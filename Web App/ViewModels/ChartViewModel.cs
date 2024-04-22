using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Identity.ViewModels
{
    public class ChartViewModel
    {
        [Display(Name = "Greenhouses")]
        public int TotalGreenhouses { get; set; }

        [Display(Name = "Plants")]
        public int TotalPlants { get; set; }

        [Display(Name = "Weights(Kg)")]
        public int TotalWeight { get; set; }

        [Display(Name = "Verified")]
        public int PercentageVerified { get; set; }

        [Display(Name = "Weights(Kg)")]
        public List<float> Weights { get; set; }

        [Display(Name = "Good State")]
        public int PercentageState { get; set; }
    }
}
