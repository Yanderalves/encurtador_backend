using System.Web;
using EncurtadorURL.DTO;
using EncurtadorURL.Services;
using EncurtadorURL.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EncurtadorURL.Routes;

public static class Routes
{
    public static void UseRoutes(this IEndpointRouteBuilder routes)
    {
        routes.MapPost("", async (
            [FromServices] IUrlService urlService, 
            [FromServices] IUrlRepository urlRepository, 
            [FromBody] RequestEncurtarDto urlDto) =>
        {
            urlDto = new RequestEncurtarDto(Url: HttpUtility.UrlDecode(urlDto.Url));

            if (!Uri.TryCreate(urlDto.Url, UriKind.Absolute, out var uri))
                return Results.BadRequest(new { Result = "A URL fornecida não é válida." });

            if (!await urlService.PingHost(uri.Host))
                return Results.BadRequest(new { Result = "Não foi possível conectar a URL." });
            
            try
            {
                var newUrl = await urlService.EncurtarUrl(urlDto);
                return Results.Ok(new { Results = newUrl });
            }
            catch (InvalidOperationException ex)
            {
                return Results.Conflict(new { Result = ex.Message });
            }
        }).WithDescription("Recebe uma URL e retorna ela encurtada");

        routes.MapGet("{url}",async (
            [FromServices] IUrlRepository urlRepository, 
            string url) =>
        {
            url = HttpUtility.UrlDecode(url);
    
            var _url = await urlRepository.GetUrlByChave(url);
            
            return _url is null ? Results.NotFound() : Results.Redirect(_url.URLOriginal);
        }).WithDescription("Recebe o código encurtado e faz o redirecionamento para a URL original");
    }
}