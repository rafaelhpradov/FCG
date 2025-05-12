using FCG.Infrastructure;
using FCG.Interfaces;
using FCG.Models;

namespace FCG.Repository
{
    public class PedidoRepository : EFRepository<Pedido>, IPedidoRepository
    {
        public PedidoRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
