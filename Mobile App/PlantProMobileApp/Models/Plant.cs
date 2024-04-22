using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlantProMobileApp.Models
{
    public class Plant
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public float Weight { get; set; }
        public DateTime HarvestDate { get; set; }
        public bool State { get; set; }
        public bool Verification { get; set; }
        public string Remarks { get; set; }
        public Greenhouse Greenhouse { get; set; }
        public Alley Alley { get; set; }
    }
}
