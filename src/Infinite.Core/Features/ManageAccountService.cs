using Microsoft.AspNetCore.Identity;

namespace Infinite.Core.Features;

public class ManageAccountService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager) : IManageAccountService
{
    public async Task<IResult<string>> GetPortFolioMd(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            var portFolioMd = await unitOfWork.GetRepository<UserPortfolio>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (portFolioMd != null) 
                return await Result<string>.SuccessAsync(data: portFolioMd.PortfolioMarkdown);
            portFolioMd = new UserPortfolio()
            {
                Id = Guid.NewGuid().ToString(),
                PortfolioMarkdown = string.Empty,
                UserId = userId
            };
            await unitOfWork.GetRepository<UserPortfolio>().AddAsync(portFolioMd);
            await unitOfWork.Commit();
            return await Result<string>.SuccessAsync(data: portFolioMd.PortfolioMarkdown);
        }
        catch (Exception e)
        {
            return await Result<string>.FailAsync(e.Message);
        }
    }

    public async Task<IResult> SavePortFolio(string userId, string markdown)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            var portFolioMd = await unitOfWork.GetRepository<UserPortfolio>().Entities
                .FirstAsync(x => x.UserId == userId);
            portFolioMd.PortfolioMarkdown = markdown;
            await unitOfWork.GetRepository<UserPortfolio>().UpdateAsync(portFolioMd, portFolioMd.Id);
            await unitOfWork.Commit();
            return await Result.SuccessAsync();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    public async Task<IResult<UserProfileInfoResponse>> GetUserProfileInfo(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            var user = await userManager.FindByIdAsync(userId) ?? throw new Exception("Something went wrong");
            var userProfileInfo = await unitOfWork.GetRepository<UserProfileInfo>().Entities
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (userProfileInfo == null)
            {
                userProfileInfo = new UserProfileInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    DateOfBirth = DateTime.MinValue,
                    UserId = user.Id
                };
                await unitOfWork.GetRepository<UserProfileInfo>().AddAsync(userProfileInfo);
                await unitOfWork.Commit();
            }
            var response = new UserProfileInfoResponse()
            {
                Email = user.Email,
                AboutMe = userProfileInfo.AboutMe,
                City = userProfileInfo.City,
                Country = userProfileInfo.Country,
                DateOfBirth = userProfileInfo.DateOfBirth,
                FullName = userProfileInfo.FullName,
                Mobile = user.PhoneNumber,
                Name = user.UserName,
                Status = userProfileInfo.Status
            };
            return await Result<UserProfileInfoResponse>.SuccessAsync(response);
        }
        catch (Exception e)
        {
            return await Result<UserProfileInfoResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<UserSubscriptionResponse>> GetUserSubscription(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            var userCurrSubscription = await unitOfWork.GetRepository<UserCurrentSubscription>()
                .Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (userCurrSubscription == null)
            {
                userCurrSubscription = new UserCurrentSubscription()
                {
                    Id = Guid.NewGuid().ToString(),
                    ExpiresOn = DateTime.MaxValue,
                    SubscriptionBasis = SubscriptionBasis.None,
                    SubscriptionDate = DateTime.Now,
                    SubscriptionPlan = SubscriptionPlan.Free,
                    UserId = userId
                };
                await unitOfWork.GetRepository<UserCurrentSubscription>().AddAsync(userCurrSubscription);
                await unitOfWork.Commit();
            }
            var response = new UserSubscriptionResponse
            {
                ExpiresOn = userCurrSubscription.ExpiresOn,
                SubscriptionBasis = userCurrSubscription.SubscriptionBasis,
                SubscriptionDate = userCurrSubscription.SubscriptionDate,
                SubscriptionPlan = userCurrSubscription.SubscriptionPlan
            };
            return await Result<UserSubscriptionResponse>.SuccessAsync(response);
        }
        catch (Exception e)
        {
            return await Result<UserSubscriptionResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult<AuthorPublicInfoResponse>> GetAuthorPublicDetails(string authorId)
    {
        try
        {
            if (string.IsNullOrEmpty(authorId))
                throw new Exception("Author not found!");
            var user = await userManager.FindByIdAsync(authorId) ?? throw new Exception("Something went wrong");
            var userProfileInfo = await unitOfWork.GetRepository<UserProfileInfo>().Entities
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (userProfileInfo == null)
            {
                userProfileInfo = new UserProfileInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    DateOfBirth = DateTime.MinValue,
                    UserId = user.Id
                };
                await unitOfWork.GetRepository<UserProfileInfo>().AddAsync(userProfileInfo);
                await unitOfWork.Commit();
            }
            var response = new AuthorPublicInfoResponse()
            {
                Id = userProfileInfo.UserId,
                AboutMe = userProfileInfo.AboutMe,
                Country = userProfileInfo.Country,
                FullName = userProfileInfo.FullName,
                Name = user.UserName,
                Status = userProfileInfo.Status
            };
            return await Result<AuthorPublicInfoResponse>.SuccessAsync(response);
        }
        catch (Exception e)
        {
            return await Result<AuthorPublicInfoResponse>.FailAsync(e.Message);
        }
    }

    public async Task<IResult> UpdateUserProfileInfo(UpdateUserProfileInfoRequest request)
    {
        try
        {
            if (string.IsNullOrEmpty(request.Email))
                throw new Exception("Email is empty!");
            var user = await userManager.FindByEmailAsync(request.Email) ?? throw new Exception("Something went wrong");
            user.UserName = request.Name;
            user.PhoneNumber = request.Mobile;
            var userProfileInfo = await unitOfWork.GetRepository<UserProfileInfo>().Entities
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (userProfileInfo == null)
            {
                userProfileInfo = new UserProfileInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    AboutMe = request.AboutMe,
                    City = request.City,
                    Country = request.Country,
                    DateOfBirth = request.DateOfBirth.GetValueOrDefault(),
                    FullName = request.FullName,
                    Status = request.Status,
                    UserId = user.Id
                };
                await unitOfWork.GetRepository<UserProfileInfo>().AddAsync(userProfileInfo);
            }
            else
            {
                userProfileInfo.AboutMe = request.AboutMe;
                userProfileInfo.City = request.City;
                userProfileInfo.Country = request.Country;
                userProfileInfo.DateOfBirth = request.DateOfBirth.GetValueOrDefault();
                userProfileInfo.FullName = request.FullName;
                userProfileInfo.Status = request.Status;
                await unitOfWork.GetRepository<UserProfileInfo>().UpdateAsync(userProfileInfo, userProfileInfo.Id);
            }
            await unitOfWork.Commit();
            return await Result.SuccessAsync();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }

    public async Task<IResult> DeleteInfiniteAccount(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            //UserProfileInfo
            var profileInfo = await unitOfWork.GetRepository<UserProfileInfo>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if(profileInfo != null)
                await unitOfWork.GetRepository<UserProfileInfo>().DeleteAsync(profileInfo);
            
            //UserPortfolio
            var portFolioMd = await unitOfWork.GetRepository<UserPortfolio>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if(portFolioMd != null)
                await unitOfWork.GetRepository<UserPortfolio>().DeleteAsync(portFolioMd);

            //IdentityUserLogin
            var userLogin = await unitOfWork.AppDbContext.UserLogins
                .FirstAsync(x => x.UserId == userId);
            unitOfWork.AppDbContext.UserLogins.Remove(userLogin);
            
            //AppUser
            var user = await userManager.FindByIdAsync(userId);
            await userManager.DeleteAsync(user!);
            await unitOfWork.Commit();

            return await Result.SuccessAsync();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }
}