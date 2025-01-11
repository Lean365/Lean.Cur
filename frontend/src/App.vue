<template>
  <a-config-provider
    :locale="antdLocale"
    :theme="{
      algorithm: currentTheme === 'dark' ? darkAlgorithm : defaultAlgorithm
    }"
  >
    <router-view></router-view>
  </a-config-provider>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useLocale } from '@/composables/useLocale';
import { useAppStore } from '@/stores/app';
import { theme } from 'ant-design-vue';
import zhCN from 'ant-design-vue/es/locale/zh_CN';
import enUS from 'ant-design-vue/es/locale/en_US';

const { darkAlgorithm, defaultAlgorithm } = theme;
const { currentLocale } = useLocale();
const appStore = useAppStore();

const antdLocale = computed(() => {
  switch (currentLocale.value) {
    case 'zh-CN':
      return zhCN;
    case 'en-US':
      return enUS;
    default:
      return enUS;
  }
});

const currentTheme = computed(() => appStore.theme);
</script> 