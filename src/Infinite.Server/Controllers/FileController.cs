using Infinite.Base.Enums;
using Infinite.Base.Requests;
using Infinite.Base.Wrapper;

namespace Infinite.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    [HttpGet("user/{userId}")]
    public IActionResult GetUserFiles(string userId)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", userId);
        return Ok(Directory.Exists(folderPath) ? Result<string[]>.Success(Directory.GetFiles(folderPath)) : Result<string[]>.Fail());
    }

    [HttpGet("project/{projectId}")]
    public IActionResult GetProjectFilesList(string projectId)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", projectId);
        return Ok(Directory.Exists(folderPath) ? Result<string[]>.Success(Directory.GetFiles(folderPath)) : Result<string[]>.Fail());
    }

    [HttpGet("project/{projectId}/files")]
    public IActionResult GetProjectFiles(string projectId)
    {
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", projectId);
        var uploads = new List<UploadRequest>();
        foreach (var file in Directory.GetFiles(folderPath))
        {
            var bytes = System.IO.File.ReadAllBytes(file);
            var request = new UploadRequest()
            {
                UploadType = UploadType.Project,
                Data = bytes,
                Extension = Path.GetExtension(file),
                FileName = Path.GetFileName(file)
            };
            uploads.Add(request);
        }
        return Ok(Result<List<UploadRequest>>.Success(uploads));
    }
}