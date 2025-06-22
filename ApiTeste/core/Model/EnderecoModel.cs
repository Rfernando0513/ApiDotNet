namespace DesafioMuralis.core.Model
{
    public class EnderecoModel
    {
        public int Id { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public int ClienteId { get; set; }
        public ClienteModel Cliente { get; set; } 
    }
}
