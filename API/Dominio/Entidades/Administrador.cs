using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalApi.Dominio.Entidades
{
    public class Administrador
    {
        [Key]// chave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]// auto incremento
        public int Id { get; set; } = default!;

        [Required] // obrigatorio o preenchimento
        [StringLength(255)]// tamanho maximo
        public string Email { get; set; } = default!;

        [Required]
        [StringLength(50)]
        public string Senha { get; set; } = default!;

        [Required]
        [StringLength(10)]
        public string Perfil { get; set; } = default!;

    }
}