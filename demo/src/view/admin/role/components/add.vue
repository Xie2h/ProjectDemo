<template>
    <el-dialog :modelValue="addVisible" :title="info == undefined ? '新增' : '修改'" width="30%" :before-close="handleClose">
        <el-form :model="form" label-width="120px" class="form" :rules="rules" ref="ruleFormRef">
            <el-form-item label="名称" prop="name">
                <el-input v-model="form.name" />
            </el-form-item>
            <el-form-item label="描述" prop="desc">
                <el-input v-model="form.desc" />
            </el-form-item>
            <el-form-item label="菜单顺序" prop="order">
                <el-input v-model="form.order" />
            </el-form-item>
            <el-form-item label="是否启用" prop="isEnable">
                <el-switch v-model="form.isEnable" />
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
import { ref, reactive, defineProps, computed, defineEmits, onMounted, watch } from 'vue'
import { addRole, editRole } from '../../../../http';
import { Role } from '../class/Role'
import type { FormInstance, FormRules } from 'element-plus'
import { ElMessage } from 'element-plus'
const ruleFormRef = ref<FormInstance>()
const props = defineProps({
    addVisible: Boolean,
    info: Role
})
const form = ref({
    id: 0,
    name: '',
    desc: '',
    order: 99,
    isEnable: false
})
const rules = reactive<FormRules>({
    name: [{ required: true, message: '请输入名称', trigger: 'blur' }]
})
//监听
watch(
    () => props.info,
    (newInfo, oldInfo) => {
        if (newInfo != undefined) {
            let currInfo: Role = (JSON.parse(newInfo as any)) as Role
            form.value = {
                id: currInfo.id,
                name: currInfo.name,
                desc: currInfo.description,
                order: currInfo.order,
                isEnable: currInfo.isEnable
            }
        } else {
            form.value = {
                id: 0,
                name: '',
                desc: '',
                order: 99,
                isEnable: false
            }
        }
    }
);
//defineEmits用于定义回调事件，里面是数组，可以定义多个事件
const emits = defineEmits(["CloseAdd"])
const handleClose = (done: () => void) => {
    emits("CloseAdd")
}

const submit = async (ruleFormRef: FormInstance | undefined) => {
    if (!ruleFormRef) return
    var parms = {
        Id: form.value.id, Name: form.value.name, Order: form.value.order, IsEnable: form.value.isEnable, Description: form.value.desc
    }
    const valid = await ruleFormRef.validate()
    if (valid) {
        // 表单校验通过，执行提交操作
        if (props.info == undefined) {
            // 执行添加逻辑 
            const res = await addRole(parms) as any as boolean
            if (res) {
                ElMessage({
                    message: '添加成功！',
                    type: 'success',
                })
            }
        } else {
            // 执行修改逻辑
            const res = await editRole(parms) as any as boolean
            if (res) {
                ElMessage({
                    message: '编辑成功！',
                    type: 'success',
                })
            }
        }
    }


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
