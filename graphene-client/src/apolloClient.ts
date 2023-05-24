import { GraphQLWsLink } from '@apollo/client/link/subscriptions'
import { createClient } from 'graphql-ws'
import {
    ApolloClient,
    InMemoryCache,
    HttpLink,
    split,
} from "@apollo/client"
import { setContext } from '@apollo/client/link/context'
import { getMainDefinition } from '@apollo/client/utilities'

const authLink = setContext((_, { headers }) => {
    const tokenStorage = JSON.parse(localStorage.getItem("okta-token-storage")!)
    const token = tokenStorage.accessToken.accessToken
    return {
        headers: {
            ...headers,
            authorization: token ? `Bearer ${token}` : ''
        }
    }
})

const wsLink = new GraphQLWsLink(createClient({
    url: 'wss://localhost:7099/graphql/',
  }));

const httpLink = new HttpLink({
    uri: 'http://localhost:5099/graphql/'
})

const splitLink = split(
    ({ query }) => {
        const definition = getMainDefinition(query);
        return (
            definition.kind === 'OperationDefinition' &&
            definition.operation === 'subscription'
        );
    },
    wsLink,
    httpLink
)

const client = new ApolloClient({
    link: authLink.concat(splitLink),
    connectToDevTools: true,
    cache: new InMemoryCache()
})

export const apolloClient = () => client