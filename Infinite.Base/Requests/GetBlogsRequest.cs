namespace Infinite.Base.Requests;

public class GetBlogsRequest
{
    public string SearchString { get; set; }
    
    public IEnumerable<string> AuthorIds { get; set; } 
}