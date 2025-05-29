using System;
using System.Collections.Generic;
using System.Linq;
using FCG.Models;
using FCG.Repository;
using FCG.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TEST_FCG._Tests_.Repository
{
    [TestClass]
    public class PedidoRepositoryTest
    {
        private ApplicationDbContext _context;
        private PedidoRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            // Seed related entities
            _context.Usuarios.AddRange(
                new Usuario { Id = 1, Nome = "User1", Email = "user1@email.com", DataNascimento = DateTime.Today.AddYears(-20), TipoUsuario = 1 },
                new Usuario { Id = 2, Nome = "User2", Email = "user2@email.com", DataNascimento = DateTime.Today.AddYears(-22), TipoUsuario = 1 }
            );
            _context.Games.AddRange(
                new Game { Id = 1, Nome = "Game1", Produtora = "Prod1", Descricao = "Desc1", DataLancamento = DateTime.Today, Preco = 10 },
                new Game { Id = 2, Nome = "Game2", Produtora = "Prod2", Descricao = "Desc2", DataLancamento = DateTime.Today, Preco = 20 }
            );
            _context.SaveChanges();

            _repository = new PedidoRepository(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public void Cadastrar_AddsPedido()
        {
            var pedido = new Pedido { UsuarioId = 1, GameId = 1 };
            _repository.Cadastrar(pedido);

            var pedidos = _repository.ObterTodos();
            Assert.AreEqual(1, pedidos.Count);
            Assert.AreEqual(1, pedidos[0].UsuarioId);
            Assert.AreEqual(1, pedidos[0].GameId);
        }

        [TestMethod]
        public void ObterTodos_ReturnsAllPedidos()
        {
            var pedido1 = new Pedido { UsuarioId = 1, GameId = 1 };
            var pedido2 = new Pedido { UsuarioId = 2, GameId = 2 };
            _repository.Cadastrar(pedido1);
            _repository.Cadastrar(pedido2);

            var pedidos = _repository.ObterTodos();
            Assert.AreEqual(2, pedidos.Count);
            Assert.IsTrue(pedidos.Any(p => p.UsuarioId == 1 && p.GameId == 1));
            Assert.IsTrue(pedidos.Any(p => p.UsuarioId == 2 && p.GameId == 2));
        }

        [TestMethod]
        public void ObterPorID_ReturnsCorrectPedido()
        {
            var pedido = new Pedido { UsuarioId = 1, GameId = 1 };
            _repository.Cadastrar(pedido);

            var result = _repository.ObterTodos().First();
            var found = _repository.ObterPorID(result.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(result.Id, found.Id);
            Assert.AreEqual(1, found.UsuarioId);
        }

        [TestMethod]
        public void ObterPorID_ReturnsNullIfNotFound()
        {
            var found = _repository.ObterPorID(999);
            Assert.IsNull(found);
        }

        [TestMethod]
        public void ObterPorNome_ReturnsNullForPedido()
        {
            // Pedido does not have Nome, so always returns null
            var result = _repository.ObterPorNome("any");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void ObterPorEmail_ReturnsNullForPedido()
        {
            // Pedido does not have Email, so always returns null
            var result = _repository.ObterPorEmail("any@email.com");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Alterar_UpdatesPedido()
        {
            var pedido = new Pedido { UsuarioId = 1, GameId = 1 };
            _repository.Cadastrar(pedido);
            var created = _repository.ObterTodos().First();

            created.GameId = 2;
            _repository.Alterar(created);

            var updated = _repository.ObterPorID(created.Id);
            Assert.IsNotNull(updated);
            Assert.AreEqual(2, updated.GameId);
        }

        [TestMethod]
        public void Deletar_RemovesPedido()
        {
            var pedido = new Pedido { UsuarioId = 1, GameId = 1 };
            _repository.Cadastrar(pedido);
            var created = _repository.ObterTodos().First();

            _repository.Deletar(created.Id);

            var found = _repository.ObterPorID(created.Id);
            Assert.IsNull(found);
        }

        [TestMethod]
        public void CadastrarEmMassa_AddsMultiplePedidos()
        {
            _repository.CadastrarEmMassa();

            var pedidos = _repository.ObterTodos();
            Assert.IsTrue(pedidos.Count >= 20); // Should be at least the number in the hardcoded list
        }
    }
}
