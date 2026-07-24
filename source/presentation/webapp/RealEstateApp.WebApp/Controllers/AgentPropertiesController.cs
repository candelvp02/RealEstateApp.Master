using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Property;
using RealEstateApp.Core.Application.Interfaces.Infrastructure;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.Dtos.Chat;
using RealEstateApp.Core.Application.ViewModels.Chat;
using RealEstateApp.Core.Application.ViewModels.Offer;

namespace RealEstateApp.WebApp.Controllers;

[Authorize(Roles = "Agent")]
public class AgentPropertiesController : Controller
{
    private readonly IPropertyService _propertyService;
    private readonly IPropertyTypeService _propertyTypeService;
    private readonly ISaleTypeService _saleTypeService;
    private readonly IImprovementService _improvementService;
    private readonly IFileService _fileService;
    private readonly IMessageService _messageService;
    private readonly IOfferService _offerService;
    private readonly IAccountService _accountService;

    public AgentPropertiesController(
        IPropertyService propertyService,
        IPropertyTypeService propertyTypeService,
        ISaleTypeService saleTypeService,
        IImprovementService improvementService,
        IFileService fileService,
        IMessageService messageService,
        IOfferService offerService,
        IAccountService accountService)
    {
        _propertyService = propertyService;
        _propertyTypeService = propertyTypeService;
        _saleTypeService = saleTypeService;
        _improvementService = improvementService;
        _fileService = fileService;
        _messageService = messageService;
        _offerService = offerService;
        _accountService = accountService;
    }

    private string CurrentAgentId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    public async Task<IActionResult> AgentHome()
    {
        var properties = await _propertyService.GetByAgentIdAsync(CurrentAgentId);

        if (properties.Count == 0)
        {
            ViewBag.NoPropertiesMessage = "You do not have any properties registered at this time.";
        }

        var items = properties.Select(MapToListItem).ToList();
        return View(items);
    }

    //mant propiedades disp. agent auth
    public async Task<IActionResult> Index()
    {
        var properties = await _propertyService.GetAvailableByAgentIdAsync(CurrentAgentId);

        if (properties.Count == 0)
        {
            ViewBag.NoPropertiesMessage = "You do not have any available properties registered at this time.";
        }

        var items = properties.Select(MapToListItem).ToList();
        return View(items);
    }

    private static PropertyListItemViewModel MapToListItem(Core.Application.Dtos.Property.PropertyDto p)
    {
        return new PropertyListItemViewModel
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
        };
    }

    private async Task PopulateCatalogsAsync(SavePropertyViewModel vm)
    {
        var types = await _propertyTypeService.GetAllAsync();
        var sales = await _saleTypeService.GetAllAsync();
        var improvements = await _improvementService.GetAllAsync();

        vm.PropertyTypes = types.Select(t => new Core.Application.ViewModels.Catalog.PropertyTypeViewModel
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            PropertiesCount = t.PropertiesCount
        }).ToList();

        vm.SaleTypes = sales.Select(s => new Core.Application.ViewModels.Catalog.SaleTypeViewModel
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            PropertiesCount = s.PropertiesCount
        }).ToList();

        vm.Improvements = improvements.Select(i => new Core.Application.ViewModels.Catalog.ImprovementViewModel
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            PropertiesCount = i.PropertiesCount
        }).ToList();
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var types = await _propertyTypeService.GetAllAsync();
        var sales = await _saleTypeService.GetAllAsync();
        var improvements = await _improvementService.GetAllAsync();

        if (types.Count == 0)
        {
            TempData["ErrorMessage"] = "There are no registered property types. You must create at least one property type before registering a property.";
            return RedirectToAction(nameof(Index));
        }
        if (sales.Count == 0)
        {
            TempData["ErrorMessage"] = "There are no registered sale types. You must create at least one sale type before registering a property.";
            return RedirectToAction(nameof(Index));
        }
        if (improvements.Count == 0)
        {
            TempData["ErrorMessage"] = "There are no registered improvements. You must create at least one improvement before registering a property.";
            return RedirectToAction(nameof(Index));
        }

        var vm = new SavePropertyViewModel();
        await PopulateCatalogsAsync(vm);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SavePropertyViewModel model)
    {
        if (model.Images is null || model.Images.Count == 0)
        {
            ModelState.AddModelError(nameof(model.Images), "You must upload at least one property image.");
        }
        else if (model.Images.Count > 4)
        {
            ModelState.AddModelError(nameof(model.Images), "Only up to 4 images are allowed per property.");
        }
        else
        {
            foreach (var img in model.Images)
            {
                if (!_fileService.IsValidImage(img))
                {
                    ModelState.AddModelError(nameof(model.Images), "The uploaded files must be valid images (.jpg, .jpeg, .png).");
                    break;
                }
            }
        }

        if (!ModelState.IsValid)
        {
            await PopulateCatalogsAsync(model);
            return View(model);
        }

        var imagePaths = new List<string>();
        foreach (var img in model.Images!)
        {
            var path = await _fileService.SaveImageAsync(img, "properties");
            imagePaths.Add(path);
        }

        var dto = new SavePropertyDto
        {
            PropertyTypeId = model.PropertyTypeId,
            SaleTypeId = model.SaleTypeId,
            Price = model.Price,
            Description = model.Description,
            Size = model.Size,
            Bedrooms = model.Bedrooms,
            Bathrooms = model.Bathrooms,
            ImprovementIds = model.SelectedImprovementIds,
            ImagePaths = imagePaths,
            AgentId = CurrentAgentId
        };

        await _propertyService.CreateAsync(dto);

        TempData["SuccessMessage"] = "The property was created successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property is null || property.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to modify this property.";
            return RedirectToAction(nameof(Index));
        }
        if (property.Status == "Sold")
        {
            TempData["ErrorMessage"] = "A property that has already been sold cannot be modified.";
            return RedirectToAction(nameof(Index));
        }

        var types = await _propertyTypeService.GetAllAsync();
        var selectedImprovementIds = new List<int>();
        var allImprovements = await _improvementService.GetAllAsync();
        selectedImprovementIds = allImprovements
            .Where(i => property.Improvements.Contains(i.Name))
            .Select(i => i.Id)
            .ToList();

        var vm = new SavePropertyViewModel
        {
            Id = property.Id,
            Code = property.Code,
            PropertyTypeId = types.FirstOrDefault(t => t.Name == property.PropertyTypeName)?.Id ?? 0,
            SaleTypeId = (await _saleTypeService.GetAllAsync()).FirstOrDefault(s => s.Name == property.SaleTypeName)?.Id ?? 0,
            Price = property.Price,
            Description = property.Description,
            Size = property.Size,
            Bedrooms = property.Bedrooms,
            Bathrooms = property.Bathrooms,
            SelectedImprovementIds = selectedImprovementIds,
            CurrentImagePaths = property.Images.Select(i => i.Path).ToList()
        };

        await PopulateCatalogsAsync(vm);
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SavePropertyViewModel model)
    {
        var existing = await _propertyService.GetByIdAsync(id);
        if (existing is null || existing.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to modify this property.";
            return RedirectToAction(nameof(Index));
        }
        if (existing.Status == "Sold")
        {
            TempData["ErrorMessage"] = "A property that has already been sold cannot be modified.";
            return RedirectToAction(nameof(Index));
        }

        var currentImages = existing.Images.Select(i => i.Path).ToList();
        var newImagesCount = model.Images?.Count ?? 0;

        if (currentImages.Count == 0 && newImagesCount == 0)
        {
            ModelState.AddModelError(nameof(model.Images), "The property must keep at least one image.");
        }
        else if (newImagesCount > 4)
        {
            ModelState.AddModelError(nameof(model.Images), "Only up to 4 images are allowed per property.");
        }
        else if (model.Images is not null)
        {
            foreach (var img in model.Images)
            {
                if (!_fileService.IsValidImage(img))
                {
                    ModelState.AddModelError(nameof(model.Images), "The uploaded files must be valid images (.jpg, .jpeg, .png).");
                    break;
                }
            }
        }

        if (!ModelState.IsValid)
        {
            model.Id = id;
            model.Code = existing.Code;
            model.CurrentImagePaths = currentImages;
            await PopulateCatalogsAsync(model);
            return View(model);
        }

        List<string> imagePaths = currentImages;
        if (model.Images is not null && model.Images.Count > 0)
        {
            imagePaths = new List<string>();
            foreach (var img in model.Images)
            {
                var path = await _fileService.SaveImageAsync(img, "properties");
                imagePaths.Add(path);
            }
        }

        var dto = new SavePropertyDto
        {
            Id = id,
            PropertyTypeId = model.PropertyTypeId,
            SaleTypeId = model.SaleTypeId,
            Price = model.Price,
            Description = model.Description,
            Size = model.Size,
            Bedrooms = model.Bedrooms,
            Bathrooms = model.Bathrooms,
            ImprovementIds = model.SelectedImprovementIds,
            ImagePaths = imagePaths,
            AgentId = CurrentAgentId
        };

        await _propertyService.UpdateAsync(id, dto);

        TempData["SuccessMessage"] = "The property was updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property is null || property.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to delete this property.";
            return RedirectToAction(nameof(Index));
        }
        if (property.Status == "Sold")
        {
            TempData["ErrorMessage"] = "A property that has already been sold cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }

        var vm = new DeletePropertyViewModel
        {
            Id = property.Id,
            Code = property.Code,
            PropertyTypeName = property.PropertyTypeName,
            MainImagePath = property.Images.Select(i => i.Path).FirstOrDefault()
        };

        return View(vm);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property is null || property.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to delete this property.";
            return RedirectToAction(nameof(Index));
        }
        if (property.Status == "Sold")
        {
            TempData["ErrorMessage"] = "A property that has already been sold cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }

        await _propertyService.DeleteAsync(id);

        TempData["SuccessMessage"] = "The property was deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(int id)
    {
        var property = await _propertyService.GetByIdAsync(id);
        if (property is null || property.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to view this property.";
            return RedirectToAction(nameof(AgentHome));
        }

        var clientIds = await _messageService.GetClientsWithConversationAsync(id, CurrentAgentId);
        var conversationClients = new List<(string ClientId, string ClientName)>();

        foreach (var clientId in clientIds)
        {
            var client = await _accountService.GetByIdAsync(clientId);
            if (client is not null)
            {
                conversationClients.Add((clientId, $"{client.FirstName} {client.LastName}"));
            }
        }

        var offers = await _offerService.GetByPropertyAsync(id);
        var offerClientIds = offers.Select(o => o.ClientId).Distinct().ToList();
        var offerClients = new List<(string ClientId, string ClientName, int Count, decimal LastAmount, string LastStatus)>();

        foreach (var clientId in offerClientIds)
        {
            var client = await _accountService.GetByIdAsync(clientId);
            var clientOffers = offers.Where(o => o.ClientId == clientId).OrderByDescending(o => o.CreatedAt).ToList();
            if (client is not null && clientOffers.Count > 0)
            {
                offerClients.Add((clientId, $"{client.FirstName} {client.LastName}", clientOffers.Count, clientOffers[0].Amount, clientOffers[0].Status));
            }
        }

        ViewBag.Property = property;
        ViewBag.ConversationClients = conversationClients;
        ViewBag.OfferClients = offerClients;

        return View();
    }

    public async Task<IActionResult> Conversation(int propertyId, string clientId)
    {
        var property = await _propertyService.GetByIdAsync(propertyId);
        if (property is null || property.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to view this conversation.";
            return RedirectToAction(nameof(AgentHome));
        }

        var client = await _accountService.GetByIdAsync(clientId);
        if (client is null)
        {
            TempData["ErrorMessage"] = "The requested client does not exist.";
            return RedirectToAction(nameof(Details), new { id = propertyId });
        }

        var messages = await _messageService.GetConversationAsync(propertyId, clientId, CurrentAgentId);

        ViewBag.PropertyId = propertyId;
        ViewBag.PropertyCode = property.Code;
        ViewBag.ClientId = clientId;
        ViewBag.ClientName = $"{client.FirstName} {client.LastName}";

        var conversation = messages.Select(m => new ConversationViewModel
        {
            Id = m.Id,
            Content = m.Content,
            SenderRole = m.SenderRole,
            CreatedAt = m.CreatedAt
        }).ToList();

        return View(conversation);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SendReply(SendMessageViewModel model)
    {
        var property = await _propertyService.GetByIdAsync(model.PropertyId);
        if (property is null || property.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to reply in this conversation.";
            return RedirectToAction(nameof(AgentHome));
        }

        if (string.IsNullOrWhiteSpace(model.Content))
        {
            TempData["ErrorMessage"] = "You must write a message before sending it.";
            return RedirectToAction(nameof(Conversation), new { propertyId = model.PropertyId, clientId = model.ClientId });
        }

        var dto = new MessageDto
        {
            Content = model.Content,
            SenderRole = "Agent",
            ClientId = model.ClientId!,
            AgentId = CurrentAgentId,
            PropertyId = model.PropertyId
        };

        await _messageService.SendAsync(dto);

        return RedirectToAction(nameof(Conversation), new { propertyId = model.PropertyId, clientId = model.ClientId });
    }

    public async Task<IActionResult> ClientOffers(int propertyId, string clientId)
    {
        var property = await _propertyService.GetByIdAsync(propertyId);
        if (property is null || property.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to view these offers.";
            return RedirectToAction(nameof(AgentHome));
        }

        var client = await _accountService.GetByIdAsync(clientId);
        if (client is null)
        {
            TempData["ErrorMessage"] = "The requested client does not exist.";
            return RedirectToAction(nameof(Details), new { id = propertyId });
        }

        var offers = await _offerService.GetByPropertyAndClientAsync(propertyId, clientId);

        ViewBag.PropertyId = propertyId;
        ViewBag.PropertyCode = property.Code;
        ViewBag.PropertyStatus = property.Status;
        ViewBag.ClientName = $"{client.FirstName} {client.LastName}";

        var items = offers.Select(o => new OfferViewModel
        {
            Id = o.Id,
            Amount = o.Amount,
            Status = o.Status,
            CreatedAt = o.CreatedAt,
            PropertyId = propertyId
        }).OrderByDescending(o => o.CreatedAt).ToList();

        return View(items);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptOffer(int offerId, int propertyId, string clientId)
    {
        var property = await _propertyService.GetByIdAsync(propertyId);
        if (property is null || property.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to manage this offer.";
            return RedirectToAction(nameof(AgentHome));
        }

        try
        {
            await _offerService.AcceptAsync(offerId);
            TempData["SuccessMessage"] = "The offer was accepted successfully and the property was marked as sold.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (KeyNotFoundException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(ClientOffers), new { propertyId, clientId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RejectOffer(int offerId, int propertyId, string clientId)
    {
        var property = await _propertyService.GetByIdAsync(propertyId);
        if (property is null || property.AgentId != CurrentAgentId)
        {
            TempData["ErrorMessage"] = "You do not have permission to manage this offer.";
            return RedirectToAction(nameof(AgentHome));
        }

        try
        {
            await _offerService.RejectAsync(offerId);
            TempData["SuccessMessage"] = "The offer was rejected successfully.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }
        catch (KeyNotFoundException ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(ClientOffers), new { propertyId, clientId });
    }
}