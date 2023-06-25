using Microsoft.Extensions.DependencyInjection;
using ProjetoPesquisaAlura.Domain;
using ProjetoPesquisaAlura.Domain.PesquisaCursos;
using ProjetoPesquisaAlura.Infrastructure;
using System;

namespace ProjetoPesquisaAlura
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configuração e inicialização da aplicação
            var serviceProvider = Startup.ConfigureServices().GetRequiredService<IPesquisaCursoService>();

            string connectionString = "Data Source=DataBaseAlura.db;";

            BancoDadosInfraestrutura bancoDados = new BancoDadosInfraestrutura(connectionString);
            bancoDados.CriarTabelaCursos();

            Console.WriteLine("Banco de dados criado com sucesso.");

            // Realizar a pesquisa
            string termoPesquisa = "RPA";
            serviceProvider.PesquisarCursos(termoPesquisa);

            // Finalizar o programa
            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }


    }
}
