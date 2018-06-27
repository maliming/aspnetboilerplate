using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;

namespace Abp.AspNetCore.Mvc.RazorPages
{
    /// <summary>
    /// https://github.com/aspnet/Mvc/pull/7800 and https://github.com/aspnet/Mvc/issues/7024
    /// Waiting for MVC to release a new version.
    /// </summary>
    public class AbpServiceBasedPageModelActivatorProvider : IPageModelActivatorProvider
    {
        public Func<PageContext, object> CreateActivator(CompiledPageActionDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            var modelType = descriptor.ModelTypeInfo?.AsType();
            if (modelType == null)
            {
                throw new ArgumentException(nameof(modelType));
            }

            return context => context.HttpContext.RequestServices.GetRequiredService(modelType);
        }

        public Action<PageContext, object> CreateReleaser(CompiledPageActionDescriptor descriptor)
        {
            return null;
        }
    }
}