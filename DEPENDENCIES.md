# 开发依赖准备

仓库保存项目源码、工程文件和协作文档。CAD 原图、授权文件、机器信息、构建结果和本地运行库不上传。

首次克隆后，请向项目负责人获取经过验证的依赖包和环境说明；单独克隆源码并不能保证立即构建运行。

## 需要准备的内容

- 与当前工程兼容的 Visual Studio、.NET 桌面开发组件和目标框架支持。现有应用目标为 .NET Framework v4.0。
- 按现有 `packages.config` 和 `.csproj` 引用准备 NuGet 包，期望目录为 `Teigha_Learn_Cad/packages`。缺失包需逐项核实，不直接升级全部版本。
- 从负责人提供的授权来源准备 `Teigha_Learn_Cad_Bin` 中的运行依赖，尤其是 Teigha、Krypton 和相关原生库。该目录也被工程通过相对路径引用，不能随意移动。
- 先构建 `ClassLibrary1` 并核实主程序引用的 `ClassLibrary1/bin/Debug/ClassLibrary1.dll`。
- 按授权流程在本机准备软件授权。不要把 `Licence.key`、`ComputerInfo.key` 或其他私密文件提交到仓库。
- CAD 测试样本和参考 Excel 通过约定的团队渠道分发，原始输入保持只读，结果输出到独立目录。

解决方案含 `Setup2.vdproj` 安装项目。首次上手可先构建应用及其依赖，安装包构建另行准备相应支持。

负责人尚需填写：Visual Studio 版本、实际构建配置、运行库位数、依赖获取位置、准确构建/启动命令和统一样例操作。上述内容未在本轮完成构建验证。
