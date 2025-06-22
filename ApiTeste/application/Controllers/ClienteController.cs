using DesafioMuralis.application.dtos;
using DesafioMuralis.core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DesafioMuralis.application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly ICliente _clienteService;
        public ClienteController(ICliente clienteService)
        {
            _clienteService = clienteService;
        }

        [HttpPost("CriarCliente")]
        public async Task<ActionResult> CriarCliente([FromBody] ClienteDto clienteDto)
        {
            if (clienteDto == null)
            {
                return BadRequest("Dados do cliente não podem ser nulos.");
            }

            try
            {
                var clienteCriado = await _clienteService.CriarCliente(clienteDto);
                return CreatedAtAction(nameof(BuscarClientePorId), new { id = clienteCriado.Id }, clienteCriado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao criar cliente: {ex.Message}");
            }
        }
        [HttpGet("ListarClientes")]
        public async Task<ActionResult> ListarClientes()
        {
            try
            {
                var clientes = await _clienteService.ListarClientes();
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao listar clientes: {ex.Message}");
            }
        }

        [HttpGet("ListarClientePorId/{id}")]
        public async Task<ActionResult> BuscarClientePorId(int id)
        {
            try
            {
                var cliente = await _clienteService.BuscarClientePorId(id);

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar cliente: {ex.Message}");
            }
        }

        [HttpPut("AtualizarCliente/{id}")]
        public async Task<ActionResult> AtualizarCliente(int id, [FromBody] ClienteDto clienteDto)
        {
            try
            {
                var cliente = await _clienteService.AtualizarCliente(id, clienteDto);

                return Ok(cliente);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao atualizar cliente: {ex.Message}");
            }
        }

        [HttpDelete("DeletarCliente/{id}")]
        public async Task<ActionResult> DeletarCliente(int id)
        {
            try
            {
                var cliente = await _clienteService.DeletarCliente(id);

                return Ok($"Cliente {cliente.Nome} deletado com sucesso!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao deletar cliente: {ex.Message}");
            }
        }

        [HttpGet("PesquisarClientesPorNome/{nome}")]
        public async Task<ActionResult> PesquisarClientesPorNome(string nome)
        {
            try
            {
                var clientes = await _clienteService.PesquisarClientesPorNome(nome);

                return Ok(clientes);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao pesquisar clientes por nome: {ex.Message}");
            }
        }
    }
}
