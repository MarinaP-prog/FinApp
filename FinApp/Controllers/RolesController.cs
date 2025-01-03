using FinApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FinApp.Controllers {
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller {
        private UserManager<AppUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        public RolesController(RoleManager<IdentityRole> roleMgr, UserManager<AppUser> userManager) {
            roleManager = roleMgr;
            this.userManager = userManager;
        }

        public ViewResult Index() => View(roleManager.Roles);

        private void Errors(IdentityResult result) {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create([Required] string name) {
            if (ModelState.IsValid) {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            return View(name);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id) {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null) {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            } else
                ModelState.AddModelError("", "No role found");
            return View("Index", roleManager.Roles);
        }

        public async Task<IActionResult> Update(string id) {
            IdentityRole identityRole = await roleManager.FindByIdAsync(id);
            List<AppUser> members = new List<AppUser>();
            List<AppUser> nonMembers = new List<AppUser>();
            foreach (AppUser member in userManager.Users) {
                var list = await userManager.IsInRoleAsync(member, identityRole.Name) ? members : nonMembers;
                list.Add(member);
            }
            return View(new RoleEdit {
                Role = identityRole,
                Members = members,
                NonMembers = nonMembers
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(RoleModification model) {
            IdentityResult result;
            if (ModelState.IsValid) {
                foreach (string userId in model.AddIds ?? new string[] { }) {
                    AppUser appUser = await userManager.FindByIdAsync(userId);
                    if (appUser != null) {
                        result = await userManager.AddToRoleAsync(appUser, model.RoleName);
                        if (!result.Succeeded) 
                            Errors(result);
                        }
                    }
                    foreach (string userId in model.DeleteIds ?? new string[] { }) {
                        AppUser user = await userManager.FindByIdAsync(userId);
                        if (user != null) {
                            result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                            if (!result.Succeeded)
                                Errors(result);

                        }
                    }
                }
                if (ModelState.IsValid)
                    return RedirectToAction(nameof(Index));
                else
                    return await Update(model.RoleId);
            }
        }
    }


