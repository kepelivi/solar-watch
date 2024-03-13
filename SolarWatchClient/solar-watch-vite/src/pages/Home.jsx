import { Link } from 'react-router-dom'

function Home() {
    return (
        <div className="home-page">
            <div className='home'>
                <h2>Welcome to SolarWatch!</h2>
                <h3>Please <Link to="/login" >log in</Link> or <Link to="/register" >register</Link> to use our website</h3>
                <br></br>
                <h1>Get SolarWatch for a city <Link to="/solar-watch" >here</Link></h1>
            </div>
        </div>
    )
}

export default Home;