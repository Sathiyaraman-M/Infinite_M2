namespace Infinite.Core.Interfaces.Services;

public interface IUploadService
{
    string UploadAsync(UploadRequest model, string id);
}