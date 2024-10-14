using Microsoft.AspNetCore.Identity;

namespace IdentityTest.Models;

public class ApplicationUser : IdentityUser
{
    public string FavouriteProgrammingLanguage { get; set; } = "C#";
}