using EncurtadorURL.DTO;
using EncurtadorURL.Services;
using Microsoft.AspNetCore.Mvc;

namespace EncurtadorURL.Routes;

public static class Routes
{
    public static void UseRoutes(this IEndpointRouteBuilder routes)
    {
        
        routes.MapPost("/url", async ([FromServices] IUrlService urlService, [FromBody] RequestEncurtarDto urlDto) =>
        {
            var result = await urlService.EncurtarUrl(urlDto);
            
            if(!result.IsSuccess)
                return Results.BadRequest(result);
            return Results.Ok(result);
                
        }).WithDescription("Recebe uma URL e retorna ela encurtada");

        routes.MapGet("{codigoEncurtamento}",async ([FromServices] IUrlService urlService, string codigoEncurtamento) =>
        {
            var result = await urlService.RetornarURLEncurtada(codigoEncurtamento);
            
            if(!result.IsSuccess)
                return Results.NotFound(result);
            return Results.Redirect(result.Value.URLOriginal);
            
        }).WithDescription("Recebe o código encurtado e faz o redirecionamento para a URL original");
    }
}