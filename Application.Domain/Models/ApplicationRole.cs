using AspNetCore.Identity.MongoDbCore.Models;

namespace Application.Domain.Models;

public class ApplicationRole : MongoIdentityRole<Guid>
{
    public ApplicationRole() : base() { }
    public ApplicationRole(string roleName) : base(roleName) { }
}
