import { Pessoa, Transacao } from "../types";

interface Props {
  transacoes: Transacao[];
  pessoas: Pessoa[];
}

function formatarMoeda(valor: number) {
  return valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
}

/** Lista todas as transações cadastradas, mostrando o nome da pessoa (não só o id). */
export function TransacoesList({ transacoes, pessoas }: Props) {
  function nomeDaPessoa(pessoaId: string) {
    return pessoas.find((p) => p.id === pessoaId)?.nome ?? "(pessoa removida)";
  }

  return (
    <div className="card">
      <h3>Transações</h3>
      {transacoes.length === 0 && <p className="vazio">Nenhuma transação cadastrada ainda.</p>}
      <ul className="lista">
        {transacoes.map((t) => (
          <li key={t.id}>
            <span>
              <strong className={t.tipo === "Receita" ? "receita" : "despesa"}>
                {t.tipo === "Receita" ? "+" : "-"} {formatarMoeda(t.valor)}
              </strong>{" "}
              — {t.descricao} ({nomeDaPessoa(t.pessoaId)})
            </span>
          </li>
        ))}
      </ul>
    </div>
  );
}
