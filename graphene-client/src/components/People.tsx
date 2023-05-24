import { gql, useMutation, useQuery } from "@apollo/client"
import { useCallback, useState } from "react"

export interface Address {
    stringNumber: number
    stringName: string
    city: string
}

export interface Person {
    id: string
    firstName: string
    lastName: string
    age: number
    personalStatement: any
    address?: Address
}

export interface PersonInput {
    firstName: string
    lastName: string
    age: number
    city: string
    streetName : string
    streetNumber: number
}

export interface PersonData {
    people: Person[]
}

export const GetPeople = gql`
query GetPeople {
      people {
       id
       firstName
       lastName
       age
       personalStatement
       address {
           streetNumber
           streetName
           city
       }
    }
}
`

export const AddPerson = gql`
mutation AddPerson($personInput: PersonInput!) {
  addPerson(personInput: $personInput) {
    id
    firstName
    lastName
    age
    address {
      city
      streetName
      streetNumber
    }
  }
}
`

const People = () => {

    const { loading, error, data } = useQuery<PersonData>(GetPeople)

    const [firstName, setFirstName] = useState<string>("")
    const [lastName, setLastName] = useState<string>("")
    const [age, setAge] = useState<number>(0)
    const [city, setCity] = useState<string>("")
    const [streetName, setStreetName] = useState<string>("")
    const [streetNumber, setStreetNumber] = useState<number>(0)

    const [addPerson] = useMutation<Person>(AddPerson)

    const onAddClick = useCallback(() => {

        const personInput: PersonInput = {
            firstName: firstName,
            lastName: lastName,
            age: age,
            city: city,
            streetName: streetName,
            streetNumber: streetNumber
          } 
    
        addPerson({
          variables: { personInput }
        })
      }, [addPerson])

    if (error)
        return <div>{error.message}</div>

    if (loading)
        return <div>Loading...</div>

    return (
        <>
        <h2>
            People
        </h2>

        <form>
            <input type="text" placeholder="First Name" value={firstName} onChange={e => setFirstName(e.target.value)} />
            <input type="text" placeholder="Last Name" value={lastName} onChange={e => setLastName(e.target.value)} />
            <input type="text" placeholder="Age" value={age} onChange={e => setAge(parseInt(e.target.value))} />
            <input type="text" placeholder="City" value={city} onChange={e => setCity(e.target.value)} />
            <input type="text" placeholder="Street Name" value={streetName} onChange={e => setStreetName(e.target.value)} />
            <input type="text" placeholder="Street Number" value={streetNumber} onChange={e => setStreetNumber(parseInt(e.target.value))} />
        </form>

        <button onClick={onAddClick}>Add Person</button>

        <table>
            <thead>
                <tr>
                    <th>First Name</th>
                    <th>Last Name</th>
                    <th>Age</th>
                    <th>Address</th>
                </tr>
            </thead>
            <tbody>
                {data &&
                    data.people.map(person => (
                        <tr key={person.id}>
                            <td>{person.firstName}</td>
                            <td>{person.lastName}</td>
                            <td>{person.age}</td>
                            <td>{person.address?.city}</td>
                        </tr>
                    ))
                }
                </tbody>
            </table>
        </>
    )
}

export default People 