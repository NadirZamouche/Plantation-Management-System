using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace PlantProMobileApp.Models
{
    public class Greenhouse
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
