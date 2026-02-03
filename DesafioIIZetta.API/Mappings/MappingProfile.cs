using AutoMapper;
using DesafioIIZetta.API.Models;
using DesafioIIZetta.API.DTOs.Cliente;
using DesafioIIZetta.API.DTOs.Livro;
using DesafioIIZetta.API.DTOs.Emprestimo;

namespace DesafioIIZetta.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // SAÍDA (GETs)
            CreateMap<Cliente, ClienteExibicaoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCliente))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NomeCliente))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailCliente));

            CreateMap<Cliente, ClienteDetalhesDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCliente))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NomeCliente))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EmailCliente))
                .ForMember(dest => dest.Cpf, opt => opt.MapFrom(src => src.Cpfcliente))
                .ForMember(dest => dest.Telefone, opt => opt.MapFrom(src => src.TelefoneCliente))
                .ForMember(dest => dest.Endereco, opt => opt.MapFrom(src => src.EnderecoCliente));

            // ENTRADA (POST e PUT)
            CreateMap<ClienteAdicionarDTO, Cliente>()
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.EmailCliente, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Cpfcliente, opt => opt.MapFrom(src => src.Cpf))
                .ForMember(dest => dest.TelefoneCliente, opt => opt.MapFrom(src => src.Telefone))
                .ForMember(dest => dest.EnderecoCliente, opt => opt.MapFrom(src => src.Endereco));

            CreateMap<ClienteAtualizarDTO, Cliente>()
                .ForMember(dest => dest.IdCliente, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.Nome))
                .ForMember(dest => dest.EmailCliente, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Cpfcliente, opt => opt.MapFrom(src => src.Cpf))
                .ForMember(dest => dest.TelefoneCliente, opt => opt.MapFrom(src => src.Telefone))
                .ForMember(dest => dest.EnderecoCliente, opt => opt.MapFrom(src => src.Endereco));


            // SAÍDA (GET)
            CreateMap<Livro, LivroExibicaoDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdLivro))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.TituloLivro))
                .ForMember(dest => dest.Autor, opt => opt.MapFrom(src => src.AutorLivro))
                .ForMember(dest => dest.Estoque, opt => opt.MapFrom(src => src.QuantidadeEstoqueLivro));

            // ENTRADA (POST)
            CreateMap<LivroAdicionarDTO, Livro>()
                .ForMember(dest => dest.TituloLivro, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.AutorLivro, opt => opt.MapFrom(src => src.Autor))
                .ForMember(dest => dest.AnoPublicacaoLivro, opt => opt.MapFrom(src => src.Ano))
                .ForMember(dest => dest.QuantidadeEstoqueLivro, opt => opt.MapFrom(src => src.Estoque));

            // ENTRADA (PUT)
            CreateMap<LivroAtualizarDTO, Livro>()
                .ForMember(dest => dest.IdLivro, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TituloLivro, opt => opt.MapFrom(src => src.Titulo))
                .ForMember(dest => dest.AutorLivro, opt => opt.MapFrom(src => src.Autor))
                .ForMember(dest => dest.AnoPublicacaoLivro, opt => opt.MapFrom(src => src.Ano))
                .ForMember(dest => dest.QuantidadeEstoqueLivro, opt => opt.MapFrom(src => src.Estoque));


            // 1. Saída: Ligar as propriedades das tabelas relacionadas aos nomes simples do DTO
            CreateMap<ClienteLivroEmprestimo, EmprestimoExibicaoDTO>()
                .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.IdClienteNavigation.NomeCliente))
                .ForMember(dest => dest.TituloLivro, opt => opt.MapFrom(src => src.IdLivroNavigation.TituloLivro));

            // 2. Entrada: Calcular as datas automaticamente ao receber o DTO
            CreateMap<EmprestimoAdicionarDTO, ClienteLivroEmprestimo>()
                .ForMember(dest => dest.DataEmprestimo, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.DataDevolucaoPrevista, opt => opt.MapFrom(src => DateTime.Now.AddDays(src.DiasEmprestimo)));
        }
    }
}