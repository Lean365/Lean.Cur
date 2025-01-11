import { computed, onMounted, watch } from 'vue';
import { useAppStore } from '../stores/app';

export type Theme = 'light' | 'dark' | 'system';

export function useTheme() {
  const appStore = useAppStore();

  const currentTheme = computed(() => appStore.theme);

  // 监听系统主题变化
  const watchSystemTheme = () => {
    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)');
    const handleChange = (e: MediaQueryListEvent) => {
      if (currentTheme.value === 'system') {
        appStore.setTheme('system');
      }
    };
    mediaQuery.addEventListener('change', handleChange);
    return () => mediaQuery.removeEventListener('change', handleChange);
  };

  // 设置主题
  const setTheme = (theme: Theme) => {
    appStore.setTheme(theme);
  };

  // 初始化
  onMounted(() => {
    const theme = currentTheme.value;
    setTheme(theme);
    watchSystemTheme();
  });

  return {
    currentTheme,
    setTheme,
  };
} 