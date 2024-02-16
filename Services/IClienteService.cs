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
        Task<Cliente> doSaveClient(ClienteDTO clientDTO);

        Task<List<Cliente>> doLoadClients();

        Task<Cliente> doLoadClientById(Guid clientId);

        Task<Cliente> doLoadClientByCnpj(long cnpj);

        Task<bool> doRemoveClient(Cliente client);

        Task<Cliente> doUpdateClient(ClienteDTO clientDTO, Cliente client);
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

        private bool doValidateClient(Cliente client) {
            bool isValid = false;

            if (client.cnpj != null && !string.IsNullOrEmpty(client.nome)) isValid = true;

            return isValid;
        }

        #endregion

        #region === FUNÇÕES DE CONSULTA ===

        public async Task<List<Cliente>> doLoadClients()
        {
            try
            {
                return await _dbContext.cliente.ToListAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Cliente> doLoadClientById(Guid clientId)
        {
            try
            {
                if (clientId == Guid.Empty) return null;

                Cliente resFindClient = await _dbContext.cliente.FindAsync(clientId);

                return resFindClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Cliente> doLoadClientByCnpj(long cnpj)
        {
            try
            {
                if (cnpj == null) return null;

                Cliente resFindClient = await _dbContext.cliente.FirstOrDefaultAsync(c => c.cnpj == cnpj);

                return resFindClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region === FUNÇÕES DE CADASTRO ===

        public async Task<Cliente> doSaveClient(ClienteDTO clienteDTO)
        {
            try
            {
                if (clienteDTO == null) return null;

                Cliente newClient = this.doFormatClient(clienteDTO, null);

                if (!this.doValidateClient(newClient)) return null;

                _dbContext.cliente.Add(newClient);

                await _dbContext.SaveChangesAsync();

                return newClient;
            }
            catch (Exception ex)
            {
                return null;
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

        public async Task<Cliente> doUpdateClient(ClienteDTO clienteDTO, Cliente client)
        {
            try
            {
                if (clienteDTO == null || client == null) return null;

                Cliente newClient = this.doFormatClient(clienteDTO, client);

                if (!this.doValidateClient(newClient)) return null;

                await _dbContext.SaveChangesAsync();

                return newClient;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }
}