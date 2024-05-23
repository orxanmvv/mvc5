using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication17.Models;
using WebApplication17.ViewModels;

namespace WebApplication17.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager,  SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
                
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register( RegisterVm registerVm)
        {
            if ( !ModelState.IsValid )
            {
                return View(registerVm);

            }
            User user = new User()
            {
                Name = registerVm.Name,
                Surname = registerVm.Surname,
                Email = registerVm.Email,
                UserName=registerVm.Name + registerVm.Surname

            };
            var result = await _userManager.CreateAsync(user, registerVm.Password);
            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                    

                }
                return View();
            }
            return RedirectToAction("Login");

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVm loginVm)
        {
            if ( !ModelState.IsValid )
            {
                return View(loginVm);
            }
            User user2;
            if (loginVm.Email.Contains("@"))
            {
                user2=await _userManager.FindByEmailAsync(loginVm.Email);


            }
            else
            {
                user2 = await _userManager.FindByNameAsync(loginVm.Email);
            }
            if(user2 == null)
            {
                ModelState.AddModelError("", "email yanlis daxil edilib");
                return View();
            }
            var result= await _signInManager.CheckPasswordSignInAsync(user2,loginVm.Password,true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "nirazdan yeniden cehd edin");
                return View();
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "email ve ya password yanlisdir ");

            }
            await _signInManager.SignInAsync(user2, loginVm.RememberMe);
            return RedirectToAction("Index", "Home");


        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
