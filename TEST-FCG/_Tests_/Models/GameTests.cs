using System;
using System.Collections.Generic;
using FCG.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FCG.Models.Tests
{
    [TestClass]
    public class GameTest
    {
        [TestMethod]
        public void CanSetAndGet_Produtora()
        {
            var game = new Game
            {
                Produtora = "TestProdutora",
                Descricao = "TestDescricao",
                Preco = 99.99m,
                DataLancamento = new DateTime(2024, 1, 1),
                Pedidos = new List<Pedido>()
            };
            Assert.AreEqual("TestProdutora", game.Produtora);
        }

        [TestMethod]
        public void CanSetAndGet_Descricao()
        {
            var game = new Game
            {
                Produtora = "Prod",
                Descricao = "Descricao do jogo",
                Preco = 10,
                DataLancamento = DateTime.Today,
                Pedidos = new List<Pedido>()
            };
            Assert.AreEqual("Descricao do jogo", game.Descricao);
        }

        [TestMethod]
        public void CanSetAndGet_Preco()
        {
            var game = new Game
            {
                Produtora = "Prod",
                Descricao = "Desc",
                Preco = 123.45m,
                DataLancamento = DateTime.Today,
                Pedidos = new List<Pedido>()
            };
            Assert.AreEqual(123.45m, game.Preco);
        }

        [TestMethod]
        public void CanSetAndGet_DataLancamento()
        {
            var date = new DateTime(2023, 12, 31);
            var game = new Game
            {
                Produtora = "Prod",
                Descricao = "Desc",
                Preco = 1,
                DataLancamento = date,
                Pedidos = new List<Pedido>()
            };
            Assert.AreEqual(date, game.DataLancamento);
        }

        [TestMethod]
        public void CanSetAndGet_Pedidos()
        {
            var pedidos = new List<Pedido>
            {
                new Pedido { Id = 1, Nome = "Pedido1", GameId = 1, UsuarioId = 1, DataCriacao = DateTime.Today }
            };
            var game = new Game
            {
                Produtora = "Prod",
                Descricao = "Desc",
                Preco = 1,
                DataLancamento = DateTime.Today,
                Pedidos = pedidos
            };
            Assert.AreEqual(pedidos, game.Pedidos);
            Assert.AreEqual(1, game.Pedidos.Count);
        }

        [TestMethod]
        public void InheritsFrom_EntityBase()
        {
            var game = new Game
            {
                Produtora = "Prod",
                Descricao = "Desc",
                Preco = 1,
                DataLancamento = DateTime.Today,
                Pedidos = new List<Pedido>()
            };
            game.Nome = "GameName";
            game.Id = 42;
            game.DataCriacao = new DateTime(2022, 1, 1);

            Assert.AreEqual("GameName", game.Nome);
            Assert.AreEqual(42, game.Id);
            Assert.AreEqual(new DateTime(2022, 1, 1), game.DataCriacao);
        }
    }
}
