using Microsoft.AspNetCore.Identity;

namespace Project.Data;

public class User: IdentityUser
{
    public string? FavAnimal {  get; set; }
}
