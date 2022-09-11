using System.Collections.Generic;
using System.Threading.Tasks;
using ConsoleMiraiHTTPAPIApp.app.Divination;
using ConsoleMiraiHTTPAPIApp.app.TRPGDice;
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
    [RegisterMiraiHttpParser(typeof(DefaultMappableMiraiHttpMessageParser<IFriendMessageEventArgs, FriendMessageEventArgs>))]
    public partial class FriendMessage : IMiraiHttpMessageHandler<IFriendMessageEventArgs>
    {
        public async Task HandleMessageAsync(IMiraiHttpSession session, IFriendMessageEventArgs e) // 法1: 使用 IMessageBase[]
        {
            //IChatMessage[] chain = new IChatMessage[]
            //{
            //    new PlainMessage($"收到了来自{e.Sender.Name}({e.Sender.Remark})[{e.Sender.Id}]的私聊消息:{string.Join(null, (IEnumerable<IChatMessage>)e.Chain)}")
            //    //                          /   好友昵称  /  /    好友备注    /  /  好友QQ号  /                                                        / 消息链 /
            //    // 你还可以在这里边加入更多的 IMessageBase
            //};
            //await session.SendFriendMessageAsync(e.Sender.Id, chain); // 向消息来源好友异步发送由以上chain表示的消息
            //e.BlockRemainingHandlers = false; // 不阻断消息传递。如需阻断请返回true

            if (e.Chain[1].ToString().StartsWith(".黄历"))
            {
                ///这个quoteId就是消息的编号，可以实现群内引用回复
                int quoteId = (e.Chain[0] as SourceMessage).Id;
                IChatMessage[] chain = new IChatMessage[]
                {
                    //new PlainMessage($"收到了来自{e.Sender.Name}({e.Sender.Remark})[{e.Sender.Id}]的私聊消息:{string.Join(null, (IEnumerable<IChatMessage>)e.Chain)}")
                    new PlainMessage($"{Divination.FuntionMain(e.Sender.Id)}")
                };
                await session.SendFriendMessageAsync(e.Sender.Id, chain, quoteId); // 向消息来源好友异步发送由以上chain表示的消息
                e.BlockRemainingHandlers = false; // 不阻断消息传递。如需阻断请返回true
            }
        }
    }
}
