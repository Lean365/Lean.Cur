# Lean.Cur

基于.NET Core的轻量级权限管理系统。

## 功能特性

- 用户管理
- 角色管理
- 权限管理
- 菜单管理
- 按钮权限控制

## 技术栈

- .NET Core
- Entity Framework Core
- SqlSugar
- JWT认证
- AutoMapper

## 项目结构

```
Lean.Cur/
├── backend/                # 后端项目
│   └── Src/
│       ├── Lean.Cur.Api/           # API层
│       ├── Lean.Cur.Application/   # 应用层
│       ├── Lean.Cur.Domain/        # 领域层
│       └── Lean.Cur.Infrastructure/# 基础设施层
└── frontend/               # 前端项目（待添加）
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
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 创建Pull Request

## 开源协议

本项目采用MIT协议。详见 [LICENSE](LICENSE) 文件。 