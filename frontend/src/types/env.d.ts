/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_APP_TITLE: string;
  readonly VITE_API_BASE_URL: string;
  readonly VITE_APP_ENV: 'development' | 'production' | 'test';
  readonly VITE_APP_MODE: string;
  readonly VITE_APP_VERSION: string;
  readonly VITE_APP_BUILD_TIME: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}

declare module 'process' {
  global {
    namespace NodeJS {
      export interface ProcessEnv {
        readonly NODE_ENV: 'development' | 'production' | 'test';
        readonly VITE_APP_TITLE: string;
        readonly VITE_API_BASE_URL: string;
        readonly VITE_APP_ENV: 'development' | 'production' | 'test';
        readonly VITE_APP_MODE: string;
        readonly VITE_APP_VERSION: string;
        readonly VITE_APP_BUILD_TIME: string;
      }
    }
  }
} 