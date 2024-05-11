namespace Infinite.Server.Authorization;

public class FileAuthorizationRequirement(string folderDirectory) : IAuthorizationRequirement
{
    public string FolderDirectory { get; } = folderDirectory;
}