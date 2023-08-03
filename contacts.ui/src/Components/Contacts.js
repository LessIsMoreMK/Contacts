import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Link } from 'react-router-dom';
import '.././App.css';

export default function Contacts() {
  const [contacts, setContacts] = useState([]);

  useEffect(() => {
    axios.get('http://localhost:5000/contacts/')
    .then(res => setContacts(res.data))
    .catch(console.error);
  }, []);

  return (
    <div className="app">
      <header style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
        <div className="header-content">
          <h2>Contacts</h2>
        </div>
        <div className="contact-list">
          {contacts.length > 0 ? 
          contacts.map((contact) => (
            <Link to={`/contact/${contact.id}`} key={contact.id} className="action-btn">
              <div key={contact.id}>
                {contact.firstName} {contact.lastName}
              </div>
            </Link>
          ))
          :
          <p>Contacts list empty, login and add your contacts.</p>
          }
        </div>
        <Link to="/contacts/new" className="action-btn" 
          style={{ alignSelf: 'center', marginTop: '20px', width: 'fit-content' }}>New Contact</Link>
      </header>
    </div>
  );
}