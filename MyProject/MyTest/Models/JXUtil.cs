using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JXUtil
{
    public class BarCode
    {
        private Hashtable _Code39 = new Hashtable();

        /// <summary>
        /// 内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 放大倍数
        /// </summary>
        public byte Magnify { get; set; }

        /// <summary>
        /// 图形高
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 字体大小
        /// </summary>
        public Font ViewFont { get; set; }

        public BarCode()
        {
            _Code39.Add("A", "1101010010110");
            _Code39.Add("B", "1011010010110");
            _Code39.Add("C", "1101101001010");
            _Code39.Add("D", "1010110010110");
            _Code39.Add("E", "1101011001010");
            _Code39.Add("F", "1011011001010");
            _Code39.Add("G", "1010100110110");
            _Code39.Add("H", "1101010011010");
            _Code39.Add("I", "1011010011010");
            _Code39.Add("J", "1010110011010");
            _Code39.Add("K", "1101010100110");
            _Code39.Add("L", "1011010100110");
            _Code39.Add("M", "1101101010010");
            _Code39.Add("N", "1010110100110");
            _Code39.Add("O", "1101011010010");
            _Code39.Add("P", "1011011010010");
            _Code39.Add("Q", "1010101100110");
            _Code39.Add("R", "1101010110010");
            _Code39.Add("S", "1011010110010");
            _Code39.Add("T", "1010110110010");
            _Code39.Add("U", "1100101010110");
            _Code39.Add("V", "1001101010110");
            _Code39.Add("W", "1100110101010");
            _Code39.Add("X", "1001011010110");
            _Code39.Add("Y", "1100101101010");
            _Code39.Add("Z", "1001101101010");
            _Code39.Add("0", "1010011011010");
            _Code39.Add("1", "1101001010110");
            _Code39.Add("2", "1011001010110");
            _Code39.Add("3", "1101100101010");
            _Code39.Add("4", "1010011010110");
            _Code39.Add("5", "1101001101010");
            _Code39.Add("6", "1011001101010");
            _Code39.Add("7", "1010010110110");
            _Code39.Add("8", "1101001011010");
            _Code39.Add("9", "1011001011010");
            _Code39.Add("+", "1001010010010");
            _Code39.Add("-", "1001010110110");
            _Code39.Add("*", "1001011011010");
            _Code39.Add("/", "1001001010010");
            _Code39.Add("%", "1010010010010");
            _Code39.Add("&", "1001001001010");
            _Code39.Add(".", "1100101011010");
            _Code39.Add(" ", "1001101011010");
        }

        public enum Code39Model
        {
            /// <summary>
            /// 基本类别 1234567890ABC
            /// </summary>
            Code39Normal,
            /// <summary>
            /// 全ASCII方式 +A+B 来表示小写
            /// </summary>
            Code39FullAscII
        }

        /// <summary>
        /// 获得条码图形
        /// </summary>
        /// <param name="text">文字信息</param>
        /// <param name="model">类别</param>
        /// <param name="stat">是否增加前后*号</param>
        /// <returns>图形</returns>
        public Bitmap GetCodeImage(Code39Model model, bool star)
        {
            string textVal = "";
            string textCode = "";
            char[] charVal = null;
            switch (model)
            {
                case Code39Model.Code39Normal:
                    textVal = Text.ToUpper();
                    break;
                default:
                    charVal = Text.ToCharArray();
                    for (int i = 0; i != charVal.Length; i++)
                    {
                        if ((int)charVal[i] >= 97 && (int)charVal[i] <= 122)
                        {
                            textVal += "+" + charVal[i].ToString().ToUpper();

                        }
                        else
                        {
                            textVal += charVal[i].ToString();
                        }
                    }
                    break;
            }
            charVal = textVal.ToCharArray();
            if (star == true) textCode += _Code39["*"];
            for (int i = 0; i != charVal.Length; i++)
            {
                if (star == true && charVal[i] == '*') throw new Exception("带有起始符号不能出现*");
                object _CharCode = _Code39[charVal[i].ToString()];
                if (_CharCode == null) throw new Exception("不可用的字符" + charVal[i].ToString());
                textCode += _CharCode.ToString();
            }
            if (star == true) textCode += _Code39["*"];
            Bitmap bmp = GetImage(textCode);
            GetViewImage(bmp, Text);
            return bmp;
        }

        /// <summary>
        /// 绘制编码图形
        /// </summary>
        /// <param name="text">编码</param>
        /// <returns>图形</returns>
        private Bitmap GetImage(string text)
        {
            char[] val = text.ToCharArray();

            //宽 == 需要绘制的数量*放大倍数 + 两个字的宽   
            Bitmap codeImg = new Bitmap(val.Length * ((int)Magnify + 1), (int)Height);
            Graphics graph = Graphics.FromImage(codeImg);

            graph.FillRectangle(Brushes.White, new Rectangle(0, 0, codeImg.Width, codeImg.Height));

            int len = 0;
            for (int i = 0; i != val.Length; i++)
            {
                int width = Magnify + 1;
                if (val[i] == '1')
                {
                    graph.FillRectangle(Brushes.Black, new Rectangle(len, 0, width, Height));

                }
                else
                {
                    graph.FillRectangle(Brushes.White, new Rectangle(len, 0, width, Height));
                }
                len += width;
            }

            graph.Dispose();
            return codeImg;
        }

        /// <summary>
        /// 绘制文字
        /// </summary>
        /// <param name="codeImage">图形</param>
        /// <param name="text">文字</param>
        private void GetViewImage(Bitmap codeImage, string text)
        {
            if (ViewFont == null) return;
            Graphics graphic = Graphics.FromImage(codeImage);
            SizeF fontSize = graphic.MeasureString(text, ViewFont);

            if (fontSize.Width > codeImage.Width || fontSize.Height > codeImage.Height - 20)
            {
                graphic.Dispose();
                return;
            }
            int starHeight = codeImage.Height - (int)fontSize.Height;
            graphic.FillRectangle(Brushes.White, new Rectangle(0, starHeight, codeImage.Width, (int)fontSize.Height));

            int _StarWidth = (codeImage.Width - (int)fontSize.Width) / 2;
            graphic.DrawString(text, ViewFont, Brushes.Black, _StarWidth, starHeight);
            graphic.Dispose();

        }
    }

    public class BarCode128
    {
        //  ASCII从32到127对应的条码区,由3个条、3个空、共11个单元构成,符号内含校验码
        private string[] Code128Encoding = new string[] {
            "11011001100", "11001101100", "11001100110", "10010011000", "10010001100", "10001001100", "10011001000", "10011000100", "10001100100", "11001001000",
            "11001000100", "11000100100", "10110011100", "10011011100", "10011001110", "10111001100", "10011101100", "10011100110", "11001110010", "11001011100",
            "11001001110", "11011100100", "11001110100", "11101101110", "11101001100", "11100101100", "11100100110", "11101100100", "11100110100", "11100110010",
            "11011011000", "11011000110", "11000110110", "10100011000", "10001011000", "10001000110", "10110001000", "10001101000", "10001100010", "11010001000",
            "11000101000", "11000100010", "10110111000", "10110001110", "10001101110", "10111011000", "10111000110", "10001110110", "11101110110", "11010001110",
            "11000101110", "11011101000", "11011100010", "11011101110", "11101011000", "11101000110", "11100010110", "11101101000", "11101100010", "11100011010",
            "11101111010", "11001000010", "11110001010", "10100110000", "10100001100", "10010110000", "10010000110", "10000101100", "10000100110", "10110010000",
            "10110000100", "10011010000", "10011000010", "10000110100", "10000110010", "11000010010", "11001010000", "11110111010", "11000010100", "10001111010",
            "10100111100", "10010111100", "10010011110", "10111100100", "10011110100", "10011110010", "11110100100", "11110010100", "11110010010", "11011011110",
            "11011110110", "11110110110", "10101111000", "10100011110", "10001011110", "10111101000", "10111100010", "11110101000", "11110100010", "10111011110",
            "10111101110", "11101011110", "11110101110", "11010000100", "11010010000", "11010011100"
            };
        private const string Code128Stop = "11000111010", Code128End = "11"; //固定码尾
        private enum Code128ChangeModes { CodeA = 101, CodeB = 100, CodeC = 99 }; //变更
        private enum Code128StartModes { CodeUnset = 0, CodeA = 103, CodeB = 104, CodeC = 105 };//各类编码的码头

        /// <summary>
        /// 绘制Code128码(以像素为单位)
        /// </summary>
        public int EncodeBarcode(string code, System.Drawing.Graphics g, int x, int y, int width, int height, bool showText)
        {
            if (string.IsNullOrEmpty(code)) new Exception("条码不能为空");
            List<int> encoded = CodetoEncoded(code); //1.拆分转义
            encoded.Add(CheckDigitCode128(encoded)); //2.加入校验码
            string encodestring = EncodeString(encoded); //3.编码

            if (showText) //计算文本的大小,字体占图像的1/4高
            {
                Font font = new System.Drawing.Font("宋体", height / 4F, System.Drawing.FontStyle.Regular, GraphicsUnit.Pixel, ((byte)(0)));
                SizeF size = g.MeasureString(code, font);
                height = height - (int)size.Height;

                int _StarWidth = (width - (int)size.Width) / 2;
                g.DrawString(code, font, System.Drawing.Brushes.Black, _StarWidth, height);
                int w = DrawBarCode(g, encodestring, x, y, width, height); //4.绘制
                return ((int)size.Width > w ? (int)size.Width : w);
            }
            else
                return DrawBarCode(g, encodestring, x, y, width, height); //4.绘制
        }

        //1.检测并将字符串拆分并加入码头
        private List<int> CodetoEncoded(string code)
        {
            List<int> encoded = new List<int>();
            int type = 0;//2:B类,3:C类
            for (int i = 0; code.Length > 0; i++)
            {
                int k = isNumber(code);
                if (k >= 4) //连续偶个数字可优先使用C类(其实并不定要转C类，但能用C类时条码会更短)
                {
                    if (type == 0) encoded.Add((int)Code128StartModes.CodeC); //加入码头
                    else if (type != 3) encoded.Add((int)(Code128ChangeModes.CodeC)); //转义
                    type = 3;
                    for (int j = 0; j < k; j = j + 2) //两位数字合为一个码身
                    {
                        encoded.Add(Int32.Parse(code.Substring(0, 2)));
                        code = code.Substring(2);
                    }
                }
                else
                {
                    if ((int)code[0] < 32 || (int)code[0] > 126) throw new Exception("字符串必须是数字或字母");
                    if (type == 0) encoded.Add((int)Code128StartModes.CodeB); //加入码头
                    else if (type != 2) encoded.Add((int)(Code128ChangeModes.CodeB)); //转义
                    type = 2;
                    encoded.Add((int)code[0] - 32);//字符串转为ASCII-32
                    code = code.Substring(1);
                }
            }
            return encoded;
        }
        //2.校验码
        private int CheckDigitCode128(List<int> encoded)
        {
            int check = encoded[0];
            for (int i = 1; i < encoded.Count; i++)
                check = check + (encoded[i] * i);
            return (check % 103);
        }

        //2.编码(对应Code128Encoding数组)
        private string EncodeString(List<int> encoded)
        {
            string encodedString = "";
            for (int i = 0; i < encoded.Count; i++)
            {
                encodedString += Code128Encoding[encoded[i]];
            }
            encodedString += Code128Stop + Code128End; // 加入结束码
            return encodedString;
        }

        //4.绘制条码(返回实际图像宽度)
        private int DrawBarCode(System.Drawing.Graphics g, string encodeString, int x, int y, int width, int height)
        {
            int w = width / encodeString.Length;
            for (int i = 0; i < encodeString.Length; i++)
            {
                g.FillRectangle(encodeString[i] == '0' ? System.Drawing.Brushes.White : System.Drawing.Brushes.Black, x, y, w, height);
                x += w;
            }
            return w * (encodeString.Length + 2);
        }
        //检测是否连续偶个数字,返回连续数字的长度
        private int isNumber(string code)
        {
            int k = 0;
            for (int i = 0; i < code.Length; i++)
            {
                if (char.IsNumber(code[i]))
                    k++;
                else
                    break;
            }
            if (k % 2 != 0) k--;
            return k;
        }

        /// <summary>
        /// 绘制Code128码到图片
        /// </summary>
        public Image EncodeBarcode(string code, int width, int height, bool showText)
        {
            Bitmap image = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(image))
            {
                g.Clear(Color.White);
                int w = EncodeBarcode(code, g, 0, 0, width, height, showText);

                Bitmap image2 = new Bitmap(w, height); //剪切多余的空白;
                using (Graphics g2 = Graphics.FromImage(image2))
                {
                    g2.DrawImage(image, 0, 0);
                    return image2;
                }

            }

        }

        /// <summary>
        /// 绘制Code128码到流
        /// </summary>
        public byte[] EncodeBarcodeByte(string code, int width, int height, bool showText)
        {
            Image image = EncodeBarcode(code, width, height, showText);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            byte[] byteImage = ms.ToArray();
            ms.Close();
            image.Dispose();
            return byteImage;

        }
    }
}