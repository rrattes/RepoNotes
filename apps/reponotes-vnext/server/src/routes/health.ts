import type { FastifyInstance } from "fastify";

export async function registerHealthRoutes(server: FastifyInstance) {
  server.get("/health", async () => ({
    service: "reponotes-api",
    status: "ok"
  }));
}
