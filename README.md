# Lean.Cur

基于.NET Core的轻量级权限管理系统，采用DDD领域驱动设计，完全由Cursor AI自动创建。

## 项目特点

- 🎯 **领域驱动设计**：采用DDD架构，实现业务逻辑与技术实现的分离
- 🔐 **统一权限管理**：基于RBAC的权限控制，支持细粒度的权限管理
- 🚀 **代码生成器**：内置代码生成器，快速生成标准化的CRUD代码
- 🌐 **多语言支持**：内置国际化支持，轻松切换多种语言
- 🛡️ **安全性**：集成JWT认证、防XSS攻击、SQL注入防护等多重安全机制

## 核心功能

### 用户权限管理
- 用户管理：用户信息管理，支持用户状态控制
- 角色管理：角色配置，角色与权限关联
- 权限管理：细粒度的权限控制，支持按钮级别权限

### 系统功能
- 菜单管理：配置系统菜单，支持多级菜单
- 部门管理：机构部门管理，支持树形结构
- 岗位管理：岗位信息维护，支持岗位分配

### 系统监控
- 操作日志：记录用户操作，支持查询和回溯
- 登录日志：记录登录信息，支持登录分析
- 服务监控：监控服务器状态，记录系统性能

## 技术架构

### 后端技术
- 核心框架：.NET Core 8.0
- ORM框架：SqlSugar
- 缓存框架：Redis
- 日志框架：NLog
- 对象映射：Mapster
- 认证框架：JWT
- 接口文档：Swagger

### 前端技术（规划中）
- 核心框架：Vue 3
- UI框架：Ant Design Vue
- 状态管理：Pinia
- 路由管理：Vue Router
- HTTP客户端：Axios
- 构建工具：Vite
- 代码规范：
  - ESLint + Prettier
  - Ant Design Vue 规范
  - TypeScript 规范
- 国际化：vue-i18n
- 主题定制：Less 变量
- 图标：@ant-design/icons-vue
- 工具库：
  - dayjs（时间处理）
  - lodash（工具函数）
  - vue-types（类型检查）

## 项目结构

```
Lean.Cur/
├── backend/                # 后端项目
│   └── Src/
│       ├── Lean.Cur.Api/           # API层：接口控制器、过滤器、中间件
│       ├── Lean.Cur.Application/   # 应用层：DTO、服务接口和实现
│       ├── Lean.Cur.Domain/        # 领域层：实体、仓储接口、领域服务
│       ├── Lean.Cur.Infrastructure/# 基础设施层：仓储实现、工具类
│       ├── Lean.Cur.Common/        # 公共层：枚举、常量、通用类
│       └── Lean.Cur.Generator/     # 代码生成器：快速生成代码
└── frontend/               # 前端项目
    ├── src/
    │   ├── api/           # API接口定义和请求封装
    │   ├── assets/        # 静态资源文件
    │   ├── components/    # 公共组件
    │   ├── hooks/         # Vue组合式API钩子
    │   ├── layouts/       # 布局组件
    │   ├── locales/       # 国际化资源
    │   ├── router/        # 路由配置
    │   ├── store/         # Pinia状态管理
    │   ├── styles/        # 全局样式和主题
    │   ├── utils/         # 工具函数
    │   └── views/         # 页面视图组件
    ├── .env               # 环境配置
    ├── .eslintrc.json     # ESLint配置
    ├── .prettierrc.json   # Prettier配置
    ├── package.json       # 项目依赖配置
    ├── tsconfig.json      # TypeScript配置
    └── vite.config.ts     # Vite构建配置
```

## 快速开始

### 环境要求
- .NET Core SDK 7.0+
- Visual Studio 2022+ / VS Code
- SQL Server 2012+ / MySQL 5.7+

### 开发环境设置
1. 克隆仓库
```bash
git clone https://github.com/Lean365/Lean.Cur.git
```

2. 还原包
```bash
cd Lean.Cur/backend
dotnet restore
```

3. 修改数据库连接
- 打开 `Lean.Cur.Api/appsettings.json`
- 修改数据库连接字符串

4. 运行项目
```bash
cd Lean.Cur.Api
dotnet run
```

## 版本发布

本项目使用GitHub Actions自动化工作流进行版本发布。每次发布新版本时：

1. 创建新的版本标签：
```bash
git tag v1.0.0
git push origin v1.0.0
```

2. GitHub Actions将自动：
- 生成更新日志
- 创建新的Release
- 发布版本

## 贡献指南

1. Fork本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'feat: 添加新特性'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 创建Pull Request

### 提交规范
- feat: 新功能
- fix: 修复问题
- docs: 文档修改
- style: 代码格式修改
- refactor: 重构代码
- perf: 优化相关
- test: 测试相关
- chore: 其他修改

## 开源协议

本项目采用MIT协议。详见 [LICENSE](LICENSE) 文件。

## 联系我们

- Issues: [github.com/Lean365/Lean.Cur/issues](https://github.com/Lean365/Lean.Cur/issues)
- 讨论: [github.com/Lean365/Lean.Cur/discussions](https://github.com/Lean365/Lean.Cur/discussions)
