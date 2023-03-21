//导入axios
import axios from 'axios'
import { ElMessage } from 'element-plus'
//创建一个axios实例 
const instance = axios.create({
    headers: {
        'content-type': 'application/json',
    },
    // withCredentials: true,
    timeout: 5000  //5秒
})
//http 拦截器
instance.interceptors.response.use(
    response => {
        //拦截请求，统一相应
        if (response.data.isSuccess) {
            return response.data.result
        } else {
            ElMessage.error(response.data.msg)
            return response.data.result
        }
    },
    //error也可以处理
    error => {
        if (error.response) {
            switch (error.response.status) {
                case 401:
                    ElMessage.warning("资源没有访问权限！")
                    break
                case 404:
                    ElMessage.warning("接口不存在，请检查接口地址是否正确！")
                    break
                case 500:
                    ElMessage.warning("内部服务器错误，请联系系统管理员！")
                    break
                default:
                    return Promise.reject(error.response.data)   // 返回接口返回的错误信息 
            }
        }
        else {
            ElMessage.error("遇到跨域错误，请设置代理或者修改后端允许跨域访问！")
        }
    }
)
export default instance