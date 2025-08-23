using FatFood_T2.Models;
using FatFood_T2.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FatFood_T2.Controllers
{
    public class LoginController : Controller
    {
        private readonly FoodOrderWebNhom2Context _context;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginController(
            FoodOrderWebNhom2Context context,
            SignInManager<IdentityUser> signInManager
            )
        {
            _context = context;
            _signInManager = signInManager;
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
                // ép int -> string để lưu vào Claim
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

            // kiểm tra role (vẫn dùng int)
            switch (user.Role)
            {
                case 2:
                    return RedirectToAction("Index", "Admin"); // fix tam thời  
                case 1:
                    return RedirectToAction("Dashboard", "Owner");
                case 0:
                    return RedirectToAction("Index", "Home");
                default:
                    return RedirectToAction("AccessDenied", "Account");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
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
                if (_context.Users.Any(u => u.Email == model.Email)) {
                    ModelState.AddModelError("Email", "Email exist. Pls try another !");
                }

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                model.Role = 0; // mặc định user thường
                model.CreatedAt = DateTime.Now;

                model.PasswordHash = model.PasswordHash;

                _context.Users.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Index", "Login");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult SignUpRes()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignUpRes(SignUpResViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra Email đã tồn tại chưa
                if (_context.Users.Any(o => o.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                }

                // Kiểm tra Tên nhà hàng đã tồn tại chưa
                if (_context.Restaurants.Any(r => r.Name == model.Name))
                {
                    ModelState.AddModelError("RestaurantName", "Tên nhà hàng đã tồn tại");
                }

                // Nếu có lỗi thì trả về view
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                // Tạo User role = 1
                var user = new User
                {
                    FName = model.FName,
                    Email = model.Email,
                    PasswordHash = model.PasswordHash,
                    Phone = model.Phone,
                    Role = 1,
                    CreatedAt = DateTime.Now
                };
                _context.Users.Add(user);
                _context.SaveChanges();

                // Tạo Restaurant gắn Owner
                var res = new Restaurant
                {
                    OwnerId = user.Id,
                    Name = model.Name,
                    Address = model.Address,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude,
                    Description = model.Description,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                _context.Restaurants.Add(res);
                _context.SaveChanges();

                return RedirectToAction("Index", "Login");
            }

            return View(model);
        }

    }
}