/// <reference types="vite/client" />

declare const PACKAGE_VERSION: string

interface ImportMetaEnv {
  readonly VITE_OKTA_ISSUER: string
  readonly VITE_OKTA_CLIENTID: string
  readonly PACKAGE_VERSION: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}