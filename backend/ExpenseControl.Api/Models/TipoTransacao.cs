namespace ExpenseControl.Api.Models;

/// <summary>
/// Tipo de uma transação financeira.
/// Despesa = saída de dinheiro / Receita = entrada de dinheiro.
/// </summary>
public enum TipoTransacao
{
    Despesa = 0,
    Receita = 1
}
