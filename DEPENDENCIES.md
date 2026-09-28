# 开发依赖准备

仓库现已包含整理后的运行依赖、启动模板和从当前源码构建的程序快照。正常运行无需安装 Visual Studio，也无需恢复 NuGet 包。

克隆后双击根目录 `启动软件.cmd`；修改源码后双击 `编译并启动.cmd`。具体操作见 README。

## 需要准备的内容

- 64 位 Windows、.NET Framework 4.x 运行环境，以及 Microsoft Visual C++ 2008 SP1 Redistributable（x64）。启动脚本检测 VC90 运行环境，缺失时显示官方下载地址。该系统环境首次安装可能需要管理员操作。
- 只有编译源码时才需要 Visual Studio / Build Tools 和 .NET 桌面编译组件。工程继续使用 .NET Framework v4.0，因此还需要对应目标框架引用程序集；本机验证使用 Visual Studio 2022 Build Tools MSBuild 和已安装的 v4.0 引用程序集。
- 当前引用直接指向 `vendor/runtime`，不再依赖旧部署目录、NuGet 缓存或本机 `ClassLibrary1/bin/Debug`。`ClassLibrary1` 已改为项目引用。
- Teigha 托管库为 AMD64，应用与 CAD 业务库固定编译为 x64。解决方案中的 AnyCPU 是已有配置名称，不表示最终程序为 32 位。
- `vendor/templates` 包含默认打开和新建图纸所需模板；两类启动方式都自动复制到运行目录。
- `app` 保存已验证的应用快照，`vendor/manifest.json` 与 `app/manifest.json` 记录 SHA256，启动时检查文件完整性。
- `Licence.key`、`ComputerInfo.key` 等本机文件继续排除上传。启动自检没有复制这些文件，授权代码没有改动。
- 二批 CAD 业务图纸和参考 Excel 仍通过团队渠道提供；默认启动模板不代表二批图纸业务验收通过。

解决方案含 `Setup2.vdproj` 安装项目。首次上手可先构建应用及其依赖，安装包构建另行准备相应支持。

`scripts/Build.ps1` 直接构建主应用及项目引用，绕过安装项目。Debug 与 Release 均使用相同的运行依赖；本次验证配置为 Debug。

## 运行库来源

`vendor/runtime` 来源于本项目已有部署目录；OpenCV x64 原生库使用已有 `dll/x64` 的对应文件。未升级第三方库。原始目录保留在本机，但不再是构建或启动的依赖。

SHA256 与本地可用许可证文本见 `vendor`。第三方运行库继续适用其原有许可证，不因纳入仓库而改变。

Microsoft VC2008 SP1 x64 系统运行环境下载：[微软官方页面](https://www.microsoft.com/en-us/download/details.aspx?id=26368)。
