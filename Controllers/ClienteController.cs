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
                List<Cliente> resFind = await _clienteService.doLoadClients();

                if (resFind == null)
                {
                    return BadRequest("Não foi possível consultar os clientes!");
                }

                return Ok(resFind);
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

                Cliente resFind = await _clienteService.doLoadClientById(id);

                if (resFind == null)
                {
                    return NotFound("Cliente inválido ou não cadastrado!");
                }

                return Ok(resFind);
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

                Cliente resFindClientCnpj = await _clienteService.doLoadClientByCnpj(clienteDTO.cnpj);

                if (resFindClientCnpj != null)
                {
                    return BadRequest("Cnpj informado ja está cadastrado!");
                }

                var resSave = await _clienteService.doSaveClient(clienteDTO);

                if (resSave == null)
                {
                    return BadRequest("Não foi possível cadastrar o novo cliente!");
                }

                return CreatedAtAction(nameof(doCreateClient), resSave);

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

                Cliente resFindClient = await _clienteService.doLoadClientById(id);

                if (resFindClient == null)
                {
                    return NotFound("Cliente inválido ou não cadastrado!");
                }

                bool resDelete = await _clienteService.doRemoveClient(resFindClient);

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

                Cliente resFindClient = await _clienteService.doLoadClientById(id);

                if (resFindClient == null)
                {
                    return NotFound("Cliente inválido ou não cadastrado!");
                }

                if (clienteDTO.cnpj == null || clienteDTO.cnpj.ToString().Length != 14)
                {
                    return BadRequest("Cnpj inválido ou não informado!");
                }

                Cliente resFindClientCnpj = await _clienteService.doLoadClientByCnpj(clienteDTO.cnpj);

                if (resFindClientCnpj != null && resFindClientCnpj.id != id)
                {
                    return BadRequest("Cnpj informado ja está cadastrado!");
                }

                var resUpdate = await _clienteService.doUpdateClient(clienteDTO, resFindClient);

                if (resUpdate == null)
                {
                    return BadRequest("Não foi possível atualizar o cliente!");
                }

                return Ok(resUpdate);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Não foi possível realizar essa operação");
            }
        }

        #endregion

    }
}