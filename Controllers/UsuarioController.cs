using FCG.DTOs;
using FCG.Helpers;
using FCG.Inputs;
using FCG.Interfaces;
using FCG.Middlewares;
using FCG.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class UsuarioController(IUsuarioRepository usuarioRepository, CriptografiaHelper criptografiaHelper, TextoHelper textoHelper, BaseLogger<UsuarioController> Logger) : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly BaseLogger<UsuarioController> _logger = Logger;
        private readonly CriptografiaHelper _criptografiaHelper = criptografiaHelper;
        private readonly TextoHelper _textoHelper = textoHelper;

        #region [Get]

        [HttpGet("cadastro-todos")]
        [Authorize(Policy = "Admin")]
        public IActionResult Get()
        {
            try
            {
                var _usuariosDto = new List<UsuarioDto>();
                var _usuarios = _usuarioRepository.ObterTodos();

                foreach (var usuario in _usuarios)
                {
                    _usuariosDto.Add(new UsuarioDto()
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Email = usuario.Email,
                        Senha = usuario.Senha,
                        DataNascimento = usuario.DataNascimento,
                        TipoUsuario = usuario.TipoUsuario
                    });
                }
                _logger.LogInfotmation("Todos os usuários exibidos com sucesso.");
                return Ok(_usuariosDto);
            }
            catch (Exception ex)
            {
                var erroResponse = new ErroResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Erro = "Bad Request",
                    Detalhe = ex.Message
                };
                _logger.LogError(erroResponse.ToString());
                return BadRequest(erroResponse);
            }
        }

        [HttpGet("pedidos-todos")]
        [Authorize(Policy = "Admin")]
        public IActionResult ObterPedidosTodos([FromQuery] int id)
        {
            try
            {
                var _usuarios = _usuarioRepository.ObterPorID(id);

                if (_usuarios == null)
                {
                    var erroResponse = new ErroResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Erro = "Not Found",
                        Detalhe = "Usuário não encontrado."
                    };
                    _logger.LogError(erroResponse.ToString());
                    return NotFound(erroResponse);
                }
                _logger.LogInfotmation("Todos os pedidos exibidos.");
                return Ok(_usuarioRepository.ObterPedidosTodos(id));
            }
            catch (Exception ex)
            {
                var erroResponse = new ErroResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Erro = "Bad Request",
                    Detalhe = ex.Message
                };
                _logger.LogError(erroResponse.ToString());
                return BadRequest(erroResponse);
            }
        }

        [HttpGet("pedidos-seis-meses")]
        [Authorize(Policy = "Admin")]
        public IActionResult ObterPedidosSeisMeses([FromQuery] int id)
        {
            try
            {
                var _usuarios = _usuarioRepository.ObterPorID(id);
                if (_usuarios == null)
                {
                    var erroResponse = new ErroResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Erro = "Not Found",
                        Detalhe = "Usuário não encontrado."
                    };
                    _logger.LogError(erroResponse.ToString());
                    return NotFound(erroResponse);
                }
                _logger.LogInfotmation("Pedidos dos últimos seis meses exibidos.");
                return Ok(_usuarioRepository.ObterPedidosSeisMeses(id));
            }
            catch (Exception ex)
            {
                var erroResponse = new ErroResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Erro = "Bad Request",
                    Detalhe = ex.Message
                };
                _logger.LogError(erroResponse.ToString());
                return BadRequest(erroResponse);
            }
        }

        [HttpGet]
        [Authorize(Policy = "Admin")]
        [Route("{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                var _usuarios = _usuarioRepository.ObterPorID(id);

                if (_usuarios == null)
                {
                    var erroResponse = new ErroResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Erro = "Not Found",
                        Detalhe = "Usuário não encontrado."
                    };
                    _logger.LogError(erroResponse.ToString());
                    return NotFound(erroResponse);
                }

                var _usuariosDto = new List<UsuarioDto>();

                _usuariosDto.Add(new UsuarioDto()
                {
                    Id = _usuarios.Id,
                    Nome = _usuarios.Nome,
                    Email = _usuarios.Email,
                    DataNascimento = _usuarios.DataNascimento,
                    TipoUsuario = _usuarios.TipoUsuario
                });
                return Ok(_usuariosDto);

                _logger.LogInfotmation($"Usuário {id} exibido com sucesso.");
                return Ok(_usuarios);
            }
            catch (Exception ex)
            {
                var erroResponse = new ErroResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Erro = "Bad Request",
                    Detalhe = ex.Message
                };
                _logger.LogError(erroResponse.ToString());
                return BadRequest(erroResponse);
            }
        }

        #endregion

        #region [Post]

        [HttpPost()]
        public IActionResult Post([FromBody] UsuarioInput usuarioInput)
        {
            try
            {
                var _usuario = new Usuario()
                {
                    Nome = usuarioInput.Nome,
                    Email = usuarioInput.Email,
                    Senha = _criptografiaHelper.Criptografar(usuarioInput.Senha),
                    DataNascimento = usuarioInput.DataNascimento,
                    Endereco = usuarioInput.Endereco,
                    TipoUsuario = usuarioInput.TipoUsuario
                };

                bool emailValido = _textoHelper.EmailValido(_usuario.Email);
                bool senhaValida = _textoHelper.SenhaValida(_criptografiaHelper.Descriptografar(_usuario.Senha));
                bool tipoUsuarioValido = _textoHelper.TipoUsuarioValido(_usuario.TipoUsuario);

                if (!emailValido)
                {
                    string erroResponse = "Email inválido.";
                    _logger.LogError(erroResponse);
                    return BadRequest(erroResponse);
                }
                else if (!senhaValida)
                {
                    string erroResponse = "Senha inválida.";
                    _logger.LogError(erroResponse);
                    return BadRequest(erroResponse);
                }
                else if (!tipoUsuarioValido)
                {
                    string erroResponse = "Tipo de usuário inválida.";
                    _logger.LogError(erroResponse);
                    return BadRequest(erroResponse);
                }
                else
                {
                    _usuarioRepository.Cadastrar(_usuario);

                    string okResponse = $"Usuário {_usuario.Id} cadastrado com sucesso.";
                    _logger.LogInfotmation(okResponse);
                    return Ok(okResponse);
                }
            }
            catch (Exception ex)
            {
                var erroResponse = new ErroResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Erro = "Bad Request",
                    Detalhe = ex.Message
                };
                _logger.LogError(erroResponse.ToString());
                return BadRequest(erroResponse);
            }
        }

        [HttpPost("cadastro-em-massa")]
        public IActionResult CadastroEmMassa()
        {
            try
            {
                _usuarioRepository.CadastrarEmMassa();
                return Ok();

                string okResponse = $"Usuários cadastrados com sucesso.";
                _logger.LogInfotmation(okResponse);
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                var erroResponse = new ErroResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Erro = "Bad Request",
                    Detalhe = ex.Message
                };
                _logger.LogError(erroResponse.ToString());
                return BadRequest(erroResponse);
            }
        }

        #endregion

        #region [Put Detete]

        [HttpPut]
        [Authorize(Policy = "Admin")]
        public IActionResult Put([FromBody] UsuarioUpdateInput usuarioInput)
        {
            try
            {
                var _usuario = _usuarioRepository.ObterPorID(usuarioInput.Id);
                {
                    _usuario.Nome = usuarioInput.Nome;
                    _usuario.Email = usuarioInput.Email;
                    _usuario.Senha = _criptografiaHelper.Criptografar(usuarioInput.Senha);
                    _usuario.DataNascimento = usuarioInput.DataNascimento;
                    _usuario.TipoUsuario = usuarioInput.TipoUsuario;
                };

                bool emailValido = _textoHelper.EmailValido(_usuario.Email);
                bool senhaValida = _textoHelper.SenhaValida(_criptografiaHelper.Descriptografar(_usuario.Senha));
                bool tipoUsuarioValido = _textoHelper.TipoUsuarioValido(_usuario.TipoUsuario);

                if (!emailValido)
                {
                    string erroResponse = "Email inválido.";
                    _logger.LogError(erroResponse);
                    return BadRequest(erroResponse);
                }
                else if (!senhaValida)
                {
                    string erroResponse = "Senha inválida.";
                    _logger.LogError(erroResponse);
                    return BadRequest(erroResponse);
                }
                else if (!tipoUsuarioValido)
                {
                    string erroResponse = "Tipo de usuário inválida.";
                    _logger.LogError(erroResponse);
                    return BadRequest(erroResponse);
                }
                else
                {
                    _usuarioRepository.Alterar(_usuario);

                    string okResponse = $"Usuário {_usuario.Id} alterado com sucesso.";
                    _logger.LogInfotmation(okResponse);
                    return Ok(okResponse);
                }
            }
            catch (Exception ex)
            {
                var erroResponse = new ErroResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Erro = "Bad Request",
                    Detalhe = ex.Message
                };
                _logger.LogError(erroResponse.ToString());
                return BadRequest(erroResponse);
            }
        }

        [HttpDelete]
        [Authorize(Policy = "Admin")]
        [Route("{id:int}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _usuarioRepository.Deletar(id);

                string okResponse = $"Usuário {id} excluído com sucesso.";
                _logger.LogInfotmation(okResponse);
                return Ok(okResponse);
            }
            catch (Exception ex)
            {
                var erroResponse = new ErroResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Erro = "Bad Request",
                    Detalhe = ex.Message
                };
                _logger.LogError(erroResponse.ToString());
                return BadRequest(erroResponse);
            }
        }

        #endregion
    }
}
