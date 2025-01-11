import { defineStore } from 'pinia';
import { ref } from 'vue';
import { request } from '@/utils/request';

export interface UserInfo {
  id: string;
  username: string;
  nickname: string;
  avatar: string;
  email: string;
  phone: string;
  roles: string[];
  permissions: string[];
}

export const useUserStore = defineStore('user', () => {
  const userInfo = ref<UserInfo | null>(null);
  const token = ref<string | null>(null);

  // 登录
  const login = async (username: string, password: string) => {
    try {
      const response = await request.post('/auth/login', {
        username,
        password,
      });
      token.value = response.data.token;
      await getUserInfo();
      return true;
    } catch (error) {
      console.error('登录失败:', error);
      throw error;
    }
  };

  // 获取用户信息
  const getUserInfo = async () => {
    try {
      const response = await request.get('/user/info');
      userInfo.value = response.data;
      return userInfo.value;
    } catch (error) {
      console.error('获取用户信息失败:', error);
      throw error;
    }
  };

  // 退出登录
  const logout = async () => {
    try {
      await request.post('/auth/logout');
      userInfo.value = null;
      token.value = null;
      return true;
    } catch (error) {
      console.error('退出登录失败:', error);
      throw error;
    }
  };

  // 修改密码
  const changePassword = async (oldPassword: string, newPassword: string) => {
    try {
      await request.post('/user/change-password', {
        oldPassword,
        newPassword,
      });
      return true;
    } catch (error) {
      console.error('修改密码失败:', error);
      throw error;
    }
  };

  return {
    userInfo,
    token,
    login,
    getUserInfo,
    logout,
    changePassword,
  };
}); 