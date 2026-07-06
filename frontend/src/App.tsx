import { useEffect, useState, useCallback } from "react";
import { api } from "./api/api";
import { ConsultaTotais, Pessoa, Transacao, TipoTransacao } from "./types";
import { PessoaForm } from "./components/PessoaForm";
import { PessoasList } from "./components/PessoasList";
import { TransacaoForm } from "./components/TransacaoForm";
import { TransacoesList } from "./components/TransacoesList";
import { Totais } from "./components/Totais";

/**
 * Componente raiz: carrega os dados do backend e distribui para os
 * componentes de cadastro/listagem/totais. Toda mutação (criar pessoa,
 * criar transação, deletar pessoa) recarrega os dados para manter a
 * tela sempre consistente com o que está persistido no backend.
 */
export default function App() {
  const [pessoas, setPessoas] = useState<Pessoa[]>([]);
  const [transacoes, setTransacoes] = useState<Transacao[]>([]);
  const [totais, setTotais] = useState<ConsultaTotais | null>(null);
  const [erroGeral, setErroGeral] = useState<string | null>(null);

  const carregarTudo = useCallback(async () => {
    try {
      const [pessoasResp, transacoesResp, totaisResp] = await Promise.all([
        api.listarPessoas(),
        api.listarTransacoes(),
        api.consultarTotais()
      ]);
      setPessoas(pessoasResp);
      setTransacoes(transacoesResp);
      setTotais(totaisResp);
      setErroGeral(null);
    } catch (err) {
      setErroGeral(
        err instanceof Error
          ? `Não foi possível conectar ao backend: ${err.message}`
          : "Não foi possível conectar ao backend."
      );
    }
  }, []);

  useEffect(() => {
    carregarTudo();
  }, [carregarTudo]);

  async function criarPessoa(nome: string, idade: number) {
    await api.criarPessoa(nome, idade);
    await carregarTudo();
  }

  async function deletarPessoa(id: string) {
    await api.deletarPessoa(id);
    await carregarTudo();
  }

  async function criarTransacao(
    descricao: string,
    valor: number,
    tipo: TipoTransacao,
    pessoaId: string
  ) {
    await api.criarTransacao(descricao, valor, tipo, pessoaId);
    await carregarTudo();
  }

  return (
    <div className="app">
      <header>
        <h1>Controle de Gastos Residenciais</h1>
      </header>

      {erroGeral && (
        <div className="card erro-conexao">
          <p>{erroGeral}</p>
          <p className="dica">Verifique se a API .NET está rodando em http://localhost:5000</p>
        </div>
      )}

      <main>
        <section>
          <PessoaForm onCriar={criarPessoa} />
          <PessoasList pessoas={pessoas} onDeletar={deletarPessoa} />
        </section>

        <section>
          <TransacaoForm pessoas={pessoas} onCriar={criarTransacao} />
          <TransacoesList transacoes={transacoes} pessoas={pessoas} />
        </section>

        <section>
          <Totais totais={totais} />
        </section>
      </main>
    </div>
  );
}
