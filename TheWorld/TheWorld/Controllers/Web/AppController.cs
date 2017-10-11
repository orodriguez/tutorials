using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Web
{
  public class AppController : Controller
  {
    readonly IMailService _mailService;
    readonly IConfigurationRoot _config;
    readonly IWorldRepository _repository;
    readonly ILogger<AppController> _logger;

    public AppController(
      IMailService mailService, 
      IConfigurationRoot config,
      IWorldRepository repository,
      ILogger<AppController> logger)
    {
      _logger = logger;
      _config = config;
      _mailService = mailService;
      _repository = repository;
    }

    public IActionResult Index() => View();

    [Authorize]
    public IActionResult Trips() => View();

    public IActionResult Contact() => View();

    [HttpPost]
    public IActionResult Contact(ContactViewModel model)
    {
      if (model.Email.Contains("aol.com"))
        ModelState.AddModelError("", "We don't support AOL addresses");

      if (ModelState.IsValid) {
        _mailService.SendMail(_config["MailSettings:ToAddress"], model.Email, "From TheWorld", model.Message);

        ModelState.Clear();

        ViewBag.UserMessage = "Messsage Sent!";
      }

      return View();
    }

    public IActionResult About() => View();
  }
}
