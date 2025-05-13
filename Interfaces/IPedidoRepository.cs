using FCG.Models;

namespace FCG.Interfaces
{
    public interface IPedidoRepository : IEFRepository<Pedido>
    {
        void CadastrarEmMassa();
    }
}
 