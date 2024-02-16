using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiBackend.Model;
using apiBackend.Contexts;
using Microsoft.EntityFrameworkCore;

namespace apiBackend.Services
{
    public interface IClienteService
    {
        Task<ResStatus> doSaveClient(ClienteDTO clientDTO);

        Task<ResStatus> doLoadClients();

        Task<ResStatus> doLoadClientById(Guid clientId);

        Task<ResStatus> doLoadClientByCnpj(long cnpj);

        Task<bool> doRemoveClient(Cliente client);

        Task<ResStatus> doUpdateClient(ClienteDTO clientDTO, Cliente client);
    }

    public class ClienteService : IClienteService
    {
        private readonly AppDbContext _dbContext;

        public ClienteService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region === FUNÇÕES GERAIS ===

        private Cliente doFormatClient(ClienteDTO clienteDTO, Cliente? client)
        {
            Cliente newClient = client ?? new Cliente();

            newClient.endereco = clienteDTO.endereco;
            newClient.telefone = clienteDTO.telefone;
            if (!string.IsNullOrEmpty(clienteDTO.nome)) newClient.nome = clienteDTO.nome;
            if (clienteDTO.cnpj != null) newClient.cnpj = clienteDTO.cnpj;

            return newClient;
        }

        private ResStatus doFormatResStatus(bool failed, Cliente? cliente, List<Cliente>? clients)
        {
            return new ResStatus(
                failed: failed,
                cliente: cliente,
                manyClientes: clients
            );
        }

        private bool doValidateClient(Cliente client)
        {
            bool isValid = false;

            if (client.cnpj != null && !string.IsNullOrEmpty(client.nome)) isValid = true;

            return isValid;
        }

        #endregion

        #region === FUNÇÕES DE CONSULTA ===

        public async Task<ResStatus> doLoadClients()
        {
            ResStatus newResFind = this.doFormatResStatus(true, null, null);

            try
            {
                List<Cliente> resFind = await _dbContext.cliente.ToListAsync();

                newResFind = this.doFormatResStatus(false, null, resFind);

                return newResFind;
            }
            catch (Exception ex)
            {
                return newResFind;
            }
        }

        public async Task<ResStatus> doLoadClientById(Guid clientId)
        {
            ResStatus newResSave = this.doFormatResStatus(true, null, null);

            try
            {
                if (clientId == Guid.Empty) return null;

                Cliente resFindClient = await _dbContext.cliente.FindAsync(clientId);

                newResSave = this.doFormatResStatus(false, resFindClient, null);

                return newResSave;
            }
            catch (Exception ex)
            {
                return newResSave;
            }
        }

        public async Task<ResStatus> doLoadClientByCnpj(long cnpj)
        {
            ResStatus newResSave = this.doFormatResStatus(true, null, null);

            try
            {
                if (cnpj == null) return null;

                Cliente resFindClient = await _dbContext.cliente.FirstOrDefaultAsync(c => c.cnpj == cnpj);

                newResSave = this.doFormatResStatus(false, resFindClient, null);

                return newResSave;
            }
            catch (Exception ex)
            {
                return newResSave;
            }
        }

        #endregion

        #region === FUNÇÕES DE CADASTRO ===

        public async Task<ResStatus> doSaveClient(ClienteDTO clienteDTO)
        {
            ResStatus newResSave = this.doFormatResStatus(true, null, null);

            try
            {
                if (clienteDTO == null) return null;

                Cliente newClient = this.doFormatClient(clienteDTO, null);

                if (!this.doValidateClient(newClient)) return null;

                _dbContext.cliente.Add(newClient);

                await _dbContext.SaveChangesAsync();

                newResSave = this.doFormatResStatus(false, newClient, null);

                return newResSave;
            }
            catch (Exception ex)
            {
                return newResSave;
            }
        }

        #endregion

        #region === FUNÇÕES DE DELETE ===

        public async Task<bool> doRemoveClient(Cliente cliente)
        {
            try
            {
                if (cliente == null) return false;

                _dbContext.cliente.Remove(cliente);

                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region === FUNÇÕES DE UPDATE ===

        public async Task<ResStatus> doUpdateClient(ClienteDTO clienteDTO, Cliente client)
        {
            ResStatus newResUpdate = this.doFormatResStatus(true, null, null);

            try
            {

                if (clienteDTO == null || client == null) return newResUpdate;

                Cliente newClient = this.doFormatClient(clienteDTO, client);

                if (!this.doValidateClient(newClient)) return newResUpdate;

                await _dbContext.SaveChangesAsync();

                newResUpdate = this.doFormatResStatus(false, newClient, null);

                return newResUpdate;
            }
            catch (Exception ex)
            {
                return newResUpdate;
            }
        }

        #endregion
    }
}