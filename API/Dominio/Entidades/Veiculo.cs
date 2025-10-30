using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.Entidades;

    public class Veiculo
    {
        [Key]// chave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]// auto incremento
        public int Id { get; set; } = default!;

        [Required] // obrigatorio o preenchimento
        [StringLength(150)]// tamanho maximo
        public string Nome { get; set; } = default!;

        [Required]
        [StringLength(100)]
        public string Marca { get; set; } = default!;

        [Required]       
        public int Ano { get; set; } = default!;

    }
