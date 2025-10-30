using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MinimalApi.Dominio.Entidades;


namespace MinimalApi.Infraestrutura.Db;

    public class DbContexto : DbContext
    {
        // private readonly IConfiguration? _configuracaoAppSettings;
        // public DbContexto(IConfiguration configuracaoAppSettings)
        // {
        // _configuracaoAppSettings = configuracaoAppSettings;

        // }
        
        public DbContexto(DbContextOptions<DbContexto> options) : base(options)
        {
        }
        public DbSet<Administrador> Administradores { get; set; } = default!;

        public DbSet<Veiculo> Veiculos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Administrador>().HasData(
            new Administrador
            {
                Id = 1,
                Email = "administrador@teste.com",
                Senha = "123456",
                Perfil = "Adm"
            });
        }
                    
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         if (_configuracaoAppSettings != null)
        //         {
        //             var stringConexao = _configuracaoAppSettings.GetConnectionString("Mysql")?.ToString();
        //             if (!string.IsNullOrEmpty(stringConexao))
        //             {
        //             optionsBuilder.UseMySql(stringConexao, ServerVersion.AutoDetect(stringConexao));

        //             }
        //         }
        //     }

        // }
    
    }
