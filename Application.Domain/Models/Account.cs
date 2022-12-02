using AspNetCore.Identity.MongoDbCore.Models;
using MongoDbGenericRepository.Attributes;

namespace Application.Domain.Models;

[CollectionName("Account")]
public class Account : MongoIdentityUser<Guid>
{
    public string Avatar { get; set; }
    public bool IsAdmin { get; set; }
}
