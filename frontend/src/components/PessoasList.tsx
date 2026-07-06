import { Pessoa } from "../types";

interface Props {
  pessoas: Pessoa[];
  onDeletar: (id: string) => Promise<void>;
}

/**
 * Lista as pessoas cadastradas. Ao deletar, o backend também remove
 * (em cascata) todas as transações vinculadas a essa pessoa.
 */
export function PessoasList({ pessoas, onDeletar }: Props) {
  return (
    <div className="card">
      <h3>Pessoas cadastradas</h3>
      {pessoas.length === 0 && <p className="vazio">Nenhuma pessoa cadastrada ainda.</p>}
      <ul className="lista">
        {pessoas.map((pessoa) => (
          <li key={pessoa.id}>
            <span>
              <strong>{pessoa.nome}</strong> — {pessoa.idade} anos
              {pessoa.ehMenorDeIdade && <span className="badge">menor de idade</span>}
            </span>
            <button
              className="btn-perigo"
              onClick={() => {
                if (confirm(`Remover ${pessoa.nome}? Isso apaga todas as transações dessa pessoa.`)) {
                  onDeletar(pessoa.id);
                }
              }}
            >
              Excluir
            </button>
          </li>
        ))}
      </ul>
    </div>
  );
}
