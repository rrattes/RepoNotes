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

## 2026-05-20 20:38:33 -03:00

**Objetivo da rodada:** Executar a validacao obrigatoria solicitada apos a implementacao de autosave antes da troca de notas e registrar os resultados no log do projeto.

**Arquivos alterados:**

- `docs/TASK_LOG.md`

**Resumo das mudancas:** Rodada de validacao/documentacao sem alteracao de codigo, produto ou UI. Foram executados build, testes, status e diff. O unico arquivo de codigo/dado ainda modificado fora do log e `sample-repository/Inbox/Bem-vindo.md`, que permanece como alteracao local nao incluida no commit.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:38 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-20 20:38 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 10 testes aprovados, 0 falhas.

**Pendencias:** A alteracao local em `sample-repository/Inbox/Bem-vindo.md` continua fora do escopo e nao foi commitada. Nao ha nova pendencia tecnica criada por esta rodada.

**Riscos tecnicos:** Baixo; rodada apenas de validacao e registro. Risco residual permanece no fluxo sincronico de salvamento antes da troca de notas, especialmente para conflitos externos de arquivo.

**Proximo passo sugerido:** Testar manualmente o fluxo no app: editar uma nota, selecionar outra, confirmar salvamento automatico e validar o comportamento quando o arquivo estiver bloqueado ou sem permissao de escrita.

## 2026-05-20 20:42:51 -03:00

**Objetivo da rodada:** Implementar selecao de repositorio local pelo usuario, com persistencia do ultimo repositorio aberto e fallback para `sample-repository`.

**Arquivos alterados:**

- `RepoNotes.App/App.axaml.cs`
- `RepoNotes.App/Services/AvaloniaFolderPickerService.cs`
- `RepoNotes.App/Services/IFolderPickerService.cs`
- `RepoNotes.App/Services/NullFolderPickerService.cs`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.App/ViewModels/AsyncRelayCommand.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Core/Services/IRepositorySettingsStore.cs`
- `RepoNotes.Storage/JsonRepositorySettingsStore.cs`
- `RepoNotes.Tests/JsonRepositorySettingsStoreTests.cs`
- `RepoNotes.Tests/MainWindowViewModelSaveTests.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O card de `Repositorio atual` virou um botao real conectado a `OpenRepositoryCommand`, abrindo o seletor de pasta do Avalonia. O ViewModel agora recarrega a arvore e a nota atual a partir da pasta escolhida, atualiza `RepositoryName`, salva o ultimo caminho em `%LOCALAPPDATA%/RepoNotes/settings.json` e usa `sample-repository` como fallback quando nao ha repositorio salvo ou quando o caminho salvo/escolhido nao existe. Foram adicionados servicos pequenos para picker de pasta e settings JSON, alem de testes para troca de repositorio, fallback e persistencia local.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:42 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado final: 0 avisos, 0 erros. Observacao: a primeira tentativa falhou porque uma instancia aberta de `RepoNotes.App.exe` bloqueava DLLs de saida; a instancia foi encerrada e o build final passou.

**Resultado dos testes:** Sucesso em `2026-05-20 20:42 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 14 testes aprovados, 0 falhas.

**Pendencias:** Ainda nao ha lista de repositorios recentes, refresh manual, criacao/renomeacao/exclusao de arquivos, nem tratamento avancado de repositorio vazio alem do status amigavel.

**Riscos tecnicos:** Medio-baixo; o fluxo ainda e sincronico ao trocar repositos e depende de acesso local ao filesystem. Repositorios muito grandes podem precisar de carregamento assicrono/cancelavel e tratamento melhor de erros de permissao.

**Proximo passo sugerido:** Testar manualmente abrir uma pasta com arquivos `.md`, fechar e reabrir o app para confirmar a persistencia, e depois implementar refresh/file operations quando a navegacao local estiver validada.

## 2026-05-20 20:47:39 -03:00

**Objetivo da rodada:** Implementar criacao real de notas Markdown e pastas no repositorio local aberto, usando nomes automaticos seguros e atualizando a arvore imediatamente.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.Core/Services/INoteRepository.cs`
- `RepoNotes.Storage/LocalMarkdownNoteRepository.cs`
- `RepoNotes.Storage/MockNoteRepository.cs`
- `RepoNotes.Tests/LocalMarkdownNoteRepositoryTests.cs`
- `RepoNotes.Tests/MainWindowViewModelCreateTests.cs`
- `RepoNotes.Tests/MainWindowViewModelSaveTests.cs`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** `NewNoteCommand` e `NewFolderCommand` agora criam arquivos `.md` e diretorios reais no repositorio local. A criacao usa nomes automaticos como `Nova nota.md`, `Nova nota 2.md`, `Nova pasta` e `Nova pasta 2`, sanitiza caracteres invalidos para Windows e evita sobrescrever itens existentes. A nota criada e selecionada e aberta no editor; a arvore e atualizada imediatamente, incluindo pastas vazias recem-criadas.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:47 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-20 20:47 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 19 testes aprovados, 0 falhas.

**Pendencias:** Ainda nao ha dialogo para escolher nome inicial, renomear, excluir, mover arquivos ou atualizar manualmente o repositorio. A criacao usa nomes automaticos nesta rodada.

**Riscos tecnicos:** Medio-baixo; nomes e caminhos sao sanitizados e protegidos contra sobrescrita, mas conflitos externos e permissoes de filesystem ainda podem exigir mensagens de erro mais detalhadas.

**Proximo passo sugerido:** Testar manualmente criar nota/pasta em raiz, dentro de pasta e a partir de uma nota selecionada; depois priorizar renomear/excluir ou um dialogo simples de nome.

## 2026-05-20 20:51:39 -03:00

**Objetivo da rodada:** Implementar renomear e excluir notas/pastas com seguranca, movendo exclusoes para uma lixeira local do repositorio.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Core/Services/INoteRepository.cs`
- `RepoNotes.Storage/LocalMarkdownNoteRepository.cs`
- `RepoNotes.Storage/MockNoteRepository.cs`
- `RepoNotes.Tests/LocalMarkdownNoteRepositoryTests.cs`
- `RepoNotes.Tests/MainWindowViewModelCreateTests.cs`
- `RepoNotes.Tests/MainWindowViewModelSaveTests.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foram adicionados `RenameSelectedItemCommand` e `DeleteSelectedItemCommand`. O storage renomeia arquivos/pastas no disco com nomes sanitizados e sem sobrescrever itens existentes. A exclusao move itens para `.reponotes-trash` dentro do repositorio e a lixeira fica fora da arvore e da lista de notas. O ViewModel salva alteracoes pendentes antes de renomear/excluir, atualiza a arvore e, se a nota aberta for excluida, abre a proxima nota disponivel. A UI ganhou botoes compactos `Ren` e `Del` na toolbar inferior da sidebar com tooltips claros.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:51 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-20 20:51 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 25 testes aprovados, 0 falhas.

**Pendencias:** Ainda nao ha dialogo de nome para renomear; a acao usa o titulo da nota quando ele foi editado ou um nome automatico com sufixo `renomeado`. Tambem nao ha restaurar da lixeira nem exclusao permanente.

**Riscos tecnicos:** Medio; mover pastas grandes para a lixeira ainda e sincronico, e conflitos externos/permissoes podem exigir feedback mais detalhado. A pasta `.reponotes-trash` precisa continuar excluida de navegacao, busca e preview futuros.

**Proximo passo sugerido:** Implementar um dialogo simples de nome para criacao/renomeacao e, depois, uma visao de lixeira para restaurar itens movidos.

## 2026-05-20 20:56:52 -03:00

**Objetivo da rodada:** Substituir o preview mockado por renderizacao real do Markdown da nota atual, mantendo visual dark consistente.

**Arquivos alterados:**

- `RepoNotes.App/App.axaml.cs`
- `RepoNotes.App/RepoNotes.App.csproj`
- `RepoNotes.App/Services/MarkdownPreviewService.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/ViewModels/MarkdownPreviewBlocks.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Tests/MarkdownPreviewServiceTests.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foi adicionado Markdig para parsear Markdown e um `MarkdownPreviewService` que converte a nota atual em blocos nativos de preview para Avalonia. O painel de preview agora renderiza titulos, paragrafos, listas, checklists simples, code blocks, blockquotes, links como texto com URL e tabelas simples em bloco monoespacado. O preview e atualizado quando `Markdown` ou `Title` mudam, sem mover logica pesada para o ViewModel.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:56 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-20 20:56 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 27 testes aprovados, 0 falhas.

**Pendencias:** A renderizacao ainda e intencionalmente simples: links nao sao clicaveis, tabelas usam texto monoespacado, imagens nao sao renderizadas e nao ha suporte visual avancado para Markdown completo.

**Riscos tecnicos:** Medio-baixo; o preview e nativo e leve, mas documentos grandes podem exigir debounce ou virtualizacao. A evolucao para links clicaveis/imagens deve preservar seguranca local e consistencia visual.

**Proximo passo sugerido:** Testar manualmente notas reais com headings, listas, checklist, code block, blockquote e tabela; depois avaliar links clicaveis e imagens locais.

## 2026-05-20 20:59:43 -03:00

**Objetivo da rodada:** Implementar busca local simples por titulo, caminho e conteudo das notas Markdown, filtrando a arvore da sidebar.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.Tests/MainWindowViewModelSearchTests.cs`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** `SearchText` agora filtra a arvore em memoria usando as notas ja carregadas pelo repositorio, sem reler arquivos a cada tecla. A busca e case-insensitive e considera titulo, nome do arquivo, caminho e conteudo Markdown. Com busca vazia, a arvore normal volta a aparecer. O status mostra a quantidade de resultados encontrados.

**Resultado do dotnet build:** Sucesso em `2026-05-20 20:59 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado final: 0 avisos, 0 erros. Observacao: a primeira tentativa falhou porque uma instancia aberta de `RepoNotes.App.exe` bloqueava o executavel de saida; a instancia foi encerrada e o build final passou.

**Resultado dos testes:** Sucesso em `2026-05-20 20:59 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 30 testes aprovados, 0 falhas.

**Pendencias:** Busca ainda nao tem destaque de termos, debounce, ranking, escopo por pasta/tag, nem busca full-text avancada. Conteudo criptografado futuro deve permanecer fora de indexacao/busca enquanto estiver bloqueado.

**Riscos tecnicos:** Medio-baixo; a busca em memoria e adequada para o MVP, mas repositorios grandes podem exigir indice incremental, cancelamento/debounce e estrategia especifica para conteudo bloqueado por criptografia.

**Proximo passo sugerido:** Testar manualmente buscas por nome de arquivo, pasta e trecho do conteudo; depois avaliar destaque visual de resultados e atalhos de navegacao entre resultados.

## 2026-05-20 21:03:13 -03:00

**Objetivo da rodada:** Adicionar suporte basico a YAML frontmatter nas notas Markdown, preparando metadados sem transformar o app em banco.

**Arquivos alterados:**

- `RepoNotes.Core/Models/NoteItem.cs`
- `RepoNotes.Storage/LocalMarkdownNoteRepository.cs`
- `RepoNotes.Tests/LocalMarkdownNoteRepositoryTests.cs`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O storage agora le frontmatter delimitado por `---` e reconhece os campos `title`, `type`, `tags`, `status`, `created` e `updated`. Notas sem frontmatter continuam funcionando, usando heading Markdown ou nome do arquivo como titulo. Ao salvar, o arquivo e escrito com frontmatter e o campo `updated` e atualizado. `NoteItem` passou a expor metadados basicos para preparar futuras funcionalidades sem adicionar banco.

**Resultado do dotnet build:** Sucesso em `2026-05-20 21:03 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-20 21:03 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 33 testes aprovados, 0 falhas.

**Pendencias:** O parser YAML e simples e cobre apenas o subconjunto necessario ao MVP; ainda nao ha UI dedicada para editar `type`, `tags` ou `status`, nem validacao avancada de formatos.

**Riscos tecnicos:** Medio-baixo; o formato e portavel e local-first, mas evolucoes futuras de entidades tecnicas e criptografia precisam manter compatibilidade com frontmatter existente e evitar indexar metadados/conteudo bloqueado quando houver criptografia.

**Proximo passo sugerido:** Testar manualmente notas com e sem frontmatter e, em rodada futura, criar uma UI compacta para editar tags/status sem expor YAML bruto ao usuario.

## 2026-05-21 08:29:45 -03:00

**Objetivo da rodada:** Fazer um checkpoint tecnico completo apos as ultimas rodadas funcionais, sem implementar nova feature.

**Arquivos alterados:**

- `docs/TASK_LOG.md`

**Status do working tree:** O checkpoint iniciou com alteracoes acidentais em `sample-repository`: `Inbox/Bem-vindo.md` modificado e notas/pastas `Nova nota`/`Nova pasta` criadas por testes manuais do app. Essas sobras foram revertidas/removidas apenas dentro de `sample-repository`. Apos a limpeza e antes do commit do log, `git status --short` nao mostrou alteracoes alem deste registro.

**Resultado do dotnet build:** Sucesso em `2026-05-21 08:29 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-21 08:29 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 33 testes aprovados, 0 falhas.

**Achados da revisao:** `.reponotes-trash` esta excluida de `Reload()` e `BuildTree()` no storage; como busca usa `GetNotes()`/`GetTree()` e preview usa a nota selecionada, a lixeira nao entra na arvore, busca ou preview. Os testes atuais cobrem mover item para `.reponotes-trash`, remover da lista de notas/arvore, notas sem frontmatter, notas com frontmatter e escrita de `updated` ao salvar.

**Pendencias atuais reais:** Templates tecnicos, Lightweight Technical Entities e exportacao simples ainda nao foram implementados. Tambem seguem pendentes UI para editar frontmatter (`type`, `tags`, `status`), dialogo de nome para criacao/renomeacao, restaurar da lixeira, exclusao permanente, links clicaveis/imagens no preview e busca com destaque/debounce/ranking.

**Riscos tecnicos atuais:** A busca em memoria ainda pode precisar de indice/debounce em repositorios grandes. Operacoes de filesystem ainda sao sincronas. O parser YAML e propositalmente simples. Futuras features de criptografia devem impedir indexacao/busca/preview de conteudo bloqueado e manter `.reponotes-trash` fora da navegacao operacional.

**Proximo passo sugerido:** Implementar templates tecnicos como proxima fatia pequena, com `NewFromTemplateCommand`, servico de templates e testes, antes de iniciar entidades tecnicas ou exportacao.

## 2026-05-21 08:35:58 -03:00

**Objetivo da rodada:** Implementar a base funcional de templates tecnicos no RepoNotes, sem criar ainda uma UI avancada de selecao.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.Core/Models/NoteTemplate.cs`
- `RepoNotes.Core/Services/INoteRepository.cs`
- `RepoNotes.Core/Services/INoteTemplateService.cs`
- `RepoNotes.Core/Services/TechnicalNoteTemplateService.cs`
- `RepoNotes.Storage/LocalMarkdownNoteRepository.cs`
- `RepoNotes.Storage/MockNoteRepository.cs`
- `RepoNotes.Tests/LocalMarkdownNoteRepositoryTests.cs`
- `RepoNotes.Tests/MainWindowViewModelSaveTests.cs`
- `RepoNotes.Tests/TechnicalNoteTemplateServiceTests.cs`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foi criada a base de templates tecnicos com modelo `NoteTemplate`, contrato `INoteTemplateService` e implementacao `TechnicalNoteTemplateService`. A lista inicial inclui Nota livre, Runbook, Handover tecnico, Incidente, Script, Prompt, Reuniao, Checklist, Aplicacao e Servidor. A criacao de nova nota passou a usar o template padrao de Nota livre internamente, mantendo a UI atual e preparando `NewFromTemplateCommand` para uma futura tela de escolha. O storage grava notas criadas por template com frontmatter basico e corpo Markdown inicial.

**Resultado do dotnet build:** Sucesso em `2026-05-21 08:34 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-21 08:35 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 38 testes aprovados, 0 falhas.

**Pendencias:** Ainda nao ha UI para escolher template; `NewFromTemplateCommand` usa o fluxo simples preparado para Nota livre. Tambem seguem pendentes edicao visual de frontmatter, personalizacao de templates e criacao a partir dos demais templates pela interface.

**Riscos tecnicos:** Baixo; os templates estao code-backed e simples, mas a futura UI de selecao precisa evitar acoplar nomes/ids diretamente no XAML. Quando entidades tecnicas entrarem, sera necessario alinhar `type`, `tags` e futuros campos de relacionamento sem quebrar notas ja criadas.

**Proximo passo sugerido:** Criar uma UI compacta de escolha de template ligada a `NewFromTemplateCommand`, mantendo Nota livre como padrao e sem adicionar editor visual de templates ainda.

## 2026-05-21 08:39:08 -03:00

**Objetivo da rodada:** Criar uma UI simples e discreta para escolher o template ao criar uma nova nota.

**Arquivos alterados:**

- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Tests/MainWindowViewModelCreateTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A sidebar ganhou um seletor compacto de templates com descricao curta e acao `Novo por template`. O botao `Nova nota` continua criando rapidamente uma Nota livre. `NewFromTemplateCommand` agora usa o template selecionado, cria arquivo Markdown real com nome automatico seguro baseado no tipo do template, atualiza a arvore e abre a nota criada no editor. Foram adicionados testes para criar Runbook por template, preservar frontmatter `type`, evitar sobrescrita com nomes unicos e deixar a nota criada selecionada.

**Resultado do dotnet build:** Sucesso em `2026-05-21 08:39 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-21 08:39 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 40 testes aprovados, 0 falhas.

**Pendencias:** A UI ainda e propositalmente simples: nao ha busca de templates, categorias, preview do template, editor de templates customizados ou persistencia de ultimo template escolhido.

**Riscos tecnicos:** Baixo; o fluxo continua local-first e baseado em Markdown/frontmatter. O principal cuidado futuro e manter o seletor compacto para nao poluir a sidebar e nao acoplar regras de template diretamente no XAML.

**Proximo passo sugerido:** Testar manualmente criacao de Runbook, Handover e Servidor pela UI e depois avaliar uma melhoria leve de UX, como lembrar o ultimo template selecionado.

## 2026-05-21 08:42:35 -03:00

**Objetivo da rodada:** Criar uma UI compacta para visualizar e editar metadados basicos da nota sem exigir edicao manual de YAML frontmatter.

**Arquivos alterados:**

- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Tests/MainWindowViewModelSaveTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O painel direito passou a mostrar uma area compacta de Info da nota com campos editaveis para `type`, `status` e `tags`, alem de campos informativos para criado, atualizado e caminho. Tags sao editadas como texto separado por virgula. Alteracoes nesses metadados marcam a nota como `Alterado` e usam o fluxo normal de salvar para escrever o frontmatter, preservando o Markdown. Foram adicionados testes cobrindo alteracao de type/tags/status, criacao de frontmatter em nota sem frontmatter e preservacao do conteudo Markdown.

**Resultado do dotnet build:** Sucesso em `2026-05-21 08:43 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-21 08:43 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 42 testes aprovados, 0 falhas.

**Pendencias:** A edicao de tags ainda e textual e nao tem chips editaveis, sugestoes, validacao avancada ou busca por tags. O seletor de status e simples e nao ha normalizacao final de valores antigos como `draft` para exibicao capitalizada.

**Riscos tecnicos:** Baixo; o fluxo reutiliza o save existente e o frontmatter ja consolidado. O maior cuidado futuro e manter compatibilidade com notas antigas e evitar que metadados de conteudo criptografado futuro sejam indexados ou exibidos quando bloqueados.

**Proximo passo sugerido:** Testar manualmente notas com e sem frontmatter e depois avaliar uma melhoria pequena para tags em chips editaveis ou filtro por tags.

## 2026-05-21 08:47:01 -03:00

**Objetivo da rodada:** Implementar fluxo basico de lixeira para restaurar itens e excluir permanentemente arquivos/pastas ja movidos para `.reponotes-trash`.

**Arquivos alterados:**

- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Core/Models/TrashItem.cs`
- `RepoNotes.Core/Services/INoteRepository.cs`
- `RepoNotes.Storage/LocalMarkdownNoteRepository.cs`
- `RepoNotes.Storage/MockNoteRepository.cs`
- `RepoNotes.Tests/LocalMarkdownNoteRepositoryTests.cs`
- `RepoNotes.Tests/MainWindowViewModelSaveTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A lixeira ganhou modelo `TrashItem`, metadado local `.trash-metadata.json` para lembrar a origem original e operacoes no repositorio para listar, restaurar, excluir permanentemente e esvaziar `.reponotes-trash`. A sidebar recebeu uma area compacta com seletor de itens da lixeira e botoes `Rest`, `Perm` e `Lim`. Restaurar tenta voltar para o caminho original e usa nome seguro com `restaurado` em caso de conflito. Exclusao permanente e esvaziamento sao limitados a itens dentro de `.reponotes-trash`, mantendo a lixeira fora da arvore principal, busca e preview.

**Resultado do dotnet build:** Sucesso em `2026-05-21 08:47 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-21 08:47 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 47 testes aprovados, 0 falhas.

**Pendencias:** Ainda nao ha modal de confirmacao visual para exclusao permanente/esvaziar lixeira, nem tela dedicada de lixeira com detalhes. A UI atual e intencionalmente compacta e operacional.

**Riscos tecnicos:** Medio-baixo; as operacoes destrutivas estao restringidas a `.reponotes-trash`, mas a falta de confirmacao visual aumenta risco de clique acidental ate a proxima rodada de UX. O metadado de lixeira e simples e pode precisar de migracao se houver historico mais rico no futuro.

**Proximo passo sugerido:** Adicionar confirmacao discreta para `DeletePermanentlyCommand` e `EmptyTrashCommand`, ou uma pequena vista de lixeira com caminho original antes de ampliar outros fluxos de arquivo.

## 2026-05-21 08:51:26 -03:00

**Objetivo da rodada:** Adicionar um dialogo simples para nomear notas/pastas na criacao e renomeacao, substituindo parcialmente nomes automaticos.

**Arquivos alterados:**

- `RepoNotes.App/App.axaml.cs`
- `RepoNotes.App/Services/AvaloniaTextPromptService.cs`
- `RepoNotes.App/Services/ITextPromptService.cs`
- `RepoNotes.App/Services/NullTextPromptService.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.Tests/MainWindowViewModelCreateTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foi criada a abstracao `ITextPromptService` com implementacao real em Avalonia para um dialogo dark simples de entrada de nome e fallback `NullTextPromptService`. Criacao de nota, criacao por template, criacao de pasta e renomeacao agora pedem nome ao usuario quando ha prompt disponivel. Cancelar interrompe a operacao sem criar/renomear. O dialogo valida nome vazio e caracteres invalidos do Windows; o storage continua responsavel por sanitizar e evitar sobrescrita com sufixos seguros. Testes com fake prompt cobrem cancelamento, nome invalido sanitizado, renomeacao valida e conflito sem sobrescrita.

**Resultado do dotnet build:** Sucesso em `2026-05-21 08:52 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-21 08:52 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 50 testes aprovados, 0 falhas.

**Pendencias:** O dialogo ainda e propositalmente simples: nao ha sugestao interativa de nome alternativo quando existe conflito, nem historico de nomes ou validacao visual mais rica. O fallback automatico continua existindo para ambientes sem UI/prompt.

**Riscos tecnicos:** Baixo; o ViewModel permanece testavel por interface e o storage segue como camada final de seguranca contra nomes invalidos e sobrescrita. O principal cuidado futuro e nao transformar prompts simples em framework modal complexo.

**Proximo passo sugerido:** Testar manualmente criacao/renomeacao com nomes validos, cancelamento e conflito; depois avaliar confirmacoes visuais para acoes destrutivas da lixeira.

## 2026-05-21 08:56:22 -03:00

**Objetivo da rodada:** Transformar tags em recurso funcional basico, exibindo tags reais das notas e permitindo filtrar por tag.

**Arquivos alterados:**

- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/ViewModels/TagFilterViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Tests/MainWindowViewModelSearchTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A sidebar deixou de usar tags mockadas e agora lista tags reais lidas do frontmatter das notas carregadas, com contagem por tag. Clicar em uma tag filtra a arvore de notas, `Limpar` remove o filtro e a busca textual combina com o filtro de tag quando ambos estao ativos. O ViewModel atualiza a lista de tags apos criacao, renomeacao, exclusao, restauracao e mudancas de metadados. Itens dentro de `.reponotes-trash` continuam fora da arvore, busca e contagem de tags.

**Resultado do dotnet build:** Sucesso em `2026-05-21 08:56 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-21 08:56 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 55 testes aprovados, 0 falhas.

**Pendencias:** A edicao de tags ainda e textual no painel Info; nao ha autocomplete, hierarquia de tags, chips editaveis ou busca dedicada por tags. Conteudo criptografado futuro ainda precisa bloquear exposicao de tags/metadados quando estiver travado.

**Riscos tecnicos:** Baixo; o filtro usa dados ja carregados em memoria e respeita a exclusao da lixeira. Em repositorios grandes, contagem e filtragem em memoria podem precisar de cache mais estruturado ou indexacao incremental.

**Proximo passo sugerido:** Testar manualmente filtros por tags em notas com frontmatter e depois avaliar chips editaveis ou sugestoes de tags no painel Info.

## 2026-05-21 09:01:42 -03:00

**Objetivo da rodada:** Melhorar a busca funcional com debounce, feedback visual mais claro e destaque simples de resultados.

**Arquivos alterados:**

- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/ViewModels/RepositoryNodeViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Views/MainWindow.axaml.cs`
- `RepoNotes.Tests/MainWindowViewModelSearchTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A busca passou a usar debounce simples antes de aplicar o filtro na arvore, mantendo os arquivos carregados em memoria e sem reler o disco. A sidebar agora mostra uma linha discreta com `Buscando...`, contagem de resultados ou mensagem de resultado vazio, alem de um botao pequeno para limpar a busca. `Ctrl+K` foca o campo de busca e `Esc` limpa a busca quando o campo esta focado. Itens de nota que aparecem como resultado recebem um indicador visual sutil na arvore, e a busca continua combinando com filtro por tag e ignorando `.reponotes-trash`.

**Resultado do dotnet build:** Sucesso em `2026-05-21 09:00 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-21 09:00 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 59 testes aprovados, 0 falhas.

**Pendencias:** O destaque ainda e simples por item de nota, sem realce inline do termo dentro do texto do TreeView. Nao ha ranking, snippets de conteudo, navegacao entre resultados ou indice persistente.

**Riscos tecnicos:** Baixo; o debounce reduz trabalho durante digitacao, mas repositorios grandes ainda podem exigir cache/indexacao incremental no futuro. Conteudo criptografado futuro deve continuar fora de busca, feedback e destaques enquanto estiver bloqueado.

**Proximo passo sugerido:** Testar manualmente `Ctrl+K`, busca sem resultado, limpar com botao/Esc e combinacao com filtro por tag antes de evoluir para snippets ou ranking.

## 2026-05-21 09:07:48 -03:00

**Objetivo da rodada:** Adicionar suporte inicial a links internos entre notas usando sintaxe `[[Nome da Nota]]`.

**Arquivos alterados:**

- `RepoNotes.App/App.axaml.cs`
- `RepoNotes.App/Services/InternalLinkService.cs`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.App/ViewModels/InternalLinkViewModel.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.Tests/InternalLinkServiceTests.cs`
- `RepoNotes.Tests/MainWindowViewModelInternalLinkTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foi criado `InternalLinkService` para detectar links `[[...]]`, resolver por titulo da nota ou nome de arquivo Markdown e identificar links quebrados sem erro. O ViewModel agora expoe `InternalLinks` para a nota atual e permite abrir links resolvidos via comando, salvando alteracoes pendentes antes de navegar. O painel direito ganhou uma lista compacta de links internos com estado `Resolvido` ou `Quebrado`. Links quebrados nao criam notas automaticamente.

**Resultado do dotnet build:** Sucesso em `2026-05-21 09:06 -03:00` usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros.

**Resultado dos testes:** Sucesso em `2026-05-21 09:06 -03:00` usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 65 testes aprovados, 0 falhas.

**Pendencias:** Links internos ainda nao sao renderizados inline dentro dos parágrafos do preview e nao ha backlinks, graph view, aliases, headings anchors ou criacao automatica de notas quebradas. A lista compacta no painel direito e o ponto inicial de navegacao.

**Riscos tecnicos:** Baixo; a resolucao usa apenas notas ja carregadas em memoria. No futuro, renomeacao de notas, aliases e titulos duplicados precisarao de regras mais explicitas para evitar ambiguidades.

**Proximo passo sugerido:** Testar manualmente links resolvidos e quebrados, depois avaliar renderizacao inline no preview ou uma secao de backlinks simples.

## 2026-05-25 10:13:42 -03:00

**Objetivo da rodada:** Fazer uma validacao local completa do estado atual do RepoNotes, confirmar build/testes apos a toolbar Markdown funcional e verificar a configuracao do GitHub Actions CI.

**Comandos executados:**

- `git status --short`
- `git log --oneline -10`
- `git fetch origin --prune`
- `git pull --ff-only`
- `.\.dotnet\dotnet.exe restore RepoNotes.sln`
- `.\.dotnet\dotnet.exe build RepoNotes.sln`
- `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`
- `.\.dotnet\dotnet.exe test RepoNotes.Tests\RepoNotes.Tests.csproj --no-build --filter FullyQualifiedName~MarkdownFormatTests --logger "console;verbosity=normal"`
- `.\.dotnet\dotnet.exe build RepoNotes.sln --no-restore --configuration Release`
- `.\.dotnet\dotnet.exe test RepoNotes.Tests\RepoNotes.Tests.csproj --no-build --configuration Release --verbosity normal`
- `git diff --stat`

**Resultado do restore:** Sucesso usando SDK local `.\.dotnet\dotnet.exe` versao `8.0.421`. Todos os projetos estavam atualizados para restauracao.

**Resultado do build:** Sucesso em Debug usando `.\.dotnet\dotnet.exe build RepoNotes.sln`. Resultado: 0 avisos, 0 erros. Tambem foi validado build Release com `--no-restore --configuration Release`, igualmente com 0 avisos e 0 erros.

**Resultado dos testes:** Sucesso usando `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`. Resultado: 85 testes aprovados, 0 falhas. `MarkdownFormatTests` existe e contem 20 testes; a execucao filtrada passou com 20/20 testes aprovados. O comando equivalente ao CI em Release tambem passou localmente com 85/85 testes aprovados.

**Status do working tree:** Apos `git fetch`, o workspace local estava 5 commits atras de `origin/main`; foi aplicado `git pull --ff-only` para validar o estado real atual. Permanecem dois arquivos nao rastreados em `sample-repository`: `sample-repository/Nova nota.md` e `sample-repository/Novo server.md`. Eles parecem restos de teste manual/criacao de nota, foram listados e nao foram removidos nesta rodada.

**Validacao da toolbar:** Por inspecao, os botoes `B`, `I`, `H1`, `H2`, `H3`, `List`, `Chk`, `Link`, `Code` e `Qt` estao ligados a handlers reais em `MainWindow.axaml.cs`, que chamam `ApplyToolbarFormat(...)` e delegam para `MainWindowViewModel.ApplyMarkdownFormat(...)`. Os testes `MarkdownFormatTests` cobrem bold, italic, headings, lista, checklist, quote, code inline/bloco e link.

**Validacao dos botoes desabilitados:** Por inspecao, `Img`, `Tbl` e `...` estao com `IsEnabled="False"` na toolbar. Os botoes contextuais `Info` e `Tags` tambem estao com `IsEnabled="False"`.

**Validacao da remocao de Notas Recentes:** Por inspecao e busca textual, o bloco mockado `NOTAS RECENTES` e os textos `Configuracao Nginx`, `Ideias para o App` e `Arquitetura em Camadas` nao aparecem mais em `RepoNotes.App/Views/MainWindow.axaml`.

**Validacao do preview Markdown:** Headings, listas, checklist, code blocks, blockquotes e tabelas simples seguem cobertos pelo `MarkdownPreviewService` e testes existentes. Inline code fica distinguivel por crases no texto renderizado. Bold, italic e bold italic ainda nao sao renderizados como rich text visual; o preview preserva marcadores (`**`, `*`) em blocos de texto simples.

**Validacao do CI:** `.github/workflows/ci.yml` existe, usa `actions/setup-dotnet@v4` com `.NET 8.0.x`, executa `dotnet restore RepoNotes.sln`, `dotnet build RepoNotes.sln --no-restore --configuration Release` e `dotnet test RepoNotes.Tests/RepoNotes.Tests.csproj --no-build --configuration Release --verbosity normal`. A configuracao e coerente e o comando equivalente passou localmente. O ultimo workflow remoto disponivel no GitHub Actions (`CI`, run `26263458700`, commit `d5e44d7`) esta com conclusao `failure`; restore e build passaram, o passo `Run tests` falhou com `Process completed with exit code 1`. Os logs detalhados nao ficaram acessiveis pela API publica sem autenticacao; ha tambem aviso de depreciacao futura de Node.js 20 para `actions/checkout@v4` e `actions/setup-dotnet@v4`.

**Pendencias reais:** Implementar rich inline preview usando `TextBlock.Inlines` ou equivalente em Avalonia para bold/italic/bold italic e inline code com estilo visual real. Investigar a falha do CI remoto com logs autenticados; uma hipotese a verificar e divergencia Linux/Windows em testes que usam caminhos com separadores ou comportamento de filesystem. Limpar ou decidir destino dos arquivos nao rastreados em `sample-repository`.

**Riscos tecnicos:** CI remoto falhando reduz confianca em merges mesmo com validacao local verde. A toolbar usa handlers no code-behind para manipular selecao do `TextBox`, o que e aceitavel para integracao visual/editor neste momento, mas deve permanecer pequeno para nao deslocar regras de negocio para a View. Preview inline ainda e textual e pode frustrar a expectativa criada pela toolbar de bold/italic.

**Proximo passo recomendado:** Primeiro corrigir/investigar o CI remoto ate ficar verde; em seguida implementar rich inline preview para bold/italic/inline code sem alterar o fluxo de storage.

## 2026-05-25 10:19:31 -03:00

**Objetivo da rodada:** Validar e corrigir o GitHub Actions CI do RepoNotes ate ele ficar verde.

**Arquivo de workflow analisado:** `.github/workflows/ci.yml`.

**Mudanca feita no CI:** O workflow ja usava .NET 8, fazia `dotnet restore RepoNotes.sln` e `dotnet build RepoNotes.sln --no-restore --configuration Release`. O passo de testes foi ajustado de `dotnet test RepoNotes.Tests/RepoNotes.Tests.csproj --no-build --configuration Release --verbosity normal` para `dotnet test RepoNotes.Tests/RepoNotes.Tests.csproj --configuration Release --verbosity normal`, permitindo que o projeto de testes seja compilado no proprio passo de teste no runner Linux.

**Validacao local:** O comando `dotnet restore RepoNotes.sln` com `dotnet` global falhou porque o SDK global nao esta instalado no Windows local. A validacao foi executada com o SDK local `.\.dotnet\dotnet.exe`: restore da solution passou, build Release da solution passou com 0 avisos e 0 erros, e `.\.dotnet\dotnet.exe test RepoNotes.Tests\RepoNotes.Tests.csproj --configuration Release` passou com 85 testes aprovados, 0 falhas.

**Status do working tree:** Antes do commit, havia a alteracao intencional em `.github/workflows/ci.yml` e arquivos nao rastreados em `sample-repository`: `Nova nota.md`, `Novo server.md` e `Runbooks/Novo application.md`. Esses arquivos de exemplo nao foram alterados, removidos nem incluidos no commit.

**Resultado esperado:** O proximo push deve disparar o GitHub Actions novamente e o passo `Run tests` deve usar o comando mais robusto sem `--no-build`.

**Pendencia de confirmar GitHub Actions verde:** Confirmar o resultado do novo workflow apos o push deste commit. Se ainda falhar, abrir logs autenticados do job para obter a mensagem exata.

**Proximo passo recomendado:** Aguardar o novo run do CI e, se persistir falha somente no GitHub Actions, ajustar os testes para compatibilidade Linux ou obter logs completos via GitHub autenticado.

## 2026-05-25 10:24:18 -03:00

**Objetivo da rodada:** Corrigir a falha persistente do GitHub Actions apos remover `--no-build` do passo de testes.

**Arquivo de workflow analisado:** `.github/workflows/ci.yml`.

**Mudanca feita no CI:** O runner foi alterado de `ubuntu-latest` para `windows-latest`. A razao e que RepoNotes e um aplicativo desktop Windows e os testes/storage atuais usam caminhos internos normalizados com `\`; no runner Linux isso pode divergir do filesystem real e quebrar testes mesmo com build local Windows verde. O passo de teste permanece sem `--no-build`: `dotnet test RepoNotes.Tests/RepoNotes.Tests.csproj --configuration Release --verbosity normal`.

**Resultado esperado:** O CI deve validar no mesmo sistema operacional alvo do MVP e reduzir falsos negativos causados por semantica de caminho Linux/Windows.

**Pendencia de confirmar GitHub Actions verde:** Confirmar o novo run apos push. Se ainda falhar em `windows-latest`, obter logs autenticados do job e corrigir o erro exato.

**Proximo passo recomendado:** Depois de obter CI verde em Windows, decidir se vale criar uma rodada separada para tornar storage/testes cross-platform e voltar a validar tambem em Linux.

## 2026-05-25 10:27:43 -03:00

**Objetivo da rodada:** Registrar validacao local pos-toolbar Markdown funcional.

**Comandos executados registrados:**

- `dotnet restore RepoNotes.sln`
- `dotnet build RepoNotes.sln`
- `dotnet test RepoNotes.sln --no-build`

**Resultado:** Sucesso. A validacao local no Windows confirmou que o build passou e que os testes passaram apos a implementacao da toolbar Markdown funcional.

**Confirmacao do build local:** Build local passou corretamente.

**Confirmacao dos testes:** Testes locais passaram corretamente. `MarkdownFormatTests` passou, validando os comportamentos de formatacao Markdown cobertos pela suite.

**Confirmacao da toolbar Markdown:** A toolbar Markdown funcional foi validada localmente, incluindo os comandos basicos de formatacao conectados ao editor.

**Pendencia principal:** Implementar rich inline preview visual real com `TextBlock.Inlines` ou equivalente em Avalonia.

**Proximo passo recomendado:** Implementar preview inline rico para bold, italic, bold+italic, inline code e link visual.

## 2026-05-25 10:44:29 -03:00

**Objetivo da rodada:** Implementar preview Markdown inline rico real para bold, italic, bold+italic, inline code e links visuais usando `TextBlock.Inlines` ou abordagem equivalente em Avalonia.

**Arquivos alterados:**

- `RepoNotes.App/Controls/MarkdownInlineTextBlock.cs`
- `RepoNotes.App/Services/MarkdownPreviewService.cs`
- `RepoNotes.App/ViewModels/MarkdownPreviewBlocks.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.Tests/MarkdownPreviewServiceTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O preview deixou de reduzir paragrafo, heading, itens de lista e blockquotes a texto simples antes da UI. Foram adicionados runs inline ricos com flags para bold, italic, code e link, e um `MarkdownInlineTextBlock` que transforma esses runs em `Run` nativos do Avalonia. Os DataTemplates do preview agora usam esses inlines para renderizar enfase real, inline code monoespacado/accent e links sublinhados, preservando os blocos existentes de headings, listas, checklists, code blocks, blockquotes e tabelas. Os testes do `MarkdownPreviewService` foram ampliados para cobrir as sintaxes inline e regressao de blocos.

**Resultado do restore:** `dotnet restore RepoNotes.sln` com `dotnet` global falhou porque nao ha SDK global instalado neste Windows. A restauracao equivalente com `.\.dotnet\dotnet.exe restore RepoNotes.sln` passou.

**Resultado do build:** A primeira tentativa com `.\.dotnet\dotnet.exe build RepoNotes.sln` falhou por colisao de nome ao referenciar `TextDecorations.Underline` dentro de um `TextBlock`; foi corrigido com qualificacao completa. O build final passou com 0 avisos e 0 erros.

**Resultado dos testes:** A primeira execucao de `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` encontrou uma regressao no texto fallback de item de lista com bold; o fallback foi alinhado aos inlines limpos. A execucao final passou com 95 testes aprovados, 0 falhas.

**Pendencias:** Links visuais no preview ainda nao sao clicaveis como anchors gerais; links internos continuam tratados pela lista existente no painel direito. Inline code usa monospace/accent, mas nao tem background por-run por limitacao simples do uso atual de `Run`.

**Riscos tecnicos:** Baixo a medio; o preview agora depende de um controle Avalonia pequeno para montar inlines nativos. Deve-se evitar expandir esse controle para virar renderer Markdown completo ou duplicar logica que pertence ao `MarkdownPreviewService`.

**Proximo passo sugerido:** Validar visualmente o preview em notas reais e, em rodada futura, avaliar clique em links externos/internos diretamente no texto se isso for prioritario.

## 2026-05-25 10:52:16 -03:00

**Objetivo da rodada:** Implementar barra da janela customizada/integrada ao visual do RepoNotes, substituindo a title bar padrao por client-side window chrome em Avalonia com controles reais de minimizar, maximizar/restaurar e fechar.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Views/MainWindow.axaml.cs`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A janela passou a usar `ExtendClientAreaToDecorationsHint`, `ExtendClientAreaTitleBarHeightHint` e `WindowDecorations=BorderOnly`, com uma barra integrada compacta de `34px`. A nova barra tem area arrastavel marcada como title bar do chrome Avalonia, contexto leve de repositorio/nota e botoes pequenos de minimizar, maximizar/restaurar e fechar. Os botoes chamam operacoes reais da `Window` no code-behind, mantendo o comportamento visual na View. Estilos dedicados foram adicionados para hover e estado de fechar sem reintroduzir top bar pesada.

**Resultado do restore:** `dotnet restore RepoNotes.sln` com `dotnet` global falhou porque nao ha SDK global instalado neste Windows. A restauracao equivalente com `.\.dotnet\dotnet.exe restore RepoNotes.sln` passou.

**Resultado do build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` passou com 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` passou com 95 testes aprovados, 0 falhas.

**Validacao manual da janela:** O app foi aberto localmente. Via UI Automation foram encontrados apenas os botoes customizados `Minimizar`, `Maximizar ou restaurar` e `Fechar` no chrome da aplicacao. O botao maximizar mudou a janela para `Maximized`, o mesmo botao restaurou para `Normal`, o botao minimizar mudou para `Minimized`, e o botao fechar encerrou o processo. A area integrada foi arrastada por simulacao de mouse e a posicao da janela mudou de `190,183` para `310,233`. O redimensionamento foi validado via `TransformPattern`, com `CanResize=True` e resize de `1600x900` para aproximadamente `1404x811`.

**Pendencias:** Validar visualmente em uso prolongado se a barra integrada esta confortavel em DPI/monitores diferentes. Avaliar se os simbolos dos botoes devem evoluir para icones vetoriais em uma rodada futura.

**Riscos tecnicos:** Medio-baixo; custom chrome depende de comportamento de janela/plataforma do Avalonia no Windows. O codigo de code-behind foi mantido pequeno e restrito a operacoes visuais da janela.

**Proximo passo sugerido:** Usar o app manualmente em 1366x768 e 1600x900 para confirmar densidade e hit areas da barra integrada, depois priorizar polimentos menores ou a proxima feature funcional.

## 2026-06-02 18:00:16 -03:00

**Objetivo da rodada:** Corrigir a experiencia visual do Markdown no preview para que headings, inline formatting, listas, checklists, quotes, code blocks e links aparecam renderizados no painel de preview, mantendo o editor como Markdown puro.

**Arquivos alterados:**

- `RepoNotes.App/Controls/MarkdownInlineTextBlock.cs`
- `RepoNotes.App/ViewModels/MarkdownPreviewBlocks.cs`
- `RepoNotes.Tests/MarkdownPreviewServiceTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Diagnostico:** O painel direito ja usa `PreviewBlocks`, e os DataTemplates de heading, paragraph, list item e quote ja apontavam para `MarkdownInlineTextBlock`. O `MarkdownPreviewService` ja entregava headings, quote e code block sem marcadores Markdown. O ponto mais fragil era o ciclo visual do `MarkdownInlineTextBlock`: ele reconstruia inlines apenas quando a propriedade mudava, podendo ficar dependente do momento de materializacao do template. As checklists tambem ainda usavam marcadores textuais parecidos com Markdown (`[ ]` e `[x]`) em vez de marcadores visuais.

**Resumo das mudancas:** `MarkdownInlineTextBlock` agora reconstrui os `Inlines` ao anexar na arvore visual, garante a colecao de inlines inicializada e limpa o `Text` fallback depois de montar os runs. Os marcadores de lista/checklist passaram a ser visuais (`•`, `☐`, `✓`) em vez de expor `-`, `[ ]` e `[x]`. Os testes do preview foram ampliados para cobrir H1/H2/H3 limpos, bold/italic/bold+italic sem marcadores, inline code sem crases, link com texto/URL, quote sem `>`, code block sem fences e checklist visual.

**Resultado do restore:** `dotnet restore RepoNotes.sln` com `dotnet` global falhou porque nao ha SDK global instalado neste Windows. A restauracao equivalente com `.\.dotnet\dotnet.exe restore RepoNotes.sln` passou.

**Resultado do build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` passou com 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` passou com 100 testes aprovados, 0 falhas.

**Validacao manual:** O app foi aberto localmente e o editor recebeu conteudo de teste via automacao com H1/H2/H3, bold, italic, bold+italic, inline code, link, lista, checklist, quote, code block e tabela. A inspecao do painel de preview mostrou textos limpos como `H1 titulo`, `H2 titulo`, `H3 titulo`, lista com `•`, checklist com `☐` e `✓`, quote sem `>`, e code block `dotnet test` sem fences. O editor continuou sendo Markdown puro.

**Pendencias:** UI Automation confirma texto limpo, mas nao valida visualmente peso/italico/cor por pixel; o comportamento visual e coberto pela estrutura de `MarkdownInlineRun` e pelo `MarkdownInlineTextBlock`. Links seguem visuais, ainda nao clicaveis no texto do preview.

**Riscos tecnicos:** Baixo; a mudanca e localizada no controle de preview inline e nos marcadores visuais. Uso de simbolos Unicode nos marcadores pode precisar de ajuste se alguma fonte/plataforma renderizar diferente.

**Proximo passo sugerido:** Fazer uma revisao visual humana do preview em uma nota real e, depois, avaliar se links externos/internos devem ficar clicaveis diretamente no texto do preview.

## 2026-06-02 18:29:35 -03:00

**Objetivo da rodada:** Tornar o preview Markdown claramente visivel e usavel na area central do app, criando alternancia real entre modo `Editor` e modo `Preview`.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.Tests/MainWindowViewModelPreviewModeTests.cs`
- `docs/UI_GUIDE.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O ViewModel ganhou estado de modo central com `IsEditorMode`, `IsPreviewMode`, `ShowEditorCommand` e `ShowPreviewCommand`, iniciando em Editor e preservando o modo atual ao trocar de nota. A area central ganhou toggle claro `Editor`/`Preview`: em Editor, o TextBox Markdown e a toolbar de formatacao continuam visiveis; em Preview, o TextBox e a toolbar sao escondidos e `PreviewBlocks` renderizados ocupam o workspace central. O painel direito deixou de exibir `PreviewBlocks` e agora fica focado em `Info`, `Links internos` e metadados, evitando a confusao entre preview lateral e preview central.

**Resultado do restore:** `dotnet restore RepoNotes.sln` com `dotnet` global falhou porque nao ha SDK global instalado neste Windows. A restauracao equivalente com `.\.dotnet\dotnet.exe restore RepoNotes.sln` passou.

**Resultado do build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` passou com 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` passou com 105 testes aprovados, 0 falhas.

**Validacao manual:** O app foi aberto localmente. Uma nota foi selecionada e conteudo Markdown de teste foi inserido no editor por automacao. O toggle `Preview` exibiu o preview renderizado no centro com H1/H2/H3, bold/italic/bold italic, inline code, link visual, lista, checklist, quote e code block limpos. A toolbar Markdown ficou escondida em Preview. O painel direito nao mostrou mais cabecalho `Preview`, apenas `Info`/`Links` e metadados. Ao clicar `Editor`, a toolbar voltou a aparecer.

**Pendencias:** O preview central ainda nao torna links clicaveis no proprio texto. O titulo da nota continua editavel mesmo em Preview porque fica na linha de titulo compartilhada; avaliar se deve virar somente leitura em rodada futura.

**Riscos tecnicos:** Baixo; a mudanca e majoritariamente de layout e estado de ViewModel. O principal risco visual e ajustar densidade do preview central apos teste humano em telas menores.

**Proximo passo sugerido:** Fazer uma revisao visual em 1366x768 e 1600x900 para calibrar padding/tipografia do Preview central e decidir se links devem abrir diretamente do texto renderizado.

## 2026-05-22 00:00:00 -03:00

**Objetivo da rodada:** Conectar a toolbar de formatacao Markdown ao editor, corrigir o preview inline de enfase, remover mockup de notas recentes e adicionar testes de formatacao.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Views/MainWindow.axaml.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Services/MarkdownPreviewService.cs`
- `RepoNotes.Tests/MarkdownFormatTests.cs`
- `docs/TASK_LOG.md`

**Resumo das mudancas:**

**Tarefa 1 — Toolbar funcional:** O TextBox do editor recebeu `x:Name="MarkdownEditor"`. Foi adicionado `ApplyMarkdownFormat(text, selStart, selEnd, formatType)` no ViewModel com logica completa para bold, italic, h1/h2/h3 (toggle insert/replace/remove), list, checklist, quote, link e code inline/bloco. O code-behind ganhou `ApplyToolbarFormat(type)` que le a selecao atual do editor, aplica a transformacao e restaura a selecao. Os botoes B, I, H1, H2, H3, List, Chk, Link, Code e Qt foram conectados via `Click` handlers. Os botoes Img, Tbl e "..." receberam `IsEnabled="False"`. Os botoes Info e Tags do context bar tambem receberam `IsEnabled="False"`.

**Tarefa 2 — Preview inline de enfase:** Adicionado o case `EmphasisInline` no `AppendInlineText` do `MarkdownPreviewService`, antes do case `ContainerInline`, para renderizar `*italic*` e `**bold**` no preview com os marcadores correspondentes.

**Tarefa 3 — Remover mockup:** O bloco NOTAS RECENTES (label + tres TextBlocks hardcoded) foi removido da sidebar. Nenhum substituto foi inserido.

**Tarefa 4 — Testes:** Criado `RepoNotes.Tests/MarkdownFormatTests.cs` com 20 testes cobrindo bold com e sem selecao, italic com e sem selecao, h1/h2/h3 toggle (inserir, substituir e remover prefixo), list toggle, checklist toggle, quote toggle, code inline (com e sem selecao) e code bloco multi-linha, link com e sem selecao.

**Resultado do dotnet build:** Nao executado nesta rodada — o SDK .NET nao esta disponivel no ambiente remoto de execucao Linux utilizado. As mudancas sao sincronizadas via push para validacao pelo CI do repositorio.

**Resultado dos testes:** Nao executado por indisponibilidade do SDK. Testes criados com logica verificada manualmente.

**Pendencias:** A toolbar funciona para selecao de texto mas ainda nao ha atalhos de teclado dedicados para cada comando de formatacao. O preview de enfase renderiza os marcadores como texto em vez de aplicar estilo visual real (negrito/italico nativos), pois a renderizacao atual usa TextBlock sem Inlines ricos.

**Riscos tecnicos:** Baixo-medio; a logica de `ApplyMarkdownFormat` e puramente funcional e coberta por testes. O risco residual e que `editor.SelectionEnd` em Avalonia 11 possa ter comportamento distinto de `SelectionStart + SelectionLength` em casos de selecao reversa, mas o uso normal da toolbar nao produz selecoes reversas.

**Proximo passo sugerido:** Executar build e testes no repositorio para validar as 20 novas asercoes. Em seguida, avaliar renderizacao rica de negrito/italico no preview usando `TextBlock` com `Inlines` em vez de texto plano com marcadores.

## 2026-05-22 01:00:00 -03:00

**Objetivo da rodada:** Verificar e confirmar que os itens da rodada anterior estao presentes e corretos no repositorio: `ApplyMarkdownFormat` no ViewModel, handlers na toolbar XAML, remocao de mockups e testes.

**Arquivos alterados:**

- `docs/TASK_LOG.md`

**Resumo das mudancas:** Rodada de verificacao sem alteracao de codigo. Confirmado que todos os 4 itens solicitados ja estavam presentes desde o merge do PR #1:

1. `ApplyMarkdownFormat` existe em `MainWindowViewModel.cs` (public, retorna ValueTuple) com logica completa para bold, italic, h1/h2/h3 toggle, list, checklist, quote, link e code inline/bloco.
2. XAML tem `Click` handlers conectados em todos os botoes funcionais (B, I, H1, H2, H3, List, Chk, Link, Code, Qt) e `IsEnabled="False"` em Img, Tbl, "...", Info e Tags.
3. Bloco NOTAS RECENTES (label + TextBlocks hardcoded) esta removido da sidebar.
4. `RepoNotes.Tests/MarkdownFormatTests.cs` existe com 226 linhas e 20 testes cobrindo todos os formatos.

**Resultado do dotnet build:** Nao executado — SDK .NET indisponivel no ambiente remoto Linux. Build e testes validados pelo CI no merge do PR #1.

**Resultado dos testes:** Nao executado por indisponibilidade do SDK.

**Pendencias:** Nenhuma nova pendencia criada por esta rodada.

**Riscos tecnicos:** Nenhum risco novo; rodada apenas de verificacao.

**Proximo passo sugerido:** Executar `dotnet build` e `dotnet test` localmente no Windows para confirmar os 20 testes passam, depois avaliar renderizacao rica de negrito/italico no preview.

## 2026-05-22 02:00:00 -03:00

**Objetivo da rodada:** Adicionar GitHub Actions CI para validar build e testes automaticamente, substituindo a dependencia de SDK local no ambiente remoto Linux.

**Arquivos alterados:**

- `.github/workflows/ci.yml`
- `docs/CODEX_RULES.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Criado `.github/workflows/ci.yml` que roda em todo push e PR para `main`. O workflow instala .NET 8, restaura dependencias, faz `dotnet build RepoNotes.sln --configuration Release` e roda `dotnet test RepoNotes.Tests/RepoNotes.Tests.csproj --no-build`. O projeto de testes e referenciado diretamente para evitar problemas de runtime headless do Avalonia.Desktop em Linux. O `CODEX_RULES.md` foi atualizado para documentar que em sessoes remotas Linux o CI substitui o build local e que nenhuma rodada pode ser encerrada sem build confirmado.

**Resultado do dotnet build:** Nao executado localmente — CI validara no proximo push.

**Resultado dos testes:** Nao executado localmente — CI validara no proximo push.

**Pendencias:** Confirmar que o primeiro CI verde passa. Se Avalonia.Desktop tiver dependencias nativas que bloqueiem o build no ubuntu, pode ser necessario adicionar pacotes `libx11-dev` ou similar ao workflow.

**Riscos tecnicos:** Baixo-medio; builds Avalonia no Linux geralmente funcionam sem display para compilacao. Se o CI falhar por dependencias nativas do Avalonia, o fix e adicionar um step `apt-get` com os pacotes necessarios.

**Proximo passo sugerido:** Monitorar o primeiro CI no GitHub Actions e corrigir se houver falha de dependencia nativa do Avalonia.

## 2026-06-02 18:45:54 -03:00

**Objetivo da rodada:** Implementar abas funcionais iniciais para notas abertas, preservando o editor Markdown puro, o modo central Editor/Preview e o layout dark atual.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/NoteTabViewModel.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.Tests/MainWindowViewModelSaveTests.cs`
- `RepoNotes.Tests/MainWindowViewModelTabsTests.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Criado `NoteTabViewModel` e adicionada colecao `OpenTabs` no `MainWindowViewModel`, com `ActiveTab`, `HasOpenTabs`, comandos de ativar/fechar aba e fechamento seguro. A selecao de nota agora abre uma nova aba ou ativa a aba existente sem duplicar e sem salvar automaticamente apenas por trocar de nota. Alteracoes ficam isoladas na aba ativa, com indicador `*` por aba. `Ctrl+S`/Salvar salva apenas a aba ativa. Fechar aba suja tenta salvar antes; se falhar, a aba permanece aberta e o status mostra erro. Rename/delete salvam abas afetadas antes de agir, atualizam tabs abertas ou fecham as tabs movidas para lixeira. O XAML substituiu a aba mockada por uma lista compacta de tabs reais no document context bar. O painel central Editor/Preview e o painel direito Info/Links foram preservados.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 115 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** Smoke test local executado com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`; a aplicacao abriu com o XAML novo e foi encerrada sem crash. A validacao fina de abrir, alternar, salvar, fechar, falha de save, rename/delete e preview/links por aba ficou coberta por testes automatizados.

**Status do working tree:** Antes do commit permanecem modificacoes intencionais nos arquivos acima e tres arquivos nao rastreados preexistentes em `sample-repository` (`Nova nota.md`, `Novo server.md`, `Runbooks/Novo application.md`). Esses arquivos de exemplo nao foram alterados nem incluidos no commit.

**Pendencias:** Ainda nao ha drag-and-drop, reorder, pin tabs, restauracao de sessao, menu de contexto ou confirmacao visual/modal para fechar aba com erro. A aba usa titulo vindo do frontmatter quando existir; renomear arquivo atualiza path/seleção, mas nao altera automaticamente o `title` do frontmatter.

**Riscos tecnicos:** Medio-baixo; o estado por aba fica em memoria e depende dos mesmos objetos `NoteItem` carregados pelo repositorio. Fluxos futuros de reload profundo, sessao persistida ou multiplas janelas podem exigir uma camada de documento aberto mais robusta.

**Proximo passo sugerido:** Validar visualmente as tabs em 1366x768 e 1600x900 e, depois, adicionar confirmacao/UX mais clara para fechamento de aba suja com falha de salvamento.

## 2026-06-02 22:48:46 -03:00

**Objetivo da rodada:** Planejar tecnicamente a implementacao futura de Visual Markdown Mode / WYSIWYG-lite sem implementar codigo.

**Arquivos alterados:**

- `docs/WYSIWYG_EDITOR_PLAN.md`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo da decisao:** Criado documento tecnico comparando tres caminhos para edicao visual: editor nativo Avalonia, WebView2/editor web embutido e abordagem hibrida incremental mantendo Markdown editor + preview. A recomendacao registrada e manter Markdown Mode/Preview como baseline, executar uma spike tecnica isolada com WebView2 antes de adicionar qualquer pacote ou dependencia, e seguir com WebView2 apenas se a spike provar viabilidade de runtime, empacotamento, round-trip Markdown, tema dark, MVVM e sincronizacao com abas. Se WebView2 for pesado demais, o caminho alternativo recomendado e WYSIWYG-lite nativo/hibrido, sem prometer WYSIWYG completo.

**Resultado do build:** Not run, documentation-only change.

**Pendencias:** Executar spike isolada para WebView2/editor web antes de qualquer implementacao. Definir como `Markdown Mode`, `Preview` e futuro `Visual Mode` convivem no toggle central. Definir criterio de round-trip Markdown aceitavel, especialmente para frontmatter, links internos, checklists, code blocks e tabelas.

**Riscos tecnicos:** WebView2 pode impactar empacotamento self-contained, CI, bridge JS/.NET, foco/atalhos, tema e sincronizacao de dirty state por aba. Editor nativo Avalonia pode virar um projeto grande de text editor. Abordagem hibrida e mais segura, mas nao entrega WYSIWYG real.

**Proximo passo sugerido:** Criar uma branch/spike pequena para testar WebView2 com um editor Markdown visual carregando e emitindo Markdown de uma nota ativa, sem tocar em storage nem substituir o editor Markdown atual.

## 2026-06-02 23:09:58 -03:00

**Objetivo da rodada:** Implementar o primeiro passo do Markdown Power Editor com modo Split View, exibindo Editor Markdown e Preview visual lado a lado.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/DocumentViewMode.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Views/MainWindow.axaml.cs`
- `RepoNotes.Tests/MainWindowViewModelTabsTests.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/WYSIWYG_EDITOR_PLAN.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O estado central de visualizacao foi expandido de Editor/Preview para `DocumentViewMode` com `Editor`, `Preview` e `Split`. O ViewModel agora expoe `ShowSplitCommand`, `IsSplitMode`, `HasEditorVisible` e `HasPreviewVisible`. O document context bar ganhou o botao compacto `Split`. No modo Split, a area central mostra o TextBox Markdown a esquerda e o preview visual nativo a direita, reutilizando `PreviewBlocks` e o mesmo `MarkdownPreviewService`. A toolbar Markdown permanece visivel em Editor e Split. O code-behind aplica a toolbar no editor visivel, seja o editor normal ou o editor do Split. Testes foram adicionados para modo padrao, alternancia Preview/Split, visibilidades derivadas, preservacao do modo ao trocar de aba e atualizacao de preview ao editar Markdown.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 119 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** Smoke test local executado com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`; a janela abriu visivelmente com o novo layout e foi encerrada sem crash. A validacao fina dos modos Editor/Preview/Split, troca de abas e atualizacao do preview ficou coberta por testes automatizados.

**Pendencias:** Conferir visualmente em uso real os tamanhos do Split em 1366x768 e 1600x900. Ainda nao ha sincronizacao de scroll entre editor e preview, nem divisoria redimensionavel entre os paineis do Split.

**Riscos tecnicos:** Baixo-medio; o Split duplica o TextBox no XAML para manter layouts simples de Editor e Split, entao futuras melhorias de editor devem lembrar de manter ambos os TextBoxes coerentes. A renderizacao do preview continua centralizada em `MarkdownPreviewService`, reduzindo risco de divergencia visual.

**Proximo passo sugerido:** Adicionar sincronizacao leve de scroll ou uma divisoria ajustavel para o Split, depois validar densidade visual em telas menores.

## 2026-06-02 23:25:24 -03:00

**Objetivo da rodada:** Remover sobras locais nao rastreadas antigas do `sample-repository` e adicionar uma divisoria redimensionavel entre editor e preview no modo Split.

**Arquivos untracked encontrados/removidos:** Foram encontrados e removidos somente os tres arquivos especificados: `sample-repository/Nova nota.md`, `sample-repository/Novo server.md` e `sample-repository/Runbooks/Novo application.md`. Nenhum outro arquivo do `sample-repository` foi removido ou alterado.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O modo Split agora usa `GridSplitter` entre o editor Markdown e o preview visual, com colunas editor/splitter/preview e larguras minimas para preservar usabilidade. O splitter recebeu estilo dark discreto com hover em accent. A toolbar continua usando o editor visivel do Split e o preview segue reutilizando `PreviewBlocks` e `MarkdownPreviewService`. A documentacao foi atualizada para registrar o divisor redimensionavel e manter scroll sync como melhoria futura.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 119 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** A aplicacao foi iniciada para validacao interativa, mas a automacao de janela via Computer Use falhou ao capturar a janela com `GetCursorPos failed: Acesso negado. (0x80070005)`. Por isso, nao foi possivel confirmar o drag do splitter via automacao nesta rodada. A compatibilidade XAML do `GridSplitter` foi validada por build e os fluxos de modo Split continuam cobertos pelos testes existentes.

**Pendencias:** Validar manualmente em uso real o arrasto do divisor no Windows. Scroll sync entre editor e preview permanece fora desta rodada.

**Riscos tecnicos:** Baixo; a implementacao usa `GridSplitter` nativo do Avalonia e nao altera ViewModel, storage, tabs ou preview service. O risco residual e apenas ajuste fino visual/min-width em telas menores.

**Proximo passo sugerido:** Fazer validacao manual direta no app em 1366x768 e 1600x900 e, depois, avaliar sincronizacao leve de scroll no Split.

## 2026-06-03 14:06:03 -03:00

**Objetivo da rodada:** Implementar atalhos de teclado para a toolbar Markdown, mantendo o editor Markdown puro e a regra contextual do `Ctrl+K`.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Views/MainWindow.axaml.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O code-behind agora captura atalhos de formatacao Markdown quando um editor Markdown esta focado e reutiliza `ApplyToolbarFormat(type)`, preservando a mesma logica da toolbar. Foram adicionados atalhos para bold, italic, H1/H2/H3, lista, checklist, code, quote e link. `Ctrl+K` ficou contextual: aplica link quando o editor Markdown esta focado e continua focando a busca quando o foco esta fora do editor. A toolbar recebeu tooltips com os atalhos correspondentes. A documentacao foi atualizada para registrar os atalhos como parte da trilha Markdown Power Editor.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 119 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** Smoke test local executado com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`; a janela abriu visivelmente e foi encerrada sem crash. A validacao interativa fina dos atalhos por automacao de UI nao foi executada nesta rodada; as transformacoes Markdown permanecem cobertas por `MarkdownFormatTests`.

**Pendencias:** Validar manualmente no Windows os atalhos em uso real, especialmente `Ctrl+Alt+1/2/3`, `Ctrl+Shift+7/8`, `Ctrl+\`` e a regra contextual do `Ctrl+K` em Editor e Split.

**Riscos tecnicos:** Baixo-medio; os atalhos vivem no code-behind por serem integracao de teclado/seleção da View. O risco residual e conflito de teclado com layout regional ou com comportamento interno do TextBox para algumas combinacoes.

**Proximo passo sugerido:** Fazer uma rodada curta de QA manual dos atalhos e, se estiverem confortaveis, avaliar scroll sync leve no Split.

## 2026-06-03 14:16:11 -03:00

**Objetivo da rodada:** Implementar uma Command Palette simples para o Markdown Power Editor, acessivel por `Ctrl+Shift+P`.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/CommandPaletteActionKind.cs`
- `RepoNotes.App/ViewModels/CommandPaletteItemViewModel.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Views/MainWindow.axaml.cs`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.Tests/MainWindowViewModelTabsTests.cs`
- `RepoNotes.Tests/MarkdownFormatTests.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Criada Command Palette compacta em overlay dark, aberta por `Ctrl+Shift+P`, com busca, estado vazio, selecao por setas, `Enter` para executar e `Esc` para fechar. A palette inclui comandos de modo (`Show Editor`, `Show Preview`, `Show Split`), formatacao Markdown, insercoes (`Insert Table`, `Insert Code Block`, `Insert Callout`) e acoes seguras ja existentes (`Save`, `New Note`, `New Folder`, `Rename`, `Move to Trash`). Comandos que dependem da selecao do TextBox continuam restritos ao code-behind da View e reutilizam `ApplyMarkdownFormat`/`ApplyMarkdownInsertion`. Foram adicionados modelos simples para itens/acoes da palette e testes para lista inicial, filtro, estado vazio, abrir/fechar, execucao de modos e save. As insercoes de tabela, code block e callout ganharam transformacoes puras e testes.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 131 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** Smoke test local executado com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`; a janela abriu visivelmente e foi encerrada sem crash. A validacao interativa fina de `Ctrl+Shift+P`, filtro, `Enter`, `Esc` e execucao de comandos nao foi automatizada nesta rodada devido ao bloqueio anterior da automacao de janela nesta maquina.

**Pendencias:** Validar manualmente no Windows o fluxo completo da palette: abrir com `Ctrl+Shift+P`, buscar `split/table/bold`, executar com `Enter`, fechar com `Esc` e confirmar comportamento em Editor/Split. A palette ainda nao tem fuzzy search complexo, categorias, historico nem extensibilidade por plugins.

**Riscos tecnicos:** Medio-baixo; a palette introduz um pequeno ponto de coordenacao entre ViewModel e code-behind para comandos dependentes de selecao do editor. O risco residual e manter a lista de comandos sincronizada com novas acoes futuras.

**Proximo passo sugerido:** Fazer QA manual da Command Palette e, se aprovada, continuar com auto-continuacao de listas/checklists/quotes ou scroll sync no Split.

## 2026-06-03 14:32:10 -03:00

**Objetivo da rodada:** Implementar paineis laterais colapsaveis/reexpansiveis para liberar area util do editor e do Split View em telas menores.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.Tests/MainWindowViewModelTabsTests.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O ViewModel agora expoe `IsLeftSidebarCollapsed`, `IsRightSidebarCollapsed`, propriedades derivadas de expansao/largura e comandos `ToggleLeftSidebarCommand` e `ToggleRightSidebarCommand`. A grade principal passou a usar colunas laterais `Auto`, com largura expandida atual (`252px` e `326px`) ou rail recolhido de `42px`. A sidebar esquerda e o painel direito ganharam botoes reais de recolher/expandir, mantendo um rail visivel para reabrir. O tema recebeu estilos reutilizaveis para `panel-toggle` e `collapsed-rail`. Foram adicionados testes de ViewModel para estado inicial e alternancia dos dois paineis.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 134 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** Smoke test local executado com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`; a janela abriu e foi encerrada sem crash observado. Os estados de colapso/expansao foram cobertos por testes unitarios de ViewModel. A validacao interativa fina de clique, ganho visual de espaco e comportamento em 1366x768/1600x900 deve ser conferida diretamente no app Windows, pois nao foi feita automacao de UI nesta rodada.

**Pendencias:** Persistir o estado recolhido/expandido entre sessoes continua futuro. Validar manualmente o comportamento visual em 1366x768 e 1600x900, especialmente com Split View ativo.

**Riscos tecnicos:** Baixo; a mudanca e concentrada em layout e estado simples de ViewModel. O risco residual e ajuste fino de largura/descoberta visual dos rails em telas pequenas.

**Proximo passo sugerido:** Fazer QA manual dos paineis colapsaveis em Editor, Preview e Split, depois avaliar persistencia simples do estado visual ou scroll sync do Split.

## 2026-06-03 14:46:07 -03:00

**Objetivo da rodada:** Implementar menus de botao direito para acoes naturais no explorer, nas abas abertas e na lixeira.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/ParameterizedRelayCommand.cs`
- `RepoNotes.App/ViewModels/ParameterizedAsyncRelayCommand.cs`
- `RepoNotes.App/ViewModels/RepositoryNodeViewModel.cs`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Views/MainWindow.axaml.cs`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.Tests/MainWindowViewModelTabsTests.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foram adicionados comandos parametrizados reutilizaveis para acoes que dependem do item clicado. O explorer agora expoe menu contextual com abrir, abrir em aba, nova nota, nova pasta, renomear, mover para lixeira e copiar caminho. As abas abertas ganharam menu com fechar, fechar outras, fechar todas, revelar no explorer e copiar caminho. A lixeira ganhou menu contextual no seletor com restaurar, excluir permanentemente e esvaziar lixeira. O code-behind ficou restrito a selecao por botao direito e integracao com clipboard para `Copy Path`. O tema recebeu estilos dark para `ContextMenu` e `MenuItem`. Testes foram adicionados para fechar outras abas, fechar todas as abas e abrir item do explorer via comando parametrizado.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 137 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** Smoke test local executado com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`; a janela abriu e foi encerrada sem crash observado. Menus visuais nao receberam teste automatizado de UI, entao a abertura real por botao direito ainda deve ser conferida diretamente no app Windows.

**Pendencias:** Confirmar manualmente a abertura dos menus por botao direito no Windows. Confirmacao visual explicita para `Delete Permanently` e `Empty Trash` ainda precisa de uma rodada propria.

**Riscos tecnicos:** Medio-baixo; menus em DataTemplates dependem de bindings para o `DataContext` raiz e devem ser validados no app real. As operacoes destrutivas continuam reaproveitando o fluxo atual da lixeira.

**Proximo passo sugerido:** Fazer QA manual dos context menus e, em seguida, implementar confirmacoes visuais dedicadas para exclusao permanente e esvaziar lixeira.

## 2026-06-03 19:43:19 -03:00

**Objetivo da rodada:** Refinar visualmente as abas de notas e a area superior do editor para reduzir sobreposicao, poluicao e competicao entre abas, breadcrumb e acoes.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A area superior central foi reorganizada em uma grade mais elastica: abas compactas ocupam a esquerda, breadcrumb truncado fica no centro e os controles `Editor`, `Preview`, `Split` e `Salvar` ficam fixos a direita. Os botoes desabilitados `Info` e `Tags` foram removidos da barra superior para reduzir ruido visual. As abas ficaram mais baixas, com largura menor, padding reduzido, titulo truncado, indicador de alteracao discreto e botao de fechar menor/alinhado. O cabecalho do painel direito foi compactado para alinhar `Info`, `Links`, `Previa` e o botao de colapso sem esmagar os textos. Botoes desabilitados da toolbar Markdown receberam estado visual mais apagado.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 137 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual em 1600x900:** Smoke test local executado com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`; a janela abriu e foi encerrada sem crash observado. A validacao visual interativa com 3 notas abertas nao foi automatizada nesta maquina.

**Validacao manual em 1366x768:** Nao foi possivel confirmar visualmente por automacao nesta rodada. A janela mantem `MinWidth` de 1366 e o layout foi ajustado para colunas elasticas, abas compactas e acoes fixas, mas a validacao fina deve ser feita diretamente no app Windows.

**Pendencias:** Validar manualmente com 3 notas abertas em 1600x900 e 1366x768, confirmando truncamento de abas, alinhamento dos modos e do botao salvar, e legibilidade do painel direito.

**Riscos tecnicos:** Baixo; a rodada e visual e nao altera storage, salvamento, tabs funcionais ou preview. O risco residual e ajuste fino de proporcoes em cenarios com muitas abas ou caminhos muito longos.

**Proximo passo sugerido:** Fazer QA visual direto no Windows com varias abas e, se aprovado, seguir para confirmacoes visuais de lixeira ou melhorias de scroll sync no Split.

## 2026-06-03 19:47:15 -03:00

**Objetivo da rodada:** Corrigir o divisor redimensionavel do modo Split para continuar disponivel apos multiplos ajustes.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O Grid do modo Split foi ajustado de `*,6,*` para `*,10,*`, aumentando a area de hit do divisor. O `GridSplitter` passou a usar `ShowsPreview=False`, cursor `SizeWestEast` e estilo com faixa discreta porem clicavel. Os `MinWidth` dos paineis editor/preview foram reduzidos de `300/280` para `240/240` para evitar travamento prematuro em resolucoes menores. A mudanca mantem `ResizeBehavior=PreviousAndNext` e nao altera a funcionalidade Split.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 137 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** Smoke test local executado com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`; a janela abriu e foi encerrada sem crash observado. A validacao especifica de arrastar o divisor multiplas vezes deve ser feita diretamente no app Windows.

**Pendencias:** Confirmar manualmente em 1366x768 e 1600x900 que o divisor permite arrastar para esquerda, direita e novamente sem ficar coberto por editor/preview.

**Riscos tecnicos:** Baixo; a correcao e concentrada no GridSplitter e nos limites minimos dos paineis. O risco residual e ajuste visual fino da largura da hit area.

**Proximo passo sugerido:** Fazer QA manual do Split em 1366x768 e 1600x900; se o divisor estiver estavel, seguir para scroll sync ou confirmacoes de lixeira.

## 2026-06-03 19:56:09 -03:00

**Objetivo da rodada:** Corrigir definitivamente o problema do divisor entre editor e preview no modo Split.

**Diagnostico do divisor:** O `GridSplitter` estava na coluna correta entre editor e preview (`Grid.Column="1"`) e usava `ResizeBehavior="PreviousAndNext"`, `ResizeDirection="Columns"`, `HorizontalAlignment="Stretch"` e `VerticalAlignment="Stretch"`. A rodada anterior ja havia aumentado a coluna para `10px`, aplicado background para hit-test, usado cursor horizontal, reduzido `MinWidth` dos paineis e desligado `ShowsPreview`. Nao havia sobreposicao intencional de `Border`, `TextBox` ou `ScrollViewer` sobre a coluna do splitter, mas o comportamento continuou reportado como instavel apos resize. O risco residual estava no proprio comportamento do `GridSplitter` com colunas star, minimos dos paineis e compressao do grid externo em resolucoes menores. Pela regra de decisao desta rodada, a solucao escolhida foi remover o drag livre visual e substituir por presets estaveis.

**Arquivos alterados:**

- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/Styles/AppTheme.axaml`
- `RepoNotes.Tests/MainWindowViewModelTabsTests.cs`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O modo Split deixou de usar `GridSplitter` arrastavel e passou a expor presets compactos `50/50`, `60/40` e `70/30`. O ViewModel agora fornece `SplitColumnDefinitions`, comandos de preset e estados ativos para a UI. A area central mostra os presets apenas quando Split esta ativo, e o separador central virou uma linha visual estatica com tooltip, evitando parecer uma alca quebrada. Os paineis do Split usam `MinWidth` mais realista (`200px`) para manter usabilidade em 1366x768. Foram adicionados testes para estado inicial dos presets, alternancia entre presets e preservacao do preset ao alternar modos.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros. A primeira tentativa de build falhou porque Avalonia nao aceita binding direto em `Grid.ColumnDefinitions`; a correcao final usa binding em `ColumnDefinition.Width`.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 140 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** Smoke test local executado com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`; a janela abriu e foi encerrada sem crash observado. O fluxo interativo esperado e ativar Split e alternar `50/50`, `60/40` e `70/30` repetidas vezes em 1366x768 e 1600x900, confirmando que editor e preview mudam de largura sem depender de drag.

**Pendencias:** Revalidar visualmente os presets no app Windows. Free-drag pode ser reavaliado no futuro somente se for implementado de forma comprovadamente estavel ou com manipulacao manual controlada.

**Riscos tecnicos:** Baixo; a solucao reduz risco ao remover dependencia do `GridSplitter` instavel. O binding direto de `ColumnDefinitions` foi descartado apos erro de build; a versao final usa `ColumnDefinition.Width` com `GridLength` exposto pela camada App.

**Proximo passo sugerido:** Validar presets do Split em 1366x768 e 1600x900 e, depois, seguir para scroll sync entre editor e preview.

## 2026-06-03 20:04:57 -03:00

**Objetivo da rodada:** Fazer checkpoint tecnico e visual pos-abas/layout e pos-estabilizacao do Split, sem implementar nova funcionalidade.

**Comandos executados:**

- `git status --short`
- `git log --oneline -10`
- `git status --short -- sample-repository`
- `.\.dotnet\dotnet.exe restore RepoNotes.sln`
- `.\.dotnet\dotnet.exe build RepoNotes.sln`
- `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build`
- `git diff --stat`

**Status do working tree:** Limpo no inicio da rodada. `sample-repository` nao apresentou arquivos modificados ou nao rastreados. Apos abrir e fechar o app para validacao visual, o working tree continuou limpo antes desta entrada de log.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 140 testes aprovados, 0 falhas, 0 ignorados.

**Validacao visual em 1600x900:** A janela foi aberta e ajustada para 1600x900. A captura mostrou a custom window bar sem title bar nativa duplicada, abas compactas, breadcrumb truncado, controles `Editor`/`Preview`/`Split`/`Salvar` alinhados sem sobrepor as abas, editor com area util ampla e painel direito `Info`/`Links`/`Previa` legivel. O overlay da automacao ficou sobre parte da regiao superior, mas nao impediu verificar alinhamento geral e ausencia de colapso visual evidente.

**Validacao visual em 1366x768:** A janela foi ajustada para 1366x768. A captura mostrou o modo Split utilizavel, controles de modo/presets/salvar ainda acessiveis, editor e preview lado a lado legiveis, painel direito ainda legivel e status bar visivel. Nao foi observada sobreposicao evidente na area superior nesta resolucao minima.

**Validacao das abas:** Os testes automatizados cobrem abertura de nota em aba, nao duplicacao ao selecionar a mesma nota, preservacao de Markdown sujo ao trocar de aba, salvamento da aba ativa, fechamento de abas sujas com save previo, falha de save sem perda de conteudo e fechamento da ultima aba sem crash. A tentativa de abrir tres notas por coordenadas na UI nao foi confiavel porque a arvore estava parcialmente recolhida e a automacao por clique nao selecionou filhos de forma deterministica; por isso a validacao funcional detalhada de 3 abas fica registrada como pendencia manual direta.

**Validacao do Split:** O modo Split foi ativado visualmente. Como o drag livre foi removido na rodada anterior, a validacao aplicavel agora e alternar presets `50/50`, `60/40` e `70/30`, nao arrastar divisor. A captura mostrou editor e preview lado a lado, separador visual estatico e presets visiveis. Testes cobrem estado inicial dos presets, troca de presets e preservacao do preset ao alternar modos.

**Validacao dos modos Editor/Preview/Split:** Split foi validado visualmente. A troca de modos e coberta por testes de ViewModel (`ShowEditor`, `ShowPreview`, `ShowSplit`) e pela Command Palette. A alternancia visual por coordenadas ficou limitada pelo overlay da automacao na regiao superior, entao a validacao interativa fina de clique nos tres modos continua recomendada no app real.

**Validacao da toolbar Markdown:** A toolbar apareceu visivelmente compacta e alinhada em Editor/Split. As transformacoes de B, I, H1/H2/H3, List, Chk, Link, Code e Qt sao cobertas por `MarkdownFormatTests` e pelo code-behind existente. Nao foram aplicadas edicoes manuais nesta rodada para evitar sujar notas do `sample-repository`.

**Validacao do preview:** O preview no Split renderizou titulo, paragrafo e lista visualmente no painel central direito. A renderizacao rich inline de bold, italic, bold italic, inline code e links permanece coberta por testes de `MarkdownPreviewService`; nao foi criada nota manual nova nesta rodada.

**Validacao dos paineis laterais:** A sidebar esquerda e o painel direito apareceram legiveis em 1600x900 e 1366x768. A tentativa de colapso/expansao por coordenadas abriu acidentalmente o seletor de repositorio; o dialogo foi cancelado sem selecionar pasta e sem alterar arquivos. Os estados de colapso/expansao continuam cobertos por testes de ViewModel.

**Problemas encontrados:** A automacao visual por coordenadas e sensivel ao overlay de controle e nao substitui QA manual completo para abrir 3 abas, trocar modos por clique e colapsar paineis com precisao. A tarefa ainda mencionava arrastar o divisor do Split, mas o produto agora usa presets estaveis em vez de drag livre; a validacao foi adaptada a essa decisao registrada.

**Pendencias reais:** Fazer QA manual direto no Windows para: abrir 3 notas reais pela arvore, confirmar truncamento de titulos longos, clicar nos tres modos, alternar presets do Split repetidamente, colapsar/expandir laterais por clique e validar toolbar com edicoes descartaveis. Implementar confirmacoes visuais para `Delete Permanently` e `Empty Trash` continua pendente.

**Riscos tecnicos:** Baixo para build/testes e medio-baixo para layout visual: os testes estao verdes, mas a validacao visual automatizada nao cobre todos os cenarios de clique e resolucao como uma sessao humana completa. O Split esta mais previsivel por usar presets, mas ainda precisa de QA manual de ergonomia.

**Proximo passo recomendado:** Fazer uma rodada curta de QA manual assistida no Windows para abas/multiplas notas e, em seguida, implementar confirmacoes visuais para exclusao permanente/esvaziar lixeira.

## 2026-06-03 20:13:34 -03:00

**Objetivo da rodada:** Refinar a area superior do editor para remover duplicacao visual do nome/caminho da nota e ganhar area util.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A area central passou de quatro linhas para tres: abas/acoes, toolbar Markdown e conteudo. Foi removido o titulo grande editavel duplicado acima do editor, removido o breadcrumb/caminho competitivo ao lado das abas e removido o caminho da barra integrada da janela. O nome da nota permanece como identidade primaria na aba, com caminho completo acessivel no tooltip da aba, no painel Info e na status bar. A toolbar Markdown subiu para ficar logo abaixo das abas, aumentando a area vertical do editor.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 140 testes aprovados, 0 falhas, 0 ignorados.

**Validacao manual:** App aberto localmente com `.\.dotnet\dotnet.exe run --project RepoNotes.App --no-build`. A captura visual confirmou que o titulo grande duplicado nao aparece mais, a toolbar Markdown ficou imediatamente abaixo da linha de abas, o nome da nota continua visivel na aba ativa, `Editor`/`Preview`/`Split`/`Salvar` permanecem alinhados a direita e o painel Info ainda mostra caminho e metadados. Durante a validacao apareceu um arquivo nao rastreado `sample-repository/Inbox/Teste.md`, identificado como nota automatica de teste, e ele foi removido isoladamente.

**Pendencias:** Validar manualmente com 3 notas abertas e titulos longos para confirmar truncamento das abas em uso real. A edicao do titulo/frontmatter agora fica principalmente pelo painel Info/metadata e pelo conteudo/frontmatter, sem campo grande dedicado no centro.

**Riscos tecnicos:** Baixo; a rodada e visual e remove XAML duplicado sem alterar storage, salvamento, tabs, preview ou ViewModel. O risco residual e algum usuario sentir falta do campo grande de edicao de titulo, mas a decisao de produto desta rodada prioriza a aba como identidade primaria.

**Proximo passo sugerido:** Fazer QA visual com 3 abas/titulos longos e seguir para confirmacoes visuais de acoes destrutivas da lixeira.

## 2026-06-03 20:42:00 -03:00

**Objetivo da rodada:** Corrigir erros ao excluir permanentemente itens da lixeira e ao esvaziar `.reponotes-trash`, mantendo a lixeira fora da arvore, busca, tags e preview.

**Diagnostico:** A UI possui dois caminhos para exclusao permanente: o botao compacto chama `DeletePermanentlyCommand`, enquanto o menu de contexto da lixeira chama `DeleteTrashItemPermanentlyCommand` com parametro. Ambos convergem para `DeletePermanently()` no ViewModel. O esvaziamento usa `EmptyTrashCommand`. O erro provavel vinha de dois pontos combinados: o `ComboBox.ContextMenu` da lixeira usava bindings relativos (`DeleteTrashItemPermanentlyCommand`, `SelectedTrashItem`) que podiam nao resolver explicitamente para o ViewModel, e o storage tratava item de lixeira ja ausente como excecao, deixando selecao/itens stale apos cliques repetidos ou alteracoes externas. Tambem faltava limpar `SelectedTrashItem` apos excluir/restaurar/esvaziar.

**Arquivos alterados:**

- `RepoNotes.App/Views/MainWindow.axaml`
- `RepoNotes.App/ViewModels/MainWindowViewModel.cs`
- `RepoNotes.Storage/LocalMarkdownNoteRepository.cs`
- `RepoNotes.Tests/LocalMarkdownNoteRepositoryTests.cs`
- `RepoNotes.Tests/MainWindowViewModelTabsTests.cs`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Os comandos do context menu da lixeira agora apontam explicitamente para `#Root.DataContext`. `DeletePermanently` no storage ficou idempotente para item ja ausente dentro da lixeira, continua bloqueando caminhos fora de `.reponotes-trash`, normaliza caminho relativo para metadados e remove entradas aninhadas ao excluir pastas. `EmptyTrash` agora cria/recria a pasta e metadado quando necessario, enumera uma copia dos itens e limpa metadados. O ViewModel recarrega `TrashItems`, limpa `SelectedTrashItem` apos restaurar/excluir/esvaziar e exibe status amigavel para selecao nula.

**Resultado do restore:** `.\.dotnet\dotnet.exe restore RepoNotes.sln` executado com sucesso; todos os projetos estavam atualizados para restauracao.

**Resultado do dotnet build:** `.\.dotnet\dotnet.exe build RepoNotes.sln` executado com sucesso, 0 avisos e 0 erros.

**Resultado dos testes:** `.\.dotnet\dotnet.exe test RepoNotes.sln --no-build` executado com sucesso: 148 testes aprovados, 0 falhas, 0 ignorados.

**Status do working tree:** Antes da correcao ja existiam alteracoes locais fora do escopo em `sample-repository`: `sample-repository/Projetos/Roadmap.md` deletado, `sample-repository/.reponotes-trash/` nao rastreado e `sample-repository/Nova nota.md` nao rastreado. Esses itens nao foram removidos nem incluidos no commit desta rodada.

**Validacao da lixeira:** Testes adicionados cobrem exclusao permanente de arquivo, exclusao permanente de pasta, selecao nula sem crash, item stale/ja inexistente sem crash, rejeicao de caminho absoluto fora da lixeira, esvaziamento com arquivos, esvaziamento com pastas, lixeira vazia sem crash, limpeza de `TrashItems`/`SelectedTrashItem` e limpeza de `.trash-metadata.json`. Testes existentes continuam cobrindo exclusao da arvore, busca e tags.

**Pendencias:** Ainda falta confirmacao visual antes de `Delete Permanently` e `Empty Trash`. A validacao manual interativa no app Windows deve ser refeita depois que a sujeira atual de `sample-repository` for decidida/limpa.

**Riscos tecnicos:** Baixo-medio; a protecao contra caminhos fora da lixeira foi reforcada, mas operacoes destrutivas ainda sao sincronas e dependem de permissoes do filesystem. A lixeira usa metadado JSON simples e pode precisar de migracao se ganhar historico mais rico.

**Proximo passo sugerido:** Implementar confirmacoes visuais compactas para exclusao permanente/esvaziar lixeira e limpar ou restaurar conscientemente os artefatos locais atuais do `sample-repository`.

## 2026-06-03 20:50:00 -03:00

**Objetivo da rodada:** Criar uma spike visual isolada em React + TypeScript + Vite para avaliar uma possivel interface desktop web/Tauri para o RepoNotes, sem migrar o app Avalonia e sem tocar em storage/logica real.

**Pasta criada:** `spikes/tauri-react-visual/`

**Stack usada:** React 19, TypeScript, Vite e CSS moderno. Tauri nao foi inicializado nesta rodada; a spike ficou como React/Vite exibivel e preparada para uma rodada posterior de empacotamento Tauri.

**Comandos executados:**

- `npm install`
- `npm run build`
- `npm run dev -- --port 5173`
- Validacao no navegador em `http://127.0.0.1:5173/`
- `git status --short`
- `git diff --stat`

**Comandos para rodar:** `cd spikes/tauri-react-visual`, depois `npm install` e `npm run dev`. URL local validada: `http://127.0.0.1:5173/`.

**Resumo visual:** A spike implementa um shell dark premium com barra superior compacta, sidebar de repositorio/busca/explorer/tags/lixeira, abas compactas, controles `Editor`/`Preview`/`Split`, botao `Save`, presets Split `50/50`, `60/40` e `70/30`, editor Markdown mockado com numeros de linha, toolbar Markdown compacta, preview visual renderizado manualmente e painel direito `Info`/`Links`.

**O que ficou mockado:** Dados do repositorio `infra-docs`, arvore IBX/LA4/Applications/LibreNMS, abas, conteudo Markdown, preview, metadados, links internos, tags, lixeira, status salvo, controles de janela e acoes de salvar/criar/buscar. Nao ha filesystem real, backend, persistencia, parser Markdown real ou comandos Tauri.

**Resultado do build/dev server:** `npm install` executado com sucesso, 0 vulnerabilidades. `npm run build` executado com sucesso apos adicionar `@types/react` e `@types/react-dom`. `npm run dev -- --port 5173` iniciou Vite em `http://127.0.0.1:5173/`.

**Validacao visual:** A pagina abriu no navegador embutido e o DOM confirmou presenca de `RepoNotes`, `infra-docs`, aba ativa `00-Overview.md`, controles `Editor`/`Preview`/`Split`/`Save`, presets Split, sidebar/lixeira, painel Info/Links e conteudo de editor/preview. A validacao efetiva ocorreu em viewport reportado como 1366x768 sem overflow horizontal/vertical. O pedido de viewport 1600x900 no navegador embutido nao foi refletido pela sessao, que permaneceu em 1366px de largura; screenshots tambem falharam por timeout no comando de captura do navegador.

**Status do working tree:** Permanecem alteracoes locais fora do escopo em `sample-repository` (`Projetos/Roadmap.md` deletado, `.reponotes-trash/` nao rastreado e `Nova nota.md` nao rastreado). A spike nao alterou codigo Avalonia nem storage real.

**Pendencias:** Rodar a spike em uma janela real de navegador a 1600x900 para captura visual, criar wrapper Tauri se a direcao for aprovada, medir startup/memoria, e comparar empacotamento Windows contra o app Avalonia.

**Riscos:** Baixo para a spike por ser isolada. Medio para qualquer decisao futura de migracao: Tauri exigiria ponte de filesystem/backend, nova estrategia de testes, empacotamento Windows e revalidacao de atalhos/editor/local-first.

**Recomendacao preliminar:** React/Vite facilita iteracao visual, densidade e responsividade mais rapidamente que XAML para prototipagem. Ainda nao justifica migracao; a melhor leitura e usar a spike como laboratorio visual e so avaliar Tauri apos uma spike tecnica de empacotamento e acesso local a arquivos.

**Proximo passo sugerido:** Validar a spike manualmente em 1600x900 e 1366x768 em navegador visivel, depois decidir se vale uma segunda spike Tauri minima com janela nativa e bridge read-only para repositorio local.

## 2026-06-04 10:32:57 -03:00

**Objetivo da rodada:** Registrar o plano completo do RepoNotes vNext, incluindo direcao React/Vite/TypeScript, Visual Markdown Editor-first, features funcionais de diferenciacao e Password Protected Notes.

**Decisao de plano vNext:** RepoNotes vNext sera reconstruido como um app React/Vite/TypeScript limpo, com Tauri posteriormente. O Avalonia fica como referencia funcional/legado. A spike visual antiga fica como referencia, nao como base de produto. O vNext sera Visual Markdown Editor-first, com Markdown limpo como formato salvo/exportavel, autosave com debounce, sem Preview separado, sem Split como modo principal inicial e sem botao Save principal.

**Features adicionadas ao plano:** Application Documentation Pack, Documentation Health Score, Review Cycle / Expiration, RACI Builder, Runbook Builder, Handover Pack, Broken Links and Orphan Documentation, Lightweight Technical Entities, Confluence-ready Export e Password Protected Notes.

**Arquivos alterados:**

- `docs/PRODUCT.md`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/ARCHITECTURE_VNEXT.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O produto foi reposicionado como editor local-first de documentacao tecnica operacional, focado em aplicacoes, ambientes, runbooks, RACI, handover, governanca e exportacao, nao como concorrente generico do Obsidian. O roadmap foi reorganizado em 19 fases vNext, de direcao visual ate empacotamento Windows. O guia de UI agora registra a direcao definitiva: visual dark premium aprovado, editor visual-first, Info fechado por padrao, tags no Info, rail de icones, lixeira visivel, autosave e maximo espaco de escrita. A arquitetura vNext documenta frontend React/Vite, Tauri futuro, Markdown como formato principal, estrategia de editor visual, autosave, filesystem local, lixeira, frontmatter e notas protegidas por senha.

**Resultado do build/test:** Not run, documentation-only change.

**Riscos:** Tentar migrar tudo de uma vez pode gerar uma reescrita fragil. Promover a spike visual diretamente pode carregar mockups e atalhos ruins para produto. WYSIWYG/Visual Markdown pode gerar Markdown ruim se a biblioteca for mal escolhida. Password Protected Notes complica busca, autosave, exportacao, links internos, lixeira e health score. O filesystem via Tauri precisa de contrato limpo. O escopo pode crescer demais se as fases nao forem respeitadas.

**Proximo passo recomendado:** Criar `apps/reponotes-vnext/` limpo com React/Vite/TypeScript e layout baseado na imagem definitiva, usando mock data e sem filesystem real.

## 2026-06-04 10:47:26 -03:00

**Objetivo da rodada:** Criar a base limpa do RepoNotes vNext em `apps/reponotes-vnext/` usando React, Vite e TypeScript, com layout inicial baseado na direcao visual definitiva, mock data e sem filesystem real.

**Pasta criada:** `apps/reponotes-vnext/`

**Stack usada:** React 19, Vite 7, TypeScript 5 e CSS moderno. Tauri, backend, filesystem, persistencia e biblioteca WYSIWYG real nao foram adicionados nesta rodada.

**Arquivos principais criados:** `package.json`, `vite.config.ts`, `tsconfig.json`, `index.html`, `README.md`, `src/main.tsx`, `src/app/App.tsx`, componentes em `src/components/layout`, `src/components/tabs`, `src/components/editor`, `src/components/common`, mock data em `src/data/mockRepository.ts`, tipos em `src/types/reponotes.ts` e estilos em `src/styles/theme.css`/`src/styles/globals.css`.

**Resumo visual:** A UI inicial possui top bar compacta com `RepoNotes vNext`, command box e controles de janela mockados; rail esquerdo de 48px com icones; sidebar apenas para navegacao com repositorio `infra-docs`, busca, arvore mockada e lixeira visivel; abas compactas com estado ativo; workspace central com toolbar e Visual Markdown Editor mockado; InfoPanel fechado por padrao com botao discreto para abrir; tags dentro do InfoPanel; status bar com `All changes saved locally`, repo/branch, palavras e zoom.

**Comandos executados:**

- `npm install`
- `npm run build`
- `npm run dev -- --port 5174`
- Validacao local de `http://127.0.0.1:5174/`
- Validacao no navegador embutido da abertura da UI, troca de aba e abertura do InfoPanel
- `git status --short`
- `git diff --stat`

**Resultado do build:** `npm run build` executado com sucesso; TypeScript e Vite build passaram.

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; `Invoke-WebRequest` retornou HTTP 200. No navegador embutido, a UI abriu, exibiu brand/repo/editor/status/lixeira, permitiu trocar para a aba `Application Documentation Pack.md` e abrir o InfoPanel com tags/actions.

**Pendencias:** Implementar uma spike real de Visual Markdown Editor interativo, definir biblioteca/estrategia de Markdown round-trip, adicionar Tauri shell, criar contrato de filesystem local e substituir mock data por servicos reais em fases futuras.

**Riscos:** A UI ainda e mockada e pode esconder complexidade real de editor, autosave, filesystem e protected notes. O vNext nao deve copiar a spike antiga nem reimplementar todas as features do Avalonia de uma vez.

**Proximo passo recomendado:** Iniciar a `Interactive Visual Markdown Editor spike` dentro do vNext, comparando candidatos de editor visual e qualidade de Markdown gerado antes de adicionar Tauri ou filesystem real.

## 2026-06-04 11:03:04 -03:00

**Objetivo da rodada:** Criar uma spike controlada de Visual Markdown Editor dentro do RepoNotes vNext, avaliando edicao visual/WYSIWYG-lite sem perder a estrategia Markdown-first.

**Arquivos alterados:**

- `apps/reponotes-vnext/package.json`
- `apps/reponotes-vnext/package-lock.json`
- `apps/reponotes-vnext/README.md`
- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/data/mockRepository.ts`
- `apps/reponotes-vnext/src/types/reponotes.ts`
- `apps/reponotes-vnext/src/styles/globals.css`
- `docs/ARCHITECTURE_VNEXT.md`
- `docs/ROADMAP.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A superficie mockada do editor vNext foi substituida por uma spike real usando `@milkdown/crepe`. O editor carrega Markdown inicial em memoria, renderiza conteudo como documento visual editavel e atualiza um painel discreto `Markdown gerado` com o Markdown produzido pelo editor. O conteudo inicial cobre heading, blockquote, tabela, checklist e code fence. A documentacao agora registra que Milkdown/Crepe e uma hipotese tecnica promissora, mas ainda nao uma decisao final de arquitetura.

**Resultado do restore/install:** `npm install` em `apps/reponotes-vnext` executado com sucesso; 267 pacotes auditados, 0 vulnerabilidades.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. Vite/TypeScript passaram. O build emitiu aviso de chunk grande: um chunk minificado de aproximadamente `1,685.81 kB` (`532.65 kB` gzip), causado pela pilha Crepe/CodeMirror; isso deve ser avaliado antes de promover a spike.

**Validacao local/dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; `Invoke-WebRequest` retornou HTTP 200. O navegador embutido confirmou `Visual Markdown Editor Spike`, DOM `.ProseMirror` do Milkdown, painel `Markdown gerado`, estado `Markdown round-trip ativo`, tabela, checklist e heading. Tambem foi feita edicao manual automatizada no editor visual; o texto inserido apareceu no documento e no painel de Markdown gerado.

**Status do working tree:** A rodada deixou alteracoes intencionais apenas em `apps/reponotes-vnext` e docs. Permanecem itens locais antigos fora do escopo em `sample-repository`: `sample-repository/Projetos/Roadmap.md` deletado, `sample-repository/.reponotes-trash/` nao rastreado e `sample-repository/Nova nota.md` nao rastreado. Eles nao foram alterados nem incluidos no commit.

**Pendencias:** Validar frontmatter como fronteira fora do corpo visual, testar documentos tecnicos maiores, medir round-trip de tabelas/checklists/links/code blocks, avaliar code splitting ou configuracao Milkdown mais enxuta, medir startup/bundle em futura shell Tauri e definir se Crepe e editor final ou apenas referencia de spike.

**Riscos tecnicos:** Bundle inicial pesado pode afetar startup desktop. O editor ainda esta em memoria e nao prova autosave, filesystem, tabs reais, protected notes, lixeira, exportacao ou metadata panel. A integracao direta com Crepe e simples, mas precisa de um contrato mais claro para estado por nota/aba antes de virar arquitetura.

**Proximo passo sugerido:** Fazer uma segunda spike curta focada em Markdown round-trip: frontmatter preservado fora do editor visual, edicoes em tabelas/checklists/links/code blocks e medicao de bundle/startup com lazy loading.

## 2026-06-04 11:14:55 -03:00

**Objetivo da rodada:** Limpar a UI principal do Visual Markdown Editor no vNext, removendo elementos de spike/debug visiveis ao usuario.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/styles/globals.css`
- `apps/reponotes-vnext/README.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A area central deixou de exibir `Visual Markdown Editor Spike`, texto explicativo sobre Milkdown/Crepe, badge `Markdown round-trip ativo` e painel `Markdown gerado`. O titulo principal agora usa o titulo real da nota mockada, o editor visual ocupa a maior parte da area central e o Markdown gerado continua disponivel tecnicamente em memoria/Crepe, sem aparecer na UI principal. O README registra que a experiencia principal foi limpa para parecer produto e que o debug Markdown nao fica visivel por padrao.

**Resultado do install:** `npm install` em `apps/reponotes-vnext` executado com sucesso; dependencias atualizadas, 267 pacotes auditados, 0 vulnerabilidades.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso de chunk grande do Crepe/CodeMirror continua presente: chunk principal de aproximadamente `1,685.25 kB` minificado (`532.40 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; HTTP 200 confirmado. O navegador embutido confirmou que `Application Documentation Pack` aparece como titulo, `.ProseMirror` do Milkdown monta, e os textos `Visual Markdown Editor Spike`, `Markdown round-trip ativo`, `Markdown gerado` e a explicacao tecnica sobre Milkdown/Crepe nao aparecem mais na UI.

**Pendencias:** Avaliar se o acesso interno ao Markdown gerado deve virar painel dev gated por flag, ferramenta de teste ou API de autosave. Continuam pendentes frontmatter fora do corpo visual, round-trip com documentos maiores e reducao/lazy loading do bundle do editor.

**Riscos tecnicos:** Baixo para UI; a mudanca remove somente diagnosticos visiveis. O risco principal continua sendo de arquitetura: Crepe ainda precisa de validacao de bundle/startup, frontmatter e persistencia antes de ser promovido para editor final.

**Proximo passo sugerido:** Implementar uma validacao de round-trip controlada para frontmatter + tabelas/checklists/links/code blocks e decidir como expor o Markdown gerado para autosave sem poluir a UI.

## 2026-06-04 11:22:34 -03:00

**Objetivo da rodada:** Implementar a Activity Bar / Left Rail do RepoNotes vNext com icones reais, baseada no mockup aprovado.

**Dependencia adicionada:** `lucide-react`.

**Arquivos alterados:**

- `apps/reponotes-vnext/package.json`
- `apps/reponotes-vnext/package-lock.json`
- `apps/reponotes-vnext/src/components/common/IconButton.tsx`
- `apps/reponotes-vnext/src/components/layout/AppShell.tsx`
- `apps/reponotes-vnext/src/components/layout/LeftRail.tsx`
- `apps/reponotes-vnext/src/components/layout/RepositorySidebar.tsx`
- `apps/reponotes-vnext/src/styles/globals.css`
- `apps/reponotes-vnext/src/types/reponotes.ts`
- `apps/reponotes-vnext/README.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O rail esquerdo passou de letras mockadas para icones reais do `lucide-react`, com logo pequeno `R` no topo, grupo principal de workspace e grupo inferior de utilitarios. Repository fica ativo por padrao. Search, Trash e Settings mudam o estado visual local e atualizam o cabecalho contextual da sidebar. Links/Graph, Tags, Tasks, Templates, Entities e Profile aparecem como placeholders futuros desabilitados, com opacidade reduzida e title claro. O rail foi ajustado para `52px`, com hover sutil e estado ativo roxo/azul.

**Resultado do install:** `npm install` em `apps/reponotes-vnext` executado com sucesso; 268 pacotes auditados, 0 vulnerabilidades.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso de chunk grande do Crepe/CodeMirror continua presente: chunk principal de aproximadamente `1,690.59 kB` minificado (`534.36 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; HTTP 200 confirmado. No navegador embutido, o DOM confirmou 10 botoes no rail com SVG real, logo `R`, grid com coluna de `52px`, Repository ativo por padrao e itens futuros desabilitados. Cliques em Search, Trash e Settings trocaram o destaque ativo e atualizaram o contexto da sidebar.

**Pendencias:** Implementar telas/estados reais para Search, Trash e Settings em rodadas futuras. Definir se os itens futuros devem permanecer visiveis desabilitados ou ficar escondidos ate terem funcao real. O bundle do editor visual ainda precisa de lazy loading/code splitting.

**Proximo passo sugerido:** Criar o layout real da area Search ou Trash dentro da sidebar, mantendo o rail como controlador visual de workspace sem implementar filesystem/Tauri ainda.

## 2026-06-04 11:29:51 -03:00

**Objetivo da rodada:** Simplificar a area central do Visual Markdown Editor no RepoNotes vNext, removendo titulo/metadata duplicados acima do editor e reduzindo a sensacao de janela dentro da janela.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/styles/globals.css`
- `apps/reponotes-vnext/README.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O header externo do editor foi removido. A area central nao mostra mais `Application Documentation Pack`, `type`, `status` ou `owner` acima do Milkdown. O titulo visivel agora fica como primeiro bloco editavel do proprio documento. O CSS removeu bordas, radius, padding e shadow da moldura externa do editor e do shell Milkdown, deixando a superficie visual mais integrada ao workspace. O primeiro `h1` dentro do ProseMirror recebeu escala de titulo de documento, enquanto a toolbar/tabs permanecem como chrome compacto.

**Resultado do install:** `npm install` em `apps/reponotes-vnext` executado com sucesso; dependencias atualizadas, 268 pacotes auditados, 0 vulnerabilidades.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso conhecido de chunk grande do Crepe/CodeMirror continua presente: chunk principal de aproximadamente `1,690.29 kB` minificado (`534.26 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; HTTP 200 confirmado.

**Validacao visual/manual:** No navegador embutido, o DOM confirmou que `.document-editor-header` nao existe mais, nao ha badges/owner dentro de `.visual-editor`, `.milkdown-shell .ProseMirror` montou com `contenteditable="true"`, o unico `h1` central e `Application Documentation Pack` como bloco do documento, existem 3 abas, o botao de Info continua presente e as bordas de `.visual-editor` e `.milkdown-shell` ficaram em `0px`. Os textos de spike/debug continuam ausentes.

**Status do working tree:** A rodada deixou alteracoes intencionais apenas em `apps/reponotes-vnext` e docs. Permanecem itens locais antigos fora do escopo em `sample-repository`: `sample-repository/Projetos/Roadmap.md` deletado, `sample-repository/.reponotes-trash/` nao rastreado e `sample-repository/Nova nota.md` nao rastreado. Eles nao foram alterados nem incluidos no commit.

**Pendencias:** Definir a experiencia real do Info panel para metadata editavel no vNext, validar frontmatter fora do corpo visual, medir lazy loading/code splitting do editor visual e decidir como persistir Markdown gerado via autosave sem expor debug na UI.

**Riscos tecnicos:** Baixo para layout. O risco principal segue sendo arquitetural: Crepe ainda adiciona bundle grande e a spike continua em memoria, sem filesystem/Tauri/autosave real.

**Proximo passo sugerido:** Fazer a proxima rodada vNext focada em frontmatter/autosave em memoria ou em code splitting/lazy loading do editor visual antes de promover Milkdown/Crepe como decisao final.

## 2026-06-04 15:36:03 -03:00

**Objetivo da rodada:** Registrar a mudanca estrategica do RepoNotes vNext para full web como direcao principal.

**Decisao registrada:** RepoNotes vNext deixa de priorizar Tauri/desktop e passa a ser um produto web-first. O frontend continua React/Vite/TypeScript. Tauri fica como opcao futura/alternativa, nao como fase principal. O produto passa a exigir backend web, banco e contratos de servico para repositorio/storage. SQLite pode atender o MVP pessoal/local; PostgreSQL fica como caminho futuro para instalacoes servidor/multiusuario. Ghost nao sera backend principal e pode ser considerado apenas como destino futuro de publicacao/exportacao.

**Arquivos alterados:**

- `docs/PRODUCT.md`
- `docs/ROADMAP.md`
- `docs/ARCHITECTURE_VNEXT.md`
- `docs/UI_GUIDE.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A documentacao agora define vNext como web-first technical documentation workspace. O roadmap foi reorganizado em fases full web, incluindo arquitetura web, validacao do editor visual, backend API, banco local, modelo de seguranca/auth, CRUD, autosave, metadata, Application Documentation Pack, health score, review cycle, RACI, runbooks, handover, broken links, entities, export, protected notes e deployment/backup. A arquitetura registra backend API futuro, SQLite MVP, PostgreSQL futuro, auth/security boundary, audit log futuro, proibicao de exposicao publica no MVP, Ghost fora do core e estrategia de notas protegidas com preferencia por criptografia client-side/zero-knowledge. O UI guide foi ajustado para linguagem de workspace web e removeu controles de janela como elemento essencial do vNext.

**Resultado do build/test:** Not run, documentation-only change.

**Riscos:** Full web exige backend e seguranca mais serios. Controle de acesso amador e proibido. Expor dados privados publicamente esta fora do escopo do MVP. Criptografia client-side/zero-knowledge complica busca, exportacao e health score. Mobile exigira layout proprio com drawers/bottom sheets, nao compressao do layout desktop. Escopo pode crescer se backend, auth e editor visual avancarem sem fases pequenas.

**Proximo passo sugerido:** Definir o contrato inicial `RepositoryService` / `StorageService` e uma proposta minima de backend API + SQLite para MVP local/private web, ainda sem expor publicamente.

## 2026-06-04 15:40:47 -03:00

**Objetivo da rodada:** Reduzir a margem superior do documento no Visual Markdown Editor para equilibrar melhor com a margem esquerda e aproximar o titulo da toolbar.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/styles/globals.css`
- `docs/TASK_LOG.md`

**Resumo do ajuste visual:** O `padding-top` do `.milkdown-shell .ProseMirror` foi reduzido de `34px` para `18px`, mantendo o recuo lateral, o titulo como primeiro bloco editavel da nota e a superficie integrada sem header externo, borda, card ou janela interna.

**Resultado do install:** `npm install` em `apps/reponotes-vnext` executado com sucesso; dependencias atualizadas, 268 pacotes auditados, 0 vulnerabilidades.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso conhecido de chunk grande do Crepe/CodeMirror continua presente: chunk principal de aproximadamente `1,690.29 kB` minificado (`534.26 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; HTTP 200 confirmado.

**Validacao visual:** No navegador embutido, o DOM confirmou `.milkdown-shell .ProseMirror` com `contenteditable="true"`, `padding-top: 18px`, `padding-left: 56px`, titulo `Application Documentation Pack` como bloco do documento, sem `.document-editor-header`, sem badges no centro e com bordas de `.visual-editor`/`.milkdown-shell` em `0px`.

**Observacoes:** Permanecem itens locais antigos fora do escopo em `sample-repository`: `sample-repository/Projetos/Roadmap.md` deletado, `sample-repository/.reponotes-trash/` nao rastreado e `sample-repository/Nova nota.md` nao rastreado. Eles nao foram alterados nem incluidos no commit.

**Proximo passo sugerido:** Validar a mesma composicao em alturas menores e, se necessario, ajustar responsivamente o padding interno do ProseMirror sem reintroduzir molduras.

## 2026-06-04 15:47:11 -03:00

**Objetivo da rodada:** Validar Markdown round-trip e fronteira de frontmatter no editor visual, ajustando a TopBar para a direcao full web workspace.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/components/layout/TopBar.tsx`
- `apps/reponotes-vnext/src/styles/globals.css`
- `apps/reponotes-vnext/src/data/mockRepository.ts`
- `apps/reponotes-vnext/README.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O mock principal passou a incluir frontmatter YAML com `title`, `type`, `status`, `owner` e `tags`. O `VisualMarkdownEditor` agora separa frontmatter do corpo Markdown antes de montar o Milkdown/Crepe, entrega apenas o corpo ao editor visual e recompõe o Markdown gerado em memoria com o frontmatter preservado. A TopBar deixou de desenhar controles fake de minimizar/maximizar/fechar e passou a mostrar elementos de workspace web: `infra-docs`, `Private workspace` e chip discreto de usuario/settings.

**Resultado do install:** `npm install` em `apps/reponotes-vnext` executado com sucesso; dependencias atualizadas, 268 pacotes auditados, 0 vulnerabilidades.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso conhecido de chunk grande do Crepe/CodeMirror continua presente: chunk principal de aproximadamente `1,690.91 kB` minificado (`534.41 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; HTTP 200 confirmado.

**Validacao visual/round-trip:** No navegador embutido, o DOM confirmou `.milkdown-shell .ProseMirror` com `contenteditable="true"`, `data-has-frontmatter="true"`, `data-frontmatter-boundary="body-only-editor"` e `data-generated-markdown-starts-with-frontmatter="true"`. O corpo visual contem o heading `Application Documentation Pack`, checklist, link `LibreNMS` e tabela, mas nao contem `title: Application Documentation Pack` nem delimitadores YAML `---`. A TopBar nao possui `.window-controls` e mostra `infra-docs`, `Private workspace` e `RA`.

**Observacoes:** Permanecem itens locais antigos fora do escopo em `sample-repository`: `sample-repository/Projetos/Roadmap.md` deletado, `sample-repository/.reponotes-trash/` nao rastreado e `sample-repository/Nova nota.md` nao rastreado. Eles nao foram alterados nem incluidos no commit.

**Proximo passo sugerido:** Adicionar testes automatizados pequenos para `splitMarkdownFrontmatter`/recomposicao de Markdown e avaliar como expor o Markdown gerado para autosave sem criar painel debug visivel.

## 2026-06-04 15:50:46 -03:00

**Objetivo da rodada:** Remover os dois logos/icones `R` redundantes da interface web do RepoNotes vNext.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/components/layout/TopBar.tsx`
- `apps/reponotes-vnext/src/components/layout/LeftRail.tsx`
- `apps/reponotes-vnext/src/styles/globals.css`
- `docs/TASK_LOG.md`

**Resumo visual:** O `brand-mark` com `R` foi removido da TopBar, mantendo o texto `RepoNotes vNext`. O `rail-logo` com `R` foi removido do topo do LeftRail, que agora comeca diretamente pelo primeiro botao funcional/ativo (`Repository`). Os estilos mortos dos dois logos foram removidos e o padding superior do rail foi ajustado para evitar buraco visual.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso conhecido de chunk grande do Crepe/CodeMirror continua presente: chunk principal de aproximadamente `1,690.78 kB` minificado (`534.38 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; HTTP 200 confirmado.

**Validacao visual:** No navegador embutido, o DOM confirmou `brandMarkExists=false`, `railLogoExists=false`, `RepoNotes vNext` ainda visivel na TopBar, primeiro botao do rail como `Repository`, 10 botoes funcionais/placeholder no rail e Milkdown ainda montado.

**Proximo passo sugerido:** Revisar a densidade da TopBar web como um todo, avaliando se `RepoNotes vNext`, command box e indicadores de workspace devem ocupar menos largura em telas menores.

## 2026-06-04 15:55:03 -03:00

**Objetivo da rodada:** Validar Markdown round-trip e fronteira de frontmatter no `VisualMarkdownEditor` do RepoNotes vNext, sem alterar codigo.

**Arquivos alterados:**

- `docs/TASK_LOG.md`

**Resumo da validacao:** O mock principal em `mockRepository.ts` ja usa uma nota com frontmatter YAML (`title`, `type`, `status`, `owner`, `tags`) e corpo Markdown com heading, blockquote, checklist, link interno, link externo, tabela e code fence. O `VisualMarkdownEditor` separa o frontmatter do corpo, entrega apenas o corpo para o Milkdown/Crepe e recompõe o Markdown em memoria com o frontmatter preservado.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso conhecido de chunk grande do Crepe/CodeMirror permanece: chunk principal de aproximadamente `1,690.78 kB` minificado (`534.38 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; a validacao no navegador embutido acessou a URL com sucesso.

**Validacao visual/round-trip:** O DOM confirmou `.milkdown-shell .ProseMirror` com `contenteditable="true"`, `data-has-frontmatter="true"`, `data-frontmatter-boundary="body-only-editor"` e `data-generated-markdown-starts-with-frontmatter="true"`. O corpo visual contem `Application Documentation Pack`, checklist, `[[10-RACI]]`, `LibreNMS`, tabela e `dotnet test`, mas nao contem `title: Application Documentation Pack` nem delimitadores YAML `---`.

**Observacoes:** Permanecem itens locais antigos fora do escopo em `sample-repository`: `sample-repository/Projetos/Roadmap.md` deletado, `sample-repository/.reponotes-trash/` nao rastreado e `sample-repository/Nova nota.md` nao rastreado. Eles nao foram alterados nem incluidos no commit.

**Proximo passo sugerido:** Adicionar testes automatizados pequenos para split/recombine de frontmatter e definir o contrato de autosave que recebera Markdown limpo recomposto sem expor painel debug na UI principal.

## 2026-06-04 16:00:18 -03:00

**Objetivo da rodada:** Adicionar uma gutter/barra discreta de numeracao ao editor visual e estabilizar o tamanho do titulo/H1 durante foco, selecao, edicao e duplo clique.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/styles/globals.css`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** O `VisualMarkdownEditor` recebeu uma gutter visual nao editavel, fora do corpo do Milkdown/ProseMirror, com numeracao discreta em coluna lateral. O layout do editor passou a usar uma grade com gutter e documento. O CSS do primeiro `h1` foi estabilizado para impedir crescimento visual em estados focado/selecionado, removendo transform e fixando tamanho/line-height consistentes.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso conhecido de chunk grande do Crepe/CodeMirror permanece: chunk principal de aproximadamente `1,690.94 kB` minificado (`534.45 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; a validacao no navegador embutido acessou a URL com sucesso.

**Validacao visual:** O navegador embutido confirmou `.editor-gutter` existente, `aria-hidden="true"`, `pointer-events: none`, `user-select: none`, gutter alinhada antes do `.ProseMirror`, editor com `contenteditable="true"` e sem header duplicado. O H1 `Application Documentation Pack` permaneceu com `font-size: 30px`, `line-height: 36px`, `transform: none` e mesma altura antes e depois do duplo clique.

**Pendencias:** A gutter e visual por blocos/linhas principais, nao uma numeracao real linha-a-linha sincronizada com o layout do ProseMirror. Tambem nao ha sincronizacao de scroll fina entre gutter e conteudo longo.

**Proximo passo sugerido:** Avaliar se a gutter deve evoluir para marcadores por bloco real derivados do documento, ou permanecer como detalhe visual ate o editor/estrutura de documento final estarem definidos.

## 2026-06-04 16:06:10 -03:00

**Objetivo da rodada:** Extrair split/recombine de frontmatter para util reutilizavel, validar os casos basicos de fronteira Markdown/frontmatter e registrar gutter real como backlog futuro.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/components/editor/markdownFrontmatter.ts`
- `apps/reponotes-vnext/README.md`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** A logica inline de frontmatter saiu do `VisualMarkdownEditor` e foi movida para `markdownFrontmatter.ts`, com funcoes pequenas para split, recombine, body-only editor Markdown e recomposicao futura para autosave. O README registra os quatro casos esperados: Markdown com frontmatter + body, Markdown sem frontmatter, frontmatter preservado ao recompor e body enviado ao editor iniciando no H1. Como o escopo de leitura desta rodada nao incluiu `package.json`, nenhum runner novo foi adicionado; a pendencia de teste unitario dedicado ficou documentada sem introduzir framework pesado.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso conhecido de chunk grande do Crepe/CodeMirror permanece: chunk principal de aproximadamente `1,690.94 kB` minificado (`534.45 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; a validacao no navegador embutido acessou a URL com sucesso.

**Validacao:** O navegador embutido confirmou Milkdown montado com `contenteditable="true"`, `data-has-frontmatter="true"`, `data-frontmatter-boundary="body-only-editor"`, `data-generated-markdown-starts-with-frontmatter="true"`, corpo visual iniciando em `Application Documentation Pack`, sem YAML `title:` ou delimitadores `---`, e sem painel debug `Markdown gerado`.

**Pendencias:** Adicionar runner de testes frontend e cobrir `markdownFrontmatter.ts` com unit tests reais assim que a estrategia de teste do vNext for escolhida. A gutter real/sincronizada por blocos/linhas do ProseMirror foi registrada no roadmap como backlog futuro.

**Proximo passo sugerido:** Definir o contrato de autosave em memoria e o limite inicial de `StorageService`/`RepositoryService` para receber Markdown limpo recomposto sem acoplar a UI ao backend.

## 2026-06-04 16:11:16 -03:00

**Objetivo da rodada:** Corrigir layout shift/mudanca de margem do editor visual quando o foco alterna entre titulo, texto e estado sem foco.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/styles/globals.css`
- `docs/TASK_LOG.md`

**Causa provavel encontrada:** O tema do Crepe/Milkdown ainda influenciava a geometria de blocos internos, e o seletor local `h1:first-child` era fragil diante da estrutura gerada pelo ProseMirror. Com isso, margens/tamanhos computados do H1 e blocos podiam variar entre estados de foco/seleção ou ficar sujeitos a estilos do tema.

**Resumo da correcao CSS:** Foram definidas variaveis fixas para largura maxima, padding do documento, fonte/line-height do corpo e geometria do titulo. O box model de `[data-milkdown-root]`, `.milkdown`, `.ProseMirror`, `.ProseMirror-focused`, paragrafo, H1 e `.ProseMirror-selectednode` foi travado para impedir mudancas de margin, padding, border-width, transform, font-size e line-height durante foco/seleção. O H1 agora usa regras estaveis para `h1`, `h1:first-child`, `h1:first-of-type` e estados focados/selecionados.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O aviso conhecido de chunk grande do Crepe/CodeMirror permanece: chunk principal de aproximadamente `1,690.94 kB` minificado (`534.45 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` iniciou Vite em `http://127.0.0.1:5174/`; a validacao no navegador embutido acessou a URL com sucesso.

**Validacao manual de foco:** A sequencia titulo, duplo clique no titulo, clique no blockquote/primeiro texto e clique fora do editor manteve as mesmas coordenadas de `.ProseMirror`, H1, blockquote, paragrafo e gutter. O H1 permaneceu em `font-size: 30px`, `line-height: 36px`, margem `0px 0px 22px`, e o documento manteve padding `18px 56px 72px 44px` em todos os estados medidos.

**Proximo passo sugerido:** Validar em mais resolucoes e, se ainda aparecer algum shift manual dificil de reproduzir, inspecionar classes dinamicas especificas emitidas pelo Crepe durante composicao/seleção com uma amostra de documento maior.

## 2026-06-04 16:25:06 -03:00

**Objetivo da rodada:** Diagnosticar o layout shift do editor visual que ainda aparece no navegador real ao alternar entre sem foco, foco no H1 e foco em paragrafo.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/styles/globals.css`
- `docs/TASK_LOG.md`

**Resumo das mudancas:** Foi adicionado um modo diagnostico somente em DEV, ativado por `?debugLayout=1`. Quando ativo, o editor mede e mostra em overlay discreta, alem de enviar `console.table`, os dados de `.editor-gutter`, `.milkdown-shell`, `.ProseMirror`, primeiro `h1`, primeiro `p` e primeiro `blockquote`. As medidas incluem `x`, `y`, `width`, `height`, `margin-left`, `margin-top`, `padding-left`, `padding-top`, `scrollLeft`, `scrollTop`, `transform`, `font-size`, `line-height` e `className`. Tambem foram adicionados os toggles `hideGutter=1` e `flatEditor=1`.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. Permanece o aviso conhecido de chunk grande do Crepe/CodeMirror: chunk principal de aproximadamente `1,694.16 kB` minificado (`535.26 kB` gzip).

**Resultado do dev server:** `npm run dev -- --port 5174` ficou disponivel em `http://127.0.0.1:5174/`; validacao por navegador interno acessou a aplicacao com sucesso.

**Validacao do diagnostico:** Sem query string, a UI normal nao exibiu overlay nem classes de debug. Com `?debugLayout=1`, a overlay apareceu com 6 linhas de medicao. Com `?debugLayout=1&hideGutter=1`, a gutter ficou `display: none`, a grade mudou de `52px 452px` para `0px 504px` e o editor expandiu para ocupar a largura liberada. Com `?debugLayout=1&flatEditor=1`, a classe `debug-flat-editor` foi aplicada sem alterar a geometria medida.

**hideGutter mudou o comportamento?:** Sim, mas apenas de forma esperada para o teste: removeu a gutter e deslocou/expandiu o conteudo pela largura de `52px` liberada. Nao houve evidencia automatizada de shift causado por foco nesse modo.

**flatEditor mudou o comportamento?:** Nao na validacao automatizada. `x`, `y`, largura, altura, margens, paddings, fonte, line-height e transform permaneceram equivalentes ao modo debug normal.

**Propriedades que mudaram:** Entre modo normal e `hideGutter`, mudaram `grid-template-columns`, `display` da gutter, `x` e largura dos elementos do editor pela remocao intencional da coluna. Durante clique por coordenadas em H1 e paragrafo, as medidas de `x`, `y`, `margin`, `padding`, `font-size`, `line-height`, `transform`, `scrollLeft` e `scrollTop` permaneceram estaveis no navegador interno.

**Conclusao provavel:** A validacao automatizada ainda nao reproduz o salto manual observado pelo usuario. A instrumentacao agora permite capturar a causa diretamente no navegador real: se o salto ocorrer, a overlay/console deve indicar se a mudanca vem de `x/y`, margin/padding, scroll interno, transform, tamanho de fonte ou classe dinamica emitida pelo Milkdown/Crepe.

**Pendencias:** Reproduzir manualmente em `http://127.0.0.1:5174/?debugLayout=1`, comparar com `&hideGutter=1` e `&flatEditor=1`, e aplicar uma correcao CSS direcionada somente depois de identificar qual propriedade muda no momento do salto.

**Proximo passo sugerido:** Usar a overlay de diagnostico no navegador onde o problema aparece, anotar a linha/propriedade que muda ao clicar no H1/paragrafo e corrigir a causa especifica em uma rodada curta.

## 2026-06-04 21:01:06 -03:00

**Objetivo da rodada:** Remover a gutter fake de numeracao do editor visual e registrar numeracao real como backlog futuro.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/styles/globals.css`
- `docs/ROADMAP.md`
- `docs/TASK_LOG.md`

**Decisao:** A gutter visual/fake foi removida para evitar uma percepcao de funcionalidade inexistente e reduzir interferencias visuais no editor. A numeracao real fica como backlog: `Real editor gutter / line numbering synchronized with ProseMirror blocks/scroll`.

**Resumo:** O editor voltou a usar uma superficie unica do Milkdown/Crepe, sem coluna de numeracao simulada e sem CSS/grid criado apenas para a gutter. O titulo continua sendo o primeiro bloco editavel e o frontmatter permanece fora do corpo visual.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. Permanece o aviso conhecido de chunk grande do Crepe/CodeMirror: chunk principal de aproximadamente `1,693.85 kB` minificado (`535.17 kB` gzip).

**Proximo passo sugerido:** Implementar autosave em memoria com contrato inicial de `StorageService`/`RepositoryService`.

## 2026-06-04 21:06:02 -03:00

**Objetivo da rodada:** Criar contratos iniciais `RepositoryService` e `StorageService` para o RepoNotes vNext, ainda usando mock/in-memory, e conectar o autosave do editor ao storage mockado.

**Arquivos alterados:**

- `apps/reponotes-vnext/src/app/App.tsx`
- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/components/layout/AppShell.tsx`
- `apps/reponotes-vnext/src/components/layout/EditorWorkspace.tsx`
- `apps/reponotes-vnext/src/components/layout/StatusBar.tsx`
- `apps/reponotes-vnext/src/services/RepositoryService.ts`
- `apps/reponotes-vnext/src/services/StorageService.ts`
- `apps/reponotes-vnext/src/services/MockRepositoryService.ts`
- `apps/reponotes-vnext/src/services/MockStorageService.ts`
- `apps/reponotes-vnext/src/types/reponotes.ts`
- `docs/TASK_LOG.md`

**Contratos criados:** `RepositoryService` cobre leitura/navegacao (`getRepository`, `listNotes`, `getNoteById`, `getActiveNote`, `listTrashItems`) e `StorageService` cobre escrita/persistencia futura (`saveNoteContent`, `saveNoteMetadata`, `moveNoteToTrash`, `restoreNote`). As implementacoes atuais sao mock/in-memory, sem backend, banco, filesystem, fetch HTTP ou autenticacao.

**Resumo:** O `App` passou a resolver a nota ativa pelo `MockRepositoryService`. O `VisualMarkdownEditor` recompõe `frontmatter + body`, marca alteracoes como `changed`, aplica debounce, simula `saving` e salva o Markdown recomposto no `MockStorageService`. A `StatusBar` agora exibe `All changes saved locally`, `Unsaved changes`, `Saving...` ou `Save error` conforme o estado local.

**Resultado do build:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. Permanece o aviso conhecido de chunk grande do Crepe/CodeMirror: chunk principal de aproximadamente `1,695.63 kB` minificado (`535.76 kB` gzip).

**Proximo passo sugerido:** Criar backend API skeleton Fastify sem integrar ainda.

## 2026-06-04 21:10:08 -03:00

**Objetivo da rodada:** Criar backend API skeleton Fastify para o RepoNotes vNext, sem integrar SQLite, auth ou frontend.

**Arquivos alterados:**

- `apps/reponotes-vnext/server/package.json`
- `apps/reponotes-vnext/server/package-lock.json`
- `apps/reponotes-vnext/server/tsconfig.json`
- `apps/reponotes-vnext/server/src/index.ts`
- `apps/reponotes-vnext/server/src/routes/health.ts`
- `apps/reponotes-vnext/server/src/routes/notes.ts`
- `apps/reponotes-vnext/server/src/types.ts`
- `docs/TASK_LOG.md`

**Endpoints criados:** `GET /health`, `GET /api/notes`, `GET /api/notes/:id` e `PUT /api/notes/:id/content`. Todos usam dados mock/in-memory e o servidor escuta em `127.0.0.1:3001` por padrao.

**Resultado do build backend:** `npm install` e `npm run build` em `apps/reponotes-vnext/server` executados com sucesso.

**Health check:** `GET http://127.0.0.1:3001/health` retornou `{"service":"reponotes-api","status":"ok"}`. Os endpoints mockados de notas tambem responderam JSON localmente, incluindo `PUT /api/notes/overview/content`.

**Resultado do build frontend:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. O frontend continua independente do backend. Permanece o aviso conhecido de chunk grande do Crepe/CodeMirror: chunk principal de aproximadamente `1,695.63 kB` minificado (`535.76 kB` gzip).

**Proximo passo sugerido:** Definir SQLite schema/migrations ou criar um API client mock no frontend sem substituir os servicos atuais ainda.

## 2026-06-04 21:14:22 -03:00

**Objetivo da rodada:** Adicionar API client no frontend e implementar `HttpRepositoryService`/`HttpStorageService` usando o backend Fastify mockado, mantendo mock/in-memory como padrao.

**Arquivos alterados:**

- `apps/reponotes-vnext/README.md`
- `apps/reponotes-vnext/src/app/App.tsx`
- `apps/reponotes-vnext/src/components/editor/VisualMarkdownEditor.tsx`
- `apps/reponotes-vnext/src/services/apiClient.ts`
- `apps/reponotes-vnext/src/services/HttpRepositoryService.ts`
- `apps/reponotes-vnext/src/services/HttpStorageService.ts`
- `apps/reponotes-vnext/src/services/serviceRegistry.ts`
- `docs/TASK_LOG.md`

**Services criados:** `apiClient` usa `fetch` nativo com base URL padrao `http://127.0.0.1:3001`, `getJson`, `putJson` e erro HTTP minimo. `HttpRepositoryService` chama `GET /api/notes` e `GET /api/notes/:id`. `HttpStorageService` chama `PUT /api/notes/:id/content` para salvar Markdown recomposto.

**Resumo:** O frontend passou a resolver `repositoryService` e `storageService` por um registry simples com `USE_HTTP_SERVICES=false` por padrao. Assim a UI continua funcionando sem backend, mas pode ser apontada para a API mock local durante testes. O autosave continua usando mock por padrao e, quando o flag for ativado com o backend rodando, chamara o `PUT` mockado.

**Resultado do build backend:** `npm run build` em `apps/reponotes-vnext/server` executado com sucesso.

**Resultado do build frontend:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. Permanece o aviso conhecido de chunk grande do Crepe/CodeMirror: chunk principal de aproximadamente `1,695.64 kB` minificado (`535.78 kB` gzip).

**Pendencias:** `USE_HTTP_SERVICES` permanece `false` antes do commit. O backend ainda e mock/in-memory, sem SQLite, auth, fallback visual ou tratamento completo de erros na UI.

**Proximo passo sugerido:** Criar SQLite schema/migrations para notas e metadados, ou evoluir o API client para fallback/estado de conexao antes de ligar HTTP por padrao.

## 2026-06-04 21:24:19 -03:00

**Objetivo da rodada:** Adicionar schema/migrations iniciais SQLite ao backend Fastify do RepoNotes vNext, sem auth, PostgreSQL ou integracao visual nova.

**Dependencia SQLite adicionada:** `better-sqlite3` com `@types/better-sqlite3`.

**Arquivos alterados:**

- `.gitignore`
- `apps/reponotes-vnext/server/package.json`
- `apps/reponotes-vnext/server/package-lock.json`
- `apps/reponotes-vnext/server/src/db/connection.ts`
- `apps/reponotes-vnext/server/src/db/migrations.ts`
- `apps/reponotes-vnext/server/src/db/schema.sql`
- `apps/reponotes-vnext/server/src/db/seed.ts`
- `apps/reponotes-vnext/server/src/index.ts`
- `apps/reponotes-vnext/server/src/routes/notes.ts`
- `apps/reponotes-vnext/server/src/types.ts`
- `docs/TASK_LOG.md`

**Tabelas criadas:** `schema_versions`, `notes`, `tags`, `note_tags` e `audit_events`. O banco padrao em desenvolvimento fica em `apps/reponotes-vnext/server/data/reponotes-dev.db`, com override por `REPNOTES_DB_PATH`; a pasta `server/data/` foi ignorada no Git.

**Endpoints atualizados:** `GET /api/notes`, `GET /api/notes/:id` e `PUT /api/notes/:id/content` agora usam SQLite. O seed inicial insere `Application Documentation Pack` quando o banco esta vazio, mantendo frontmatter separado do body. `PUT` atualiza `body_markdown`, `frontmatter`, `updated_at` e registra evento simples em `audit_events`.

**Resultado do build backend:** `npm run build` em `apps/reponotes-vnext/server` executado com sucesso.

**Health check:** `GET http://127.0.0.1:3001/health` retornou `{"service":"reponotes-api","status":"ok"}`. `GET /api/notes` retornou a nota seed do SQLite. `PUT /api/notes/overview/content` retornou status `saved` e atualizou o banco local de desenvolvimento.

**Resultado do build frontend:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. Permanece o aviso conhecido de chunk grande do Crepe/CodeMirror: chunk principal de aproximadamente `1,695.64 kB` minificado (`535.78 kB` gzip).

**Proximo passo sugerido:** Conectar frontend HTTP services ao backend SQLite durante uma validacao controlada, mantendo fallback/estado de erro antes de ligar HTTP por padrao.

## 2026-06-04 22:21:13 -03:00

**Objetivo da rodada:** Conectar os services HTTP do frontend ao backend SQLite em modo controlado, com fallback seguro para mock quando a API estiver indisponivel.

**Arquivos alterados:**

- `apps/reponotes-vnext/server/src/index.ts`
- `apps/reponotes-vnext/src/app/App.tsx`
- `apps/reponotes-vnext/src/components/layout/AppShell.tsx`
- `apps/reponotes-vnext/src/components/layout/StatusBar.tsx`
- `apps/reponotes-vnext/src/services/apiClient.ts`
- `apps/reponotes-vnext/src/services/serviceRegistry.ts`
- `docs/TASK_LOG.md`

**Comportamento mock:** Sem `VITE_REPONOTES_USE_HTTP=true`, o frontend continua usando `MockRepositoryService` e `MockStorageService`. Validado no navegador em `http://127.0.0.1:5174/`, com StatusBar exibindo `Local mock`.

**Comportamento HTTP:** Com `VITE_REPONOTES_USE_HTTP=true` e backend Fastify/SQLite rodando em `127.0.0.1:3001`, o frontend carregou via API e a StatusBar exibiu `API connected`. O backend recebeu CORS local restrito a `http://127.0.0.1:5174` e `http://localhost:5174` para permitir o workspace Vite sem expor host publico.

**Fallback:** Com `VITE_REPONOTES_USE_HTTP=true` e o backend parado, o app nao quebrou, caiu para mock e a StatusBar exibiu `API offline, using local mock`. O `apiClient` agora diferencia erro HTTP de erro de conexao.

**Resultado do build backend:** `npm run build` em `apps/reponotes-vnext/server` executado com sucesso.

**Resultado do build frontend:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. Permanece o aviso conhecido de chunk grande do Crepe/CodeMirror: chunk principal de aproximadamente `1,699.48 kB` minificado (`536.89 kB` gzip).

**Proximo passo sugerido:** Implementar notes CRUD minimo sobre a API SQLite, com criacao/listagem/atualizacao de metadados antes de ligar HTTP por padrao para todos os fluxos.

## 2026-06-04 22:30:10 -03:00

**Objetivo da rodada:** Implementar Notes CRUD minimo sobre a API SQLite, mantendo soft delete, restore e services mock/HTTP compilando.

**Arquivos alterados:**

- `apps/reponotes-vnext/server/src/index.ts`
- `apps/reponotes-vnext/server/src/routes/notes.ts`
- `apps/reponotes-vnext/server/src/types.ts`
- `apps/reponotes-vnext/src/services/apiClient.ts`
- `apps/reponotes-vnext/src/services/HttpRepositoryService.ts`
- `apps/reponotes-vnext/src/services/HttpStorageService.ts`
- `apps/reponotes-vnext/src/services/MockStorageService.ts`
- `apps/reponotes-vnext/src/services/StorageService.ts`
- `apps/reponotes-vnext/src/services/serviceRegistry.ts`
- `docs/TASK_LOG.md`

**Endpoints CRUD criados:** `POST /api/notes`, `PATCH /api/notes/:id/metadata`, `DELETE /api/notes/:id` e `POST /api/notes/:id/restore`, alem dos endpoints existentes de listagem, leitura e salvamento de conteudo.

**Comportamento soft delete/restore:** `DELETE` preenche `deleted_at`, atualiza `updated_at` e registra `audit_events`. Notas deletadas deixam de aparecer em `GET /api/notes` e `GET /api/notes/:id` retorna 404. `POST /api/notes/:id/restore` limpa `deleted_at`, atualiza `updated_at` e registra auditoria. Validacao local confirmou criacao, patch de metadata, soft delete, 404 apos delete, exclusao da listagem, restore e 4 eventos de auditoria para a nota de teste.

**Services atualizados:** `StorageService` ganhou `createNote`; `HttpStorageService` chama os endpoints CRUD; `MockStorageService` implementa criacao em memoria e os metodos existentes continuam mantendo fallback seguro.

**Resultado do build backend:** `npm run build` em `apps/reponotes-vnext/server` executado com sucesso.

**Resultado do build frontend:** `npm run build` em `apps/reponotes-vnext` executado com sucesso. Permanece o aviso conhecido de chunk grande do Crepe/CodeMirror: chunk principal de aproximadamente `1,701.01 kB` minificado (`537.24 kB` gzip).

**Proximo passo sugerido:** Criar UI minima para criar nota e mover para lixeira, reaproveitando os services ja conectados ao SQLite/fallback.
