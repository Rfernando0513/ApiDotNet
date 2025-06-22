namespace DesafioMuralis.core.Model
{
    public class ClienteModel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public EnderecoModel Endereco { get; set; }
        public List<ContatoModel> Contatos { get; set; }
    }
}
