using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using apiBackend.Model;
using apiBackend.Services;

namespace apiBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        #region === ROTAS DE CONSULTA ===

        [HttpGet]
        public async Task<IActionResult> doGetClients()
        {
            try
            {
                ResStatus resFind = await _clienteService.doLoadClients();

                if (resFind.failed)
                {
                    return BadRequest("Não foi possível consultar os clientes!");
                }

                return Ok(resFind.manyClientes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Não foi possível realizar essa operação");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> doGetClientById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Código inválido ou não preenchido!");
                }

                ResStatus resFind = await _clienteService.doLoadClientById(id);

                if (resFind.cliente == null || resFind.failed)
                {
                    return NotFound("Cliente inválido ou não cadastrado!");
                }

                return Ok(resFind.cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Não foi possível realizar essa operação!");
            }
        }

        #endregion

        #region === ROTAS DE CADASTRO === 

        [HttpPost]
        public async Task<IActionResult> doCreateClient(ClienteDTO clienteDTO)
        {
            try
            {
                if (clienteDTO.cnpj == null || clienteDTO.cnpj.ToString().Length != 14)
                {
                    return BadRequest("Cnpj inválido ou não informado!");
                }

                ResStatus resFindClientCnpj = await _clienteService.doLoadClientByCnpj(clienteDTO.cnpj);

                if (resFindClientCnpj.failed) {
                    return NotFound("Não foi possível validar o CNPJ, tente novamente mais tarde!");
                }

                if (resFindClientCnpj.cliente != null)
                {
                    return BadRequest("Cnpj informado ja está cadastrado!");
                }

                ResStatus resSave = await _clienteService.doSaveClient(clienteDTO);

                if (resSave.cliente == null || resSave.failed)
                {
                    return BadRequest("Não foi possível cadastrar o novo cliente!");
                }

                return CreatedAtAction(nameof(doCreateClient), resSave.cliente);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Não foi possível realizar essa operação");
            }
        }

        #endregion

        #region === ROTAS DE DELETE ===

        [HttpDelete("{id}")]
        public async Task<IActionResult> doDeleteClientById(Guid id)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Código inválido ou não informado!");
                }

                ResStatus resFindClient = await _clienteService.doLoadClientById(id);

                if (resFindClient.cliente == null || resFindClient.failed)
                {
                    return NotFound("Cliente inválido ou não cadastrado!");
                }

                bool resDelete = await _clienteService.doRemoveClient(resFindClient.cliente);

                if (!resDelete)
                {
                    return BadRequest("Não foi possível excluir o cliente!");
                }

                return Ok("Cliente excluído com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Não foi possível realizar essa operação");
            }
        }

        #endregion

        #region === ROTAS DE UPDATE ===

        [HttpPut("{id}")]
        public async Task<IActionResult> doUpdateClientById(Guid id, ClienteDTO clienteDTO)
        {
            try
            {
                if (id == Guid.Empty)
                {
                    return BadRequest("Código inválido ou não informado!");
                }

                ResStatus resFindClient = await _clienteService.doLoadClientById(id);

                if (resFindClient.cliente == null || resFindClient.failed)
                {
                    return NotFound("Cliente inválido ou não cadastrado!");
                }

                if (clienteDTO.cnpj == null || clienteDTO.cnpj.ToString().Length != 14)
                {
                    return BadRequest("Cnpj inválido ou não informado!");
                }

                ResStatus resFindClientCnpj = await _clienteService.doLoadClientByCnpj(clienteDTO.cnpj);

                if (resFindClient.failed) {
                    return NotFound("Não foi possível validar o CNPJ, tente novamente mais tarde!");
                }

                if (resFindClientCnpj.cliente != null && resFindClientCnpj.cliente.id != id)
                {
                    return BadRequest("Cnpj informado ja está cadastrado!");
                }

                ResStatus resUpdate = await _clienteService.doUpdateClient(clienteDTO, resFindClient.cliente);

                if (resUpdate.failed || resUpdate.cliente == null)
                {
                    return BadRequest("Não foi possível atualizar o cliente!");
                }

                return Ok(resUpdate.cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Não foi possível realizar essa operação");
            }
        }

        #endregion

    }
}