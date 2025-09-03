using EncurtadorURL.Models;
using EncurtadorURL.DTO;
using EncurtadorURL.Models.Response;

namespace EncurtadorURL.Services;

public interface IUrlService
{
    Task<Result<URLModel>> EncurtarUrl(RequestEncurtarDto urlDto);
    Task<bool> PingHost(string host);
    Task<Result<URLModel>> RetornarURLEncurtada(string url);
}