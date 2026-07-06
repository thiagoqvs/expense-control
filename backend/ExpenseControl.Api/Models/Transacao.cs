namespace ExpenseControl.Api.Models;

/// <summary>
/// Representa uma transação financeira (receita ou despesa) vinculada a uma pessoa.
/// </summary>
public class Transacao
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Descricao { get; set; } = string.Empty;

    /// <summary>
    /// Valor monetário da transação. Sempre armazenado como positivo;
    /// o sinal (soma/subtração) é aplicado apenas no cálculo dos totais,
    /// de acordo com o Tipo.
    /// </summary>
    public decimal Valor { get; set; }

    public TipoTransacao Tipo { get; set; }

    /// <summary>
    /// Identificador da pessoa dona da transação. Deve existir no cadastro de pessoas
    /// (validado na camada de serviço antes da criação).
    /// </summary>
    public Guid PessoaId { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
}
