import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

const Logout = () => {
  const navigate = useNavigate();

  useEffect(() => {
    const logout = async () => {
      const response = await fetch('http://localhost:5000/logout', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
      });
  
      if (!response.ok) {
        const data = await response.json();
        console.error(data.error);
        return;
      }
  
      localStorage.removeItem('jwtToken');
  
      navigate("/");
    };

    logout();
  }, [navigate]);

  return null;
}

export default Logout;
