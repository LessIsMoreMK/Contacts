import React, { useState, useEffect, useContext } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import '.././App.css';
import { CategoryContext } from './Categories';
import { handleErrorMessage } from './ErrorHelper.js';

const UpdateContact = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const [contact, setContact] = useState(null);
  const [categories, fetchCategories] = useContext(CategoryContext);
  const [customCategory, setCustomCategory] = useState('');
  const [showSubCategory, setShowSubCategory] = useState(false);
  const [showCustomCategory, setShowCustomCategory] = useState(false);
  const [subCategories, setSubCategories] = useState([]);
  const [businessId, setBusinessId] = useState(null);
  const [otherId, setOtherId] = useState(null);
  const [errorMessage, setErrorMessage] = useState(null);

  useEffect(() => {
    axios.get(`http://localhost:5000/contacts/${id}`, {
      headers: {
        'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
      },
    })
      .then(response => {
        // Format birthDate to be compatible with the input date field format
        response.data.birthDate = response.data.birthDate?.split('T')[0];
        setContact(response.data);
      })
      .catch(error => {
        handleErrorMessage(error)
      });

    const businessCategory = categories.find(category => category.name === 'Business');
    const otherCategory = categories.find(category => category.name === 'Other');
    setBusinessId(businessCategory ? businessCategory.id : null);
    setOtherId(otherCategory ? otherCategory.id : null);
  }, [id, categories]);

  const handleChange = (e) => {
    const updatedContact = {
      ...contact,
      [e.target.name]: e.target.value,
    };

    setContact(updatedContact);

    const selectedCategory = categories.find(category => category.id === updatedContact.categoryId);

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

    let updatedContact = {
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
        updatedContact.categoryId = parsedData.Id;

        fetchCategories();
      } catch (error) {
        handleErrorMessage(error);
        return;
      }
    }

    axios.put(`http://localhost:5000/contacts`, updatedContact, {
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

  if (!contact) {
    return <div>Loading...</div>;
  }

  return (
    <form className="contact-details" onSubmit={handleSubmit}>
      <h2>Update Contact</h2>
      <div className="detail-item">
        <p>First Name:</p>
        <input className="input-box" type="text" name="firstName" value={contact?.firstName || ''} onChange={handleChange} />
      </div>
      <div className="detail-item">
        <p>Last Name:</p>
        <input className="input-box" type="text" name="lastName" value={contact.lastName} onChange={handleChange} />
      </div>
      <div className="detail-item">
        <p>Email:</p>
        <input className="input-box" type="text" name="email" value={contact?.email || ''} onChange={handleChange} />
      </div>
      <div className="detail-item">
        <p>Phone:</p>
        <input className="input-box" type="text" name="phone" value={contact?.phone || ''} onChange={handleChange} />
      </div>
      <div className="detail-item">
        <p>Birth Date:</p>
        <input className="input-box" type="date" name="birthDate" value={contact?.birthDate || ''} onChange={handleChange} />
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
      <button type="submit" className="action-btn">Update</button>
      {errorMessage && <p className="error">{errorMessage}</p>}
    </form>
  );
}

export default UpdateContact;