using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleMiraiHTTPAPIApp.app.TRPGDice
{
    internal class TRPG_Dice
    {
        /// <summary>
        /// DND骰子功能的文字处理部分
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string dndroll(string msg)
        {
            int num = 0, surfaces = 0, values = 0, times = 1;
            string result = "";
            string par = msg.Substring(6);
            string[] pararray = Regex.Split(par, "#", RegexOptions.IgnoreCase);
            try
            {
                if (pararray.Length > 1)
                {
                    times = Convert.ToInt32(pararray[0]);
                    if (times > 50)
                    {
                        return " 你投这么多轮，没毛病吧？";
                    }
                }
                pararray = Regex.Split(pararray[pararray.Length - 1], "k", RegexOptions.IgnoreCase);
                if (pararray.Length > 1)
                {
                    values = Convert.ToInt32(pararray[1]);
                }
                pararray = Regex.Split(pararray[0], "d", RegexOptions.IgnoreCase);
                if (pararray.Length > 1)
                {
                    num = Convert.ToInt32(pararray[0]);
                    surfaces = Convert.ToInt32(pararray[1]);
                    if (num > 101 || surfaces > 101)
                    {
                        return "你刚从宛平南路600号逃出来的？";
                    }
                    if (values > num)
                    {
                        return "你要选的最高数字数量大于每轮投掷数，有问题呀兄弟，改一改参数撒";
                    }
                    result = roll(times, num, surfaces, values);
                }
            }
            catch (Exception e)
            {
            }
            return " 你的投掷结果如下\n" + result;
        }
        /// <summary>
        /// 进行roll数字循环
        /// </summary>
        /// <param name="times">次数</param>
        /// <param name="num">骰子数量</param>
        /// <param name="surfaces">骰子面数</param>
        /// <param name="values">从结果里选出最高的数字有几个</param>
        /// <returns>返回组合好的文字结构</returns>
        public static string roll(int times, int num, int surfaces, int values)
        {
            int i, t, sum = 0;
            string resultmsg = "";
            long tick = DateTime.Now.Ticks;
            Random ran = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));
            for (t = 1; t <= times; t++)
            {
                int currentsum = 0;
                List<int> ResultList = new List<int>();

                for (i = 0; i < num; i++)
                {
                    ResultList.Add(ran.Next(1, surfaces + 1));
                }

                int[] ay = ResultList.ToArray();
                string[] result = Array.ConvertAll(ay, _ => _.ToString());
                string s = string.Join(",", result);
                s = "{" + s + "}";
                resultmsg = resultmsg + s;
                //Console.WriteLine(s);
                if (values > 0)
                {
                    ResultList.Sort();
                    ResultList.Reverse();
                    for (i = 0; i < values; i++)
                    {
                        currentsum += ResultList[i];
                    }
                }
                if (currentsum > 0)
                {
                    resultmsg = resultmsg + "依据规则取得最高值相加和为" + currentsum.ToString() + "\n";
                    //Console.WriteLine(currentsum);
                    sum += currentsum;
                }
            }
            if (times > 1)
            {
                resultmsg = resultmsg + "以上结果累计相加 = " + sum.ToString() + "\n";
                //Console.WriteLine(sum);
            }
            return resultmsg;
        }
    }
}
