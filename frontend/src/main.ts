import { createApp } from 'vue';
import { createPinia } from 'pinia';
import Antd from 'ant-design-vue';
import App from './App.vue';
import router from './router/index';
import i18n from '@/locales';
import { showVersion } from './utils/version';

import 'ant-design-vue/dist/reset.css';
import 'virtual:svg-icons-register';

// 显示版本号
showVersion();

const app = createApp(App);

app.use(createPinia());
app.use(router);
app.use(i18n);
app.use(Antd);

app.mount('#app'); 