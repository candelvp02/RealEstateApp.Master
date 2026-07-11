using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;

namespace RealEstateApp.WebApp.Controllers;

public class HomeController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyTypeService _propertyTypeService;

    public HomeController(IPropertyService propertyService, IPropertyTypeService propertyTypeService)
    {
        _propertyService = propertyService;
        _propertyTypeService = propertyTypeService;
    }

    public async Task<IActionResult> Index(PropertyFilterViewModel filter)
    {
        var propertyTypes = await _propertyTypeService.GetAllAsync();
        filter.PropertyTypes = propertyTypes
            .Select(pt => new Core.Application.ViewModels.Catalog.PropertyTypeViewModel
            {
                Id = pt.Id,
                Name = pt.Name,
                Description = pt.Description,
                PropertiesCount = pt.PropertiesCount
            })
            .ToList();

        List<PropertyDto> properties;
        var hasFilters = filter.PropertyTypeId.HasValue || filter.MinPrice.HasValue || filter.MaxPrice.HasValue
                          || filter.Bedrooms.HasValue || filter.Bathrooms.HasValue;
        var hasCodeSearch = !string.IsNullOrWhiteSpace(filter.Code);

        if (filter.MinPrice.HasValue && filter.MinPrice.Value < 0)
        {
            ModelState.AddModelError(nameof(filter.MinPrice), "Minimum price cannot be less than zero.");
        }
        if (filter.MaxPrice.HasValue && filter.MaxPrice.Value < 0)
        {
            ModelState.AddModelError(nameof(filter.MaxPrice), "Maximum price cannot be less than zero.");
        }
        if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue && filter.MinPrice > filter.MaxPrice)
        {
            ModelState.AddModelError(nameof(filter.MinPrice), "Minimum price cannot be greater than maximum price.");
        }
        if (filter.Bedrooms.HasValue && filter.Bedrooms < 0)
        {
            ModelState.AddModelError(nameof(filter.Bedrooms), "Bedrooms cannot be less than zero.");
        }
        if (filter.Bathrooms.HasValue && filter.Bathrooms < 0)
        {
            ModelState.AddModelError(nameof(filter.Bathrooms), "Bathrooms cannot be less than zero.");
        }

        if (!ModelState.IsValid)
        {
            ViewBag.NoResultsMessage = "Please correct the filter values entered.";
            return View(new List<PropertyListItemViewModel>());
        }

        if (hasCodeSearch)
        {
            var property = await _propertyService.GetAvailableByCodeAsync(filter.Code!.Trim());
            properties = property is null ? new List<PropertyDto>() : new List<PropertyDto> { property };

            if (properties.Count == 0)
            {
                ViewBag.NoResultsMessage = "No available property was found with the entered code.";
            }
        }
        else if (hasFilters)
        {
            var filterDto = new PropertyFilterDto
            {
                PropertyTypeId = filter.PropertyTypeId,
                MinPrice = filter.MinPrice,
                MaxPrice = filter.MaxPrice,
                Bedrooms = filter.Bedrooms,
                Bathrooms = filter.Bathrooms
            };
            properties = await _propertyService.GetAvailableFilteredAsync(filterDto);

            if (properties.Count == 0)
            {
                ViewBag.NoResultsMessage = "No available properties were found with the selected filters.";
            }
        }
        else
        {
            properties = await _propertyService.GetAvailableAsync();
        }

        var items = properties.Select(p => new PropertyListItemViewModel
        {
            Id = p.Id,
            Code = p.Code,
            PropertyTypeName = p.PropertyTypeName,
            SaleTypeName = p.SaleTypeName,
            MainImagePath = p.Images.Select(i => i.Path).FirstOrDefault(),
            Price = p.Price,
            Size = p.Size,
            Bedrooms = p.Bedrooms,
            Bathrooms = p.Bathrooms,
            Status = p.Status
        }).ToList();

        ViewBag.Filter = filter;
        return View(items);
    }

    public async Task<IActionResult> Details(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);

        if (property is null || property.Status != "Available")
        {
            TempData["ErrorMessage"] = "The requested property does not exist or is not available.";
            return RedirectToAction(nameof(Index));
        }

        var vm = new PropertyDetailsViewModel
        {
            Id = property.Id,
            Code = property.Code,
            PropertyTypeName = property.PropertyTypeName,
            SaleTypeName = property.SaleTypeName,
            Price = property.Price,
            Size = property.Size,
            Bedrooms = property.Bedrooms,
            Bathrooms = property.Bathrooms,
            Description = property.Description,
            Status = property.Status,
            ImagePaths = property.Images.Select(i => i.Path).ToList(),
            Improvements = property.Improvements,
            AgentId = property.AgentId,
            AgentFullName = property.AgentFullName,
            AgentPhone = property.AgentPhone,
            AgentEmail = property.AgentEmail,
            AgentProfilePicturePath = property.AgentProfilePicturePath
        };

        return View(vm);
    }

    public IActionResult Error()
    {
        return View();
    }
}