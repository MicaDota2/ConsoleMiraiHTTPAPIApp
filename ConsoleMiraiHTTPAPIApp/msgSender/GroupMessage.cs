using System.Collections.Generic;
using System.Reflection.Emit;
using System.Threading.Tasks;
using ConsoleMiraiHTTPAPIApp.app.Dota2Bot;
using ConsoleMiraiHTTPAPIApp.app.TRPGDice;
using Mirai.CSharp.HttpApi.Builders;
using Mirai.CSharp.HttpApi.Handlers;
using Mirai.CSharp.HttpApi.Models.ChatMessages;
using Mirai.CSharp.HttpApi.Models.EventArgs;
using Mirai.CSharp.HttpApi.Parsers;
using Mirai.CSharp.HttpApi.Parsers.Attributes;
using Mirai.CSharp.HttpApi.Session;

#pragma warning disable CA1822 // Mark members as static // 示例方法禁用Information
#pragma warning disable IDE0059 // 不需要赋值 // 禁用+1
namespace ConsoleMiraiHTTPAPIApp.msgSender
{
    [RegisterMiraiHttpParser(typeof(DefaultMappableMiraiHttpMessageParser<IGroupMessageEventArgs, GroupMessageEventArgs>))]
    public partial class GroupMessage : IMiraiHttpMessageHandler<IGroupMessageEventArgs>
    {
        public async Task HandleMessageAsync(IMiraiHttpSession session, IGroupMessageEventArgs e) // 法1: 使用 IMessageBase[]
        {
            //if (e.Sender.Group.Id == 277913860)
            //{
            //    // 临时消息和群消息一致, 不多写例子了
            //    IChatMessage[] chain = new IChatMessage[]
            //    {
            //    new AtMessage(1012803704),
            //    new PlainMessage($"收到了来自{e.Sender.Name}[{e.Sender.Id}]{{{e.Sender.Permission}}}的群消息:{string.Join(null, (IEnumerable<IChatMessage>)e.Chain)}"),
            //        // / 发送者群名片 /  / 发送者QQ号 /   /发送者在群内权限 / / 消息链 /
            //        // 你还可以在这里边加入更多的 IMessageBase

            //    new PlainMessage($"\r\ne.Chain.Length：{e.Chain.Length}\r\ne.Chain[0]:{e.Chain[0]}\r\ne.Chain[1]:{e.Chain[1]}")
            //    };

            //    await session.SendGroupMessageAsync(277913860, chain); // 向消息来源群异步发送由以上chain表示的消息
            //    e.BlockRemainingHandlers = false; // 不阻断消息传递。如需阻断请返回true
            //}


            if (e.Chain[1].ToString().Length >= 9 && e.Chain[1].ToString().StartsWith(".roll"))
            {                
                ///这个quoteId就是消息的编号，可以实现群内引用回复
                int quoteId = (e.Chain[0] as SourceMessage).Id;
                IChatMessage[] chain = new IChatMessage[]
                {
                    new PlainMessage($"\r\n{TRPG_Dice.dndroll(e.Chain[1].ToString())}")
                };
                await session.SendGroupMessageAsync(e.Sender.Group.Id, chain, quoteId); // 向消息来源群异步发送由以上chain表示的消息
                e.BlockRemainingHandlers = false; // 不阻断消息传递。如需阻断请返回true
            }

            try
            {
                if (e.Chain[1].ToString().StartsWith(".更新战绩"))
                {
                    List<string> perUpdateReports = await Dota2WatchRunner.UpdateAllPlayers();

                    foreach (string report in perUpdateReports)
                    {
                        IChatMessage[] chain = new IChatMessage[]
                        {
                            new PlainMessage($"\r\n{report}")
                        };
                        await session.SendGroupMessageAsync(e.Sender.Group.Id, chain); // 向消息来源群异步发送由以上chain表示的消息
                    }
                    e.BlockRemainingHandlers = false; // 不阻断消息传递。如需阻断请返回true
                }

                // .监视 昵称 1321321321
                // 数字是steam短ID
                if (e.Chain[1].ToString().StartsWith(".监视"))
                {
                    string rawStr = e.Chain[1].ToString();
                    string[] strList = rawStr.Split(' ');
                    if (strList.Length <= 2) return;

                    Dota2WatchRunner.AddPlayerState state = await Dota2WatchRunner.AddPlayer(strList[1], long.Parse(strList[2]));
                    string msg = Dota2WatchRunner.GetAddPlayerState(state);

                    ///这个quoteId就是消息的编号，可以实现群内引用回复
                    int quoteId = (e.Chain[0] as SourceMessage).Id;
                    IChatMessage[] chain = new IChatMessage[]
                    {
                    new PlainMessage($"{msg}")
                    };
                    await session.SendGroupMessageAsync(e.Sender.Group.Id, chain, quoteId); // 向消息来源群异步发送由以上chain表示的消息
                    e.BlockRemainingHandlers = false; // 不阻断消息传递。如需阻断请返回true
                }

                if (e.Chain[1].ToString().StartsWith(".查询"))
                {
                    string rawStr = e.Chain[1].ToString();
                    string[] strList = rawStr.Split(' ');
                    if (strList.Length <= 3) return;

                    int databaseID = int.Parse(strList[1]);
                    int propertyID = int.Parse(strList[2]);
                    int quoteId = (e.Chain[0] as SourceMessage).Id;
                    await Dota2WatchRunner.Find(e.Sender.Group.Id, quoteId, databaseID, propertyID, strList[3]);
                    e.BlockRemainingHandlers = false; // 不阻断消息传递。如需阻断请返回true
                }

                if (e.Chain[1].ToString().StartsWith(".所有玩家"))
                {
                    int quoteId = (e.Chain[0] as SourceMessage).Id;
                    await Dota2WatchRunner.FindAllPlayers(e.Sender.Group.Id, quoteId);
                    e.BlockRemainingHandlers = false; // 不阻断消息传递。如需阻断请返回true
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                Console.WriteLine(exception.StackTrace);
            }
        }
    }
}
