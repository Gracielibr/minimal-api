using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.ModelViews
{
    public struct Home
    {
        public string Mensagem { get => "Bem Vindo a API de veículos - Minimal API"; }
        public string Doc { get => "/swagger"; }
        
    }
}