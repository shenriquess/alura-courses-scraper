using System.Collections.Generic;

namespace ProjetoPesquisaAlura.Domain.PesquisaCursos
{
    public interface IPesquisaCursoService
    {
        void PesquisarCursos(string termoPesquisa);
        void InserirCursos(List<Curso> cursos);
    }
}
