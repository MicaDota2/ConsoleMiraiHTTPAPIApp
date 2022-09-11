using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiraiHTTPAPIApp.app.Dota2Bot
{
    internal class Dota2Enums
    {
        #region Enums
        public enum Team
        {
            Radiant = 0,
            Dire
        }

        public enum Result
        {
            Lose = 0,
            Win = 1,
        }
                
        /// <summary>
        /// 游戏模式
        /// 详见https://wiki.teamfortress.com/wiki/WebAPI/GetMatchDetails
        /// </summary>
        public enum GameMode
        {
            //0: "No Game Mode",
            //1: "全英雄选择",
            //2: "队长模式",
            //3: "随机征召",
            //4: "小黑屋",
            //5: "全部随机",
            //7: "万圣节活动",
            //8: "反队长模式",
            //9: "贪魔活动",
            //10: "教程",
            //11: "中路模式",
            //12: "生疏模式",
            //13: "新手模式",
            //14: "Compendium Matchmaking",
            //15: "自定义游戏",
            //16: "队长征召",
            //17: "平衡征召",
            //18: "技能征召",
            //19: "活动模式",
            //20: "全英雄死亡随机",
            //21: "中路SOLO",
            //22: "全英雄选择",
            //23: "加速模式"
            None = 0,
            AllPick,
            CaptainsMode,
            RandomDraft,
            SingleDraft,
            AllRandom,
            Intro,
            DireTide,
            ReverseCaptainsMode,
            TheGreeviling,
            Tutorial,
            MidOnly,
            LeastPlayed,
            NewPlayerPool,
            CompendiumMatchmaking,
            CoopVSBots,
            CaptainsDraft,
            BalancedDraft,
            AbilityDraft,
            EventMode,
            AllRandomDeathmatch,
            MidOnly1v1,
            RankedAllPick, // RankMatchmaking?
            Turbo,
        }

        /// <summary>
        /// 房间模式
        /// </summary>
        public enum Lobby
        {
            //-1: "非法ID",
            //0:  "普通匹配",
            //1:  "练习",
            //2:  "锦标赛",
            //3:  "教程",
            //4:  "合作对抗电脑",
            //5:  "组排模式",
            //6:  "单排模式",
            //7:  "天梯匹配",
            //8:  "中路SOLO",
            //12: "夜魇暗潮"
            Invalid = -1,
            PublicMatchmaking,
            Practise,
            Tournament,
            Tutorial,
            CoopWithBots,
            TeamMatch,
            SoloQueue,
            RankedMatchMaking,
            SoloMid,

            Diretide = 12,
        }
        /// <summary>
        /// 比赛长度枚举
        /// </summary>
        public enum MatchLength
        {
            Normal = 0,
            Short,
            Long,
        }

        /// <summary>
        /// 英雄id
        /// </summary>
        public enum Hero
        {
            Unknown = 0,
            AntiMage = 1,
            Axe,
            Bane,
            Bloodseeker,
            CrystalMaiden,
            DrowRanger,
            EarthShaker,
            Juggernaut,
            Mirana,
            Morphling = 10,
            ShadowFiend,
            PhantomLancer,
            Puck,
            Pudge,
            Razor,
            SandKing,
            StormSpirit,
            Sven,
            Tiny,
            VengefulSpirit = 20,
            WindRanger,
            Zeus,
            Kunkka,

            Lina = 25,
            Lion,
            ShadowShaman,
            Slardar,
            Tidehunter,
            WitchDoctor = 30,
            Lich,
            Riki,
            Enigma,
            Tinker,
            Sniper,
            Necrophos,
            Warlock,
            BeastMaster,
            QueenofPain,
            Venomancer = 40,
            FacelessVoid,
            WraithKing,
            DeathProphet,
            PhantomAssassin,
            Pugna,
            TemplarAssassin,
            Viper,
            Luna,
            DragonKnight,
            Dazzle = 50,
            Clockwerk,
            Leshrac,
            NaturesProphet,
            Lifestealer,
            DarkSeer,
            Clinkz,
            OmniKnight,
            Enchantress,
            Huskar,
            NightStalker = 60,
            Broodmother,
            BountyHunter,
            Weaver,
            Jakiro,
            Batrider,
            Chen,
            Spectre,
            AncientApparition,
            Doom,
            Ursa = 70,
            SpiritBreaker,
            Gyrocopter,
            Alchemist,
            Invoker,
            Silencer,
            OutworldDevourer,
            Lycan,
            Brewmaster,
            ShadowDemon,
            LoneDruid = 80,
            ChaosKnight,
            Meepo,
            TreantProtector,
            OgreMagi,
            Undying,
            Rubick,
            Disrupter,
            NyxAssassin,
            NagaSiren,
            KeeperoftheLight = 90,
            Io,
            Visage,
            Slark,
            Medusa,
            TrollWarlord,
            CentaurWarrior,
            Magnus,
            Timbersaw,
            Bristleback,
            Tusk = 100,
            SkywrathMage,
            Abaddon,
            ElderTitan,
            LegionCommander,
            Techies,
            EmberSpirit,
            EarthSpirit,
            Underlord,
            Terrorblade,
            Phoenix = 110,
            Oracle,
            WinterWyvern,
            ArcWarden,
            MonkeyKing,

            DarkWillow = 119,
            Pangolier = 120,
            Grimstroke,

            Hoodwink = 123,

            VoidSpirit = 126,

            Snapfire = 128,
            Mars,

            Dawnbreaker = 135,
            Marci,
            PrimalBeast,

        }
        #endregion

        /// <summary>
        /// 根据slot判断队伍, 返回1为天辉, 2为夜魇
        /// </summary>
        /// <param name="slot"></param>
        /// <returns></returns>
        static public Team GetTeamBySlot(int slot)
        {
            return slot < 100 ? Team.Radiant : Team.Dire;
        }

        /// <summary>
        /// 根据英雄ID取英雄名字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        static public Hero GetHeroByID(int id)
        {
            return Enum.IsDefined(typeof(Hero), id) ? (Hero)id : Hero.Unknown;
        }

        /// <summary>
        /// 取队伍阵营
        /// </summary>
        /// <param name="team"></param>
        /// <returns></returns>
        static public string GetTeamName(Team team)
        {
            switch (team)
            {
                case Team.Dire:
                    return "夜魇";
                case Team.Radiant:
                default:
                    return "天辉";
            }
        }

        /// <summary>
        /// 取比赛结果是输是赢
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        static public string GetResult(Result result)
        {
            switch (result)
            {
                case Result.Lose:
                    return "遗憾落败";
                case Result.Win:
                default:
                    return "获得了胜利";
            }
        }

        /// <summary>
        /// 获取比赛类型的名字，不一定对
        /// </summary>
        /// <param name="gameMode"></param>
        /// <returns></returns>
        static public string GetGameMode(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.AllPick:
                    return "全英雄选择";
                case GameMode.CaptainsMode:
                    return "队长模式";
                case GameMode.RandomDraft:
                    return "随机征召";
                case GameMode.SingleDraft:
                    return "单一征兆";
                case GameMode.AllRandom:
                    return "全英雄随机";
                case GameMode.Intro:
                    return "介绍模式？";
                case GameMode.DireTide:
                    return "夜魇暗潮";
                case GameMode.ReverseCaptainsMode:
                    return "逆队长征召";
                case GameMode.TheGreeviling:
                    return "贪魔活动";
                case GameMode.Tutorial:
                    return "教程";
                case GameMode.MidOnly:
                    return "中路对单";
                case GameMode.LeastPlayed:
                    return "生疏模式";
                case GameMode.NewPlayerPool:
                    return "新手模式";
                case GameMode.CompendiumMatchmaking:
                    return "Compendium Matchmaking";
                case GameMode.CoopVSBots:
                    return "合作对抗电脑";
                case GameMode.CaptainsDraft:
                    return "队长征召";
                case GameMode.BalancedDraft:
                    return "平衡征召";
                case GameMode.AbilityDraft:
                    return "技能征召";
                case GameMode.EventMode:
                    return "活动模式";
                case GameMode.AllRandomDeathmatch:
                    return "全英雄死亡随机";
                case GameMode.MidOnly1v1:
                    return "单中模式";
                case GameMode.RankedAllPick:
                    return "全英雄选择";
                case GameMode.Turbo:
                    return "加速模式";
                case GameMode.None:
                default:
                    return "未知游戏类型";
            }
        }

        /// <summary>
        /// 获取英雄的名字，返回了array，便于添加英雄昵称
        /// </summary>
        /// <param name="hero"></param>
        /// <returns></returns>
        static public string[] GetHeroName(Hero hero)
        {
            switch (hero)
            {
                case Hero.AntiMage:
                    return new string[] { "敌法师", "敌法" };
                case Hero.Axe:
                    return new string[] { "斧王" };
                case Hero.Bane:
                    return new string[] { "祸乱之源", "祸乱" };
                case Hero.Bloodseeker:
                    return new string[] { "血魔" };
                case Hero.CrystalMaiden:
                    return new string[] { "水晶室女", "冰女" };
                case Hero.DrowRanger:
                    return new string[] { "卓尔游侠", "小黑" };
                case Hero.EarthShaker:
                    return new string[] { "撼地者", "小牛", "牛头" };
                case Hero.Juggernaut:
                    return new string[] { "主宰", "剑圣", "jugg", "奶棒人" };
                case Hero.Mirana:
                    return new string[] { "米拉娜", "白虎", "pom" };
                case Hero.Morphling:
                    return new string[] { "变体精灵", "水人" };
                case Hero.ShadowFiend:
                    return new string[] { "影魔", "SF" };
                case Hero.PhantomLancer:
                    return new string[] { "幻影长矛手", "猴子" };
                case Hero.Puck:
                    return new string[] { "帕克" };
                case Hero.Pudge:
                    return new string[] { "帕吉", "屠夫" };
                case Hero.Razor:
                    return new string[] { "剃刀", "电棍", "电魂" };
                case Hero.SandKing:
                    return new string[] { "沙王", "SK" };
                case Hero.StormSpirit:
                    return new string[] { "风暴之灵", "蓝猫" };
                case Hero.Sven:
                    return new string[] { "斯温", "SW", "流浪剑客" };
                case Hero.Tiny:
                    return new string[] { "小小" };
                case Hero.VengefulSpirit:
                    return new string[] { "复仇之魂", "复仇", "VS" };
                case Hero.WindRanger:
                    return new string[] { "风行者", "风行" };
                case Hero.Zeus:
                    return new string[] { "宙斯", "雷电将军（x" };
                case Hero.Kunkka:
                    return new string[] { "昆卡", "船长" };
                case Hero.Lina:
                    return new string[] { "莉娜", "火女" };
                case Hero.Lion:
                    return new string[] { "莱恩", "lion" };
                case Hero.ShadowShaman:
                    return new string[] { "暗影萨满", "小Y", "小歪" };
                case Hero.Slardar:
                    return new string[] { "斯拉达", "大鱼", "大鱼人" };
                case Hero.Tidehunter:
                    return new string[] { "潮汐猎人", "潮汐" };
                case Hero.WitchDoctor:
                    return new string[] { "巫医" };
                case Hero.Lich:
                    return new string[] { "巫妖" };
                case Hero.Riki:
                    return new string[] { "力丸", "隐刺" };
                case Hero.Enigma:
                    return new string[] { "谜团" };
                case Hero.Tinker:
                    return new string[] { "修补匠", "TK", "tinker" };
                case Hero.Sniper:
                    return new string[] { "狙击手", "火枪", "小炮" };
                case Hero.Necrophos:
                    return new string[] { "瘟疫法师", "NEC", "死灵法师" };
                case Hero.Warlock:
                    return new string[] { "术士" };
                case Hero.BeastMaster:
                    return new string[] { "兽王" };
                case Hero.QueenofPain:
                    return new string[] { "痛苦女王", "女王", "QOP" };
                case Hero.Venomancer:
                    return new string[] { "剧毒术士", "剧毒" };
                case Hero.FacelessVoid:
                    return new string[] { "虚空假面", "虚空", "锤头鲨" };
                case Hero.WraithKing:
                    return new string[] { "冥魂大帝", "骷髅王" };
                case Hero.DeathProphet:
                    return new string[] { "死亡先知", "DP" };
                case Hero.PhantomAssassin:
                    return new string[] { "幻影刺客", "幻刺", "PA" };
                case Hero.Pugna:
                    return new string[] { "帕格纳", "骨法" };
                case Hero.TemplarAssassin:
                    return new string[] { "圣堂刺客", "圣堂", "TA" };
                case Hero.Viper:
                    return new string[] { "冥界亚龙", "毒龙" };
                case Hero.Luna:
                    return new string[] { "露娜", "月骑" };
                case Hero.DragonKnight:
                    return new string[] { "龙骑士", "龙骑" };
                case Hero.Dazzle:
                    return new string[] { "戴泽", "暗牧" };
                case Hero.Clockwerk:
                    return new string[] { "发条技师", "发条" };
                case Hero.Leshrac:
                    return new string[] { "拉席克", "老鹿" };
                case Hero.NaturesProphet:
                    return new string[] { "先知" };
                case Hero.Lifestealer:
                    return new string[] { "噬魂鬼", "小狗" };
                case Hero.DarkSeer:
                    return new string[] { "黑暗贤者", "黑贤", "兔子" };
                case Hero.Clinkz:
                    return new string[] { "克林克兹", "小骷髅", "骨弓" };
                case Hero.OmniKnight:
                    return new string[] { "全能骑士", "全能" };
                case Hero.Enchantress:
                    return new string[] { "魅惑魔女", "小鹿" };
                case Hero.Huskar:
                    return new string[] { "哈斯卡" };
                case Hero.NightStalker:
                    return new string[] { "暗夜魔王", "夜魔" };
                case Hero.Broodmother:
                    return new string[] { "育母蜘蛛", "蜘蛛" };
                case Hero.BountyHunter:
                    return new string[] { "赏金猎人", "赏金" };
                case Hero.Weaver:
                    return new string[] { "编织者", "蚂蚁" };
                case Hero.Jakiro:
                    return new string[] { "杰奇洛", "双头龙" };
                case Hero.Batrider:
                    return new string[] { "蝙蝠骑士", "蝙蝠" };
                case Hero.Chen:
                    return new string[] { "陈", "老陈" };
                case Hero.Spectre:
                    return new string[] { "幽鬼", "UG", "SPE" };
                case Hero.AncientApparition:
                    return new string[] { "远古冰魄", "冰魂" };
                case Hero.Doom:
                    return new string[] { "末日使者", "末日", "DOOM" };
                case Hero.Ursa:
                    return new string[] { "熊战士", "拍拍", "拍拍熊" };
                case Hero.SpiritBreaker:
                    return new string[] { "裂魂人", "白牛" };
                case Hero.Gyrocopter:
                    return new string[] { "矮人直升机", "飞机", "直升机" };
                case Hero.Alchemist:
                    return new string[] { "炼金术士", "炼金" };
                case Hero.Invoker:
                    return new string[] { "祈求者", "卡尔" };
                case Hero.Silencer:
                    return new string[] { "沉默术士", "沉默" };
                case Hero.OutworldDevourer:
                    return new string[] { "殁境神蚀者", "黑鸟" };
                case Hero.Lycan:
                    return new string[] { "狼人" };
                case Hero.Brewmaster:
                    return new string[] { "酒仙", "熊猫", "熊猫酒仙" };
                case Hero.ShadowDemon:
                    return new string[] { "暗影恶魔", "毒狗" };
                case Hero.LoneDruid:
                    return new string[] { "德鲁伊", "熊德" };
                case Hero.ChaosKnight:
                    return new string[] { "混沌骑士", "CK", "混沌" };
                case Hero.Meepo:
                    return new string[] { "米波", "地卜师" };
                case Hero.TreantProtector:
                    return new string[] { "树精卫士", "大树", "树精" };
                case Hero.OgreMagi:
                    return new string[] { "食人魔魔法师", "蓝胖" };
                case Hero.Undying:
                    return new string[] { "不死尸王", "尸王" };
                case Hero.Rubick:
                    return new string[] { "拉比克" };
                case Hero.Disrupter:
                    return new string[] { "干扰者", "萨尔" };
                case Hero.NyxAssassin:
                    return new string[] { "司夜刺客", "小强", "NA" };
                case Hero.NagaSiren:
                    return new string[] { "娜迦海妖", "小娜迦", "水母大王" };
                case Hero.KeeperoftheLight:
                    return new string[] { "光之守卫", "光法" };
                case Hero.Io:
                    return new string[] { "艾欧", "IO", "小精灵" };
                case Hero.Visage:
                    return new string[] { "维萨吉", "死灵龙" };
                case Hero.Slark:
                    return new string[] { "斯拉克", "小鱼" };
                case Hero.Medusa:
                    return new string[] { "美杜莎", "MED" };
                case Hero.TrollWarlord:
                    return new string[] { "巨魔战将", "巨魔" };
                case Hero.CentaurWarrior:
                    return new string[] { "半人马战行者", "人马" };
                case Hero.Magnus:
                    return new string[] { "马格纳斯", "猛犸" };
                case Hero.Timbersaw:
                    return new string[] { "伐木机" };
                case Hero.Bristleback:
                    return new string[] { "钢背兽", "钢背" };
                case Hero.Tusk:
                    return new string[] { "巨牙海民", "海民" };
                case Hero.SkywrathMage:
                    return new string[] { "天怒法师", "天怒" };
                case Hero.Abaddon:
                    return new string[] { "亚巴顿" };
                case Hero.ElderTitan:
                    return new string[] { "上古巨神", "大牛" };
                case Hero.LegionCommander:
                    return new string[] { "军团指挥官", "军团" };
                case Hero.Techies:
                    return new string[] { "工程师", "炸弹人" };
                case Hero.EmberSpirit:
                    return new string[] { "灰烬之灵", "火猫" };
                case Hero.EarthSpirit:
                    return new string[] { "大地之灵", "土猫" };
                case Hero.Underlord:
                    return new string[] { "孽主", "大屁股" };
                case Hero.Terrorblade:
                    return new string[] { "恐怖利刃", "TB" };
                case Hero.Phoenix:
                    return new string[] { "凤凰" };
                case Hero.Oracle:
                    return new string[] { "神谕者", "神域" };
                case Hero.WinterWyvern:
                    return new string[] { "寒冬飞龙", "冰龙" };
                case Hero.ArcWarden:
                    return new string[] { "天穹守望者", "电狗" };
                case Hero.MonkeyKing:
                    return new string[] { "齐天大圣", "大圣" };
                case Hero.DarkWillow:
                    return new string[] { "邪影芳灵", "小仙女", "小花仙", "花仙子" };
                case Hero.Pangolier:
                    return new string[] { "石鳞剑士", "滚滚" };
                case Hero.Grimstroke:
                    return new string[] { "天涯墨客", "墨客" };
                case Hero.Hoodwink:
                    return new string[] { "森海飞霞", "小松鼠" };
                case Hero.VoidSpirit:
                    return new string[] { "虚无之灵", "紫猫" };
                case Hero.Snapfire:
                    return new string[] { "电炎绝手", "老奶奶" };
                case Hero.Mars:
                    return new string[] { "玛尔斯" };
                case Hero.Dawnbreaker:
                    return new string[] { "破晓辰星", "锤妹" };
                case Hero.Marci:
                    return new string[] { "玛西" };
                case Hero.PrimalBeast:
                    return new string[] { "獸", "畜", "兽" };
                case Hero.Unknown:
                default:
                    return new string[] { "未知英雄" };
            }
        }

        /// <summary>
        /// 判断比赛的时长，30分以下是短，50分以上是长
        /// </summary>
        /// <param name="duration">传入游戏时间，单位是秒</param>
        /// <returns></returns>
        static public MatchLength GetMatchLength(TimeSpan duration)
        {
            MatchLength matchLength = MatchLength.Normal;
            if (duration < TimeSpan.FromMinutes(30.0f))
            {
                matchLength = MatchLength.Short;
            }
            if (duration > TimeSpan.FromMinutes(50.0f))
            {
                matchLength = MatchLength.Long;
            }
            return matchLength;
        }

        static public string GetDuration(TimeSpan duration)
        {
            string durationStr = "";
            if (duration.TotalHours >= 1.0)
            {
                durationStr += duration.Hours + "小时";
            }

            if (duration.Minutes != 0)
            {
                durationStr += duration.Minutes + "分";
            }

            durationStr += duration.Seconds + "秒";
            return durationStr;
        }


        /// <summary>
        /// 对不同的时长和结果生成不同的战报
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="result"></param>
        /// <param name="team"></param>
        /// <param name="enermyTeam"></param>
        /// <returns></returns>
        static public string GetReportDesc(TimeSpan duration, Result result, string team = "", string enermyTeam = "")
        {
            MatchLength matchLength = GetMatchLength(duration);
            switch (matchLength)
            {
                case MatchLength.Short:
                    if (result == Result.Lose)
                    {
                        return string.Format("{0}所向披靡，仅使用了{2}就轻松战胜{1}", team, enermyTeam, GetDuration(duration));
                    }
                    else
                    {
                        return string.Format("{0}惨遭{1}蹂躏，才{2}就败下阵来", team, enermyTeam, GetDuration(duration));
                    }
                case MatchLength.Long:
                    if (result == Result.Lose)
                    {
                        return string.Format("可歌可泣，{0}鏖战了{2}，力克对手{1}，终于取得了来之不易的胜利", team, enermyTeam, GetDuration(duration));
                    }
                    else
                    {
                        return string.Format("顶级长痛，{0}苦苦支撑了{2}，虽然不敌{1}但打出了风采", team, enermyTeam, GetDuration(duration));
                    }
                case MatchLength.Normal:
                default:
                    if (result == Result.Lose)
                    {
                        return string.Format("{0}战斗了{1}，取得了胜利", team, GetDuration(duration));
                    }
                    else
                    {
                        return string.Format("{0}战斗了{1}，输掉了比赛", team, GetDuration(duration));
                    }
            }
        }

    }
}
