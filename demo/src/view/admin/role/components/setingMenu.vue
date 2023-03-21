<template>
    <el-dialog :modelValue="setingVisible" title="分配菜单" width="50%" :before-close="handleClose">
        <div class="content">
            <p>
                <el-tree-v2 :data="treedata" :props="{ value: 'id', label: 'name', children: 'children' }" show-checkbox
                    ref="tree" :height="208" />
            </p>
            <div class="btn">
                <el-button type="primary" @click="submit()">确定</el-button>
                <el-button @click="close()">取消</el-button>
            </div>
        </div>
    </el-dialog>
</template>
<script lang="ts" setup>
import { ref, defineProps, computed, defineEmits, onMounted, toRaw, toRef } from 'vue'
import { getMenuDataNew, settingMenu } from '../../../../http/index'
import { ElMessage, ElTreeV2 } from 'element-plus'
import { AnyColumn } from 'element-plus/es/components/table-v2/src/common';
const props = defineProps({
    setingVisible: Boolean,
    roleId: String //
})
const tree = ref(ElTreeV2)

//读取下拉数据
const treedata = ref()
onMounted(async () => {
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
})
const emits = defineEmits(["CloseSeting"])
const handleClose = (done: () => void) => {
    emits("CloseSeting")
}
const close = async () => {
    emits("CloseSeting")
}
const submit = async () => {
    let arr: any[] = []
    const nodes: [] = tree.value.getCheckedNodes()
    if (nodes.length == 0) {
        ElMessage({
            message: '请选择需要分配的菜单！',
            type: 'warning',
        })
    } else {
        nodes.forEach(element => {
            arr.push(toRaw(element))
        });
        let mids: string = arr.map(item => { return `'${item.id}'` }).join(",")
        let res = (await settingMenu(props.roleId as string, mids)) as any as boolean
        if (res) {
            ElMessage({
                message: '设置成功！',
                type: 'success',
            })
            emits("CloseSeting")
        } else {
            ElMessage({
                message: '设置失败，请联系系统管理员！',
                type: 'error',
            })
        }
    }
}
</script>