using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.Interfaces.Infrastructure;

namespace RealEstateApp.WebApp.Services;

public class FileService : IFileService
{
    private readonly IWebHostEnvironment _environment;
    private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png" };

    public FileService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public bool IsValidImage(IFormFile file)
    {
        if (file is null || file.Length == 0) return false;

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return AllowedExtensions.Contains(extension);
    }

    public async Task<string> SaveImageAsync(IFormFile file, string folder)
    {
        if (!IsValidImage(file))
            throw new InvalidOperationException("The selected file does not have a valid image format.");

        var uploadsFolder = Path.Combine(_environment.WebRootPath, "images", folder);
        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/images/{folder}/{fileName}";
    }

    public void DeleteImage(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) return;

        var fullPath = Path.Combine(_environment.WebRootPath, path.TrimStart('/').Replace("images/", "images/"));
        var normalizedPath = Path.Combine(_environment.WebRootPath, path.TrimStart('/'));

        if (File.Exists(normalizedPath))
        {
            File.Delete(normalizedPath);
        }
    }
}