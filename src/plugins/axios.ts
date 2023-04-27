import axios from "axios";

const instance = axios.create({
    baseURL: process.env.NODE_ENV === 'development' ? 'https://localhost:7039' : 'https://testapi.dichvubanker.com',
    timeout: 10000,
    timeoutErrorMessage: 'the request been timeout'
});
// instance.interceptors.request.use((config) => {
//     // const token = GenerateInformation.GetToken(store)
//     const controller = new AbortController();
//     // if(!Helper.IS_NULL_OR_EMPTY(token))
//     // {
//     //     config.headers.Authorization = `Bearer ${token}`
//     // }

//     config.withCredentials = true;
//     const cfg = {
//       ...config,
//       signal: controller.signal,
//     };
//     return cfg;
// }, (error) => {
//     return Promise.reject(error)
// })

// instance.interceptors.response.use((res) => {
//     return res;
// }, (error) => {
//     console.log(error,error.headers)
//     return Promise.reject(error)
// })
export default instance;