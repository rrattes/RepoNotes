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
