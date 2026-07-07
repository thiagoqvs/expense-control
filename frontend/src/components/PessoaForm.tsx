import { useState, FormEvent } from "react";

interface Props {
  onCriar: (nome: string, idade: number) => Promise<void>;
}


export function PessoaForm({ onCriar }: Props) {
  const [nome, setNome] = useState("");
  const [idade, setIdade] = useState("");
  const [erro, setErro] = useState<string | null>(null);
  const [enviando, setEnviando] = useState(false);

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();
    setErro(null);

    const idadeNum = Number(idade);
    if (!nome.trim() || Number.isNaN(idadeNum) || idadeNum < 0) {
      setErro("Preencha nome e idade válidos.");
      return;
    }

    try {
      setEnviando(true);
      await onCriar(nome.trim(), idadeNum);
      setNome("");
      setIdade("");
    } catch (err) {
      setErro(err instanceof Error ? err.message : "Erro ao cadastrar pessoa.");
    } finally {
      setEnviando(false);
    }
  }

  return (
    <form onSubmit={handleSubmit} className="card">
      <h3>Cadastrar pessoa</h3>
      <div className="form-row">
        <input
          type="text"
          placeholder="Nome"
          value={nome}
          onChange={(e) => setNome(e.target.value)}
        />
        <input
          type="number"
          placeholder="Idade"
          value={idade}
          min={0}
          onChange={(e) => setIdade(e.target.value)}
        />
        <button type="submit" disabled={enviando}>
          {enviando ? "Salvando..." : "Cadastrar"}
        </button>
      </div>
      {erro && <p className="erro">{erro}</p>}
    </form>
  );
}
