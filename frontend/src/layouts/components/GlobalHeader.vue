<template>
  <div class="header">
    <div class="left">
      <menu-unfold-outlined
        v-if="collapsed"
        class="action-icon"
        @click="() => $emit('update:collapsed', false)"
      />
      <menu-fold-outlined
        v-else
        class="action-icon"
        @click="() => $emit('update:collapsed', true)"
      />
      <breadcrumb />
    </div>
    <div class="right">
      <locale-picker class="action-icon" />
      <theme-picker class="action-icon" />
      <a-dropdown>
        <span class="action-icon">
          <svg-icon name="user" :size="16" />
        </span>
        <template #overlay>
          <a-menu>
            <a-menu-item key="profile" @click="router.push('/profile')">
              <svg-icon name="user" :size="14" />
              <span>{{ $t('app.menu.profile') }}</span>
            </a-menu-item>
            <a-menu-divider />
            <a-menu-item key="logout" @click="handleLogout">
              <svg-icon name="logout" :size="14" />
              <span>{{ $t('app.menu.logout') }}</span>
            </a-menu-item>
          </a-menu>
        </template>
      </a-dropdown>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useUserStore } from '@/stores/user';
import SvgIcon from '@/components/SvgIcon/index.vue';
import {
  MenuUnfoldOutlined,
  MenuFoldOutlined,
} from '@ant-design/icons-vue';
import Breadcrumb from '../Breadcrumb/index.vue';
import LocalePicker from '../LocalePicker/index.vue';
import ThemePicker from '../ThemePicker/index.vue';

const router = useRouter();
const userStore = useUserStore();

defineProps({
  collapsed: {
    type: Boolean,
    required: true
  }
});

const handleLogout = () => {
  userStore.logout();
  router.push('/login');
};
</script>

<style lang="less" scoped>
.header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  padding: 0 24px;
  background: transparent;
  height: 64px;

  .left {
    display: flex;
    align-items: center;
    gap: 16px;
  }

  .right {
    display: flex;
    align-items: center;
    gap: 8px;
  }

  .action-icon {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    width: 32px;
    height: 32px;
    cursor: pointer;
    transition: all 0.3s;
    color: var(--ant-text-color);

    &:hover {
      color: var(--ant-primary-color);
    }
  }

  :deep(.ant-dropdown-menu-item) {
    display: flex;
    align-items: center;
    gap: 8px;
  }

  :deep(.svg-icon) {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    line-height: 1;
    vertical-align: middle;
  }
}

:deep(.ant-breadcrumb) {
  display: inline-flex;
  align-items: center;
}
</style> 