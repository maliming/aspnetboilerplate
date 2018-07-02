using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Abp.Aspects;
using Abp.AspNetCore.Configuration;
using Abp.AspNetCore.Mvc.Extensions;
using Abp.Auditing;
using Abp.Dependency;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Abp.AspNetCore.Mvc.Auditing
{
    public class AbpAuditAsyncPageFilter : IAsyncPageFilter, ITransientDependency
    {
        private readonly IAbpAspNetCoreConfiguration _configuration;

        private readonly IAuditingHelper _auditingHelper;

        public AbpAuditAsyncPageFilter(IAbpAspNetCoreConfiguration configuration, IAuditingHelper auditingHelper)
        {
            _configuration = configuration;
            _auditingHelper = auditingHelper;
        }


        public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
            PageHandlerExecutionDelegate next)
        {
            if (!ShouldSaveAudit(context))
            {
                await next();
                return;
            }

            using (AbpCrossCuttingConcerns.Applying(context.HandlerInstance, AbpCrossCuttingConcerns.Auditing))
            {
                var auditInfo = _auditingHelper.CreateAuditInfo(
                    context.HandlerInstance.GetType(),
                    context.HandlerMethod.MethodInfo,
                    context.HandlerArguments
                );

                var stopwatch = Stopwatch.StartNew();

                try
                {
                    var result = await next();
                    if (result.Exception != null && !result.ExceptionHandled)
                    {
                        auditInfo.Exception = result.Exception;
                    }
                }
                catch (Exception ex)
                {
                    auditInfo.Exception = ex;
                    throw;
                }
                finally
                {
                    stopwatch.Stop();
                    auditInfo.ExecutionDuration = Convert.ToInt32(stopwatch.Elapsed.TotalMilliseconds);
                    await _auditingHelper.SaveAsync(auditInfo);
                }
            }
        }

        private bool ShouldSaveAudit(PageHandlerExecutingContext actionContext)
        {
            return _configuration.IsAuditingEnabled &&
                   actionContext.ActionDescriptor.IsPageAction() &&
                   _auditingHelper.ShouldSaveAudit(actionContext.HandlerMethod.MethodInfo, true);
        }

        public async Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
        {
            await Task.CompletedTask;
        }
    }
}