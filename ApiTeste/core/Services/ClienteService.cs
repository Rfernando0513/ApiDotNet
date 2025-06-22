using AutoMapper;
using DesafioMuralis.application.dtos;
using DesafioMuralis.core.Model;
using DesafioMuralis.infra.Data;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.OpenApi.Any;

namespace DesafioMuralis.core.Services
{
    public class ClienteService : ICliente
    {
        private readonly ApiDbContext _context;
        private readonly IMapper _mapper;
        private readonly string _viaCepUrl;
        public ClienteService(ApiDbContext context, IMapper maper)
        {
            this._context = context;
            this._mapper = maper;
            this._viaCepUrl = Environment.GetEnvironmentVariable("VIACEP_URL") ?? throw new Exception("VIACEP_URL não encontrada");
        }

        public async Task<ClienteDto> CriarCliente(ClienteDto clienteDto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(clienteDto.Nome))
                    throw new ArgumentException("O nome do cliente é obrigatório.");

                if (clienteDto.Endereco is null || string.IsNullOrWhiteSpace(clienteDto.Endereco.Cep))
                    throw new ArgumentException("O CEP é obrigatório.");

                if (string.IsNullOrWhiteSpace(clienteDto.Endereco.Numero))
                    throw new ArgumentException("O número do endereço é obrigatório.");

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{_viaCepUrl}{clienteDto.Endereco.Cep}/json/");

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Erro ao consultar o CEP");
                }

                var viaCep = await response.Content.ReadFromJsonAsync<EnderecoDto>();
                if (viaCep is null || string.IsNullOrEmpty(viaCep.Logradouro) || string.IsNullOrEmpty(viaCep.Cidade))
                {
                    throw new Exception("CEP inválido ou não encontrado");
                }

                var cliente = _mapper.Map<ClienteModel>(clienteDto);
                cliente.DataCadastro = DateTime.Now;

                cliente.Endereco = new EnderecoModel
                {
                    Cep = viaCep.Cep,
                    Logradouro = viaCep.Logradouro,
                    Cidade = viaCep.Cidade,
                    Numero = clienteDto.Endereco.Numero,
                    Complemento = clienteDto.Endereco.Complemento
                };

                if (clienteDto.Contatos is not null && clienteDto.Contatos.Any())
                {
                    cliente.Contatos = clienteDto.Contatos
                        .Select(c => _mapper.Map<ContatoModel>(c))
                        .ToList();
                }

                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                return _mapper.Map<ClienteDto>(cliente);
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"Erro de validação: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao criar cliente: {ex.Message}");
            }
        }

        public async Task<List<ClienteDto>> ListarClientes()
        {
            try
            {
                var clientes = await _context.Clientes
                    .Include(c => c.Endereco)
                    .Include(c => c.Contatos)
                    .ToListAsync();

                return _mapper.Map<List<ClienteDto>>(clientes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar clientes: {ex.Message}");
            }
        }
        public async Task<ClienteDto> BuscarClientePorId(int id)
        {
            try
            {
                var cliente = await _context.Clientes
                    .Include(c => c.Endereco)
                    .Include(c => c.Contatos)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    throw new Exception($"Cliente não encontrado");
                }

                return cliente is not null ? _mapper.Map<ClienteDto>(cliente) : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar cliente por ID: {ex.Message}");
            }
        }

        public async Task<ClienteDto> AtualizarCliente(int id, ClienteDto clienteDto)
        {
            try
            {
                var cliente = await _context.Clientes
                    .Include(c => c.Endereco)
                    .Include(c => c.Contatos)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (cliente == null)
                {
                    throw new Exception($"Cliente não encontrado");
                }

                using var httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{_viaCepUrl}{clienteDto.Endereco.Cep}/json/");

                var viaCep = await response.Content.ReadFromJsonAsync<EnderecoDto>();
                if (viaCep is null || string.IsNullOrEmpty(viaCep.Logradouro) || string.IsNullOrEmpty(viaCep.Cidade))
                {
                    throw new Exception("CEP inválido ou não encontrado");
                }

                cliente.Nome = clienteDto.Nome;

                if (cliente.Endereco != null && clienteDto.Endereco != null)
                {
                    cliente.Endereco.Cep = viaCep.Cep;
                    cliente.Endereco.Logradouro = viaCep.Logradouro;
                    cliente.Endereco.Cidade = viaCep.Cidade;
                    cliente.Endereco.Numero = clienteDto.Endereco.Numero;
                    cliente.Endereco.Complemento = clienteDto.Endereco.Complemento;
                }

                _context.Contato.RemoveRange(cliente.Contatos);

                cliente.Contatos = clienteDto.Contatos?
                    .Select(c => new ContatoModel
                    {
                        Tipo = c.Tipo,
                        Texto = c.Texto,
                        ClienteId = id
                    }).ToList();

                await _context.SaveChangesAsync();

                return _mapper.Map<ClienteDto>(cliente);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar cliente por ID: {ex.Message}");
            }
        }

        public async Task<ClienteDto> DeletarCliente(int id)
        {
            try
            {
                var cliente = _context.Clientes
                    .Include(c => c.Endereco)
                    .Include(c => c.Contatos)
                    .FirstOrDefault(c => c.Id == id);

                if (cliente == null)
                {
                    throw new Exception($"Cliente não encontrado");
                }

                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();

                await _context.Clientes.ToListAsync();

                return cliente is not null ? _mapper.Map<ClienteDto>(cliente) : null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar cliente por ID: {ex.Message}");
            }
        }

        public async Task<List<ClienteDto>> PesquisarClientesPorNome(string nome)
        {
            try
            {
                var clientes = await _context.Clientes
                    .Include(c => c.Endereco)
                    .Include(c => c.Contatos)
                    .Where(c => c.Nome.ToLower().Contains(nome.ToLower()))
                    .ToListAsync();

                if(clientes == null)
                {
                    throw new Exception($"Nenhum cliente encontrado com o nome: {nome}");
                }

                return _mapper.Map<List<ClienteDto>>(clientes);

            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao buscar clientes por nome: {ex.Message}");
            }
        }
    }
}
