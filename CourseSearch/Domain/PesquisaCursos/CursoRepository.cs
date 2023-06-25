using Dapper;
using ProjetoPesquisaAlura.Infrastructure;
using ProjetoPesquisaAlura.Domain.PesquisaCursos;


namespace ProjetoPesquisaAlura.Domain.PesquisaCursos
{
    public class CursoRepository : ICursoRepository
    {
        private readonly IBancoDados _bancoDados;

        public CursoRepository(IBancoDados bancoDados)
        {
            _bancoDados = bancoDados;
        }

        public void InserirCurso(Curso curso)
        {
            using (var connection = _bancoDados.ObterConexao())
            {
                connection.Execute("INSERT INTO Cursos (Titulo, Professor, Carga_Horaria, Descricao) VALUES (@Titulo, @Professor, @Carga_Horaria, @Descricao)", curso);
            }
        }
    }
}
