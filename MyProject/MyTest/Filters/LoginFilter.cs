using Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyTest.Filters
{
   
    /// <summary>
    /// 验证特性（验证是否拥登录）
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class LoginFilter : ActionFilterAttribute, IActionFilter
    {
        /// <summary>
        /// Action执行前
        /// </summary>
        /// <param name="filterContext">上下文</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                object[] actionFilter = filterContext.ActionDescriptor.GetCustomAttributes(typeof(NoCompress), false);
                object[] controllerFilter = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(typeof(NoCompress), false);
                if (controllerFilter.Length == 1 || actionFilter.Length == 1)
                {
                    return;
                }
                if (LoginHelper.UserId==0||string.IsNullOrEmpty(LoginHelper.UserId.ToString()) || string.IsNullOrEmpty(LoginHelper.UserName))
                {
                    HttpContext.Current.Response.Clear();
                    HttpContext.Current.Response.Write("<script>window.location.href='/Home/Index'</script>");
                    HttpContext.Current.Response.End();
                    filterContext.Result = new EmptyResult();
                }
                base.OnActionExecuting(filterContext);
            }
            catch (Exception exception)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Write(exception.Message);
                HttpContext.Current.Response.End();
                filterContext.Result = new EmptyResult();
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class NoCompress : Attribute
    {
        public NoCompress()
        {
        }
    }
}