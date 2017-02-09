using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Application.Common
{
    public static class ConvertHelper
    {
        #region 普通转换
        /// <summary>
        /// 转换Int类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RtnData"></param>
        /// <returns></returns>
        public static int ToInt(this object data, int RtnData)
        {
            int rtnData = RtnData;
            try
            {
                rtnData = Convert.ToInt32(data);
            }
            catch
            {
                rtnData = RtnData;
            }
            return rtnData;
        }

        /// <summary>
        /// 转换long类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RtnData"></param>
        /// <returns></returns>
        public static long ToLong(this object data, long RtnData)
        {
            long rtnData = RtnData;
            try
            {
                if (data != null && data.ToString() != "")
                    rtnData = Convert.ToInt64(data);
            }
            catch
            {
                rtnData = RtnData;
            }
            return rtnData;
        }

        /// <summary>
        /// 转换float类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RtnData"></param>
        /// <returns></returns>
        public static float ToFloat(this object data, float RtnData)
        {
            float rtnData = RtnData;
            try
            {
                if (data != null)
                    rtnData = Convert.ToSingle(data);
            }
            catch
            {
                rtnData = RtnData;
            }
            return rtnData;
        }

        /// <summary>
        /// 转换人名币类型字符串
        /// </summary>
        /// <param name="Money"></param>
        /// <returns></returns>
        public static string ToMoney(this object Money)
        {
            double floatMoney = Money.ToDouble(0);
            if (floatMoney <= 0)
            {
                return "0.00";
            }
            else
            {
                floatMoney = Math.Round(floatMoney, 1, MidpointRounding.AwayFromZero);//
                string rtnStr = floatMoney.ToString("0.0") + "0";
                return rtnStr;
            }
        }

        /// <summary>
        /// 转换人名币类型字符串
        /// </summary>
        /// <param name="Money"></param>
        /// <returns></returns>
        public static string ToMoney_2(this object Money)
        {
            double floatMoney = Money.ToDouble(0);
            if (floatMoney <= 0)
            {
                return "0.00";
            }
            else
            {
                floatMoney = Math.Round(floatMoney, 2, MidpointRounding.AwayFromZero);//
                string rtnStr = floatMoney.ToString("0.00");
                return rtnStr;
            }
        }


        /// <summary>
        /// 转换double类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RtnData"></param>
        /// <returns></returns>
        public static double ToDouble(this object data, double RtnData)
        {
            double rtnData = RtnData;
            try
            {
                if (data != null)
                    rtnData = Convert.ToDouble(data);
            }
            catch
            {
                rtnData = RtnData;
            }
            return rtnData;
        }
        /// <summary>
        /// 转换decimal类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RtnData"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this object data, decimal RtnData)
        {
            decimal rtnData = RtnData;
            try
            {
                if (data != null)
                    rtnData = Math.Round(Convert.ToDecimal(data), 2, MidpointRounding.AwayFromZero);
            }
            catch
            {
                rtnData = RtnData;
            }
            return rtnData;
        }

        /// <summary>
        /// 转换decimal类型(保留两位小数)
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RtnData"></param>
        /// <returns></returns>
        public static decimal ToDecimal_2(this object data, decimal RtnData)
        {
            decimal rtnData = RtnData;
            try
            {
                if (data != null)
                    rtnData = Convert.ToDecimal(data);
            }
            catch
            {
                rtnData = RtnData;
            }
            return rtnData;
        }

        /// <summary>
        /// 转换string类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RtnData"></param>
        /// <returns></returns>
        public static string ToString(this object data, string RtnData)
        {
            string rtnData = RtnData;
            try
            {
                if (data != null && data.ToString().Length > 0)
                    rtnData = Convert.ToString(data);
            }
            catch
            {
                rtnData = RtnData;
            }
            return rtnData;
        }

        /// <summary>
        /// 转换char类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RtnData"></param>
        /// <returns></returns>
        public static char ToChar(this object data, char RtnData)
        {
            char rtnData = RtnData;
            try
            {
                if (data != null && data.ToString().Length > 0)
                    rtnData = Convert.ToChar(data);
            }
            catch
            {
                rtnData = RtnData;
            }
            return rtnData;
        }

        /// <summary>
        /// 转换datetime类型
        /// </summary>
        /// <param name="data">要转换的字符串</param>
        /// <param name="RtnData">默认返回值</param>
        /// <param name="format">格式化时间字符串</param>
        /// <returns></returns>
        public static string ToDateTime(this object data, string RtnData, string format)
        {
            string rtnData = RtnData;
            try
            {
                if (data != null && data.ToString().Length > 0 && Convert.ToDateTime(data).ToString("yyyy") != "1900")
                    rtnData = Convert.ToDateTime(data).ToString(format);
            }
            catch
            {
            }
            return rtnData;
        }

        /// <summary>
        /// 转换datetime类型
        /// </summary>
        /// <param name="data"></param>
        /// <param name="RtnData"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this object data, DateTime RtnData)
        {
            DateTime rtnData = RtnData;
            try
            {
                if (data != null && data.ToString().Length > 0 && Convert.ToDateTime(data).ToString("yyyy") != "1900")
                    rtnData = Convert.ToDateTime(data);
            }
            catch
            {
            }
            return rtnData;
        }

        /// <summary>
        /// 转换guid类型
        /// </summary>
        /// <param name="value"></param>
        /// <param name="guid"></param>
        /// <returns></returns>
        //public static Guid ToGuid(this string value, Guid guid)
        //{
        //    Guid rtnGuid;
        //    if (!Guid.TryParse(value, out rtnGuid))
        //        rtnGuid = guid;
        //    return rtnGuid;
        //}

        public static byte[] StreamToBytes(System.IO.Stream stream)
        {
            if (stream == null)
                return null;
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, System.IO.SeekOrigin.Begin);
            return bytes;
        }
        public static System.IO.Stream BytesToStream(byte[] bytes)
        {
            if (bytes == null)
                return null;
            System.IO.Stream stream = new System.IO.MemoryStream(bytes);
            return stream;
        }


        /// <summary>
        /// 为字符串中的非英文字符编码
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToHexString(string s)
        {
            char[] chars = s.ToCharArray();
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int index = 0; index < chars.Length; index++)
            {
                bool needToEncode = NeedToEncode(chars[index]);
                if (needToEncode)
                {
                    string encodedString = ToHexString(chars[index]);
                    builder.Append(encodedString);
                }
                else
                {
                    builder.Append(chars[index]);
                }
            }

            return builder.ToString();
        }

        /// <summary>
        ///指定 一个字符是否应该被编码
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static bool NeedToEncode(char chr)
        {
            string reservedChars = "$-_.+!*'(),@=&";

            if (chr > 127)
                return true;
            if (char.IsLetterOrDigit(chr) || reservedChars.IndexOf(chr) >= 0)
                return false;

            return true;
        }

        /// <summary>
        /// 为非英文字符串编码
        /// </summary>
        /// <param name="chr"></param>
        /// <returns></returns>
        private static string ToHexString(char chr)
        {
            System.Text.UTF8Encoding utf8 = new System.Text.UTF8Encoding();
            byte[] encodedBytes = utf8.GetBytes(chr.ToString());
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int index = 0; index < encodedBytes.Length; index++)
            {
                builder.AppendFormat("%{0}", Convert.ToString(encodedBytes[index], 16));
            }
            return builder.ToString();
        }
        #endregion

        #region 强制转化

        /// <summary>
        /// object转化为Bool类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool ObjToBool(this object obj)
        {
            bool flag;
            if (obj == null)
            {
                return false;
            }
            if (obj.Equals(DBNull.Value))
            {
                return false;
            }
            return (bool.TryParse(obj.ToString(), out flag) && flag);
        }

        /// <summary>
        /// object强制转化为DateTime类型(吃掉异常)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ObjToDateNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            try
            {
                return new DateTime?(Convert.ToDateTime(obj));
            }
            catch (ArgumentNullException ex)
            {
                return null;
            }
        }

        /// <summary>
        /// int强制转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ObjToInt(this object obj)
        {
            if (obj != null)
            {
                int num;
                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }
                if (int.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }
            return 0;
        }

        /// <summary>
        /// 强制转化为long
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ObjToLong(this object obj)
        {
            if (obj != null)
            {
                long num;
                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }
                if (long.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }
            return 0;
        }

        /// <summary>
        /// 强制转化可空int类型
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int? ObjToIntNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            return new int?(ObjToInt(obj));
        }

        /// <summary>
        /// 强制转化为string
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjToStr(this object obj)
        {
            if (obj == null)
            {
                return "";
            }

            if (obj.Equals(DBNull.Value))
            {
                return "";
            }
            return Convert.ToString(obj);
        }

        /// <summary>
        /// Decimal转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal ObjToDecimal(this object obj)
        {
            if (obj == null)
            {
                return 0M;
            }
            if (obj.Equals(DBNull.Value))
            {
                return 0M;
            }
            try
            {
                return Convert.ToDecimal(obj);
            }
            catch
            {
                return 0M;
            }
        }

        /// <summary>
        /// Decimal可空类型转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal? ObjToDecimalNull(this object obj)
        {
            if (obj == null)
            {
                return null;
            }
            if (obj.Equals(DBNull.Value))
            {
                return null;
            }
            return new decimal?(ObjToDecimal(obj));
        }

        /// <summary>
        /// Double强制转化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ObjToDouble(this object obj)
        {
            if (obj != null)
            {
                double num;
                if (obj.Equals(DBNull.Value))
                {
                    return 0;
                }
                if (double.TryParse(obj.ToString(), out num))
                {
                    return num;
                }
            }
            return 0;
        }
        #endregion

        #region 判断对象是否为空

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(this T data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()) || data.ToString() == "")
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }

        /// <summary>
        /// 判断对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(this object data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }
        #endregion

        #region 验证判断
        public static bool IsInclude(string[] IPRegion, string IP)
        {
            //验证    
            if (null == IPRegion || null == IP || 0 == IPRegion.Length)
                return false;

            if (!ValidateIPAddress(IP))
                return false;

            if (1 == IPRegion.Length)
            {
                if (!ValidateIPAddress(IPRegion[0]))
                    return false;

                if (0 == Compare(IPRegion[0], IP))
                    return true;
            }

            if (!(ValidateIPAddress(IPRegion[0]) && ValidateIPAddress(IPRegion[1])))
                return false;

            uint IPNum = TransNum(IP);
            uint IPNum1 = TransNum(IPRegion[0]);
            uint IPNum2 = TransNum(IPRegion[1]);

            //比较    
            if (Math.Min(IPNum1, IPNum2) <= IPNum && Math.Max(IPNum1, IPNum2) >= IPNum)
                return true;

            return false;
        }
        public static bool ValidateIPAddress(string strIP)
        {
            if (null == strIP || "" == strIP.Trim() || Convert.IsDBNull(strIP))
                return false;

            return Regex.IsMatch(strIP, @"^((\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.){3}(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])$");
        }
        public static int Compare(string IP1, string IP2)
        {
            if (!(ValidateIPAddress(IP1) && ValidateIPAddress(IP2)))
                throw new Exception("IP Address isn''t Well Format!");

            uint IPNum1 = TransNum(IP1);
            uint IPNum2 = TransNum(IP2);

            if (IPNum1 == IPNum2)
                return 0;

            return IPNum1 > IPNum2 ? 1 : -1;
        }

        #region 检查输入的参数是不是某些定义好的特殊字符：这个方法目前用于密码输入的安全检查
        /// <summary>
        /// 检查输入的参数是不是某些定义好的特殊字符：这个方法目前用于密码输入的安全检查
        /// </summary>
        public static bool isContainSpecChar(string strInput)
        {
            string[] list = new string[] { "123456", "654321" };
            bool result = new bool();
            for (int i = 0; i < list.Length; i++)
            {
                if (strInput == list[i])
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
        #endregion


        #region 判断是否以英文字符结束
        public static bool IsEndWithLetter(string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
                return false;
            return Regex.IsMatch(inputStr, "[a-zA-Z]$");
        }
        #endregion
        #endregion

        #region 其它转换
        public static uint TransNum(string IPAddr)
        {
            if (!ValidateIPAddress(IPAddr))
                throw new Exception("IP Address isn''t Well Format!");

            string[] IPStrArray = new string[4];
            IPStrArray = IPAddr.Split('.');
            return MAKELONG(MAKEWORD(byte.Parse(IPStrArray[3]), byte.Parse(IPStrArray[2])), MAKEWORD(byte.Parse(IPStrArray[1]), byte.Parse(IPStrArray[0])));
        }
        /// <summary>    
        /// 移位转换_8    
        /// </summary>    
        /// <param name="bLow"></param>    
        /// <param name="bHigh"></param>    
        /// <returns></returns>    
        private static ushort MAKEWORD(byte bLow, byte bHigh)
        {
            return ((ushort)(((byte)(bLow)) | ((ushort)((byte)(bHigh))) << 8));
        }

        /// <summary>    
        /// 移位转换_16    
        /// </summary>    
        /// <param name="bLow"></param>    
        /// <param name="bHigh"></param>    
        /// <returns></returns>    
        private static uint MAKELONG(ushort bLow, ushort bHigh)
        {
            return ((uint)(((ushort)(bLow)) | ((uint)((ushort)(bHigh))) << 16));
        }

        /// <summary>
        /// 移除字符串中的所有空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimAll(string str)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            CharEnumerator CEnumerator = str.GetEnumerator();
            while (CEnumerator.MoveNext())
            {
                byte[] array = new byte[1];
                array = System.Text.Encoding.ASCII.GetBytes(CEnumerator.Current.ToString());
                int asciicode = (short)(array[0]);
                if (asciicode != 32)
                {
                    sb.Append(CEnumerator.Current.ToString());
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将字符串转换为数组
        /// </summary>
        /// <param name="str"></param>
        /// <param name="strSplit"></param>
        /// <returns></returns>
        public static string[] TransStringToArray(string str, char strSplit)
        {
            string[] arr;
            str = string.IsNullOrEmpty(str) ? string.Empty : str;
            if (str.IndexOf(strSplit) == -1)
            {
                arr = new string[] { str };
            }
            else
            {
                string[] temparr = str.Split(strSplit);
                arr = new string[temparr.Length - 1];
                for (int i = 0; i < temparr.Length; i++)
                {
                    if (temparr[i] != "")
                        arr[i] = temparr[i];
                }
            }
            return arr;
        }
        #endregion

        #region 可转换类型验证
        /// <summary>
        /// 是否为日期型字符串
        /// </summary>
        /// <param name="StrSource">日期字符串(2008-05-08)</param>
        /// <returns></returns>
        public static bool IsDate(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-9]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$");
        }

        /// <summary>
        /// 是否为时间型字符串
        /// </summary>
        /// <param name="source">时间字符串(15:00:00)</param>
        /// <returns></returns>
        public static bool IsTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^((20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$");
        }

        /// <summary>
        /// 是否为日期+时间型字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsDateTime(string StrSource)
        {
            return Regex.IsMatch(StrSource, @"^(((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-)) (20|21|22|23|[0-1]?\d):[0-5]?\d:[0-5]?\d)$ ");
        }

        /// <summary>
        /// 是否为日期 用TryParse判断
        /// </summary>
        /// <param name="strSource"></param>
        /// <returns></returns>
        public static bool IsDateTime2(string strSource)
        {
            DateTime dt = DateTime.Now;
            return DateTime.TryParse(strSource, out dt);
        }
        /// <summary>
        /// 是否为Int32
        /// </summary>
        /// <param name="StrSource"></param>
        /// <returns></returns>
        public static bool IsInt32(string StrSource)
        {
            try
            {
                Int32.Parse(StrSource);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 匹配正整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUint(string input)
        {
            return Regex.IsMatch(input, @"^[0-9]*[1-9][0-9]*$");
        }

        /// <summary>
        /// 匹配非负整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNotNagtive(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }

        /// <summary>
        /// 判断输入的字符串只包含数字
        /// 可以匹配整数和浮点数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumber(string input)
        {
            string pattern = "^-?\\d+$|^(-?\\d+)(\\.\\d+)?$";
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// 判断输入的字符串字包含英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEnglisCh(string input)
        {
            return Regex.IsMatch(input, @"^[A-Za-z]+$");
        }

        /// <summary>
        /// 是否包含有汉字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChineseCh(string input)
        {
            return Regex.IsMatch(input, @"^[\u4e00-\u9fa5]+$");
        }

        /// <summary>
        /// 是否只包含数字和英文字母
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumAndEnCh(string input)
        {
            return Regex.IsMatch(input, @"^[A-Za-z0-9]+$");
        }

        /// <summary>
        /// 是否只包含数字\英文字母和下划线
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNumEnAndUnderlineCh(string input)
        {
            return Regex.IsMatch(input, @"^[A-Za-z0-9_]+$");
        }

        /// <summary>
        /// 是否是一个url链接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsURL(string input)
        {
            string pattern = @"^[a-zA-Z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$";
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// 是否表示ip4地址
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsIPv4(string input)
        {
            string[] IPs = input.Split('.');

            for (int i = 0; i < IPs.Length; i++)
            {
                if (!Regex.IsMatch(IPs[i], @"^\d+$"))
                {
                    return false;
                }
                if (Convert.ToUInt16(IPs[i]) > 255)
                {
                    return false;
                }
            }
            return true;
        }


        /// <summary>
        /// 匹配3位或4位区号的电话号码，其中区号可以用小括号括起来，
        /// 也可以不用，区号与本地号间可以用连字号或空格间隔，
        /// 也可以没有间隔
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhone(string input)
        {
            string pattern = "^\\(0\\d{2}\\)[- ]?\\d{8}$|^0\\d{2}[- ]?\\d{8}$|^\\(0\\d{3}\\)[- ]?\\d{7}$|^0\\d{3}[- ]?\\d{7}$";
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// 是否是手机 匹配13/2/4/5/7/8/ 开头的11位号码
        /// </summary>
        /// <param name="inputStr">手机号</param>
        /// <returns>是手机号返回true 否则返回false</returns>
        public static bool IsMobile(string inputStr)
        {
            if (string.IsNullOrWhiteSpace(inputStr) || inputStr.Trim().Length != 11)
                return false;

            string s = @"^(1[3-578][0-9])\d{8}$"; // @"^(13[0-9]|15[0-9]|18[0-9])\d{8}$"; //at: 2015616
            return Regex.IsMatch(inputStr, s);
        }

        /// <summary>
        /// 是否为email
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(string input)
        {
            string pattern = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            return Regex.IsMatch(input, pattern);
        }



        #endregion

        #region 充值类型转换
        public static string ShowRechargeTypeName(int type)
        {

            string rtnStr = "";
            switch (type)
            {
                case 2:
                    rtnStr = "银联在线";
                    break;
                case 3:
                    rtnStr = "网银";
                    break;
                case 4:
                    rtnStr = "支付宝";
                    break;
                case 5:
                    rtnStr = "会员充值卡";
                    break;
                case 6:
                    rtnStr = "财付通";
                    break;
                case 7:
                    rtnStr = "神州行卡";
                    break;
                case 8:
                    rtnStr = "移动短信";
                    break;
                case 9:
                    rtnStr = "联通短信";
                    break;
                case 10:
                    rtnStr = "电信短信";
                    break;
                case 11:
                    rtnStr = "微信支付";
                    break;
            }
            return rtnStr;
        }
        #endregion

        #region 转换数字{将数字1,2，3转成一，二，三}(by wangwei)
        /// <summary>
        /// 转换为为整数（最高位数为亿）
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static string IntToChineseNumerals(string x)
        {
            int len = x.Length;
            string ret, temp;
            if (len <= 4)
                ret = ChangeInt(x);
            else if (len <= 8)
            {
                ret = ChangeInt(x.Substring(0, len - 4)) + "万";
                temp = ChangeInt(x.Substring(len - 4, 4));
                if (temp.IndexOf("千") == -1 && temp != "")
                    ret += "零" + temp;
                else
                    ret += temp;
            }
            else
            {
                ret = ChangeInt(x.Substring(0, len - 8)) + "亿";
                temp = ChangeInt(x.Substring(len - 8, 4));
                if (temp.IndexOf("千") == -1 && temp != "")
                    ret += "零" + temp;
                else
                    ret += temp;
                ret += "万";
                temp = ChangeInt(x.Substring(len - 4, 4));
                if (temp.IndexOf("千") == -1 && temp != "")
                    ret += "零" + temp;
                else
                    ret += temp;
            }
            int i;
            if ((i = ret.IndexOf("零万")) != -1)
                ret = ret.Remove(i + 1, 1);
            while ((i = ret.IndexOf("零零")) != -1)
                ret = ret.Remove(i, 1);
            if (ret[ret.Length - 1] == '零' && ret.Length > 1)
                ret = ret.Remove(ret.Length - 1, 1);
            return ret;
        }

        public static char ToNum(char x)
        {
            string strChnNames = "零一二三四五六七八九";
            string strNumNames = "0123456789";
            return strChnNames[strNumNames.IndexOf(x)];
        }

        public static string ChangeInt(string x)
        {
            string[] strArrayLevelNames = new string[4] { "", "十", "百", "千" };
            string ret = "";
            int i;
            for (i = x.Length - 1; i >= 0; i--)
                if (x[i] == '0')
                    ret = ToNum(x[i]) + ret;
                else
                    ret = ToNum(x[i]) + strArrayLevelNames[x.Length - 1 - i] + ret;
            while ((i = ret.IndexOf("零零")) != -1)
                ret = ret.Remove(i, 1);
            if (ret[ret.Length - 1] == '零' && ret.Length > 1)
                ret = ret.Remove(ret.Length - 1, 1);
            if (ret.Length >= 2 && ret.Substring(0, 2) == "一十")
                ret = ret.Remove(0, 1);
            return ret;
        }
        #endregion

        #region 根据年月日计算星期
        /// <summary>
        /// 根据年月日计算星期几(Label2.Text=CaculateWeekDay(2004,12,9);)
        /// </summary>
        /// <param name="y">年</param>
        /// <param name="m">月</param>
        /// <param name="d">日</param>
        /// <returns></returns>
        public static string CaculateWeekDay(int y, int m, int d)
        {
            if (m == 1) m = 13;
            if (m == 2) m = 14;
            int week = (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7 + 1;
            string weekstr = "";
            switch (week)
            {
                case 1: weekstr = "星期一"; break;
                case 2: weekstr = "星期二"; break;
                case 3: weekstr = "星期三"; break;
                case 4: weekstr = "星期四"; break;
                case 5: weekstr = "星期五"; break;
                case 6: weekstr = "星期六"; break;
                case 7: weekstr = "星期日"; break;
            }
            return weekstr;
        }
        #endregion

        #region MD5加密
        /// <summary>
        /// md5加密
        /// </summary>
        /// <param name="input">要加密的字符串</param>
        /// <returns>加密结果</returns>
        public static string MD5Encrypt(string input)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            string encoded = BitConverter.ToString(md5.ComputeHash(Encoding.Default.GetBytes(input))).Replace("-", "");
            return encoded;
        }
        #endregion
    }
}