import { WebSocketLink } from '@apollo/client/link/ws'
import {
    ApolloClient,
    InMemoryCache,
    HttpLink,
    split,
} from "@apollo/client"
import { getMainDefinition } from '@apollo/client/utilities'

const wsLink = new WebSocketLink({
  uri: 'ws://localhost:5099/graphql/',
    options: {
        reconnect: true,
        lazy: true
    }
})

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
    link: splitLink,
    connectToDevTools: true,
    cache: new InMemoryCache()
})

export const apolloClient = () => client