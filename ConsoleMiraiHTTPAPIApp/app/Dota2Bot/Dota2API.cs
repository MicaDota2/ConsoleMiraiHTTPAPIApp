using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace ConsoleMiraiHTTPAPIApp.app.Dota2Bot
{
    internal class Dota2API
    {
        // 这里替换成你自己的API_KEY
        static string API_KEY = GlobalConfig.steamAPIKey;

        /// <summary>
        /// 短steamID转成长steamID
        /// </summary>
        /// <param name="shortSteamID"></param>
        /// <returns></returns>
        static public long ShortSteamIDToLongSteamID(long shortSteamID)
        {
            long longSteamID = shortSteamID + 76561197960265728;
            return longSteamID;
        }

        /// <summary>
        /// 长steamID转成短steamID
        /// </summary>
        /// <param name="longSteamID"></param>
        /// <returns></returns>
        static public long LongSteamIDToShortSteamID(long longSteamID)
        {
            long shortSteamID = longSteamID - 76561197960265728;
            return shortSteamID;
        }

        /// <summary>
        /// 用于进行http代理
        /// </summary>
        /// <returns></returns>
        static public HttpClientHandler GetClientHandler()
        {
            WebProxy proxy = new WebProxy
            {
                // http代理信息，如果需要开代理，记得改这里的配置，再去掉下面的注释
                Address = new Uri("http://127.0.0.1:58591"),
                BypassProxyOnLocal = true,
                UseDefaultCredentials = true
            };

            HttpClientHandler handler = new HttpClientHandler
            {
                // 因为steam的部分服务在国内访问比较有问题
                // 需要开启代理的话要取消注释掉这行
                // Proxy = proxy
            };

            return handler;
        }

        /// <summary>
        /// 获取的是UTC时间
        /// </summary>
        /// <param name="seconds"></param>
        /// <returns></returns>
        static public DateTime GetDateTimeBySeconds(long seconds)
        {
            return DateTime.Parse("1970-01-01 00:00:00").AddSeconds(seconds);
        }

        /// <summary>
        /// 通过dateime获取秒数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        static public long GetSecondsByDateTime(DateTime dateTime)
        {
            return (long)(dateTime - DateTime.Parse("1970-01-01 00:00:00")).TotalSeconds;
        }

        // 当查询的玩家数只有一名时，用这个筛选方法
        static public Dota2Json.GetMatches.Player? GetPlayerFromMatchPlayers(Dota2Json.GetMatches.Player[] players, long shortSteamID)
        {
            foreach (Dota2Json.GetMatches.Player player in players)
            {
                if (player.account_id == shortSteamID)
                {
                    return player;
                }
            }
            return null;
        }

        /// <summary>
        /// 当查询的玩家数只有一名时，用这个筛选方法
        /// </summary>
        /// <param name="players"></param>
        /// <param name="shortSteamID"></param>
        /// <returns></returns>
        static public Dota2Json.MatchDetailInfo.Player? GetPlayerFromMatchDetailInfoPlayer(Dota2Json.MatchDetailInfo.Player[] players, long shortSteamID)
        {
            foreach (Dota2Json.MatchDetailInfo.Player player in players)
            {
                if (player.account_id == shortSteamID)
                {
                    return player;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据steam短ID，获取该玩家最近一场的的比赛信息
        /// </summary>
        /// <param name="shortSteamID"></param>
        /// <returns></returns>
        static public async Task<string?> GetLastMatchIDByShortSteamID(long shortSteamID)
        {
            string url = string.Format("https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/v001/?key={0}&account_id={1}&matches_requested=1",
                                        API_KEY, shortSteamID);
            try
            {
                HttpClientHandler handler = GetClientHandler();
                using (HttpClient client = new HttpClient(handler: handler))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    client.Timeout = TimeSpan.FromSeconds(30.0);

                    Console.WriteLine(url);
                    var message = await client.GetAsync(url);
                    //if (message.IsSuccessStatusCode)
                    //{
                    //    var str = await message.Content.ReadAsStringAsync();
                    //    Console.WriteLine(str);
                    //}                    
                    Dota2Json.GetMatches.HttpResult? httpResult = await client.GetFromJsonAsync<Dota2Json.GetMatches.HttpResult>(url);

                    //2022年9月4日红牛加的，为了提示报错的状态码信息，不知道对不对
                    if ((int)message.StatusCode >= 400)
                    {
                        if ((int)message.StatusCode == 401)
                        {
                            Console.WriteLine("Unauthorized request 401. 请确认APIkey是否正确");
                        }
                        else if ((int)message.StatusCode == 503)
                        {
                            Console.WriteLine("服务器繁忙或者超出了API频次的限制，请稍等30秒以上后再尝试");
                        }
                        else
                        {
                            Console.WriteLine("接受数据失败StatusCode:" + message.StatusCode + "\t URL:" + url);
                        }
                    }

                    if (httpResult.HasValue)
                    {
                        return httpResult.Value.result.matches[0].match_id.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Short Steam ID: " + shortSteamID + "获取最近的比赛ID失败了，请确认是否已经公开了战绩。以下是报错信息");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException?.Message);
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        /// <summary>
        /// 用于获取最近的数场比赛
        /// </summary>
        /// <param name="shortSteamID"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        static public async Task<List<Dota2Json.GetMatches.Match>> GetMatchesByShortSteamID(long shortSteamID, int count = 10)
        {
            string url = string.Format("https://api.steampowered.com/IDOTA2Match_570/GetMatchHistory/v001/?key={0}&account_id={1}&matches_requested={2}",
                                        API_KEY, shortSteamID, count);

            try
            {
                HttpClientHandler handler = GetClientHandler();
                using (HttpClient client = new HttpClient(handler: handler))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    client.Timeout = TimeSpan.FromSeconds(30.0);

                    Console.WriteLine(url);
                    var message = await client.GetAsync(url);
                    //if (message.IsSuccessStatusCode)
                    //{
                    //    var str = await message.Content.ReadAsStringAsync();
                    //    Console.WriteLine(str);
                    //}
                    Dota2Json.GetMatches.HttpResult? httpResult = await client.GetFromJsonAsync<Dota2Json.GetMatches.HttpResult>(url);
                    if (httpResult.HasValue)
                    {
                        if (httpResult.Value.result.matches != null)
                        { return httpResult.Value.result.matches.ToList(); }
                        else
                        { return new List<Dota2Json.GetMatches.Match>(); }
                    }
                    else
                    {
                        return new List<Dota2Json.GetMatches.Match>();
                    }
                }

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("Short Steam ID: " + shortSteamID + "获取最近的" + count + "场比赛失败了，请确认是否已经公开了战绩。以下是报错信息");
                Console.WriteLine("HttpRequestException thrown at: {0}", e.Source);
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException?.Message);
                return new List<Dota2Json.GetMatches.Match>();
            }
        }

        /// <summary>
        /// 仅用于获取某场比赛的详细信息，只有这样才能知道获胜方
        /// </summary>
        /// <param name="matchID"></param>
        /// <returns></returns>
        static public async Task<Dota2Json.MatchDetailInfo.Result?> GetMatchDetailInfo(long matchID)
        {
            string url = string.Format("https://api.steampowered.com/IDOTA2Match_570/GetMatchDetails/V001/?key={0}&match_id={1}",
                                        API_KEY, matchID);

            try
            {
                HttpClientHandler handler = GetClientHandler();
                using (HttpClient client = new HttpClient(handler: handler))
                {
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    client.Timeout = TimeSpan.FromSeconds(30.0);

                    Console.WriteLine(url);
                    var message = await client.GetAsync(url);
                    //if (message.IsSuccessStatusCode)
                    //{
                    //    var str = await message.Content.ReadAsStringAsync();
                    //    Console.WriteLine(str);
                    //}
                    Dota2Json.MatchDetailInfo.HttpResult? httpResult = await client.GetFromJsonAsync<Dota2Json.MatchDetailInfo.HttpResult>(url);

                    //2022年9月4日红牛加的，为了提示报错的状态码信息，不知道对不对
                    if ((int)message.StatusCode >= 400)
                    {
                        if ((int)message.StatusCode == 401)
                        {
                            Console.WriteLine("Unauthorized request 401. 请确认APIkey是否正确");
                        }
                        else if ((int)message.StatusCode == 503)
                        {
                            Console.WriteLine("服务器繁忙或者超出了API频次的限制，请稍等30秒以上后再尝试");
                        }
                        else
                        {
                            Console.WriteLine("接受数据失败StatusCode:" + message.StatusCode + "\t URL:" + url);
                        }
                    }

                    if (httpResult.HasValue)
                    {
                        Dota2Json.MatchDetailInfo.Result result = httpResult.Value.result;

                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("MatchID: " + matchID + "获取比赛详细信息失败了。以下是报错信息");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.InnerException?.Message);
                Console.WriteLine(e.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 对某位dota2玩家生成战报
        /// </summary>
        /// <param name="player"></param>
        /// <param name="hero"></param>
        /// <returns></returns>
        static public string GetPerPlayerReport(Dota2Json.MatchDetailInfo.Player player, out Dota2Enums.Hero hero)
        {
            //hero = Dota2Enums.GetHeroByID(player.hero_id);
            hero = (Dota2Enums.Hero)player.hero_id;

            int kills = player.kills;
            int deaths = player.deaths;
            int assits = player.assists;
            float kda = (kills + assits) / MathF.Max(1.0f, deaths);
            Dota2Enums.Team team = Dota2Enums.GetTeamBySlot(player.player_slot);
            int lastHit = player.last_hits;
            int heroDamage = player.hero_damage;
            int towerDamage = player.tower_damage;
            int gpm = player.gold_per_min;
            int xpm = player.xp_per_min;
            return "造成了" + heroDamage + "点伤害，"
                    // + "击杀了" + kills + "名敌方英雄，"
                    + "KDA为" + string.Format("{0:f2}", kda) + "，"
                    + "GPM为" + gpm;
        }
    }
}
