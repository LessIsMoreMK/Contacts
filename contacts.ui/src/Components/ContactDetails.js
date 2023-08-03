import React, { useState, useEffect, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import '.././App.css';
import { CategoryContext } from './Categories';
import { handleErrorMessage } from './ErrorHelper.js';

const ContactDetails = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [contact, setContact] = useState(null);
  const [message, setMessage] = useState(null);
  const [categories] = useContext(CategoryContext);
  const [errorMessage, setErrorMessage] = useState(null);

  useEffect(() => {
    axios.get(`http://localhost:5000/contacts/${id}`, {
      headers: {
        'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
      },
    })
      .then(response => {
        setContact(response.data);
      })
      .catch(error => {
        setErrorMessage(handleErrorMessage(error));
      });
  }, [id]);

  const handleDelete = () => {
    const confirmDelete = window.confirm("Are you sure you want to delete this contact?");
    if (confirmDelete) {
      axios.delete(`http://localhost:5000/contacts/${id}`, {
        headers: {
          'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
        },
      })
        .then(response => {
          navigate("/"); 
        })
        .catch(error => {
            setErrorMessage(handleErrorMessage(error));
        });
    }
  }

  const handleUpdate = () => {
    navigate(`/contacts/${id}/update`);
  }

  if (!contact) {
    return <div style={{textAlign: 'center'}}>{message ? message : 'Loading...'}</div>
  }

  const birthDateObj = new Date(contact.birthDate);
  const formattedBirthDate = birthDateObj.toLocaleDateString();

  const category = categories.find(cat => cat.id === contact.categoryId);
  const subcategory = category ? category.subcategories.find(sub => sub.id === contact.subCategoryId) : null;

  return (
    <div className="contact-details">
      <h2>Contact Details</h2>
      <div className="detail-item">
        <p>First Name:</p>
        <b><p>{contact.firstName || '-'}</p></b>
      </div>
      <div className="detail-item">
        <p>Last Name:</p>
        <b><p>{contact.lastName || '-'}</p></b>
      </div>
      <div className="detail-item">
        <p>Email:</p>
        <b><p>{contact.email || '-'}</p></b>
      </div>
      <div className="detail-item">
        <p>Phone:</p>
        <b><p>{contact.phone || '-'}</p></b>
      </div>
      <div className="detail-item">
        <p>Birth Date:</p>
        <b><p>{formattedBirthDate || '-'}</p></b>
      </div>
      <div className="detail-item">
        <p>Category:</p>
        <b><p>{category ? category.name : '-'}</p></b>
      </div>
      {subcategory && (
        <div className="detail-item">
            <p>Subcategory:</p>
            <b><p>{subcategory.name}</p></b>
        </div>
        )}
      <div className="button-group">
        <a className="action-btn" onClick={handleDelete}>Delete Contact</a>
        <a className="action-btn" onClick={handleUpdate}>Update Contact</a>
      </div>
    </div>
  );
}

export default ContactDetails;
