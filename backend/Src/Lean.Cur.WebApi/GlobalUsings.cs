// 系统命名空间
global using System.Text;
global using System.ComponentModel.DataAnnotations;

// Microsoft基础设施
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Caching.StackExchangeRedis;
global using Microsoft.IdentityModel.Tokens;

// 第三方组件
global using NLog;
global using NLog.Web;
global using OfficeOpenXml;
global using SqlSugar;
global using StackExchange.Redis;

// Lean.Cur 通用模块
global using Lean.Cur.Common.Auth.Jwt;
global using Lean.Cur.Common.Auth.SlideVerify;
global using Lean.Cur.Common.Configs;
global using Lean.Cur.Common.Excel;
global using Lean.Cur.Common.Utils;
global using Lean.Cur.Common.Pagination;
global using Lean.Cur.Common.Enums;
global using Lean.Cur.Common.Models;
global using Lean.Cur.Common.Extensions;

// Lean.Cur 领域模块
global using Lean.Cur.Domain.Cache;
global using Lean.Cur.Domain.Repositories;
global using Lean.Cur.Domain.Entities;
global using Lean.Cur.Domain.Entities.Admin;
global using Lean.Cur.Domain.Entities.Logging;
global using Lean.Cur.Domain.Entities.Routine;

// Lean.Cur 应用层
global using Lean.Cur.Application.Authorization;
global using Lean.Cur.Application.Services.Auth;
global using Lean.Cur.Application.Services.Admin;
global using Lean.Cur.Application.Services.Logging;
global using Lean.Cur.Application.Services.Routine;
global using Lean.Cur.Application.Dtos.Auth;
global using Lean.Cur.Application.Dtos.Admin;
global using Lean.Cur.Application.Dtos.Logging;
global using Lean.Cur.Application.Dtos.Routine;

// Lean.Cur 基础设施
global using Lean.Cur.Infrastructure.Attributes;
global using Lean.Cur.Infrastructure.Cache;
global using Lean.Cur.Infrastructure.Database;
global using Lean.Cur.Infrastructure.Extensions;
global using Lean.Cur.Infrastructure.Repositories;
global using Lean.Cur.Infrastructure.Services.Auth;
global using Lean.Cur.Infrastructure.Services.Admin;
global using Lean.Cur.Infrastructure.Services.Logging;
global using Lean.Cur.Infrastructure.Services.Routine;

// Lean.Cur WebApi
global using Lean.Cur.WebApi.Filters;
global using Lean.Cur.WebApi.Middlewares;
global using Lean.Cur.WebApi.Controllers;

// 类型别名
global using LeanPermissionAttribute = Lean.Cur.Application.Authorization.LeanPermissionAttribute;