using AutoMapper;
using DesafioMuralis.application.dtos;
using DesafioMuralis.core.Model;

namespace DesafioMuralis.application.Mapper
{
    public class ClienteMapper : Profile
    {
        public ClienteMapper()
        {
            CreateMap<ClienteModel, ClienteDto>()
                .ReverseMap();

            CreateMap<ContatoModel, ContatoDto>()
                .ReverseMap();

            CreateMap<EnderecoModel, EnderecoDto>()
                .ReverseMap();
        }
    }
}
