using Identity.Models;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Identity.ViewModels
{
    public class PlanningUserAlleyViewModel
    {
        public int PlanningID { get; set; }
        public DateTime PlanDate { get; set; }
        public string? Order { get; set; }
        public int AlleyID { get; set; }
        public List<Alley> Alleys { get; set; }
        public string UserID { get; set; }
        public List<ApplicationUser> Users { get; set; }
    }
}