using IdentityTest.Data;
using IdentityTest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityTest.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JokesController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public JokesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Jokes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Joke>>> GetJokes()
    {
        return await _context.Jokes.ToListAsync();
    }

    // GET: api/Jokes/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Joke>> GetJoke(int id)
    {
        var joke = await _context.Jokes.FindAsync(id);

        if (joke == null)
        {
            return NotFound();
        }

        return joke;
    }

    // POST: api/Jokes
    [HttpPost]
    [Authorize(Policy = "WriterPolicy")]
    public async Task<ActionResult<Joke>> PostJoke(Joke joke)
    {
        if (User.Identity != null)
            if (User.Identity.Name != null)
                joke.Author = User.Identity.Name;
        _context.Jokes.Add(joke);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetJoke", new { id = joke.Id }, joke);
    }

    // PUT: api/Jokes/5
    [HttpPut("{id}")]
    [Authorize(Policy = "WriterPolicy")]
    public async Task<IActionResult> PutJoke(int id, Joke joke)
    {
        if (id != joke.Id)
        {
            return BadRequest();
        }

        var existingJoke = await _context.Jokes.FindAsync(id);
        if (existingJoke == null)
        {
            return NotFound();
        }

        if (User.Identity != null && existingJoke.Author != User.Identity.Name)
        {
            return Forbid();
        }

        existingJoke.JokeText = joke.JokeText;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!JokeExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Jokes/5
    [HttpDelete("{id}")]
    [Authorize(Policy = "WriterPolicy")]
    public async Task<IActionResult> DeleteJoke(int id)
    {
        var joke = await _context.Jokes.FindAsync(id);
        if (joke == null)
        {
            return NotFound();
        }

        if (User.Identity != null && joke.Author != User.Identity.Name)
        {
            return Forbid();
        }

        _context.Jokes.Remove(joke);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool JokeExists(int id)
    {
        return _context.Jokes.Any(e => e.Id == id);
    }
}