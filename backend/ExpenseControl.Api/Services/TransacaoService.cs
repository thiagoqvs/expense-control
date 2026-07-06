using ExpenseControl.Api.Data;
using ExpenseControl.Api.Dtos;
using ExpenseControl.Api.Models;

namespace ExpenseControl.Api.Services;

/// <summary>
/// Regras de negócio relacionadas ao cadastro de transações.
/// </summary>
public class TransacaoService
{
    private readonly JsonDataStore _store;
    private readonly PessoaService _pessoaService;

    public TransacaoService(JsonDataStore store, PessoaService pessoaService)
    {
        _store = store;
        _pessoaService = pessoaService;
    }

    public List<TransacaoDto> Listar()
    {
        return _store.LerTransacoes()
            .OrderByDescending(t => t.DataCriacao)
            .Select(TransacaoDto.DeModelo)
            .ToList();
    }

    public TransacaoDto Criar(CriarTransacaoDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Descricao))
            throw new RegraDeNegocioException("A descrição da transação é obrigatória.");

        if (dto.Valor <= 0)
            throw new RegraDeNegocioException("O valor da transação deve ser maior que zero.");

        // O identificador de pessoa precisa existir no cadastro de pessoas
        var pessoa = _pessoaService.BuscarPorId(dto.PessoaId);
        if (pessoa is null)
            throw new RegraDeNegocioException("Pessoa informada não encontrada. Cadastre a pessoa antes de lançar a transação.");

        // Regra central: menor de idade só pode cadastrar DESPESA
        if (pessoa.EhMenorDeIdade && dto.Tipo == TipoTransacao.Receita)
            throw new RegraDeNegocioException(
                $"{pessoa.Nome} tem {pessoa.Idade} anos e é menor de idade: apenas despesas podem ser cadastradas para menores de 18 anos.");

        var transacoes = _store.LerTransacoes();

        var transacao = new Transacao
        {
            Descricao = dto.Descricao.Trim(),
            Valor = dto.Valor,
            Tipo = dto.Tipo,
            PessoaId = dto.PessoaId
        };

        transacoes.Add(transacao);
        _store.SalvarTransacoes(transacoes);

        return TransacaoDto.DeModelo(transacao);
    }
}
