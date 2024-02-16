using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apiBackend.Model
{

    [Table("cliente")]
    public class Cliente
    {
        [Key]
        public Guid id { get; private set; }

        [Required]
        [MaxLength(255)]
        public string nome { get; set; }

        [Required]
        public long cnpj { get; set; }

        public DateTime datacadastro { get; private set; }

        [MaxLength(255)]
        public string endereco { get; set; }

        public long? telefone { get; set; }
    }
}