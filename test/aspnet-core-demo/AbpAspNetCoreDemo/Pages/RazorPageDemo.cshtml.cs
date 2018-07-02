using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AbpAspNetCoreDemo.Pages
{
    public class RazorPageDemo : AbpPageModel
    {
        public RazorPageDemo()
        {
            LocalizationSourceName = "AbpAspNetCoreDemoModule";
        }

        public void OnGet()
        {

        }

        public void OnPost()
        {
            throw new ArgumentException("arg exception");
        }
    }
}
