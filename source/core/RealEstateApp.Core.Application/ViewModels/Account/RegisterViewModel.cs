using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Core.Application.ViewModels.Account;

public class RegisterViewModel
{
    [Required(ErrorMessage = "First name is required.")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessage = "Last name is required.")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = default!;

    [Required(ErrorMessage = "Phone is required.")]
    public string Phone { get; set; } = default!;

    [Required(ErrorMessage = "Profile picture is required.")]
    [Display(Name = "Profile Picture")]
    public IFormFile ProfilePicture { get; set; } = default!;

    [Required(ErrorMessage = "Username is required.")]
    [Display(Name = "Username")]
    public string UserName { get; set; } = default!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "You must enter a valid email address.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;

    [Required(ErrorMessage = "Password confirmation is required.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Password and confirmation do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = default!;

    [Required(ErrorMessage = "You must select a user type.")]
    [Display(Name = "User Type")]
    public string RoleName { get; set; } = default!;
}