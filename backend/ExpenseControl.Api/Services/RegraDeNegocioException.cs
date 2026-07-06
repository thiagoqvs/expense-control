namespace ExpenseControl.Api.Services;

/// <summary>
/// Lançada quando uma operação viola uma regra de negócio (ex: pessoa não encontrada,
/// menor de idade tentando cadastrar receita, etc). É tratada nos controllers para
/// retornar um 400/404 amigável em vez de um erro 500 genérico.
/// </summary>
public class RegraDeNegocioException : Exception
{
    public RegraDeNegocioException(string message) : base(message) { }
}
