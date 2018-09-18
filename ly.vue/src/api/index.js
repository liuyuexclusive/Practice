import axios from "axios";
let base = "http://localhost:58976/";

let authorization = { Authorization: "Bearer " + localStorage.token };

var request = (url, method, params) => {
    var config = {
        url: `${base}`+ url,
        method: method,
        headers: { Authorization: "Bearer " + localStorage.token }
    }
    if (method === "get") {
        config.params = params
    }
    else {
        config.data = params
    }

    return axios(config).then(data => {
        return data.data;
    }).catch(error => {
        if (error.response.status === 401) {
            alert("登录失效，请重新登录");
            localStorage.removeItem("token");
            window.location.href = "http://" + window.location.host + "/#/"
        }
        return error;
    });
}

export const login = params => request(`user/login`, "put", params)
export const register = params => request(`user/register`, "post", params)
export const getValidateCode = params => request(`user/getValidateCode`, "get", params)
export const getRoles = params => request(`role`, "get", params)
export const addRole = params => request(`role/add`, "post", params)
export const getTest = params => request(`user/getest`, "get", params)
