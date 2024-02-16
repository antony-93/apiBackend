using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace apiBackend.Model
{
    public class ClienteDTO
    {
        [Required]
        [MaxLength(255)]
        public string nome { get; set; }

        [Required]
        public long cnpj { get; set; }

        [MaxLength(255)]
        public string endereco { get; set; }

        public long? telefone { get; set; }
    }
}