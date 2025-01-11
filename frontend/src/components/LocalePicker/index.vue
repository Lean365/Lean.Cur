<template>
  <a-dropdown placement="bottomRight">
    <a-button type="text" class="action">
      <template #icon>
        <translation-outlined />
      </template>
    </a-button>
    <template #overlay>
      <a-menu :selected-keys="[currentLocale]">
        <a-menu-item key="zh-CN" @click="() => setLocale('zh-CN')">
          <template #icon>
            <span class="fi fi-cn"></span>
          </template>
          简体中文
        </a-menu-item>
        <a-menu-item key="en-US" @click="() => setLocale('en-US')">
          <template #icon>
            <span class="fi fi-us"></span>
          </template>
          English
        </a-menu-item>
      </a-menu>
    </template>
  </a-dropdown>
</template>

<script setup>
import { useLocale } from '@/composables/useLocale';
import { TranslationOutlined } from '@ant-design/icons-vue';
import 'flag-icons/css/flag-icons.min.css';

const { currentLocale, setLocale } = useLocale();
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

  .fi {
    font-size: 14px;
    margin-right: 8px;
  }
}
</style> 