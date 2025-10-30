using System.Net;
using System.Text;
using System.Text.Json;
using MinimalApi.Dominio.DTOs;
using MinimalApi.Dominio.Enus;
using MinimalApi.Dominio.ModelViews;
using MinimalApi.DTOs;
using Test.Helpers;

namespace Test.Requests;

[TestClass]
public class AdministradorRequestsTest
{
    private static string? _token;

    [ClassInitialize]
    public static async Task ClassInit(TestContext testContext)
    {
        Setup.ClassInit(testContext);
        _token = await ObterToken();
    }

    [ClassCleanup]
    public static void ClassCleanup()
    {
        Setup.ClassCleanup();
    }

    private static async Task<string> ObterToken()
    {
        var loginDto = new LoginDTO
        {
            Email = "adm@teste.com",
            Senha = "123456"
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

    private HttpRequestMessage CriarRequestComToken(HttpMethod method, string url)
    {
        var request = new HttpRequestMessage(method, url);
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _token);
        return request;
    }

    [TestMethod]
    public async Task TestarLoginComCredenciaisCorretas()
    {
        // Arrange
        var loginDto = new LoginDTO
        {
            Email = "adm@teste.com",
            Senha = "123456"
        };
        var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

        // Act
        var response = await Setup.client.PostAsync("/administradores/login", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var result = await response.Content.ReadAsStringAsync();
        var admLogado = JsonSerializer.Deserialize<AdministradorLogado>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.IsNotNull(admLogado);
        Assert.AreEqual("adm@teste.com", admLogado.Email);
        Assert.AreEqual("Adm", admLogado.Perfil);
        Assert.IsFalse(string.IsNullOrEmpty(admLogado.Token));
    }

    [TestMethod]
    public async Task TestarLoginComCredenciaisInvalidas()
    {
        // Arrange
        var loginDto = new LoginDTO
        {
            Email = "email@inexistente.com",
            Senha = "senha_errada"
        };
        var content = new StringContent(JsonSerializer.Serialize(loginDto), Encoding.UTF8, "application/json");

        // Act
        var response = await Setup.client.PostAsync("/administradores/login", content);

        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarListarAdministradores()
    {
        // Arrange
        var request = CriarRequestComToken(HttpMethod.Get, "/administradores");

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        var administradores = JsonSerializer.Deserialize<List<AdministradorModelView>>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.IsNotNull(administradores);
        Assert.IsTrue(administradores.Count > 0);
    }

    [TestMethod]
    public async Task TestarBuscarAdministradorPorId()
    {
        // Arrange
        var request = CriarRequestComToken(HttpMethod.Get, "/administradores/1");

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        var administrador = JsonSerializer.Deserialize<AdministradorModelView>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.IsNotNull(administrador);
        Assert.AreEqual(1, administrador.Id);
        Assert.AreEqual("adm@teste.com", administrador.Email);
    }

    [TestMethod]
    public async Task TestarBuscarAdministradorPorIdInexistente()
    {
        // Arrange
        var request = CriarRequestComToken(HttpMethod.Get, "/administradores/999");

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarCriarAdministrador()
    {
        // Arrange
        var novoAdministrador = new AdministradorDTO
        {
            Email = "novo@teste.com",
            Senha = "123456",
            Perfil = Perfil.Editor
        };
        var content = new StringContent(JsonSerializer.Serialize(novoAdministrador), Encoding.UTF8, "application/json");
        
        var request = CriarRequestComToken(HttpMethod.Post, "/administradores");
        request.Content = content;

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        var administradorCriado = JsonSerializer.Deserialize<AdministradorModelView>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
        
        Assert.IsNotNull(administradorCriado);
        Assert.AreEqual("novo@teste.com", administradorCriado.Email);
        Assert.AreEqual("Editor", administradorCriado.Perfil);
    }

    [TestMethod]
    public async Task TestarCriarAdministradorComDadosInvalidos()
    {
        // Arrange
        var administradorInvalido = new AdministradorDTO
        {
            Email = "", // Email vazio - inválido
            Senha = "", // Senha vazia - inválida
            Perfil = null // Perfil nulo - inválido
        };
        var content = new StringContent(JsonSerializer.Serialize(administradorInvalido), Encoding.UTF8, "application/json");
        
        var request = CriarRequestComToken(HttpMethod.Post, "/administradores");
        request.Content = content;

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
        
        var result = await response.Content.ReadAsStringAsync();
        Assert.IsTrue(result.Contains("email") || result.Contains("senha") || result.Contains("perfil") || 
            result.Contains("Mensagens") || result.Contains("mensagens") || result.Contains("erro"));
    }
    

    [TestMethod]
    public async Task TestarAcessoSemToken()
    {
        // Act - Tenta acessar endpoint protegido sem token
        var response = await Setup.client.GetAsync("/administradores");

        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarAcessoComTokenInvalido()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/administradores");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "token_invalido");

        // Act
        var response = await Setup.client.SendAsync(request);

        // Assert
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [TestMethod]
    public async Task TestarHomeEndpoint()
    {
        // Arrange & Act - Endpoint público
        var response = await Setup.client.GetAsync("/");

        // Assert
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }
}