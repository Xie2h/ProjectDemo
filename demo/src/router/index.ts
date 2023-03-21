import { createRouter, createWebHistory } from 'vue-router'
import LoginPage from '../view/index/LoginPage.vue'
import RootPage from '../view/index/RootPage.vue'
import DeskTop from '../view/index/DeskTop.vue'
import PersonCenter from '../view/index/PersonCenter.vue'
import { MenuModel } from '../view/admin/menu/class/MenuModel'
import Tool from '../global'
import { UserInfo } from '../../src/view/index/class/UserInfo'
import { getUserMenus } from '../http/index'
let routes = [
  {
    path: "/", component: RootPage,
    children: [
      { name: "工作台", path: "/desktop", component: DeskTop },
      { name: "个人信息", path: "/person", component: PersonCenter },
    ]
  },
  { path: "/login", component: LoginPage }
]

const toolObj = new Tool()

//当前存在用户信息且有效，才会读取动态路由
if (localStorage["nickname"] != undefined) {
  const user: UserInfo = JSON.parse(new Tool().FormatToken(localStorage["token"]))
  const expDate = toolObj.FormatDate(user.exp)
  const currDate = toolObj.GetDate()
  if (expDate >= currDate) {
    //读取webapi，获取路由列表
    const list: MenuModel[] = await getUserMenus() as any as MenuModel[]
    let data = []
    if (list.length > 0) {
      for (let i = 0; i < list.length; i++) {
        //PS1：这里import动态路由导入，需要通过string+变量的方式导入，如果直接传入一个变量，编译器无法成功解析！ 
        //PS2：动态导入提示警告 需要加上/* @vite-ignore */才能消除警告
        data.push({ name: list[i].name, path: list[i].index, component: () => import(/* @vite-ignore */`${list[i].filePath}.vue`) })
      }
      routes[0].children = routes[0].children?.concat(data as []) as []
    }
  }
}

//创建路由
const router = createRouter({
  history: createWebHistory(),
  routes: routes
})

//路由守卫
router.beforeEach((to, form) => {
  if (localStorage["nickname"] != undefined) {
    const user: UserInfo = JSON.parse(new Tool().FormatToken(localStorage["token"]))
    const expDate = toolObj.FormatDate(user.exp)
    const currDate = toolObj.GetDate()
    if (to.path == "/login") {
      if (expDate >= currDate) {
        return { path: "/desktop" }
      } else {
        toolObj.ClearLocalStorage()
      }
    } else {
      if (expDate < currDate) {
        toolObj.ClearLocalStorage()
        return { path: "/login" }
      }
    }
  } else {
    //避免无限重定向，因此要做个判断
    if (to.path !== "/login") {
      return { path: "/login" }
    }
  }
})
export default router;