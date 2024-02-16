using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace apiBackend.Model
{
    public class ResStatus
    {
        public bool failed { get; set; }

        public Cliente cliente { get; set; }

        public List<Cliente> manyClientes { get; set; }

        public ResStatus(bool failed, Cliente? cliente, List<Cliente>? manyClientes)
        {
            this.failed = failed;
            this.cliente = cliente;
            this.manyClientes = manyClientes;
        }
    }
}