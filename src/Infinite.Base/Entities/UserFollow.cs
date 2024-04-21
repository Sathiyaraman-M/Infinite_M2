namespace Infinite.Base.Entities;

public class UserFollow : IEntity<string>
{
    public string Id { get; set; }
    
    public AppUser Followed { get; set; }
    public string FollowedId { get; set; }
    public UserProfileInfo FollowedProfileInfo { get; set; }
    public string FollowedProfileInfoId { get; set; }
    
    public AppUser Follower { get; set; }
    public string FollowerId { get; set; }
    public UserProfileInfo FollowerProfileInfo { get; set; }
    public string FollowerProfileInfoId { get; set; }
}