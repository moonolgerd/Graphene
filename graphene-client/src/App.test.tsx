import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';
import { ApolloClient, ApolloProvider, InMemoryCache } from '@apollo/client'

test('renders learn react link', () => {

  const client = new ApolloClient({
    cache: new InMemoryCache()
  })

  render(
    <ApolloProvider client={client}>
      <App />
    </ApolloProvider>)
  const linkElement = screen.getByText(/Weather forecast/i);
  expect(linkElement).toBeInTheDocument();
});
