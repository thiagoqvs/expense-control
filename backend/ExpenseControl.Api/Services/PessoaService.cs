using ExpenseControl.Api.Data;
using ExpenseControl.Api.Dtos;
using ExpenseControl.Api.Models;

namespace ExpenseControl.Api.Services;

/// <summary>
/// Regras de negócio relacionadas ao cadastro de pessoas.
/// </summary>
public class PessoaService
{
    private readonly JsonDataStore _store;

    public PessoaService(JsonDataStore store)
    {
        _store = store;
    }

    public List<PessoaDto> Listar()
    {
        return _store.LerPessoas()
            .Select(PessoaDto.DeModelo)
            .ToList();
    }

    public PessoaDto Criar(CriarPessoaDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nome))
            throw new RegraDeNegocioException("O nome da pessoa é obrigatório.");

        if (dto.Idade < 0)
            throw new RegraDeNegocioException("A idade não pode ser negativa.");

        var pessoas = _store.LerPessoas();

        var pessoa = new Pessoa
        {
            Nome = dto.Nome.Trim(),
            Idade = dto.Idade
        };

        pessoas.Add(pessoa);
        _store.SalvarPessoas(pessoas);

        return PessoaDto.DeModelo(pessoa);
    }

    /// <summary>
    /// Deleta a pessoa e, em cascata, todas as transações vinculadas a ela
    /// (requisito explícito do desafio).
    /// </summary>
    public void Deletar(Guid id)
    {
        var pessoas = _store.LerPessoas();
        var pessoa = pessoas.FirstOrDefault(p => p.Id == id);

        if (pessoa is null)
            throw new RegraDeNegocioException("Pessoa não encontrada.");

        pessoas.Remove(pessoa);
        _store.SalvarPessoas(pessoas);

        // Cascata: remove todas as transações dessa pessoa
        var transacoes = _store.LerTransacoes();
        transacoes.RemoveAll(t => t.PessoaId == id);
        _store.SalvarTransacoes(transacoes);
    }

    /// <summary>Usado pelo serviço de transação para validar existência/idade.</summary>
    public Pessoa? BuscarPorId(Guid id)
    {
        return _store.LerPessoas().FirstOrDefault(p => p.Id == id);
    }
}
