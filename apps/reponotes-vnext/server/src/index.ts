import Fastify from "fastify";
import { registerHealthRoutes } from "./routes/health.js";
import { registerNoteRoutes } from "./routes/notes.js";

export async function buildServer() {
  const server = Fastify({
    logger: true
  });

  await registerHealthRoutes(server);
  await registerNoteRoutes(server);

  return server;
}

const server = await buildServer();
const port = Number(process.env.PORT ?? 3001);
const host = process.env.HOST ?? "127.0.0.1";

await server.listen({ host, port });
