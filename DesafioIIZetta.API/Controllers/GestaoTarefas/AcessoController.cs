using AutoMapper;
using DesafioIIZetta.API.DTOs.GestaoTarefas.Usuario;
using DesafioIIZetta.API.Excecoes;
using DesafioIIZetta.API.Interfaces;
using DesafioIIZetta.API.Interfaces.GestaoTarefas;
using DesafioIIZetta.API.Models.GestaoTarefas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DesafioIIZetta.API.Controllers.GestaoTarefas{
    [Route("api/[controller]")]
    [ApiController]
    public class AcessoController : ControllerBase{
        private readonly IUsuarioRepository _usuarioRepository; 
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ISenhaService _senhaService;

        public AcessoController(IUsuarioRepository usuarioRepository, IConfiguration configuration, IMapper mapper, ISenhaService senhaService)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _mapper = mapper;
            _senhaService = senhaService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(UsuarioRegistroDTO novoUsuarioDTO){
            // Verifica a unicidade do e-mail para evitar duplicidade de contas no sistema
            await _usuarioRepository.UsuarioJaExisteAsync(novoUsuarioDTO.EmailUsuario);

            var usuario = _mapper.Map<Usuario>(novoUsuarioDTO);
            // Utilizo um serviço de Hash para garantir a integridade e proteção dos dados.
            usuario.SenhaUsuario = _senhaService.GerarHash(novoUsuarioDTO.SenhaUsuario);

            await _usuarioRepository.AdicionarAsync(usuario);
            await _usuarioRepository.SalvarAlteracoesAsync();

            return Ok("Usuário (Comandante da Biblioteca) registrado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioAcessoDTO dadosAcessoDTO)
        {
            var usuario = await _usuarioRepository.BuscarPorEmailAsync(dadosAcessoDTO.EmailUsuario);
            // Verifica se o usuário existe e se o hash da senha coincide.
            if (usuario == null || !_senhaService.VerificarSenha(dadosAcessoDTO.SenhaUsuario, usuario.SenhaUsuario)){
                throw new TarefaException("E-mail ou senha inválidos.");
            }
            // Emite o token de acesso após a autenticação bem-sucedida
            var tokenString = GerarTokenJwt(usuario);

            return Ok(new{token = tokenString,usuario = usuario.NomeUsuario });

        }

        /// <summary>
        /// Gera um token JWT contendo as Claims de identificação do usuário.
        /// </summary>
        private string GerarTokenJwt(Usuario usuario){
            var tokenHandler = new JwtSecurityTokenHandler();
            var chaveString = _configuration["Jwt:ChaveSecreta"];

            if (string.IsNullOrEmpty(chaveString))
                throw new Exception("Chave JWT não configurada no servidor.");

            var chave = Encoding.ASCII.GetBytes(chaveString);
            //Define as informações que serão embutidas no token
            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, usuario.NomeUsuario),
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),// Token expira em 3 horas para segurança
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}