using Identity.Models;
using Identity.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Identity.Controllers
{
    [Authorize(Roles = "Manager")]
    public class GreenhouseController : Controller
    {
        private readonly IIdentityRepository<Greenhouse> greenhouseRepository;

        public GreenhouseController(IIdentityRepository<Greenhouse> greenhouseRepository)
        {
            this.greenhouseRepository = greenhouseRepository;
        }

        // GET
        public ActionResult Index()
        {
            var greenhouses = greenhouseRepository.List();
            return View(greenhouses);
        }

        //GET
        //public ActionResult Details(int id)
        //{
        //    var greenhouse = greenhouseRepository.Find(id);
        //    return View(greenhouse);
        //}

        // GET
        public ActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Greenhouse greenhouse)
        {
            try
            {
                if (greenhouse.Name == null)
                {
                    //TempData["error"] = "Please give a name for the greenhouse";
                    TempData["error"] = "Veuillez donner un nom pour la serre";
                    return View();
                }

                if (greenhouseRepository.List().Any(a => a.Name == greenhouse.Name))
                {
                    //TempData["error"] = "Greenhouse already exists";
                    TempData["error"] = "Serre existe déjà";
                    return View();
                }

                greenhouseRepository.Add(greenhouse);
                //TempData["success"] = "Greenhouse created successfully!";
                TempData["success"] = "Serre a été créée avec succès!";
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
            var greehnouse = greenhouseRepository.Find(id);
            return View(greehnouse);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Greenhouse greenhouse)
        {
            try
            {
                if (greenhouse.Name == null)
                {
                    //TempData["error"] = "Please give a name for the greenhouse";
                    TempData["error"] = "Veuillez donner un nom pour la serre";
                    return View();
                }

                greenhouseRepository.Update(id, greenhouse);
                //TempData["success"] = "Greenhouse updated successfully!";
                TempData["success"] = "Serre a été modifiée avec succès!";
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
            var greehnouse = greenhouseRepository.Find(id);
            return View(greehnouse);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Greenhouse greenhouse)
        {
            try
            {
                greenhouseRepository.Delete(id);
                //TempData["success"] = "Greenhouse deleted successfully!";
                TempData["success"] = "Serre a été supprimée avec succès!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Search(string term)
        {
            var result = greenhouseRepository.Search(term);
            return View("Index", result);
        }
    }
}