using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Extensions.Configuration;
using MinimalApi.Dominio.Servicos;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.Entidades;
using MinimalApi.DTOs;

namespace Test.Domain.Servico;

[TestClass]
public class AdministradorServicoTest
{
 private DbContexto CriarContextoDeTeste()
    {
        // SQLite em memória - não precisa de servidor externo
        var options = new DbContextOptionsBuilder<DbContexto>()
            .UseSqlite("Data Source=:memory:")
            .Options;

        // Cria o contexto usando reflection para forçar o construtor correto
        var context = (DbContexto)Activator.CreateInstance(
            typeof(DbContexto), 
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            new object[] { options },
            null)!;

        context.Database.OpenConnection();
        context.Database.EnsureCreated();

        return context;
    }

    [TestCleanup]
    public void Cleanup()
    {
        // Limpeza automática
        using var context = CriarContextoDeTeste();
        context.Database.EnsureDeleted();
    }





    [TestMethod]
    public void TestandoSalvarAdministrador()
    {
        // Arrange
        var context = CriarContextoDeTeste();
        context.Administradores.RemoveRange(context.Administradores);
        context.SaveChanges();

        var adm = new Administrador();
        adm.Email = "teste@teste.com";
        adm.Senha = "teste";
        adm.Perfil = "Adm";


        var administradorServico = new AdministradorServico(context);

        // Act
        administradorServico.Incluir(adm);

        //Assert
        Assert.AreEqual(1, administradorServico.Todos(1).Count());


    }

    [TestMethod]
    public void TestandoBuscaPorId()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Administradores.RemoveRange(context.Administradores);
        context.SaveChanges();

        var adm = new Administrador
        {
            Email = "teste@teste.com",
            Senha = "teste",
            Perfil = "Adm"
        };


        var administradorServico = new AdministradorServico(context);

        // Act
        administradorServico.Incluir(adm);
        var admDoBanco = administradorServico.BuscaPorId(adm.Id);

        //Assert

        Assert.IsNotNull(admDoBanco);
        Assert.AreEqual(adm.Id, admDoBanco.Id);
        Assert.AreEqual("teste@teste.com", admDoBanco.Email);


    }
    [TestMethod]
    public void TestandoLoginComCredenciaisCorretas()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Administradores.RemoveRange(context.Administradores);
        context.SaveChanges();

        var adm = new Administrador 
        { 
            Email = "administrador@teste.com", 
            Senha = "123456", 
            Perfil = "Adm" 
        };

        var administradorServico = new AdministradorServico(context);
        administradorServico.Incluir(adm);

        var loginDTO = new LoginDTO 
        { 
            Email = "administrador@teste.com", 
            Senha = "123456" 
        };

        // Act
        var resultado = administradorServico.Login(loginDTO);

        // Assert
        Assert.IsNotNull(resultado);
        Assert.AreEqual("administrador@teste.com", resultado.Email);
        Assert.AreEqual("Adm", resultado.Perfil);
    }

    [TestMethod]
    public void TestandoLoginComCredenciaisIncorretas()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Administradores.RemoveRange(context.Administradores);
        context.SaveChanges();

        var adm = new Administrador 
        { 
            Email = "administrador@teste.com", 
            Senha = "123456", 
            Perfil = "Adm" 
        };

        var administradorServico = new AdministradorServico(context);
        administradorServico.Incluir(adm);

        var loginDTO = new LoginDTO 
        { 
            Email = "administrador@teste.com", 
            Senha = "senha_errada" // Senha incorreta
        };

        // Act
        var resultado = administradorServico.Login(loginDTO);

        // Assert
        Assert.IsNull(resultado);
    }

    [TestMethod]
    public void TestandoLoginComEmailInexistente()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Administradores.RemoveRange(context.Administradores);
        context.SaveChanges();

        var administradorServico = new AdministradorServico(context);

        var loginDTO = new LoginDTO
        {
            Email = "email_inexistente@teste.com",
            Senha = "123456"
        };

        // Act
        var resultado = administradorServico.Login(loginDTO);

        // Assert
        Assert.IsNull(resultado);
    }
    
    [TestMethod]
    public void TestandoTodosComPaginacao()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Administradores.RemoveRange(context.Administradores);
        context.SaveChanges();

        var administradorServico = new AdministradorServico(context);

        // Adiciona 15 administradores
        for (int i = 1; i <= 15; i++)
        {
            var adm = new Administrador 
            { 
                Email = $"administrador{i}@teste.com", 
                Senha = "senha", 
                Perfil = "Adm" 
            };
            administradorServico.Incluir(adm);
        }

        // Act
        var primeiraPagina = administradorServico.Todos(1);
        var segundaPagina = administradorServico.Todos(2);

        // Assert
        Assert.AreEqual(10, primeiraPagina.Count); // 10 itens por página
        Assert.AreEqual(5, segundaPagina.Count);   // 5 itens na segunda página
    }

}

        