using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiraiHTTPAPIApp
{
    public class GlobalConfig
    {    
        /// <summary>
        /// QQ机器人的QQ号
        /// </summary>
        static public int qqNumber = 10000;
        /// <summary>
        /// QQ机器人部署的服务器地址，记得更换
        /// </summary>
        static public string qqBotHost = "192.168.1.1";
        /// <summary>
        /// QQ机器人部署的服务器端口，这个端口是HTTP API的接收数据的端口，记得更换
        /// </summary>
        static public int qqBotPort = 1080; // 端口
        /// <summary>
        /// QQ机器人HTTP API的验证密钥，记得更换
        /// </summary>
        static public string qqBotAuthKey = "sdas1dsad"; // 凭据

        #region Dota2监视助手部分
        /// <summary>
        /// steam API KEY，记得更换，申请地址https://steamcommunity.com/dev/
        /// </summary>
        static public string steamAPIKey = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
        #region 弃用的数据库配置部分
        ///// <summary>
        ///// dota2监视小助手用的数据库用户名，记得更换
        ///// </summary>
        //static public string dota2WatcherDBUser = "xxxxxxxxx";
        ///// <summary>
        ///// dota2监视小助手用的密码，记得更换
        ///// </summary>
        //static public string dota2WatcherDBPWD = "xxxxxxxxxxxxxxxxxxxx";
        ///// <summary>
        ///// dota2监视小助手用的数据库所在的服务器地址，记得更换
        ///// </summary>
        //static public string dota2WatcherDBServer = "192.168.1.1";
        ///// <summary>
        ///// dota2监视小助手用的数据库所在服务器的端口，记得更换
        ///// </summary>
        //static public string dota2WatcherDBPort = "3307";
        #endregion
        /// <summary>
        /// dota2监视小助手用的数据库名字，记得更换
        /// </summary>
        static public string dota2WatcherDBName = "dota2watcher";
        /// <summary>
        /// dota2监视小助手汇报的群号，记得更换
        /// </summary>
        static public long dota2WatcherReportToQun = 88888888;
        /// <summary>
        /// dota2监视小助手报喜不报忧模式，记得更换，true，就是报喜不报忧
        /// </summary>
        static public bool dota2WatcherOnlyReportWin = false;
        /// <summary>
        /// dota2监视小助手报监控间隔，数值单位是分钟，根据要监视的量控制下间隔，因为steamAPI是有每日请求上限的
        /// </summary>
        static public int dota2WatcherTimeLag = 2;
        #endregion

    }
}
