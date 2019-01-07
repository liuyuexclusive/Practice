import axios from "axios";
let base = "http://localhost:9000/";


export const gatewayHost = base

export const request = (url, method, params) => {
    var config = {
        url: `${gatewayHost}` + url,
        method: method,
        headers: { Authorization: "Bearer " + localStorage.token }
    }

    if (method === "get") {
        config.params = params
    } else {
        config.data = params
    }

    return axios(config)
        .then(data => {
            var result = data.data;
            if (!result.Success) {
                alert(result.Message)
            } else {
                return result;
            }
        })
        .catch(error => {
            switch (error.response.status) {
                case 401:
                    alert('登录失效，请重新登录');
                    window.location.href = "http://" + window.location.host + "/#/";
                    break;
                case 404:
                    alert(`接口${error.response.config.url}不存在`);
                    break;
                default:
                    alert(`${error.response.config.statusText}`);
                    break;
            }
        });
}


