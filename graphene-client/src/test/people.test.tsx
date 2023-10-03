import { render, screen, waitFor, userEvent } from '../utils/test-utils'
import { describe, expect, it, vi } from 'vitest'
import { MockedProvider } from '@apollo/client/testing'
import { GetPeople, PersonData } from '../components/People'
import { AddPerson, PersonInput } from '../components/People'
import People from '../components/People'

const mockData: PersonData = {
  people: [
    {
      id: '1',
      firstName: 'John',
      lastName: 'Doe',
      age: 30,
      personalStatement: undefined,
      address: {
        city: 'San Francisco',
        streetName: 'Market St',
        streetNumber: 123
      }
    },
    {
      id: '2',
      firstName: 'Jone',
      lastName: 'Doe',
      age: 30,
      personalStatement: undefined,
      address: {
        city: 'San Francisco',
        streetName: 'Market St',
        streetNumber: 123
      }
    }
  ]
}

const mockAddPerson = vi.fn()

const mockMutation = {
  request: {
    query: AddPerson,
    variables: {
      personInput: {
        firstName: 'Test',
        lastName: 'Person',
        age: 40,
        city: 'San Francisco',
        streetName: 'Market St',
        streetNumber: 789
      }
    }
  },
  result: {
    data: {
      addPerson: {
        id: '3',
        firstName: 'Test',
        lastName: 'Person',
        age: 40,
        city: 'San Francisco',
        streetName: 'Market St',
        streetNumber: 789
      }
    }
  }
}

const mockError = new Error('Something went wrong')

const mockQuery = {
  request: {
    query: GetPeople
  },
  result: {
    data: mockData
  }
}

const mockErrorQuery = {
  request: {
    query: GetPeople
  },
  error: mockError
}

describe('People', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('renders loading state', () => {
    render(
      <MockedProvider mocks={[]} addTypename={false}>
        <People />
      </MockedProvider>
    )

    expect(screen.getByText('Loading...')).toBeInTheDocument()
  })

  it('renders error state', async () => {
    render(
      <MockedProvider mocks={[mockErrorQuery]} addTypename={false} >
        <People />
      </MockedProvider>
    )

    await waitFor(() => {
      expect(screen.getByText(mockError.message)).toBeInTheDocument()
    })
  })

  it('renders data', async () => {
    render(
      <MockedProvider mocks={[mockQuery]} addTypename={false} >
        <People />
      </MockedProvider>
    )

    await waitFor(() => {
      expect(screen.getByText(mockData.people[0].firstName)).toBeInTheDocument()
      expect(screen.getByText(mockData.people[1].firstName)).toBeInTheDocument()
    })
  })

  // it('adds a person', async () => {
  //   render(
  //     <MockedProvider mocks={[mockQuery, mockMutation]} addTypename={false} >
  //       <People />
  //     </MockedProvider>
  //   )

  //   await waitFor(() => {
  //     userEvent.type(screen.getByLabelText('First Name'), 'Test')
  //     userEvent.type(screen.getByLabelText('Last Name'), 'Person')
  //     userEvent.type(screen.getByLabelText('Age'), '40')
  //     userEvent.type(screen.getByLabelText('City'), 'San Francisco')
  //     userEvent.type(screen.getByLabelText('Street Name'), 'Market St')
  //     userEvent.type(screen.getByLabelText('Street Number'), '789')
  //     userEvent.click(screen.getByText('Add'))
  //   })

  //   expect(mockAddPerson).toHaveBeenCalledWith({
  //     variables: {
  //       personInput: {
  //         firstName: 'Test',
  //         lastName: 'Person',
  //         age: 40,
  //         city: 'San Francisco',
  //         streetName: 'Market St',
  //         streetNumber: 789
  //       }
  //     }
  //   })
  // })
})