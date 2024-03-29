﻿using Microsoft.AspNetCore.Identity;

namespace Infinite.Core.Features;

public class ManageAccountService : IManageAccountService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<AppUser> _userManager;

    public ManageAccountService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    public async Task<IResult<string>> GetPortFolioMd(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User not found!");
            var portFolioMd = await _unitOfWork.GetRepository<UserPortfolio>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if (portFolioMd != null) 
                return await Result<string>.SuccessAsync(data: portFolioMd.PortfolioMarkdown);
            portFolioMd = new UserPortfolio()
            {
                Id = Guid.NewGuid().ToString(),
                PortfolioMarkdown = string.Empty,
                UserId = userId
            };
            await _unitOfWork.GetRepository<UserPortfolio>().AddAsync(portFolioMd);
            await _unitOfWork.Commit();
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
            var portFolioMd = await _unitOfWork.GetRepository<UserPortfolio>().Entities
                .FirstAsync(x => x.UserId == userId);
            portFolioMd.PortfolioMarkdown = markdown;
            await _unitOfWork.GetRepository<UserPortfolio>().UpdateAsync(portFolioMd, portFolioMd.Id);
            await _unitOfWork.Commit();
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
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new Exception("Something went wrong");
            var userProfileInfo = await _unitOfWork.GetRepository<UserProfileInfo>().Entities
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (userProfileInfo == null)
            {
                userProfileInfo = new UserProfileInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    DateOfBirth = DateTime.MinValue,
                    UserId = user.Id
                };
                await _unitOfWork.GetRepository<UserProfileInfo>().AddAsync(userProfileInfo);
                await _unitOfWork.Commit();
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
            var userCurrSubscription = await _unitOfWork.GetRepository<UserCurrentSubscription>()
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
                await _unitOfWork.GetRepository<UserCurrentSubscription>().AddAsync(userCurrSubscription);
                await _unitOfWork.Commit();
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
            var user = await _userManager.FindByIdAsync(authorId);
            if (user == null)
                throw new Exception("Something went wrong");
            var userProfileInfo = await _unitOfWork.GetRepository<UserProfileInfo>().Entities
                .FirstOrDefaultAsync(x => x.UserId == user.Id);
            if (userProfileInfo == null)
            {
                userProfileInfo = new UserProfileInfo
                {
                    Id = Guid.NewGuid().ToString(),
                    DateOfBirth = DateTime.MinValue,
                    UserId = user.Id
                };
                await _unitOfWork.GetRepository<UserProfileInfo>().AddAsync(userProfileInfo);
                await _unitOfWork.Commit();
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
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                throw new Exception("Something went wrong");
            user.UserName = request.Name;
            user.PhoneNumber = request.Mobile;
            var userProfileInfo = await _unitOfWork.GetRepository<UserProfileInfo>().Entities
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
                await _unitOfWork.GetRepository<UserProfileInfo>().AddAsync(userProfileInfo);
            }
            else
            {
                userProfileInfo.AboutMe = request.AboutMe;
                userProfileInfo.City = request.City;
                userProfileInfo.Country = request.Country;
                userProfileInfo.DateOfBirth = request.DateOfBirth.GetValueOrDefault();
                userProfileInfo.FullName = request.FullName;
                userProfileInfo.Status = request.Status;
                await _unitOfWork.GetRepository<UserProfileInfo>().UpdateAsync(userProfileInfo, userProfileInfo.Id);
            }
            await _unitOfWork.Commit();
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
            var profileInfo = await _unitOfWork.GetRepository<UserProfileInfo>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if(profileInfo != null)
                await _unitOfWork.GetRepository<UserProfileInfo>().DeleteAsync(profileInfo);
            
            //UserPortfolio
            var portFolioMd = await _unitOfWork.GetRepository<UserPortfolio>().Entities
                .FirstOrDefaultAsync(x => x.UserId == userId);
            if(portFolioMd != null)
                await _unitOfWork.GetRepository<UserPortfolio>().DeleteAsync(portFolioMd);

            //IdentityUserLogin
            var userLogin = await _unitOfWork.AppDbContext.UserLogins
                .FirstAsync(x => x.UserId == userId);
            _unitOfWork.AppDbContext.UserLogins.Remove(userLogin);
            
            //AppUser
            var user = await _userManager.FindByIdAsync(userId);
            await _userManager.DeleteAsync(user!);
            await _unitOfWork.Commit();

            return await Result.SuccessAsync();
        }
        catch (Exception e)
        {
            return await Result.FailAsync(e.Message);
        }
    }
}