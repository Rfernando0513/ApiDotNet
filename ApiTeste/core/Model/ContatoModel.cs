namespace DesafioMuralis.core.Model
{
    public class ContatoModel
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public int ClienteId { get; set; } 

        public ClienteModel Cliente { get; set; }

    }
}
