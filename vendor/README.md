# 随仓库提供的运行依赖

- `runtime`：从本项目现有部署中整理的 x64 CAD 运行库、Krypton、表格库、OpenCV 原生及托管库、CAD 插件和字体。
- `templates`：默认启动、创建图纸和读取回退所用模板。
- `licenses`：现有依赖包中可取得的许可证文本。
- `manifest.json`：逐文件大小与 SHA256，启动准备时验证。

未复制授权密钥、机器信息、分析中间结果、业务输出或旧版应用 EXE。应用快照在 `app`，由当前源码重新构建。

OpenCV 原生库选用原部署 `dll/x64` 中的版本，不采用重复的 x86 文件。原有 NuGet 元数据保留在工程中作版本记录，实际构建不依赖在线还原。

Teigha 原代码版权声明：Teigha .NET for .dwg files 2009–2010 by Open Design Alliance. All rights reserved. 各第三方组件继续按原有许可证使用；本目录不是对其授予新的许可。

不要将运行生成的文件放入本目录。需要更新第三方版本时单独提交、记录来源并重新验证 manifest、构建和启动。
