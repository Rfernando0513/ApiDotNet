namespace DesafioMuralis.application.dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public EnderecoDto Endereco { get; set; }
        public List<ContatoDto> Contatos { get; set; }
    }
}
