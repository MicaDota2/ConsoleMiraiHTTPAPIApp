# ConsoleMiraiHTTPAPIApp
## 关于本项目

### 这是一个 [mirai](https://github.com/mamoe/mirai) 平台的QQ机器人,使用Mirai-Api-Http(采用[Mirai-CSharp](https://github.com/Executor-Cheng/Mirai-CSharp))协议, 实现了TRPG的骰子、趣味万年历以及Dota2处刑BOT的功能

**有任何建议都可以发issue，随缘更新，接受pull request。**




## 功能介绍

- [x] 1. Dota2处刑BOT（Contribute by [zznewclear13](https://github.com/zznewclear13)）
- [x] 2. 趣味黄历（Contribute by [Pinkuburu](https://github.com/Pinkuburu)）
- [x] 3. TRPG骰子（Contribute by [Pinkuburu](https://github.com/Pinkuburu)）

## Windows开发和使用:
单纯的因为菜和懒，插件代码并无直接的release版本以开箱即用，需要自行clone代码修改配置编译后生成。

本项目基于.NET 6 LTS + Visual Studio 2022开发，请自行下载VS2022和.NET 6的环境。

### 一、安装基于Mirai-Api-Http的mirai机器人平台

1. 到 [iTXTech/mcl-installer](https://github.com/iTXTech/mcl-installer) 根据说明下载和安装机器人框架，电脑上找个地方，运行工具后一路无脑回车安装即可<br>
2. 安装 [mirai-api-http插件](https://github.com/project-mirai/mirai-api-http)根据说明下载和安装插件，按照说明安装就可以了，记录下`config/net.mamoe.mirai-api-http/setting.yml`配置文件里的信息，以用于插件配置的修改。<br>
3. 登录QQ机器人，在命令行窗口输入login 你的Q号 Q密码即可，例如：login 10000 password

### 二、生成和使用本项目

1. 参阅GlobalConfig.cs内的说明，修改相关配置<br>
2. 根据自己的需求修改代码，添加功能<br>
3. 编译或生成<br>

**大功告成**




## 命令说明:

1. 【趣味黄历功能】与机器人进行好友私聊，发出命令“.黄历”，即可获得每日趣味黄历占卜，功能复刻了十年前Acfun用户中心的趣味黄历<br>

2. 【TRPG骰子功能】在任意群，输入.roll开头的命令，即可触发TRPG的骰子的功能<br>

   支持的命令格式如下：

   .roll 6d10，意思投掷6枚6面骰子

   .roll 6d10k3，意思投掷6枚6面骰子，但只取最高的三个点数的骰子总和

   .roll 3#6d10，意思投掷6枚6面骰子，连投三轮

   .roll 3#6d10k3，意思投掷6枚6面骰子，连投三轮，每轮只取最高的三个点数的骰子总和

3. 【Dota2处刑BOT】在任意群，输入“.监视”，即可把某个玩家的Dota2酬勤活动往指定的群里发战报（这是在代码里写死的，因为steamAPI有请求上线，所以这个东西也不适合广而告之服务太多的dota玩家）。当群友打完一把比赛后，BOT会向群里发送这句比赛的数据。

   命令的使用方式：<br>
   
   .监视 鸭头肉 321580662
   
   功能只支持官方模式，例如天梯、快速、普通等，游廊的游戏记录不会被解析。
