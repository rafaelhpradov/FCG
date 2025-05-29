using System;
using System.Collections.Generic;
using FCG.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TEST_FCG._Tests_.Models
{
    [TestClass]
    public class UsuarioTests
    {
        [TestMethod]
        public void CanSetAndGet_Nome()
        {
            var usuario = new Usuario
            {
                Nome = "Alice",
                Email = "alice@email.com",
                DataNascimento = new DateTime(1995, 5, 5),
                TipoUsuario = 1
            };
            usuario.Nome = "Bob";
            Assert.AreEqual("Bob", usuario.Nome);
        }

        [TestMethod]
        public void CanSetAndGet_Email()
        {
            var usuario = new Usuario
            {
                Nome = "Alice",
                Email = "alice@email.com",
                DataNascimento = new DateTime(1995, 5, 5),
                TipoUsuario = 1
            };
            usuario.Email = "bob@email.com";
            Assert.AreEqual("bob@email.com", usuario.Email);
        }

        [TestMethod]
        public void CanSetAndGet_Senha()
        {
            var usuario = new Usuario
            {
                Nome = "Alice",
                Email = "alice@email.com",
                DataNascimento = new DateTime(1995, 5, 5),
                TipoUsuario = 1
            };
            usuario.Senha = "123456";
            Assert.AreEqual("123456", usuario.Senha);
        }

        [TestMethod]
        public void CanSetAndGet_DataNascimento()
        {
            var usuario = new Usuario
            {
                Nome = "Alice",
                Email = "alice@email.com",
                DataNascimento = new DateTime(1995, 5, 5),
                TipoUsuario = 1
            };
            var date = new DateTime(2000, 1, 1);
            usuario.DataNascimento = date;
            Assert.AreEqual(date, usuario.DataNascimento);
        }

        [TestMethod]
        public void CanSetAndGet_Endereco()
        {
            var usuario = new Usuario
            {
                Nome = "Alice",
                Email = "alice@email.com",
                DataNascimento = new DateTime(1995, 5, 5),
                TipoUsuario = 1
            };
            usuario.Endereco = "123 Main St";
            Assert.AreEqual("123 Main St", usuario.Endereco);
        }

        [TestMethod]
        public void CanSetAndGet_TipoUsuario()
        {
            var usuario = new Usuario
            {
                Nome = "Alice",
                Email = "alice@email.com",
                DataNascimento = new DateTime(1995, 5, 5),
                TipoUsuario = 1
            };
            usuario.TipoUsuario = 2;
            Assert.AreEqual(2, usuario.TipoUsuario);
        }

        [TestMethod]
        public void CanSetAndGet_Pedidos()
        {
            var pedidos = new List<Pedido>
            {
                new Pedido { UsuarioId = 1, GameId = 1 },
                new Pedido { UsuarioId = 1, GameId = 2 }
            };
            var usuario = new Usuario
            {
                Nome = "Alice",
                Email = "alice@email.com",
                DataNascimento = new DateTime(1995, 5, 5),
                TipoUsuario = 1,
                Pedidos = pedidos
            };

            var pedidosList = usuario.Pedidos.ToList(); // Convert ICollection<Pedido> to List<Pedido>
            Assert.AreEqual(2, pedidosList.Count);
            Assert.AreEqual(1, pedidosList[0].UsuarioId);
            Assert.AreEqual(2, pedidosList[1].GameId);
        }

        [TestMethod]
        public void CanSetAndGet_BaseProperties()
        {
            var usuario = new Usuario
            {
                Nome = "TestUser",
                Email = "test@email.com",
                DataNascimento = new DateTime(2000, 1, 1),
                TipoUsuario = 1,
                Id = 42
            };
            usuario.DataCriacao = new DateTime(2022, 2, 2);
            usuario.Email = "updated@email.com";

            Assert.AreEqual("TestUser", usuario.Nome);
            Assert.AreEqual(42, usuario.Id);
            Assert.AreEqual(new DateTime(2022, 2, 2), usuario.DataCriacao);
            Assert.AreEqual("updated@email.com", usuario.Email);
        }
    }
}
