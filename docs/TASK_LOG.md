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

## 2026-05-20 16:35:25 -03:00

**Objetivo da rodada:** Registrar como requisito de produto o suporte futuro a Lightweight Technical Entities inspiradas em inventario/DCIM/IPAM, sem transformar RepoNotes em substituto completo do NetBox.

**Arquivos alterados:**

- `docs/PRODUCT.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O produto agora documenta uma camada futura de entidades tecnicas leves para Application, Server, Network Device, Site, Environment, IP / Endpoint, Owner / Team e Vendor / Product. O requisito deixa claro que essas entidades enriquecem a documentacao local, permitindo relacionar notas, runbooks, scripts, incidentes e handovers a contextos tecnicos. O roadmap ganhou um milestone incremental de Technical Entities e delimitou que full DCIM/IPAM, rack/cabling/patch panels, lifecycle de VLANs e circuitos ficam fora do escopo.

**Resultado do dotnet build:** Not run, documentation-only change.

**Pendencias:** Definir formato de armazenamento local das entidades, relacionamento nota-entidade e estrategia de navegacao/exportacao quando a implementacao for priorizada.

**Riscos tecnicos:** Medio; o requisito pode crescer demais se nao permanecer leve e focado em documentacao. Deve continuar separado de uma fonte de verdade DCIM/IPAM completa.

**Proximo passo sugerido:** Quando o fluxo de notas locais estiver estavel, desenhar um modelo minimo de entidades em Markdown/frontmatter ou JSON local antes de qualquer UI ou persistencia complexa.

## 2026-05-20 17:26:09 -03:00

**Objetivo da rodada:** Registrar como requisito de produto o suporte futuro a User-Managed Encryption com senha propria do usuario, independente da senha do PC/Windows.

**Arquivos alterados:**

- `docs/PRODUCT.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O produto agora documenta criptografia opcional baseada em senha criada pelo usuario para proteger pasta, subpasta ou repositorio inteiro. A decisao registra que a senha nao depende da conta Windows, senha do PC, dominio corporativo, permissao de administrador, DPAPI ou Windows Credential Manager como modelo principal. O roadmap ganhou um milestone futuro para definir formato de armazenamento, desbloqueio, busca/preview/indexacao/exportacao em conteudo bloqueado, UX de perda de senha, backup e portabilidade.

**Resultado do dotnet build:** Not run, documentation-only change.

**Pendencias:** Definir formato criptografado, derivacao de chave, comportamento de desbloqueio, politicas de cache/indexacao, UX para senha perdida e impacto em backup/portabilidade antes de qualquer implementacao.

**Riscos tecnicos:** Alto; perda de senha torna conteudo irrecuperavel, busca/indexacao precisam evitar plaintext indevido, criptografia de repositorio inteiro pode impactar preview/navegacao/performance, e a feature nao pode crescer para um sistema corporativo de gestao de chaves.

**Proximo passo sugerido:** Apos estabilizar notas locais, escrever um design tecnico curto para criptografia de escopo folder/subfolder/repository antes de adicionar bibliotecas, modelos ou UI.

## 2026-05-20 20:18:08 -03:00

**Objetivo da rodada:** Refinar o uso da area superior da interface para separar melhor navegacao global e contexto do documento aberto, mantendo densidade e visual premium dark.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A area superior do editor agora funciona como uma document context bar compacta, combinando aba da nota, breadcrumb com repositorio/caminho e acoes contextuais `Salvar`, `Info` e `Tags`. O botao `Salvar` saiu da toolbar de formatacao e foi reposicionado junto ao contexto da nota. A toolbar do editor ficou focada em comandos de formatacao, enquanto a barra global permanece dedicada a navegacao/app-level controls.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:18 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Pendencias:** `Info` e `Tags` ainda sao acoes visuais/contextuais sem painel funcional dedicado nesta rodada.

**Riscos tecnicos:** Baixo; mudanca visual em XAML sem alteracao de regra de negocio. Risco residual de ajuste fino em larguras do breadcrumb em telas menores.

**Proximo passo sugerido:** Testar visualmente o breadcrumb e, em rodada futura, conectar `Info`/`Tags` a paineis reais ou remover placeholders caso nao sejam priorizados.

## 2026-05-20 20:25:39 -03:00

**Objetivo da rodada:** Remover totalmente a barra superior interna do RepoNotes, reduzir chrome visual e mover controles globais para a sidebar para priorizar conteudo.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A top bar interna foi removida do layout raiz, junto com menu/logo, titulo `RepoNotes`, seletor de repositorio, busca e botoes soltos do canto superior direito. O seletor de repositorio e a busca foram movidos para o topo da sidebar, enquanto configuracoes foi movido para a mini-toolbar inferior esquerda. A area central passa a iniciar diretamente no contexto do documento/editor, mantendo breadcrumb e acoes da nota.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:25 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Pendencias:** A mini-toolbar inferior ainda usa rotulos textuais compactos (`N`, `*`, `Cfg`) que podem ser refinados com iconografia real em uma rodada visual futura.

**Riscos tecnicos:** Baixo; alteracao visual em XAML sem mudanca de regra de negocio. Risco residual de a busca na sidebar precisar de ajuste em telas muito estreitas.

**Proximo passo sugerido:** Testar visualmente o novo fluxo sem top bar e decidir se a sidebar deve receber icones finais para repositorio, busca e configuracoes.

## 2026-05-20 20:30:38 -03:00

**Objetivo da rodada:** Refinar a sidebar apos a remocao da top bar, eliminando placeholders textuais e corrigindo a ambiguidade do seletor de repositorio.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O topo da sidebar agora mostra um cartao informativo de `Repositorio atual`, sem comando de troca ou atalho para configuracoes. A mini-toolbar inferior deixou de usar `N`, `*` e `Cfg` e passou a exibir acoes compactas `Novo`, `Fav` e `Config`, com tooltips claros para nova nota, favoritos e configuracoes. Foi adicionado um comando placeholder seguro para favoritos, sem implementar funcionalidade real.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:30 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Pendencias:** Seletor real de repositorio, tela de configuracoes e favoritos continuam fora do MVP desta rodada.

**Riscos tecnicos:** Baixo; a mudanca e visual e adiciona apenas um comando placeholder no ViewModel para feedback de status. Risco residual de ajuste fino dos labels compactos em larguras muito estreitas.

**Proximo passo sugerido:** Testar visualmente a sidebar e, em rodada futura, substituir labels compactos por iconografia final quando houver biblioteca de icones definida.

## 2026-05-20 20:36:57 -03:00

**Objetivo da rodada:** Implementar protecao contra perda de alteracoes ao trocar de nota, salvando automaticamente a nota atual antes da troca e bloqueando a selecao se o salvamento falhar.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.Tests/MainWindowViewModelSaveTests.cs`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O ViewModel agora tenta salvar alteracoes pendentes antes de selecionar outra nota. Se o salvamento passa, a troca continua e o status volta para `Salvo`; se falha, o app mantem a nota editada aberta, preserva o conteudo e mostra `Erro ao salvar` com uma mensagem curta. Os testes cobrem troca com salvamento automatico e bloqueio de troca em erro de IO.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:36 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-20 20:36 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 10 testes aprovados, 0 falhas.

**Pendencias:** Ainda nao ha modal de confirmacao ou opcao de descartar alteracoes; o comportamento atual sempre tenta salvar antes de trocar de nota.

**Riscos tecnicos:** Medio-baixo; o fluxo protege contra perda de edicao, mas ainda e sincronico e nao trata conflitos externos de arquivo ou merge de alteracoes feitas fora do app.

**Proximo passo sugerido:** Adicionar tratamento visual mais claro para erros persistentes de salvamento e, depois, avaliar autosave com debounce ou dialogo de conflito quando houver alteracoes externas.
