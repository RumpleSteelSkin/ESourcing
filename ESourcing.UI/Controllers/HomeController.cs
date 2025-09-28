using ESourcing.Core.Entities;
using ESourcing.UI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ESourcing.UI.Controllers;

public class HomeController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginModel, string? returnUrl)
    {
        returnUrl ??= Url.Content("~/");
        if (!ModelState.IsValid || loginModel.Email == null) return View();
        var user = await userManager.FindByEmailAsync(loginModel.Email);
        if (user != null)
        {
            await signInManager.SignOutAsync();
            if (loginModel.Password == null) return View();
            var result = await signInManager.PasswordSignInAsync(user, loginModel.Password, false, false);
            if (result.Succeeded)
            {
                HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString());
                return LocalRedirect(returnUrl);
            }
        }

        ModelState.AddModelError("", "Email address is not valid or password");
        return View();
    }

    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Signup(AppUserViewModel signupModel)
    {
        if (signupModel.Email != null && await userManager.FindByEmailAsync(signupModel.Email) != null)
            return LocalRedirect(Url.Content("~/"));
        if (!ModelState.IsValid) return View(signupModel);
        var usr = new AppUser
        {
            FirstName = signupModel.FirstName,
            Email = signupModel.Email,
            LastName = signupModel.LastName,
            PhoneNumber = signupModel.PhoneNumber,
            UserName = signupModel.UserName
        };
        if (signupModel.UserSelectTypeId == 1)
        {
            usr.IsBuyer = true;
            usr.IsSeller = false;
        }
        else
        {
            usr.IsSeller = true;
            usr.IsBuyer = false;
        }

        if (signupModel.Password == null) return View(signupModel);
        var result = await userManager.CreateAsync(usr, signupModel.Password);
        if (result.Succeeded)
            return RedirectToAction("Login");
        foreach (var item in result.Errors)
        {
            ModelState.AddModelError("", item.Description);
        }

        return View(signupModel);
    }

    public IActionResult Logout()
    {
        signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}