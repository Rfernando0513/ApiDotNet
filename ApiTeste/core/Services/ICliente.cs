using DesafioMuralis.application.dtos;

namespace DesafioMuralis.core.Services
{
    public interface ICliente
    {
        Task<ClienteDto> CriarCliente(ClienteDto cliente);
        Task<List<ClienteDto>> ListarClientes();
        Task<ClienteDto> BuscarClientePorId(int id);
        Task<ClienteDto> AtualizarCliente(int id, ClienteDto clienteDto);
        Task<ClienteDto> DeletarCliente(int id);
        Task<List<ClienteDto>> PesquisarClientesPorNome(string nome);

    }
}
