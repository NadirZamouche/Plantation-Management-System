using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlantProMobileApp.Models
{
    public class Planning
    {
        [Key]
        public int Id { get; set; }
        public DateTime PlanDate { get; set; }
        public string Order { get; set; }
        public Alley Alley { get; set; }
        public ApplicationUser User { get; set; }
    }
}