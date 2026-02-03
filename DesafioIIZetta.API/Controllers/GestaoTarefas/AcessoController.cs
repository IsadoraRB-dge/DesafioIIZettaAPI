using AutoMapper;
using DesafioIIZetta.API.DTOs.GestaoTarefas.Usuario;
using DesafioIIZetta.API.Interfaces; 
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

        public AcessoController(IUsuarioRepository usuarioRepository, IConfiguration configuration, IMapper mapper){
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> Registrar(UsuarioRegistroDTO novoUsuarioDTO){
           
            if (await _usuarioRepository.UsuarioJaExisteAsync(novoUsuarioDTO.EmailUsuario)){
                return BadRequest("Usuário já existe!");
            }

            var usuario = _mapper.Map<Usuario>(novoUsuarioDTO);

            await _usuarioRepository.AdicionarAsync(usuario);
            await _usuarioRepository.SalvarAlteracoesAsync();

            return Ok("Usuário (Comandante da Biblioteca) registrado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UsuarioAcessoDTO dadosAcessoDTO){
            var usuario = await _usuarioRepository.BuscarPorEmailESenhaAsync(dadosAcessoDTO.EmailUsuario, dadosAcessoDTO.SenhaUsuario);

            if (usuario == null){
                return Unauthorized("E-mail ou senha incorretos.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var chaveString = _configuration["Jwt:ChaveSecreta"];
            var chave = Encoding.ASCII.GetBytes(chaveString);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, usuario.NomeUsuario),
                    new Claim(ClaimTypes.NameIdentifier, usuario.IdUsuario.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(3),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(chave), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                usuario = usuario.NomeUsuario
            });
        }
    }
}