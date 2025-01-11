import { defineStore } from 'pinia';
import { ref } from 'vue';

export interface UserSettings {
  // 基本设置
  language: string;
  timezone: string;
  dateFormat: string;
  timeFormat: string;
  
  // 安全设置
  twoFactorEnabled: boolean;
  
  // 通知设置
  emailNotification: boolean;
  pushNotification: boolean;
  notificationFrequency: 'realtime' | 'daily' | 'weekly';
  
  // 外观设置
  theme: 'light' | 'dark' | 'system';
  compactMode: boolean;
  fontSize: number;
}

const defaultSettings: UserSettings = {
  language: 'zh-CN',
  timezone: 'UTC+8',
  dateFormat: 'YYYY-MM-DD',
  timeFormat: 'HH:mm:ss',
  twoFactorEnabled: false,
  emailNotification: true,
  pushNotification: true,
  notificationFrequency: 'realtime',
  theme: 'light',
  compactMode: false,
  fontSize: 14,
};

export const useSettingsStore = defineStore('settings', () => {
  // 状态
  const settings = ref<UserSettings>({ ...defaultSettings });
  
  // 从本地存储加载设置
  const loadSettings = () => {
    const savedSettings = localStorage.getItem('userSettings');
    if (savedSettings) {
      settings.value = { ...defaultSettings, ...JSON.parse(savedSettings) };
    }
  };
  
  // 保存设置到本地存储
  const saveSettings = (newSettings: Partial<UserSettings>) => {
    settings.value = { ...settings.value, ...newSettings };
    localStorage.setItem('userSettings', JSON.stringify(settings.value));
  };
  
  // 重置设置
  const resetSettings = () => {
    settings.value = { ...defaultSettings };
    localStorage.setItem('userSettings', JSON.stringify(defaultSettings));
  };
  
  // 初始化时加载设置
  loadSettings();
  
  return {
    settings,
    saveSettings,
    resetSettings,
  };
}); 