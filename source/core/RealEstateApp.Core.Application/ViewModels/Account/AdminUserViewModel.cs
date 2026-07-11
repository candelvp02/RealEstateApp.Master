using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.Account;

public class AdminUserViewModel
{
    public string? Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "Cedula is required.")]
    public string Cedula { get; set; } = default!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "You must enter a valid email address.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Username is required.")]
    [Display(Name = "Username")]
    public string UserName { get; set; } = default!;

    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Password and confirmation do not match.")]
    [Display(Name = "Confirm Password")]
    public string? ConfirmPassword { get; set; }

    public bool IsActive { get; set; }
}