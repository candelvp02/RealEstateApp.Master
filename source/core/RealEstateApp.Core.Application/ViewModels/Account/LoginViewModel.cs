using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.Account;

public class LoginViewModel
{
    [Required(ErrorMessage = "Email or username is required.")]
    [Display(Name = "Email or Username")]
    public string UserNameOrEmail { get; set; } = default!;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = default!;
}