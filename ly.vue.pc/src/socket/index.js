import { v1 } from "uuid";
import { gatewayHost } from "@/api";

export const lysocket = (type) => {
    let key = v1();
    let result = new WebSocket(gatewayHost.replace("http", "ws") + "ws/" + type + "/" + key);
    result.key = key;
    return result;
}
