import { createApp } from 'vue'
import { createPinia } from 'pinia'
import App from './App.vue'
import router from './router'
const pinia = createPinia();
import gAuth from 'vue3-google-login';

const app = createApp(App);
app.use(gAuth, {
    clientId: '874742790685-7d96kkir7acvm2evb3rooci7d1l22a0i.apps.googleusercontent.com'
});
app.use(router)
.use(pinia)
.mount('#app');
