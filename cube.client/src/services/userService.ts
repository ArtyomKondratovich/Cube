import config from "@/config"
import authHeader from "@/helpers/authHeader";
import axios from 'axios'

const API_URL = config.apiUrl;

class UserService {
    getPublicContent() {
        return axios.get(API_URL + 'all');
      }
    
      getUserBoard() {
        return axios.get(API_URL + 'user', { headers: authHeader() });
      }
    
      getModeratorBoard() {
        return axios.get(API_URL + 'mod', { headers: authHeader() });
      }
    
      getAdminBoard() {
        return axios.get(API_URL + 'admin', { headers: authHeader() });
      }
}

export default new UserService()