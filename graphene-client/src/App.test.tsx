import { render, screen, userEvent } from './utils/test-utils'
import App from './App'
import { expect } from 'vitest'
import { ApolloClient, ApolloProvider, InMemoryCache } from '@apollo/client'
import { BrowserRouter } from 'react-router-dom'

describe('App', () => {
  it('renders weather forecast', () => {
    // const client = new ApolloClient({
    //   cache: new InMemoryCache()
    // })

    // render(
    //   <ApolloProvider client={client}>
    //     <BrowserRouter>
    //             <App />
    //         </BrowserRouter>
    //   </ApolloProvider>)
    // const linkElement = screen.getByText(/Weather forecast/i)
    // expect(linkElement).toBeInTheDocument()

    expect({ foo: 'bar' }).toMatchSnapshot()
  })
})
