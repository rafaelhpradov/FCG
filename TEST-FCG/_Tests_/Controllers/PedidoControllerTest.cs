using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FCG.Controllers;
using FCG.Interfaces;
using FCG.Models;
using FCG.DTOs;
using FCG.Inputs;
using FCG.Middlewares;
using Microsoft.AspNetCore.Mvc;

namespace TEST_FCG._Tests_.Controllers
{
    [TestClass]
    public class PedidoControllerTest
    {
        private Mock<IPedidoRepository> _pedidoRepoMock;
        private Mock<BaseLogger<GameController>> _loggerMock;
        private PedidoController _controller;

        [TestInitialize]
        public void Setup()
        {
            _pedidoRepoMock = new Mock<IPedidoRepository>();
            _loggerMock = new Mock<BaseLogger<GameController>>(MockBehavior.Loose, null as Microsoft.Extensions.Logging.ILogger<GameController>);
            _controller = new PedidoController(_pedidoRepoMock.Object, _loggerMock.Object);
        }

        [TestMethod]
        public void ReturnsOkWhenGettingAllPedidos()
        {
            var pedidos = new List<Pedido>
            {
                new Pedido { Id = 1, UsuarioId = 10, GameId = 20, DataCriacao = DateTime.Today }
            };
            _pedidoRepoMock.Setup(r => r.ObterTodos()).Returns(pedidos);

            var result = _controller.Get();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<PedidoDto>));
            var value = okResult.Value as List<PedidoDto>;
            Assert.AreEqual(1, value.Count);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingAllPedidos()
        {
            _pedidoRepoMock.Setup(r => r.ObterTodos()).Throws(new Exception("fail"));
            var result = _controller.Get();
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenPedidoFoundById()
        {
            var pedido = new Pedido { Id = 2, UsuarioId = 11, GameId = 21, DataCriacao = DateTime.Today };
            _pedidoRepoMock.Setup(r => r.ObterPorID(2)).Returns(pedido);

            var result = _controller.Get(2);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(PedidoDto));
            var dto = okResult.Value as PedidoDto;
            Assert.AreEqual(2, dto.Id);
        }

        [TestMethod]
        public void ReturnsNotFoundWhenPedidoNotFoundById()
        {
            _pedidoRepoMock.Setup(r => r.ObterPorID(99)).Returns((Pedido)null);

            var result = _controller.Get(99);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.IsInstanceOfType(notFound.Value, typeof(ErroResponse));
            var error = notFound.Value as ErroResponse;
            Assert.AreEqual("Not Found", error.Erro);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingPedidoById()
        {
            _pedidoRepoMock.Setup(r => r.ObterPorID(It.IsAny<int>())).Throws(new Exception("fail"));

            var result = _controller.Get(1);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenPostingNewPedido()
        {
            var input = new PedidoInput
            {
                UsuarioId = 12,
                GameId = 22
            };

            _pedidoRepoMock.Setup(r => r.Cadastrar(It.IsAny<Pedido>()));

            var result = _controller.Post(input);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("cadastrado com sucesso"));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenPostingNewPedido()
        {
            _pedidoRepoMock.Setup(r => r.Cadastrar(It.IsAny<Pedido>())).Throws(new Exception("fail"));

            var input = new PedidoInput
            {
                UsuarioId = 13,
                GameId = 23
            };

            var result = _controller.Post(input);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenBulkRegisteringPedidos()
        {
            _pedidoRepoMock.Setup(r => r.CadastrarEmMassa());

            var result = _controller.CadastroEmMassa();

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenBulkRegisteringPedidos()
        {
            _pedidoRepoMock.Setup(r => r.CadastrarEmMassa()).Throws(new Exception("fail"));

            var result = _controller.CadastroEmMassa();

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenUpdatingPedido()
        {
            var updateInput = new PedidoUpdateInput
            {
                Id = 14,
                GameId = 24
            };

            var pedido = new Pedido
            {
                Id = 14,
                UsuarioId = 15,
                GameId = 99,
                DataCriacao = DateTime.Today
            };

            _pedidoRepoMock.Setup(r => r.ObterPorID(14)).Returns(pedido);
            _pedidoRepoMock.Setup(r => r.Alterar(It.IsAny<Pedido>()));

            var result = _controller.Put(updateInput);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("alterado com sucesso"));
        }

        [TestMethod]
        public void ReturnsNotFoundWhenUpdatingNonexistentPedido()
        {
            _pedidoRepoMock.Setup(r => r.ObterPorID(99)).Returns((Pedido)null);

            var updateInput = new PedidoUpdateInput
            {
                Id = 99,
                GameId = 25
            };

            var result = _controller.Put(updateInput);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.IsInstanceOfType(notFound.Value, typeof(ErroResponse));
            var error = notFound.Value as ErroResponse;
            Assert.AreEqual("Not Found", error.Erro);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenUpdatingPedido()
        {
            _pedidoRepoMock.Setup(r => r.ObterPorID(It.IsAny<int>())).Throws(new Exception("fail"));

            var updateInput = new PedidoUpdateInput
            {
                Id = 15,
                GameId = 26
            };

            var result = _controller.Put(updateInput);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenDeletingPedido()
        {
            _pedidoRepoMock.Setup(r => r.Deletar(16));

            var result = _controller.Delete(16);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("deletado com sucesso"));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenDeletingPedido()
        {
            _pedidoRepoMock.Setup(r => r.Deletar(It.IsAny<int>())).Throws(new Exception("fail"));

            var result = _controller.Delete(17);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }
    }
}
