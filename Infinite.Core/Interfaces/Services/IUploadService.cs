namespace Infinite.Core.Interfaces.Services;

public interface IUploadService
{
    string Upload(UploadRequest model, string id);
}