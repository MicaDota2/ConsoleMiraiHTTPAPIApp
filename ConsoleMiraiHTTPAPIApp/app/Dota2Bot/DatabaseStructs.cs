using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiraiHTTPAPIApp.app.Dota2Bot
{
    // 这个类用来储存所有要储存到数据库中的数据类型
    internal class DatabaseStructs
    {
        // 储存在playerHelper数据库中的数据类型
        // 实现的接口的函数说明，见ISqliteStruct
        public struct Player : ISqliteStruct
        {
            public long shortSteamID { get; set; }
            public string nickname { get; set; }
            public long lastMatchID { get; set; }
            public long lastUpdateTime { get; set; }

            public string GetTableFormat()
            {
                return "ShortSteamID INTEGER NOT NULL PRIMARY KEY, " +
                        "Nickname TEXT, " +
                        "LastMatchID INTEGER DEFAULT 0, " +
                        "LastUpdateTime INTEGER DEFAULT 0";
            }

            public string GetTableColumns()
            {
                return "ShortSteamID, Nickname, LastMatchID, LastUpdateTime";
            }

            public string GetTableValues()
            {
                return string.Format("{0}, \'{1}\', {2}, {3}",
                        shortSteamID, nickname, lastMatchID, lastUpdateTime);
            }

            public void FromTableValues(SQLiteDataReader reader)
            {
                try
                {
                    shortSteamID = reader.GetInt64(0);
                    nickname = reader.GetString(1);
                    lastMatchID = reader.GetInt64(2);
                    lastUpdateTime = reader.GetInt64(3);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }

            public string GetPropertyName(int propertyID)
            {
                string[] properties = new string[]
                {
                    "ShortSteamID", "NickName", "LastMatchID", "LastUpdateTime"
                };

                return properties[propertyID % 4];
            }

            public string GetPropertyValue(int propertyID)
            {
                string[] properties = new string[]
                {
                    shortSteamID.ToString(), "\'" + nickname + "\'", lastMatchID.ToString(), lastUpdateTime.ToString()
                };

                return properties[propertyID % 4];
            }

            public override string ToString()
            {
                return string.Format("{0}, ShortSteamID为{1}, 最近一场比赛编号为{2}, 更新于{3}。",
                    nickname, shortSteamID, lastMatchID, Dota2API.GetDateTimeBySeconds(lastUpdateTime));
            }

        }

        // 储存在matchHelper和updateHelper数据库中的数据类型
        // 实现的接口的函数说明，见ISqliteStruct
        public struct Match : ISqliteStruct
        {
            public long matchID { get; set; }
            public int gameMode { get; set; }
            public long startTime { get; set; }
            public long duration { get; set; }
            public long playerID { get; set; }
            public string nickname { get; set; }
            public int heroID { get; set; }
            public int kills { get; set; }
            public bool win { get; set; }
            public bool isRadiant { get; set; }
            public string report { get; set; }

            public string GetTableFormat()
            {
                return "MatchID INTEGER NOT NULL, " +
                        "GameMode INTEGER DEFAULT 0, " +
                        "StartTime INTEGER, " +
                        "Duration INTEGER, " +
                        "PlayerID INTEGER NOT NULL, " +
                        "Nickname TEXT, " +
                        "HeroID INTEGER DEFAULT 0, " +
                        "Kills INTEGER DEFAULT 0, " +
                        "Win BOOLEAN DEFAULT TRUE, " +
                        "IsRadiant BOOLEAN DEFAULT TRUE, " + 
                        "Report TEXT";
            }

            public string GetTableColumns()
            {
                return "MatchID, GameMode, StartTime, Duration, PlayerID, Nickname, HeroID, Kills, Win, IsRadiant, Report";
            }

            public string GetTableValues()
            {
                return string.Format("{0}, {1}, {2}, {3}, {4}, \'{5}\', {6}, {7}, {8}, {9}, \'{10}\'",
                        matchID, gameMode, startTime, duration, playerID, nickname, heroID, kills, win, isRadiant, report);
            }

            public string GetPropertyName(int propertyID)
            {
                string[] properties = new string[]
                {
                    "MatchID", "GameMode", "StartTime", "Duration", "PlayerID", "Nickname", "HeroID", "Kills", "Win", "IsRadiant", "Report"
                };

                return properties[propertyID % 11];
            }

            public string GetPropertyValue(int propertyID)
            {
                string[] properties = new string[]
                {
                    matchID.ToString(), gameMode.ToString(), startTime.ToString(), duration.ToString(), playerID.ToString(), 
                    nickname, heroID.ToString(), kills.ToString(), win.ToString().ToUpper(), isRadiant.ToString().ToUpper(), report
                };

                return properties[propertyID % 11];
            }


            public void FromTableValues(SQLiteDataReader reader)
            {
                try
                {
                    matchID = reader.GetInt64(0);
                    gameMode = reader.GetInt32(1);
                    startTime = reader.GetInt64(2);
                    duration = reader.GetInt32(3);
                    playerID = reader.GetInt64(4);
                    nickname = reader.GetString(5);
                    heroID = reader.GetInt32(6);
                    kills = reader.GetInt32(7);
                    win = reader.GetBoolean(8);
                    isRadiant = reader.GetBoolean(9);
                    report = reader.GetString(10);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }

            public override string ToString()
            {
                return string.Format("比赛ID为{0}，游戏模式为{1}，开始时间为{2}，时长为{3}，玩家ID为{4}，玩家昵称为{5}，使用英雄为{6}，击杀数为{7}，结果为{8}，阵营为{9}，报告为{10}。",
                            matchID, Dota2Enums.GetGameMode((Dota2Enums.GameMode)gameMode),
                            Dota2API.GetDateTimeBySeconds(startTime), TimeSpan.FromSeconds(duration).ToString("mm':'ss"),
                            playerID, nickname,
                            Dota2Enums.GetHeroName((Dota2Enums.Hero)heroID)[0],
                            kills,
                            win ? "胜利" : "失败",
                            isRadiant ? "天辉" : "夜魇",
                            report);
            }

        }
    }
}
