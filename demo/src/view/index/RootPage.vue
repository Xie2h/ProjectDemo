<template>
  <div class="common-layout">
    <el-container>
      <el-aside class="aside-menu">
        <el-row>
          <el-col :span="24">
            <div class="homepageLogo">
              <ul>
                <li>
                  <el-image style="width: 50px; height: 50px" :src="url" fit="fill" />
                </li>
                <li><span>Donvv</span></li>
              </ul>
            </div>
          </el-col>
        </el-row>
        <el-row class="tac">
          <el-col :span="24">
            <el-menu active-text-color="#ffd04b" background-color="#545c64" class="el-menu-vertical-demo"
              default-active="2" text-color="#fff" router @select="handleSelect">
              <!-- 默认会有个首页的入口 -->
              <el-menu-item index='/desktop'>
                <template #title>
                  <el-icon>
                    <list />
                  </el-icon>
                  <span>工作台</span>
                </template>
              </el-menu-item>

              <!-- 用动态路由的方式去实现嵌套菜单！ -->
              <TreeMenuVue :list="res"></TreeMenuVue>
            </el-menu>
          </el-col>
        </el-row>
      </el-aside>
      <el-container>
        <el-header>
          <HeaderCom></HeaderCom>
        </el-header>
        <el-main>
          <router-view></router-view>
        </el-main>
      </el-container>
    </el-container>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, toRef } from 'vue'
import HeaderCom from '../../components/HeaderCom.vue'
import TreeMenuVue from '../../components/TreeMenu.vue';
import { useRouter, useRoute } from 'vue-router'
import { useStore } from 'vuex'
import { TagModel } from '../../class/TagModel';
import { getUserMenus } from '../../http/index'
import { MenuModel } from '../admin/menu/class/MenuModel';
const url = ref('/images/logo.ico')
url.value = "/images/icon.png"


const res = ref()

onMounted(async () => {
  res.value = await getUserMenus() as any as MenuModel[]
})
const store = useStore()
const router = useRouter()
const handleSelect = (index: string) => {
  //根据index从路由列表中获取name
  let name = router.getRoutes().filter(item => item.path == "/" + index)[0].name as string
  //修改vuex的值-标签值的改变
  let model = new TagModel()
  model.index = index
  model.name = name
  store.commit("AddTag", model)
}
</script>


<style lang="scss" scoped>
.homepageLogo {
  height: 50px;
  line-height: 50px;

  span {
    color: white;
    font-size: 24px;
  }

  ul {
    li {
      float: left;
      margin-left: 5px;
    }
  }
}
</style>




