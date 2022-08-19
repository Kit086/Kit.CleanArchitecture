 **This repository is no longer maintained, please move to [Kit086/CleanArchitecture](https://github.com/Kit086/CleanArchitecture)**
 
 <img align="left" width="116" height="116" src="https://raw.githubusercontent.com/Kit086/Kit.CleanArchitecture/main/.github/kiticon.png" />
 
 # Clean Architecture Solution - Razor Pages Template


<br/>

这是一个 AspNetCore Razor Pages 的简洁架构解决方案模板（Clean Architecture Solution Template），fork 自 [jasontaylordev/CleanArchitecture](https://github.com/jasontaylordev/CleanArchitecture)。原项目是 Angular 的，但是我一般不使用 Angular 等前端框架和类库，所以改造了一个 Razor Pages 的模板。

## 了解简洁架构

[Clean Architecture with ASP.NET Core 3.0 • Jason Taylor • GOTO 2019](https://www.youtube.com/watch?v=dK4Yb6-LxAk)

这是原作者 Jason Taylor 在 GOTO Conferences 上关于 AspNetCore 3.0 简洁架构的演讲，虽然 AspNetCore 3.0 已经老了（但是很多公司连 dotnet core 都还没用上），但是不妨碍我们借助 dotnet 代码理解简洁架构。

顺便说一下，还有另一个优秀的简洁架构模板：[ardalis/CleanArchitecture](https://github.com/ardalis/CleanArchitecture)，作者也录制了很多课程来讲解简洁架构。该模板使用 Razor Pages，没有直接选择它是因为我第一次没有听懂 ardalis 的英语（我不熟悉英澳口音）。

## 与原模板的区别

1. 前端使用 Razor Pages，原模板使用 Angular (SPA)
2. 开发环境使用 Sqlite，原模板使用 InMemoryDB：我不喜欢 InMemoryDB，我喜欢能看得到的真正的数据库
3. 生产环境使用 MariaDB 10.6，原模板使用 SQL Server：我负担不起 SQL Server，用 docker 跑一个 Express 版本的都限制最小 2GB 内存，服务器费用太贵
4. 业务参数校验（需要查数据库来确定某个字段是否唯一时）使用非异步的方法，原模板使用异步方法：Razor Pages 使用 FluentValidation 参数校验时 `MustAsync()` 抛异常，暂时改为 `Must()`
5. 授权认证使用 Microsoft.AspNetCore.Identity，原模板使用 Microsoft.AspNetCore.ApiAuthorization.IdentityServer：这个库很神奇，依赖了很多第三方库，其中某个库使用了旧版本的 AutoMapper，让人怀疑它是否快要被放弃了。而且这个 Razor Pages 的模板也不需要为 Web API 做授权

## TODO

以下是我计划在未来完成的开发（我可能要出去打工了，遥遥无期）：
1. 补充完测试代码
2. 测试库从 NUnit + Moq 改为 XUnit + NSubstitite：因为我习惯使用后者
3. 支持 MinimalAPI：因为很多人有 Razor Pages + Web API 的需求

## 技术

* [ASP.NET Core 6](https://docs.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-6.0)
* [Entity Framework Core 6](https://docs.microsoft.com/en-us/ef/core/)
* AspNetCore Razor Pages
* [MediatR](https://github.com/jbogard/MediatR)
* [AutoMapper](https://automapper.org/)
* [FluentValidation](https://fluentvalidation.net/)
* [NUnit](https://nunit.org/), [FluentAssertions](https://fluentassertions.com/), [Moq](https://github.com/moq) & [Respawn](https://github.com/jbogard/Respawn)

## 开始使用

1. 将本代码库 clone 到本地
2. 在命令行中 cd 到有 .sln 文件的目录下，运行 `dotnet new --install .\`
3. 创建一个你自己的项目文件夹，cd 到该文件夹下，运行命令 `dotnet new ca-sln-rp` 创建基于该模板的解决方案
4. 数据库配置：
   - 想暂时使用 Sqlite 的话，cd 到 `src\RazorPages` 目录下，运行命令 `dotnet ef database update` 来创建 Sqlite 数据库文件
   - 想使用 MariaDB 10.6 的话，首先配置好 appsettings.json 中的 DefaultConnection 数据库连接字符串，然后将 appsettings.json 中 UseSqlite 的值改为 false，删除 `src\Infrastructure\Persistence\Migrations` 文件夹，cd 到 `src\Infrastructure` 目录下，运行 `dotnet ef migrations add Init --startup-project ..\src\RazorPages --output-dir .\Persistence\Migrations` 生成数据库迁移文件，然后 cd 到 `src\RazorPages` 目录下，运行命令 `dotnet ef database update` 来创建 Sqlite 数据库文件
5. cd 到 `src\RazorPages` 目录下，运行 `dotnet run` 命令即可运行起项目

默认用户名为 `administrator@localhost`

默认密码为 `Administrator1!`

## 概览

### Domain

这一层包含所有特定于领域层的实体、枚举、异常、接口、类型和逻辑。 

### Application

这一层包含所有应用程序逻辑。它依赖于 Domain 层，但不依赖于任何其他层或项目。这一层定义了由外部的层实现的接口。例如如果应用程序需要访问通知服务，那么应该在 Application 层添加一个新接口，并在 Infrastructure 层中创建一个实现。 

### Infrastructure

这一层包含用于访问外部资源的类，如文件系统、web 服务、smtp 等。这些类应该基于在 Application 层中定义的接口。 

### Razor Pages

这一层是一个基于 AspNetCore 的 Razor Pages 应用。这一层同时依赖于 Application 层和 Infrastructure 层，但对 Infrastructure 层的依赖只是为了依赖注入。

## 许可证

该项目使用 [MIT license](LICENSE).
