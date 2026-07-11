using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Core.Application.ViewModels.Account;

public class ProfileViewModel
{
    public string Id { get; set; } = default!;

    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "Phone is required.")]
    public string Phone { get; set; } = default!;

    [Display(Name = "New Profile Picture")]
    public IFormFile? ProfilePicture { get; set; }

    public string? CurrentProfilePicturePath { get; set; }
}