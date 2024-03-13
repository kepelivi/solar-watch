import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const res = await fetch('http://localhost:5132/Auth/Login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ Email: email, Password: password })
      });
      if (!res.ok) {
        throw new Error(`HTTP error! status: ${res.status}`);
      }
      const data = await res.json();
      localStorage.setItem('token', JSON.stringify(data.token));
      navigate('/');
    }
    catch (error) {
      throw error;
    }
  }

  return (
    <div className="login-container">
      <form className='login' onSubmit={e => handleLogin(e)}>
        <label>E-mail</label>
        <input type='text' onChange={e => setEmail(e.target.value)} value={email} required />
        <label>Password</label>
        <input type='password' onChange={e => setPassword(e.target.value)} value={password} required />
        <button type='submit'>Login</button>
      </form>
    </div>
  );
}

export default Login;