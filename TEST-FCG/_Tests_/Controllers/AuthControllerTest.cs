using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using FCG.Controllers;
using FCG.Interfaces;
using FCG.Models;
using FCG.Middlewares;
using FCG.Helpers;

namespace TEST_FCG._Tests_.Controllers
{
    [TestClass]
    public class AuthControllerTest
    {
        private Mock<IConfiguration> _configMock;
        private Mock<IUsuarioRepository> _usuarioRepoMock;
        private Mock<CriptografiaHelper> _criptografiaHelperMock;
        private Mock<LoginHelper> _loginHelperMock;
        private Mock<BaseLogger<AuthController>> _loggerMock;
        private AuthController _controller;

        [TestInitialize]
        public void Setup()
        {
            _configMock = new Mock<IConfiguration>();
            _usuarioRepoMock = new Mock<IUsuarioRepository>();
            _criptografiaHelperMock = new Mock<CriptografiaHelper>();
            _loginHelperMock = new Mock<LoginHelper>();
            _loggerMock = new Mock<BaseLogger<AuthController>>(MockBehavior.Loose, null as Microsoft.Extensions.Logging.ILogger<AuthController>);

            _configMock.Setup(c => c["Jwt:ExpirationMinutes"]).Returns("60");
            _configMock.Setup(c => c["Jwt:Key"]).Returns("supersecretkey12345678901234567890");
            _configMock.Setup(c => c["Jwt:Issuer"]).Returns("issuer");
            _configMock.Setup(c => c["Jwt:Audience"]).Returns("audience");

            _controller = new AuthController(
                _configMock.Object,
                _usuarioRepoMock.Object,
                _criptografiaHelperMock.Object,
                _loginHelperMock.Object,
                _loggerMock.Object
            );
        }

        [TestMethod]
        public void ReturnsOkWhenLoginIsSuccessful()
        {
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "user1",
                Email = "user1@email.com",
                Senha = "encrypted",
                DataNascimento = DateTime.Today.AddYears(-20),
                TipoUsuario = 1
            };

            _usuarioRepoMock.Setup(r => r.ObterPorNome("user1")).Returns(usuario);
            _criptografiaHelperMock.Setup(h => h.Descriptografar("encrypted")).Returns("password");
            _loginHelperMock.Setup(h => h.VerificarCadastroEmail("user1@email.com", "user1@email.com")).Returns(true);
            _loginHelperMock.Setup(h => h.VerificarCadastroSenha("password", "password")).Returns(true);

            var result = _controller.Logar("user1", "user1@email.com", "password");

            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.IsTrue(okResult.Value.ToString().Contains("token"));
            Assert.IsTrue(okResult.Value.ToString().Contains("Bearer"));
        }

        [TestMethod]
        public void ReturnsBadRequestWhenEmailIsIncorrect()
        {
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "user1",
                Email = "user1@email.com",
                Senha = "encrypted",
                DataNascimento = DateTime.Today.AddYears(-20),
                TipoUsuario = 1
            };

            _usuarioRepoMock.Setup(r => r.ObterPorNome("user1")).Returns(usuario);
            _criptografiaHelperMock.Setup(h => h.Descriptografar("encrypted")).Returns("password");
            _loginHelperMock.Setup(h => h.VerificarCadastroEmail("user1@email.com", "wrong@email.com")).Returns(false);

            var result = _controller.Logar("user1", "wrong@email.com", "password");

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("Email incorreto.", badRequest.Value);
        }

        [TestMethod]
        public void ReturnsBadRequestWhenPasswordIsIncorrect()
        {
            var usuario = new Usuario
            {
                Id = 1,
                Nome = "user1",
                Email = "user1@email.com",
                Senha = "encrypted",
                DataNascimento = DateTime.Today.AddYears(-20),
                TipoUsuario = 1
            };

            _usuarioRepoMock.Setup(r => r.ObterPorNome("user1")).Returns(usuario);
            _criptografiaHelperMock.Setup(h => h.Descriptografar("encrypted")).Returns("password");
            _loginHelperMock.Setup(h => h.VerificarCadastroEmail("user1@email.com", "user1@email.com")).Returns(true);
            _loginHelperMock.Setup(h => h.VerificarCadastroSenha("password", "wrongpass")).Returns(false);

            var result = _controller.Logar("user1", "user1@email.com", "wrongpass");

            var badRequest = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequest);
            Assert.AreEqual("Senha incorreta.", badRequest.Value);
        }

        [TestMethod]
        public void ReturnsUnauthorizedWhenUserNotFound()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome("notfound")).Returns((Usuario)null);

            var result = _controller.Logar("notfound", "any@email.com", "any");

            var unauthorized = result as UnauthorizedObjectResult;
            Assert.IsNotNull(unauthorized);
            Assert.IsInstanceOfType(unauthorized.Value, typeof(ErroResponse));
            var error = unauthorized.Value as ErroResponse;
            Assert.AreEqual("Unauthorized", error.Erro);
            Assert.AreEqual("Usuário ou senha inválidos.", error.Detalhe);
        }

        [TestMethod]
        public void ReturnsBadRequestIfExceptionWhenLogin()
        {
            _usuarioRepoMock.Setup(r => r.ObterPorNome(It.IsAny<string>())).Throws(new Exception("fail"));

            try
            {
                var result = _controller.Logar("user", "email", "senha");
                // If no exception, fail the test
                Assert.Fail("Expected exception was not thrown.");
            }
            catch (Exception ex)
            {
                Assert.AreEqual("fail", ex.Message);
            }
        }
    }
}
