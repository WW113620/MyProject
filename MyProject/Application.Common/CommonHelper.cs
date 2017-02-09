using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Application.Common
{
    public class CommonHelper
    {
        public static string LoginCookieID { get { return "Login_Cookies_UserID"; } }
        public static string LoginCookieName { get { return "Login_Cookies_UserName"; } }
        public static string LoginCookieNameType { get { return "Login_Cookies_UserNameType"; } }
        public static string LoginCookieRealName { get { return "Login_Cookies_RealName"; } }
        public static string LoginCookieNickName { get { return "Login_Cookies_NickName"; } }
        public static string LoginCookiePassword { get { return "Login_Cookies_Userpwd"; } }

        public static string CreateGuid(string type)
        {
            string result = string.Empty;
            switch(type.ToLower())
            {
                default:
                case "user":
                    result = "U"+Guid.NewGuid().ToString("N");
                    break;
            }
            return result;
        }

        #region Common
          public static string GetPostValueUseRequest(string FormKey, int maxLength = 0, bool isClearHtml = false, bool isClearSql = false, bool isReservationSimpleTag = false)
        {
            string RetValue = string.Empty;
            try
            {
                RetValue = HttpContext.Current.Request[FormKey].ToString();
                if (string.IsNullOrWhiteSpace(RetValue))
                    return string.Empty;

                RetValue = RetValue.Trim();

                if (isClearHtml)
                {
                    RetValue = RetValue.StringNoHtml(isReservationSimpleTag, maxLength);
                }
                if (isClearSql)
                {
                    RetValue = RetValue.SqlFilterExt();
                }
                if (maxLength > 0 && RetValue.Length > maxLength)
                {
                    RetValue = RetValue.Substring(0, maxLength);
                }
            }
            catch (Exception ex)
            {

            }
            return RetValue;
        }

        /// <summary>
        /// 取得页面表单传递的值  仅取表单和url中的值
        /// </summary>
        /// <param name="FormKey"></param>
        /// <param name="maxLength">截取长度</param>
        /// <param name="isClearHtml">是否清除html标签</param>
        /// <param name="isClearSql">是否清除sql标签</param>
        /// <param name="isReservationSimpleTag">清除html标签时是否保留大于小于号、制表和换行符</param>
        /// <param name="routeData">路由中的值</param>
        /// <returns></returns>
        public static string GetPostValue(string FormKey, int maxLength = 0, bool isClearHtml = false, bool isClearSql = false, bool isReservationSimpleTag = false, System.Web.Routing.RouteValueDictionary routeDataValue = null)
        {
            string RetValue =string.Empty;
            try
            {
                if (routeDataValue != null && routeDataValue.Keys.Contains(FormKey) && routeDataValue[FormKey].ToString("").Length > 0)
                {
                    RetValue = routeDataValue[FormKey].ToString("");
                }
                else
                {
                    if (HttpContext.Current.Request.Form[FormKey] != null)
                    {
                        RetValue = HttpContext.Current.Request.Form[FormKey].ToString();
                    }
                    else if (HttpContext.Current.Request.QueryString[FormKey] != null)
                    {
                        RetValue = HttpContext.Current.Request.QueryString[FormKey].ToString();
                    }
                }
                if (string.IsNullOrWhiteSpace(RetValue))
                    return string.Empty;

                RetValue = RetValue.Trim();
                if (isClearHtml)
                {
                    RetValue = RetValue.StringNoHtml(isReservationSimpleTag, maxLength);
                }
                if (isClearSql)
                {
                    RetValue = RetValue.SqlFilterExt();
                }
                if (maxLength > 0 && RetValue.Length > maxLength)
                {
                    RetValue = RetValue.Substring(0, maxLength);
                }
                RetValue = RetValue.Trim();
            }
            catch (Exception ex)
            {

            }
            if (RetValue == "*")
                RetValue = string.Empty;
            return RetValue.TrimEnd(';');
        }


        /// <summary>
        /// 是否是整数值
        /// </summary>
        /// <param name="strValue">要验证的字符串</param>
        /// <returns>验证结果。是整数：true 否则：false</returns>
        public static bool isIntValue(string strValue)
        {
            try
            {
                Convert.ToInt64(strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 是否是浮点数值
        /// </summary>
        /// <param name="strValue">要验证的字符串</param>
        /// <returns>验证结果。是浮点数：true 否则：false</returns>
        public static bool isFloatValue(string strValue)
        {
            try
            {
                Convert.ToSingle(strValue);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 去掉字符串中的数字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveNumber(string key)
        {
            return Regex.Replace(key, @"\d", "");
        }

        /// <summary>
        /// 去掉字符串中的非数字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveNotNumber(string key)
        {
            return Regex.Replace(key, @"[^\d]*", "");
        }
        /// <summary>
        ///全角半角数字转换
        /// </summary>
        /// <param name="strNum">要转换的数字</param>
        /// <returns>转换结果</returns>
        public static string ChangeCharToInt(string strNum)
        {
            strNum = strNum.Replace("０", "0");
            strNum = strNum.Replace("１", "1");
            strNum = strNum.Replace("２", "2");
            strNum = strNum.Replace("３", "3");
            strNum = strNum.Replace("４", "4");

            strNum = strNum.Replace("５", "5");
            strNum = strNum.Replace("６", "6");
            strNum = strNum.Replace("７", "7");
            strNum = strNum.Replace("８", "8");
            strNum = strNum.Replace("９", "9");
            return strNum;
        }

        /// <summary>
        /// 字符中不能存在数字
        /// </summary>
        /// <param name="str">要验证的字符串</param>
        /// <returns>返回结果：包含数字返回false 否则返回true</returns>
        public static bool IsHaveIntChar(string str)
        {
            bool isTrue = true;
            string strCutChar = "";
            for (int i = 0; i < str.Length; i++)
            {
                strCutChar = str.Substring(i, 1);
                if (isIntValue(strCutChar))
                {
                    isTrue = false;
                    break;
                }
            }
            return isTrue;
        }

        /// <summary>
        /// 是否是四位字母
        /// </summary>
        /// <param name="code">要验证的字符串</param>
        /// <returns>是4位字母返回true 否则返回false</returns>
        public static bool IsMagazineCode(string code)
        {
            string pattern = @"^[a-zA-Z]{4}$";
            return (new System.Text.RegularExpressions.Regex(pattern)).IsMatch(code);
        }

        /// <summary>
        /// 获取手机运营商
        /// </summary>
        /// <param name="mobile">手机号</param>
        /// <returns>手机运营商</returns>
        public static CellularCarrierType GetCellularCarrierType(string mobile)
        {
            if (mobile.StartsWith("0") || mobile.StartsWith("+860"))
            {
                mobile = mobile.Substring(mobile.IndexOf("") + 1);
            }

            if (IsMobile(mobile))
            {
                string[] chinaUnicom = new string[] { "130", "131", "132", "155", "156", "186" };
                string[] chinaMobile = new string[] { "134", "135", "136", "137", "138", "139", "150", "151", "152", "157", "158", "159", "188" };
                string[] chinaTelecom = new string[] { "133", "153", "189" };
                if (chinaUnicom.Contains(mobile.Substring(0, 3)))
                {
                    return CellularCarrierType.联通;
                }
                if (chinaMobile.Contains(mobile.Substring(0, 3)))
                {
                    return CellularCarrierType.移动;
                }

                if (chinaTelecom.Contains(mobile.Substring(0, 3)))
                {
                    return CellularCarrierType.电信;
                }
            }

            return CellularCarrierType.其它;


        }

        /// <summary>
        /// 是否是手机
        /// </summary>
        /// <param name="inputStr">手机号</param>
        /// <returns>是手机号返回true 否则返回false</returns>
        public static bool IsMobile(string inputStr)
        {
            if (inputStr.Trim().Length != 11)
                return false;

            string s = @"^(1[3-578][0-9])\d{8}$"; // @"^(13[0-9]|15[0-9]|18[0-9])\d{8}$"; //at: 2015616
            return System.Text.RegularExpressions.Regex.IsMatch(inputStr, s);
        }

        /// <summary>
        /// 检测非法字符
        /// </summary>
        /// <param name="para">要验证的字符串</param>
        /// <returns>包含非法字符返回 false 否则返回true</returns>
        public static bool checkParameter(String para)
        {
            int flag = 0;
            flag += para.IndexOf("'") + 1;
            flag += para.IndexOf(";") + 1;
            flag += para.IndexOf("1=1") + 1;
            flag += para.IndexOf("|") + 1;
            flag += para.IndexOf("<") + 1;
            flag += para.IndexOf(">") + 1;
            if (flag != 0)
            {
                return false;
            }
            return true;
        }

        public static bool IsEmail(string strValue)
        {
            //邮箱的首字母可以是数字，例如qq的邮箱。
            //在C#中，半角句点 .，不再代表任意字符，而是一个普通字符
            if (string.IsNullOrEmpty(strValue))
                return false;
            Regex regex = new Regex(@"^[a-zA-Z0-9][a-zA-Z0-9_\-]*@[a-zA-Z0-9_\-]+(\.[a-zA-Z0-9_\-]+)+$", RegexOptions.IgnoreCase);
            return regex.IsMatch(strValue);
        }

        public static bool IsUrlFormat(string strValue)
        {
            Regex re = new Regex(@"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?");
            return re.IsMatch(strValue);
        }

        /// <summary>
        /// 数字由阿拉伯数字转换为汉字，如：1300230012->十三亿零二十三万零十二
        /// </summary>
        /// <param name="digit"></param>
        /// <returns></returns>
        public static string MultiNumberToCn(int? digit)
        {
            StringBuilder CnResult = new StringBuilder();
            int length = 0;

            //781,834,892
            string realNumberString = digit.ToString();
            length = digit.ToString().Length;
            string CurrentCn = string.Empty;
            string PreCn = string.Empty;
            for (int i = 0; i < length; i++)
            {

                CurrentCn = SingleNumberToCn(realNumberString[i], length - i - 1);

                if (CurrentCn == "零" && CurrentCn.Equals(PreCn))
                {
                    PreCn = CurrentCn;
                    continue;
                }
                PreCn = CurrentCn;
                CnResult.Append(CurrentCn);
            }
            //替换掉：首尾的汉字：零
            return Regex.Replace(CnResult.ToString(), "^[\u96F6]+|[\u96F6]+$", "", RegexOptions.IgnoreCase).Replace("一十", "十");
        }

        private static string SingleNumberToCn(char digit, int unitIndex)
        {
            string[] ReadUnitName = new string[] { "", "十", "百", "千", "万", "十", "百", "千", "亿", "十", "百" };
            string ResultString = "";
            switch (digit)
            {
                case '0':
                    ResultString = "零";
                    break;
                case '1':
                    ResultString = "一" + ReadUnitName[unitIndex];
                    break;
                case '2':
                    ResultString = "二" + ReadUnitName[unitIndex];
                    break;
                case '3':
                    ResultString = "三" + ReadUnitName[unitIndex];
                    break;
                case '4':
                    ResultString = "四" + ReadUnitName[unitIndex];
                    break;
                case '5':
                    ResultString = "五" + ReadUnitName[unitIndex];
                    break;
                case '6':
                    ResultString = "六" + ReadUnitName[unitIndex];
                    break;
                case '7':
                    ResultString = "七" + ReadUnitName[unitIndex];
                    break;
                case '8':
                    ResultString = "八" + ReadUnitName[unitIndex];
                    break;
                case '9':
                    ResultString = "九" + ReadUnitName[unitIndex];
                    break;
                default:
                    ResultString = null;
                    break;

            }
            return ResultString;
        }
  
        #region 连接符连接的数字字符串和数字列表转换
        /// <summary>
        /// 数字列表转换为使用连接符连接的字符串
        /// </summary>
        /// <param name="rangList"></param>
        /// <param name="strGapJoin">数字不连续时的连接符</param>
        /// <param name="strContinuousJoin">数字连续时的连接符</param>
        /// <returns></returns>
        public static string CovertToRangString(List<int> rangList, string strGapJoin = "+", string strContinuousJoin = "-")
        {
            if (rangList == null || rangList.Count == 0)
                return string.Empty;
            System.Text.StringBuilder sbRang = new System.Text.StringBuilder();
            RemoveRepeatIntListItem(ref rangList, true);
            int iCount = rangList.Count;
            if (iCount == 1)
                sbRang.Append(rangList[0]);
            else
            {
                for (int i = 0; i < iCount; i++)
                {
                    if (i == 0) //第一个
                    {
                        if (rangList[i + 1] - rangList[i] == 1) //跟后面一个的差值 = 1, 添加 - 
                            sbRang.AppendFormat("{0}-", rangList[i]);
                        else //跟后面一个的差值 > 1, 添加 + 
                            sbRang.AppendFormat("{0}+", rangList[i]);
                    }
                    else if (i == iCount - 1) //最后一个 
                    {
                        sbRang.Append(rangList[i]);
                    }
                    else //中间的
                    {
                        if (rangList[i] - rangList[i - 1] > 1) //跟前面一个差值 > 1
                        {
                            if (rangList[i + 1] - rangList[i] == 1) //跟后面一个差值 = 1
                                sbRang.AppendFormat("{0}-", rangList[i]);
                            else //跟后面一个的差值 > 1
                                sbRang.AppendFormat("{0}+", rangList[i]);
                        }
                        else //跟前面的差值 = 1
                        {
                            if (rangList[i + 1] - rangList[i] == 1) //跟后面一个差值 = 1
                            { }
                            else //跟后面一个的差值 > 1
                            {
                                sbRang.AppendFormat("{0}+", rangList[i]);
                            }
                        }
                    }
                }
            }
            return sbRang.ToString();
        }

        /// <summary>
        /// 使用连接符连接的字符串转换为数字列表
        /// </summary>
        /// <param name="strRange"></param>
        /// <param name="charGapSplit">数字不连续时的连接符数组</param>
        /// <param name="charContinuousSplit">数字连续时的连接符数组</param>
        /// <returns></returns>
        public static List<int> CovertToIntList(string strRange, char[] charGapSplit = null, char[] charContinuousSplit = null)
        {
            List<int> rangeList = new List<int>();
            if (string.IsNullOrEmpty(strRange))
                return rangeList;
            if (charGapSplit == null || charGapSplit.Length == 0)
                charGapSplit = new char[] { '+' };
            if (charContinuousSplit == null || charContinuousSplit.Length == 0)
                charContinuousSplit = new char[] { '-' };

            string[] strRangeArr_1 = strRange.Split(charGapSplit, StringSplitOptions.RemoveEmptyEntries);
            string[] strRangeArr_2 = { };
            int iTemp_A = 0;
            int iTemp_B = 0;
            foreach (string strRange_1 in strRangeArr_1)
            {
                strRangeArr_2 = strRange_1.Split(charContinuousSplit, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strRangeArr_2.Length; i++)
                {
                    if (Int32.TryParse(strRangeArr_2[i], out iTemp_A))
                    {
                        if (i + 1 < strRangeArr_2.Length && Int32.TryParse(strRangeArr_2[i + 1], out iTemp_B))
                        {
                            if (iTemp_A <= iTemp_B)
                            {
                                for (int j = iTemp_A; j <= iTemp_B; j++)
                                {
                                    rangeList.Add(j);
                                }
                            }
                            else
                            {
                                for (int j = iTemp_A; j >= iTemp_B; j--)
                                {
                                    rangeList.Add(j);
                                }
                            }
                        }
                        else
                        {
                            rangeList.Add(iTemp_A);
                        }
                    }
                }
            }
            for (int i = 0; i < rangeList.Count; i++)
            {
                for (int j = i + 1; j < rangeList.Count; j++)
                {
                    if (rangeList[i] == rangeList[j])
                    {
                        rangeList.RemoveAt(j);
                        j--;
                    }
                }
            }
            //rangeList.Sort();
            RemoveRepeatIntListItem(ref rangeList, true);
            return rangeList;
        }

        /// <summary>
        /// 去除列表中重复的数字,并排序
        /// </summary>
        /// <param name="list"></param>
        /// <param name="bIsAsc"></param>
        public static void RemoveRepeatIntListItem(ref List<int> list, bool bIsAsc = true)
        {
            if (list == null || list.Count == 0)
                return;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[i] == list[j])
                    {
                        list.RemoveAt(j);
                        j--;
                    }
                }
            }
            if (bIsAsc)
                list = list.OrderBy(s => s).ToList<int>();
            else
                list = list.OrderByDescending(s => s).ToList<int>();
        }
        #endregion
     
        //输出指定数值四舍五入后，保留decimals位小数的结果
        public static string DigitDecimal(decimal? value, int decimals)
        {
            decimal temp = Math.Round(value.GetValueOrDefault(0), decimals);
            string strFormat = string.Empty;
            string formatString = string.Format("0.{0}", strFormat.PadRight(decimals, '0'));
            return temp.ToString(formatString);

        }

        #endregion
    }

    public enum CellularCarrierType : int
    {
        联通 = 1,
        移动 = 2,
        电信 = 3,
        其它 = 9
    }
}