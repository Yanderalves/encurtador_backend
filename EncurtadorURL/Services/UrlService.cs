using System.Net.NetworkInformation;
using EncurtadorURL.DTO;
using EncurtadorURL.Models;
using EncurtadorURL.Repositories;

namespace EncurtadorURL.Services;

public class UrlService : IUrlService
{
    private readonly IUrlRepository _urlRepository;
    private readonly IHttpContextAccessor _httpContextAccessor; 

    public UrlService(IUrlRepository urlRepository, IHttpContextAccessor httpContextAccessor) 
    {
        _urlRepository = urlRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<URLModel> EncurtarUrl(RequestEncurtarDto urlDto)
    {
        var urlExists = await _urlRepository.GetUrlByUrl(urlDto.Url); 
        if (urlExists is not null)
        {
            throw new InvalidOperationException($"A URL já foi encurtada, {urlExists.URLEncurtada}");
        }

        var guid = Guid.NewGuid();
        var chave = guid.ToString().Substring(0, 8);

        var request = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{request.Scheme}://{request.Host}";

        var newUrl = new URLModel
        {
            Id = guid,
            ChaveEncurtada = chave,
            URLEncurtada = $"{baseUrl}/{chave}",
            URLOriginal = urlDto.Url
        };

        await _urlRepository.AddUrl(newUrl);
        return newUrl;
    }

    public async Task<bool> PingHost(string host)
    {
        using var ping = new Ping();
        try
        {
            var reply = await ping.SendPingAsync(host, 1000); // Timeout de 1000ms
            return reply.Status == IPStatus.Success;
        }
        catch (PingException)
        {
            return false;
        }
    }
}