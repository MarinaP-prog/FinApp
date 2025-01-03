using FinApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.Controllers {
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller {
        private UserManager<AppUser> userManager;
        private IPasswordHasher<AppUser> passwordHasher;
        private IPasswordValidator<AppUser> passwordValidator;

        public AdminController(UserManager<AppUser> usrMgr, IPasswordHasher<AppUser> password, IPasswordValidator<AppUser> passwordValidator) {
            userManager = usrMgr;
            passwordHasher = password;
            this.passwordValidator = passwordValidator;
        }

        public IActionResult Index() {
            return View(userManager.Users);
        }
        public ViewResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> Create(UserVM user) {
            if (ModelState.IsValid) {
                AppUser appUser = new AppUser {
                    UserName = user.Name,
                    Email = user.Email
                };

                IdentityResult result = await userManager.CreateAsync(appUser, user.Password);

                if (result.Succeeded)
                    return RedirectToAction("Index");
                else {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }
        public async Task<IActionResult> Update(string id) {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, string email, string password) {
            AppUser user = await userManager.FindByIdAsync(id);
            if (user != null) {
                IdentityResult validPassword = null;
                if (!string.IsNullOrEmpty(email))
                    user.Email = email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");
               
                if (!string.IsNullOrEmpty(password)) { 
                    validPassword = await passwordValidator.ValidateAsync(userManager, user, password);
                if (validPassword.Succeeded)
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    Errors(validPassword);
            }
            else
                ModelState.AddModelError("", "Password cannot be empty");
            if(validPassword != null && validPassword.Succeeded) {
                    IdentityResult result = await userManager.UpdateAsync(user);

                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
                
            } else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        private void Errors(IdentityResult result) {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string id) {
            AppUser appUser = await userManager.FindByIdAsync(id);
            if (appUser != null) {
                IdentityResult identityResult = await userManager.DeleteAsync(appUser);
                if (identityResult.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(identityResult);
            } else
                ModelState.AddModelError("", "User Not Found");
            return View("Index", userManager.Users);
        }
    }
}

