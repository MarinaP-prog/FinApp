using FinApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FinApp.Controllers {
    [Authorize]
    public class AccountController : Controller {
        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult AccessDenied() {
            return View();
        }
        public IActionResult Index() {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Login(string returnUrl) { 
        Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login) {
            if (ModelState.IsValid) {
                AppUser appUser = await userManager.FindByNameAsync(login.UserName);
                if (appUser != null) {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(appUser, login.Password, login.Remember, false);
                    if (result.Succeeded) {
                        return Redirect(login.ReturnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(login.UserName), "Login failed: Invalid username or password");
            }
            return View(login);
        }
        public async Task<IActionResult> Logout() {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
