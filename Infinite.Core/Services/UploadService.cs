using Infinite.Core.Interfaces.Services;

namespace Infinite.Core.Services;

public class UploadService : IUploadService
{
    public string UploadAsync(UploadRequest request, string id)
    {
        if (request.Data == null) return string.Empty;
        var streamData = new MemoryStream(request.Data);
        if (streamData.Length > 0)
        {
            var folderName = Path.Combine("Files", id);
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var exists = Directory.Exists(pathToSave);
            if (!exists)
                Directory.CreateDirectory(pathToSave);
            var fileName = request.FileName.Trim('"');
            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);
            if (File.Exists(dbPath))
            {
                dbPath = NextAvailableFilename(dbPath);
                fullPath = NextAvailableFilename(fullPath);
            }

            using var stream = new FileStream(fullPath, FileMode.Create);
            streamData.CopyTo(stream);
            return dbPath;
        }
        else
        {
            return string.Empty;
        }
    }

    private const string NumberPattern = " ({0})";

    private static string NextAvailableFilename(string path)
    {
        // Short-cut if already available
        if (!File.Exists(path))
            return path;

        // If path has extension then insert the number pattern just before the extension and return next filename
        return Path.HasExtension(path) ? GetNextFilename(path.Insert(path.LastIndexOf(Path.GetExtension(path), StringComparison.Ordinal), NumberPattern)) :
            // Otherwise just append the pattern to the path and return next filename
            GetNextFilename(path + NumberPattern);
    }

    private static string GetNextFilename(string pattern)
    {
        var temp = string.Format(pattern, 1);
        //if (tmp == pattern)
        //throw new ArgumentException("The pattern must include an index place-holder", "pattern");

        if (!File.Exists(temp))
            return temp; // short-circuit if no matches

        int min = 1, max = 2; // min is inclusive, max is exclusive/untested

        while (File.Exists(string.Format(pattern, max)))
        {
            min = max;
            max *= 2;
        }

        while (max != min + 1)
        {
            int pivot = (max + min) / 2;
            if (File.Exists(string.Format(pattern, pivot)))
                min = pivot;
            else
                max = pivot;
        }

        return string.Format(pattern, max);
    }
}