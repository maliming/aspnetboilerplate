using System.Reflection;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Abp.AspNetCore.Mvc.Extensions
{
    public static class ActionDescriptorExtensions
    {
        public static ControllerActionDescriptor AsControllerActionDescriptor(this ActionDescriptor actionDescriptor)
        {
            if (!actionDescriptor.IsControllerAction())
            {
                throw new AbpException($"{nameof(actionDescriptor)} should be type of {typeof(ControllerActionDescriptor).AssemblyQualifiedName}");
            }

            return actionDescriptor as ControllerActionDescriptor;
        }

        public static PageActionDescriptor AsPageActionDescriptor(this ActionDescriptor actionDescriptor)
        {
            if (!actionDescriptor.IsPageAction())
            {
                throw new AbpException(
                    $"{nameof(actionDescriptor)} should be type of {typeof(PageActionDescriptor).AssemblyQualifiedName}");
            }

            return actionDescriptor as PageActionDescriptor;
        }

        public static CompiledPageActionDescriptor AsCompiledPageActionDescriptor(this ActionDescriptor actionDescriptor)
        {
            if (!actionDescriptor.IsCompiledPageAction())
            {
                throw new AbpException(
                    $"{nameof(actionDescriptor)} should be type of {typeof(CompiledPageActionDescriptor).AssemblyQualifiedName}");
            }

            return actionDescriptor as CompiledPageActionDescriptor;
        }

        public static MethodInfo GetMethodInfo(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.AsControllerActionDescriptor().MethodInfo;
        }

        public static bool IsControllerAction(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor is ControllerActionDescriptor;
        }

        public static bool IsPageAction(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor is PageActionDescriptor;
        }

        public static bool IsCompiledPageAction(this ActionDescriptor actionDescriptor)
        {
            return actionDescriptor is CompiledPageActionDescriptor;
        }
    }
}
