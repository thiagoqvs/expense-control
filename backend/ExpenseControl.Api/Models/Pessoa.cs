using System.Text.Json.Serialization;

namespace ExpenseControl.Api.Models;

/// <summary>
/// Representa uma pessoa do controle de gastos residenciais.
/// O Id é gerado automaticamente pelo sistema (Guid), garantindo unicidade
/// sem depender de um banco de dados relacional com auto-incremento.
/// </summary>
public class Pessoa
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Nome { get; set; } = string.Empty;

    public int Idade { get; set; }

    /// <summary>
    /// Regra de negócio central: pessoas menores de 18 anos só podem
    /// ter transações do tipo Despesa (nunca Receita).
    /// Não é persistido no arquivo JSON: é sempre recalculado a partir da
    /// Idade atual, para nunca ficar desatualizado.
    /// </summary>
    [JsonIgnore]
    public bool EhMenorDeIdade => Idade < 18;
}
