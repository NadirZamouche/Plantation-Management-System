using Identity.Models;
using Identity.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Identity.Controllers
{
    [Authorize(Roles = "Manager")]
    public class AlleyController : Controller
    {
        private readonly IIdentityRepository<Alley> alleyRepository;

        public AlleyController(IIdentityRepository<Alley> alleyRepository)
        {
            this.alleyRepository = alleyRepository;
        }

        // GET
        public ActionResult Index()
        {
            var alleys = alleyRepository.List();
            return View(alleys);
        }

        // GET
        //public ActionResult Details(int id)
        //{
        //    var alley = alleyRepository.Find(id);
        //    return View(alley);
        //}

        // GET
        public ActionResult Create()
        {
            return View();
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Alley alley)
        {
            try
            {
                if (alley.Name == null)
                {
                    //TempData["error"] = "Please give a name for the alley";
                    TempData["error"] = "Veuillez donner un nom pour l'allée";
                    return View();
                }

                // Check if the alley name already exists
                if (alleyRepository.List().Any(a => a.Name == alley.Name))
                {
                    //TempData["error"] = "Alley already exists";
                    TempData["error"] = "Allée existe déjà";
                    return View();
                }

                alleyRepository.Add(alley);
                //TempData["success"] = "Alley created successfully!";
                TempData["success"] = "Allée a été créée avec succès!";
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
            var alley = alleyRepository.Find(id);
            return View(alley);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Alley alley)
        {
            try
            {
                if (alley.Name == null)
                {
                    //TempData["error"] = "Please give a name for the alley";
                    TempData["error"] = "Veuillez donner un nom pour l'allée";
                    return View();
                }

                alleyRepository.Update(id, alley);
                //TempData["success"] = "Alley updated successfully!";
                TempData["success"] = "Allée a été modifiée avec succès!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AlleyController/Delete/5
        public ActionResult Delete(int id)
        {
            var alley = alleyRepository.Find(id);
            return View(alley);
        }

        // POST: AlleyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Alley alley)
        {
            try
            {
                alleyRepository.Delete(id);
                //TempData["success"] = "Alley deleted successfully!";
                TempData["success"] = "Allée a été supprimée avec succès!";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Search(string term)
        {
            var result = alleyRepository.Search(term);
            return View("Index", result);
        }
    }
}