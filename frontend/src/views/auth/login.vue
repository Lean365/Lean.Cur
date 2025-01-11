<template>
  <div class="auth-container">
    <div class="auth-content">
      <div class="auth-header">
        <img src="@/assets/logo/logo.svg" alt="logo" />
        <h1>Lean.Cur</h1>
      </div>
      <a-form
        :model="formState"
        name="login"
        @finish="handleFinish"
        autocomplete="off"
      >
        <a-form-item
          name="username"
          :rules="[{ required: true, message: $t('auth.login.usernameRequired') }]"
        >
          <a-input v-model:value="formState.username" :placeholder="$t('auth.login.username')">
            <template #prefix>
              <user-outlined />
            </template>
          </a-input>
        </a-form-item>
        <a-form-item
          name="password"
          :rules="[{ required: true, message: $t('auth.login.passwordRequired') }]"
        >
          <a-input-password v-model:value="formState.password" :placeholder="$t('auth.login.password')">
            <template #prefix>
              <lock-outlined />
            </template>
          </a-input-password>
        </a-form-item>
        <a-form-item>
          <div class="auth-options">
            <a-checkbox v-model:checked="formState.remember">{{ $t('auth.login.remember') }}</a-checkbox>
            <a class="auth-forgot" @click="handleForgotPassword">{{ $t('auth.login.forgot') }}</a>
          </div>
        </a-form-item>
        <a-form-item>
          <a-button
            type="primary"
            html-type="submit"
            class="auth-button"
            :loading="loading"
          >
            {{ $t('auth.login.submit') }}
          </a-button>
        </a-form-item>
      </a-form>
      <div class="auth-footer">
        <a-space>
          <theme-picker />
          <locale-picker />
        </a-space>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { message } from 'ant-design-vue';
import { UserOutlined, LockOutlined } from '@ant-design/icons-vue';
import { useUserStore } from '@/stores/user';
import ThemePicker from '@/components/ThemePicker/index.vue';
import LocalePicker from '@/components/LocalePicker/index.vue';

interface FormState {
  username: string;
  password: string;
  remember: boolean;
}

const router = useRouter();
const userStore = useUserStore();
const loading = ref(false);

const formState = reactive<FormState>({
  username: '',
  password: '',
  remember: true,
});

const handleFinish = async (values: FormState) => {
  try {
    loading.value = true;
    await userStore.login(values);
    message.success($t('auth.login.success'));
    router.push('/dashboard');
  } catch (error) {
    console.error('登录失败:', error);
    message.error($t('auth.login.failed'));
  } finally {
    loading.value = false;
  }
};

const handleForgotPassword = () => {
  router.push('/auth/forgot-password');
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
  width: 368px;
  padding: 24px;
  background: var(--ant-component-background);
  border-radius: var(--ant-border-radius-base);
  box-shadow: var(--ant-box-shadow-base);
}

.auth-header {
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 40px;

  img {
    height: 44px;
    margin-right: 16px;
  }

  h1 {
    margin: 0;
    color: var(--ant-heading-color);
    font-size: 33px;
    font-weight: 600;
  }
}

.auth-options {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.auth-forgot {
  color: var(--ant-primary-color);
  cursor: pointer;

  &:hover {
    color: var(--ant-primary-color-hover);
  }
}

.auth-button {
  width: 100%;
}

.auth-footer {
  margin-top: 24px;
  text-align: center;
}
</style> 