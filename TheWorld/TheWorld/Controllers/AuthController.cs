using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TheWorld.Models;
using TheWorld.ViewModels;
using static System.String;

namespace TheWorld.Controllers
{
  public class AuthController : Controller
  {
    private readonly SignInManager<WorldUsers> _signInManager;

    public AuthController(SignInManager<WorldUsers> signInManager)
    {
      _signInManager = signInManager;
    }

    public IActionResult Login()
    {
      if (User.Identity.IsAuthenticated)
        return RedirectToAction("Trips", "App");

      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login(LoginViewModel vm, string returnUrl)
    {
      if (!ModelState.IsValid)
        return View();

      var signInResult = await _signInManager
        .PasswordSignInAsync(vm.Username, vm.Password, 
          isPersistent: true, 
          lockoutOnFailure: false);

      if (!signInResult.Succeeded)
      {
        ModelState.AddModelError("", "Username or password incorrect");
        return View();
      }
          
      if (IsNullOrWhiteSpace(returnUrl))
        return RedirectToAction("Trips", "Action");

      return Redirect(returnUrl);
    }
    
    [HttpGet]
    public async Task<IActionResult> Logout()
    {
      if (!User.Identity.IsAuthenticated)
        return RedirectToAction("Index", "App");

      await _signInManager.SignOutAsync();

      return RedirectToAction("Index", "App");
    }
  }
}
