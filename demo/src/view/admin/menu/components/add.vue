<template>
    <el-dialog :modelValue="addVisible" :title="info == undefined ? '新增' : '修改'" width="30%" :before-close="handleClose">
        <el-form :model="form" label-width="120px" class="form" :rules="rules" ref="ruleFormRef">
            <el-form-item label="菜单路由" prop="index">
                <el-input v-model="form.index" />
            </el-form-item>
            <el-form-item label="菜单名称" prop="name">
                <el-input v-model="form.name" />
            </el-form-item>
            <el-form-item label="文件路径" prop="filePath">
                <el-input v-model="form.filePath" />
            </el-form-item>
            <el-form-item label="父级菜单" prop="parentId">
                <el-tree-select :props="{ value: 'id', label: 'name', children: 'children' }" v-model="form.parentId"
                    :data="treedata" check-strictly />
            </el-form-item>
            <el-form-item label="菜单顺序" prop="order">
                <el-input v-model="form.order" />
            </el-form-item>
            <el-form-item label="是否启用" prop="isEnable">
                <el-switch v-model="form.isEnable" />
            </el-form-item>
            <el-form-item label="描述" prop="description">
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
import { addMenu, editMenu, getMenuDataNew } from '../../../../http';
import { MenuModel } from '../class/MenuModel'
import type { FormInstance, FormRules } from 'element-plus'
import { ElMessage } from 'element-plus'
const ruleFormRef = ref<FormInstance>()
const props = defineProps({
    addVisible: Boolean,
    info: MenuModel
})
const form = ref({
    id: props.info?.id,
    index: '',
    name: '',
    filePath: '',
    parentId: 0,
    order: 99,
    isEnable: false,
    description: ""
})
const rules = reactive<FormRules>({
    name: [{ required: true, message: '请输入名称', trigger: 'blur' }]
})
//监听
watch(
    () => props.info,
    (newInfo, oldInfo) => {
        if (newInfo != undefined) {
            let currInfo: MenuModel = (JSON.parse(newInfo as any)) as MenuModel
            form.value = {
                id: currInfo.id,
                index: currInfo.index,
                name: currInfo.name,
                filePath: currInfo.filePath,
                parentId: currInfo.parentId,
                order: currInfo.order,
                isEnable: currInfo.isEnable,
                description: currInfo.description
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
        Name: "",
        Index: "",
        FilePath: "",
        ParentId: 0,
        Description: "",
        PageIndex: 1,
        PageSize: 10
    }
    treedata.value = (await getMenuDataNew(parms)).data
}
const submit = async (ruleFormRef: FormInstance | undefined) => {
    if (!ruleFormRef) return
    var parms = {
        Id: form.value.id
        , Name: form.value.name
        , Index: form.value.index
        , FilePath: form.value.filePath
        , ParentId: form.value.parentId
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
            const res = await addMenu(parms) as any as boolean
            if (res) {
                ElMessage({
                    message: '添加成功！',
                    type: 'success',
                })
            }
        } else {
            // 执行修改逻辑
            const res = await editMenu(parms) as any as boolean
            if (res) {
                ElMessage({
                    message: '编辑成功！',
                    type: 'success',
                })
            }
        }
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
