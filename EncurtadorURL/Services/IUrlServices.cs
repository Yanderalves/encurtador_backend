using EncurtadorURL.Models;
using EncurtadorURL.DTO;

namespace EncurtadorURL.Services;

public interface IUrlService
{
    Task<URLModel> EncurtarUrl(RequestEncurtarDto urlDto);
    Task<bool> PingHost(string host);
}