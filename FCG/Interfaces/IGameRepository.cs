using FCG.Models;

namespace FCG.Interfaces
{
    public interface IGameRepository : IEFRepository<Game>
    {
        void CadastrarEmMassa();
    }
}
 