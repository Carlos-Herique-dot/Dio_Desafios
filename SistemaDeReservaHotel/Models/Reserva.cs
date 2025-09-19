using System;
using System.Data.Common;
using System.IO.Compression;
using System.Security.Cryptography;
using Microsoft.Data.Sqlite;
using SQLitePCL;

namespace SistemaDeReservaHotel.Models
{
    public class Reserva
    {

        public string? Hospedes { get; set; }
        public Suite? Suite { get; set; }
        public int DiasReservados { get; set; }

        private const string connectionString = "Data Source=dbHotel.db";

        public void CriarBancoDeDados()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    var createTableCommand = connection.CreateCommand();

                    createTableCommand.CommandText = @"CREATE TABLE IF NOT EXISTS Pessoas(
                    Id INTEGER PRIMARY KEY,
                    Nome TEXT NOT NULL,
                    Sobrenome TEXT);
                    
                    CREATE TABLE IF NOT EXISTS Suites (
                    Id INTEGER PRIMARY KEY,
                    TipoSuite TEXT NOT NULL,
                    Capacidade INTEGER NOT NULL);
                    
                    CREATE TABLE IF NOT EXISTS Reservas (
                    Id INTEGER PRIMARY KEY,
                    PessoaId INTEGER,
                    SuiteId INTEGER,
                    FOREIGN KEY(PessoaId) REFERENCES Pessoas(Id), 
                    FOREIGN KEY(SuiteId) REFERENCES Suites(Id)   
                    );";

                    createTableCommand.ExecuteNonQuery();
                    Console.WriteLine("Banco de dados criado com sucesso.");

                }
                catch (SqliteException ex)
                {
                    Console.WriteLine("Erro ao conectar no banco de dados" + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void CadastrarPessoas(string cpf, string nome, string sobrenome)
        {
            using (var conn = new SqliteConnection(connectionString))
            {
                conn.Open();

                var cmd = conn.CreateCommand();
                cmd.CommandText = "INSERT INTO Pessoas (ID, Nome, Sobrenome) VALUES (@cpf, @nome, @sobrenome)";

                cmd.Parameters.AddWithValue("@cpf", cpf);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@sobrenome", sobrenome);

                cmd.ExecuteNonQuery();

                Console.WriteLine("Cliente Cadastrado com Sucesso.");
                conn.Close();
            }

        }

        public string BuscarCliente(string cpf)
        {
            string Id = "";
            try
            {
                using (SqliteConnection conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    string sql = "SELECT ID, Nome, Sobrenome FROM Pessoas WHERE Id = @cpf";
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    var source = cmd.ExecuteReader();

                    while (source.Read())
                    {
                        Id = source.GetString(source.GetOrdinal("Id"));
                        string PNome = source.GetString(source.GetOrdinal("Nome"));
                        string SNome = source.GetString(source.GetOrdinal("Sobrenome"));
                        string NomeCompleto = $"{PNome} {SNome}";
                        Console.WriteLine($"O nome completo do hospede é {NomeCompleto}");
                    }

                    conn.Close();
                }
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Erro ao buscar cliente" + ex.Message);
            }
            return Id;
        }

        public int QuantidadeDeCLientes()
        {
            string sql = "SELECT * FROM Pessoas";
            int i = 0;
            try
            {
                using (SqliteConnection conn = new SqliteConnection(connectionString))
                {

                    conn.Open();
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    var reader = cmd.ExecuteReader();

                    foreach (var item in reader)
                    {

                        i++;

                    }
                }
                return i;

            }
            catch (SqliteException ex)
            {
                Console.WriteLine($"Erro ao contar clientes {ex.Message})");
                return i;
            }
        }

        public string BuscarSuites(string suiteDesejada)
        {
            string sql = "SELECT * FROM Suites WHERE TipoSuite = @suiteDesejada";
            string id = "";
            string tipo, capacidade;
            SqliteConnection conn = new SqliteConnection(connectionString);
            try
            {
                conn.Open();
                SqliteCommand cmd = new SqliteCommand(sql, conn);
                cmd.Parameters.AddWithValue("@suiteDesejada", suiteDesejada);
                var dados = cmd.ExecuteReader();

                while (dados.Read())
                {
                    id = dados.GetString(dados.GetOrdinal("Id"));
                    tipo = dados.GetString(dados.GetOrdinal("TipoSuite"));
                    capacidade = dados.GetString(dados.GetOrdinal("Capacidade"));
                }
                conn.Close();
                
            }
            catch (SqliteException ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
            }
            return id;
        } 
        
        public void Reservar()
        {
            string pragma = "PRAGMA foreign_keys = ON;";
            string sql = "INSERT INTO Reservas (PessoaId, SuiteId) VALUES (@cpf, @tipoId)";

            Console.WriteLine("Informe o CPF do cliente");
            string cpf = Console.ReadLine()!;

            BuscarCliente(cpf);

            Console.WriteLine("Esse é o nome do hospede?[S/N]");
            string resposta = Console.ReadLine()!.ToLower();

            Console.WriteLine("Informe a Suite desejada [Basic/Master/Premium]");
            string suiteDesejada = Console.ReadLine()!;
            string idSuite = BuscarSuites(suiteDesejada);

            Console.WriteLine("Quanto tempo pretende ficar? [dias]");
            int tempo = Convert.ToInt32(Console.ReadLine());

            decimal valor = 0;
            decimal valorAtualizado = 0;
            switch (suiteDesejada)
            {
                case "Basic":
                    valor = tempo * 100;
                    valorAtualizado = valor;
                    if (tempo >= 10)
                    {
                        valorAtualizado = (valor * 90) / 100;
                    }
                    break;
                case "Master":
                    valor = tempo * 150;
                    valorAtualizado = valor;
                    if (tempo >= 10)
                    {
                        valorAtualizado = (valor * 90) / 100;
                    }
                    break;
                case "Premium":
                    valor = tempo * 250;
                    valorAtualizado = valor;
                    if (tempo >= 10)
                    {
                        valorAtualizado = (valor * 90) / 100;
                    }
                    break;
            }

            if (resposta == "s")
            {
                SqliteConnection conn = new SqliteConnection(connectionString);
                try
                {
                    conn.Open();
                    //obrigatório pragma no SQLite para rodar Foreich Key
                    SqliteCommand prag = new SqliteCommand(pragma, conn);
                    prag.ExecuteNonQuery();
                    //Fim pragma
                    SqliteCommand cmd = new SqliteCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    cmd.Parameters.AddWithValue("@tipoId", idSuite);
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Hospede agendado com sucesso.");
                    conn.Close();
                    Console.WriteLine($"O Valor que o hospede pagará é {valorAtualizado:C}");
                }
                catch (SqliteException ex)
                {
                    Console.WriteLine($"Erro {ex.Message}");
                }
            }
            else if (resposta == "n")
            {
                Console.WriteLine("Vamos fechar tudo.");
            }
        }
    }

   
}