import axios from 'axios'
//需要拦截器的地方使用instance对象， 有自定义返回逻辑的地方沿用axios，在组件内部处理返回结果即可
import instance from './filter'
const http = "http://localhost:5162/api";

//获取token
export const getToken = (name: string, password: string) => {
    return instance.get(http + "/Login/GetToken?name=" + name + "&password=" + password);
}

//订单模块
//获取列表
export const getOrdersDataNew = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Orders/GetOrders", parms)
}
//添加
export const addOrders = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Orders/Add", parms)
}
//修改
export const editOrders = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Orders/Edit", parms)
}
//删除
export const delOrders = async (id: number) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(http + "/Orders/Del?id=" + id)
}
//BatchDel
export const batchDelOrders = async (ids: string) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(http + "/Orders/BatchDel?ids=" + ids)
}


//菜单模块
//获取列表
export const getMenuDataNew = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Menu/GetMenus", parms)
}
//添加
export const addMenu = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Menu/Add", parms)
}
//修改
export const editMenu = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Menu/Edit", parms)
}
//删除
export const delMenu = async (id: number) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(http + "/Menu/Del?id=" + id)
}
//BatchDel
export const batchDelMenu = async (ids: string) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(http + "/Menu/BatchDel?ids=" + ids)
}
//分配菜单
export const settingMenu = async (rid: string, mids: string) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(`${http}/Menu/SettingMenu?rid=${rid}&mids=${mids}`)
}
//角色模块
//获取列表
export const getRoleData = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Role/GetRoles", parms)
}
//添加
export const addRole = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Role/Add", parms)
}
//修改
export const editRole = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Role/Edit", parms)
}
//删除
export const delRole = async (id: number) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(http + "/Role/Del?id=" + id)
}
//BatchDel
export const batchDelRole = async (ids: string) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(http + "/Role/BatchDel?ids=" + ids)
}

//用户模块
//获取列表
export const getUserData = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Users/GetUsers", parms)
}
//添加
export const addUsers = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Users/Add", parms)
}
//修改
export const editUsers = async (parms: {}) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.post(http + "/Users/Edit", parms)
}
//删除
export const delUsers = async (id: number) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(http + "/Users/Del?id=" + id)
}
//BatchDel
export const batchDelUsers = async (ids: string) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(http + "/Users/BatchDel?ids=" + ids)
}
//分配
export const settingRole = async (pid: string, rids: string) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(`${http}/Users/SettingRole?pid=${pid}&rids=${rids}`)
}
//根据角色获取菜单
export const getUserMenus = async () => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(`${http}/Menu/GetUserMenus`)
}
//个人中心修改用户昵称和密码
export const editNickNameOrPassword = async (nickName: string, password: string) => {
    instance.defaults.headers.common['Authorization'] = "Bearer " + localStorage["token"];
    return instance.get(`${http}/Users/EditNickNameOrPassword?nickName=${nickName}&password=${password}`)
}