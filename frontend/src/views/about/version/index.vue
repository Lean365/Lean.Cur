<template>
  <div class="version-container">
    <a-card :bordered="false">
      <template #title>
        <span>版本信息</span>
      </template>

      <!-- 当前版本信息 -->
      <a-descriptions title="当前版本" bordered>
        <a-descriptions-item label="版本号" :span="2">v1.0.0</a-descriptions-item>
        <a-descriptions-item label="发布时间">2024-01-01</a-descriptions-item>
        <a-descriptions-item label="框架信息" :span="3">
          <a-space>
            <a-tag color="blue">Vue 3.x</a-tag>
            <a-tag color="green">Spring Boot 3.x</a-tag>
            <a-tag color="purple">Ant Design Vue 4.x</a-tag>
            <a-tag color="orange">MyBatis Plus 3.x</a-tag>
          </a-space>
        </a-descriptions-item>
        <a-descriptions-item label="技术支持" :span="3">
          <a href="https://github.com/your-repo" target="_blank">GitHub</a>
        </a-descriptions-item>
      </a-descriptions>

      <!-- 版本历史 -->
      <a-divider orientation="left">版本历史</a-divider>
      <a-timeline mode="alternate">
        <a-timeline-item>
          <template #dot>
            <tag-outlined style="font-size: 16px" />
          </template>
          <div class="version-item">
            <h4>v1.0.0 <a-tag color="blue">最新版本</a-tag></h4>
            <p class="version-date">2024-01-01</p>
            <div class="version-content">
              <h5>新特性：</h5>
              <ul>
                <li>用户管理：提供用户的增删改查功能</li>
                <li>角色管理：对系统角色进行维护</li>
                <li>菜单管理：维护系统菜单和权限</li>
                <li>部门管理：维护组织架构</li>
                <li>字典管理：对系统中的枚举值进行维护</li>
              </ul>
              <h5>功能优化：</h5>
              <ul>
                <li>优化登录流程，提供记住密码功能</li>
                <li>优化页面加载速度</li>
                <li>优化系统权限控制</li>
              </ul>
              <h5>问题修复：</h5>
              <ul>
                <li>修复部分浏览器兼容性问题</li>
                <li>修复已知的安全漏洞</li>
              </ul>
            </div>
          </div>
        </a-timeline-item>

        <a-timeline-item>
          <template #dot>
            <experiment-outlined style="font-size: 16px" />
          </template>
          <div class="version-item">
            <h4>v0.9.0 <a-tag color="orange">Beta</a-tag></h4>
            <p class="version-date">2023-12-01</p>
            <div class="version-content">
              <h5>新特性：</h5>
              <ul>
                <li>工作流管理：支持流程定义和部署</li>
                <li>代码生成：支持自定义模板生成代码</li>
              </ul>
              <h5>功能优化：</h5>
              <ul>
                <li>优化系统性能</li>
                <li>优化用户体验</li>
              </ul>
            </div>
          </div>
        </a-timeline-item>

        <a-timeline-item>
          <template #dot>
            <bug-outlined style="font-size: 16px" />
          </template>
          <div class="version-item">
            <h4>v0.8.0 <a-tag color="purple">Alpha</a-tag></h4>
            <p class="version-date">2023-11-01</p>
            <div class="version-content">
              <h5>新特性：</h5>
              <ul>
                <li>系统监控：服务器和应用监控</li>
                <li>日志管理：系统日志和操作日志</li>
              </ul>
              <h5>问题修复：</h5>
              <ul>
                <li>修复已知问题</li>
              </ul>
            </div>
          </div>
        </a-timeline-item>
      </a-timeline>

      <!-- 技术栈 -->
      <a-divider orientation="left">技术栈</a-divider>
      <a-row :gutter="[16, 16]">
        <a-col :span="6">
          <a-card title="前端技术" size="small">
            <p>Vue 3.x</p>
            <p>Vite 4.x</p>
            <p>Ant Design Vue 4.x</p>
            <p>TypeScript 5.x</p>
          </a-card>
        </a-col>
        <a-col :span="6">
          <a-card title="后端技术" size="small">
            <p>Spring Boot 3.x</p>
            <p>Spring Security 6.x</p>
            <p>MyBatis Plus 3.x</p>
            <p>Redis 7.x</p>
          </a-card>
        </a-col>
        <a-col :span="6">
          <a-card title="开发工具" size="small">
            <p>IDEA 2023</p>
            <p>VS Code</p>
            <p>Git</p>
            <p>Maven</p>
          </a-card>
        </a-col>
        <a-col :span="6">
          <a-card title="其他技术" size="small">
            <p>Docker</p>
            <p>Jenkins</p>
            <p>Nginx</p>
            <p>MySQL 8.x</p>
          </a-card>
        </a-col>
      </a-row>
    </a-card>
  </div>
</template>

<script setup>
import {
  TagOutlined,
  ExperimentOutlined,
  BugOutlined
} from '@ant-design/icons-vue';
</script>

<style scoped>
.version-container {
  padding: 24px;
}

.version-item {
  margin-bottom: 24px;
}

.version-date {
  color: #999;
  font-size: 12px;
  margin: 4px 0;
}

.version-content {
  background: #fafafa;
  padding: 16px;
  border-radius: 4px;
}

.version-content h5 {
  color: #666;
  margin: 8px 0;
}

.version-content ul {
  margin: 0;
  padding-left: 20px;
}

.version-content li {
  margin: 4px 0;
}

:deep(.ant-timeline-item-content) {
  max-width: 400px;
}
</style> 