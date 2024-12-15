using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Models;
using TestTask.Services.Interfaces;

namespace TestTask.Services.Implementations;

public class BookService(ApplicationDbContext context) : IBookService
{
    public async Task<Book> GetBook()
    {
        return await context.Books
                    .OrderByDescending(b => b.QuantityPublished * b.Price)
                    .FirstOrDefaultAsync();
    }

    public async Task<List<Book>> GetBooks()
    {
        return await context.Books
                    .Where(b => EF.Functions.Like(b.Title,"%Red%")
                        && b.PublishDate > new DateTime(2012,5,25))
                    .ToListAsync();
    }
}
