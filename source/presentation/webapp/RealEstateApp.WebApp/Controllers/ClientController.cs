using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;

namespace RealEstateApp.WebApp.Controllers;

[Authorize(Roles = "Client")]
public class ClientController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyTypeService _propertyTypeService;
    private readonly IFavoritePropertyService _favoritePropertyService;

    public ClientController(
        IPropertyService propertyService,
        IPropertyTypeService propertyTypeService,
        IFavoritePropertyService favoritePropertyService)
    {
        _propertyService = propertyService;
        _propertyTypeService = propertyTypeService;
        _favoritePropertyService = favoritePropertyService;
    }

    private string CurrentClientId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

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

        if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue && filter.MinPrice > filter.MaxPrice)
        {
            ModelState.AddModelError(nameof(filter.MinPrice), "Minimum price cannot be greater than maximum price.");
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

        var favoriteIds = await _favoritePropertyService.GetFavoritePropertyIdsAsync(CurrentClientId);

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
            Status = p.Status,
            IsFavorite = favoriteIds.Contains(p.Id)
        }).ToList();

        ViewBag.Filter = filter;
        return View(items);
    }

    public async Task<IActionResult> MyProperties()
    {
        var favoriteIds = await _favoritePropertyService.GetFavoritePropertyIdsAsync(CurrentClientId);
        var allProperties = await _propertyService.GetAvailableAsync();

        var favoriteProperties = allProperties.Where(p => favoriteIds.Contains(p.Id)).ToList();

        if (favoriteProperties.Count == 0)
        {
            ViewBag.NoResultsMessage = "You do not have any available favorite properties at this time.";
        }

        var items = favoriteProperties.Select(p => new PropertyListItemViewModel
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
            Status = p.Status,
            IsFavorite = true
        }).ToList();

        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleFavorite(int propertyId, string returnAction)
    {
        var isFavorite = await _favoritePropertyService.IsFavoriteAsync(CurrentClientId, propertyId);

        try
        {
            if (isFavorite)
            {
                await _favoritePropertyService.RemoveAsync(CurrentClientId, propertyId);
                TempData["SuccessMessage"] = "The property was removed from your favorites successfully.";
            }
            else
            {
                await _favoritePropertyService.AddAsync(CurrentClientId, propertyId);
                TempData["SuccessMessage"] = "The property was added to your favorites successfully.";
            }
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return returnAction switch
        {
            "MyProperties" => RedirectToAction(nameof(MyProperties)),
            "Details" => RedirectToAction("Details", new { id = propertyId }),
            _ => RedirectToAction(nameof(Index))
        };
    }
}