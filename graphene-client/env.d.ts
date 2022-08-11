/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_OKTA_ISSUER: string
  readonly VITE_OKTA_CLIENTID: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}