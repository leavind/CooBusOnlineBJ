using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Resources;

//1.2.0 增加地铁路线图的缩放功能。 更新北京交委的路线数据源，不再从爱帮获取，提升APP稳定性。
//1.1.4 修复闪退和917等路线打不开的bug。 路线信息有更新，请手动下载。
//1.1.1 修复查看地铁路线的的闪退。
//1.1.0 增加线路数据。包含酷米客及北京交委官方的数据。路线数量超过安卓和IOS。但是问题挺多，需要时间改进！
[assembly: AssemblyTitle("RealTimeBus")]
[assembly: AssemblyDescription("RealTimeBus")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("RealTimeBus")]
[assembly: AssemblyProduct("RealTimeBus")]
[assembly: AssemblyCopyright("Copyright © Leavind 2014")]
[assembly: AssemblyTrademark("RealTimeBus")]
[assembly: AssemblyCulture("")]

// 将 ComVisible 设置为 false 会使此程序集中的类型
// COM 组件不可见。  如果需要从 COM 访问此程序集中的某个类型，
// 则将该类型上的 ComVisible 特性设置为 true。
[assembly: ComVisible(false)]

// 如果此项目向 COM 公开，则下列 GUID 用于类型库的 ID
[assembly: Guid("51366e23-0d70-424c-b554-d3828be91004")]

// 程序集的版本信息由下列四个值组成: 
//
//      主版本
//     次版本
//      内部版本号
//      修订号
//
// 可以指定所有这些值，也可以使用“修订号”和“内部版本号”的默认值，
// 方法是按如下方式使用“*”: 
[assembly: AssemblyVersion("1.2.0.0")]
[assembly: AssemblyFileVersion("1.2.0.0")]
[assembly: NeutralResourcesLanguageAttribute("zh-CN")]
