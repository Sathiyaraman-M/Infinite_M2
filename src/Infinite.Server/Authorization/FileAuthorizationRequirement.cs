namespace Infinite.Server.Authorization;

public class FileAuthorizationRequirement : IAuthorizationRequirement
{
    public string FolderDirectory { get; }

    public FileAuthorizationRequirement(string folderDirectory)
    {
        FolderDirectory = folderDirectory;
    }
}