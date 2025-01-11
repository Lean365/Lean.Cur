<template>
  <div class="dashboard">
    <div class="dashboard-header">
      <h2>仪表盘</h2>
      <a-breadcrumb>
        <a-breadcrumb-item>首页</a-breadcrumb-item>
        <a-breadcrumb-item>仪表盘</a-breadcrumb-item>
      </a-breadcrumb>
    </div>
    <div class="dashboard-content">
      <a-row :gutter="16">
        <a-col :span="6">
          <a-card>
            <template #title>
              <team-outlined />
              <span>用户总数</span>
            </template>
            <div class="card-content">
              <h3>{{ stats.users }}</h3>
              <p>较上月增长 {{ stats.userGrowth }}%</p>
            </div>
          </a-card>
        </a-col>
        <a-col :span="6">
          <a-card>
            <template #title>
              <interaction-outlined />
              <span>活跃用户</span>
            </template>
            <div class="card-content">
              <h3>{{ stats.activeUsers }}</h3>
              <p>较上月增长 {{ stats.activeGrowth }}%</p>
            </div>
          </a-card>
        </a-col>
        <a-col :span="6">
          <a-card>
            <template #title>
              <solution-outlined />
              <span>角色数量</span>
            </template>
            <div class="card-content">
              <h3>{{ stats.roles }}</h3>
              <p>较上月增长 {{ stats.roleGrowth }}%</p>
            </div>
          </a-card>
        </a-col>
        <a-col :span="6">
          <a-card>
            <template #title>
              <key-outlined />
              <span>权限数量</span>
            </template>
            <div class="card-content">
              <h3>{{ stats.permissions }}</h3>
              <p>较上月增长 {{ stats.permissionGrowth }}%</p>
            </div>
          </a-card>
        </a-col>
      </a-row>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import {
  TeamOutlined,
  InteractionOutlined,
  SolutionOutlined,
  KeyOutlined,
} from '@ant-design/icons-vue';

interface Stats {
  users: number;
  userGrowth: number;
  activeUsers: number;
  activeGrowth: number;
  roles: number;
  roleGrowth: number;
  permissions: number;
  permissionGrowth: number;
}

const stats = ref<Stats>({
  users: 1234,
  userGrowth: 12.5,
  activeUsers: 789,
  activeGrowth: 8.3,
  roles: 15,
  roleGrowth: 5.2,
  permissions: 156,
  permissionGrowth: 3.8,
});
</script>

<style lang="less" scoped>
.dashboard {
  &-header {
    margin-bottom: 24px;

    h2 {
      margin: 0 0 12px;
      color: var(--ant-heading-color);
      font-size: 16px;
    }
  }

  &-content {
    .card-content {
      text-align: center;

      h3 {
        margin: 12px 0;
        color: var(--ant-heading-color);
        font-size: 24px;
      }

      p {
        margin: 0;
        color: var(--ant-text-color-secondary);
      }
    }

    :deep(.ant-card-head-title) {
      .anticon {
        margin-right: 12px;
        color: var(--ant-primary-color);
      }
    }
  }
}
</style> 