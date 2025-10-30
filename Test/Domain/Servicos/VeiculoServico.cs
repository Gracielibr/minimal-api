using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;
using MinimalApi.Infraestrutura.Db;
using MinimalApi.Dominio.Servicos;


namespace Test.Domain.Servicos;

[TestClass]
public class VeiculoServicoTest
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
    public void TestandoSalvarVeiculo()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Veiculos.RemoveRange(context.Veiculos);
        context.SaveChanges();

        var v = new Veiculo();
        v.Nome = "Sandero";
        v.Marca = "Renault";
        v.Ano = 2014;


        var veiculoServico = new VeiculoServico(context);

        // Act
        veiculoServico.Incluir(v);

        //Assert
        Assert.AreEqual(1, veiculoServico.Todos(1).Count());


    }

    [TestMethod]
    public void TestandoBuscaPorId()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Veiculos.RemoveRange(context.Veiculos);
        context.SaveChanges();

        var v = new Veiculo
        {
            Nome = "HB20",
            Marca = "Hyundai",
            Ano = 2015
        };


        var veiculoServico = new VeiculoServico(context);

        // Act
        veiculoServico.Incluir(v);
        var veiculoDoBanco = veiculoServico.BuscarPorId(v.Id);

        //Assert

        Assert.IsNotNull(veiculoDoBanco);
        Assert.AreEqual(v.Id, veiculoDoBanco.Id);
        Assert.AreEqual("HB20", veiculoDoBanco.Nome);
        Assert.AreEqual("Hyundai", veiculoDoBanco.Marca);
        Assert.AreEqual(2015, veiculoDoBanco.Ano);


    }
    
    [TestMethod]
    public void TestandoAtualizarVeiculo()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Veiculos.RemoveRange(context.Veiculos);
        context.SaveChanges();

        var v = new Veiculo 
        { 
            Nome = "Fusca",
            Marca = "Volkswagen", 
            Ano = 1990
        };

        var veiculoServico = new VeiculoServico(context);
        veiculoServico.Incluir(v);

        // Act
        v.Nome = "Fusca Atualizado";
        v.Ano = 1991;
        veiculoServico.Atualizar(v);
        
        var veiculoAtualizado = veiculoServico.BuscarPorId(v.Id);

        // Assert
        Assert.IsNotNull(veiculoAtualizado);
        Assert.AreEqual("Fusca Atualizado", veiculoAtualizado.Nome);
        Assert.AreEqual(1991, veiculoAtualizado.Ano);
    }

    [TestMethod]
    public void TestandoApagarVeiculo()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Veiculos.RemoveRange(context.Veiculos);
        context.SaveChanges();

        var v = new Veiculo 
        { 
            Nome = "Uno",
            Marca = "Fiat", 
            Ano = 2015
        };

        var veiculoServico = new VeiculoServico(context);
        veiculoServico.Incluir(v);

        // Act
        veiculoServico.Apagar(v);
        var veiculoApagado = veiculoServico.BuscarPorId(v.Id);

        // Assert
        Assert.IsNull(veiculoApagado);
    }

    [TestMethod]
    public void TestandoTodosComPaginacao()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Veiculos.RemoveRange(context.Veiculos);
        context.SaveChanges();

        var veiculoServico = new VeiculoServico(context);

        // Adiciona 15 veículos
        for (int i = 1; i <= 15; i++)
        {
            var v = new Veiculo
            {
                Nome = $"Veículo {i}",
                Marca = "Marca Teste",
                Ano = 2000 + i
            };
            veiculoServico.Incluir(v);
        }

        // Act
        var primeiraPagina = veiculoServico.Todos(1);
        var segundaPagina = veiculoServico.Todos(2);

        // Assert
        Assert.AreEqual(10, primeiraPagina.Count); // 10 itens por página
        Assert.AreEqual(5, segundaPagina.Count);   // 5 itens na segunda página
    }
    [TestMethod]
    public void TestandoTodosComFiltroPorNome()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Veiculos.RemoveRange(context.Veiculos);
        context.SaveChanges();

        var veiculoServico = new VeiculoServico(context);

        // Adiciona veículos com nomes diferentes
        var veiculos = new List<Veiculo>
        {
            new Veiculo { Nome = "Gol", Marca = "Volkswagen", Ano = 2020 },
            new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2021 },
            new Veiculo { Nome = "Corolla", Marca = "Toyota", Ano = 2022 }
        };

        foreach (var veiculo in veiculos)
        {
            veiculoServico.Incluir(veiculo);
        }

        // Act - Filtra por nome que contém "ol"
        var resultado = veiculoServico.Todos(1, "ol");

        // Assert
        Assert.AreEqual(2, resultado.Count); // Gol e Corolla
        Assert.IsTrue(resultado.Any(v => v.Nome == "Gol"));
        Assert.IsTrue(resultado.Any(v => v.Nome == "Corolla"));
    }

    [TestMethod]
    public void TestandoTodosComFiltroPorMarca()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Veiculos.RemoveRange(context.Veiculos);
        context.SaveChanges();

        var veiculoServico = new VeiculoServico(context);

        // Adiciona veículos de marcas diferentes
        var veiculos = new List<Veiculo>
        {
            new Veiculo { Nome = "Gol", Marca = "Volkswagen", Ano = 2020 },
            new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2021 },
            new Veiculo { Nome = "Onix", Marca = "Chevrolet", Ano = 2022 },
            new Veiculo { Nome = "Fiesta", Marca = "Ford", Ano = 2019 }
        };

        foreach (var veiculo in veiculos)
        {
            veiculoServico.Incluir(veiculo);
        }

        // Act - Filtra por marca que contém "volk"
        var resultado = veiculoServico.Todos(1, null, "volk");

        // Assert
        Assert.AreEqual(1, resultado.Count);
        Assert.AreEqual("Volkswagen", resultado[0].Marca);
        Assert.AreEqual("Gol", resultado[0].Nome);
    }

    [TestMethod]
    public void TestandoTodosSemFiltros()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Veiculos.RemoveRange(context.Veiculos);
        context.SaveChanges();

        var veiculoServico = new VeiculoServico(context);

        // Adiciona alguns veículos
        for (int i = 1; i <= 5; i++)
        {
            var v = new Veiculo 
            { 
                Nome = $"Veículo {i}",
                Marca = "Marca Teste", 
                Ano = 2000 + i
            };
            veiculoServico.Incluir(v);
        }

        // Act - Chama sem filtros
        var resultado = veiculoServico.Todos();

        // Assert
        Assert.AreEqual(5, resultado.Count); // Todos os veículos
    }

    [TestMethod]
    public void TestandoTodosComFiltroNomeMarcaCombinados()
    {
        // Arrange
        using var context = CriarContextoDeTeste();
        context.Veiculos.RemoveRange(context.Veiculos);
        context.SaveChanges();

        var veiculoServico = new VeiculoServico(context);

        var veiculos = new List<Veiculo>
        {
            new Veiculo { Nome = "Gol", Marca = "Volkswagen", Ano = 2020 },
            new Veiculo { Nome = "Saveiro", Marca = "Volkswagen", Ano = 2021 },
            new Veiculo { Nome = "Civic", Marca = "Honda", Ano = 2022 },
            new Veiculo { Nome = "Golf", Marca = "Volkswagen", Ano = 2023 }
        };

        foreach (var veiculo in veiculos)
        {
            veiculoServico.Incluir(veiculo);
        }

        // Act - Filtra por nome "gol" e marca "volk"
        var resultado = veiculoServico.Todos(1, "gol", "volk");

        // Assert
        Assert.AreEqual(2, resultado.Count);
        Assert.IsTrue(resultado.All(v => v.Marca == "Volkswagen"));
        Assert.IsTrue(resultado.Any(v => v.Nome == "Gol"));
        Assert.IsTrue(resultado.Any(v => v.Nome == "Golf"));
    }
}
   

