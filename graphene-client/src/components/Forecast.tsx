import { gql, useMutation, useQuery } from '@apollo/client'
import { useCallback, useEffect, useState } from 'react'

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

  const [date, setDate] = useState<Date>(new Date())
  const [summary, setSummary] = useState<string>('Freezing')
  const [temperatureC, setTemperatureC] = useState<number>(Math.round(Math.random() * 100))


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

   
    const newForecast: ForecastInput = {
      date: date.toDateString(),
      summary: summary,
      temperatureC: temperatureC
    }
    sendForecast({
      variables: { forecast: newForecast }
    })
  }, [sendForecast])

  if (!data)
      return <h2>No data</h2>

  if (loading)
    return <p><em>Loading... Please refresh once the ASP.NET backend has started.</em></p>


    
    return (
      <>

      <h2 id="tabelLabel">Weather forecast</h2>

      <form>
        <input type="text" placeholder='Date' value={date.toDateString()} onChange={e => setDate(new Date(e.target.value))} />
        <input type="text" placeholder='Summary' value={summary} onChange={e => setSummary(e.target.value)} />
        <input type="text" placeholder='TemperatureC' value={temperatureC} onChange={e => setTemperatureC(parseInt(e.target.value))} />
      </form>

      <button type="button" onClick={onAddClick}>Add Forecast</button>
      
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
          {data && data.weatherForecast.map(forecast =>
            <tr key={forecast.date}>
              <td>{forecast.date}</td>
              <td>{forecast.temperatureC}</td>
              <td>{forecast.temperatureF}</td>
              <td>{forecast.summary}</td>
            </tr>
          )}
        </tbody>
      </table>
      </>
    )
  }

export default WeatherForecast