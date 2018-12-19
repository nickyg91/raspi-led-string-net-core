import axios, { AxiosRequestConfig, AxiosPromise } from 'axios';
export default class LedService {
    public async setLedColors(green: number, blue: number, red: number) {
        return axios.get(`http://192.168.1.157/api/colorchange/${green}/${red}/${blue}`);
    }
}
