using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Application.Common
{
    public static class StringExtends
    {
        /// <summary>
        /// 处理掉不可见的Unicode字符
        /// </summary>
        private readonly static string[] mUnicodeChars = new string[] { "\u0000", "\u0001", "\u0002", "\u0003", "\u0004", "\u0005", "\u0006", "\u0007", "\u0008", "\u000b", "\u000c", "\u000e", "\u000f", "\u0010", "\u0011", "\u0012", "\u0013", "\u0014", "\u0015", "\u0016", "\u0017", "\u0018", "\u0019", "\u001a", "\u001b", "\u001c", "\u001d", "\u001e", "\u001f", "\\s{2,}", "\u007f" };
        //"\u0009","\u000a",, "\u000d"
        private readonly static string[] mUnicodePlusChars = new string[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F" };

        public static string ReplaceUnicodePlus(this string str) {
            foreach (string mMidLetter in mUnicodePlusChars)
            {
                foreach (string mChar in mUnicodePlusChars)
                {
                    foreach (string mEndLetter in mUnicodePlusChars)
                    {                 
                        str = Regex.Replace(str, string.Format(@"\ue{0}{1}{2}", mMidLetter, mChar, mEndLetter), "");
                    }
                }
            }
            return str;
        }
        public static string ReplaceUnicode(this string str)
        {
            string unicodeChars = System.Configuration.ConfigurationManager.AppSettings["UnicodeChars"];
            if (!string.IsNullOrEmpty(unicodeChars))
            {
                string[] rs = new string[] { };
                if (unicodeChars.Contains(";"))
                {
                    rs = unicodeChars.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                }
                else
                    rs = new string[] { unicodeChars };
                foreach (string r in rs)
                {
                    str = Regex.Replace(str, r, "");
                }
            }
            str = Regex.Replace(str, @"[。]{1}[\r\n\t\s]+", @"%%。%%%%%%%%");
            str = Regex.Replace(str, @"[？]{1}[\r\n\t\s]+", @"%%？%%%%%%%%");
            str = Regex.Replace(str, @"[！]{1}[\r\n\t\s]+", @"%%！%%%%%%%%");
            str = Regex.Replace(str, @"[.]{1}[\r\n\t\s]+", @"%%.%%%%%%%%");
            str = Regex.Replace(str, @"[?]{1}[\r\n\t\s]+", @"%%?%%%%%%%%");
            str = Regex.Replace(str, @"[!]{1}[\r\n\t\s]+", @"%%!%%%%%%%%");
            str = Regex.Replace(str, @"\r", "");
            str = Regex.Replace(str, @"\t", "");
            str = Regex.Replace(str, @"\n", "");
            str = Regex.Replace(str, @"%%。%%%%%%%%", "。\r\n\t\t\t\t");
            str = Regex.Replace(str, @"%%？%%%%%%%%", "？\r\n\t\t\t\t");
            str = Regex.Replace(str, @"%%！%%%%%%%%", "！\r\n\t\t\t\t");
            str = Regex.Replace(str, @"%%.%%%%%%%%", ".\r\n\t\t\t\t");
            str = Regex.Replace(str, @"%%?%%%%%%%%", "?\r\n\t\t\t\t");
            str = Regex.Replace(str, @"%%!%%%%%%%%", "!\r\n\t\t\t\t");
            str = Regex.Replace(str, @"[\$]+", "\r\n");
            foreach (string mChar in mUnicodeChars)
            {
                str = Regex.Replace(str, mChar, "");
            }
            return str;
        }

        public static string ReplaceEx(this string original, string pattern, string replacement)
        {
            int count, position0, position1, currentPosition;
            count = position0 = position1 = currentPosition = 0;
            string upperString = original.ToUpper();
            string upperPattern = pattern.ToUpper();
            int inc = (original.Length / pattern.Length) * (replacement.Length - pattern.Length);
            char[] chars = new char[original.Length + Math.Max(0, inc)];
            while ((position1 = upperString.IndexOf(upperPattern, position0)) != -1)
            {
                currentPosition++;
                for (int i = position0; i < position1; ++i)
                    chars[count++] = original[i];

                if (currentPosition % 3 == 0)
                {
                    for (int i = 0; i < replacement.Length; ++i)
                        chars[count++] = replacement[i];
                }
                else
                {
                    for (int i = 0; i < pattern.Length; ++i)
                        chars[count++] = pattern[i];
                }
                position0 = position1 + pattern.Length;
            }
            if (position0 == 0) return original;
            for (int i = position0; i < original.Length; ++i)
                chars[count++] = original[i];
            return new string(chars, 0, count);
        }

        /// <summary>
        /// 替换掉',()\n\r---等
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSqlTag(this string str)
        {
            if (str == null || str.Length == 0)
                return string.Empty;
            str = str.StringNoHtml();
            str = str.Replace("'", "‘").Replace(",", "，").Replace("(", "（").Replace(")", "）").Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("--", "－－").Replace("-", "－");
            return str;
        }

        private static string sqlfilter = "exec|insert|select|delete|update|chr|mid|master|or|and|truncate|char|declare|join|cmd|;|'|--";
        /// <summary>
        /// 去除sql保留字
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string SqlFilterExt(this string sql)
        {
            if (string.IsNullOrEmpty(sql))
                return string.Empty;
            foreach (string i in sqlfilter.Split('|'))
            {
                sql = sql.Replace(i + " ", string.Empty).Replace(" " + i, string.Empty).Replace(i + "%20", string.Empty).Replace("%20" + i, string.Empty);
            }
            sql = sql.Replace("'", "");
            return sql;
        }

        public static bool IsContainsSqlFilter(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            bool isContains = false;
            str = str.Trim();
            foreach (string i in sqlfilter.Split('|'))
            {
                if (str.IndexOf(i + "%20") != -1 || str.IndexOf("%20" + i) != -1 || str.IndexOf(i + " ") != -1 || str.IndexOf(" " + i) != -1)
                {
                    isContains = true;
                    break;
                }
            }
            return isContains;
        }
        
        /// <summary>
        /// 获取字符串字节数
        /// </summary>
        /// <param name="str1"></param>
        /// <returns></returns>
        public static long GetStringByteLength(this string str1)
        {
            string Text = str1.ToString("").Trim();
            long len = 0;
            try
            {
                for (int i = 0; i < Text.Length; i++)
                {
                    byte[] byte_len = System.Text.Encoding.Default.GetBytes(Text.Substring(i, 1));
                    if (byte_len.Length % 2 == 0)
                        len += 2;  //如果长度大于1，是中文，占两个字节，+2
                    else
                        len += 1;  //如果长度等于1，是英文，占一个字节，+1
                }
            }
            catch (Exception ex)
            { }
            return len;
        }

        /// <summary>
        /// 按字节截取字符串
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="MaxByteNum"></param>
        /// <param name="bl"></param>
        /// <returns></returns>
        public static string Cutbyte(this string str1, int MaxByteNum, bool bl = false)
        {
            if (str1 == null || str1.Length <= 0)
                return string.Empty;
            //if (str1.Length <= MaxByteNum)
            //    return str1;
            string Text = str1.ToString("").Trim();
            int len = 0;
            string returnStr = string.Empty;
            for (int i = 0; i < Text.Length; i++)
            {
                byte[] byte_len = System.Text.Encoding.Default.GetBytes(Text.Substring(i, 1));
                if (byte_len.Length % 2 == 0)
                    len += 2;  //如果长度大于1，是中文，占两个字节，+2
                else
                    len += 1;  //如果长度等于1，是英文，占一个字节，+1
                returnStr += Text.Substring(i, 1);
                if (len >= (MaxByteNum - 1))
                {
                    break;
                }
            }
            if (Text.Trim() != "" && bl && Text != returnStr)
                returnStr += "...";
            return returnStr;
        }

        #region 去除HTML标签
        /// <summary>
        /// 去掉所有的html标签 将 小于号,大于号,空格 替换为空字符串
        /// </summary>
        /// <param name="Htmlstring"></param>
        /// <param name="isReservationSimpleTag">是否保留大于小于号、制表和换行符</param>
        /// <param name="maxLength">截取的长度</param>
        /// <returns></returns>
        public static string ClearAllHTML(this string Htmlstring, bool isReservationSimpleTag = false, int maxLength = 0)
        {
            if (Htmlstring == null || Htmlstring.Length <= 0)
                return string.Empty;
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除iframe
            Htmlstring = Regex.Replace(Htmlstring, @"<iframe[^>]*?>.*?</iframe>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"%(27|32|3E|3C|3D|3F)", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            if (!isReservationSimpleTag)
            {
                Htmlstring = Htmlstring.Replace("<", string.Empty);
                Htmlstring = Htmlstring.Replace(">", string.Empty);
                Htmlstring = Htmlstring.Replace("\r", string.Empty);
                Htmlstring = Htmlstring.Replace("\n", string.Empty);
                Htmlstring = Htmlstring.Trim();
            }
            if (maxLength > 0 && Htmlstring.Length > maxLength)
                Htmlstring = Htmlstring.Substring(0, maxLength);
            return Htmlstring;
        }

        /// <summary>
        /// 去掉全部HTML的, 将 小于号=>  gt;,大于号=>  lt;,空格=>  nbsp 进行转换
        /// </summary>
        /// <param name="strHtml"></param>
        /// <param name="isReservationSimpleTag">是否保留大于小于号、制表和换行符</param>
        /// <param name="maxLength">截取的长度</param>
        /// <returns></returns>
        public static string StringNoHtml(this string strHtml, bool isReservationSimpleTag = false, int maxLength=0)
        {
            if (String.IsNullOrWhiteSpace(strHtml))
            {
                return string.Empty;
            }
            else
            {
                string[] aryReg ={ 
                @"<script[^>]*?>.*?</script>", 
                @"<iframe[^>]*?>.*?</iframe>", 
                @"<!--.*\n(-->)?", 
                @"<(\/\s*)?(.|\n)*?(\/\s*)?>", 
                @"<(\w|\s|""|'| |=|\\|\.|\/|#)*", 
                @"([\r\n|\s])*", 
                @"&(quot|#34);", 
                @"&(amp|#38);", 
                @"&(lt|#60);", 
                @"&(gt|#62);", 
                @"&(nbsp|#160);", 
                @"&(iexcl|#161);", 
                @"&(cent|#162);", 
                @"&(pound|#163);", 
                @"&(copy|#169);",
                @"%(27|32|3E|3C|3D|3F)",  
                @"&#(\d+);"};

                string newReg = aryReg[0];
                string strOutput = strHtml.Replace("&nbsp;", " ");
                for (int i = 0; i < aryReg.Length; i++)
                {
                    Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                    strOutput = regex.Replace(strOutput, string.Empty);
                }
                if (!isReservationSimpleTag)
                {
                    strOutput = strOutput.Replace("<", "&gt;").Replace(">", "&lt;").Replace("\r", string.Empty).Replace("\n", string.Empty);
                }
                if (maxLength > 0 && strOutput.Length > maxLength)
                    strOutput = strOutput.Substring(0, maxLength);
                return strOutput.Replace(" ", "&nbsp;");
            }
        }
        
        public static bool IsContainsHtmlFilter(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return false;
            bool isContains = false;
            str = str.Trim();
            string[] aryReg ={ 
                @"<script[^>]*?>.*?</script>", 
                @"<iframe[^>]*?>.*?</iframe>", 
                @"<!--.*\n(-->)?", 
                @"<(\/\s*)?(.|\n)*?(\/\s*)?>", 
                @"<(\w|\s|""|'| |=|\\|\.|\/|#)*", 
                //@"([\r\n|\s])*", 
                @"&(quot|#34);", 
                @"&(amp|#38);", 
                @"&(lt|#60);", 
                @"&(gt|#62);", 
                //@"&(nbsp|#160);", 
                @"&(iexcl|#161);", 
                @"&(cent|#162);", 
                @"&(pound|#163);", 
                @"&(copy|#169);", 
                @"%(27|32|3E|3C|3D|3F)",
                @"&#(\d+);"};

            string newReg = aryReg[0];
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                if (regex.IsMatch(str))
                {
                    isContains = true;
                    break;
                }
            }
            return isContains;
        }

        /// <summary>
        /// 字符串清理 用于html显示 清理掉html标签四个关键字",<,>,'
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="maxLength">按照字符串长度截取</param>
        /// <returns></returns>
        public static string InputTextNoHTML(this string inputString, int maxLength)
        {
            StringBuilder retVal = new StringBuilder();

            // 检查是否为空
            if ((inputString != null) && (inputString != String.Empty))
            {
                inputString = inputString.Trim();

                //检查长度
                if (inputString.Length > maxLength)
                    inputString = inputString.Substring(0, maxLength);

                //替换危险字符
                for (int i = 0; i < inputString.Length; i++)
                {
                    switch (inputString[i])
                    {
                        case '"':
                            retVal.Append("&quot;");
                            break;
                        case '<':
                            retVal.Append("&lt;");
                            break;
                        case '>':
                            retVal.Append("&gt;");
                            break;
                        default:
                            retVal.Append(inputString[i]);
                            break;
                    }
                }
                retVal.Replace("'", " ");// 替换单引号
            }
            return retVal.ToString();

        }
        #endregion

        #region 字符串Encode/Decode
        /// <summary>
        /// 转换成 HTML code
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Encode(this string str)
        {
            str = str.Replace("&", "&amp;");
            str = str.Replace("'", "''");
            str = str.Replace("\"", "&quot;");
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\n", "<br>");
            return str;
        }
        /// <summary>
        ///解析html成 普通文本
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string Decode(this string str)
        {
            str = str.Replace("<br>", "\n");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&quot;", "\"");
            return str;
        }

        /// <summary>
        /// 字符串编码
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static string HtmlEncode(this string inputData)
        {
            return System.Web.HttpUtility.HtmlEncode(inputData);
        }

        public static string HtmlDecode(this string inputData)
        {
            return System.Web.HttpUtility.HtmlDecode(inputData);
        }

        public static string UrlEncode(this string inputData)
        {
            return System.Web.HttpUtility.UrlEncode(inputData);
        }
        public static string UrlDecode(this string inputData)
        {
            return System.Web.HttpUtility.UrlDecode(inputData);
        }
        #endregion

        /// <summary>
        /// Method to make sure that user's inputs are not malicious
        /// </summary>
        /// <param name="text">User's Input</param>
        /// <param name="maxLength">Maximum length of input</param>
        /// <returns>The cleaned up version of the input</returns>
        public static string FilterInputText(this string text, int maxLength)
        {

            if (string.IsNullOrEmpty(text))
                return string.Empty;
            text = text.Trim();
            if (text.Length > maxLength)
                text = text.Substring(0, maxLength);
            text = Regex.Replace(text, "[\\s]{2,}", " ");	//two or more spaces
            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n");	//<br>
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " ");	//&nbsp;
            text = Regex.Replace(text, "<(.|\\n)*?>", string.Empty);	//any other tags
            text = Regex.Replace(text, "[？]", "?");    //全角？
            text = Regex.Replace(text, "[×]", "*");     //全角*
            text = text.Replace("'", "''");
            return text;
        }

        //过滤掉Kbase中不可见的特殊字符，包括换行符
        //过滤函数
        public static string FilterKbaseCharacter(this string inputStr)
        {
            if (string.IsNullOrEmpty(inputStr))
            {
                return "";
            }
            string cnAbstractFormat = inputStr.Replace("<", "");
            cnAbstractFormat = cnAbstractFormat.Replace("  ", " ");
            cnAbstractFormat = cnAbstractFormat.Replace(">", "");
            cnAbstractFormat = cnAbstractFormat.Replace("\0", "");//过滤掉kbase里的特有的符号，\0不可见
            cnAbstractFormat = cnAbstractFormat.Replace("\n", "");
            cnAbstractFormat = cnAbstractFormat.Replace("\r", "");
            cnAbstractFormat = cnAbstractFormat.Replace("0x07", "");
            cnAbstractFormat = cnAbstractFormat.Replace("0x01", "");
            cnAbstractFormat = cnAbstractFormat.Replace('\t', ' ');
            cnAbstractFormat = cnAbstractFormat.Replace(@"<正>", "");
            cnAbstractFormat = cnAbstractFormat.Replace('"', ' ');
            cnAbstractFormat = cnAbstractFormat.Replace('\'', ' ');
            cnAbstractFormat = cnAbstractFormat.Replace("$$", "");
            //cnAbstractFormat = Regex.Replace(cnAbstractFormat, @"\s", "");//替换掉所有的空格
            string[] noSeeChar = { "\u0000", "\u0001", "\u0002", "\u0003", "\u0004", "\u0005", "\u0006", "\u0007", "\u0008", "\u0009", "\u000A", "\u000B", "\u000C", "\u000D", "\u000E", "\u000F", "\u0010", "\u0011", "\u0012", "\u0013", "\u0014", "\u0015", "\u0016", "\u0017", "\u0018", "\u0019", "\u001A", "\u001B", "\u001C", "\u001D", "\u001E", "\u001F" };
            foreach (string str in noSeeChar)
            {
                //该语句替换的是不可见的ASCII码值十进制为00-31的。
                cnAbstractFormat = cnAbstractFormat.Replace(str, "");
            }

            //cnAbstractFormat = Regex.Replace(cnAbstractFormat, @"\u0020", ""); //32，空格
            cnAbstractFormat = Regex.Replace(cnAbstractFormat, @"\u007f", ""); //127，删除
            cnAbstractFormat = Regex.Replace(cnAbstractFormat, @"\u003c", ""); //60，<
            cnAbstractFormat = Regex.Replace(cnAbstractFormat, @"\u003e", ""); //62，>    
            return cnAbstractFormat;
        }

        /// <summary>
        /// 进行正则替换
        /// </summary>
        /// <param name="input"></param>
        /// <param name="patternt"></param>
        /// <param name="replacement"></param>
        /// <returns></returns>
        public static string Replace(this string input, string patternt, string replacement)
        {
            return Regex.Replace(input, patternt, replacement);
        }

        /// <summary>
        /// 进行正则分隔为数组
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static string[] Split(this string input, string pattern, RegexOptions options=RegexOptions.None)
        {
            return Regex.Split(input, pattern, RegexOptions.IgnoreCase);
        }


        /// <summary>
        /// 计算字符串的字符长度,一个汉字算两个字符
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int GetCount(this string input)
        {
            return Regex.Replace(input, @"[\u4e00-\u9fa5/g]", "aa").Length;
        }


        /// <summary>
        /// 判断compare在input字符串中出现的次数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="compare"></param>
        /// <returns></returns>
        public static int GetSubStringCount(this string input, string compare)
        {
            int index = input.IndexOf(compare);
            if (index != -1)
            {
                return 1 + GetSubStringCount(input.Substring(index + compare.Length), compare);
            }
            else
            {
                return 0;
            }
        }

        public static bool IsMatch(this string input, string pattern)
        {
            if (input == null || input == "") return false;
            Regex regex = new Regex(pattern);
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// 替换字符串中间的字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="iStart"></param>
        /// <param name="iEnd"></param>
        /// <param name="cReplaceTag"></param>
        /// <returns></returns>
        public static string ReplaceMidStr(this string input, int iStart, int iEnd, char cReplaceTag='*')
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;
            int iLen = input.Length;
            if (iStart <= 1)
                iStart = 1;
            if (iEnd >= iLen)
                iEnd = iLen;
            if(iStart > iEnd)
            {
                throw new Exception("iStart must less than iEnd");
            }

            //12365478912, 3-3, 
            if(iStart == iEnd)
            {
                if(iStart == 1)
                    return cReplaceTag.ToString() + input.Substring(1);
                else if(iEnd == iLen)
                    return input.Substring(0,iLen-1) + cReplaceTag.ToString();
                else
                    return input.Substring(0, iStart-1) + cReplaceTag.ToString() + input.Substring(iEnd);
            }
            else
            {
                if(iStart == 1 && iEnd == iLen)
                    return string.Empty.PadRight(iLen, cReplaceTag);
                else if(iStart == 1 && iEnd < iLen)
                    return input.Substring(iEnd).PadLeft(iLen, cReplaceTag);
                else if(iStart > 1 && iEnd == iLen)
                    return input.Substring(0, iStart-1).PadRight(iLen, cReplaceTag);
                else
                    return input.Substring(0, iStart-1) + string.Empty.PadRight(iEnd-iStart+1, cReplaceTag) + input.Substring(iEnd);
            }
        }

        /// <summary>
        /// 转换长数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ChangeLongNumber(this long input, int f = 2)
        {
            long tmpcount = input.ToLong(0);
            string strCorpusReadCount = string.Format("{0}", tmpcount);
            if (tmpcount > 10000 && tmpcount < 100000000)
            {
                switch (f)
                {
                    case 0:
                        strCorpusReadCount = string.Format("{0}万", (tmpcount / 10000));
                        break;
                    case 1:
                        strCorpusReadCount = string.Format("{0:0.0}万", (tmpcount / 10000f));
                        break;
                    case 2:
                    default:
                        strCorpusReadCount = string.Format("{0:F}万", (tmpcount / 10000f));
                        break;
                }


            }
            else if (tmpcount > 100000000)
            {
                switch (f)
                {
                    case 0:
                        strCorpusReadCount = string.Format("{0}亿", (tmpcount / 100000000));
                        break;
                    case 1:
                        strCorpusReadCount = string.Format("{0:0.0}亿", (tmpcount / 100000000f));
                        break;
                    case 2:
                    default:
                        strCorpusReadCount = string.Format("{0:F}亿", (tmpcount / 100000000f));
                        break;
                }

            }
            return strCorpusReadCount;
        }

        /// <summary>
        /// 转换int数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ChangeLongNumber(this int input, int f = 2)
        {
            int tmpcount = input.ToInt(0);
            string strCorpusReadCount = string.Format("{0}", tmpcount);
            if (tmpcount > 10000 && tmpcount < 100000000)
            {
                switch (f)
                {
                    case 0:
                        strCorpusReadCount = string.Format("{0}万", (tmpcount / 10000));
                        break;
                    case 1:
                        strCorpusReadCount = string.Format("{0:0.0}万", (tmpcount / 10000f));
                        break;
                    case 2:
                    default:
                        strCorpusReadCount = string.Format("{0:F}万", (tmpcount / 10000f));
                        break;
                }


            }
            else if (tmpcount > 100000000)
            {
                switch (f)
                {
                    case 0:
                        strCorpusReadCount = string.Format("{0}亿", (tmpcount / 100000000));
                        break;
                    case 1:
                        strCorpusReadCount = string.Format("{0:0.0}亿", (tmpcount / 100000000f));
                        break;
                    case 2:
                    default:
                        strCorpusReadCount = string.Format("{0:F}亿", (tmpcount / 100000000f));
                        break;
                }

            }
            return strCorpusReadCount;
        }
          
        /// <summary>
        /// 断词检索 替换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceBreakWord(this string str)
        {
            if (str == null || str.Length == 0)
                return string.Empty;
            if (str.Trim().IndexOf(" 或者 ") > 0)
            {
                str = str.Replace(" 或者 ", " ");
            }
            return str;
        }
    }
}
