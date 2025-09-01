using EncurtadorURL.Context;
using EncurtadorURL.Models;
using Microsoft.EntityFrameworkCore;

namespace EncurtadorURL.Repositories;

public class UrlRepository : IUrlRepository
{
    private readonly DatabaseContext _context;

    public UrlRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<URLModel?> GetUrlByUrl(string url)
    {
        return await _context.URLs.FirstOrDefaultAsync(x => x.URLOriginal == url);
    }

    public async Task AddUrl(URLModel url)
    {
        await _context.URLs.AddAsync(url);
        await _context.SaveChangesAsync();
    }

    public async Task<URLModel?> GetUrlByChave(string chave)
    {
        return await _context.URLs.FirstOrDefaultAsync(x => x.ChaveEncurtada == chave);
    }
}