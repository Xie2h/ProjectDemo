<template>
    <div class="welcome">
        <el-link
            href="https://chain.donvv.com/?token=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiS0hVMDAwMTAwMTk2NCIsImV4cCI6MTY3OTc2OTM4OSwicmVmcmVzaCI6IjE2Nzg3MTgxODkxMDUiLCJicmFuY2giOiJkU1lIVUpEaTI2OHRRNm5RNWlLOUNRPT0iLCJzaG9wIjoiVm5uS2daS3ZaY1JodUlpTmcrVjlxdz09IiwidXNlciI6InBzVk95MlNVN25NYmhoRmY5RldpOGc9PSIsIm5hbWUiOiJzMeaHguW-rueuoeeQhuWRmCIsIm5pY2siOiJzMeaHguW-rueuoeeQhuWRmCIsImF2YXRhciI6IiIsIm5iZiI6MTY3ODY4OTM4OX0.rng7uNOZdY5q3BLH9JOtYPers5hqQYI0MWDzzzpcEt0"
            target="_blank" :underline="false">
            <el-image style=" " :src="WelcomeImageUrl"></el-image>
        </el-link>
    </div>
</template>

<script lang="ts" setup>
import { ref } from 'vue'
const WelcomeImageUrl = ref('/images/Welcome.png')


</script>

<style scoped>
.welcome {
    margin-top: 30px;
    display: flex;
    justify-content: center
}
</style>