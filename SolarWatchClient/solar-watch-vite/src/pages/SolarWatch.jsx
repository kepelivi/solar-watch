import { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';

function SolarWatch() {
    const [solar, setSolar] = useState(null);
    const [city, setCity] = useState('');
    const [date, setDate] = useState('');

    const token = JSON.parse(localStorage.getItem('token'));

    async function fetchSolar(e) {
        e.preventDefault();
        const res = await fetch(`http://localhost:5132/SolarWatch/GetSolarWatch?cityName=${city}&date=${date}`, {
            headers: { Authorization: `Bearer ${token}` }
        })
        await res.json()
            .then(s => setSolar(s));
    }

    return (
        <div className='solar-page'>
            {token ? (
                <div className='get-solar-container'>
                    <form className='get-solar-form' onSubmit={e => fetchSolar(e)}>
                        <label>City</label>
                        <input type='text' onChange={e => setCity(e.target.value)} value={city} required />
                        <label>Date</label>
                        <input type='date' onChange={e => setDate(e.target.value)} value={date} required />
                        <button type='submit'>Get solar data</button>
                    </form>
                    {solar && (
                        <div className='solar-data-container'>
                            <h3>Sunrise: {solar.sunrise}</h3>
                            <h3>Sunset: {solar.sunset}</h3>
                        </div>
                    )}
                </div>
            ) : (
                <h2>Please <Link to='/login'>login</Link> to use SolarWatch.</h2>
            )}
        </div>
    )
}

export default SolarWatch;