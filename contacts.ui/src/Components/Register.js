import React, { useState } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';

const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5000';

const Register = () => {
  const [form, setForm] = useState({
    username: '',
    password: '',
    message: '',
  });

  const handleInputChange = ({ target: { name, value } }) => {
    setForm(prevForm => ({ ...prevForm, [name]: value }));
  }

  const register = async (event) => {
    event.preventDefault();

    try {
      await axios.post(`${API_URL}/users`, {
        username: form.username,
        password: form.password,
      });

      setForm(prevForm => ({ ...prevForm, message: "User added successfully" })); 
    } catch (error) {
      const errorMessage = error.response 
        ? error.response.data.message
        : 'An error occurred';

      setForm(prevForm => ({ ...prevForm, message: errorMessage })); 
    }
  }

  return (
    <div className="form-container">
        <h2>Register</h2>
        <form onSubmit={register} className="form">
            <input
                type="text"
                name="username"
                placeholder="Username"
                onChange={handleInputChange}
                className="form-input"
            />
            <input
                type="password"
                name="password"
                placeholder="Password"
                onChange={handleInputChange}
                className="form-input"
            />
            <button type="submit" className="form-button">Register</button>
        </form>
        <div>{form.message}</div>
    </div>
  );
}

Register.propTypes = {
  username: PropTypes.string,
  password: PropTypes.string,
  message: PropTypes.string,
}

export default Register;