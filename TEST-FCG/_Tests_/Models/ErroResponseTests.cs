using FCG.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TEST_FCG._Tests_.Models
{
    [TestClass]
    public class ErroResponseTests
    {
        [TestMethod]
        public void CanSetAndGet_StatusCode()
        {
            var error = new ErroResponse();
            error.StatusCode = 404;
            Assert.AreEqual(404, error.StatusCode);
        }

        [TestMethod]
        public void CanSetAndGet_Erro()
        {
            var error = new ErroResponse();
            error.Erro = "Not Found";
            Assert.AreEqual("Not Found", error.Erro);
        }

        [TestMethod]
        public void CanSetAndGet_Detalhe()
        {
            var error = new ErroResponse();
            error.Detalhe = "Resource does not exist";
            Assert.AreEqual("Resource does not exist", error.Detalhe);
        }

        [TestMethod]
        public void ToString_ReturnsExpectedFormat()
        {
            var error = new ErroResponse
            {
                StatusCode = 400,
                Erro = "Bad Request",
                Detalhe = "Invalid input"
            };

            var result = error.ToString();

            Assert.AreEqual("StatusCode: 400 - Error: Bad Request - Detail: Invalid input", result);
        }
    }
}
