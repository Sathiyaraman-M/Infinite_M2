using Infinite.Core.Specifications.Base;

namespace Infinite.Core.Specifications;

public class AuthorSearchFilterSpecification : BaseSpecification<AppUser>
{
    public AuthorSearchFilterSpecification(string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            FilterCondition = x => true;
        }
        else
        {
            searchString = searchString.ToLower();
            FilterCondition = x => x.UserName.ToLower().StartsWith(searchString);
        }
    }
}