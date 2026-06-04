export type MarkdownParts = {
  body: string;
  frontmatter: string;
};

const frontmatterPattern = /^---\r?\n[\s\S]*?\r?\n---(?:\r?\n|$)/;

export function splitMarkdownFrontmatter(markdown: string): MarkdownParts {
  const match = markdown.match(frontmatterPattern);

  if (!match) {
    return {
      body: markdown,
      frontmatter: ""
    };
  }

  return {
    body: markdown.slice(match[0].length),
    frontmatter: match[0].trimEnd()
  };
}

export function combineMarkdownFrontmatter(frontmatter: string, body: string) {
  if (!frontmatter) {
    return body;
  }

  return `${frontmatter}\n\n${body}`;
}

export function getEditorBodyMarkdown(markdown: string) {
  return splitMarkdownFrontmatter(markdown).body;
}

export function recomposeMarkdownForAutosave(parts: MarkdownParts) {
  return combineMarkdownFrontmatter(parts.frontmatter, parts.body);
}
