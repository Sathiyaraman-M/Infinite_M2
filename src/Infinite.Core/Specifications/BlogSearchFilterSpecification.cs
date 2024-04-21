using Infinite.Core.Specifications.Base;

namespace Infinite.Core.Specifications;

public class BlogSearchFilterSpecification : BaseSpecification<Blog>
{
    public BlogSearchFilterSpecification(string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            FilterCondition = p => true;
        }
        else
        {
            searchString = searchString.ToLower();
            FilterCondition = p => p.Title.ToLower().Contains(searchString);
        }
    }
}