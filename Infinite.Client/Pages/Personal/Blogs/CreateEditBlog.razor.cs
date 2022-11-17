using System.Net.Http.Json;
using Infinite.Base.Enums;
using Infinite.Base.Requests;
using Infinite.Base.Responses;
using Microsoft.AspNetCore.Components;

namespace Infinite.Client.Pages.Personal.Blogs;

public partial class CreateEditBlog
{
    [Parameter]
    public Guid? BlogId { get; set; }
    
    [Parameter]
    public Guid? DraftId { get; set; }

    private EditBlogRequest Model { get; set; } = new();

    private string _markdown;
    private string Markdown
    {
        get => _markdown;
        set
        {
            _markdown = value;
            CalculateSize(value);
        }
    }

    private int Rows { get; set; }
    
    private void CalculateSize(string value)
    {
        Rows = Math.Max(value.Split('\n').Length, value.Split('\r').Length);
        Rows = Math.Max(Rows, 2);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (DraftId != Guid.Empty && DraftId != null)
        {
            var result = await HttpClient.GetFromJsonAsync<Result<FullBlogDraftResponse>>($"api/blogs/draft/{DraftId}");
            if (result!.Succeeded)
            {
                var response = result.Data;
                Model = new EditBlogRequest
                {
                    AuthorName = response.AuthorName,
                    BlogId = response.BlogId,
                    CreatedTime = response.SaveDateTime,
                    LastSaveTime = response.SaveDateTime,
                    Markdown = response.MarkdownContent,
                    Title = response.Title,
                    UserId = response.AuthorId,
                    Visibility = Visibility.Public,
                    DraftId = response.Id
                };
                _markdown = response.MarkdownContent;
                CalculateSize(_markdown);
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    Toast.Add("Error", message, Severity.Error);
                }
                NavigationManager.NavigateTo("/Personal");
            }
        }
        else if (BlogId != Guid.Empty && BlogId != null)
        {
            var result = await HttpClient.GetFromJsonAsync<Result<FullBlogResponse>>($"api/blogs/{BlogId}");
            if (result!.Succeeded)
            {
                var response = result.Data;
                Model = new EditBlogRequest
                {
                    AuthorName = response.AuthorName,
                    BlogId = response.Id,
                    CreatedTime = response.CreatedDate,
                    LastSaveTime = response.LastEditedDate,
                    Markdown = response.MarkdownContent,
                    Title = response.Title,
                    UserId = response.AuthorId,
                    Visibility = response.Visibility
                };
                _markdown = response.MarkdownContent;
                CalculateSize(_markdown);
            }
            else
            {
                foreach (var message in result.Messages)
                {
                    Toast.Add("Error", message, Severity.Error);
                }
                NavigationManager.NavigateTo("/Personal");
            }
        }
        await base.OnParametersSetAsync();
    }

    private async Task SaveToDraftsAsync()
    {
        Model.Markdown = Markdown;
        if(string.IsNullOrEmpty(Model.DraftId)) Model.DraftId = Guid.Empty.ToString();
        if(string.IsNullOrEmpty(Model.BlogId)) Model.BlogId = Guid.Empty.ToString();
        var result = await BlogHttpClient.SaveToDrafts(Model);
        if (result.Succeeded)
        {
            Toast.Add("Success", result.Messages.FirstOrDefault(), Severity.Success);
            NavigationManager.NavigateTo("/Personal");
        }
        else
        {
            foreach (var message in result.Messages)
            {
                Toast.Add("Error", message, Severity.Error);
            }
        }
    }

    private async Task SubmitAsync()
    {
        Model.Markdown = Markdown;
        if(string.IsNullOrEmpty(Model.BlogId)) Model.BlogId = Guid.Empty.ToString();
        var result = await BlogHttpClient.PublishAsync(Model);
        if (result.Succeeded)
        {
            Toast.Add("Success", result.Messages[0], Severity.Success);
            NavigationManager.NavigateTo("/Personal");
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