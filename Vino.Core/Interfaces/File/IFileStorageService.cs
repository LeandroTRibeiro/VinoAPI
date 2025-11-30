namespace BetterThanYou.Core.Interfaces.File;

public interface IFileStorageService
{
    Task<string> SaveImageAsync(Stream imageStream, string fileName);
}