import { useState, FormEvent } from "react";
import { Pessoa, TipoTransacao } from "../types";

interface Props {
  pessoas: Pessoa[];
  onCriar: (descricao: string, valor: number, tipo: TipoTransacao, pessoaId: string) => Promise<void>;
}


export function TransacaoForm({ pessoas, onCriar }: Props) {
  const [descricao, setDescricao] = useState("");
  const [valor, setValor] = useState("");
  const [tipo, setTipo] = useState<TipoTransacao>("Despesa");
  const [pessoaId, setPessoaId] = useState("");
  const [erro, setErro] = useState<string | null>(null);
  const [enviando, setEnviando] = useState(false);

  const pessoaSelecionada = pessoas.find((p) => p.id === pessoaId);
  const soPermiteDespesa = pessoaSelecionada?.ehMenorDeIdade ?? false;

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setErro(null);

    const valorNum = Number(valor);
    if (!descricao.trim() || Number.isNaN(valorNum) || valorNum <= 0 || !pessoaId) {
      setErro("Preencha descrição, valor (> 0) e selecione uma pessoa.");
      return;
    }

    try {
      setEnviando(true);
      await onCriar(descricao.trim(), valorNum, tipo, pessoaId);
      setDescricao("");
      setValor("");
      setTipo("Despesa");
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro ao cadastrar transação.");
    } finally {
      setEnviando(false);
    }
  }

  return (
    <form onSubmit={handleSubmit} className="card">
      <h3>Cadastrar transação</h3>
      <div className="form-row">
        <select value={pessoaId} onChange={(e) => setPessoaId(e.target.value)}>
          <option value="">Selecione a pessoa</option>
          {pessoas.map((p) => (
            <option key={p.id} value={p.id}>
              {p.nome} {p.ehMenorDeIdade ? "(menor de idade)" : ""}
            </option>
          ))}
        </select>

        <input
          type="text"
          placeholder="Descrição"
          value={descricao}
          onChange={(e) => setDescricao(e.target.value)}
        />

        <input
          type="number"
          placeholder="Valor"
          value={valor}
          min={0.01}
          step="0.01"
          onChange={(e) => setValor(e.target.value)}
        />

        <select
          value={tipo}
          onChange={(e) => setTipo(e.target.value as TipoTransacao)}
        >
          <option value="Despesa">Despesa</option>
          <option value="Receita" disabled={soPermiteDespesa}>
            Receita
          </option>
        </select>

        <button type="submit" disabled={enviando}>
          {enviando ? "Salvando..." : "Cadastrar"}
        </button>
      </div>

      {soPermiteDespesa && (
        <p className="aviso">
          {pessoaSelecionada?.nome} é menor de idade: apenas despesas podem ser cadastradas.
        </p>
      )}
      {erro && <p className="erro">{erro}</p>}
    </form>
  );
}
