import Fastify from "fastify";
import { runMigrations } from "./db/migrations.js";
import { seedDatabase } from "./db/seed.js";
import { registerHealthRoutes } from "./routes/health.js";
import { registerNoteRoutes } from "./routes/notes.js";

export async function buildServer() {
  runMigrations();
  seedDatabase();

  const server = Fastify({
    logger: true
  });

  server.addHook("onRequest", async (request, reply) => {
    const allowedOrigins = new Set([
      "http://127.0.0.1:5174",
      "http://localhost:5174"
    ]);
    const origin = request.headers.origin;

    if (origin && allowedOrigins.has(origin)) {
      reply.header("Access-Control-Allow-Origin", origin);
      reply.header("Vary", "Origin");
    }

    reply.header("Access-Control-Allow-Headers", "Content-Type, Accept");
    reply.header("Access-Control-Allow-Methods", "DELETE, GET, PATCH, POST, PUT, OPTIONS");

    if (request.method === "OPTIONS") {
      return reply.code(204).send();
    }
  });

  await registerHealthRoutes(server);
  await registerNoteRoutes(server);

  return server;
}

const server = await buildServer();
const port = Number(process.env.PORT ?? 3001);
const host = process.env.HOST ?? "127.0.0.1";

await server.listen({ host, port });
