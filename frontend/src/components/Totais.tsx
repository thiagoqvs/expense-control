import { ConsultaTotais } from "../types";

interface Props {
  totais: ConsultaTotais | null;
}

function formatarMoeda(valor: number) {
  return valor.toLocaleString("pt-BR", { style: "currency", currency: "BRL" });
}


export function Totais({ totais }: Props) {
  if (!totais) return null;

  return (
    <div className="card">
      <h3>Consulta de totais</h3>
      {totais.pessoas.length === 0 && <p className="vazio">Nenhuma pessoa cadastrada ainda.</p>}
      <table className="tabela-totais">
        <thead>
          <tr>
            <th>Pessoa</th>
            <th>Receitas</th>
            <th>Despesas</th>
            <th>Saldo</th>
          </tr>
        </thead>
        <tbody>
          {totais.pessoas.map((p) => (
            <tr key={p.pessoaId}>
              <td>{p.nome}</td>
              <td className="receita">{formatarMoeda(p.totalReceitas)}</td>
              <td className="despesa">{formatarMoeda(p.totalDespesas)}</td>
              <td className={p.saldo >= 0 ? "receita" : "despesa"}>{formatarMoeda(p.saldo)}</td>
            </tr>
          ))}
        </tbody>
        <tfoot>
          <tr>
            <td><strong>Total geral</strong></td>
            <td className="receita"><strong>{formatarMoeda(totais.totalGeralReceitas)}</strong></td>
            <td className="despesa"><strong>{formatarMoeda(totais.totalGeralDespesas)}</strong></td>
            <td className={totais.saldoGeral >= 0 ? "receita" : "despesa"}>
              <strong>{formatarMoeda(totais.saldoGeral)}</strong>
            </td>
          </tr>
        </tfoot>
      </table>
    </div>
  );
}
