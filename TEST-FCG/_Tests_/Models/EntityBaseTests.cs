using System;
using FCG.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TEST_FCG._Tests_.Models
{
    [TestClass]
    public class EntityBaseTests
    {
        [TestMethod]
        public void Constructor_SetsDataCriacaoToToday()
        {
            var entity = new EntityBase();
            Assert.AreEqual(DateTime.Now.Date, entity.DataCriacao);
        }

        [TestMethod]
        public void Nome_DefaultsToEmptyString()
        {
            var entity = new EntityBase();
            Assert.AreEqual(string.Empty, entity.Nome);
        }

        [TestMethod]
        public void CanSetAndGet_Nome()
        {
            var entity = new EntityBase();
            entity.Nome = "TestName";
            Assert.AreEqual("TestName", entity.Nome);
        }

        [TestMethod]
        public void CanSetAndGet_Id()
        {
            var entity = new EntityBase();
            entity.Id = 42;
            Assert.AreEqual(42, entity.Id);
        }

        [TestMethod]
        public void CanSetAndGet_DataCriacao()
        {
            var entity = new EntityBase();
            var date = new DateTime(2020, 1, 1);
            entity.DataCriacao = date;
            Assert.AreEqual(date, entity.DataCriacao);
        }

        [TestMethod]
        public void CanSetAndGet_Email()
        {
            var entity = new EntityBase();
            entity.Email = "test@email.com";
            Assert.AreEqual("test@email.com", entity.Email);
        }

        [TestMethod]
        public void Email_CanBeNull()
        {
            var entity = new EntityBase();
            entity.Email = null;
            Assert.IsNull(entity.Email);
        }
    }
}
