export const handleErrorMessage = (error) => {
    console.error('Error:', error);
    if (error.response.status === 401) { //TODO: Why is this not working? CORS?
        return "You need to login for this action.";
    } else if (error.response) {
      return error.response.data.error || error.response.data.message || 'An error occurred';
    } 
    else {
      return 'An error occurred. Login and try again.';
    }
  }