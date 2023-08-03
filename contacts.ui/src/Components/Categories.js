import React, { createContext, useState, useEffect } from 'react';
import axios from 'axios';

export const CategoryContext = createContext();

export const CategoryProvider = ({ children }) => {
  const [errorMessage, setErrorMessage] = useState(null);
  const [categories, setCategories] = useState([]);

    const fetchCategories = async () => {
        try {
            const response = await axios.get('http://localhost:5000/categories', {
                headers: {
                    'Authorization': 'Bearer ' + localStorage.getItem('jwtToken'),
                },
            });
            setCategories(response.data);
        } catch (error) {
            console.error('Error:', error);
            console.log('Error response:', error.response);
        }
    }

    useEffect(() => {
        fetchCategories();
    }, []);

    return (
      <CategoryContext.Provider value={[categories, fetchCategories]}>
        {children}
      </CategoryContext.Provider>
    );
};