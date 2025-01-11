<template>
  <div class="profile-container">
    <a-card :bordered="false">
      <a-row :gutter="24">
        <!-- 左侧个人信息 -->
        <a-col :span="8">
          <a-card :bordered="false">
            <template #cover>
              <div class="avatar-wrapper">
                <a-upload
                  v-model:file-list="fileList"
                  :show-upload-list="false"
                  :before-upload="beforeUpload"
                  @change="handleAvatarChange"
                >
                  <div class="avatar-uploader">
                    <a-avatar :size="104" :src="userInfo.avatar">
                      <template #icon><user-outlined /></template>
                    </a-avatar>
                    <div class="avatar-uploader-trigger">
                      <camera-outlined />
                      <span>{{ $t('profile.avatar.change') }}</span>
                    </div>
                  </div>
                </a-upload>
              </div>
            </template>
            <template #title>
              <div class="profile-name">{{ userInfo.nickname }}</div>
            </template>
            <div class="profile-info">
              <p>
                <mail-outlined />
                <span>{{ userInfo.email }}</span>
              </p>
              <p>
                <phone-outlined />
                <span>{{ userInfo.phone }}</span>
              </p>
              <p>
                <environment-outlined />
                <span>{{ userInfo.location }}</span>
              </p>
            </div>
          </a-card>
        </a-col>

        <!-- 右侧详细信息 -->
        <a-col :span="16">
          <a-card :bordered="false">
            <a-tabs v-model:activeKey="activeTab">
              <!-- 基本信息 -->
              <a-tab-pane key="basic" :tab="$t('profile.tabs.basic')">
                <a-form :label-col="{ span: 4 }" :wrapper-col="{ span: 14 }">
                  <a-form-item :label="$t('profile.basic.nickname')">
                    <a-input v-model:value="userInfo.nickname" />
                  </a-form-item>
                  <a-form-item :label="$t('profile.basic.email')">
                    <a-input v-model:value="userInfo.email" />
                  </a-form-item>
                  <a-form-item :label="$t('profile.basic.phone')">
                    <a-input v-model:value="userInfo.phone" />
                  </a-form-item>
                  <a-form-item :label="$t('profile.basic.location')">
                    <a-input v-model:value="userInfo.location" />
                  </a-form-item>
                  <a-form-item :label="$t('profile.basic.bio')">
                    <a-textarea v-model:value="userInfo.bio" :rows="4" />
                  </a-form-item>
                </a-form>
              </a-tab-pane>

              <!-- 最近活动 -->
              <a-tab-pane key="activity" :tab="$t('profile.tabs.activity')">
                <a-timeline>
                  <a-timeline-item v-for="activity in activities" :key="activity.id">
                    <template #dot>
                      <component :is="activity.icon" />
                    </template>
                    <div class="activity-content">
                      <div class="activity-title">{{ activity.title }}</div>
                      <div class="activity-time">{{ activity.time }}</div>
                      <div class="activity-description">{{ activity.description }}</div>
                    </div>
                  </a-timeline-item>
                </a-timeline>
              </a-tab-pane>
            </a-tabs>
          </a-card>
        </a-col>
      </a-row>
    </a-card>
  </div>
</template>

<script setup lang="ts">
import { ref, reactive } from 'vue';
import { useUserStore } from '@/stores/user';
import { message } from 'ant-design-vue';
import type { UploadChangeParam, UploadProps } from 'ant-design-vue';
import {
  MailOutlined,
  PhoneOutlined,
  EnvironmentOutlined,
  EditOutlined,
  LoginOutlined,
  LogoutOutlined,
  UserOutlined,
  CameraOutlined,
} from '@ant-design/icons-vue';

const userStore = useUserStore();
const activeTab = ref('basic');

// 用户信息
const userInfo = reactive({
  nickname: '张三',
  email: 'zhangsan@example.com',
  phone: '13800138000',
  location: '北京',
  bio: '这是一段个人简介',
  avatar: '',
});

// 最近活动
const activities = ref([
  {
    id: 1,
    icon: EditOutlined,
    title: '修改了个人信息',
    time: '2024-01-20 10:00:00',
    description: '更新了头像和个人简介',
  },
  {
    id: 2,
    icon: LoginOutlined,
    title: '登录系统',
    time: '2024-01-20 09:30:00',
    description: '从 Chrome 浏览器登录',
  },
  {
    id: 3,
    icon: LogoutOutlined,
    title: '退出系统',
    time: '2024-01-19 18:00:00',
    description: '正常退出系统',
  },
]);

// 头像上传相关
const fileList = ref([]);
const beforeUpload = (file: File) => {
  const isImage = file.type.startsWith('image/');
  if (!isImage) {
    message.error($t('profile.avatar.typeError'));
    return false;
  }
  const isLt2M = file.size / 1024 / 1024 < 2;
  if (!isLt2M) {
    message.error($t('profile.avatar.sizeError'));
    return false;
  }
  return true;
};

const handleAvatarChange = async (info: UploadChangeParam) => {
  if (info.file.status === 'uploading') {
    return;
  }
  if (info.file.status === 'done') {
    try {
      const response = info.file.response;
      userInfo.avatar = response.url;
      message.success($t('profile.avatar.uploadSuccess'));
    } catch (error) {
      message.error($t('profile.avatar.uploadFailed'));
    }
  }
};
</script>

<style lang="less" scoped>
.profile-container {
  padding: 24px;
  background-color: var(--ant-layout-body-background);

  .avatar-wrapper {
    display: flex;
    justify-content: center;
    padding: 24px 0;
    background: var(--ant-component-background);

    .avatar-uploader {
      position: relative;
      cursor: pointer;

      &:hover .avatar-uploader-trigger {
        opacity: 1;
      }

      .avatar-uploader-trigger {
        position: absolute;
        left: 0;
        right: 0;
        bottom: 0;
        height: 32px;
        display: flex;
        flex-direction: column;
        align-items: center;
        justify-content: center;
        background: rgba(0, 0, 0, 0.45);
        opacity: 0;
        transition: opacity 0.3s;
        border-bottom-left-radius: 50%;
        border-bottom-right-radius: 50%;
        color: #fff;
        font-size: 12px;

        .anticon {
          font-size: 16px;
          margin-bottom: 4px;
        }
      }
    }
  }

  .profile-name {
    margin: 0;
    color: var(--ant-heading-color);
    font-size: 20px;
    text-align: center;
  }

  .profile-info {
    p {
      margin: 8px 0;
      color: var(--ant-text-color);

      .anticon {
        margin-right: 8px;
        color: var(--ant-text-color-secondary);
      }
    }
  }

  .activity-content {
    .activity-title {
      color: var(--ant-heading-color);
      font-weight: 500;
    }

    .activity-time {
      margin: 4px 0;
      color: var(--ant-text-color-secondary);
      font-size: 12px;
    }

    .activity-description {
      color: var(--ant-text-color);
    }
  }
}
</style> 