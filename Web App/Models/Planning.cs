using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Identity.Models
{
    public class Planning
    {
        [Key]
        public int Id { get; set; }
        public DateTime PlanDate { get; set; }
        public string? Order { get; set; }
        public Alley Alley { get; set; }
        public ApplicationUser User { get; set; }
    }
}