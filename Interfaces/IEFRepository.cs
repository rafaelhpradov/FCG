using FCG.Models;

namespace FCG.Interfaces
{
    public interface IEFRepository<T> where T : EntityBase
    {
        IList<T> ObterTodos();
        T ObterPorID(int id);
        T ObterPorNome(string nome);
        void Cadastrar(T entity);
        void Alterar(T entity);
        void Deletar(int id);
    }
}
