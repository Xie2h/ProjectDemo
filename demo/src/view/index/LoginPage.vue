<template>
    <div class="login">
        <div class="relative">
            <div class="right">
                <el-row>
                    <el-col :span="24">
                        <h2>登录</h2>
                    </el-col>
                </el-row>
                <el-row>
                    <el-col :span="24">
                        <el-form :model="form" label-width="120px" label-position="top" size="large" class="form"
                            :rules="rules" ref="ruleFormRef">
                            <el-form-item label="用户名" prop="userName">
                                <el-input v-model="form.userName" />
                            </el-form-item>
                            <el-form-item label="密码" prop="passWord">
                                <el-input v-model="form.passWord" type="password" show-password />
                            </el-form-item>
                            <el-form-item>
                                <el-button class="submitBtn" type="primary" @click="onSubmit(ruleFormRef)">登录
                                </el-button>
                            </el-form-item>
                        </el-form>
                    </el-col>
                </el-row>
            </div>
        </div>
    </div>
</template>


<script lang="ts" setup>
import { ref, reactive } from 'vue'
import type { FormInstance, FormRules } from 'element-plus'
import { ElMessage } from 'element-plus'
import { getToken } from '../../http/index'
import Tool from '../../global'
import { UserInfo } from './class/UserInfo'
import { useStore } from 'vuex';
import { useRouter } from 'vue-router';
const store = useStore()
const router = useRouter()
// const url = ref('/images/icon.png')
// const boxbg = ref('/images/svgs/login-box-bg.svg')

const form = reactive({
    userName: '',
    passWord: ''
})
const rules = reactive<FormRules>({
    //用户名
    userName: [{ required: true, message: "请输入用户名", trigger: 'blur' }],
    //密码
    passWord: [{ required: true, message: "请输入密码", trigger: 'blur' }]
})
const ruleFormRef = ref<FormInstance>()

const onSubmit = async (ruleFormRef: FormInstance | undefined) => {
    if (!ruleFormRef) return;
    await ruleFormRef.validate(async (valid, fields) => {
        if (valid) {
            //请求后端数据，获取token，并将token放入localStorage
            const token = await getToken(form.userName, form.passWord) as any as string
            const user: UserInfo = JSON.parse(new Tool().FormatToken(token))
            localStorage["token"] = token
            localStorage["nickname"] = user.NickName
            store.commit("SettingNickName", user.NickName)
            store.commit("SettingToken", token)
            router.push({ path: '/desktop' });

        } else {
            console.log("校验不通过！")
            console.log(fields)
            let errors: string = "";
            fields?.userName?.forEach(element => {
                errors += element.message + ';'
            })
            fields?.passWord?.forEach(element => {
                errors += element.message + ';'
            })
            ElMessage({
                type: "warning",
                message: errors
            })
        }
    })
}


</script>
<style lang="scss" scoped>
.login {
    width: 100%;
    height: 100%;

    .relative {
        width: 100%;
        height: 100%;
        text-align: center;

        .right {
            width: 30%;
            position: absolute;
            top: 25%;
            left: 50%;
            transform: translate(-50%, -50%);

            float: left;
            padding-top: 15%;

            .form {
                width: 60%;
                margin: 0px auto;

                .submitBtn {
                    width: 100%;
                }
            }
        }
    }
}
</style>