import CookieModule from '../../modules/cookie';

function ApiService() {

  const cookie = CookieModule();
  const baseURL = "https://192.168.0.106:7096/api/";

  const appendRoute = (route) => {
    return baseURL + route;
  };

  const headerConfig = () => {
    const auth = cookie.getCookie("token");

    if (auth) {
      return {
        headers: {
          'Content-Type': 'application/json',
          Authorization: `Bearer ${auth}`
        }
      };
    } else {
      return {
        headers: {
          'Content-Type': 'application/json'
        }
      };
    }
  };

  return {
    appendRoute,
    headerConfig 
  };

}

export default ApiService;