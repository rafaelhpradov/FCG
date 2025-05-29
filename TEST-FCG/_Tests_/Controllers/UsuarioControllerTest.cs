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
using FCG.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace TEST_FCG._Tests_.Controllers
{
    [TestClass]
    public class UsuarioControllerTest
    {
        private Mock<IUsuarioRepository> _usuarioRepoMock;
        private Mock<CriptografiaHelper> _criptografiaHelperMock;
        private Mock<TextoHelper> _textoHelperMock;
        private Mock<BaseLogger<UsuarioController>> _loggerMock;
        private UsuarioController _controller;

        [TestInitialize]
        public void Setup()
        {
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _criptografiaHelperMock = new Mock<CriptografiaHelper>();
            _textoHelperMock = new Mock<TextoHelper>();
            _loggerMock = new Mock<BaseLogger<UsuarioController>>(MockBehavior.Loose, null as Microsoft.Extensions.Logging.ILogger<UsuarioController>);
            _controller = new UsuarioController(
                _usuarioRepoMock.Object,
                _criptografiaHelperMock.Object,
                _textoHelperMock.Object,
                _loggerMock.Object
            );
        }

        [TestMethod]
        public void ReturnsOkWhenGettingAllUsuarios()
        {
            var usuarios = new List<Usuario>
            {
                new Usuario { Id = 1, Nome = "User1", Email = "user1@email.com", Senha = "senha", DataNascimento = DateTime.Today, TipoUsuario = 1 }
            };
            _usuarioRepoMock.Setup(r => r.ObterTodos()).Returns(usuarios);

            var result = _controller.Get();

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(List<UsuarioDto>));
            var value = okResult.Value as List<UsuarioDto>;
            Assert.AreEqual(1, value.Count);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingAllUsuarios()
        {
            _usuarioRepoMock.Setup(r => r.ObterTodos()).Throws(new Exception("fail"));
            var result = _controller.Get();
            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenGettingPedidosTodos()
        {
            var usuario = new Usuario { Id = 2, Nome = "User2", Email = "user2@email.com", Senha = "senha", DataNascimento = DateTime.Today, TipoUsuario = 2 };
            var usuarioDto = new UsuarioDto { Id = 2, Nome = "User2", Email = "user2@email.com", DataNascimento = DateTime.Today.ToString("yyyy-MM-dd"), TipoUsuario = 2 };
            _usuarioRepoMock.Setup(r => r.ObterPorID(2)).Returns(usuario);
            _usuarioRepoMock.Setup(r => r.ObterPedidosTodos(2)).Returns(usuarioDto);

            var result = _controller.ObterPedidosTodos(2);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(usuarioDto, okResult.Value);
        }

        [TestMethod]
        public void ReturnsNotFoundWhenGettingPedidosTodosForNonexistentUser()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorID(99)).Returns((Usuario)null);

            var result = _controller.ObterPedidosTodos(99);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.IsInstanceOfType(notFound.Value, typeof(ErroResponse));
            var error = notFound.Value as ErroResponse;
            Assert.AreEqual("Not Found", error.Erro);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingPedidosTodos()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorID(It.IsAny<int>())).Throws(new Exception("fail"));

            var result = _controller.ObterPedidosTodos(1);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenGettingPedidosSeisMeses()
        {
            var usuario = new Usuario { Id = 3, Nome = "User3", Email = "user3@email.com", Senha = "senha", DataNascimento = DateTime.Today, TipoUsuario = 2 };
            var usuarioDto = new UsuarioDto { Id = 3, Nome = "User3", Email = "user3@email.com", DataNascimento = DateTime.Today.ToString("yyyy-MM-dd"), TipoUsuario = 2 };
            _usuarioRepoMock.Setup(r => r.ObterPorID(3)).Returns(usuario);
            _usuarioRepoMock.Setup(r => r.ObterPedidosSeisMeses(3)).Returns(usuarioDto);

            var result = _controller.ObterPedidosSeisMeses(3);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(usuarioDto, okResult.Value);
        }

        [TestMethod]
        public void ReturnsNotFoundWhenGettingPedidosSeisMesesForNonexistentUser()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorID(99)).Returns((Usuario)null);

            var result = _controller.ObterPedidosSeisMeses(99);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.IsInstanceOfType(notFound.Value, typeof(ErroResponse));
            var error = notFound.Value as ErroResponse;
            Assert.AreEqual("Not Found", error.Erro);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingPedidosSeisMeses()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorID(It.IsAny<int>())).Throws(new Exception("fail"));

            var result = _controller.ObterPedidosSeisMeses(1);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenUsuarioFoundById()
        {
            var usuario = new Usuario { Id = 4, Nome = "User4", Email = "user4@email.com", Senha = "senha", DataNascimento = DateTime.Today, TipoUsuario = 1 };
            _usuarioRepoMock.Setup(r => r.ObterPorID(4)).Returns(usuario);

            var result = _controller.Get(4);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(UsuarioDto));
            var dto = okResult.Value as UsuarioDto;
            Assert.AreEqual(4, dto.Id);
        }

        [TestMethod]
        public void ReturnsNotFoundWhenUsuarioNotFoundById()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorID(99)).Returns((Usuario)null);

            var result = _controller.Get(99);

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.IsInstanceOfType(notFound.Value, typeof(ErroResponse));
            var error = notFound.Value as ErroResponse;
            Assert.AreEqual("Not Found", error.Erro);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingUsuarioById()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorID(It.IsAny<int>())).Throws(new Exception("fail"));

            var result = _controller.Get(1);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenUsuarioFoundByName()
        {
            var usuario = new Usuario { Id = 5, Nome = "User5", Email = "user5@email.com", Senha = "senha", DataNascimento = DateTime.Today, TipoUsuario = 2 };
            _usuarioRepoMock.Setup(r => r.ObterPorNome("User5")).Returns(usuario);

            var result = _controller.Get("User5");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsInstanceOfType(okResult.Value, typeof(UsuarioDto));
            var dto = okResult.Value as UsuarioDto;
            Assert.AreEqual("User5", dto.Nome);
        }

        [TestMethod]
        public void ReturnsNotFoundWhenUsuarioNotFoundByName()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome("NotFound")).Returns((Usuario)null);

            var result = _controller.Get("NotFound");

            var notFound = result as NotFoundObjectResult;
            Assert.IsNotNull(notFound);
            Assert.IsInstanceOfType(notFound.Value, typeof(ErroResponse));
            var error = notFound.Value as ErroResponse;
            Assert.AreEqual("Not Found", error.Erro);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenGettingUsuarioByName()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome(It.IsAny<string>())).Throws(new Exception("fail"));

            var result = _controller.Get("any");

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsConflictIfUsuarioAlreadyExistsByName()
        {
            var usuario = new Usuario { Id = 6, Nome = "User6", Email = "user6@email.com", Senha = "senha", DataNascimento = DateTime.Today, TipoUsuario = 1 };
            _usuarioRepoMock.Setup(r => r.ObterPorNome("User6")).Returns(usuario);

            var input = new UsuarioInput
            {
                Nome = "User6",
                Email = "user6@email.com",
                Senha = "Senha@123",
                DataNascimento = DateTime.Today,
                Endereco = "Rua 1",
                TipoUsuario = 1
            };

            var result = _controller.Post(input);

            var conflict = result as ConflictObjectResult;
            Assert.IsNotNull(conflict);
            Assert.IsInstanceOfType(conflict.Value, typeof(ErroResponse));
            var error = conflict.Value as ErroResponse;
            Assert.AreEqual("Usuário já cadastrado.", error.Erro);
        }

        [TestMethod]
        public void ReturnsConflictIfUsuarioAlreadyExistsByEmail()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome(It.IsAny<string>())).Returns((Usuario)null);
            var usuario = new Usuario { Id = 7, Nome = "User7", Email = "user7@email.com", Senha = "senha", DataNascimento = DateTime.Today, TipoUsuario = 1 };
            _usuarioRepoMock.Setup(r => r.ObterPorEmail("user7@email.com")).Returns(usuario);

            var input = new UsuarioInput
            {
                Nome = "User7",
                Email = "user7@email.com",
                Senha = "Senha@123",
                DataNascimento = DateTime.Today,
                Endereco = "Rua 2",
                TipoUsuario = 1
            };

            var result = _controller.Post(input);

            var conflict = result as ConflictObjectResult;
            Assert.IsNotNull(conflict);
            Assert.IsInstanceOfType(conflict.Value, typeof(ErroResponse));
            var error = conflict.Value as ErroResponse;
            Assert.AreEqual("Email já cadastrado.", error.Erro);
        }

        [TestMethod]
        public void ReturnsBadRequestIfEmailInvalidOnPost()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome(It.IsAny<string>())).Returns((Usuario)null);
            _usuarioRepoMock.Setup(r => r.ObterPorEmail(It.IsAny<string>())).Returns((Usuario)null);
            _criptografiaHelperMock.Setup(c => c.Criptografar(It.IsAny<string>())).Returns("senhaCripto");
            _criptografiaHelperMock.Setup(c => c.Descriptografar(It.IsAny<string>())).Returns("Senha@123");
            _textoHelperMock.Setup(t => t.EmailValido(It.IsAny<string>())).Returns(false);

            var input = new UsuarioInput
            {
                Nome = "User8",
                Email = "invalidemail",
                Senha = "Senha@123",
                DataNascimento = DateTime.Today,
                Endereco = "Rua 3",
                TipoUsuario = 1
            };

            var result = _controller.Post(input);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("Email inválido.", badRequest.Value);
        }

        [TestMethod]
        public void ReturnsBadRequestIfSenhaInvalidOnPost()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome(It.IsAny<string>())).Returns((Usuario)null);
            _usuarioRepoMock.Setup(r => r.ObterPorEmail(It.IsAny<string>())).Returns((Usuario)null);
            _criptografiaHelperMock.Setup(c => c.Criptografar(It.IsAny<string>())).Returns("senhaCripto");
            _criptografiaHelperMock.Setup(c => c.Descriptografar(It.IsAny<string>())).Returns("Senha@123");
            _textoHelperMock.Setup(t => t.EmailValido(It.IsAny<string>())).Returns(true);
            _textoHelperMock.Setup(t => t.SenhaValida(It.IsAny<string>())).Returns(false);

            var input = new UsuarioInput
            {
                Nome = "User9",
                Email = "user9@email.com",
                Senha = "Senha@123",
                DataNascimento = DateTime.Today,
                Endereco = "Rua 4",
                TipoUsuario = 1
            };

            var result = _controller.Post(input);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("Senha inválida.", badRequest.Value);
        }

        [TestMethod]
        public void ReturnsBadRequestIfTipoUsuarioInvalidOnPost()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome(It.IsAny<string>())).Returns((Usuario)null);
            _usuarioRepoMock.Setup(r => r.ObterPorEmail(It.IsAny<string>())).Returns((Usuario)null);
            _criptografiaHelperMock.Setup(c => c.Criptografar(It.IsAny<string>())).Returns("senhaCripto");
            _criptografiaHelperMock.Setup(c => c.Descriptografar(It.IsAny<string>())).Returns("Senha@123");
            _textoHelperMock.Setup(t => t.EmailValido(It.IsAny<string>())).Returns(true);
            _textoHelperMock.Setup(t => t.SenhaValida(It.IsAny<string>())).Returns(true);
            _textoHelperMock.Setup(t => t.TipoUsuarioValido(It.IsAny<short>())).Returns(false);

            var input = new UsuarioInput
            {
                Nome = "User10",
                Email = "user10@email.com",
                Senha = "Senha@123",
                DataNascimento = DateTime.Today,
                Endereco = "Rua 5",
                TipoUsuario = 99
            };

            var result = _controller.Post(input);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("Tipo de usuário inválida.", badRequest.Value);
        }

        [TestMethod]
        public void ReturnsOkWhenPostingNewUsuario()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome(It.IsAny<string>())).Returns((Usuario)null);
            _usuarioRepoMock.Setup(r => r.ObterPorEmail(It.IsAny<string>())).Returns((Usuario)null);
            _criptografiaHelperMock.Setup(c => c.Criptografar(It.IsAny<string>())).Returns("senhaCripto");
            _criptografiaHelperMock.Setup(c => c.Descriptografar(It.IsAny<string>())).Returns("Senha@123");
            _textoHelperMock.Setup(t => t.EmailValido(It.IsAny<string>())).Returns(true);
            _textoHelperMock.Setup(t => t.SenhaValida(It.IsAny<string>())).Returns(true);
            _textoHelperMock.Setup(t => t.TipoUsuarioValido(It.IsAny<short>())).Returns(true);

            var input = new UsuarioInput
            {
                Nome = "User11",
                Email = "user11@email.com",
                Senha = "Senha@123",
                DataNascimento = DateTime.Today,
                Endereco = "Rua 6",
                TipoUsuario = 1
            };

            _usuarioRepoMock.Setup(r => r.Cadastrar(It.IsAny<Usuario>()));

            var result = _controller.Post(input);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("cadastrado com sucesso"));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenPostingNewUsuario()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome(It.IsAny<string>())).Throws(new Exception("fail"));

            var input = new UsuarioInput
            {
                Nome = "User12",
                Email = "user12@email.com",
                Senha = "Senha@123",
                DataNascimento = DateTime.Today,
                Endereco = "Rua 7",
                TipoUsuario = 1
            };

            var result = _controller.Post(input);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenBulkRegisteringUsuarios()
        {
            _usuarioRepoMock.Setup(r => r.CadastrarEmMassa());

            var result = _controller.CadastroEmMassa();

            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenBulkRegisteringUsuarios()
        {
            _usuarioRepoMock.Setup(r => r.CadastrarEmMassa()).Throws(new Exception("fail"));

            var result = _controller.CadastroEmMassa();

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenUpdatingUsuario()
        {
            var updateInput = new UsuarioUpdateInput
            {
                Id = 13,
                Nome = "User13",
                Email = "user13@email.com",
                Senha = "Senha@123",
                DataNascimento = DateTime.Today,
                Endereco = "Rua 8",
                TipoUsuario = 1
            };

            var usuario = new Usuario
            {
                Id = 13,
                Nome = "OldName",
                Email = "old@email.com",
                Senha = "oldsenha",
                DataNascimento = DateTime.Today.AddDays(-1),
                Endereco = "OldEndereco",
                TipoUsuario = 2
            };

            _usuarioRepoMock.Setup(r => r.ObterPorID(13)).Returns(usuario);
            _criptografiaHelperMock.Setup(c => c.Criptografar(It.IsAny<string>())).Returns("senhaCripto");
            _criptografiaHelperMock.Setup(c => c.Descriptografar(It.IsAny<string>())).Returns("Senha@123");
            _textoHelperMock.Setup(t => t.EmailValido(It.IsAny<string>())).Returns(true);
            _textoHelperMock.Setup(t => t.SenhaValida(It.IsAny<string>())).Returns(true);
            _textoHelperMock.Setup(t => t.TipoUsuarioValido(It.IsAny<short>())).Returns(true);
            _usuarioRepoMock.Setup(r => r.Alterar(It.IsAny<Usuario>()));

            var result = _controller.Put(updateInput);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("alterado com sucesso"));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenUpdatingUsuario()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorID(It.IsAny<int>())).Throws(new Exception("fail"));

            var updateInput = new UsuarioUpdateInput
            {
                Id = 14,
                Nome = "User14",
                Email = "user14@email.com",
                Senha = "Senha@123",
                DataNascimento = DateTime.Today,
                Endereco = "Rua 9",
                TipoUsuario = 1
            };

            var result = _controller.Put(updateInput);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }

        [TestMethod]
        public void ReturnsOkWhenDeletingUsuario()
        {
            _usuarioRepoMock.Setup(r => r.Deletar(15));

            var result = _controller.Delete(15);

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("excluído com sucesso"));
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenDeletingUsuario()
        {
            _usuarioRepoMock.Setup(r => r.Deletar(It.IsAny<int>())).Throws(new Exception("fail"));

            var result = _controller.Delete(16);

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.IsInstanceOfType(badRequest.Value, typeof(ErroResponse));
            var error = badRequest.Value as ErroResponse;
            Assert.AreEqual("Bad Request", error.Erro);
        }
    }
}
