using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace SistemaDeReservaHotel.Models
{
    public class Suite
    {
        public string? TipoSuite { get; set; }
        public int Capacidade { get; set; }
        public decimal ValorDiaria { get; set; }
        private const string connectionString = "Data Source=dbHotel.db";

        public void CadastrarSuites(string tipo, int capacidade)
        {
            string sql = "INSERT INTO Suites (TipoSuite, Capacidade) VALUES (@tipo, @capacidade)";
            try
            {
                using (SqliteConnection conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@tipo", tipo);
                    cmd.Parameters.AddWithValue("@capacidade", capacidade);
                    cmd.ExecuteNonQuery();

                    Console.WriteLine("Suites Cadastradas com sucesso");
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Erro ao Cadastrar Suites {ex.Message}");
            }
        }
    }
}