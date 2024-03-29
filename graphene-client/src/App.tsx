import './App.css'
import WeatherForecast from './components/Forecast'
import People from './components/People'
import { OktaAuth, toRelativeUrl } from '@okta/okta-auth-js'
import { Security, LoginCallback } from '@okta/okta-react'
import { Link, Route, Routes, useNavigate } from 'react-router-dom'
import { Loading } from './components/Loading'
import { Home } from './components/Home'
import { RequiredAuth } from './components/SecureRoute'

export const oktaAuth = new OktaAuth({
    issuer: import.meta.env.VITE_OKTA_ISSUER,
    clientId: import.meta.env.VITE_OKTA_CLIENTID,
    redirectUri: `${window.location.origin}/login/callback`,
})

const App = () => {

    //const { authState, oktaAuth } = useOktaAuth()
    const navigate = useNavigate()

    //useEffect(() => {
    //    if (!authState || !authState.isAuthenticated) {
    //        console.log("authenticated")
    //    }
    //})

    const restoreOriginalUri = async (_oktaAuth: OktaAuth, originalUri: string) => {
        navigate(toRelativeUrl(originalUri || '/', window.location.origin))
    }

    const login = async () => await oktaAuth.signInWithRedirect({ originalUri: "/" })
    //const logout = async () => await oktaAuth.signOut()

    return (
        <div>
            <div>Okta Issuer: {import.meta.env.VITE_OKTA_ISSUER}</div>
            <div>Okta ClientID: {import.meta.env.VITE_OKTA_CLIENTID}</div>
            <div>Version : {import.meta.env.VITE_PACKAGE_VERSION}</div>
            <Security oktaAuth={oktaAuth}
                restoreOriginalUri={restoreOriginalUri}>
                <div>
                    <nav>
                        <Link to="/">Home</Link>
                        <Link to="/weatherForecast">Weather Forecast</Link>
                        <Link to="/people">People</Link>
                    </nav>

                    {/*<div>*/}
                    {/*    {!authState?.isAuthenticated && <button onClick={login}>Log In</button>}*/}
                    {/*    {authState?.isAuthenticated && <button onClick={logout}>Log Out</button>}*/}
                    {/*</div>*/}
                </div>
                <button onClick={login}>Login</button>
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/weatherForecast" element={<RequiredAuth />}>
                        <Route path="" element={<WeatherForecast />} />
                    </Route>
                    <Route path="/people" element={<People /> } />
                    <Route path="/login/callback" element={<LoginCallback loadingElement={<Loading />} />} />
                </Routes>
            </Security>
        </div>
    )
}

export default App
