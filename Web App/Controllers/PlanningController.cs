using Identity.Models.Repositories;
using Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Identity.Controllers
{
    [Authorize(Roles = "Manager")]
    public class PlanningController : Controller
    {
        private readonly IIdentityRepository<Planning> planningRepository;
        private readonly IIdentityRepository<Alley> alleyRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public PlanningController(IIdentityRepository<Planning> planningRepository, IIdentityRepository<Alley> alleyRepository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.planningRepository = planningRepository;
            this.alleyRepository = alleyRepository;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET
        public ActionResult Index()
        {
            var plannings = planningRepository.List().ToList();
            return View(plannings);
        }

        //// GET
        //public ActionResult Details(int id)
        //{
        //    var planning = planningRepository.Find(id);
        //    return View(planning);
        //}

        // GET
        public ActionResult Create()
        {
            return View(GetAllUsersAndAlleys());
        }

        // POST: PlanningController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(PlanningUserAlleyViewModel model)
        {
            try
            {
                if (model.UserID == "-1" && model.AlleyID == -1)
                {
                    //TempData["error"] = "Please select a user and an alley from the list";
                    TempData["error"] = "Veuillez sélectionner un agent et une allée dans la liste";
                    return View(GetAllUsersAndAlleys());
                }
                else if (model.UserID == "-1")
                {
                    //TempData["error"] = "Please select a user from the list";
                    TempData["error"] = "Veuillez sélectionner un agent dans la liste";
                    return View(GetAllUsersAndAlleys());
                }
                else if (model.AlleyID == -1)
                {
                    //TempData["error"] = "Please select an alley from the list";
                    TempData["error"] = "Veuillez sélectionner une allée dans la liste";
                    return View(GetAllUsersAndAlleys());
                }

                var user =  await _userManager.FindByIdAsync(model.UserID);
                var alley = alleyRepository.Find(model.AlleyID);

                Planning planning = new Planning
                {
                    Id = model.PlanningID,
                    PlanDate = DateTime.Now,
                    Order = model.Order,
                    Alley = alley,
                    User = user
                };

                planningRepository.Add(planning);
                //TempData["success"] = "Plant created successfully!";
                TempData["success"] = "Le planning a été créé avec succès !";
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
            var planning = planningRepository.Find(id);
            //This one gets only the Agent users
            var agentRole = _roleManager.Roles.FirstOrDefault(r => r.Name == "Agent");
            var agentUsers = _userManager.GetUsersInRoleAsync(agentRole.Name).Result;
            //var userId = planning.User == null ? "1" : planning.User.Id;
            //var alleyId = planning.Alley == null ? 1 : planning.Alley.Id;
            var viewModel = new PlanningUserAlleyViewModel
            {
                PlanningID = planning.Id,
                PlanDate= planning.PlanDate,
                Order = planning.Order,
                UserID = planning.User.Id,
                //Users = _userManager.Users.ToList(), //All
                Users = agentUsers.ToList(), //Only Agents
                AlleyID = planning.Alley.Id,
                Alleys = alleyRepository.List().ToList()
            };
            return View(viewModel);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, PlanningUserAlleyViewModel viewModel)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(viewModel.UserID);
                var alley = alleyRepository.Find(viewModel.AlleyID);

                Planning planning = new Planning
                {
                    Id = viewModel.PlanningID,
                    PlanDate = viewModel.PlanDate,
                    Order = viewModel.Order,
                    User = user,
                    Alley = alley
                };

                planningRepository.Update(viewModel.PlanningID, planning);
                //TempData["success"] = "Plant updated successfully!";
                TempData["success"] = "Planning a été modifié avec succès !";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PlanningController/Delete/5
        public ActionResult Delete(int id)
        {
            var planning = planningRepository.Find(id);
            return View(planning);
        }

        // POST: PlanningController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                planningRepository.Delete(id);
                //TempData["success"] = "Planning deleted successfully!";
                TempData["success"] = "Planning a été supprimé avec succès !";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //This one gets all users no matter what
        //List<ApplicationUser> FillSelectList1()
        //{
        //    var users = _userManager.Users.ToList();
        //    users.Insert(0, new ApplicationUser { Id = "-1", FullName = "--- Please Select a User ---" });
        //    return users;
        //}

        //This one gets only the Agent users
        List<ApplicationUser> FillSelectList1()
        {
            var agentRole = _roleManager.FindByNameAsync("Agent").Result;
            var usersInAgentRole = _userManager.GetUsersInRoleAsync(agentRole.Name).Result;

            //usersInAgentRole.Insert(0, new ApplicationUser { Id = "-1", FullName = "--- Please Select a User ---" });
            usersInAgentRole.Insert(0, new ApplicationUser { Id = "-1", FullName = "--- Veuillez sélectionner un agent ---" });
            return usersInAgentRole.ToList();
        }

        List<Alley> FillSelectList2()
        {
            var alleys = alleyRepository.List().ToList();
            //alleys.Insert(0, new Alley { Id = -1, Name = "--- Please Select an Alley ---" });
            alleys.Insert(0, new Alley { Id = -1, Name = "--- Veuillez sélectionner une allée ---" });
            return alleys;
        }

        PlanningUserAlleyViewModel GetAllUsersAndAlleys()
        {
            var vmodel = new PlanningUserAlleyViewModel
            {
                Users = FillSelectList1(),
                Alleys = FillSelectList2()
            };
            return vmodel;
        }

        public ActionResult Search(string term)
        {
            var result = planningRepository.Search(term);
            return View("Index", result);
        }
    }
}