using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.Offer;

public class SendOfferViewModel
{
    public int PropertyId { get; set; }

    [Required(ErrorMessage = "You must enter the offer amount.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "The offer amount must be a numeric value greater than zero.")]
    [Display(Name = "Offer Amount")]
    public decimal Amount { get; set; }
}