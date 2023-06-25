using Microsoft.Extensions.DependencyInjection;
using ProjetoPesquisaAlura.Domain.PesquisaCursos;
using ProjetoPesquisaAlura.Infrastructure;
using System;

namespace ProjetoPesquisaAlura.Domain
{
    public static class Startup
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<IPesquisaCursoService, PesquisaCursoService>();
            services.AddTransient<IWebDriverFactory, SeleniumWebDriverFactory>();
            services.AddSingleton<ICursoRepository, CursoRepository>();
            services.AddSingleton<IBancoDados>(provider => new BancoDadosInfraestrutura("Data Source=DataBaseAlura.db;"));

            return services.BuildServiceProvider();
        }
    }
}
