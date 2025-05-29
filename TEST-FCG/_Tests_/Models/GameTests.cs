using System;
using System.Collections.Generic;
using FCG.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TEST_FCG._Tests_.Models
{
    [TestClass]
    public class GameTests
    {
        [TestMethod]
        public void CanSetAndGet_Produtora()
        {
            var game = new Game { Produtora = "StudioX", Descricao = "Desc", Preco = 1, DataLancamento = DateTime.Today };
            game.Produtora = "NewStudio";
            Assert.AreEqual("NewStudio", game.Produtora);
        }

        [TestMethod]
        public void CanSetAndGet_Descricao()
        {
            var game = new Game { Produtora = "StudioX", Descricao = "Desc", Preco = 1, DataLancamento = DateTime.Today };
            game.Descricao = "New Description";
            Assert.AreEqual("New Description", game.Descricao);
        }

        [TestMethod]
        public void CanSetAndGet_Preco()
        {
            var game = new Game { Produtora = "StudioX", Descricao = "Desc", Preco = 1, DataLancamento = DateTime.Today };
            game.Preco = 99.99m;
            Assert.AreEqual(99.99m, game.Preco);
        }

        [TestMethod]
        public void CanSetAndGet_DataLancamento()
        {
            var game = new Game { Produtora = "StudioX", Descricao = "Desc", Preco = 1, DataLancamento = DateTime.Today };
            var date = new DateTime(2022, 5, 1);
            game.DataLancamento = date;
            Assert.AreEqual(date, game.DataLancamento);
        }

        [TestMethod]
        public void CanSetAndGet_Pedidos()
        {
            var pedidos = new List<Pedido>
            {
                new Pedido { UsuarioId = 1, GameId = 1 },
                new Pedido { UsuarioId = 2, GameId = 1 }
            };
            var game = new Game { Produtora = "StudioX", Descricao = "Desc", Preco = 1, DataLancamento = DateTime.Today, Pedidos = pedidos };
            Assert.AreEqual(2, game.Pedidos.Count);
            Assert.AreEqual(1, pedidos[0].UsuarioId); // Accessing the list directly
        }

        [TestMethod]
        public void CanSetAndGet_BaseProperties()
        {
            var game = new Game
            {
                Nome = "TestGame",
                Id = 123,
                Produtora = "StudioX",
                Descricao = "Desc",
                Preco = 1,
                DataLancamento = DateTime.Today
            };
            game.DataCriacao = new DateTime(2020, 1, 1);
            game.Email = "test@game.com";

            Assert.AreEqual("TestGame", game.Nome);
            Assert.AreEqual(123, game.Id);
            Assert.AreEqual(new DateTime(2020, 1, 1), game.DataCriacao);
            Assert.AreEqual("test@game.com", game.Email);
        }
    }
}
