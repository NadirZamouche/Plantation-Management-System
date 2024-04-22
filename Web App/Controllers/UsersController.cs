using Identity.Models;
using Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text;

namespace Identity.Controllers
{
    [Authorize (Roles = "Manager")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController (UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users
                .Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = _userManager.GetRolesAsync(user).Result
                })
                .ToList();
            return View(users);
        }

        //GET
        public async Task<IActionResult> ManageRoles(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (User == null)
            {
                return NotFound();
            }

            var roles = await _roleManager.Roles.ToListAsync();

            var viewmodel = new UserRolesViewModel
            {
                UserId = user.Id,
                UserFullName = user.FullName,
                Roles = roles.Select(role => new RoleViewModel  //show only the selected roles!
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    IsSelected = _userManager.IsInRoleAsync(user, role.Name).Result
                }).ToList(),
            };
            return View(viewmodel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ManageRoles(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId); //we make sure the user exists
            if (User == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user); //check all roles in database

            foreach (var role in model.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected) //he has a role and the admin unselected that role 
                {
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName); //the user looses that role
                }
                else if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected) //he doesn't have a role and the admin selected a new role 
                {
                    await _userManager.AddToRoleAsync(user, role.RoleName); //the user obtains that role
                }
            }
            TempData["success"] = "Les rôles ont été modifiés avec succès!";
            return RedirectToAction(nameof(Index));
        }

        //GET
        public IActionResult Add()
        {
            var viewmodel = new AddUserViewModel();
            return View(viewmodel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(AddUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            } 

            // Check if Email already exists
            if (await _userManager.FindByNameAsync(model.Email) != null)
            {
                //ModelState.AddModelError("Email", "Email already exists");
                TempData["error"] = "Cette adresse mail est déjà attribué à un autre utilisateur";
                return View(model);
            }

            // Check if FullName already exists
            bool userExists = await _userManager.Users.AnyAsync(u => u.FullName == model.FullName); //same thing different approach
            if (userExists)
            {
                //ModelState.AddModelError("FullName", "Full name already exists");
                TempData["error"] = "Ce nom et prénom ont déjà été attribués à un autre utilisateur";
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }
            await _userManager.AddToRoleAsync(user, "Agent");  //default role is Agent.
            TempData["success"] = "Utilisateur a été créé avec succès!";
            return RedirectToAction(nameof(Index));
        }

        //GET
        public async Task<IActionResult> Edit(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (User == null)
            {
                return NotFound();
            }

            var viewmodel = new ProfileFormViewModel
            {
                Id = UserId,
                FullName = user.FullName,
                Email= user.Email,
            };
            return View(viewmodel);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProfileFormViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id); //we make sure the user exists
            if (User == null)
            {
                return NotFound();
            }

            var UserWithSameEmail = await _userManager.FindByEmailAsync(model.Email);

            if(UserWithSameEmail != null && UserWithSameEmail.Id != model.Id) //check if an Email already exists for another user.
            {
                //ModelState.AddModelError("Email", "this Email is already assigned to another user");
                TempData["error"] = "Cette adresse mail est déjà attribué à un autre utilisateur";
                return View(model);
            }

            var userWithSameFullName = await _userManager.Users.FirstOrDefaultAsync(u => u.FullName == model.FullName && u.Id != model.Id);
            if (userWithSameFullName != null)
            {
                //ModelState.AddModelError("FullName", "Full name is already assigned to another user");
                TempData["error"] = "Ce nom et prénom ont déjà été attribués à un autre utilisateur";
                return View(model);
            }

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.UserName = model.Email;
            
            await _userManager.UpdateAsync(user);
            TempData["success"] = "Utilisateur a été modifié avec succès!";
            return RedirectToAction(nameof(Index));
        }


        // GET: Users/Delete/{UserId}
        public async Task<IActionResult> Delete(string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user == null)
            {
                return NotFound();
            }

            var viewmodel = new ProfileFormViewModel
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
            };
            return View(viewmodel);
        }

        // POST: Users/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                return NotFound();
            }
            TempData["success"] = "Utilisateur a été supprimé avec succès!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search(string term)
        {
            var users = _userManager.Users
                .Where(u => u.FullName.Contains(term) || u.Email.Contains(term))
                .Select(u => new UserViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email,
                    Roles = _userManager.GetRolesAsync(u).Result
                })
                .ToList();

            return View("Index", users);
        }
    }
}