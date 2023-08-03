import React, { useState, useContext, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import axios from 'axios';
import { CategoryContext } from './Categories';
import { handleErrorMessage } from './ErrorHelper.js';

const AddContact = () => {
  const navigate = useNavigate();
  const [subCategories, setSubCategories] = useState([]);
  const [customCategory, setCustomCategory] = useState('');
  const [showSubCategory, setShowSubCategory] = useState(false);
  const [showCustomCategory, setShowCustomCategory] = useState(false);
  const [errorMessage, setErrorMessage] = useState(null);
  const [businessId, setBusinessId] = useState(null);
  const [otherId, setOtherId] = useState(null);
  const [categories, fetchCategories] = useContext(CategoryContext);
  const [contact, setContact] = useState({
    firstName: '',
    lastName: '',
    email: '',
    phone: '',
    birthDate: '',
    categoryId: '',
    subCategoryId: '',
  });

  useEffect(() => {
    const businessCategory = categories.find(category => category.name === 'Business');
    const otherCategory = categories.find(category => category.name === 'Other');
    setBusinessId(businessCategory ? businessCategory.id : null);
    setOtherId(otherCategory ? otherCategory.id : null);
  }, [categories]);

const handleChange = (e) => {
  const newContact = {
    ...contact,
    [e.target.name]: e.target.value,
  };

  setContact(newContact);

  const selectedCategory = categories.find(category => category.id === newContact.categoryId);

  if (e.target.name === 'categoryId' && selectedCategory) {
    setSubCategories(selectedCategory.subcategories || []);
    if (selectedCategory.id === businessId) {
      setShowSubCategory(true);
      setShowCustomCategory(false);
    } else if (selectedCategory.id === otherId) {
      setShowSubCategory(false);
      setShowCustomCategory(true);
    } else {
      setShowSubCategory(false);
      setShowCustomCategory(false);
    }
  }
}

const handleSubmit = async (e) => {
  e.preventDefault();

  setErrorMessage(null);  

  const formattedContact = {
    ...contact,
    birthDate: contact.birthDate !== '' ? `${contact.birthDate}T00:00:00Z` : null,
    subCategoryId: contact.categoryId === businessId ? contact.subCategoryId : null,
    categoryId: contact.categoryId !== '' ? contact.categoryId : null,
  };

  if (contact.categoryId === otherId) {
    try {
      const categoryResponse = await axios.post('http://localhost:5000/categories', { name: customCategory }, {
          headers: {
              'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
          },
      });

      const parsedData = JSON.parse(categoryResponse.data.message);
      formattedContact.categoryId = parsedData.Id;
      
      fetchCategories();
    } catch (error) {
      setErrorMessage(error.response.data.error || 'An error occurred');
      return;
    }
  }

  axios.post('http://localhost:5000/contacts/', formattedContact, {
    headers: {
      'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
    },
  })
  .then(response => {
    navigate('/');
  })
  .catch(error => {
    setErrorMessage(handleErrorMessage(error));
  });
}
  
  return (
    <form className="contact-details" onSubmit={handleSubmit}>
      <h2>Add Contact</h2>
      <div className="detail-item">
        <p>First Name:</p>
        <input className="input-box" type="text" name="firstName" onChange={handleChange} />
      </div>
      <div className="detail-item">
        <p>Last Name:</p>
        <input className="input-box" type="text" name="lastName" onChange={handleChange} />
      </div>
      <div className="detail-item">
        <p>Email:</p>
        <input className="input-box" type="text" name="email" onChange={handleChange} />
      </div>
      <div className="detail-item">
        <p>Phone:</p>
        <input className="input-box" type="text" name="phone" onChange={handleChange} />
      </div>
      <div className="detail-item">
        <p>Birth Date:</p>
        <input className="input-box" type="date" name="birthDate" onChange={handleChange} />
      </div>
      <div className="detail-item">
        <p>Category:</p>
        <select className="input-box" name="categoryId" value={contact.categoryId} onChange={handleChange}>
          <option value="">Select a category</option>
          {categories && categories.map(category => (
            <option key={category.id} value={category.id}>{category.name}</option>
          ))}
        </select>
      </div>
      {showSubCategory && subCategories.length > 0 && (
        <div className="detail-item">
          <p>Sub Category:</p>
          <select className="input-box" name="subCategoryId" value={contact.subCategoryId} onChange={handleChange}>
            <option value="">Select a sub category</option>
            {subCategories.map(subCategory => (
              <option key={subCategory.id} value={subCategory.id}>{subCategory.name}</option>
            ))}
          </select>
        </div>
      )}

      {showCustomCategory && (
        <div className="detail-item">
          <p>Custom Category:</p>
          <input className="input-box" type="text" value={customCategory} onChange={(e) => setCustomCategory(e.target.value)} />
        </div>
      )}
      <button type="submit" className="action-btn">Create</button>
      {errorMessage && <p className="error">{errorMessage}</p>}
    </form>
  );
}

export default AddContact;
