using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using task4.Models;
using task4.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace task4.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize]
        public IActionResult Index() => View(_userManager.Users.ToList());

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Block(string id)
        {
            if (await IsActiveAsync(id))
            {
                if (id != null)
                {
                    string[] ids = id.Split(',');
                    foreach (var Id in ids)
                    {
                        Console.WriteLine(Id);
                        User user = await _userManager.FindByIdAsync(Id);
                        if (user != null)
                        {
                            user.LockoutEnd = DateTime.Now.AddYears(200);
                            user.Blocked = true;
                            IdentityResult result = await _userManager.UpdateAsync(user);
                        }
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Unblock(string id)
        {
            if (id != null)
            {
                string[] ids = id.Split(',');
                foreach (var Id in ids)
                {
                    Console.WriteLine(Id);
                    User user = await _userManager.FindByIdAsync(Id);
                    if (user != null)
                    {
                        user.LockoutEnd = null;
                        user.Blocked = false;
                        IdentityResult result = await _userManager.UpdateAsync(user);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            if (await IsActiveAsync(id))
            {
            if(id != null)
            {
                string[] ids = id.Split(',');
                foreach(var Id in ids)
                {
                    User user = await _userManager.FindByIdAsync(Id);
                    if (user != null)
                    {
                        IdentityResult result = await _userManager.DeleteAsync(user);
                    }
                }
            }           
            }
            return RedirectToAction("Index");
        }


        public async Task<bool> IsActiveAsync(string id)
        {
            var context = _httpContextAccessor.HttpContext;
            var user = await _userManager.GetUserAsync(context.User);

            if (context.User.Identity.IsAuthenticated && user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
