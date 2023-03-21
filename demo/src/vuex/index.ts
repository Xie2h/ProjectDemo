import { createStore } from 'vuex'
import { TagModel } from '../class/TagModel'
const store = createStore({
    //状态变量
    state() {
        return {
            TagArrs: [] as TagModel[],
            Token: localStorage["token"],
            NickName: localStorage["nickname"]
        }
    },
    //方法
    mutations: {
        //添加选项卡
        AddTag(state: any, tag: TagModel) {
            if ((state.TagArrs as TagModel[]).filter(item => item.index.indexOf(tag.index) > -1).length == 0) {
                state.TagArrs.push(tag)
            }
        },
        SettingNickName(state: any, NickName) {
            state.NickName = NickName
        },
        SettingToken(state: any, Token) {
            state.Token = Token
        }
    }
})
export default store