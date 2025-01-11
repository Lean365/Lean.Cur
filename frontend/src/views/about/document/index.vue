<template>
  <div class="document-container">
    <a-card :bordered="false">
      <template #title>
        <span>系统文档</span>
      </template>

      <a-tabs v-model:activeKey="activeKey">
        <!-- 使用说明 -->
        <a-tab-pane key="guide" tab="使用说明">
          <a-collapse v-model:activeKey="guideActiveKeys">
            <!-- 快速开始 -->
            <a-collapse-panel key="1" header="快速开始">
              <h4>系统登录</h4>
              <p>1. 访问系统登录页面</p>
              <p>2. 输入用户名和密码</p>
              <p>3. 点击登录按钮进入系统</p>
              
              <h4>修改密码</h4>
              <p>1. 点击右上角用户头像</p>
              <p>2. 选择修改密码</p>
              <p>3. 输入原密码和新密码</p>
              <p>4. 点击确定完成修改</p>

              <h4>退出登录</h4>
              <p>1. 点击右上角用户头像</p>
              <p>2. 选择退出登录</p>
            </a-collapse-panel>

            <!-- 功能介绍 -->
            <a-collapse-panel key="2" header="功能介绍">
              <h4>系统管理</h4>
              <ul>
                <li>用户管理：维护系统用户信息</li>
                <li>角色管理：维护系统角色及权限分配</li>
                <li>菜单管理：维护系统菜单及按钮权限</li>
                <li>部门管理：维护组织架构</li>
                <li>字典管理：维护系统中的数据字典</li>
              </ul>

              <h4>系统监控</h4>
              <ul>
                <li>在线用户：查看当前在线用户</li>
                <li>操作日志：查看系统操作记录</li>
                <li>登录日志：查看系统登录记录</li>
                <li>服务监控：查看服务器状态</li>
              </ul>

              <h4>工作流程</h4>
              <ul>
                <li>流程定义：维护工作流程定义</li>
                <li>流程部署：部署工作流程</li>
                <li>流程管理：管理运行中的流程</li>
              </ul>
            </a-collapse-panel>

            <!-- 常见问题 -->
            <a-collapse-panel key="3" header="常见问题">
              <h4>Q: 忘记密码怎么办？</h4>
              <p>A: 请联系系统管理员重置密码。</p>

              <h4>Q: 如何申请新的权限？</h4>
              <p>A: 请联系您的部门主管或系统管理员进行权限分配。</p>

              <h4>Q: 系统支持哪些浏览器？</h4>
              <p>A: 推荐使用 Chrome、Firefox、Edge 等现代浏览器，不支持 IE。</p>

              <h4>Q: 如何导出数据？</h4>
              <p>A: 在列表页面点击导出按钮，选择导出格式即可。</p>
            </a-collapse-panel>
          </a-collapse>
        </a-tab-pane>

        <!-- 开发文档 -->
        <a-tab-pane key="dev" tab="开发文档">
          <a-collapse v-model:activeKey="devActiveKeys">
            <!-- 环境准备 -->
            <a-collapse-panel key="1" header="环境准备">
              <h4>开发环境</h4>
              <ul>
                <li>JDK 17</li>
                <li>Maven 3.8+</li>
                <li>Node.js 16+</li>
                <li>MySQL 8.0+</li>
                <li>Redis 7.0+</li>
              </ul>

              <h4>开发工具</h4>
              <ul>
                <li>IDEA 2023（后端开发）</li>
                <li>VS Code（前端开发）</li>
                <li>Git（版本控制）</li>
                <li>Postman（接口测试）</li>
              </ul>
            </a-collapse-panel>

            <!-- 项目结构 -->
            <a-collapse-panel key="2" header="项目结构">
              <h4>前端项目结构</h4>
              <pre>
frontend
├── src
│   ├── api           # 接口请求
│   ├── assets        # 静态资源
│   ├── components    # 公共组件
│   ├── layout        # 布局组件
│   ├── router        # 路由配置
│   ├── store         # 状态管理
│   ├── utils         # 工具函数
│   └── views         # 页面组件
└── package.json      # 项目配置
              </pre>

              <h4>后端项目结构</h4>
              <pre>
backend
├── src/main/java
│   ├── config        # 配置类
│   ├── controller    # 控制器
│   ├── entity        # 实体类
│   ├── mapper        # 数据访问
│   ├── service       # 业务逻辑
│   └── utils         # 工具类
└── pom.xml          # 项目依赖
              </pre>
            </a-collapse-panel>

            <!-- 开发规范 -->
            <a-collapse-panel key="3" header="开发规范">
              <h4>命名规范</h4>
              <ul>
                <li>类名：使用大驼峰命名法，如 UserController</li>
                <li>方法名：使用小驼峰命名法，如 getUserList</li>
                <li>变量名：使用小驼峰命名法，如 userList</li>
                <li>常量名：使用大写下划线，如 MAX_COUNT</li>
              </ul>

              <h4>注释规范</h4>
              <ul>
                <li>类注释：说明类的作用、作者、创建时间</li>
                <li>方法注释：说明方法的功能、参数、返回值</li>
                <li>关键代码注释：说明代码的实现逻辑</li>
              </ul>

              <h4>提交规范</h4>
              <ul>
                <li>feat: 新功能</li>
                <li>fix: 修复问题</li>
                <li>docs: 修改文档</li>
                <li>style: 修改格式</li>
                <li>refactor: 重构代码</li>
                <li>test: 修改测试</li>
              </ul>
            </a-collapse-panel>
          </a-collapse>
        </a-tab-pane>

        <!-- API文档 -->
        <a-tab-pane key="api" tab="API文档">
          <div class="api-doc">
            <h3>接口说明</h3>
            <p>系统采用 RESTful API 设计规范，接口文档使用 Swagger 生成。</p>
            
            <h4>接口地址</h4>
            <ul>
              <li>开发环境：<a href="http://localhost:8080/swagger-ui.html" target="_blank">http://localhost:8080/swagger-ui.html</a></li>
              <li>测试环境：<a href="http://test-api.example.com/swagger-ui.html" target="_blank">http://test-api.example.com/swagger-ui.html</a></li>
            </ul>

            <h4>认证方式</h4>
            <p>使用 JWT Token 认证，在请求头中添加 Authorization: Bearer {token}</p>

            <h4>响应格式</h4>
            <pre>
{
  "code": 200,      // 状态码
  "msg": "success", // 提示信息
  "data": {         // 响应数据
    // ...
  }
}
            </pre>

            <h4>常见状态码</h4>
            <ul>
              <li>200：请求成功</li>
              <li>400：请求参数错误</li>
              <li>401：未授权</li>
              <li>403：禁止访问</li>
              <li>404：资源不存在</li>
              <li>500：服务器错误</li>
            </ul>
          </div>
        </a-tab-pane>
      </a-tabs>
    </a-card>
  </div>
</template>

<script setup>
import { ref } from 'vue';

// 当前激活的标签页
const activeKey = ref('guide');

// 使用说明展开的面板
const guideActiveKeys = ref(['1']);

// 开发文档展开的面板
const devActiveKeys = ref(['1']);
</script>

<style scoped>
.document-container {
  padding: 24px;
}

h4 {
  margin: 16px 0 8px;
  color: #333;
}

ul {
  margin: 8px 0;
  padding-left: 20px;
}

pre {
  background: #f5f5f5;
  padding: 16px;
  border-radius: 4px;
  overflow: auto;
}

.api-doc {
  max-width: 800px;
}

:deep(.ant-collapse-content-box) {
  max-height: calc(100vh - 300px);
  overflow-y: auto;
}
</style> 