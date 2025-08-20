using FatFood_T2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FatFood_T2.Controllers
{
    public class LoginController : Controller
    {
        private readonly FoodOrderWebNhom2Context _context; 

        public LoginController(FoodOrderWebNhom2Context context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && u.PasswordHash == password);

            if (user == null)
            {
                ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) 
            };

            await HttpContext.SignInAsync(
                IdentityConstants.ApplicationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            switch (user.Role)
            {
                case 2:
                    return RedirectToAction("Dashboard", "Admin");
                case 1:
                    return RedirectToAction("Dashboard", "Owner");
                case 0: 
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUp(User model)
        {
            if (ModelState.IsValid)
            {
                model.Role = 0;
                model.CreatedAt = DateTime.Now;

                model.PasswordHash = model.PasswordHash;

                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Login");
            }
            return View(model);
        }
    }
}
