import { render, screen } from './utils/test-utils'
import { describe, expect, it } from 'vitest'
import App from './App'
import { apolloClient } from './apolloClient'
import { ApolloProvider } from '@apollo/client'
import { BrowserRouter } from 'react-router-dom'

describe('App', () => {
  it('renders weather forecast', () => {
    render(
      <ApolloProvider client={apolloClient()}>
        <BrowserRouter>
          <App />
        </BrowserRouter>
      </ApolloProvider>)
    const linkElement = screen.getByText(/Weather forecast/i)
    expect(linkElement).toMatchSnapshot()
  })
})
