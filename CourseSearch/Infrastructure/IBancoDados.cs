using System.Data;

namespace ProjetoPesquisaAlura.Infrastructure
{
    public interface IBancoDados
    {
        IDbConnection ObterConexao();
    }
}
