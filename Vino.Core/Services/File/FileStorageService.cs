using BetterThanYou.Core.Interfaces.File;
using BetterThanYou.Core.Interfaces.Services;

namespace BetterThanYou.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
    private readonly string _uploadPath;

    public FileStorageService()
    {
        _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "uploads", "products");
        
        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<string> SaveImageAsync(Stream imageStream, string fileName)
    {
        var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
        var filePath = Path.Combine(_uploadPath, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageStream.CopyToAsync(fileStream);
        }

        return $"/uploads/products/{uniqueFileName}";
    }
}