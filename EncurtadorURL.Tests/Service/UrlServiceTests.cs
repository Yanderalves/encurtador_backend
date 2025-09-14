using EncurtadorURL.DTO;
using EncurtadorURL.Models;
using EncurtadorURL.Repositories;
using EncurtadorURL.Services;
using Moq;
using Xunit;
using Moq.Protected;
using System.Net;

namespace EncurtadorURL.Test;

public class UrlServiceTests
{
    private readonly Mock<IUrlRepository> _mockUrlRepository;
    private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
    private readonly UrlService _urlService;

    public UrlServiceTests()
    {
        // Arrange (Organizar): Inicializar os mocks
        _mockUrlRepository = new Mock<IUrlRepository>();
        _mockHttpClientFactory = new Mock<IHttpClientFactory>();

        // Configurar o mock do IHttpClientFactory para simular uma requisição bem-sucedida
        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK
            });

        _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(new HttpClient(mockHttpMessageHandler.Object));

        // Act (Agir): Criar a instância da classe a ser testada, injetando os mocks
        _urlService = new UrlService(
            _mockUrlRepository.Object,
            _mockHttpClientFactory.Object
        );
    }

    [Fact]
    public async Task EncurtarUrl_ComUrlValidaENova_DeveRetornarSucesso()
    {
        // Arrange (Organizar)
        // Simular que a URL não existe no banco de dados
        _mockUrlRepository.Setup(r => r.GetUrlByUrl(It.IsAny<string>()))
            .ReturnsAsync((URLModel)null);

        var urlDto = new RequestEncurtarDto(Url: "https://www.google.com");
        var baseUrl = "http://localhost:5026";

        // Act (Agir)
        var result = await _urlService.EncurtarUrl(urlDto, baseUrl);

        // Assert (Afirmar)
        // Verificar se o resultado é de sucesso
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.False(string.IsNullOrEmpty(result.Value.URLEncurtada));
        Assert.StartsWith(baseUrl, result.Value.URLEncurtada);

        // Verificar se o método AddUrl do repositório foi chamado
        _mockUrlRepository.Verify(r => r.AddUrl(It.IsAny<URLModel>()), Times.Once);
    }

    [Fact]
    public async Task EncurtarUrl_ComUrlInvalida_DeveRetornarFalha()
    {
        // Arrange (Organizar)
        var urlDto = new RequestEncurtarDto(Url: "url_invalida_sem_formato");
        var baseUrl = "http://localhost:5026";

        // Act (Agir)
        var result = await _urlService.EncurtarUrl(urlDto, baseUrl);

        // Assert (Afirmar)
        // Verificar se o resultado é de falha
        Assert.False(result.IsSuccess);
        Assert.Equal("A URL fornecida não é válida.", result.ErrorMessage);
        
        // Verificar que o repositório não foi acessado para adicionar a URL
        _mockUrlRepository.Verify(r => r.AddUrl(It.IsAny<URLModel>()), Times.Never);
    }

    [Fact]
    public async Task EncurtarUrl_ComUrlJaExistente_DeveRetornarFalha()
    {
        // Arrange (Organizar)
        // Simular que a URL já existe no banco de dados
        var existingUrl = new URLModel { URLOriginal = "https://www.google.com", URLEncurtada = "http://localhost:5026/abc12345" };
        _mockUrlRepository.Setup(r => r.GetUrlByUrl(It.IsAny<string>()))
            .ReturnsAsync(existingUrl);

        var urlDto = new RequestEncurtarDto(Url: "https://www.google.com");
        var baseUrl = "http://localhost:5026";

        // Act (Agir)
        var result = await _urlService.EncurtarUrl(urlDto, baseUrl);

        // Assert (Afirmar)
        // Verificar se o resultado é de falha e a mensagem de erro está correta
        Assert.False(result.IsSuccess);
        Assert.Equal($"Url Já foi encurtada. {existingUrl.URLEncurtada}", result.ErrorMessage);
        
        // Verificar que o repositório não foi acessado para adicionar a URL
        _mockUrlRepository.Verify(r => r.AddUrl(It.IsAny<URLModel>()), Times.Never);
    }
}