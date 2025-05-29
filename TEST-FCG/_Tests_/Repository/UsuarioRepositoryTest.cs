using System;
using System.Collections.Generic;
using System.Linq;
using FCG.Models;
using FCG.Repository;
using FCG.Infrastructure;
using FCG.Helpers;
using FCG.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TEST_FCG._Tests_.Repository
{
    [TestClass]
    public class UsuarioRepositoryTest
    {
        private ApplicationDbContext _context;
        private UsuarioRepository _repository;
        private CriptografiaHelper _criptografiaHelper;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ApplicationDbContext(options);
            _context.Database.EnsureCreated();

            _criptografiaHelper = new CriptografiaHelper();
            _repository = new UsuarioRepository(_context, _criptografiaHelper);

            // Seed related entities
            _context.Games.Add(new Game
            {
                Id = 1,
                Nome = "Game1",
                Produtora = "Prod1",
                Descricao = "Desc1",
                DataLancamento = DateTime.Today,
                Preco = 10
            });
            _context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public void Cadastrar_AddsUsuario()
        {
            var usuario = new Usuario
            {
                Nome = "User1",
                Email = "user1@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                TipoUsuario = 1,
                Senha = _criptografiaHelper.Criptografar("senha123")
            };

            _repository.Cadastrar(usuario);

            var usuarios = _repository.ObterTodos();
            Assert.AreEqual(1, usuarios.Count);
            Assert.AreEqual("User1", usuarios[0].Nome);
        }

        [TestMethod]
        public void ObterTodos_ReturnsAllUsuarios()
        {
            var usuario1 = new Usuario
            {
                Nome = "User1",
                Email = "user1@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                TipoUsuario = 1,
                Senha = _criptografiaHelper.Criptografar("senha123")
            };
            var usuario2 = new Usuario
            {
                Nome = "User2",
                Email = "user2@email.com",
                DataNascimento = new DateTime(1999, 1, 1),
                TipoUsuario = 2,
                Senha = _criptografiaHelper.Criptografar("senha456")
            };

            _repository.Cadastrar(usuario1);
            _repository.Cadastrar(usuario2);

            var usuarios = _repository.ObterTodos();
            Assert.AreEqual(2, usuarios.Count);
            Assert.IsTrue(usuarios.Any(u => u.Nome == "User1"));
            Assert.IsTrue(usuarios.Any(u => u.Nome == "User2"));
        }

        [TestMethod]
        public void ObterPorID_ReturnsCorrectUsuario()
        {
            var usuario = new Usuario
            {
                Nome = "User1",
                Email = "user1@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                TipoUsuario = 1,
                Senha = _criptografiaHelper.Criptografar("senha123")
            };
            _repository.Cadastrar(usuario);

            var created = _repository.ObterTodos().First();
            var found = _repository.ObterPorID(created.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(created.Id, found.Id);
            Assert.AreEqual("User1", found.Nome);
        }

        [TestMethod]
        public void ObterPorID_ReturnsNullIfNotFound()
        {
            var found = _repository.ObterPorID(999);
            Assert.IsNull(found);
        }

        [TestMethod]
        public void ObterPorNome_ReturnsCorrectUsuario()
        {
            var usuario = new Usuario
            {
                Nome = "User1",
                Email = "user1@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                TipoUsuario = 1,
                Senha = _criptografiaHelper.Criptografar("senha123")
            };
            _repository.Cadastrar(usuario);

            var found = _repository.ObterPorNome("User1");
            Assert.IsNotNull(found);
            Assert.AreEqual("User1", found.Nome);
        }

        [TestMethod]
        public void ObterPorNome_ReturnsNullIfNotFound()
        {
            var found = _repository.ObterPorNome("NotFound");
            Assert.IsNull(found);
        }

        [TestMethod]
        public void ObterPorEmail_ReturnsCorrectUsuario()
        {
            var usuario = new Usuario
            {
                Nome = "User1",
                Email = "user1@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                TipoUsuario = 1,
                Senha = _criptografiaHelper.Criptografar("senha123")
            };
            _repository.Cadastrar(usuario);

            var found = _repository.ObterPorEmail("user1@email.com");
            Assert.IsNotNull(found);
            Assert.AreEqual("user1@email.com", found.Email);
        }

        [TestMethod]
        public void ObterPorEmail_ReturnsNullIfNotFound()
        {
            var found = _repository.ObterPorEmail("notfound@email.com");
            Assert.IsNull(found);
        }

        [TestMethod]
        public void Alterar_UpdatesUsuario()
        {
            var usuario = new Usuario
            {
                Nome = "User1",
                Email = "user1@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                TipoUsuario = 1,
                Senha = _criptografiaHelper.Criptografar("senha123")
            };
            _repository.Cadastrar(usuario);
            var created = _repository.ObterTodos().First();

            created.Nome = "UpdatedUser";
            _repository.Alterar(created);

            var updated = _repository.ObterPorID(created.Id);
            Assert.IsNotNull(updated);
            Assert.AreEqual("UpdatedUser", updated.Nome);
        }

        [TestMethod]
        public void Deletar_RemovesUsuario()
        {
            var usuario = new Usuario
            {
                Nome = "User1",
                Email = "user1@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                TipoUsuario = 1,
                Senha = _criptografiaHelper.Criptografar("senha123")
            };
            _repository.Cadastrar(usuario);
            var created = _repository.ObterTodos().First();

            _repository.Deletar(created.Id);

            var found = _repository.ObterPorID(created.Id);
            Assert.IsNull(found);
        }

        [TestMethod]
        public void ObterPedidosSeisMeses_ReturnsOnlyRecentPedidos()
        {
            var usuario = new Usuario
            {
                Nome = "User1",
                Email = "user1@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                TipoUsuario = 1,
                Senha = _criptografiaHelper.Criptografar("senha123"),
                Pedidos = new List<Pedido>()
            };
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            var oldPedido = new Pedido
            {
                UsuarioId = usuario.Id,
                GameId = 1,
                DataCriacao = DateTime.Now.AddMonths(-7)
            };
            var recentPedido = new Pedido
            {
                UsuarioId = usuario.Id,
                GameId = 1,
                DataCriacao = DateTime.Now.AddMonths(-2)
            };
            _context.Pedidos.AddRange(oldPedido, recentPedido);
            _context.SaveChanges();

            var result = _repository.ObterPedidosSeisMeses(usuario.Id);

            Assert.IsNotNull(result);
            Assert.AreEqual(usuario.Id, result.Id);
            Assert.IsNotNull(result.Pedidos);
            Assert.IsTrue(result.Pedidos.All(p => DateTime.Parse(p.DataCriacao) >= DateTime.Now.AddMonths(-6).Date));
            Assert.IsTrue(result.Pedidos.Count == 1);
        }
    }
}
