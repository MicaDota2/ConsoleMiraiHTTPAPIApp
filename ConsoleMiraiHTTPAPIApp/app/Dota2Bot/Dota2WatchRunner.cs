using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiraiHTTPAPIApp.app.Dota2Bot
{
    // 使用方法
    // 1. 添加一位玩家到数据库中
    // Dota2WatchRunner.AddPlayerState state = await Dota2WatchRunner.AddPlayer("xxx", 123131321);
    // 2. 更新所有玩家的比赛数据并生成该次更新的战报
    // string? perUpdateReport = await Dota2WatchRunner.UpdateAllPlayers();
    internal static class Dota2WatchRunner
    {
        // 所有数据库都记录在这里
        // playerHelper: 用于记录所有的刀刀社玩家
        // matchHelper: 用于记录所有刀刀社玩家的比赛
        // updateHelper: 用于记录当次更新新增的刀刀社玩家的比赛
        static public SqliteHelper<DatabaseStructs.Player> playerHelper =
            new SqliteHelper<DatabaseStructs.Player>(GlobalConfig.dota2WatcherDBName + ".db", "Players");
        static public SqliteHelper<DatabaseStructs.Match> matchHelper =
            new SqliteHelper<DatabaseStructs.Match>(GlobalConfig.dota2WatcherDBName + ".db", "Matches");
        static public SqliteHelper<DatabaseStructs.Match> updateHelper =
            new SqliteHelper<DatabaseStructs.Match>(GlobalConfig.dota2WatcherDBName + ".db", "PerUpdateMatches");
       
        /// <summary>
        /// 用于判断添加新刀刀社玩家是否成功
        /// </summary>
        public enum AddPlayerState
        {
            Failed = -1,
            Added,
            Updated,
            Skipped
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        static public string GetAddPlayerState(AddPlayerState state)
        {
            switch (state)
            {
                case AddPlayerState.Failed:
                    return "失败，很可能是网络的问题。";
                case AddPlayerState.Added:
                    return "添加成功！";
                case AddPlayerState.Updated:
                    return "数据库内已有该玩家，已更新。";
                case AddPlayerState.Skipped:
                    return "数据库内已有该玩家，已忽略。";
                default:
                    return "是没见过的报错呢。";
            }
        }

        /// <summary>
        /// 从已获得的比赛编号中，生成该刀刀社玩家每一场比赛的个人战报
        /// 并且填充用于储存到数据库里的结构体
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="shortSteamID"></param>
        /// <param name="results"></param>
        /// <param name="player"></param>
        /// <param name="matches"></param>
        /// <returns></returns>
        static private bool GetPlayerAndMatch(string nickname, long shortSteamID, Dota2Json.MatchDetailInfo.Result?[] results,
                                                out DatabaseStructs.Player player, out List<DatabaseStructs.Match> matches)
        {
            long lastMatchID = 0L;
            matches = new List<DatabaseStructs.Match>();
            bool hasLastMatch = false;
            foreach (Dota2Json.MatchDetailInfo.Result? result in results)
            {
                if (result.HasValue)
                {
                    DatabaseStructs.Match match = new DatabaseStructs.Match();
                    match.matchID = result.Value.match_id;
                    match.gameMode = result.Value.game_mode;
                    match.startTime = result.Value.start_time;
                    match.duration = result.Value.duration;
                    match.playerID = shortSteamID;
                    match.nickname = nickname;
                    Dota2Json.MatchDetailInfo.Player? dota2Player = Dota2API.GetPlayerFromMatchDetailInfoPlayer(result.Value.players, shortSteamID);
                    if (dota2Player.HasValue)
                    {
                        Dota2Enums.Team team = Dota2Enums.GetTeamBySlot(dota2Player.Value.player_slot);
                        Dota2Enums.Team winTeam = result.Value.radiant_win ? Dota2Enums.Team.Radiant : Dota2Enums.Team.Dire;
                        match.heroID = dota2Player.Value.hero_id;
                        match.kills = dota2Player.Value.kills;
                        match.win = (team == winTeam);
                        match.isRadiant = (team == Dota2Enums.Team.Radiant);
                        string gameReport = Dota2API.GetPerPlayerReport(dota2Player.Value, out Dota2Enums.Hero hero);
                        match.report = nickname + "使用" + Dota2Enums.GetHeroName(hero)[0] + gameReport + "。";
                        matches.Add(match);
                        if (!hasLastMatch)
                        {
                            hasLastMatch = true;
                            lastMatchID = result.Value.match_id;
                        }
                    }
                }
                else
                {
                    matches = new List<DatabaseStructs.Match>();
                    player = new DatabaseStructs.Player();
                    return false;
                }
            }

            player = new DatabaseStructs.Player
            {
                shortSteamID = shortSteamID,
                nickname = nickname,
                lastMatchID = lastMatchID,
                lastUpdateTime = Dota2API.GetSecondsByDateTime(DateTime.UtcNow)
            };
            return true;
        }


        /// <summary>
        /// 用于添加一位刀刀社玩家到playerHelper数据库中
        /// 同时也会添加该玩家的最近的十场比赛到matchHelper数据库中
        /// 同时也会获取这十场比赛的详细信息，生成该刀刀社玩家每一场比赛的个人战报
        /// shouldOverride为true时，即使数据库中有这位玩家，也会进行覆盖
        /// 获取比赛失败时，不会进行任何添加操作
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="shortSteamID"></param>
        /// <param name="shouldOverride"></param>
        /// <returns></returns>
        static public async Task<AddPlayerState> AddPlayer(string nickname, long shortSteamID, bool shouldOverride = false)
        {
            SqliteHelper<DatabaseStructs.Player> playerHelper =
                new SqliteHelper<DatabaseStructs.Player>(GlobalConfig.dota2WatcherDBName + ".db", "Players");
            SqliteHelper<DatabaseStructs.Match> matchHelper =
                new SqliteHelper<DatabaseStructs.Match>(GlobalConfig.dota2WatcherDBName + ".db", "Matches");


            playerHelper.OpenOrCreateSql();
            playerHelper.CreateTableIfNotExists();

            matchHelper.OpenOrCreateSql();
            matchHelper.CreateTableIfNotExists();

            try
            {
                bool exists = playerHelper.CheckIfDataExists(0, shortSteamID.ToString());
                if (exists)
                {
                    if (shouldOverride)
                    {
                        return await UpdatePlayer(nickname, shortSteamID);
                    }
                    else
                    {
                        Console.WriteLine(string.Format("ShortSteamID: {0}已经在表里了。", shortSteamID));
                        return AddPlayerState.Skipped;
                    }
                }

                List<Dota2Json.GetMatches.Match> dota2Matches = await Dota2API.GetMatchesByShortSteamID(shortSteamID, 10);
                if (dota2Matches.Count == 0)
                {
                    Console.WriteLine(string.Format("ShortSteamID: {0}并没有找到比赛，请确认已公开战绩。", shortSteamID));
                    return AddPlayerState.Failed;
                }

                List<Task<Dota2Json.MatchDetailInfo.Result?>> matchTasks = new List<Task<Dota2Json.MatchDetailInfo.Result?>>();
                foreach (Dota2Json.GetMatches.Match dota2Match in dota2Matches)
                {
                    matchTasks.Add(Dota2API.GetMatchDetailInfo(dota2Match.match_id));
                    Thread.Sleep(1000);
                }
                Dota2Json.MatchDetailInfo.Result?[] results = await Task.WhenAll(matchTasks);

                bool succeed = GetPlayerAndMatch(nickname, shortSteamID, results, out DatabaseStructs.Player player, out List<DatabaseStructs.Match> matches);
                if (!succeed)
                {
                    Console.WriteLine(string.Format("ShortSteamID: {0}有比赛未能获取到结果，无法录入数据库。", shortSteamID));
                    return AddPlayerState.Failed;
                }

                matchHelper.InsertData(matches);
                playerHelper.InsertData(player);
                Console.WriteLine(string.Format("向数据库中添加了{0}，ShortSteamID为{1}", nickname, shortSteamID)); ;
                Console.WriteLine(shortSteamID + "添加了" + matches.Count + "场比赛。");
                return AddPlayerState.Added;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return AddPlayerState.Failed;
            }
            finally
            {
                playerHelper.CloseSql();
                matchHelper.CloseSql();
            }
        }

        /// <summary>
        /// 用于更新一位刀刀社玩家的比赛信息
        /// 获取最近的十场比赛，添加尚未记录的比赛
        /// 会获取尚未记录比赛的详细信息，生成该刀刀社玩家每一场比赛的个人战报
        /// 获取比赛失败时，不会进行任何添加操作
        /// </summary>
        /// <param name="nickname"></param>
        /// <param name="shortSteamID"></param>
        /// <returns></returns>
        static public async Task<AddPlayerState> UpdatePlayer(string nickname, long shortSteamID)
        {
            List<Dota2Json.GetMatches.Match> dota2Matches = await Dota2API.GetMatchesByShortSteamID(shortSteamID, 10);
            if (dota2Matches.Count == 0)
            {
                Console.WriteLine(string.Format("ShortSteamID: {0}并没有找到比赛，请确认已公开战绩。", shortSteamID));
                return AddPlayerState.Failed;
            }

            List<DatabaseStructs.Player> playerList = playerHelper.GetData(0, shortSteamID.ToString());
            if(playerList.Count == 0) return AddPlayerState.Failed; // 理论上不会出现这个情况
            DatabaseStructs.Player oldPlayer = playerList[0];
            
            int lastMatchIndex = dota2Matches.FindIndex((Dota2Json.GetMatches.Match match) => match.match_id == oldPlayer.lastMatchID);
            if (lastMatchIndex == 0)
            {
                // 无需更新比赛信息
                Console.WriteLine(string.Format("ShortSteamID: {0}当前所有比赛均已录入数据库，这里只更新了时间。", shortSteamID));
                oldPlayer.lastUpdateTime = Dota2API.GetSecondsByDateTime(DateTime.UtcNow);
                int[] tempPropertyIDs = new int[] { 0, 3 };
                string tempSearchValue = oldPlayer.shortSteamID.ToString();
                playerHelper.UpdateData(oldPlayer, tempPropertyIDs, tempSearchValue);

                return AddPlayerState.Skipped;
            }
            else if (lastMatchIndex >= 1)
            {
                // 剔除已有的比赛信息
                dota2Matches.RemoveRange(lastMatchIndex, dota2Matches.Count - lastMatchIndex);
            }

            List<Task<Dota2Json.MatchDetailInfo.Result?>> matchTasks = new List<Task<Dota2Json.MatchDetailInfo.Result?>>();
            foreach (Dota2Json.GetMatches.Match dota2Match in dota2Matches)
            {
                matchTasks.Add(Dota2API.GetMatchDetailInfo(dota2Match.match_id));
                Thread.Sleep(1000);
            }
            Dota2Json.MatchDetailInfo.Result?[] results = await Task.WhenAll(matchTasks);

            bool succeed = GetPlayerAndMatch(nickname, shortSteamID, results, out DatabaseStructs.Player newPlayer, out List<DatabaseStructs.Match> matches);
            if (!succeed)
            {
                Console.WriteLine(string.Format("ShortSteamID: {0}有比赛未能获取到结果，无法录入数据库。", shortSteamID));
                return AddPlayerState.Failed;
            }

            int[] propertyIDs = new int[] { 0, 2, 3 };
            string searchValue = oldPlayer.shortSteamID.ToString();

            matchHelper.InsertData(matches);
            updateHelper.InsertData(matches);
            playerHelper.UpdateData(newPlayer, propertyIDs, searchValue);

            Console.WriteLine(newPlayer.shortSteamID + "更新了" + matches.Count + "场比赛。");
            return AddPlayerState.Updated;
        }

        /// <summary>
        /// 生成战报的详细程度
        /// </summary>
        public enum DetailedLevel
        {
            Simple = 0,
            Normal,
            Detailed
        }

        /// </summary>
        /// 判断这场比赛哪边有刀刀社玩家
        /// </summary>
        public enum PlayerTeam
        {
            RadiantOnly = -1,
            Both = 0,
            DireOnly = 1
        }

        /// <summary>
        /// 用于生成战报的玩家信息
        /// </summary>
        public struct ReportPlayer
        {
            public long shortSteamID { get; set; }
            public string nickname { get; set; }
            public Dota2Enums.Hero hero { get; set; }
            public int kills { get; set; }
            public string playerReport { get; set; }

            public string GetKillReport()
            {
                string killStr = (kills >= 10) ? string.Format("拿下了{0}个人头", kills) : "";
                return nickname + "使用" + Dota2Enums.GetHeroName(hero)[0] + killStr;
            }
        }

        /// <summary>
        /// 用于生成战报的比赛信息
        /// </summary>
        public struct ReportMatch
        {
            public long matchID { get; set; }
            public Dota2Enums.GameMode gameMode { get; set; }
            public List<ReportPlayer> radiantPlayers { get; set; }
            public List<ReportPlayer> direPlayers { get; set; }
            public PlayerTeam playerTeam { get; set; }
            public long startTime { get; set; }
            public long duration { get; set; }
            public bool radiantWins { get; set; }
            public string matchReport { get; set; }

            /// <summary>
            /// 用于判断刀刀社玩家的阵营，也能判断内战的情况
            /// </summary>
            /// <param name="hasRadiant"></param>
            /// <param name="bothDireAndRadiant"></param>
            /// <returns></returns>
            private string GetTeamReport()
            {                
                int playerTeamInt = 0;
                if (radiantPlayers.Count > 0) playerTeamInt --;
                if (direPlayers.Count > 0) playerTeamInt ++;

                playerTeam = (PlayerTeam)playerTeamInt;

                if (playerTeam != PlayerTeam.Both)
                {
                    List<ReportPlayer> playerList = (playerTeam == PlayerTeam.RadiantOnly) ? radiantPlayers : direPlayers;
                    string nickNameStr = "";
                    if (playerList.Count >= 2)
                    {
                        for (int i = 0; i < playerList.Count; i++)
                        {
                            if (i != 0)
                            {
                                nickNameStr += (i == playerList.Count - 1) ? "和" : "，";
                            }
                            nickNameStr += playerList[i].nickname;
                        }
                        nickNameStr += "进行了一场组排。";
                    }
                    return nickNameStr;
                }
                else
                {
                    string radiantNickNames = "";
                    for (int i = 0; i < radiantPlayers.Count; i++)
                    {
                        if (i != 0)
                        {
                            radiantNickNames += (i == radiantPlayers.Count - 1) ? "和" : "，";
                        }
                        radiantNickNames += radiantPlayers[i].nickname;
                    }

                    string direNickNames = "";
                    for (int i = 0; i < direPlayers.Count; i++)
                    {
                        if (i != 0)
                        {
                            direNickNames += (i == direPlayers.Count - 1) ? "和" : "，";
                        }
                        direNickNames += direPlayers[i].nickname;
                    }

                    return radiantNickNames + "对阵" + direNickNames + "。";
                }
            }

            /// <summary>
            /// 用于生成当场比赛的战报
            /// </summary>
            /// <param name="detailedLevel"></param>
            /// <returns></returns>
            public string GetMatchReport()
            {
                DateTime endTime = Dota2API.GetDateTimeBySeconds(startTime + duration) + TimeSpan.FromHours(8.0);
                string timeStr = string.Format("[{0}年{1}月{2}日{3}时{4:d2}分速报]\r\n", endTime.Year, endTime.Month, endTime.Day, endTime.Hour, endTime.Minute);
                string gameModeStr = string.Format("[{0}]", Dota2Enums.GetGameMode(gameMode));

                string teamReport = GetTeamReport();

                string killReport = "";
                foreach (ReportPlayer player in radiantPlayers)
                {
                    killReport += player.GetKillReport() + "，";
                }
                foreach (ReportPlayer player in direPlayers)
                {
                    killReport += player.GetKillReport() + "，";
                }

                Dota2Enums.Team winTeam = radiantWins ? Dota2Enums.Team.Radiant : Dota2Enums.Team.Dire;
                string winStr;
                if (playerTeam == PlayerTeam.Both)
                {
                    winStr = Dota2Enums.GetTeamName(winTeam) + "取得了胜利，";
                }
                else
                {
                    bool isWin = winTeam == ((playerTeam == PlayerTeam.RadiantOnly) ? Dota2Enums.Team.Radiant : Dota2Enums.Team.Dire);
                    winStr = isWin ? "取得了胜利，" : "遗憾落败，";
                }
                string durationStr = string.Format("战斗时长{0}。\r\n", Dota2Enums.GetDuration(TimeSpan.FromSeconds(duration)));

                string detailedReport = "[比赛详情]\r\n";
                foreach (ReportPlayer player in radiantPlayers)
                {
                    detailedReport += player.playerReport;
                }
                foreach (ReportPlayer player in direPlayers)
                {
                    detailedReport += player.playerReport;
                }

                string matchIDStr = string.Format("希望了解更多请观看比赛ID[{0}]", matchID);

                return timeStr
                    + gameModeStr + teamReport + killReport + winStr + durationStr
                    + detailedReport + matchIDStr;
            }
        }

        /// <summary>
        /// 用于生成当次更新的战报
        /// </summary>
        /// <param name="matches"></param>
        /// <param name="detailedLevel"></param>
        /// <returns></returns>
        static private List<string> GetPerUpdateReport(List<DatabaseStructs.Match> matches, DetailedLevel detailedLevel = DetailedLevel.Normal, bool reportWinOnly = false)
        {
            List<string> perUpdateReport = new List<string>();
            if (matches.Count == 0) return perUpdateReport;

            Dictionary<long, ReportMatch> reportMatchDict = new Dictionary<long, ReportMatch>();

            foreach (DatabaseStructs.Match match in matches)
            {
                if (match.gameMode == (int)Dota2Enums.GameMode.EventMode) continue;

                ReportPlayer reportPlayer = new ReportPlayer
                {
                    shortSteamID = match.playerID,
                    nickname = match.nickname,
                    hero = Dota2Enums.GetHeroByID(match.heroID),
                    kills = match.kills,
                    playerReport = match.report
                };
                if (reportMatchDict.ContainsKey(match.matchID))
                {
                    ReportMatch reportMatch = reportMatchDict[match.matchID];
                    if (match.isRadiant)
                    {
                        reportMatch.radiantPlayers.Add(reportPlayer);
                    }
                    else
                    {
                        reportMatch.direPlayers.Add(reportPlayer);
                    }
                }
                else
                {
                    ReportMatch reportMatch = new ReportMatch
                    {
                        matchID = match.matchID,
                        gameMode = (Dota2Enums.GameMode)match.gameMode,
                        radiantPlayers = new List<ReportPlayer>(),
                        direPlayers = new List<ReportPlayer>(),
                        playerTeam = PlayerTeam.Both,
                        startTime = match.startTime,
                        duration = match.duration,
                        radiantWins = !(match.isRadiant ^ match.win),
                        matchReport = ""
                    };
                    if (match.isRadiant)
                    {
                        reportMatch.radiantPlayers.Add(reportPlayer);
                    }
                    else
                    {
                        reportMatch.direPlayers.Add(reportPlayer);
                    }
                    reportMatchDict[match.matchID] = reportMatch;
                }
            }

            List<ReportMatch> sortedMatch = reportMatchDict.Values.ToList();
            sortedMatch.Sort((ReportMatch match1, ReportMatch match2) =>
            {
                return Math.Sign(match1.startTime + match1.duration - match2.startTime - match2.duration);
            });

            foreach (ReportMatch match in sortedMatch)
            {
                if(reportWinOnly && (match.playerTeam != PlayerTeam.Both))
                {
                    bool win = (match.playerTeam == PlayerTeam.RadiantOnly) ^ match.radiantWins;
                    if (!win) continue;
                }
                perUpdateReport.Add(match.GetMatchReport());
            }

            return perUpdateReport;
        }

        /// <summary>
        /// 用于更新所有在playerHelper数据库中的玩家的比赛信息
        /// 返回该次更新所获得的战报
        /// </summary>
        /// <returns></returns>         
        static public async Task<List<string>> UpdateAllPlayers(bool reportWinOnly=false)
        {
            playerHelper.OpenOrCreateSql();
            playerHelper.CreateTableIfNotExists();

            matchHelper.OpenOrCreateSql();
            matchHelper.CreateTableIfNotExists();

            updateHelper.OpenOrCreateSql();
            updateHelper.CreateTableIfNotExists();
            updateHelper.DeleteAllData();

            try
            {
                List<DatabaseStructs.Player> playerList = playerHelper.GetAllData();
                List<Task> updateTasks = new List<Task>();
                foreach (DatabaseStructs.Player player in playerList)
                {
                    updateTasks.Add(UpdatePlayer(player.nickname, player.shortSteamID));
                    Thread.Sleep(1000);
                }
                await Task.WhenAll(updateTasks);

                List<DatabaseStructs.Match> matches = updateHelper.GetAllData();
                List<string> perUpdateReport = GetPerUpdateReport(matches, DetailedLevel.Detailed, reportWinOnly);
                return perUpdateReport;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return new List<string>();
            }
            finally
            {
                playerHelper.CloseSql();
                matchHelper.CloseSql();
                updateHelper.CloseSql();
            }
        }

        /// <summary>
        /// 定时更新所有在playerHelper数据库中的玩家的比赛信息
        /// 并将战报发送出去
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static public async Task UpdateAllPlayersEveryXMinutes(double x)
        {
            while (true)
            {
                //如果改为true，就是报喜不报忧
                List<string> perUpdateReports = await UpdateAllPlayers(GlobalConfig.dota2WatcherOnlyReportWin);
                foreach (string report in perUpdateReports)
                {
                    Mirai.CSharp.HttpApi.Models.ChatMessages.IChatMessage[] chain = new Mirai.CSharp.HttpApi.Models.ChatMessages.IChatMessage[]
                    {
                        new Mirai.CSharp.HttpApi.Models.ChatMessages.PlainMessage($"{report}")
                    };                    
                    await Program.Session.SendGroupMessageAsync(GlobalConfig.dota2WatcherReportToQun, chain);
                }
                Thread.Sleep(TimeSpan.FromMinutes(x));
            }
        }

        static public async Task Find(long groupID, int quoteID, int databaseID, int propertyID, string searchValue)
        {
            databaseID = databaseID % 2;

            SqliteHelper<DatabaseStructs.Player> tempPlayerHelper =
                new SqliteHelper<DatabaseStructs.Player>(GlobalConfig.dota2WatcherDBName + ".db", "Players");
            SqliteHelper<DatabaseStructs.Match> tempMatchHelper =
                new SqliteHelper<DatabaseStructs.Match>(GlobalConfig.dota2WatcherDBName + ".db", "Matches");

            tempPlayerHelper.OpenOrCreateSql();
            tempPlayerHelper.CreateTableIfNotExists();

            tempMatchHelper.OpenOrCreateSql();
            tempMatchHelper.CreateTableIfNotExists();

            try
            {
                string reply = "";
                if(databaseID == 0)
                {
                    List<DatabaseStructs.Player> playerList = tempPlayerHelper.GetData(propertyID, searchValue);
                    if (playerList == null || playerList.Count == 0)
                    {
                        reply = "玩家列表为空。";
                    }
                    else
                    {
                        for (int i = 0; i < playerList.Count; i++)
                        {
                            reply += playerList[i].ToString() + "\n";
                        }
                    }
                }
                else
                {
                    List<DatabaseStructs.Match> matchList = tempMatchHelper.GetData(propertyID, searchValue);
                    if (matchList == null || matchList.Count == 0)
                    {
                        reply = "比赛列表为空。";
                    }
                    else
                    {
                        for (int i = 0; i < matchList.Count; i++)
                        {
                            reply += matchList[i].ToString() + "\n";
                        }
                    }
                }

                Mirai.CSharp.HttpApi.Models.ChatMessages.IChatMessage[] chain = new Mirai.CSharp.HttpApi.Models.ChatMessages.IChatMessage[]
                {
                    new Mirai.CSharp.HttpApi.Models.ChatMessages.PlainMessage($"{reply}")
                };
                await Program.Session.SendGroupMessageAsync(groupID, chain, quoteID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                tempPlayerHelper.CloseSql();
                tempMatchHelper.CloseSql();
            }
        }

        static public async Task FindAllPlayers(long groupID, int quoteID)
        {
            SqliteHelper<DatabaseStructs.Player> tempPlayerHelper =
                new SqliteHelper<DatabaseStructs.Player>(GlobalConfig.dota2WatcherDBName + ".db", "Players");

            tempPlayerHelper.OpenOrCreateSql();
            tempPlayerHelper.CreateTableIfNotExists();

            try
            {
                string reply = "";
                List<DatabaseStructs.Player> playerList = tempPlayerHelper.GetAllData();
                if(playerList == null || playerList.Count == 0)
                {
                    reply = "玩家列表为空。";
                }
                else
                {
                    for (int i = 0; i < playerList.Count; i++)
                    {
                        reply += playerList[i].ToString() + "\n";
                    }
                }

                Mirai.CSharp.HttpApi.Models.ChatMessages.IChatMessage[] chain = new Mirai.CSharp.HttpApi.Models.ChatMessages.IChatMessage[]
                {
                    new Mirai.CSharp.HttpApi.Models.ChatMessages.PlainMessage($"{reply}")
                };
                await Program.Session.SendGroupMessageAsync(groupID, chain, quoteID);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                tempPlayerHelper.CloseSql();
            }
        }
    }
}
