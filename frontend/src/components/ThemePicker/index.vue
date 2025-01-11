<template>
  <a-dropdown placement="bottomRight">
    <span class="action-icon">
      <bg-colors-outlined v-if="theme === 'light'" />
      <bulb-outlined v-else />
    </span>
    <template #overlay>
      <a-menu>
        <a-menu-item key="light" @click="handleThemeChange('light')">
          <span>{{ $t('app.header.theme.light') }}</span>
        </a-menu-item>
        <a-menu-item key="dark" @click="handleThemeChange('dark')">
          <span>{{ $t('app.header.theme.dark') }}</span>
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { useAppStore } from '@/stores/app';
import {
  BulbOutlined,
  BgColorsOutlined,
} from '@ant-design/icons-vue';

const appStore = useAppStore();
const currentTheme = computed(() => appStore.theme);

const setTheme = (theme: 'light' | 'dark') => {
  appStore.setTheme(theme);
  // 设置 HTML 的 data-theme 属性
  document.documentElement.setAttribute('data-theme', theme);
};
</script>

<style lang="less" scoped>
.action {
  height: 64px;
  padding: 0 12px;
  font-size: 16px;
  color: var(--ant-text-color);

  &:hover {
    background: rgba(0, 0, 0, 0.025);
  }
}

:deep(.ant-dropdown-menu-item) {
  min-width: 160px;
}
</style> 
