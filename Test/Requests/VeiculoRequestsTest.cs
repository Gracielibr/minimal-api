using System.Net;
using System.Text;
using System.Text.Json;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class VeiculoRequestsTest
{
    private static string? _tokenAdm;
    private static string? _tokenEditor;

    [ClassInitialize]
    public static async Task ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
        _tokenAdm = await ObterToken("adm@teste.com", "123456");
        _tokenEditor = await ObterToken("editor@teste.com", "123456");
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    private static async Task<string> ObterToken(string email, string senha)
    {
        var loginDto = new LoginDTO
        {
            Email = email,
            Senha = senha
        };
        var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");
        var response = await Setup.client.PostAsync("/administradores/login", content);
        
        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        return admLogado?.Token ?? "";
    }

    private HttpRequestMessage CriarRequestComToken(HttpMethod method, string url, string? token)
{
    var request = new HttpRequestMessage(method, url);
    
    if (!string.IsNullOrEmpty(token))
    {
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
    
    return request;
}

    // TESTES QUE FUNCIONAM PARA AMBOS OS PERFIS (Adm e Editor)
    [TestMethod]
    public async Task TestarListarVeiculosComPerfilAdm()
    {
        // Arrange
        var request = CriarRequestComToken(HttpMethod.Get, "/veiculos", _tokenAdm);

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarListarVeiculosComPerfilEditor()
    {
        // Arrange
        var request = CriarRequestComToken(HttpMethod.Get, "/veiculos", _tokenEditor);

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarBuscarVeiculoPorIdComPerfilEditor()
    {
        // Arrange
        var request = CriarRequestComToken(HttpMethod.Get, "/veiculos/1", _tokenEditor);

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarCriarVeiculoComPerfilEditor()
    {
        // Arrange
        var novoVeiculo = new VeiculoDTO
        {
            Nome = "Gol",
            Marca = "Volkswagen",
            Ano = 2020
        };
        var content = new StringContent(JsonSerializer.Serialize(novoVeiculo), Encoding.UTF8, "application/json");
        
        var request = CriarRequestComToken(HttpMethod.Post, "/veiculos", _tokenEditor);
        request.Content = content;

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
    }

    // TESTES QUE SÓ FUNCIONAM PARA Adm
    [TestMethod]
    public async Task TestarAtualizarVeiculoComPerfilAdm()
    {
        // Arrange - Primeiro cria um veículo
        var veiculoParaCriar = new VeiculoDTO
        {
            Nome = "Veiculo Teste",
            Marca = "Marca Teste", 
            Ano = 2020
        };
        var contentCriar = new StringContent(JsonSerializer.Serialize(veiculoParaCriar), Encoding.UTF8, "application/json");
        var requestCriar = CriarRequestComToken(HttpMethod.Post, "/veiculos", _tokenAdm);
        requestCriar.Content = contentCriar;
        var responseCriar = await Setup.client.SendAsync(requestCriar);
        var veiculoCriado = JsonSerializer.Deserialize<Veiculo>(await responseCriar.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Arrange - Atualizar o veículo
        var veiculoAtualizado = new VeiculoDTO
        {
            Nome = "Veiculo Atualizado",
            Marca = "Marca Atualizada",
            Ano = 2023
        };
        var contentAtualizar = new StringContent(JsonSerializer.Serialize(veiculoAtualizado), Encoding.UTF8, "application/json");
        
        var requestAtualizar = CriarRequestComToken(HttpMethod.Put, $"/veiculos/{veiculoCriado?.Id}", _tokenAdm);
        requestAtualizar.Content = contentAtualizar;

        // Act
        var response = await Setup.client.SendAsync(requestAtualizar);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarAtualizarVeiculoComPerfilEditor_DeveFalhar()
    {
        // Arrange
        var veiculoAtualizado = new VeiculoDTO
        {
            Nome = "Veiculo Atualizado",
            Marca = "Marca Atualizada",
            Ano = 2023
        };
        var content = new StringContent(JsonSerializer.Serialize(veiculoAtualizado), Encoding.UTF8, "application/json");
        
        var request = CriarRequestComToken(HttpMethod.Put, "/veiculos/1", _tokenEditor);
        request.Content = content;

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert - Editor NÃO pode atualizar veículos
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarDeletarVeiculoComPerfilAdm()
    {
        // Arrange - Primeiro cria um veículo
        var veiculoParaCriar = new VeiculoDTO
        {
            Nome = "Veiculo Para Deletar",
            Marca = "Marca Teste",
            Ano = 2020
        };
        var contentCriar = new StringContent(JsonSerializer.Serialize(veiculoParaCriar), Encoding.UTF8, "application/json");
        var requestCriar = CriarRequestComToken(HttpMethod.Post, "/veiculos", _tokenAdm);
        requestCriar.Content = contentCriar;
        var responseCriar = await Setup.client.SendAsync(requestCriar);
        var veiculoCriado = JsonSerializer.Deserialize<Veiculo>(await responseCriar.Content.ReadAsStringAsync(), new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        // Act - Deletar o veículo
        var requestDeletar = CriarRequestComToken(HttpMethod.Delete, $"/veiculos/{veiculoCriado?.Id}", _tokenAdm);
        var response = await Setup.client.SendAsync(requestDeletar);

        // Assert
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarDeletarVeiculoComPerfilEditor_DeveFalhar()
    {
        // Arrange & Act
        var request = CriarRequestComToken(HttpMethod.Delete, "/veiculos/1", _tokenEditor);
        var response = await Setup.client.SendAsync(request);

        // Assert - Editor NÃO pode deletar veículos
        Assert.AreEqual(HttpStatusCode.Forbidden, response.StatusCode);
    }

    // TESTES DE VALIDAÇÃO
    [TestMethod]
    public async Task TestarCriarVeiculoComDadosInvalidos()
    {
        // Arrange
        var veiculoInvalido = new VeiculoDTO
        {
            Nome = "",
            Marca = "",
            Ano = 1800
        };
        var content = new StringContent(JsonSerializer.Serialize(veiculoInvalido), Encoding.UTF8, "application/json");
        
        var request = CriarRequestComToken(HttpMethod.Post, "/veiculos", _tokenAdm);
        request.Content = content;

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarAcessoSemToken()
    {
        // Act
        var response = await Setup.client.GetAsync("/veiculos");

        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
public async Task TestarListarVeiculosComFiltroNome()
{
    var request = CriarRequestComToken(HttpMethod.Get, "/veiculos?nome=Gol", _tokenAdm!);
    var response = await Setup.client.SendAsync(request);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
}

[TestMethod]
public async Task TestarListarVeiculosComFiltroMarca()
{
    var request = CriarRequestComToken(HttpMethod.Get, "/veiculos?marca=Volkswagen", _tokenAdm!);
    var response = await Setup.client.SendAsync(request);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
}

[TestMethod]
public async Task TestarListarVeiculosComPaginacao()
{
    var request = CriarRequestComToken(HttpMethod.Get, "/veiculos?pagina=1", _tokenAdm!);
    var response = await Setup.client.SendAsync(request);
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
}

[TestMethod]
public async Task TestarBuscarVeiculoPorIdInexistente()
{
    var request = CriarRequestComToken(HttpMethod.Get, "/veiculos/9999", _tokenAdm!);
    var response = await Setup.client.SendAsync(request);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
}

[TestMethod]
public async Task TestarAtualizarVeiculoInexistente()
{
    var veiculoAtualizado = new VeiculoDTO
    {
        Nome = "Teste",
        Marca = "Teste", 
        Ano = 2020
    };
    var content = new StringContent(JsonSerializer.Serialize(veiculoAtualizado), Encoding.UTF8, "application/json");
    
    var request = CriarRequestComToken(HttpMethod.Put, "/veiculos/9999", _tokenAdm!);
    request.Content = content;
    
    var response = await Setup.client.SendAsync(request);
    Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
}
}