import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

function Registration() {
    const [userEmail, setUserEmail] = useState('');
    const [userName, setUserName] = useState('');
    const [userPassword, setUserPassword] = useState('');
    const navigate = useNavigate();

    const handleSubmit = async e => {
        e.preventDefault();
        try {
            const res = await fetch('http://localhost:5132/Auth/Register', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Email: userEmail, Username: userName, Password: userPassword })
            });
            if (!res.ok) {
                throw new Error(`HTTP error! status: ${res.status}`);
            }
            navigate('/');
        }
        catch (error) {
            throw error;
        }
    }

    return (
        <div className="register-container">
            <form className='register' onSubmit={handleSubmit}>
                <label>E-mail</label>
                <input type='email' onChange={e => setUserEmail(e.target.value)} value={userEmail} required />
                <label>Username</label>
                <input type='text' onChange={e => setUserName(e.target.value)} value={userName} required />
                <label>Password</label>
                <input type='password' onChange={e => setUserPassword(e.target.value)} value={userPassword} required />
                <button type='submit'>Register</button>
            </form>
        </div>
    );
}

export default Registration;