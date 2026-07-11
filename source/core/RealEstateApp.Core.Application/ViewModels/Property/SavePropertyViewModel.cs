using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.ViewModels.Catalog;

namespace RealEstateApp.Core.Application.ViewModels.Property;

public class SavePropertyViewModel
{
    public int Id { get; set; }

    public string? Code { get; set; }

    [Required(ErrorMessage = "Property type is required.")]
    [Display(Name = "Property Type")]
    public int PropertyTypeId { get; set; }

    [Required(ErrorMessage = "Sale type is required.")]
    [Display(Name = "Sale Type")]
    public int SaleTypeId { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Size is required.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Size must be greater than zero.")]
    public decimal Size { get; set; }

    [Required(ErrorMessage = "Bedrooms is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Bedrooms cannot be negative.")]
    public int Bedrooms { get; set; }

    [Required(ErrorMessage = "Bathrooms is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Bathrooms cannot be negative.")]
    public int Bathrooms { get; set; }

    [Required(ErrorMessage = "You must select at least one improvement.")]
    [Display(Name = "Improvements")]
    public List<int> SelectedImprovementIds { get; set; } = new();

    [Display(Name = "Property Images (1 to 4)")]
    public List<IFormFile>? Images { get; set; }

    public List<string>? CurrentImagePaths { get; set; }
    public List<PropertyTypeViewModel>? PropertyTypes { get; set; }
    public List<SaleTypeViewModel>? SaleTypes { get; set; }
    public List<ImprovementViewModel>? Improvements { get; set; }
}