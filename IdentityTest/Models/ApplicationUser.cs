using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace IdentityTest.Models;

public class ApplicationUser : IdentityUser
{
    [MaxLength(256)]
    public string FavouriteProgrammingLanguage { get; set; } = "C#";
}