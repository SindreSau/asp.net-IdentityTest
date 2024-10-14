using System.ComponentModel.DataAnnotations;

namespace IdentityTest.Models;

public class Joke
{
    public int Id { get; set; }
    [MaxLength(256)]
    public string JokeText { get; set; } = string.Empty;
    [MaxLength(256)]
    public string Author { get; set; } = string.Empty;

    public virtual ApplicationUser? ApplicationUser { get; set; }
}