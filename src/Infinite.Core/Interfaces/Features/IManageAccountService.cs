namespace Infinite.Core.Interfaces.Features;

public interface IManageAccountService
{
    Task<IResult<string>> GetPortFolioMd(string userId);

    Task<IResult> SavePortFolio(string userId, string markdown);

    Task<IResult<UserProfileInfoResponse>> GetUserProfileInfo(string userId);
    
    Task<IResult<UserSubscriptionResponse>> GetUserSubscription(string userId);
    
    Task<IResult<AuthorPublicInfoResponse>> GetAuthorPublicDetails(string authorId);

    Task<IResult> UpdateUserProfileInfo(UpdateUserProfileInfoRequest request);

    Task<IResult> DeleteInfiniteAccount(string userId);
}