<template>
  <div class="login-container">
    <div class="login-content">
      <div class="login-header">
        <img src="@/assets/images/logo/logo.svg" alt="logo" />
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
          :rules="[{ required: true, message: '请输入用户名' }]"
        >
          <a-input v-model:value="formState.username" placeholder="用户名">
            <template #prefix>
              <user-outlined />
            </template>
          </a-input>
        </a-form-item>
        <a-form-item
          name="password"
          :rules="[{ required: true, message: '请输入密码' }]"
        >
          <a-input-password v-model:value="formState.password" placeholder="密码">
            <template #prefix>
              <lock-outlined />
            </template>
          </a-input-password>
        </a-form-item>
        <a-form-item>
          <a-checkbox v-model:checked="formState.remember">记住我</a-checkbox>
          <a class="login-form-forgot" href="">忘记密码</a>
        </a-form-item>
        <a-form-item>
          <a-button
            type="primary"
            html-type="submit"
            class="login-form-button"
            :loading="loading"
          >
            登录
          </a-button>
        </a-form-item>
      </a-form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { useRouter } from 'vue-router';
import { UserOutlined, LockOutlined } from '@ant-design/icons-vue';
import { message } from 'ant-design-vue';

interface FormState {
  username: string;
  password: string;
  remember: boolean;
}

const router = useRouter();
const loading = ref(false);
const formState = reactive<FormState>({
  username: '',
  password: '',
  remember: true,
});

const handleFinish = async (values: FormState) => {
  try {
    loading.value = true;
    // TODO: 实现登录逻辑
    console.log('登录信息:', values);
    message.success('登录成功');
    router.push('/dashboard');
  } catch (error) {
    console.error('登录失败:', error);
    message.error('登录失败，请重试');
  } finally {
    loading.value = false;
  }
};
</script>

<style lang="less" scoped>
.login-container {
  display: flex;
  align-items: center;
  justify-content: center;
  min-height: 100vh;
  background: var(--ant-layout-body-background);
}

.login-content {
  width: 368px;
  padding: 24px;
  background: var(--ant-component-background);
  border-radius: var(--ant-border-radius-base);
  box-shadow: var(--ant-box-shadow-base);
}

.login-header {
  display: flex;
  align-items: center;
  justify-content: center;
  margin-bottom: 24px;

  img {
    height: 44px;
    margin-right: 12px;
  }

  h1 {
    margin: 0;
    color: var(--ant-heading-color);
    font-size: 24px;
  }
}

.login-form-forgot {
  float: right;
}

.login-form-button {
  width: 100%;
}
</style> 