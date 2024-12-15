using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class AuthorService(ApplicationDbContext context) : IAuthorService
{
    public async Task<Author> GetAuthor()
    {
        return await context.Authors
                .Include(a => a.Books)
                .Where(a => a.Books.Any())
                .OrderByDescending(a => a.Books
                    .OrderByDescending(b => b.Title.Length)
                    .Max(b => b.Title.Length))
                .ThenBy(a => a.Id)
                .Select(a => new Author()
                {
                    Books = null,
                    Id = a.Id,
                    Name = a.Name,
                    Surname = a.Surname
                })
                .FirstOrDefaultAsync();
    }

    public async Task<List<Author>> GetAuthors()
    {
        return await context.Authors
                .Include(a => a.Books)
                .Where(a => a.Books.Any(b => b.PublishDate.Year > 2015) && a.Books
                    .Count(b => b.PublishDate.Year > 2015) % 2 == 0)
                .Select(a => new Author()
                {
                    Books = null,
                    Id = a.Id,
                    Name = a.Name,
                    Surname = a.Surname
                })
                .ToListAsync();
    }
}
