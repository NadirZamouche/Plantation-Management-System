using Identity.Models;
using Identity.Models.Repositories;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Identity.Controllers
{
    [Authorize(Roles = "Manager")]
    public class PlantController : Controller
    {
        private readonly IIdentityRepository<Plant> plantRepository;
        private readonly IIdentityRepository<Greenhouse> greenhouseRepository;
        private readonly IIdentityRepository<Alley> alleyRepository;

        public PlantController(IIdentityRepository<Plant> plantRepository, IIdentityRepository<Greenhouse> greenhouseRepository, IIdentityRepository<Alley> alleyRepository)
        {
            this.plantRepository = plantRepository;
            this.greenhouseRepository = greenhouseRepository;
            this.alleyRepository = alleyRepository;
        }

        // GET
        public ActionResult Index()
        {
            var plants = plantRepository.List().ToList();
            return View(plants);
        }

        //// GET
        //public ActionResult Details(int id)
        //{
        //    var plant = plantRepository.Find(id);
        //    return View(plant);
        //}

        // GET
        public ActionResult Create()
        {
            return View(GetAllGreenhouseAndAlleys());
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PlantGreenhouseAlleyViewModel model)
        {
            try
            {
                if (model.Name == null && model.GreenhouseID == -1 && model.AlleyID == -1)
                {
                    //TempData["error"] = "Please give a name and select a greenhouse and an alley from the list";
                    TempData["error"] = "Veuillez donner un nom à la plante, sélectionner une serre et une allée dans la liste";
                    return View(GetAllGreenhouseAndAlleys());
                }
                else if (model.Name == null && model.GreenhouseID == -1)
                {
                    //TempData["error"] = "Please give a name and select a greenhouse from the list";
                    TempData["error"] = "Veuillez donner un nom à la plante et sélectionner une serre dans la liste";
                    return View(GetAllGreenhouseAndAlleys());
                }
                else if (model.Name == null && model.AlleyID == -1)
                {
                    //TempData["error"] = "Please give a name and select an alley from the list";
                    TempData["error"] = "Veuillez donner un nom à la plante et sélectionner une allée dans la liste";
                    return View(GetAllGreenhouseAndAlleys());
                }
                else if (model.GreenhouseID == -1 && model.AlleyID == -1)
                {
                    //TempData["error"] = "Please select a greenhouse and an alley from the list";
                    TempData["error"] = "Veuillez sélectionner une serre et une allée dans la liste";
                    return View(GetAllGreenhouseAndAlleys());
                }
                else if (model.Name == null)
                {
                    //TempData["error"] = "Please give a name";
                    TempData["error"] = "Veuillez donner un nom à la plante";
                    return View(GetAllGreenhouseAndAlleys());
                }
                else if (model.GreenhouseID == -1)
                {
                    //TempData["error"] = "Please select a greenhouse from the list";
                    TempData["error"] = "Veuillez sélectionner une serre dans la liste";
                    return View(GetAllGreenhouseAndAlleys());
                }
                else if (model.AlleyID == -1)
                {
                    //TempData["error"] = "Please select an alley from the list";
                    TempData["error"] = "Veuillez sélectionner une allée dans la liste";
                    return View(GetAllGreenhouseAndAlleys());
                }

                if (plantRepository.List().Any(a => a.Name == model.Name))
                {
                    //TempData["error"] = "Plant already exists";
                    TempData["error"] = "Plante existe déjà";
                    return View(GetAllGreenhouseAndAlleys());
                }

                var greenhouse = greenhouseRepository.Find(model.GreenhouseID);
                var alley = alleyRepository.Find(model.AlleyID);
                Plant plant = new Plant
                {
                    Id = model.PlantID,
                    Name = model.Name,
                    Weight = 0,
                    HarvestDate = DateTime.Now.AddMonths(9),
                    State = true,
                    Verification = true,
                    Remarks = "",
                    Greenhouse = greenhouse,
                    Alley = alley
                };

                plantRepository.Add(plant);
                //TempData["success"] = "Plant created successfully!";
                TempData["success"] = "Plante a été créée avec succès!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET
        public ActionResult Edit(int id)
        {
            var plant = plantRepository.Find(id);
            //var greenhouseId = plant.Greenhouse == null ? 1 : plant.Greenhouse.Id;
            //this is an If Condition saying that if the plant has no greenhouse for greenhouse the
            //first greenhouse will be shown in the edit form list otherwise il will show the first one
            //however i will make both greenhouse and alley requiered in the attributes later on

            //var alleyId = plant.Alley == null ? 1 : plant.Alley.Id;
            //same thing here

            var viewModel = new PlantGreenhouseAlleyViewModel
            {
                PlantID = plant.Id,
                Name = plant.Name,
                Weight = plant.Weight,
                HarvestDate = plant.HarvestDate,
                State = plant.State,
                Verification = plant.Verification,
                Remarks = plant.Remarks,
                GreenhouseID = plant.Greenhouse.Id,
                Greenhouses = greenhouseRepository.List().ToList(),
                AlleyID = plant.Alley.Id,
                Alleys = alleyRepository.List().ToList()
            };
            return View(viewModel);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PlantGreenhouseAlleyViewModel viewModel)
        {
            try
            {
                if (viewModel.Name == null)
                {
                    //TempData["error"] = "Please give a name for the plant";
                    TempData["error"] = "Veuillez donner un nom à la plante";
                    return View(GetAllGreenhouseAndAlleys());
                }

                var greenhouse = greenhouseRepository.Find(viewModel.GreenhouseID);
                var alley = alleyRepository.Find(viewModel.AlleyID);

                Plant plant = new Plant
                {
                    Id = viewModel.PlantID,
                    Name = viewModel.Name,
                    Weight = viewModel.Weight,
                    HarvestDate = viewModel.HarvestDate,
                    State = viewModel.State,
                    Verification = viewModel.Verification,
                    Remarks = viewModel.Remarks,
                    Greenhouse = greenhouse,
                    Alley = alley
                };

                plantRepository.Update(viewModel.PlantID, plant);
                //TempData["success"] = "Plant updated successfully!";
                TempData["success"] = "Plante a été modifiée avec succès!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET
        public ActionResult Delete(int id)
        {
            var plant = plantRepository.Find(id);
            return View(plant);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                plantRepository.Delete(id);
                //TempData["success"] = "Plant deleted successfully!";
                TempData["success"] = "Plante a été supprimée avec succès!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        List<Greenhouse> FillSelectList1()
        {
            var greenhouses = greenhouseRepository.List().ToList();
            //greenhouses.Insert(0, new Greenhouse { Id = -1, Name = "--- Please Select a Greenhouse ---" });
            greenhouses.Insert(0, new Greenhouse { Id = -1, Name = "--- Veuillez sélectionner une serre ---" });
            return greenhouses;
        }

        List<Alley> FillSelectList2()
        {
            var alleys = alleyRepository.List().ToList();
            //alleys.Insert(0, new Alley { Id = -1, Name = "--- Please Select an Alley ---" });
            alleys.Insert(0, new Alley { Id = -1, Name = "--- Veuillez sélectionner une allée ---" });
            return alleys;
        }

        PlantGreenhouseAlleyViewModel GetAllGreenhouseAndAlleys()
        {
            var vmodel = new PlantGreenhouseAlleyViewModel
            {
                Greenhouses = FillSelectList1(),
                Alleys = FillSelectList2()
            };
            return vmodel;
        }

        public ActionResult Search(string term)
        {
            var result = plantRepository.Search(term);
            return View("Index", result);
        }
    }
}