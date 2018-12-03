import { v1 } from "uuid";
import { gatewayHost } from "@/api";

export const sysSocket = (type) => {
    let key = v1();
    let result = new WebSocket(gatewayHost.replace("https", "wss") + "ws/LY.SysService/" + type + "/" + key);
    result.key = key;
    return result;
}
