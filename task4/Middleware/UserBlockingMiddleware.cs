using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using task4.Models;



namespace task4.Middleware
{
    public class UserBlockingMiddleware
    {
        private readonly RequestDelegate _next;

        public UserBlockingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, SignInManager<User> signInManager)
        {
            var userManager = context.RequestServices.GetService<UserManager<User>>();
            var user = await userManager.GetUserAsync(context.User);
            if(user == null && context.User.Identity.IsAuthenticated)
            {
                await signInManager.SignOutAsync();
                context.Response.Redirect("/Account/Login");
            }
            if (user != null && user.Blocked)
            {
                await signInManager.SignOutAsync();
                foreach (var cookieKey in context.Request.Cookies.Keys)
                {
                    context.Response.Cookies.Delete(cookieKey);
                }
                context.Response.Redirect("/Account/Login");
                return;
            }
            await _next(context);
        }
    }
}
