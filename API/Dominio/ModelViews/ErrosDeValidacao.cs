using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.ModelViews;

    
//struct (struct é um tipo valor, classe é um tipo referência) - não precisa instanciar
//struct é mais leve, mas não pode ter herança  e não pode ter construtor
//struct é usado para coisas pequenas, simples, que não vão mudar muito de estado
//struct é usado para agrupar dados relacionados   
public struct ErrosDeValidacao
    {
        public List<string> Mensagens { get; set; }
    }