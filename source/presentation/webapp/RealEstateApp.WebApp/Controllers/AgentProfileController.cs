using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Interfaces.Infrastructure;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Account;

namespace RealEstateApp.WebApp.Controllers;

[Authorize(Roles = "Agent")]
public class AgentProfileController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IFileService _fileService;

    public AgentProfileController(IAccountService accountService, IFileService fileService)
    {
        _accountService = accountService;
        _fileService = fileService;
    }

    private string CurrentAgentId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> Index()
    {
        var agent = await _accountService.GetByIdAsync(CurrentAgentId);
        if (agent is null)
        {
            TempData["ErrorMessage"] = "Your user could not be found.";
            return RedirectToAction("AgentHome", "AgentProperties");
        }

        var vm = new ProfileViewModel
        {
            Id = agent.Id,
            FirstName = agent.FirstName,
            LastName = agent.LastName,
            Phone = agent.Phone ?? string.Empty,
            CurrentProfilePicturePath = agent.ProfilePicturePath
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            var current = await _accountService.GetByIdAsync(CurrentAgentId);
            model.CurrentProfilePicturePath = current?.ProfilePicturePath;
            return View(model);
        }

        var agent = await _accountService.GetByIdAsync(CurrentAgentId);
        if (agent is null)
        {
            TempData["ErrorMessage"] = "Your user could not be found.";
            return RedirectToAction("AgentHome", "AgentProperties");
        }

        string? newPicturePath = null;

        if (model.ProfilePicture is not null)
        {
            if (!_fileService.IsValidImage(model.ProfilePicture))
            {
                ModelState.AddModelError(nameof(model.ProfilePicture), "The selected file does not have a valid image format.");
                model.CurrentProfilePicturePath = agent.ProfilePicturePath;
                return View(model);
            }

            newPicturePath = await _fileService.SaveImageAsync(model.ProfilePicture, "users");
        }

        var dto = new UserDto
        {
            Id = agent.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Phone = model.Phone,
            Email = agent.Email,
            UserName = agent.UserName,
            ProfilePicturePath = newPicturePath ?? agent.ProfilePicturePath,
            Role = "Agent"
        };

        await _accountService.UpdateAsync(dto);

        TempData["SuccessMessage"] = "Your profile was updated successfully.";
        return RedirectToAction(nameof(Index));
    }
}