using System.Text.Json.Serialization;

namespace DesafioMuralis.application.dtos
{
    public class EnderecoDto
    {
        public string Cep { get; set; }
        public string? Logradouro { get; set; }

        [JsonPropertyName("localidade")]
        public string? Cidade { get; set; }
        public string Numero { get; set; }
        public string? Complemento { get; set; }
    }
}