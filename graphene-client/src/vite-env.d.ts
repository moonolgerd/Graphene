/// <reference types="vite/client" />

interface ImportMetaEnv {
  readonly VITE_OKTA_ISSUER: string
  readonly VITE_OKTA_CLIENTID: string
  readonly VITE_PACKAGE_VERSION: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}