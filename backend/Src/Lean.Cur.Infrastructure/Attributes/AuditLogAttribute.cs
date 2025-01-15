using Lean.Cur.Application.Services.Logging;
using Lean.Cur.Common.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace Lean.Cur.Infrastructure.Attributes
{
    /// <summary>
    /// 审计日志特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AuditLogAttribute : ActionFilterAttribute
    {
        private readonly string _operationType;
        private readonly string _operationDesc;
        private Stopwatch? _stopwatch;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operationType">操作类型</param>
        /// <param name="operationDesc">操作描述</param>
        public AuditLogAttribute(string operationType, string operationDesc)
        {
            _operationType = operationType;
            _operationDesc = operationDesc;
        }

        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="context">上下文</param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();
            base.OnActionExecuting(context);
        }

        /// <summary>
        /// 执行后
        /// </summary>
        /// <param name="context">上下文</param>
        public override async void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                _stopwatch?.Stop();

                // 获取当前用户ID
                var userId = context.HttpContext.User.GetUserId();
                if (userId <= 0)
                {
                    return;
                }

                // 获取审计日志服务
                var auditLogService = context.HttpContext.RequestServices.GetService<IAuditLogService>();
                if (auditLogService == null)
                {
                    return;
                }

                // 获取操作结果
                var status = context.Exception == null ? 1 : 0;
                var errorMessage = context.Exception?.Message;

                // 记录审计日志
                await auditLogService.AddAuditLogAsync(
                    userId,
                    _operationType,
                    _operationDesc,
                    context.HttpContext,
                    _stopwatch?.ElapsedMilliseconds ?? 0,
                    status,
                    errorMessage);
            }
            catch
            {
                // 记录审计日志失败不影响正常业务
            }
            finally
            {
                base.OnActionExecuted(context);
            }
        }
    }
} 