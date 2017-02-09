using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Application.Common
{
    public class LoginHelper
    {
        /// <summary>
        /// 登录地址
        /// </summary>
        public static string LoginUrl
        {
            get { return ConfigHelper.LoginAction; }
        }
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public static long UserId
        {
            get { return CookieHelper.GetCookie(CommonHelper.LoginCookieID).ToLong(0); }
        }
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public static string UserName
        {
            get { return CookieHelper.GetCookie(CommonHelper.LoginCookieName); }
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        public static void Logout()
        {
            CookieHelper.DelCookie(CommonHelper.LoginCookieID);
            CookieHelper.DelCookie(CommonHelper.LoginCookieName);
            if (HttpContext.Current.Session != null)
            {
                HttpContext.Current.Session.Clear();
            }
        }

    }
}