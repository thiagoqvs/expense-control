# Controle de Gastos Residenciais

Sistema de controle de gastos residenciais com cadastro de pessoas, cadastro de
transações (receitas/despesas) e consulta de totais.

- **Backend:** .NET 8 (C#) — API REST
- **Frontend:** React + TypeScript (Vite)
- **Persistência:** arquivos JSON em disco (os dados sobrevivem ao fechar a aplicação)

## Estrutura do repositório

```
backend/
  ExpenseControl.sln
  ExpenseControl.Api/
    Models/          -> Pessoa, Transacao, TipoTransacao
    Data/            -> JsonDataStore (leitura/escrita dos arquivos JSON)
    Services/        -> regras de negócio (PessoaService, TransacaoService, TotaisService)
    Controllers/      -> endpoints da API (PessoasController, TransacoesController, TotaisController)
    Dtos/            -> objetos de entrada/saída da API
    data/            -> pessoas.json e transacoes.json (gerados/atualizados em tempo de execução)
frontend/
  src/
    api/api.ts        -> cliente HTTP que fala com o backend
    components/        -> formulários e listagens (Pessoa, Transação, Totais)
    App.tsx           -> orquestra o carregamento e as mutações de dados
```

## Como rodar

### Backend

```bash
cd backend/ExpenseControl.Api
dotnet restore
dotnet run
```

A API sobe em `http://localhost:5000`. Os dados ficam persistidos em
`backend/ExpenseControl.Api/data/*.json`.

> O projeto não usa nenhum pacote NuGet externo (só o SDK do ASP.NET Core), então
> `dotnet restore`/`dotnet run` funcionam mesmo sem acesso à internet.

### Frontend

```bash
cd frontend
npm install
npm run dev
```

A aplicação abre em `http://localhost:5173` e já está configurada para conversar com o
backend em `http://localhost:5000` (CORS liberado apenas para essa origem).

> Suba o backend antes do frontend, senão as listagens aparecem vazias com um aviso de
> conexão na tela.

## Regras de negócio implementadas

- **Pessoa**
  - `Id` gerado automaticamente (`Guid`), nome e idade.
  - Criação, listagem e exclusão.
  - **Ao excluir uma pessoa, todas as transações vinculadas a ela são excluídas em
    cascata** (feito em `PessoaService.Deletar`, que remove a pessoa e depois filtra as
    transações órfãs).

- **Transação**
  - `Id` gerado automaticamente, descrição, valor, tipo (Despesa/Receita) e o id da
    pessoa dona da transação.
  - Criação e listagem (sem edição/exclusão, conforme especificado).
  - O `PessoaId` informado precisa existir no cadastro de pessoas — validado em
    `TransacaoService.Criar` antes de gravar.
  - **Menores de 18 anos só podem ter transações do tipo Despesa.** Essa regra é
    validada no backend (fonte da verdade, em `TransacaoService.Criar`) e também
    refletida no frontend (o select de "Receita" fica desabilitado quando a pessoa
    selecionada é menor de idade), para dar feedback imediato ao usuário.

- **Consulta de totais**
  - Para cada pessoa: soma de receitas, soma de despesas e saldo (receita − despesa).
  - Ao final, o total geral (soma de todas as pessoas) de receitas, despesas e saldo
    líquido.
  - Calculado em `TotaisService.Calcular`, que agrupa as transações por `PessoaId`.

## Decisões técnicas

- **Persistência em JSON** em vez de banco de dados: mantém a solução rodando em
  qualquer máquina sem dependências externas, ao mesmo tempo que cumpre o requisito de
  persistência entre execuções. O acesso aos arquivos é protegido por lock para evitar
  race condition.
- **DTOs separados dos modelos de domínio**, para não expor detalhes internos na API e
  deixar claro o contrato de entrada/saída de cada endpoint.
- **Validação de regras de negócio na camada de serviço**, não nos controllers nem no
  frontend — o frontend só replica a regra do menor de idade como UX, mas quem garante
  a regra de verdade é o backend.

## Endpoints da API

| Método | Rota                | Descrição                                   |
|--------|----------------------|----------------------------------------------|
| GET    | `/api/pessoas`       | Lista todas as pessoas                        |
| POST   | `/api/pessoas`       | Cria uma pessoa (`{ nome, idade }`)           |
| DELETE | `/api/pessoas/{id}`  | Remove a pessoa e suas transações em cascata  |
| GET    | `/api/transacoes`    | Lista todas as transações                     |
| POST   | `/api/transacoes`    | Cria uma transação (`{ descricao, valor, tipo, pessoaId }`) |
| GET    | `/api/totais`        | Retorna totais por pessoa + total geral       |

## Autor

**Thiago Victorino**

- GitHub: [github.com/thiagoqvs](https://github.com/thiagoqvs)
- LinkedIn: [linkedin.com/in/thiagoqvictorino](https://linkedin.com/in/thiagoqvictorino)
