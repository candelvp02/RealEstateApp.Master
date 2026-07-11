using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Core.Application.Interfaces.Infrastructure;

public interface IFileService
{
    Task<string> SaveImageAsync(IFormFile file, string folder);
    void DeleteImage(string path);
    bool IsValidImage(IFormFile file);
}