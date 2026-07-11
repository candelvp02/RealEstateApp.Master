using System.ComponentModel.DataAnnotations;
using RealEstateApp.Core.Application.ViewModels.Common;

namespace RealEstateApp.Core.Application.ViewModels.Catalog;

public class PropertyTypeViewModel : BasicViewModel
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = default!;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = default!;

    public int PropertiesCount { get; set; }
}