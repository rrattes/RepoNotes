const DEFAULT_BASE_URL = "http://127.0.0.1:3001";

export class ApiClientError extends Error {
  constructor(
    message: string,
    public readonly status: number,
    public readonly body: string
  ) {
    super(message);
    this.name = "ApiClientError";
  }
}

async function requestJson<TResponse>(path: string, init?: RequestInit): Promise<TResponse> {
  const baseUrl = DEFAULT_BASE_URL.replace(/\/$/, "");
  const response = await fetch(`${baseUrl}${path}`, {
    ...init,
    headers: {
      Accept: "application/json",
      "Content-Type": "application/json",
      ...init?.headers
    }
  });

  if (!response.ok) {
    const body = await response.text();
    throw new ApiClientError(`HTTP ${response.status} for ${path}`, response.status, body);
  }

  return response.json() as Promise<TResponse>;
}

export function getJson<TResponse>(path: string): Promise<TResponse> {
  return requestJson<TResponse>(path);
}

export function putJson<TBody, TResponse>(path: string, body: TBody): Promise<TResponse> {
  return requestJson<TResponse>(path, {
    body: JSON.stringify(body),
    method: "PUT"
  });
}
