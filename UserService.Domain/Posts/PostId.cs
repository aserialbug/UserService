using MassTransit;
using UserService.Domain.Common;

namespace UserService.Domain.Posts;

public class PostId : SimpleId
{
    public PostId(NewId id) : base(id)
    {
    }
    
    public static PostId New()
    {
        return new(NewId.Next());
    }
    
    public static PostId Parse(string value)
    {
        var idTmp = value.Split('_');

        if (idTmp.Length != 2 ||
            !idTmp[0].Equals(nameof(PostId)))
            throw new ArgumentException($"Value {value} is invalid {nameof(PostId)}");
        
        var id = new NewId(idTmp[1]);
        
        return new PostId(id);
    }
}