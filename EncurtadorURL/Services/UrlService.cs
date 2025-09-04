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
    private readonly IHttpClientFactory _httpClientFactory;

    public UrlService(IUrlRepository urlRepository, IHttpClientFactory httpClientFactory) 
    {
        _urlRepository = urlRepository;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Result<URLModel>> EncurtarUrl(RequestEncurtarDto urlDto, string baseUrl)
    {
        urlDto = new RequestEncurtarDto(Url: HttpUtility.UrlDecode(urlDto.Url));

        if (!urlDto.Url.StartsWith("http://") && !urlDto.Url.StartsWith("https://"))
            urlDto = new RequestEncurtarDto(Url: $"https://{urlDto.Url}");

        if (!Uri.TryCreate(urlDto.Url, UriKind.Absolute, out var uri))
            return Result<URLModel>.Failure("A URL fornecida não é válida.");

        if (!await CheckUrlAvailability(urlDto.Url))
            return Result<URLModel>.Failure("Não foi possível conectar a URL.");
        
        var urlExists = await _urlRepository.GetUrlByUrl(urlDto.Url);
        
        if (urlExists is not null)
            return Result<URLModel>.Failure($"Url Já foi ecncurtada. {urlExists.URLEncurtada}");

        var guid = Guid.NewGuid();
        var chave = guid.ToString().Substring(0, 8);

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

        public async Task<bool> CheckUrlAvailability(string url)
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(5);
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
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