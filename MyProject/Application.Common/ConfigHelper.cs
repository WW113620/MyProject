using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Application.Common
{
    public class ConfigHelper
    {
        public static readonly string strConnection = ConfigurationManager.ConnectionStrings["strConn"].ConnectionString;

        /// <summary>
        /// 取得配置文件中的appSettings值
        /// </summary>
        /// <param name="sKey">appSettings key值</param>
        /// <returns>value值</returns>
        public static string GetConfigValue(string sKey)
        {          
            string sValue = null;
            if ((sValue = System.Configuration.ConfigurationManager.AppSettings[sKey]) == null)
            {
                sValue = "";
            }
            return sValue;
        }

        public static string LoginAction
        {
           
            get { return string.Format("{0}/Home/Index" ,  HttpContext.Current.Request.Url.Authority); }
        }

    }

}