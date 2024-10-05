import './assets/main.css'
import * as bootstrap from 'bootstrap/dist/js/bootstrap.bundle';

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(router)
app.provide('bootstrap', bootstrap);
app.mount('#app')
