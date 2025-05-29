using System;
using FCG.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TEST_FCG._Tests_.Models
{
    [TestClass]
    public class PedidoTests
    {
        [TestMethod]
        public void CanSetAndGet_UsuarioId()
        {
            var pedido = new Pedido { UsuarioId = 10, GameId = 20 };
            pedido.UsuarioId = 99;
            Assert.AreEqual(99, pedido.UsuarioId);
        }

        [TestMethod]
        public void CanSetAndGet_GameId()
        {
            var pedido = new Pedido { UsuarioId = 10, GameId = 20 };
            pedido.GameId = 77;
            Assert.AreEqual(77, pedido.GameId);
        }

        [TestMethod]
        public void CanSetAndGet_Usuario()
        {
            var usuario = new Usuario
            {
                Nome = "User1",
                Email = "user1@email.com",
                DataNascimento = new DateTime(1990, 1, 1),
                TipoUsuario = 1
            };
            var pedido = new Pedido { UsuarioId = 1, GameId = 2 };
            pedido.Usuario = usuario;
            Assert.AreEqual("User1", pedido.Usuario.Nome);
            Assert.AreEqual("user1@email.com", pedido.Usuario.Email);
        }

        [TestMethod]
        public void CanSetAndGet_Game()
        {
            var game = new Game
            {
                Nome = "GameX",
                Produtora = "StudioX",
                Descricao = "Desc",
                Preco = 10,
                DataLancamento = DateTime.Today
            };
            var pedido = new Pedido { UsuarioId = 1, GameId = 2 };
            pedido.Game = game;
            Assert.AreEqual("GameX", pedido.Game.Nome);
            Assert.AreEqual("StudioX", pedido.Game.Produtora);
        }

        [TestMethod]
        public void CanSetAndGet_BaseProperties()
        {
            var pedido = new Pedido
            {
                Nome = "PedidoTest",
                Id = 123,
                UsuarioId = 1,
                GameId = 2
            };
            pedido.DataCriacao = new DateTime(2021, 1, 1);
            pedido.Email = "pedido@email.com";

            Assert.AreEqual("PedidoTest", pedido.Nome);
            Assert.AreEqual(123, pedido.Id);
            Assert.AreEqual(new DateTime(2021, 1, 1), pedido.DataCriacao);
            Assert.AreEqual("pedido@email.com", pedido.Email);
        }
    }
}
