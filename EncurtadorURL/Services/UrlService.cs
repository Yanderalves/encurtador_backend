using System.Net.NetworkInformation;
using System.Web;
using EncurtadorURL.DTO;
using EncurtadorURL.Models;
using EncurtadorURL.Models.Response;
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

    public async Task<Result<URLModel>> EncurtarUrl(RequestEncurtarDto urlDto)
    {
        urlDto = new RequestEncurtarDto(Url: HttpUtility.UrlDecode(urlDto.Url));

        
        if (!urlDto.Url.StartsWith("http://") && !urlDto.Url.StartsWith("https://"))
            urlDto = new RequestEncurtarDto(Url: $"https://{urlDto.Url}");

        if (!Uri.TryCreate(urlDto.Url, UriKind.Absolute, out var uri))
            return Result<URLModel>.Failure("A URL fornecida não é válida.");

        if (!await PingHost(uri.Host))
            return Result<URLModel>.Failure("Não foi possível conectar a URL.");
        
        var urlExists = await _urlRepository.GetUrlByUrl(urlDto.Url);
        
        if (urlExists is not null)
            return Result<URLModel>.Failure($"Url Já foi ecncurtada. {urlExists.URLEncurtada}");

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
        return Result<URLModel>.Success(newUrl);
    }

    public async Task<bool> PingHost(string host)
    {
        using var ping = new Ping();
        try
        {
            var reply = await ping.SendPingAsync(host, 1000); 
            return reply.Status == IPStatus.Success;
        }
        catch (PingException)
        {
            return false;
        }
    }

    public async Task<Result<URLModel>> RetornarURLEncurtada(string codigoEncurtamento)
    {
        codigoEncurtamento = HttpUtility.UrlDecode(codigoEncurtamento);
    
        var urlEncurtada = await _urlRepository.GetUrlByChave(codigoEncurtamento);
        
        return urlEncurtada is null ? Result<URLModel>.Failure("URL não encontrada") : Result<URLModel>.Success(urlEncurtada);
    }
}