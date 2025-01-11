<template>
  <div class="auth-container">
    <div class="auth-content">
      <a-result
        status="success"
        :title="$t('auth.logout.title')"
        :sub-title="$t('auth.logout.subtitle')"
      >
        <template #extra>
          <a-button type="primary" @click="handleBackToLogin">
            {{ $t('auth.logout.backToLogin') }}
          </a-button>
        </template>
      </a-result>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useUserStore } from '@/stores/user';

const router = useRouter();
const userStore = useUserStore();

onMounted(async () => {
  await userStore.logout();
});

const handleBackToLogin = () => {
  router.push('/auth/login');
};
</script>

<style lang="less" scoped>
.auth-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background: var(--ant-layout-body-background);
}

.auth-content {
  width: 400px;
  padding: 24px;
  background: var(--ant-component-background);
  border-radius: var(--ant-border-radius-base);
  box-shadow: var(--ant-box-shadow-base);
}
</style> 