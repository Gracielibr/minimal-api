using MinimalApi.Dominio.Entidades;
using MinimalApi.Dominio.Interfaces;


namespace Test.Mocks;

public class VeiculoServicoMock : IVeiculoServico
{
    private static List<Veiculo> veiculos = new List<Veiculo>()
    {        
        new Veiculo
        {
            Id = 1,
            Nome = "Gol",
            Marca = "Volkswagen",
            Ano = 2020
        },
        new Veiculo
        {
            Id = 2,
            Nome = "Civic",
            Marca = "Honda", 
            Ano = 2021
        },
        new Veiculo
        {
            Id = 3,
            Nome = "Onix",
            Marca = "Chevrolet",
            Ano = 2022
        },
        new Veiculo
        {
            Id = 4,
            Nome = "Corolla",
            Marca = "Toyota",
            Ano = 2023
        }
    };
    public void Apagar(Veiculo veiculo)
    {
        veiculos.Remove(veiculo);
    }

    public void Atualizar(Veiculo veiculo)
    {
        var veiculoExistente = veiculos.Find(v => v.Id == veiculo.Id);
        if (veiculoExistente != null)
        {
            veiculoExistente.Nome = veiculo.Nome;
            veiculoExistente.Marca = veiculo.Marca;
            veiculoExistente.Ano = veiculo.Ano;
        }
    }

    public Veiculo? BuscarPorId(int id)
    {
        return veiculos.Find(v => v.Id == id);
    }

    public void Incluir(Veiculo veiculo)
    {
        veiculo.Id = veiculos.Count + 1;
        veiculos.Add(veiculo);
    }

    public List<Veiculo> Todos(int? pagina = 1, string? nome = null, string? marca = null)
    {
        var query = veiculos.AsQueryable();

        if (!string.IsNullOrEmpty(nome))
        {
            query = query.Where(v => v.Nome.ToLower().Contains(nome.ToLower()));
        }

        if (!string.IsNullOrEmpty(marca))
        {
            query = query.Where(v => v.Marca.ToLower().Contains(marca.ToLower()));
        }

        int itensPorPagina = 10;
        if (pagina != null)
        {
            query = query.Skip(((int)pagina - 1) * itensPorPagina).Take(itensPorPagina);
        }

        return query.ToList();
    
    }
}