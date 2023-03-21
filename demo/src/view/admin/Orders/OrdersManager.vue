<script lang="ts" setup>
import { reactive, ref, onMounted, toRaw } from 'vue'
import type { ElTable } from 'element-plus'
import type { FormInstance, FormRules } from 'element-plus'
import { ElMessage } from 'element-plus'
import { Search, RefreshRight } from '@element-plus/icons-vue'
import addVue from './components/add.vue'
import { OrdersModel } from './class/OrdersModel'
import { getOrdersDataNew, delOrders, batchDelOrders } from '../../../http/index'
const ruleFormRef = ref<FormInstance>()
const multipleTableRef = ref<InstanceType<typeof ElTable>>()
const form = reactive({
    nickName: '',
    goodsName: '',
    customersName: '',

    price: 0,
    num: 0,
    sum: 0,
    userType: 0,
    isEnable: true,
    description: "",
    pageIndex: 1,
    pageSize: 10,
    Total: 0
})
const rules = reactive<FormRules>({
    customersName: [{ required: false, message: '请输入名称', trigger: 'blur' }]
})

const onSubmit = async (ruleFormRef: FormInstance | undefined) => {
    if (!ruleFormRef) return;
    await ruleFormRef.validate((valid, fields) => {
        if (valid) {
            // tableData.value = tableData.value?.filter(s => s.name == form.name) 
            LoadTableData()
            console.log(fields)
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
const handleQuery = (index: number, row: OrdersModel) => {
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
const handleEdit = (index: number, row: OrdersModel) => {
    info.value = JSON.stringify(row)
    addVisible.value = true
}

// //单个删除
const handleDelete = async (index: number, row: OrdersModel) => {
    tableData.value = tableData.value?.filter(s => s.id != row.id)
    const res = await delOrders(row.id) as any as boolean
    if (res) {
        ElMessage({
            message: '删除成功！',
            type: 'success',
        })
    }
}
// //批量删除
const Del = async () => {
    let arr: any[] = multipleTableRef.value?.getSelectionRows()
    let ids: string = arr.map(item => { return "'" + item.id + "'" }).join(',')
    const res = await batchDelOrders(ids) as any as boolean
    if (res) {
        ElMessage({
            message: '删除成功！',
            type: 'success',
        })
        LoadTableData()
    }
}
const queryDialog = ref(false)
const queryDialogClose = () => {
    queryDialog.value = false;
}
//表格
const tableData = ref<Array<OrdersModel>>()
onMounted(async () => {
    LoadTableData()
})
const LoadTableData = async (name: string = "") => {
    let parms = {
        goodsName: form.goodsName,
        customersName: form.customersName,
        num: form.num,
        price: form.price,
        sum: form.sum,
        Index: "",
        FilePath: "",
        nickName: form.nickName,
        Description: form.description,
        PageIndex: form.pageIndex,
        PageSize: form.pageSize
    }
    let res = await getOrdersDataNew(parms) as any
    console.log(res)
    form.Total = res.total
    tableData.value = res.data as OrdersModel[]
    console.log(form.Total)
}
// //分页
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
                    <el-form-item label="客户名称" prop="customersName">
                        <el-input v-model="form.customersName" placeholder="请输入名称" />
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
                </p>
            </div>
        </template>
        <el-table :data="tableData" style="width: 100%" ref="multipleTableRef" row-key="id">
            <el-table-column type="selection" width="55" />
            <el-table-column prop="id" label="订单编号" width="100" />
            <el-table-column label="时间" width="200">
                <template #default="scope">
                    <div>
                        <el-icon>
                            <timer />
                        </el-icon>
                        <span style="margin-left: 10px">{{ scope.row.createDate }}</span>
                    </div>
                </template>
            </el-table-column>
            <el-table-column label="客户名称">
                <template #default="scope">
                    <div>{{ scope.row.customersName }}</div>
                </template>
            </el-table-column>
            <el-table-column label="商品名称">
                <template #default="scope">
                    <div>{{ scope.row.goodsName }}</div>
                </template>
            </el-table-column>
            <el-table-column label="单价">
                <template #default="scope">
                    <div>{{ scope.row.price }}</div>
                </template>
            </el-table-column>

            <el-table-column label="数量">
                <template #default="scope">
                    <div>{{ scope.row.num }}</div>
                </template>
            </el-table-column>
            <el-table-column label="总价">
                <template #default="scope">
                    <div>{{ scope.row.sum }}</div>
                </template>
            </el-table-column>
            <el-table-column label="创建人">
                <template #default="scope">
                    <div>{{ scope.row.nickName }}</div>
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
        <div><el-pagination background :page-size="10" :pager-count="7" layout="prev, pager, next" :total="form.Total"
                @current-change="handleCurrentChange" /></div>

    </el-card>

    <addVue :addVisible="addVisible" :info="info" @CloseAdd="CloseAdd"></addVue>
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