using FCG.DTOs;
using FCG.Models;

namespace FCG.Interfaces
{
    public interface IUsuarioRepository : IEFRepository<Usuario>
    {
        void CadastrarEmMassa();
        UsuarioDto ObterPedidosTodos(int id);
        UsuarioDto ObterPedidosSeisMeses(int id);
    }
}
 