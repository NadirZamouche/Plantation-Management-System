using Identity.Data;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Identity.Controllers
{
    [Authorize]
    public class ChartController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ChartController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var countgreenhouses = _db.Greenhouses.Count();  //total greenhoues

            var countplants = _db.Plants.Count();            //total plants

            var countweight = _db.Plants.Sum(p => p.Weight); //total weights

            var countverifiedPlants = _db.Plants.Count(p => p.Verification == true);
            var percentageverified = (float)countverifiedPlants / countplants * 100; //percentage verified plants

            var weights = _db.Plants.Select(p => p.Weight).ToList();//list of weights

            var countstatePlants = _db.Plants.Count(p => p.State == true);
            var percentagestate = (float)countstatePlants / countplants * 100; //percentage state plants

            if (countplants == 0)
            {
                percentageverified = 0;
                percentagestate = 0;
            }

            var viewModel = new ChartViewModel
            {
                TotalGreenhouses = countgreenhouses,
                TotalPlants = countplants,
                TotalWeight = (int)Math.Round(countweight),
                PercentageVerified = (int)Math.Round(percentageverified),
                Weights = weights,
                PercentageState = (int)Math.Round(percentagestate)
            };

            return View(viewModel);
        }
    }
}