using System.Text.Json.Serialization;

namespace ExpenseControl.Api.Models;


public class Pessoa
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nome { get; set; } = string.Empty;

    public int Idade { get; set; }


    [JsonIgnore]
    public bool EhMenorDeIdade => Idade < 18;
}
