import React from 'react';
import { NavLink } from 'react-router-dom';

const Navbar = () =>{
    return (
      <nav className="navbar">
        <div className="nav-links">
        <NavLink className="nav-link" to="">Contacts</NavLink>
          <NavLink className="nav-link" to="/login">Login</NavLink>
          <NavLink className="nav-link" to="/register">Register</NavLink>
          <NavLink className="nav-link" to="/logout">Logout</NavLink>
        </div>
      </nav>
    );
  }

export default Navbar;