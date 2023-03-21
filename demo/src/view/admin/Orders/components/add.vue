<template>
    <el-dialog :modelValue="addVisible" :title="info == undefined ? '新增' : '修改'" width="30%" :before-close="handleClose">
        <el-form :model="form" label-width="120px" class="form" :rules="rules" ref="ruleFormRef">
            <el-form-item label="客户名称" prop="customersName">
                <el-input v-model="form.customersName" />
            </el-form-item>
            <el-form-item label="商品名称" prop="goodsName">
                <el-input v-model="form.goodsName" />
            </el-form-item>
            <el-form-item label="单价" prop="price">
                <el-input v-model="form.price" />
            </el-form-item>
            <el-form-item label="数量" prop="num">
                <el-input v-model="form.num" />
            </el-form-item>
            <el-form-item label="备注" prop="description">
                <el-input v-model="form.description" />
            </el-form-item>
            <!-- 按钮操作组 -->
            <el-form-item class="btn">
                <el-button type="primary" @click="submit(ruleFormRef)">确定</el-button>
                <el-button @click="close(ruleFormRef)">取消</el-button>
            </el-form-item>
        </el-form>
    </el-dialog>
</template>

<script lang="ts" setup>
import { ref, reactive, defineProps, computed, defineEmits, onMounted, watch, toRaw } from 'vue'
import { addOrders, editOrders, getOrdersDataNew } from '../../../../http';
import { OrdersModel } from '../class/OrdersModel'
import type { FormInstance, FormRules } from 'element-plus'
import { ElMessage } from 'element-plus'
const ruleFormRef = ref<FormInstance>()
const props = defineProps({
    addVisible: Boolean,
    info: OrdersModel
})
const form = ref({
    id: props.info?.id,
    index: '',
    goodsName: '',
    customersName: '',
    NickName: '',
    price: 0,
    num: 0,
    sum: 0,
    filePath: '',
    order: 99,
    isEnable: false,
    description: ""
})
const rules = reactive<FormRules>({
    goodsName: [{ required: true, message: '请输入商品名称', trigger: 'blur' }],
    customersName: [{ required: true, message: '请输入客户名称', trigger: 'blur' }],
    price: [{ required: true, message: '请输入商品单价', trigger: 'blur' }, { type: 'number', message: '请输入正确数据', trigger: 'blur', transform: (value) => Number(value) }],
    num: [{ required: true, message: '请输入商品数量', trigger: 'blur' }, { type: 'number', message: '请输入正确数据', trigger: 'blur', transform: (value) => Number(value) }],
})
//监听
watch(
    () => props.info,
    (newInfo, oldInfo) => {
        if (newInfo != undefined) {
            let currInfo: OrdersModel = (JSON.parse(newInfo as any)) as OrdersModel
            form.value = {
                id: currInfo.id,//
                index: currInfo.index,
                goodsName: currInfo.goodsName,//
                customersName: currInfo.customersName,//
                NickName: currInfo.NickName,//
                price: currInfo.price,//
                num: currInfo.num,//
                sum: currInfo.sum,//
                filePath: currInfo.filePath,
                order: currInfo.order,
                isEnable: currInfo.isEnable,
                description: currInfo.description//
            }
        }
    }
);
// defineEmits用于定义回调事件，里面是数组，可以定义多个事件
const emits = defineEmits(["CloseAdd"])
const handleClose = (done: () => void) => {
    emits("CloseAdd")
}
//读取下拉数据
const treedata = ref()
onMounted(() => {
    LoadMenuData()
})
const LoadMenuData = async () => {
    let parms = {
        goodsName: "",
        customersName: "",
        NickName: "",
        Index: "",
        FilePath: "",
        ParentId: 0,
        Description: "",
        PageIndex: 1,
        PageSize: 10
    }
    treedata.value = (await getOrdersDataNew(parms)).data
}
const submit = async (ruleFormRef: FormInstance | undefined) => {
    if (!ruleFormRef) return
    var parms = {
        Id: form.value.id
        , GoodsName: form.value.goodsName
        , customersName: form.value.customersName
        , price: form.value.price
        , num: form.value.num
        , sum: form.value.sum
        , Index: form.value.index
        , FilePath: form.value.filePath
        , Order: form.value.order
        , IsEnable: form.value.isEnable
        , Description: form.value.description
    }
    console.log(parms)
    const valid = await ruleFormRef.validate()
    if (valid) {
        // 表单校验通过，执行提交操作

        if (props.info == undefined) {
            // 执行添加逻辑 

            const res = await addOrders(parms) as any as boolean
            console.log(res);
            if (res) {
                ElMessage({
                    message: '添加成功！',
                    type: 'success',
                })
            }
        } else {
            // 执行修改逻辑

            const res = await editOrders(parms) as any as boolean
            if (res) {
                ElMessage({
                    message: '编辑成功！',
                    type: 'success',
                })
            }
        }
    } else {
        // 表单校验不通过，阻止提交操作
        console.log('表单校验不通过')

    }

    //添加菜单之后重新加载下拉框
    LoadMenuData()
    ruleFormRef.resetFields()
    emits("CloseAdd")
}
const close = (ruleFormRef: FormInstance | undefined) => {
    if (!ruleFormRef) return
    ruleFormRef.resetFields()
    emits("CloseAdd")
}
</script>

<style lang="scss" scoped>
.form {
    min-height: 500px;

    .btn {
        position: absolute;
        bottom: 10px;
    }
}
</style>
