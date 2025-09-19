using System.IO;
using SistemaDeReservaHotel.Models;

Pessoa cliente = new Pessoa();
Reserva reserva = new Reserva();
Suite suite = new Suite();

try
{
    bool dbExiste = File.Exists("dbHotel.db");
    if (!dbExiste)
    {
        reserva.CriarBancoDeDados();
    }

    Menu();

}
catch (System.Exception ex)
{
    Console.WriteLine("Erro na busca pelo diretório." + ex.Message);
}

void Menu()
{
    while (true)
    {
        Console.WriteLine("Hotel do Céu");
        Console.WriteLine("Informe o que deseja");
        Console.WriteLine
        ("Cadastrar Hospede:[hospede]\nCadastrar outra suite:[suite]\nFazer Reserva:[reserva]");
        string desejo = Console.ReadLine()!;

        switch (desejo)
        {
            case "hospede":
                Console.WriteLine("Nome do Cliente");
                string Nome = Console.ReadLine()!;
                Console.WriteLine("Sobrenome do Cliente");
                string Sobrenome = Console.ReadLine()!;
                Console.WriteLine("CPF do Cliente");
                string Cpf = Console.ReadLine()!;

                reserva.CadastrarPessoas(Cpf, Nome, Sobrenome);
                break;
            case "reserva":
                reserva.Reservar();
                break;
            case "sair":
                break;
        }
        break;
    }
    

}

