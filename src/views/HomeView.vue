<template>
  <div class="home">
    <img alt="Vue logo" src="../assets/logo.png">
    <HelloWorld msg="Welcome to Your Vue.js + TypeScript App"/>
    <GoogleLogin :callback="callback"/>
    <p>{{ userInfo }}</p>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue';
import HelloWorld from '@/components/HelloWorld.vue'; // @ is an alias to /src
import axios from '@/plugins/axios'
// import { computed } from '@vue/reactivity';

export default defineComponent({
  name: 'HomeView',
  components: {
    HelloWorld,
  },
  setup () {
    const userInfo = ref<any>();
    const buildCookie = (source) => {
      const cookie = source.replace(" httponly", process.env.NODE_ENV === 'development' ? "Domain=localhost" : 'Domain=testauth.dichvubanker.com');

      return Promise.resolve(cookie)
    }
    const callback = async ({credential}) => {
      const payload = {
        token: credential
      };
      const res = await axios.post('/api/auth/google', payload);
      const newCookie = await buildCookie(res.data[0]);
      document.cookie = newCookie;
      const user = await axios.get('/api/auth/me', { withCredentials: true });
      userInfo.value = user.data;
      // console.log(response.credential, res, user)
      // console.log("Handle the userData", userData)
    }
    return {
      callback,
      userInfo
    }
  }
});
</script>
