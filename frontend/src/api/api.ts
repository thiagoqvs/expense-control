import type { ConsultaTotais, Pessoa, Transacao, TipoTransacao } from "../types";

const BASE_URL = "http://localhost:5000/api";


async function request<T>(path: string, options?: RequestInit): Promise<T> {
  const response = await fetch(`${BASE_URL}${path}`, {
    headers: { "Content-Type": "application/json" },
    ...options
  });

  if (!response.ok) {
    let mensagem = `Erro na requisição (${response.status})`;
    try {
      const corpo = await response.json();
      if (corpo?.erro) mensagem = corpo.erro;
    } catch {
    }
    throw new Error(mensagem);
  }

  if (response.status === 204) return undefined as T;
  return response.json() as Promise<T>;
}

export const api = {
  // ---------- Pessoas ----------
  listarPessoas: () => request<Pessoa[]>("/pessoas"),

  criarPessoa: (nome: string, idade: number) =>
    request<Pessoa>("/pessoas", {
      method: "POST",
      body: JSON.stringify({ nome, idade })
    }),

  deletarPessoa: (id: string) =>
    request<void>(`/pessoas/${id}`, { method: "DELETE" }),

  // ---------- Transações ----------
  listarTransacoes: () => request<Transacao[]>("/transacoes"),

  criarTransacao: (descricao: string, valor: number, tipo: TipoTransacao, pessoaId: string) =>
    request<Transacao>("/transacoes", {
      method: "POST",
      body: JSON.stringify({ descricao, valor, tipo, pessoaId })
    }),

  // ---------- Totais ----------
  consultarTotais: () => request<ConsultaTotais>("/totais")
};
