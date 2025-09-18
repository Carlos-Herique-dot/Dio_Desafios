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

static void Menu()
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
                continue;
            case "suite":
                continue;
            case "reserva":
                continue;
            case "sair":
                break; 
        }
    
    }
    

}

