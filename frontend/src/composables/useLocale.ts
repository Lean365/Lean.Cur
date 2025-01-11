import { computed, onMounted } from 'vue';
import { useI18n } from 'vue-i18n';
import { useAppStore } from '../stores/app';
import zhCN from 'ant-design-vue/locale/zh_CN';
import enUS from 'ant-design-vue/locale/en_US';

export type Locale = 'zh-CN' | 'en-US';

export function useLocale() {
  const { locale } = useI18n();
  const appStore = useAppStore();

  const currentLocale = computed(() => appStore.locale);

  const setLocale = async (lang: Locale) => {
    // 设置 vue-i18n 的语言
    locale.value = lang;
    // 设置 store 中的语言
    appStore.setLocale(lang);
    // 设置 HTML 的 lang 属性
    document.documentElement.setAttribute('lang', lang);
    // 设置 antd 的语言
    appStore.setAntdLocale(lang === 'zh-CN' ? zhCN : enUS);
  };

  // 初始化
  onMounted(() => {
    const lang = currentLocale.value;
    setLocale(lang);
  });

  return {
    currentLocale,
    setLocale,
  };
} 