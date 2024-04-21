namespace Infinite.Base.Responses;

public record FollowStatResponse(string UserId, string UserName, string FullName, int Followers, int Followed);