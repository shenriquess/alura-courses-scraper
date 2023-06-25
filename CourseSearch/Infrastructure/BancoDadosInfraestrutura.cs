using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace ProjetoPesquisaAlura.Infrastructure
{
    public class BancoDadosInfraestrutura : IBancoDados
    {
        private readonly string _connectionString;

        public BancoDadosInfraestrutura(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection ObterConexao()
        {
            return new SQLiteConnection(_connectionString);
        }

        public void ExecutarComando(string sql, object parametros = null)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sql;
                    if (parametros != null)
                    {
                        foreach (var parametro in ObterParametros(parametros))
                        {
                            command.Parameters.AddWithValue(parametro.Key, parametro.Value);
                        }
                    }
                    command.ExecuteNonQuery();
                }
            }
        }

        public void CriarTabelaCursos()
        {
            string sqlVerificarTabela = @"
                SELECT COUNT(*) FROM sqlite_master 
                WHERE type = 'table' AND name = 'Cursos';
            ";

            using (var connection = new SQLiteConnection(_connectionString))
            {
                connection.Open();
                int tabelaExiste = 0;

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = sqlVerificarTabela;
                    tabelaExiste = Convert.ToInt32(command.ExecuteScalar());
                }

                if (tabelaExiste == 0)
                {
                    string sqlCriarTabela = @"
                        CREATE TABLE Cursos (
                            ID INTEGER PRIMARY KEY AUTOINCREMENT,
                            Titulo TEXT,
                            Professor TEXT,
                            Carga_Horaria TEXT,
                            Descricao TEXT
                        );
                    ";

                    ExecutarComando(sqlCriarTabela);
                }
            }
        }

        private IEnumerable<KeyValuePair<string, object>> ObterParametros(object parametros)
        {
            var properties = parametros.GetType().GetProperties();
            foreach (var property in properties)
            {
                yield return new KeyValuePair<string, object>(property.Name, property.GetValue(parametros));
            }
        }
    }
}
