using ExemploExplorando.Models;

Pessoa p1 = new Pessoa(nome: "Carlos", sobrenome: "Henrique");

Pessoa p2 = new Pessoa(nome: "Leonador", sobrenome: "Buta");

Curso cursoDeIngles = new Curso();
cursoDeIngles.Nome = "Inglês";
cursoDeIngles.Alunos = new List<Pessoa>();

cursoDeIngles.AdicionarAluno(p1);
cursoDeIngles.AdicionarAluno(p2);
cursoDeIngles.ListarAlunos();