using System.Net.Http.Json;
using System.Security.Claims;
using BlazorSlice.Dialog;
using BlazorSlice.Dialog.Services;
using Infinite.Base.Enums;
using Infinite.Base.Requests;
using Infinite.Base.Responses;
using Infinite.Client.Shared.Dialogs;
using Microsoft.AspNetCore.Components.Forms;

namespace Infinite.Client.Pages.Personals.Projects;

public partial class CreateEditProject
{
    private static string _pageTitle = "Share new project";
    private string _userId;
    private string _userName;

    [Parameter] public Guid? Id { get; set; }

    private EditProjectRequest Model { get; set; } = new();

    private IEnumerable<IBrowserFile> _browserFiles;

    private void OnChangeFiles(InputFileChangeEventArgs args)
    {
        _browserFiles = args.FileCount == 1 ? new [] { args.File } : args.GetMultipleFiles();
    }

    private async Task AddSelectedFiles()
    {
        if (_browserFiles != null && _browserFiles.Count() != 0)
        {
            foreach (var file in _browserFiles)
            {
                var extension = Path.GetExtension(file.Name);
                var fileName = $"{file.Name}";
                var data = new byte[file.Size];
                var result = await file.OpenReadStream(200 * 1024 * 1024).ReadAsync(data);
                Toast.Add("Info", "File Added Successfully", Severity.Info);
                Model.Uploads.Add(new UploadRequest()
                {
                    Data = data,
                    FileName = $"{file.Name}",
                    Extension = extension,
                    UploadType = UploadType.Project
                });
            }
            _browserFiles = null;
            StateHasChanged();
        }
        else
        {
            Toast.Add("Empty", "No Files Added");
        }
    }

    private static string GetBytesReadable(long i)
    {
        // Get absolute value
        var absoluteI = (i < 0 ? -i : i);
        // Determine the suffix and readable value
        string suffix;
        double readable;
        if (absoluteI >= 0x1000000000000000) // Exabyte
        {
            suffix = "EB";
            readable = (i >> 50);
        }
        else if (absoluteI >= 0x4000000000000) // Petabyte
        {
            suffix = "PB";
            readable = (i >> 40);
        }
        else if (absoluteI >= 0x10000000000) // Terabyte
        {
            suffix = "TB";
            readable = (i >> 30);
        }
        else if (absoluteI >= 0x40000000) // Gigabyte
        {
            suffix = "GB";
            readable = (i >> 20);
        }
        else if (absoluteI >= 0x100000) // Megabyte
        {
            suffix = "MB";
            readable = (i >> 10);
        }
        else if (absoluteI >= 0x400) // Kilobyte
        {
            suffix = "KB";
            readable = i;
        }
        else
        {
            return i.ToString("0 B"); // Byte
        }
        // Divide by 1024 to get fractional value
        readable = (readable / 1024);
        // Return formatted number with suffix
        return readable.ToString("0.## ") + suffix;
    }

    protected override async Task OnParametersSetAsync()
    {
        var user = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User;
        _userId = user.FindFirstValue("sub");
        _userName = user.FindFirstValue("name");
        if (Id != Guid.Empty && Id != null)
        {   
            _pageTitle = "Edit the project";
            var result = await ProjectHttpClient.GetFullProject(Id.ToString());
            if (result?.Succeeded ?? false)
            {
                var response = result.Data;
                Model = new EditProjectRequest()
                {
                    Abstract = response.Abstract,
                    Authors = response.Authors,
                    CreatedDate = response.CreatedDate,
                    Description = response.Description,
                    ElaborateObjectives = response.ElaborationObjectives,
                    Requirements = response.Requirements,
                    Id = response.Id,
                    LastEditedDate = response.LastEditedDate,
                    Tags = response.Tags,
                    TeamProject = response.TeamProject,
                    Title = response.Title,
                    Visibility = response.Visibility,
                    Type = response.Type,
                    Objective = response.Objective,
                    CapitalSourceType = response.CapitalSourceType,
                    Complexity = response.Complexity,
                };
                var filesResult =
                    await HttpClient.GetFromJsonAsync<Result<List<UploadRequest>>>($"api/files/project/{Id}/files");
                if (filesResult?.Succeeded ?? false)
                {
                    Model.Uploads = filesResult.Data;
                    StateHasChanged();
                }
                foreach (var message in filesResult?.Messages ?? new List<string>() { "File Uploads Failed" }) 
                {
                    Toast.Add("File Error", message, Severity.Error);
                }
            }
            else
            {
                foreach (var message in result?.Messages ?? new List<string>()) 
                {
                    Toast.Add("Error", message, Severity.Error);
                }
                NavigationManager.NavigateTo("/Personal");
            }
        }
        else
        {
            Model.Authors.Add(new ProjectAuthorResponse(_userId, _userName));
        }
        await base.OnParametersSetAsync();
    }

    private void ChangeAuthorMode(bool teamProject)
    {
        if (Model.TeamProject != teamProject)
        {
            Model.Authors.Clear();
            if(!string.IsNullOrEmpty(_userId) && !string.IsNullOrEmpty(_userName))
                Model.Authors.Add(new ProjectAuthorResponse(_userId, _userName));
        }
        Model.TeamProject = teamProject;
    }

    private async Task UpdateAuthors()
    {
        var parameters = new DialogParameters()
        {
            { nameof(AddEditAuthorsDialog.Authors), Model.Authors }
        };
        var options = new DialogOptions()
        {
            Position = DialogPosition.Centered
        };
        var dialog = DialogService.Show<AddEditAuthorsDialog>("Select Project Authors", parameters, options);
        var result = await dialog.Result;
        if (!result.Cancelled)
        {
            Model.Authors = result.Data as List<ProjectAuthorResponse>;
        }
    }

    private async Task SubmitAsync()
    {
        if(string.IsNullOrEmpty(Model.Id)) Model.Id = Guid.Empty.ToString();
        var result = await ProjectHttpClient.SaveProject(Model);
        if (result.Succeeded)
        {
            Toast.Add("Success", result.Messages[0], Severity.Success);
            NavigationManager.NavigateTo($"/Personal/Projects/{result.Data}/Blogs");
        }
        else
        {
            foreach (var message in result.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
            }
        }
    }
}