import { defineStore } from 'pinia';
import type { Locale } from '@/composables/useLocale';

interface AppState {
  locale: Locale;
  antdLocale: any;
  theme: 'light' | 'dark';
}

export const useAppStore = defineStore('app', {
  state: (): AppState => ({
    locale: localStorage.getItem('locale') as Locale || 'zh-CN',
    antdLocale: null,
    theme: localStorage.getItem('theme') as 'light' | 'dark' || 'light',
  }),

  actions: {
    setLocale(locale: Locale) {
      this.locale = locale;
      localStorage.setItem('locale', locale);
    },
    setAntdLocale(locale: any) {
      this.antdLocale = locale;
    },
    setTheme(theme: 'light' | 'dark') {
      this.theme = theme;
      localStorage.setItem('theme', theme);
    },
  },
}); 