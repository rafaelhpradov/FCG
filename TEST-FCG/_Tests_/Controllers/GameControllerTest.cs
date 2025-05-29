using Moq;
using FCG.Controllers;
using FCG.Interfaces;
using FCG.Models;
using FCG.DTOs;
using FCG.Inputs;
using FCG.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Z.BulkOperations;

namespace TEST_FCG._Tests_.Controllers
{
    [TestClass]
    public class GameControllerTests
    {
        private Mock<IGameRepository> _gameRepoMock;
        private Mock<BaseLogger<GameController>> _loggerMock;
        private Mock<BaseError> _errorMock;
        private GameController _controller;

        [TestInitialize]
        public void Setup()
        {
            _gameRepoMock = new Mock<IGameRepository>();
            _loggerMock = new Mock<BaseLogger<GameController>>(MockBehavior.Loose, null as Microsoft.Extensions.Logging.ILogger<GameController>);
            _errorMock = new Mock<BaseError>();
            _controller = new GameController(_gameRepoMock.Object, _loggerMock.Object, _errorMock.Object);
        }

        [TestMethod]
        public void ReturnsOkWhenGettingAllGames()
        {
            var games = new List<Game>
        {
            new Game { Id = 1, Nome = "Game1", Produtora = "Prod1", Descricao = "Desc1", DataLancamento = DateTime.Today, Preco = 10 }
        };
            _gameRepoMock.Setup(r => r.ObterTodos()).Returns(games);

            var result = _controller.get();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<GameDto>));
            var value = okResult.Value as List<GameDto>;
            Assert.AreEqual(1, value.Count);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingAllGames()
        {
            _gameRepoMock.Setup(r => r.ObterTodos()).Throws(new Exception("fail"));
            var result = _controller.get();
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenGameFoundByName()
        {
            var game = new Game { Id = 2, Nome = "Game2", Produtora = "Prod2", Descricao = "Desc2", DataLancamento = DateTime.Today, Preco = 20 };
            _gameRepoMock.Setup(r => r.ObterPorNome("Game2")).Returns(game);

            var result = _controller.Get("Game2");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(GameDto));
            var dto = okResult.Value as GameDto;
            Assert.AreEqual("Game2", dto.Nome);
        }

        [TestMethod]
        public void ReturnsNotFoundWhenGameNotFoundByName()
        {
            _gameRepoMock.Setup(r => r.ObterPorNome("NotFound")).Returns((Game)null);

            var result = _controller.Get("NotFound");

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.IsInstanceOfType(notFound.Value, typeof(ErroResponse));
            var error = notFound.Value as ErroResponse;
            Assert.AreEqual("Not Found", error.Erro);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingGameByName()
        {
            _gameRepoMock.Setup(r => r.ObterPorNome(It.IsAny<string>())).Throws(new Exception("fail"));

            var result = _controller.Get("any");

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenGameFoundById()
        {
            var game = new Game { Id = 3, Nome = "Game3", Produtora = "Prod3", Descricao = "Desc3", DataLancamento = DateTime.Today, Preco = 30 };
            _gameRepoMock.Setup(r => r.ObterPorID(3)).Returns(game);

            var result = _controller.Get(3);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(GameDto));
            var dto = okResult.Value as GameDto;
            Assert.AreEqual(3, dto.Id);
        }

        [TestMethod]
        public void ReturnsNotFoundWhenGameNotFoundById()
        {
            _gameRepoMock.Setup(r => r.ObterPorID(99)).Returns((Game)null);

            var result = _controller.Get(99);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.IsInstanceOfType(notFound.Value, typeof(ErroResponse));
            var error = notFound.Value as ErroResponse;
            Assert.AreEqual("Not Found", error.Erro);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingGameById()
        {
            _gameRepoMock.Setup(r => r.ObterPorID(It.IsAny<int>())).Throws(new Exception("fail"));

            var result = _controller.Get(1);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenPostingNewGame()
        {
            var input = new GameInput
            {
                Nome = "Game4",
                Produtora = "Prod4",
                Descricao = "Desc4",
                DataLancamento = DateTime.Today,
                Preco = 40
            };

            _gameRepoMock.Setup(r => r.Cadastrar(It.IsAny<Game>()));

            var result = _controller.Post(input);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("cadastrado com sucesso"));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenPostingNewGame()
        {
            _gameRepoMock.Setup(r => r.Cadastrar(It.IsAny<Game>())).Throws(new Exception("fail"));

            var input = new GameInput
            {
                Nome = "Game4",
                Produtora = "Prod4",
                Descricao = "Desc4",
                DataLancamento = DateTime.Today,
                Preco = 40
            };

            var result = _controller.Post(input);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenBulkRegisteringGames()
        {
            _gameRepoMock.Setup(r => r.CadastrarEmMassa());

            var result = _controller.CadastroEmMassa();

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenBulkRegisteringGames()
        {
            _gameRepoMock.Setup(r => r.CadastrarEmMassa()).Throws(new Exception("fail"));

            var result = _controller.CadastroEmMassa();

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenUpdatingGame()
        {
            var updateInput = new GameUpdateInput
            {
                Id = 5,
                Nome = "Game5",
                Produtora = "Prod5",
                Descricao = "Desc5",
                DataLancamento = DateTime.Today,
                Preco = 50
            };

            var game = new Game
            {
                Id = 5,
                Nome = "OldName",
                Produtora = "OldProd",
                Descricao = "OldDesc",
                DataLancamento = DateTime.Today.AddDays(-1),
                Preco = 10
            };

            _gameRepoMock.Setup(r => r.ObterPorID(5)).Returns(game);
            _gameRepoMock.Setup(r => r.Alterar(It.IsAny<Game>()));

            var result = _controller.Put(updateInput);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("alterado com sucesso"));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenUpdatingGame()
        {
            _gameRepoMock.Setup(r => r.ObterPorID(It.IsAny<int>())).Throws(new Exception("fail"));

            var updateInput = new GameUpdateInput
            {
                Id = 5,
                Nome = "Game5",
                Produtora = "Prod5",
                Descricao = "Desc5",
                DataLancamento = DateTime.Today,
                Preco = 50
            };

            var result = _controller.Put(updateInput);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenDeletingGame()
        {
            _gameRepoMock.Setup(r => r.Deletar(6));

            var result = _controller.Delete(6);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("deletado com sucesso"));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenDeletingGame()
        {
            _gameRepoMock.Setup(r => r.Deletar(It.IsAny<int>())).Throws(new Exception("fail"));

            var result = _controller.Delete(6);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }
    }
}
