    using AutoMapper;
    using DesafioIIZetta.API.DTOs.Biblioteca.Cliente;
    using DesafioIIZetta.API.DTOs.Biblioteca.Emprestimo;
    using DesafioIIZetta.API.DTOs.GestaoTarefas.Tarefa;
    using DesafioIIZetta.API.DTOs.GestaoTarefas.Usuario;
    using DesafioIIZetta.API.DTOs.Livro;
    using DesafioIIZetta.API.Models;
    using DesafioIIZetta.API.Models.Biblioteca;
    using DesafioIIZetta.API.Models.GestaoTarefas;

    namespace DesafioIIZetta.API.Mappings
    {
        public class MappingProfile : Profile
        {
            public MappingProfile()
            {

                // --- MAPEAMENTOS DE CLIENTE ---

                // Saida Cliente (GETs): Simplifica os nomes das propriedades para o DTO
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

                // Entrada (POST e PUT): Mapeia o DTO simplificado de volta para as propriedades da Model
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

                
                // --- MAPEAMENTOS DE LIVRO ---

                // Saida Livro (GET)
                CreateMap<Livro, LivroExibicaoDTO>()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdLivro))
                    .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.TituloLivro))
                    .ForMember(dest => dest.Autor, opt => opt.MapFrom(src => src.AutorLivro))
                    .ForMember(dest => dest.Estoque, opt => opt.MapFrom(src => src.QuantidadeEstoqueLivro));

                // Entrada (POST PUT)
                CreateMap<LivroAdicionarDTO, Livro>()
                    .ForMember(dest => dest.TituloLivro, opt => opt.MapFrom(src => src.Titulo))
                    .ForMember(dest => dest.AutorLivro, opt => opt.MapFrom(src => src.Autor))
                    .ForMember(dest => dest.AnoPublicacaoLivro, opt => opt.MapFrom(src => src.Ano))
                    .ForMember(dest => dest.QuantidadeEstoqueLivro, opt => opt.MapFrom(src => src.Estoque));

                CreateMap<LivroAtualizarDTO, Livro>()
                    .ForMember(dest => dest.IdLivro, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dest => dest.TituloLivro, opt => opt.MapFrom(src => src.Titulo))
                    .ForMember(dest => dest.AutorLivro, opt => opt.MapFrom(src => src.Autor))
                    .ForMember(dest => dest.AnoPublicacaoLivro, opt => opt.MapFrom(src => src.Ano))
                    .ForMember(dest => dest.QuantidadeEstoqueLivro, opt => opt.MapFrom(src => src.Estoque));


                // --- MAPEAMENTOS DE EMPRÉSTIMO ---

                //Resolve propriedades de objetos relacionados e calcula lógica de negócio
                    CreateMap<ClienteLivroEmprestimo, EmprestimoExibicaoDTO>()
                    // Acessa o Nome do Cliente e Título do Livro através das propriedades de navegação 
                    .ForMember(dest => dest.NomeCliente, opt => opt.MapFrom(src => src.IdClienteNavigation.NomeCliente))
                    .ForMember(dest => dest.TituloLivro, opt => opt.MapFrom(src => src.IdLivroNavigation.TituloLivro))

                    // Lógica de Cálculo de Multa: R$ 2,00 por dia de atraso, calculada na hora do mapeamento
                    .ForMember(dest => dest.ValorMulta, opt => opt.MapFrom(src =>
                    (src.DataDevolucaoReal ?? DateTime.Now).Date > src.DataDevolucaoPrevista.Date
                    ?(decimal)((src.DataDevolucaoReal ?? DateTime.Now).Date.Subtract(src.DataDevolucaoPrevista.Date).Days * 2.00)
                    : 0m))

                    .ForMember(dest => dest.DiasAtraso, opt => opt.MapFrom(src =>
                    (src.DataDevolucaoReal ?? DateTime.Now).Date > src.DataDevolucaoPrevista.Date
                        ? (src.DataDevolucaoReal ?? DateTime.Now).Date.Subtract(src.DataDevolucaoPrevista.Date).Days
                    : 0))

                    .ForMember(dest => dest.EstaAtrasado, opt => opt.MapFrom(src =>
                    (src.DataDevolucaoReal ?? DateTime.Now).Date > src.DataDevolucaoPrevista.Date));

                // Configuração Inicial de Empréstimo: Define as datas automaticamente no ato da criação
                CreateMap<EmprestimoAdicionarDTO, ClienteLivroEmprestimo>()
                    .ForMember(dest => dest.DataEmprestimo, opt => opt.MapFrom(src => DateTime.Now))
                    .ForMember(dest => dest.DataDevolucaoPrevista, opt => opt.MapFrom(src => DateTime.Now.AddDays(src.DiasEmprestimo)));

                // --- USUARIO ---
                CreateMap<UsuarioRegistroDTO, Usuario>();
                CreateMap<UsuarioAcessoDTO, Usuario>();

                // --- TAREFAS ---

                CreateMap<Tarefa, TarefaExibicaoDTO>();
                CreateMap<TarefaAdicionarDTO, Tarefa>();
                //Ignoro o Id na atualização para evitar conflitos de Tracking do Entity Framework,ele já rastreia o objeto origina
                CreateMap<TarefaAtualizarDTO, Tarefa>()
                    .ForMember(dest => dest.IdTarefa, opt => opt.Ignore());
        }
        }
    }