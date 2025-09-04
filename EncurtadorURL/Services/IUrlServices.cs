using EncurtadorURL.Models;
using EncurtadorURL.DTO;
using EncurtadorURL.Models.Response;

namespace EncurtadorURL.Services;

public interface IUrlService
{
    Task<Result<URLModel>> EncurtarUrl(RequestEncurtarDto urlDto, string baseUrl);
    Task<Result<URLModel>> RetornarURLEncurtada(string url);
    public Task<bool> CheckUrlAvailability(string url);
}