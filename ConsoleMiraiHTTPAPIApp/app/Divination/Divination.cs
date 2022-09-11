using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiraiHTTPAPIApp.app.Divination
{
    internal class Divination
    {
        #region 不用的代码
        //public static readonly List<string> listName = new List<string>()
        //{
        //    "看片子",
        //    "组模型",
        //    "投稿情感区",
        //    "逛匿名版",
        //    "和女神聊天",
        //    "啪啪啪",
        //    "熬夜",
        //    "锻炼",
        //    "散步",
        //    "打排位赛",
        //    "汇报工作",
        //    "抚摸猫咪",
        //    "遛狗",
        //    "烹饪",
        //    "告白",
        //    "求站内信",
        //    "追新番",
        //    "打卡日常",
        //    "下副本",
        //    "抢沙发",
        //    "网购",
        //    "跳槽",
        //    "读书",
        //    "早睡",
        //    "逛街"
        //};
        //public static readonly List<string> listGood = new List<string>()
        //{
        //    "释放压力，重铸自我",
        //    "今天的喷漆会很完美",
        //    "问题圆满解决",
        //    "今天也要兵库北",
        //    "女神好感度上升",
        //    "啪啪啪啪啪啪啪",
        //    "夜间的效率更高",
        //    "八分钟给你比利般的身材",
        //    "遇到妹子主动搭讪",
        //    "遇到大腿上分500",
        //    "被夸奖工作认真",
        //    "才不是特意蹭你的呢",
        //    "遇见女神遛狗搭讪",
        //    "黑暗料理界就由我来打败",
        //    "其实我也喜欢你好久了",
        //    "最新种子入手",
        //    "完结之前我绝不会死",
        //    "怒回首页",
        //    "配合默契一次通过",
        //    "沙发入手弹无虚发",
        //    "商品大减价",
        //    "新工作待遇大幅提升",
        //    "知识就是力量",
        //    "早睡早起方能养生",
        //    "物美价廉大优惠"
        //};
        //public static readonly List<string> listBad = new List<string>()
        //{
        //    "会被家人撞到",
        //    "精神不集中板件被剪断了",
        //    "会被当事人发现",
        //    "看到丧尸在晒妹",
        //    "我去洗澡了，呵呵",
        //    "会卡在里面",
        //    "明天有很重要的事",
        //    "会拉伤肌肉",
        //    "走路会踩到水坑",
        //    "我方三人挂机",
        //    "上班偷玩游戏被扣工资",
        //    "死开！愚蠢的人类",
        //    "狗狗随地大小便被罚款",
        //    "难道这就是……仰望星空派？",
        //    "对不起，你是一个好人",
        //    "收到有码葫芦娃",
        //    "会被剧透",
        //    "会被老板发现",
        //    "会被灭到散团",
        //    "会被挂起来羞耻play",
        //    "问题产品需要退换",
        //    "再忍一忍就加薪了",
        //    "注意力完全无法集中",
        //    "会在半夜醒来，然后失眠",
        //    "会遇到奸商"
        //};
        #endregion

        //复刻了10年前acfun用户中心里的每日黄历，包括文案都没改，当时的js代码在这里，https://github.com/Pinkuburu/qqrobot/blob/master/rili.php
        public static string FuntionMain(long senderQQ)
        {
            var fullYear = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;
            int seed = fullYear * 37621 + (month + 1) * 539 + day;

            
            var sg = rnd(seed, 8) % 100;
            var sb = rnd(seed, 4) % 100;
            var l = rnd(seed, 9) % 3 + 2;
            string sgstr = SomethingGood(sg, seed, l);
            l = rnd(seed, 7) % 3 + 2;
            string sbstr = SomethingBad(sb, seed, l);
            string lkstr = Lucky(seed, senderQQ);
            string a = string.Format("{0}\r\n\r\n宜：\r\n{1}\r\n不宜：\r\n{2}", lkstr,sgstr, sbstr);

            return a;
        }

        /// <summary>
        /// 产生一个随机数
        /// </summary>
        /// <param name="a">传入用户的QQ</param>
        /// <param name="b"></param>
        /// <returns>随机数的结果</returns>
        public static long rnd(long a, int b)
        {
            var n = a % 11117;
            for (int i = 0; i < 25 + b; i++)
            {
                n = n * n;
                n = n % 11117;
            }
            return n;
        }

        /// <summary>
        /// 从list里生成适宜事项
        /// </summary>
        /// <param name="sg"></param>
        /// <param name="seed"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static string SomethingGood(long sg, int seed, long l)
        {
            List<string> listName = new List<string>()
        {
            "看片子",
            "组模型",
            "投稿情感区",
            "逛匿名版",
            "和女神聊天",
            "啪啪啪",
            "熬夜",
            "锻炼",
            "散步",
            "打排位赛",
            "汇报工作",
            "抚摸猫咪",
            "遛狗",
            "烹饪",
            "告白",
            "求站内信",
            "追新番",
            "打卡日常",
            "下副本",
            "抢沙发",
            "网购",
            "跳槽",
            "读书",
            "早睡",
            "逛街"
        };
            List<string> listGood = new List<string>()
        {
            "释放压力，重铸自我",
            "今天的喷漆会很完美",
            "问题圆满解决",
            "今天也要兵库北",
            "女神好感度上升",
            "啪啪啪啪啪啪啪",
            "夜间的效率更高",
            "八分钟给你比利般的身材",
            "遇到妹子主动搭讪",
            "遇到大腿上分500",
            "被夸奖工作认真",
            "才不是特意蹭你的呢",
            "遇见女神遛狗搭讪",
            "黑暗料理界就由我来打败",
            "其实我也喜欢你好久了",
            "最新种子入手",
            "完结之前我绝不会死",
            "怒回首页",
            "配合默契一次通过",
            "沙发入手弹无虚发",
            "商品大减价",
            "新工作待遇大幅提升",
            "知识就是力量",
            "早睡早起方能养生",
            "物美价廉大优惠"
        };

            string test = "";
            for (int i = 0; i < l; i++)
            {
                var n = Convert.ToInt32(sg * listName.Count * 0.01);
                var a = "今天适宜：" + listName[n] + "：" + listGood[n];
                test += a + "\r\n";
                listName.RemoveAt(n);
                listGood.RemoveAt(n);
            }
            return test;
        }

        /// <summary>
        /// 从list里生成不宜事项
        /// </summary>
        /// <param name="sg"></param>
        /// <param name="seed"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public static string SomethingBad(long sb, int seed, long l)
        {
            List<string> listName = new List<string>()
        {
            "看片子",
            "组模型",
            "投稿情感区",
            "逛匿名版",
            "和女神聊天",
            "啪啪啪",
            "熬夜",
            "锻炼",
            "散步",
            "打排位赛",
            "汇报工作",
            "抚摸猫咪",
            "遛狗",
            "烹饪",
            "告白",
            "求站内信",
            "追新番",
            "打卡日常",
            "下副本",
            "抢沙发",
            "网购",
            "跳槽",
            "读书",
            "早睡",
            "逛街"
        };
            List<string> listBad = new List<string>()
        {
            "会被家人撞到",
            "精神不集中板件被剪断了",
            "会被当事人发现",
            "看到丧尸在晒妹",
            "我去洗澡了，呵呵",
            "会卡在里面",
            "明天有很重要的事",
            "会拉伤肌肉",
            "走路会踩到水坑",
            "我方三人挂机",
            "上班偷玩游戏被扣工资",
            "死开！愚蠢的人类",
            "狗狗随地大小便被罚款",
            "难道这就是……仰望星空派？",
            "对不起，你是一个好人",
            "收到有码葫芦娃",
            "会被剧透",
            "会被老板发现",
            "会被灭到散团",
            "会被挂起来羞耻play",
            "问题产品需要退换",
            "再忍一忍就加薪了",
            "注意力完全无法集中",
            "会在半夜醒来，然后失眠",
            "会遇到奸商"
        };
            string test = "";
            for (int i = 0; i < l; i++)
            {
                var n = Convert.ToInt32(sb * listName.Count * 0.01);
                var a = listName[n] + "：" + listBad[n];
                test += a + "\r\n";
                listName.RemoveAt(n);
                listBad.RemoveAt(n);
            }
            return test;
        }

        /// <summary>
        /// 生成幸运指数
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="senderQQ"></param>
        /// <returns>幸运指数的文字</returns>
        public static string Lucky(int seed,long senderQQ) {
            var n = rnd(seed * senderQQ, 6) % 100;
            var t = "末吉";
            string msg = string.Empty;
            if (n < 5)
            {
                //5%
                t = "大凶";
            }
            else if (n < 20)
            {
                //15%
                t = "凶";
            }
            else if (n < 50)
            {
                //30%
                t = "末吉";
            }
            else if (n < 60)
            {
                //10%
                t = "半吉";
            }
            else if (n < 70)
            {
                //10%
                t = "吉";
            }
            else if (n < 80)
            {
                //10%
                t = "小吉";
            }
            else if (n < 90)
            {
                //10%
                t = "中吉";
            }
            else
            {
                //5%
                t = "大吉";
            }
            msg = "今日运势指数："+ n+"\r\n"+t;
            return msg;
        }
    }
}
