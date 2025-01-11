import { createI18n } from 'vue-i18n';
import zhCNApp from './app/zh-CN';
import zhCNLogin from './login/zh-CN';
import zhCNDashboard from './dashboard/zh-CN';
import zhCNSettings from './settings/zh-CN';
import zhCNUser from './user/zh-CN';
import zhCNProfile from './profile/zh-CN';

import enUSApp from './app/en-US';
import enUSLogin from './login/en-US';
import enUSDashboard from './dashboard/en-US';
import enUSSettings from './settings/en-US';
import enUSUser from './user/en-US';
import enUSProfile from './profile/en-US';

const messages = {
  'zh-CN': {
    app: zhCNApp,
    login: zhCNLogin,
    dashboard: zhCNDashboard,
    settings: zhCNSettings,
    user: zhCNUser,
    profile: zhCNProfile,
  },
  'en-US': {
    app: enUSApp,
    login: enUSLogin,
    dashboard: enUSDashboard,
    settings: enUSSettings,
    user: enUSUser,
    profile: enUSProfile,
  },
};

const i18n = createI18n({
  legacy: false,
  locale: localStorage.getItem('language') || 'zh-CN',
  fallbackLocale: 'zh-CN',
  messages,
});

export default i18n; 