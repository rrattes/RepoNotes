import { httpRepositoryService } from "./HttpRepositoryService";
import { httpStorageService } from "./HttpStorageService";
import { mockRepositoryService } from "./MockRepositoryService";
import { mockStorageService } from "./MockStorageService";
import type { RepositoryService } from "./RepositoryService";
import type { StorageService } from "./StorageService";

export const USE_HTTP_SERVICES = false;

export const repositoryService: RepositoryService = USE_HTTP_SERVICES
  ? httpRepositoryService
  : mockRepositoryService;

export const storageService: StorageService = USE_HTTP_SERVICES
  ? httpStorageService
  : mockStorageService;
