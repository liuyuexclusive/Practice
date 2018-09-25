import axios from "axios";
import { Message, MessageBox } from "element-ui";
let base = "http://localhost:58976/";

export const request = (url, method, params) => {
    var config = {
        url: `${base}` + url,
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
                Message({ message: result.Message, type: "error" })
            } else {
                if (result.Message && result.Message != "") {
                    Message({ message: result.Message, type: "success" })
                }
                return result;
            }
        })
        .catch(error => {
            switch (error.response.status) {
                case 401:
                    MessageBox.alert(`登录失效，请重新登录`, "", { type: "error" }).then(() => {
                        localStorage.removeItem("token");
                        window.location.href = "http://" + window.location.host + "/#/"
                    })
                    break;
                case 404:
                    Message({ message: `接口${error.response.config.url}不存在`, type: "error" })
                    break;
                default:
                    Message({ message: `${error.response.config.statusText}`, type: "error" })
                    break;
            }
        });
}
