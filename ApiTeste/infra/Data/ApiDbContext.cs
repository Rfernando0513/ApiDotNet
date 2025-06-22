using DesafioMuralis.core.Model;
using Microsoft.EntityFrameworkCore;

namespace DesafioMuralis.infra.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<ClienteModel> Clientes { get; set; }
        public DbSet<ContatoModel> Contato { get; set; }
        public DbSet<EnderecoModel> Endereco { get; set; }
    }
}
