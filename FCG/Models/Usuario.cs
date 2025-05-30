using Microsoft.EntityFrameworkCore.Query.Internal;

namespace FCG.Models
{
    public class Usuario : EntityBase, IEmail
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public string Senha { get; set; }
        public required DateTime DataNascimento { get; set; }
        public string Endereco { get; set; }
        public required short TipoUsuario { get; set; }

        #region [Navegacao]
        public virtual ICollection<Pedido> Pedidos { get; set; }
        #endregion
    }
}
 