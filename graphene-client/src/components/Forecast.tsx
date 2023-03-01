import { gql, useMutation, useQuery } from '@apollo/client'
import React, { useCallback, useEffect } from 'react'

export const FORECASTS = gql`
query {
  weatherForecast {
    date
    summary
    temperatureC
    temperatureF
  }
}
`

export const ADD_FORECAST = gql`
mutation AddWeatherForecast($forecast: WeatherForecastInput!) {
  addWeatherForecast(weatherForecastInput: $forecast) {
    summary
    temperatureC
    temperatureF
    date
  }
}
`

export const FORECAST_ADDED = gql`
subscription {
  onNewWeatherForecast {
    summary
    temperatureC
    temperatureF
    date
  }
}
`

export interface WeatherForecastData {
  weatherForecast: Forecast[]
}

export interface Forecast extends ForecastInput {
  temperatureF: number
}

export interface ForecastInput {
  date: string
  temperatureC: number
  summary: string
}

export interface NewForecastEvent {
  onNewWeatherForecast: Forecast
}

const WeatherForecast = () => {
  const { loading, error, data, subscribeToMore } = useQuery<WeatherForecastData>(FORECASTS)

  if (error) {
    console.error(error)
  }

  useEffect(() => {
    subscribeToMore<NewForecastEvent>({
      document: FORECAST_ADDED,
      updateQuery: addForecast
    })
  }, [subscribeToMore])

  const addForecast = (prev: WeatherForecastData,
    { subscriptionData }: { subscriptionData: { data: NewForecastEvent } }) => {

    if (!subscriptionData.data) return prev;
    const newWeatherForecast = subscriptionData.data.onNewWeatherForecast;

    const weatherForecast = [...prev.weatherForecast]
    weatherForecast.push(newWeatherForecast)

    return { weatherForecast }
  }

  const [sendForecast] = useMutation<ForecastInput>(ADD_FORECAST)

  const onAddClick = useCallback(() => {

    const date = new Date().toUTCString()

    const newForecast: ForecastInput = {
      date: date,
      summary: 'Freezing',
      temperatureC: Math.round(Math.random() * 100)
    }
    sendForecast({
      variables: { forecast: newForecast }
    })
  }, [sendForecast])

  const renderForecastsTable = (forecasts?: Forecast[]) => {

    if (!forecasts)
      return <h2>No data</h2>

    return (
      <table aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp. (C)</th>
            <th>Temp. (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts!.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
    )
  }

  let contents = loading
    ? <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>
    : renderForecastsTable(data?.weatherForecast)

  return (
    <div>
      <h2 id="tabelLabel">Weather forecast</h2>
      <p>This component demonstrates fetching data from the server using GraphQL queries, mutations, and subscription</p>
      <p>See <a href="http://localhost:5099/graphql" rel="noreferrer noopener" target="_blank">GraphQL playground</a></p>
      <button type="button" onClick={onAddClick}>Add Forecast</button>
      {contents}
    </div>
  )
}

export default WeatherForecast