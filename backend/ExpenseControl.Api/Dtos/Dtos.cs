using ExpenseControl.Api.Models;

namespace ExpenseControl.Api.Dtos;


public record CriarPessoaDto(string Nome, int Idade);

public record PessoaDto(Guid Id, string Nome, int Idade, bool EhMenorDeIdade)
{
    public static PessoaDto DeModelo(Pessoa p) => new(p.Id, p.Nome, p.Idade, p.EhMenorDeIdade);
}


public record CriarTransacaoDto(string Descricao, decimal Valor, TipoTransacao Tipo, Guid PessoaId);

public record TransacaoDto(Guid Id, string Descricao, decimal Valor, TipoTransacao Tipo, Guid PessoaId, DateTime DataCriacao)
{
    public static TransacaoDto DeModelo(Transacao t) => new(t.Id, t.Descricao, t.Valor, t.Tipo, t.PessoaId, t.DataCriacao);
}


public record TotalPessoaDto(Guid PessoaId, string Nome, decimal TotalReceitas, decimal TotalDespesas, decimal Saldo);

public record ConsultaTotaisDto(List<TotalPessoaDto> Pessoas, decimal TotalGeralReceitas, decimal TotalGeralDespesas, decimal SaldoGeral);
