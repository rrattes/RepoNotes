# RepoNotes Task Log

## 2026-05-20 16:06:29 -03:00

**Objetivo da rodada:** Criar a documentacao operacional do projeto e estabelecer a regra de log obrigatorio para rodadas futuras.

**Arquivos alterados:**

- `docs/PRODUCT.md`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/CODEX_RULES.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foram criados os documentos base de produto, roadmap, guia visual, regras de desenvolvimento para Codex e o log de tarefas do projeto.

**Resultado do dotnet build:** Sucesso em `2026-05-20 16:06 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Pendencias:** Nenhuma pendencia desta rodada.

**Riscos tecnicos:** Baixo risco; a rodada altera apenas documentacao.

**Proximo passo sugerido:** Usar estes documentos como referencia para as proximas mudancas e manter o `TASK_LOG.md` atualizado a cada rodada.

## 2026-05-20 16:13:20 -03:00

**Objetivo da rodada:** Compactar e refinar a interface principal para aumentar a area util de escrita, mantendo o visual premium dark e preservando MVVM/bindings.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A sidebar foi reduzida para `252px`, o preview para `326px`, a top bar para `54px` e a status bar para `38px`. A area acima do editor foi compactada com aba de `32px`, titulo de `50px`, toolbar de `34px`, botoes menores e menos padding externo/interno no editor. O preview, sidebar, tree, tags e status bar tambem receberam ajustes de densidade para deixar o editor central mais dominante.

**Resultado do dotnet build:** Sucesso em `2026-05-20 16:13 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Pendencias:** Nenhuma pendencia desta rodada.

**Riscos tecnicos:** Baixo risco funcional; as mudancas sao visuais. Risco residual: a UI pode precisar de ajuste fino visual em telas menores caso algum texto de botao fique apertado.

**Proximo passo sugerido:** Testar visualmente em 1366x768 e 1600x900, depois considerar um controle simples de recolhimento para preview/sidebar em rodada futura se o usuario solicitar.

## 2026-05-20 16:18:32 -03:00

**Objetivo da rodada:** Iniciar a transicao do mockup visual para um editor funcional local-first, adicionando carregamento e salvamento basico de notas Markdown locais sem alterar o visual principal.

**Arquivos alterados:**

- `RepoNotes.Core/Services/INoteRepository.cs`
- `RepoNotes.Storage/LocalMarkdownNoteRepository.cs`
- `RepoNotes.Storage/MockNoteRepository.cs`
- `RepoNotes.App/App.axaml.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Tests/LocalMarkdownNoteRepositoryTests.cs`
- `sample-repository/Inbox/Bem-vindo.md`
- `sample-repository/Projetos/Roadmap.md`
- `sample-repository/Runbooks/Deploy-local.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foi criada uma implementacao `LocalMarkdownNoteRepository` para ler arquivos `.md`, montar a arvore de pastas/notas e salvar a nota selecionada de volta em disco. O app passou a usar esse repositorio local por padrao, com `sample-repository` como pasta inicial. O ViewModel agora marca a nota como `Alterado` ao editar e salva via `SaveNoteCommand`, acionado por `Ctrl+S`. O mock repository continua disponivel como fallback/teste.

**Resultado do dotnet build:** Sucesso em `2026-05-20 16:18 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-20 16:18 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 4 testes aprovados, 0 falhas.

**Pendencias:** Ainda nao ha seletor de repositorio, criacao/renomeacao/exclusao de notas, refresh manual da arvore ou renderizacao Markdown real no preview.

**Riscos tecnicos:** Baixo a medio; a escrita em arquivo ja funciona, mas ainda nao ha tratamento visual de erro de IO nem controle de conflito se o arquivo for alterado fora do app enquanto esta aberto.

**Proximo passo sugerido:** Adicionar uma acao visual discreta de salvar e/ou tratamento de erro no ViewModel, depois implementar selecao de pasta de repositorio local.

## 2026-05-20 16:20:17 -03:00

**Objetivo da rodada:** Corrigir a top bar removendo botoes falsos de minimizar, maximizar e fechar, mantendo apenas controles reais da aplicacao.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foram removidos os botoes fake de controle de janela (`-`, `□`, `X`) da top bar. A coluna direita foi reduzida para comportar apenas os botoes de tema e configuracoes. O guia de UI agora registra que o MVP usa os controles nativos de janela do Windows/Avalonia e que custom window chrome fica fora do escopo atual.

**Resultado do dotnet build:** Sucesso em `2026-05-20 16:20 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Pendencias:** Nenhuma pendencia desta rodada.

**Riscos tecnicos:** Baixo risco; a mudanca e visual e remove apenas controles sem funcionalidade real.

**Proximo passo sugerido:** Testar visualmente a top bar com a barra nativa do sistema e, em rodada futura, avaliar se tema/configuracoes devem receber icones finais.

## 2026-05-20 16:32:48 -03:00

**Objetivo da rodada:** Endurecer o fluxo de edicao/salvamento Markdown local com feedback claro de estado, botao discreto de salvar e tratamento basico de erro sem alterar o layout principal.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Tests/RepoNotes.Tests.csproj`
- `RepoNotes.Tests/MainWindowViewModelSaveTests.cs`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O ViewModel agora diferencia `Salvo`, `Alterado`, `Salvando...` e `Erro ao salvar`, evita chamar `SaveNote` quando a nota nao foi alterada, preserva o conteudo em caso de erro e expoe `LastErrorMessage`. A toolbar recebeu um botao compacto `Salvar` conectado ao `SaveNoteCommand`, mantendo `Ctrl+S`. Foram adicionados testes para salvar nota alterada, ignorar salvamento sem alteracao e exibir erro sem perder conteudo.

**Resultado do dotnet build:** Sucesso em `2026-05-20 16:32 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros. Observacao: a primeira tentativa falhou porque uma instancia aberta de `RepoNotes.App.exe` bloqueava o arquivo de saida; a instancia foi encerrada e o build final passou.

**Resultado dos testes:** Sucesso em `2026-05-20 16:32 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 7 testes aprovados, 0 falhas.

**Pendencias:** Ainda nao ha modal para avisar sobre alteracoes nao salvas ao trocar de nota, nem tratamento visual dedicado para erro alem do status e mensagem curta.

**Riscos tecnicos:** Medio-baixo; o fluxo evita crash em erros comuns de IO, mas ainda nao cobre conflitos externos de arquivo ou recuperacao automatica.

**Proximo passo sugerido:** Adicionar aviso simples ao trocar de nota com alteracoes pendentes ou implementar autosave com debounce em uma rodada futura.
