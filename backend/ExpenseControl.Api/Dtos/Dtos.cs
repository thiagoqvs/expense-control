using ExpenseControl.Api.Models;

namespace ExpenseControl.Api.Dtos;

// ---------- Pessoa ----------

/// <summary>Dados recebidos para criar uma pessoa.</summary>
public record CriarPessoaDto(string Nome, int Idade);

/// <summary>Dados retornados de uma pessoa.</summary>
public record PessoaDto(Guid Id, string Nome, int Idade, bool EhMenorDeIdade)
{
    public static PessoaDto DeModelo(Pessoa p) => new(p.Id, p.Nome, p.Idade, p.EhMenorDeIdade);
}

// ---------- Transação ----------

/// <summary>Dados recebidos para criar uma transação.</summary>
public record CriarTransacaoDto(string Descricao, decimal Valor, TipoTransacao Tipo, Guid PessoaId);

/// <summary>Dados retornados de uma transação.</summary>
public record TransacaoDto(Guid Id, string Descricao, decimal Valor, TipoTransacao Tipo, Guid PessoaId, DateTime DataCriacao)
{
    public static TransacaoDto DeModelo(Transacao t) => new(t.Id, t.Descricao, t.Valor, t.Tipo, t.PessoaId, t.DataCriacao);
}

// ---------- Totais ----------

/// <summary>Totais consolidados de uma única pessoa.</summary>
public record TotalPessoaDto(Guid PessoaId, string Nome, decimal TotalReceitas, decimal TotalDespesas, decimal Saldo);

/// <summary>Resposta completa da consulta de totais: cada pessoa + o total geral.</summary>
public record ConsultaTotaisDto(List<TotalPessoaDto> Pessoas, decimal TotalGeralReceitas, decimal TotalGeralDespesas, decimal SaldoGeral);
