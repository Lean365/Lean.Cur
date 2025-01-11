<template>
  <div class="changelog-container">
    <a-card :bordered="false">
      <template #title>
        <span>更新日志</span>
      </template>

      <a-timeline>
        <!-- v1.0.0 -->
        <a-timeline-item>
          <template #dot>
            <tag-outlined style="font-size: 16px" />
          </template>
          <div class="version-item">
            <div class="version-header">
              <h3>v1.0.0 <a-tag color="blue">最新版本</a-tag></h3>
              <span class="version-date">2024-01-01</span>
            </div>
            <div class="version-content">
              <div class="change-group">
                <h4><plus-circle-outlined /> 新增功能</h4>
                <ul>
                  <li>用户管理：提供用户的增删改查功能</li>
                  <li>角色管理：对系统角色进行维护</li>
                  <li>菜单管理：维护系统菜单和权限</li>
                  <li>部门管理：维护组织架构</li>
                  <li>字典管理：对系统中的枚举值进行维护</li>
                  <li>工作流管理：支持流程定义和部署</li>
                  <li>代码生成：支持自定义模板生成代码</li>
                </ul>
              </div>
              <div class="change-group">
                <h4><tool-outlined /> 功能优化</h4>
                <ul>
                  <li>优化登录流程，提供记住密码功能</li>
                  <li>优化页面加载速度</li>
                  <li>优化系统权限控制</li>
                  <li>优化表格组件，支持列固定和列宽调整</li>
                  <li>优化表单验证，提供更友好的提示信息</li>
                </ul>
              </div>
              <div class="change-group">
                <h4><bug-outlined /> 问题修复</h4>
                <ul>
                  <li>修复部分浏览器兼容性问题</li>
                  <li>修复已知的安全漏洞</li>
                  <li>修复文件上传偶发性失败的问题</li>
                  <li>修复导出数据时可能出现的内存溢出问题</li>
                </ul>
              </div>
            </div>
          </div>
        </a-timeline-item>

        <!-- v0.9.0 -->
        <a-timeline-item>
          <template #dot>
            <experiment-outlined style="font-size: 16px" />
          </template>
          <div class="version-item">
            <div class="version-header">
              <h3>v0.9.0 <a-tag color="orange">Beta</a-tag></h3>
              <span class="version-date">2023-12-01</span>
            </div>
            <div class="version-content">
              <div class="change-group">
                <h4><plus-circle-outlined /> 新增功能</h4>
                <ul>
                  <li>系统监控：新增服务器监控功能</li>
                  <li>日志管理：新增操作日志和登录日志</li>
                  <li>数据备份：支持数据库定时备份</li>
                </ul>
              </div>
              <div class="change-group">
                <h4><tool-outlined /> 功能优化</h4>
                <ul>
                  <li>优化系统性能</li>
                  <li>优化用户体验</li>
                  <li>优化代码结构</li>
                </ul>
              </div>
              <div class="change-group">
                <h4><bug-outlined /> 问题修复</h4>
                <ul>
                  <li>修复菜单权限控制问题</li>
                  <li>修复数据导入时的编码问题</li>
                </ul>
              </div>
            </div>
          </div>
        </a-timeline-item>

        <!-- v0.8.0 -->
        <a-timeline-item>
          <template #dot>
            <bug-outlined style="font-size: 16px" />
          </template>
          <div class="version-item">
            <div class="version-header">
              <h3>v0.8.0 <a-tag color="purple">Alpha</a-tag></h3>
              <span class="version-date">2023-11-01</span>
            </div>
            <div class="version-content">
              <div class="change-group">
                <h4><plus-circle-outlined /> 新增功能</h4>
                <ul>
                  <li>基础框架搭建</li>
                  <li>用户认证和授权</li>
                  <li>基础功能实现</li>
                </ul>
              </div>
              <div class="change-group">
                <h4><tool-outlined /> 功能优化</h4>
                <ul>
                  <li>优化项目结构</li>
                  <li>优化开发体验</li>
                </ul>
              </div>
              <div class="change-group">
                <h4><bug-outlined /> 问题修复</h4>
                <ul>
                  <li>修复已知问题</li>
                </ul>
              </div>
            </div>
          </div>
        </a-timeline-item>
      </a-timeline>
    </a-card>
  </div>
</template>

<script setup>
import {
  TagOutlined,
  ExperimentOutlined,
  BugOutlined,
  PlusCircleOutlined,
  ToolOutlined
} from '@ant-design/icons-vue';
</script>

<style scoped>
.changelog-container {
  padding: 24px;
}

.version-item {
  margin-bottom: 32px;
}

.version-header {
  display: flex;
  align-items: center;
  margin-bottom: 16px;
}

.version-header h3 {
  margin: 0;
  margin-right: 12px;
}

.version-date {
  color: #999;
  font-size: 14px;
}

.version-content {
  background: #fafafa;
  padding: 16px;
  border-radius: 4px;
}

.change-group {
  margin-bottom: 16px;
}

.change-group:last-child {
  margin-bottom: 0;
}

.change-group h4 {
  color: #666;
  margin: 0 0 8px 0;
  display: flex;
  align-items: center;
}

.change-group h4 :deep(.anticon) {
  margin-right: 8px;
}

.change-group ul {
  margin: 0;
  padding-left: 24px;
}

.change-group li {
  margin: 4px 0;
}

:deep(.ant-timeline-item-content) {
  margin-left: 28px;
}
</style> 