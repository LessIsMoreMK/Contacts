import React from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { CategoryProvider } from './Components/Categories';

import Login from './Components/Login';
import Register from './Components/Register';
import Contacts from './Components/Contacts';
import Navbar from './Components/Navbar';
import Logout from './Components/Logout';
import UpdateContact from './Components/UpdateContact';
import ContactDetails from './Components/ContactDetails';
import AddContact from './Components/AddContact';

const App = () => {
  return (
    <Router>
      <Navbar />
      <CategoryProvider>
        <div className="content">
          <Routes>
            <Route path="/" element={<Contacts />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route path="/logout" element={<Logout />} />
            <Route path="/contact/:id" element={<ContactDetails />} />
            <Route path="/contacts/:id/update" element={<UpdateContact />} />
            <Route path="/contacts/new" element={<AddContact />} />
          </Routes>
        </div>
      </CategoryProvider>
    </Router>
  );
}

export default App;