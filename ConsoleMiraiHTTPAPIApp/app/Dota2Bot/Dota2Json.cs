using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMiraiHTTPAPIApp.app.Dota2Bot
{
    namespace Dota2Json
    {
        // 用于处理GetMatches API对应的JSON数据的结构体
        namespace GetMatches
        {
            public struct Player
            {
                /// <summary>
                /// 玩家的steam短ID
                /// </summary>
                public long account_id { get; set; }
                /// <summary>
                /// 当值大于100就是夜餍方
                /// </summary>
                public int player_slot { get; set; }
                /// <summary>
                /// teamnumber0=天辉，1=夜餍
                /// </summary>
                public int team_number { get; set; }
                /// <summary>
                /// 玩家处于队伍中的几楼
                /// </summary>
                public int team_slot { get; set; }
                /// <summary>
                /// 英雄ID
                /// </summary>
                public int hero_id { get; set; }
            }

            public struct Match
            {
                /// <summary>
                /// 比赛ID
                /// </summary>
                public long match_id { get; set; }
                public long match_seq_num { get; set; }
                /// <summary>
                /// 比赛开始时间
                /// </summary>
                public long start_time { get; set; }
                /// <summary>
                /// lobby模式
                /// </summary>
                public int lobby_type { get; set; }
                public int radiant_team_id { get; set; }
                public int dire_team_id { get; set; }
                /// <summary>
                /// 玩家列表
                /// </summary>
                public Player[] players { get; set; }
            }


            public struct Result
            {
                public int status { get; set; }
                public int num_results { get; set; }
                public int total_results { get; set; }
                public int results_remaining { get; set; }

                public Match[] matches { get; set; }
            }

            public struct HttpResult
            {
                public Result result { get; set; }
            }
        }

        // 用于处理MatchDetailInfo API对应的JSON数据的结构体
        namespace MatchDetailInfo
        {
            public struct AbilityUpgrade
            {
                public int ability { get; set; }
                public int time { get; set; }
                public int level { get; set; }
            }

            public struct PickBan
            {
                public bool is_pick { get; set; }
                public int hero_id { get; set; }
                public int team { get; set; }
                public int order { get; set; }
            }

            public struct Player
            {
                public long account_id                      { get; set; }
                public int player_slot                      { get; set; }
                public int team_number                      { get; set; }
                public int team_slot                        { get; set; }
                public int hero_id                          { get; set; }
                public int item_0                           { get; set; }
                public int item_1                           { get; set; }
                public int item_2                           { get; set; }
                public int item_3                           { get; set; }
                public int item_4                           { get; set; }
                public int item_5                           { get; set; }
                public int backpack_0                       { get; set; }
                public int backpack_1                       { get; set; }
                public int backpack_2                       { get; set; }
                public int item_neutral                     { get; set; }
                public int kills                            { get; set; }
                public int deaths                           { get; set; }
                public int assists                          { get; set; }
                public int leaver_status                    { get; set; }
                public int last_hits                        { get; set; }
                public int denies                           { get; set; }
                public int gold_per_min                     { get; set; }
                public int xp_per_min                       { get; set; }
                public int level                            { get; set; }
                public int net_worth                        { get; set; }
                public int aghanims_scepter                 { get; set; }
                public int aghanims_shard                   { get; set; }
                public int moonshard                        { get; set; }
                public int hero_damage                      { get; set; }
                public int tower_damage                     { get; set; }
                public int gold                             { get; set; }
                public int gold_spent                       { get; set; }
                public int scaled_hero_damage               { get; set; }
                public AbilityUpgrade[] ability_upgrades   { get; set; }

            }

            public struct Result
            {
                public Player[] players { get; set; }
                /// <summary>
                /// 确认胜利方，true是天辉赢，false是夜餍赢
                /// </summary>
                public bool radiant_win { get; set; }
                /// <summary>
                /// 比赛历时多少秒
                /// </summary>
                public int duration { get; set; }
                public int pre_game_duration { get; set; }
                /// <summary>
                /// 开始时间
                /// </summary>
                public long start_time { get; set; }
                /// <summary>
                /// 比赛ID
                /// </summary>
                public long match_id { get; set; }
                public long match_seq_num { get; set; }
                public int tower_status_radiant { get; set; }
                public int tower_status_dire { get; set; }
                public int barracks_status_radiant { get; set; }
                public int barracks_statuc_dire { get; set; }
                public int cluster { get; set; }
                public int first_blood_time { get; set; }
                /// <summary>
                /// 比赛模式
                /// </summary>
                public int lobby_type { get; set; }
                public int human_players { get; set; }
                public int positive_votes { get; set; }
                public int negative_votes { get; set; }
                /// <summary>
                /// 游戏模式
                /// </summary>
                public int game_mode { get; set; }
                public int flags { get; set; }
                public int engine { get; set; }
                /// <summary>
                /// 天辉人头数
                /// </summary>
                public int radiant_score { get; set; }
                /// <summary>
                /// 夜餍人头数
                /// </summary>
                public int dire_score { get; set; }
                public PickBan[] picks_bans { get; set; }
            }

            public struct HttpResult
            {
                public Result result { get; set; }
            }
        }
    }
}
