using RepoNotes.Core.Models;

namespace RepoNotes.Core.Services;

public sealed class TechnicalNoteTemplateService : INoteTemplateService
{
    public const string FreeNoteTemplateId = "free-note";

    private readonly IReadOnlyList<NoteTemplate> _templates =
    [
        new()
        {
            Id = FreeNoteTemplateId,
            Name = "Nota livre",
            Description = "Nota Markdown simples para registro rapido.",
            SuggestedType = "note",
            MarkdownBody = """
            # {{title}}

            Escreva sua nota aqui.
            """
        },
        new()
        {
            Id = "runbook",
            Name = "Runbook",
            Description = "Procedimento operacional repetivel com pre-requisitos e rollback.",
            SuggestedType = "runbook",
            SuggestedTags = ["runbook", "operacao"],
            MarkdownBody = """
            # {{title}}

            ## Objetivo

            Descreva o objetivo operacional deste runbook.

            ## Escopo

            - Ambiente:
            - Sistema:
            - Owner:

            ## Pre-requisitos

            - Acesso necessario:
            - Dependencias:
            - Janela de execucao:

            ## Procedimento

            1. Validar estado inicial.
            2. Executar procedimento.
            3. Conferir resultado.

            ## Validacao

            - [ ] Logs conferidos.
            - [ ] Servico respondendo.
            - [ ] Evidencias registradas.

            ## Rollback

            Descreva como reverter a mudanca com seguranca.
            """
        },
        new()
        {
            Id = "technical-handover",
            Name = "Handover tecnico",
            Description = "Transferencia de contexto tecnico entre pessoas ou equipes.",
            SuggestedType = "handover",
            SuggestedTags = ["handover"],
            MarkdownBody = """
            # {{title}}

            ## Contexto

            ## Estado atual

            ## Decisoes importantes

            ## Pendencias

            - [ ] 

            ## Contatos e owners

            ## Links e referencias
            """
        },
        new()
        {
            Id = "incident",
            Name = "Incidente",
            Description = "Registro de incidente, impacto, timeline e acoes.",
            SuggestedType = "incident",
            SuggestedTags = ["incidente"],
            MarkdownBody = """
            # {{title}}

            ## Resumo

            ## Impacto

            ## Timeline

            - 

            ## Causa raiz

            ## Acoes imediatas

            - [ ] 

            ## Acoes preventivas

            - [ ] 
            """
        },
        new()
        {
            Id = "script",
            Name = "Script",
            Description = "Documentacao de script com uso, parametros e exemplos.",
            SuggestedType = "script",
            SuggestedTags = ["script"],
            MarkdownBody = """
            # {{title}}

            ## Objetivo

            ## Linguagem / Runtime

            ## Como executar

            ```powershell
            # comando aqui
            ```

            ## Parametros

            ## Saida esperada

            ## Riscos
            """
        },
        new()
        {
            Id = "prompt",
            Name = "Prompt",
            Description = "Prompt reutilizavel com contexto, entrada e saida esperada.",
            SuggestedType = "prompt",
            SuggestedTags = ["prompt"],
            MarkdownBody = """
            # {{title}}

            ## Objetivo

            ## Contexto

            ## Prompt

            ```text
            Escreva o prompt aqui.
            ```

            ## Entrada esperada

            ## Saida esperada
            """
        },
        new()
        {
            Id = "meeting",
            Name = "Reuniao",
            Description = "Ata curta com participantes, decisoes e proximas acoes.",
            SuggestedType = "meeting",
            SuggestedTags = ["reuniao"],
            MarkdownBody = """
            # {{title}}

            ## Participantes

            - 

            ## Pauta

            ## Decisoes

            - 

            ## Proximas acoes

            - [ ] 
            """
        },
        new()
        {
            Id = "checklist",
            Name = "Checklist",
            Description = "Lista operacional simples para execucao e conferencia.",
            SuggestedType = "checklist",
            SuggestedTags = ["checklist"],
            MarkdownBody = """
            # {{title}}

            ## Checklist

            - [ ] Item 1
            - [ ] Item 2
            - [ ] Item 3

            ## Observacoes
            """
        },
        new()
        {
            Id = "application",
            Name = "Aplicacao",
            Description = "Ficha tecnica leve de uma aplicacao.",
            SuggestedType = "application",
            SuggestedTags = ["aplicacao"],
            MarkdownBody = """
            # {{title}}

            ## Visao geral

            ## Owner / Time

            ## Ambientes

            - Desenvolvimento:
            - Homologacao:
            - Producao:

            ## Dependencias

            ## Runbooks relacionados
            """
        },
        new()
        {
            Id = "server",
            Name = "Servidor",
            Description = "Ficha tecnica leve de servidor ou host.",
            SuggestedType = "server",
            SuggestedTags = ["servidor", "infra"],
            MarkdownBody = """
            # {{title}}

            ## Identificacao

            - Hostname:
            - Ambiente:
            - Site:
            - Owner:

            ## Funcoes

            ## Servicos

            ## Acessos

            ## Observacoes operacionais
            """
        }
    ];

    public IReadOnlyList<NoteTemplate> GetTemplates() => _templates;

    public NoteTemplate GetDefaultTemplate() =>
        _templates.First(template => template.Id == FreeNoteTemplateId);

    public NoteTemplate? GetTemplateById(string templateId) =>
        _templates.FirstOrDefault(template => string.Equals(template.Id, templateId, StringComparison.OrdinalIgnoreCase));
}
