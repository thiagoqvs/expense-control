using ExpenseControl.Api.Data;
using ExpenseControl.Api.Dtos;
using ExpenseControl.Api.Models;

namespace ExpenseControl.Api.Services;

/// <summary>
/// Calcula os totais de receitas, despesas e saldo por pessoa, além do total geral.
/// </summary>
public class TotaisService
{
    private readonly JsonDataStore _store;

    public TotaisService(JsonDataStore store)
    {
        _store = store;
    }

    public ConsultaTotaisDto Calcular()
    {
        var pessoas = _store.LerPessoas();
        var transacoes = _store.LerTransacoes();

        var totaisPorPessoa = pessoas.Select(pessoa =>
        {
            var transacoesDaPessoa = transacoes.Where(t => t.PessoaId == pessoa.Id).ToList();

            var totalReceitas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Receita)
                .Sum(t => t.Valor);

            var totalDespesas = transacoesDaPessoa
                .Where(t => t.Tipo == TipoTransacao.Despesa)
                .Sum(t => t.Valor);

            return new TotalPessoaDto(
                pessoa.Id,
                pessoa.Nome,
                totalReceitas,
                totalDespesas,
                totalReceitas - totalDespesas);
        }).ToList();

        var totalGeralReceitas = totaisPorPessoa.Sum(p => p.TotalReceitas);
        var totalGeralDespesas = totaisPorPessoa.Sum(p => p.TotalDespesas);

        return new ConsultaTotaisDto(
            totaisPorPessoa,
            totalGeralReceitas,
            totalGeralDespesas,
            totalGeralReceitas - totalGeralDespesas);
    }
}
