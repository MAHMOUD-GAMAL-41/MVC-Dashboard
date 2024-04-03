using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
		private readonly SignInManager<ApplicationUser> signInManager;
		private readonly ILogger<AccountController> _logger;

		public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
			ILogger<AccountController> logger



			)
		{
            this.userManager = userManager;
			this.signInManager = signInManager;
			_logger = logger;
		}
        public IActionResult SignUp()
        {
                return View(new SignUpViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel input)
        {
            if(ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Email = input.Email,
                    UserName = input.Email.Split("@")[0],
                    IsActive = true
                };
                var result = await userManager.CreateAsync(user, input.Password);
                
                if(result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                foreach(var error in result.Errors) 
                { 
                ModelState.AddModelError("", error.Description);
                }
            }
            return View(input);
        }
		public IActionResult LogIn()
		{
			return View(new SignInViewModel());
		}
		[HttpPost]
		public async Task<IActionResult> LogIn(SignInViewModel input)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(input.Email);

				if (user is null)
				{
					ModelState.AddModelError("", "Email does not exist");
				}
				else if (await userManager.CheckPasswordAsync(user, input.Password))
				{
					var result = await signInManager.PasswordSignInAsync(user, input.Password, input.RememberMe, false);


					if (result.Succeeded)
					{
						_logger.LogInformation("User logged in.");
						return RedirectToAction("Index", "Home");
					}
					else if (result.IsLockedOut)
					{
						ModelState.AddModelError("", "User account locked out.");
					}
					else
					{
						ModelState.AddModelError("", "Invalid login attempt.");
					}
					}
				else
				{
					_logger.LogInformation("Failed login attempt for user: {0}", input.Email);
					ModelState.AddModelError("", "Invalid email or password.");
				}



			}
			return View(input);
		}
		public async Task<IActionResult> SignOut()
		{
			await signInManager.SignOutAsync();
			return RedirectToAction("Login");
		}
		public IActionResult ForgetPassword()
		{
			return View(new ForgetPasswordViewModel());
		}
		[HttpPost]
		public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel input)
		{
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(input.Email);
				if (user == null)
				{
					ModelState.AddModelError("", "Email does not Exist");
				}
				else
				{
					var token = await userManager.GeneratePasswordResetTokenAsync(user);
					var ResetPasswordLink = Url.Action(
                                                        "ResetPassword",
														"Account",
														 new { Email = input.Email, Token = token },
														 "https");
					var email = new Email
					{
						Title = "Reset Password",
						Body = ResetPasswordLink,
						To = input.Email
					};
					EmailSettings.SendEmail(email);
					return RedirectToAction("Login");
				}
			}
			return View(input);
		}
        public IActionResult ResetPassword(string email,string token)
        {
                return View(new ResetPasswordViewModel());
        }
		[HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel input)
        {
			if (ModelState.IsValid)
			{
				var user = await userManager.FindByEmailAsync(input.Email);
				if (user == null)
				{
					ModelState.AddModelError("", "Email does not Exist");
				}
				else
				{
					var result = await userManager.ResetPasswordAsync(user, input.Token, input.Password);


                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
			}
			return View(input);
		}
		public IActionResult AccessDenied()
		{
			return View(); 
		}
    }
}
