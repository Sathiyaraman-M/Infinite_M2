using Infinite.Core.Specifications;

namespace Infinite.Core.Features;

public class BlogService(IUnitOfWork unitOfWork) : IBlogService
{
    public async Task<IResult<FullBlogResponse>> GetFullBlog(string id, string userId)
    {
        try
        {
            //Get Blog from database
            var blog = await unitOfWork.GetRepository<Blog>().GetByIdAsync(id) ?? throw new Exception("Blog not found");

            //Create response object
            var blogResponse = new FullBlogResponse
            {
                Id = blog.Id,
                MarkdownContent = blog.Markdown,
                AuthorName = blog.AuthorName,
                CreatedDate = blog.CreatedDate,
                LastEditedDate = blog.LastEditedDate,
                Title = blog.Title,
                AuthorId = blog.UserId,
                Visibility = blog.Visibility
            };
            
            //Check whether personal or some other people's blog and check blog Visibility
            if (userId != blog.UserId && blog.Visibility == Visibility.Private)
                throw new Exception("You don't have access to the specified blog");
            
            //Return the response object
            return await Result<FullBlogResponse>.SuccessAsync(blogResponse);
        }
        catch (Exception e)
        {
            return await Result<FullBlogResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<FullBlogDraftResponse>> GetFullBlogDraft(string id, string userId)
    {
        try
        {
            //Get BlogDraft from database
            var draft = await unitOfWork.GetRepository<BlogDraft>().GetByIdAsync(id) ?? throw new Exception("Draft not found");
            if (draft.UserId != userId)
                throw new Exception("Draft not found");
            
            //Create response object
            var draftResponse = new FullBlogDraftResponse
            {
                Id = draft.Id,
                MarkdownContent = draft.MarkdownContent,
                AuthorName = draft.AuthorName,
                Title = draft.Title,
                AuthorId = draft.UserId,
                SaveDateTime = draft.SaveDateTime,
                BlogId = draft.BlogId
            };
            
            //Return the response object
            return await Result<FullBlogDraftResponse>.SuccessAsync(draftResponse);
        }
        catch (Exception e)
        {
            return await Result<FullBlogDraftResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<List<MinifiedBlogResponse>>> GetRecentNBlogs(string authorId, string userId, string exclude = null, int n = 4)
    {
        try
        {
            var blogs = await unitOfWork.GetRepository<Blog>().Entities
                .Where(x => x.UserId == authorId)
                .WhereIf(!string.IsNullOrEmpty(exclude), x => x.Id != exclude)
                .WhereIf(authorId != userId, x => x.Visibility == Visibility.Public)
                .OrderByDescending(x => x.CreatedDate)
                .Take(n)
                .Select(x => new MinifiedBlogResponse
                {
                    AuthorName = x.AuthorName,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    Title = x.Title
                })
                .ToListAsync();
            return await Result<List<MinifiedBlogResponse>>.SuccessAsync(blogs);
        }
        catch (Exception e)
        {
            return await Result<List<MinifiedBlogResponse>>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<List<MinifiedBlogDraftResponse>>> GetRecentNDrafts(string userId, int n = 4)
    {
        try
        {
            var blogs = await unitOfWork.GetRepository<BlogDraft>().Entities
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.SaveDateTime)
                .Take(n)
                .Select(x => new MinifiedBlogDraftResponse
                {
                    AuthorName = x.AuthorName,
                    Id = x.Id,
                    Title = x.Title,
                    BlogId = x.BlogId,
                    SaveDateTime = x.SaveDateTime
                })
                .ToListAsync();
            return await Result<List<MinifiedBlogDraftResponse>>.SuccessAsync(blogs);
        }
        catch (Exception e)
        {
            return await Result<List<MinifiedBlogDraftResponse>>.FailAsync(e.Message);
        }
    }

    public async Task<PaginatedResult<MinifiedBlogResponse>> GetAllBlogs(int pageNumber, int pageSize, string searchString, string authorId, string userId)
    {
        try
        {
            return await unitOfWork.GetRepository<Blog>().Entities
                .Where(x => x.UserId == authorId)
                .WhereIf(authorId != userId, x => x.Visibility == Visibility.Public)
                .Specify(new BlogSearchFilterSpecification(searchString))
                .OrderByDescending(x => x.CreatedDate)
                .Select(x => new MinifiedBlogResponse()
                {
                    AuthorName = x.AuthorName,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    Title = x.Title
                })
                .ToPaginatedListAsync(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            return PaginatedResult<MinifiedBlogResponse>.Failure(new List<string>() { e.Message });
        }
    }

    public async Task<PaginatedResult<MinifiedBlogDraftResponse>> GetAllDrafts(int pageNumber, int pageSize, string searchString, string userId)
    {
        try
        {
            return await unitOfWork.GetRepository<BlogDraft>().Entities
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.SaveDateTime)
                .Select(x => new MinifiedBlogDraftResponse()
                {
                    AuthorName = x.AuthorName,
                    Id = x.Id,
                    Title = x.Title,
                    SaveDateTime = x.SaveDateTime,
                    BlogId = x.BlogId
                })
                .ToPaginatedListAsync(pageNumber, pageSize);
        }
        catch (Exception e)
        {
            return PaginatedResult<MinifiedBlogDraftResponse>.Failure(new List<string>() { e.Message });
        }
    }

    public async Task<IResult> PublishAsync(EditBlogRequest request, string userId)
    {
        try
        {
            //Throw exceptions for invalid data
            if (string.IsNullOrEmpty(request.Title))
                throw new Exception("Please enter a blog title");
            if (string.IsNullOrEmpty(request.Markdown))
                throw new Exception("Please enter some blog content");

            //Check for valid GUID and decide add/edit operation
            var isGuid = Guid.TryParse(request.BlogId, out var id);
            if (isGuid && id == Guid.Empty)
            {
                //Retrieve AuthorName from User Profile Info
                var authorName = (await unitOfWork.GetRepository<UserProfileInfo>().Entities
                    .FirstAsync(x => x.UserId == userId)).FullName;
                
                //Create a blog object
                var blog = new Blog
                {
                    AuthorName = authorName,
                    CreatedDate = DateTime.Now,
                    Id = Guid.NewGuid().ToString(),
                    LastEditedDate = DateTime.Now,
                    Markdown = request.Markdown,
                    Title = request.Title,
                    UserId = userId,
                    Visibility = request.Visibility
                };
                
                //Add the new blog
                await unitOfWork.GetRepository<Blog>().AddAsync(blog);
                
                //Delete the draft after publish if present
                var draftForNew = await unitOfWork.GetRepository<BlogDraft>().GetByIdAsync(request.DraftId);
                if (draftForNew != null)
                    await unitOfWork.GetRepository<BlogDraft>().DeleteAsync(draftForNew);
                
                //Save changes to database
                await unitOfWork.Commit();
                return await Result.SuccessAsync("Added Blog Successfully");
            }
            //Throw exception if invalid Id found
            if (string.IsNullOrEmpty(request.BlogId) || !isGuid)
                throw new Exception("Invalid Request");
            
            //Get the existingBlog
            var existingBlog = await unitOfWork.GetRepository<Blog>().GetByIdAsync(request.BlogId);
            existingBlog.Title = request.Title;
            existingBlog.Markdown = request.Markdown;
            existingBlog.AuthorName = request.AuthorName;
            existingBlog.Visibility = request.Visibility;
            existingBlog.LastEditedDate = DateTime.Now;
            
            //Update the blog
            await unitOfWork.GetRepository<Blog>().UpdateAsync(existingBlog, request.BlogId);

            //Delete the draft after publish if present
            var draft = await unitOfWork.GetRepository<BlogDraft>().GetByIdAsync(request.DraftId);
            if (draft != null)
                await unitOfWork.GetRepository<BlogDraft>().DeleteAsync(draft);
            
            //Save changes to database
            await unitOfWork.Commit();
            return await Result.SuccessAsync("Updated Blog Successfully");
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    public async Task<IResult> SaveToDraftsAsync(EditBlogRequest request, string userId)
    {
        try
        {
            //Throw exceptions for invalid data
            if (string.IsNullOrEmpty(request.Title))
                throw new Exception("Please enter a blog title");
            if (string.IsNullOrEmpty(request.Markdown))
                throw new Exception("Please enter some blog content");
            
            //Throw exception if invalid BlogId found
            var isBlogIdGuid = Guid.TryParse(request.BlogId, out var blogId);
            if (!isBlogIdGuid || string.IsNullOrEmpty(request.BlogId))
                throw new Exception("Invalid Request");
            
            //Check for valid GUID and decide add/edit operation
            var isGuid = Guid.TryParse(request.DraftId, out var id);
            if (isGuid && id == Guid.Empty)
            {
                //Update DraftId if BlogDraft already exists
                if (blogId != Guid.Empty)
                {
                    var unexpectedExistingDraft = await unitOfWork.GetRepository<BlogDraft>().Entities
                        .FirstOrDefaultAsync(x => x.BlogId == request.BlogId);
                    if (unexpectedExistingDraft != null)
                        id = Guid.Parse(unexpectedExistingDraft.Id);
                }

                //Retrieve AuthorName from User Profile Info
                var authorName = (await unitOfWork.GetRepository<UserProfileInfo>().Entities
                    .FirstAsync(x => x.UserId == userId)).FullName;
                
                //Create a BlogDraft object
                var draft = new BlogDraft
                {
                    AuthorName = authorName,
                    SaveDateTime = DateTime.Now,
                    BlogId = request.BlogId,
                    MarkdownContent = request.Markdown,
                    Title = request.Title,
                    UserId = userId,
                };

                if (id == Guid.Empty)
                {
                    //Add the new BlogDraft
                    id = Guid.NewGuid();
                    draft.Id = id.ToString();
                    await unitOfWork.GetRepository<BlogDraft>().AddAsync(draft);
                }
                else
                {
                    //Update the existing BlogDraft
                    draft.Id = id.ToString();
                    await unitOfWork.GetRepository<BlogDraft>().UpdateAsync(draft, draft.Id);
                }

                //Save changes in database
                await unitOfWork.Commit();
                return await Result.SuccessAsync("Saved to drafts successfully");
            }
            //Throw exception if invalid Id found
            if (string.IsNullOrEmpty(request.DraftId) || !isGuid)
                throw new Exception("Invalid Request");
            
            //Get the existing draft object
            var existingDraft = await unitOfWork.GetRepository<BlogDraft>().GetByIdAsync(request.DraftId);
            existingDraft.Title = request.Title;
            existingDraft.AuthorName = request.AuthorName;
            existingDraft.MarkdownContent = request.Markdown;
            existingDraft.SaveDateTime = DateTime.Now;
            existingDraft.BlogId = request.BlogId;
            existingDraft.UserId = request.UserId;
            
            //Update the existing draft
            await unitOfWork.GetRepository<BlogDraft>().UpdateAsync(existingDraft, request.DraftId);

            //Save changes in database
            await unitOfWork.Commit();
            return await Result.SuccessAsync("Updated the draft successfully");
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    public async Task<IResult> DeleteBlogAsync(string id, string userId)
    {
        try
        {
            var blog = await unitOfWork.GetRepository<Blog>().GetByIdAsync(id) ?? throw new Exception("Blog not found");
            if (blog.UserId != userId)
                throw new Exception("Cannot delete some other's blog");
            await unitOfWork.GetRepository<Blog>().DeleteAsync(blog);
            await unitOfWork.Commit();
            return await Result.SuccessAsync("Deleted successfully");
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    public async Task<IResult> DiscardDraftAsync(string id, string userId)
    {
        try
        {
            var draft = await unitOfWork.GetRepository<BlogDraft>().GetByIdAsync(id) ?? throw new Exception("Draft not found");
            if (draft.UserId != userId)
                throw new Exception("Cannot delete some other's draft");
            await unitOfWork.GetRepository<BlogDraft>().DeleteAsync(draft);
            await unitOfWork.Commit();
            return await Result.SuccessAsync("Deleted successfully");
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }
}