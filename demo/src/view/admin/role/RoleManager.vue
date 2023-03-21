<script lang="ts" setup>
import { reactive, ref, onMounted, toRaw } from 'vue'
import type { ElTable } from 'element-plus'
import type { FormInstance, FormRules } from 'element-plus'
import { ElMessage } from 'element-plus'
import { Search, RefreshRight } from '@element-plus/icons-vue'
import addVue from './components/add.vue'
import { Role } from '../role/class/Role'
import { getRoleData, delRole, batchDelRole } from '../../../http/index'
import setingMenu from './components/setingMenu.vue'
const ruleFormRef = ref<FormInstance>()
const multipleTableRef = ref<InstanceType<typeof ElTable>>()
const form = reactive({
  name: "",
  isEnable: true,
  description: "",
  pageIndex: 1,
  pageSize: 10,
  Total: 0
})
const rules = reactive<FormRules>({
  name: [{ required: false, message: '请输入名称', trigger: 'blur' }]
})

const onSubmit = async (ruleFormRef: FormInstance | undefined) => {
  if (!ruleFormRef) return;
  await ruleFormRef.validate((valid, fields) => {
    if (valid) {
      // tableData.value = tableData.value?.filter(s => s.name == form.name) 
      LoadTableData()
    } else {
      console.log('error submit!', fields)
    }
  })
}
const resetForm = (ruleFormRef: FormInstance | undefined) => {
  if (!ruleFormRef) return
  ruleFormRef.resetFields()
  LoadTableData()
}
const handleQuery = (index: number, row: Role) => {
  console.log(index, row)
  queryDialog.value = true
}
const addVisible = ref(false)
const add = () => {
  addVisible.value = true
}
const CloseAdd = () => {
  addVisible.value = false
  info.value = undefined
  LoadTableData()
}
const info = ref()
const handleEdit = (index: number, row: Role) => {
  info.value = JSON.stringify(row)
  addVisible.value = true
}
//单个删除
const handleDelete = async (index: number, row: Role) => {
  tableData.value = tableData.value?.filter(s => s.id != row.id)
  const res = await delRole(row.id) as any as boolean
  if (res) {
    ElMessage({
      message: '删除成功！',
      type: 'success',
    })
  }
}
//批量删除
const Del = async () => {
  let arr: any[] = multipleTableRef.value?.getSelectionRows()
  let ids: string = arr.map(item => { return "'" + item.id + "'" }).join(',')
  const res = await batchDelRole(ids) as any as boolean
  if (res) {
    ElMessage({
      message: '删除成功！',
      type: 'success',
    })
    LoadTableData()
  }
}
//分配菜单
const setingVisible = ref(false)
const roleId = ref()
const Seting = () => {
  //获取当前选择的行
  let arr: any[] = multipleTableRef.value?.getSelectionRows()
  if (arr.length == 0) {
    ElMessage({
      message: '请选择需要分配角色的用户！',
      type: 'warning',
    })
  } else if (arr.length > 1) {
    ElMessage({
      message: '只能选择单个用户！',
      type: 'warning',
    })
  }
  else {
    roleId.value = toRaw(arr[0].id)
    setingVisible.value = true
  }
}
const CloseSeting = () => {
  setingVisible.value = false
  LoadTableData()
}
const queryDialog = ref(false)
const queryDialogClose = () => {
  queryDialog.value = false;
}
//表格
const tableData = ref<Array<Role>>()
onMounted(async () => {
  LoadTableData()
})
const LoadTableData = async (name: string = "") => {
  let parms = {
    Name: form.name,
    IsEnable: form.isEnable,
    Description: form.description,
    PageIndex: form.pageIndex,
    PageSize: form.pageSize
  }
  let res = await getRoleData(parms) as any
  form.Total = res.total
  tableData.value = res.data as Role[]
}
//分页
const handleCurrentChange = (val: number) => {
  form.pageIndex = val
  LoadTableData()
}
</script>
<template>
  <el-card class="box-card">
    <template #header>
      <div class="card-header">
        <el-form :inline="true" :model="form" class="demo-form-inline" :rules="rules" ref="ruleFormRef">
          <el-form-item label="名称" prop="name">
            <el-input v-model="form.name" placeholder="请输入名称" />
          </el-form-item>
          <el-form-item>

          </el-form-item>
          <el-form-item>
            <el-button type="primary" @click="onSubmit(ruleFormRef)">
              <el-icon>
                <search />
              </el-icon>查询
            </el-button>
            <el-button @click="resetForm(ruleFormRef)">
              <el-icon>
                <refresh-right />
              </el-icon>重置
            </el-button>
          </el-form-item>
        </el-form>
        <p>
          <el-button type="primary" @click="add">新增</el-button>
          <el-button type="danger" @click="Del">删除</el-button>
          <el-button type="primary" @click="Seting">分配菜单</el-button>
        </p>
      </div>
    </template>
    <el-table :data="tableData" style="width: 100%" ref="multipleTableRef">
      <el-table-column type="selection" width="55" />
      <el-table-column label="序号" width="70">
        <template #default="scope">
          <div>{{ scope.row.id }}</div>
        </template>
      </el-table-column>
      <el-table-column label="名称">
        <template #default="scope">
          <div>{{ scope.row.name }}</div>
        </template>
      </el-table-column>
      <el-table-column label="描述">
        <template #default="scope">
          <div>{{ scope.row.description }}</div>
        </template>
      </el-table-column>
      <el-table-column label="排序">
        <template #default="scope">
          <div>{{ scope.row.order }}</div>
        </template>
      </el-table-column>
      <el-table-column label="是否启用">
        <template #default="scope">
          <div>{{ scope.row.isEnable }}</div>
        </template>
      </el-table-column>
      <el-table-column label="操作" align="right" width="200">
        <template #default="scope">
          <!-- <el-button size="small" type="success" @click="handleQuery(scope.$index, scope.row)">详情</el-button> -->
          <el-button size="small" type="primary" @click="handleEdit(scope.$index, scope.row)">编辑</el-button>
          <el-button size="small" type="danger" @click="handleDelete(scope.$index, scope.row)">删除</el-button>
        </template>
      </el-table-column>
    </el-table>
    <el-pagination background layout="prev, pager, next" :total="form.Total" @current-change="handleCurrentChange" />
  </el-card>
  <el-dialog v-model="queryDialog" title="详情" width="30%" :before-close="queryDialogClose">
    <table class="queryTable">
      <tr>
        <td class="left">列名</td>
        <td class="right">this is name </td>
      </tr>
      <tr>
        <td class="left">列名</td>
        <td class="right">this is name </td>
      </tr>
      <tr>
        <td class="left">列名</td>
        <td class="right">this is name </td>
      </tr>
      <tr>
        <td class="left">列名</td>
        <td class="right">this is name </td>
      </tr>
      <tr>
        <td class="left">列名</td>
        <td class="right">this is name </td>
      </tr>
      <tr>
        <td class="left">列名</td>
        <td class="right">this is name </td>
      </tr>
      <tr>
        <td class="left">列名</td>
        <td class="right">this is name </td>
      </tr>
      <tr>
        <td class="left">列名</td>
        <td class="right">this is name </td>
      </tr>
    </table>
  </el-dialog>
  <addVue :addVisible="addVisible" :info="info" @CloseAdd="CloseAdd"></addVue>
  <setingMenu :setingVisible="setingVisible" :roleId="roleId" @CloseSeting="CloseSeting"></setingMenu>
</template>
<style lang="scss" scoped>
.el-pagination {
  margin-top: 50px;
}

.cell {
  text-align: center !important;
}

.queryTable {
  width: 500px;
  border-collapse: collapse;

  tr {
    height: 50px;

    td {
      padding: 10px;
    }

    .left {
      width: 30%;
      background-color: #F5F7FA;
    }

    .right {
      width: 80%;
    }
  }
}
</style>