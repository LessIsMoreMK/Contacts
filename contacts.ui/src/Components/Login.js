import React, { useState } from 'react';
import axios from 'axios';
import { handleErrorMessage } from './ErrorHelper.js';

export default function Login() {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [message, setMessage] = useState('');

  const login = async (event) => {
    event.preventDefault();

    axios.post('http://localhost:5000/login', {
        username,
        password,
    })
    .then((response) => {
        localStorage.setItem('jwtToken', response.data.token);
        setMessage('Login successful');
    })
    .catch((error) => {
        setMessage(handleErrorMessage(error));
    });
  }

  return (
    <div className="form-container">
      <h2>Login</h2>
      <form onSubmit={login} className="form">
        <input
            type="text"
            placeholder="Username"
            onChange={e => setUsername(e.target.value)}
            className="form-input"
        />
        <input
            type="password"
            placeholder="Password"
            onChange={e => setPassword(e.target.value)}
            className="form-input"
        />
        <button type="submit" className="form-button">Login</button>
      </form>
      <div>{message}</div>
    </div>
  );
}