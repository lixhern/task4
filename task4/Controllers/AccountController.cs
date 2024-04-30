using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using task4.Models;
using task4.ViewModels;

namespace task4.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Name, RegistrationDate = DateTime.Now, AuthorizationDate = DateTime.Now, Blocked = false };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string retrunUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = retrunUrl });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _userManager.FindByNameAsync(model.Name);
                var result =
                    await _signInManager.PasswordSignInAsync(model.Name, model.Password, false, false);
                if (result.Succeeded)
                {

                    user.AuthorizationDate = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction("Index", "Home");
                }
                else
                {

                    if (user != null && user.LockoutEnd != null)
                    {
                        ModelState.AddModelError("", "You have been blocked");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid name or(and) password");
                    }
                }
            }
            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
