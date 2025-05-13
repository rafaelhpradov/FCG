using FCG.DTOs;
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
    public class PedidoController(IPedidoRepository pedidoRepository, BaseLogger<GameController> Logger) : ControllerBase
    {
        private readonly IPedidoRepository _pedidoRepository = pedidoRepository;
        private readonly BaseLogger<GameController> _logger = Logger;

        #region [Get]

        [HttpGet("todos")]
        [Authorize(Policy = "Admin")]
        public IActionResult Get()
        {
            try
            {
                var _pedidosDto = new List<PedidoDto>();
                var _pedidos = _pedidoRepository.ObterTodos();
                
                foreach (var pedido in _pedidos)
                {
                    _pedidosDto.Add(new PedidoDto()
                    {
                        Id = pedido.Id,
                        DataCriacao = pedido.DataCriacao.ToString("yyyy-MM-dd"),
                        UsuarioId = pedido.UsuarioId,
                        GameId = pedido.GameId,
                    });
                }
                _logger.LogInfotmation("Todos os pedidos exibidos com sucesso.");
                return Ok(_pedidosDto);
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
        [Route("{id:int}")]
        public IActionResult Get([FromRoute] int id)
        {
            try
            {
                var _pedido = _pedidoRepository.ObterPorID(id);
                if (_pedido == null)
                {
                    var erroResponse = new ErroResponse
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Erro = "Not Found",
                        Detalhe = "Game não encontrado."
                    };
                    _logger.LogError(erroResponse.ToString());
                    return NotFound(erroResponse);
                }

                var _pedidoDto = new PedidoDto()
                {
                    Id = _pedido.Id,
                    DataCriacao = _pedido.DataCriacao.ToString("yyyy-MM-dd"),
                    UsuarioId = _pedido.UsuarioId,
                    GameId = _pedido.GameId,
                };

                _logger.LogInfotmation($"Pedido exibido com sucesso.");
                return Ok(_pedidoDto);
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

        [HttpPost]
        public IActionResult Post([FromBody] PedidoInput pedidoInput)
        {
            try
            {
                var _pedido = new Pedido()
                {
                    UsuarioId = pedidoInput.UsuarioId,
                    GameId = pedidoInput.GameId,
                };
                _pedidoRepository.Cadastrar(_pedido);

                string okResponse = $"Pedido {_pedido.Id} cadastrado com sucesso.";
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

        [HttpPost("Cadastro-em-massa")]
        public IActionResult CadastroEmMassa()
        {
            try
            {
                _pedidoRepository.CadastrarEmMassa();
                return Ok();

                string okResponse = $"Games cadastrados com sucesso.";
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

        #region [Put Delete]

        [HttpPut]
        [Route("{Id:int}")]
        public IActionResult Put([FromBody] PedidoInput pedidoInput, [FromRoute] int Id)
        {
            try
            {
                var _pedido = _pedidoRepository.ObterPorID(Id);
                {
                    _pedido.GameId = pedidoInput.GameId;
                };
                _pedidoRepository.Alterar(_pedido);

                string okResponse = $"Pedido {_pedido.Id} alterado com sucesso.";
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

        [HttpDelete]
        [Authorize(Policy = "Admin")]
        [Route("{Id:int}")]
        public IActionResult Delete([FromRoute] int Id)
        {
            try
            {
                _pedidoRepository.Deletar(Id);

                string okResponse = $"Pedido {Id} deletado com sucesso.";
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
