using EncurtadorURL.Models;

namespace EncurtadorURL.Repositories;

public interface IUrlRepository
{
    Task<URLModel?> GetUrlByUrl(string url);
    Task AddUrl(URLModel url);
    Task<URLModel?> GetUrlByChave(string chave);
}