using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;
using System.Linq;

namespace RealTimeBusBJ
{
    public partial class App : Application
    {
        /// <summary>
        /// 视图用于进行绑定的静态 ViewModel。
        /// </summary>
        /// <returns>MainViewModel 对象。</returns>

        /// <summary>
        /// 提供对电话应用程序的根框架的轻松访问。
        /// </summary>
        /// <returns>电话应用程序的根框架。</returns>
        public static PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Application 对象的构造函数。
        /// </summary>
        public App()
        {
            // 未捕获的异常的全局处理程序。
            UnhandledException += Application_UnhandledException;

            // 标准 XAML 初始化
            InitializeComponent();

            // 特定于电话的初始化
            InitializePhoneApplication();

            // 语言显示初始化
            InitializeLanguage();

            //// 调试时显示图形分析信息。
            //if (Debugger.IsAttached)
            //{
            //    // 显示当前帧速率计数器
            //    Application.Current.Host.Settings.EnableFrameRateCounter = true;

            //    // 显示在每个帧中重绘的应用程序区域。
            //    //Application.Current.Host.Settings.EnableRedrawRegions = true；

            //    // 启用非生产分析可视化模式，
            //    // 该模式显示递交给 GPU 的包含彩色重叠区的页面区域。
            //    //Application.Current.Host.Settings.EnableCacheVisualization = true；

            //    // 通过禁用以下对象阻止在调试过程中关闭屏幕
            //    // 应用程序的空闲检测。
            //    //  注意: 仅在调试模式下使用此设置。禁用用户空闲检测的应用程序在用户不使用电话时将继续运行
            //    // 并且消耗电池电量。
            //    PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            //}
            using (db.DBContext dbs = new db.DBContext())
            {
                //不存在数据库的时候才创建新数据库，注意，如果修改了数据库的话，需要删除重新创建数据库
                if (dbs.DatabaseExists() == false)
                {
                    dbs.CreateDatabase();
                }
                else
                {
                    try
                    {
                        //1.7.6 加了新表，如果数据库没有这个表，说明是旧数据库，删除重建数据库
                        int ct = dbs.BusStations.Count();
                        ct = dbs.BusLines_ab.Count();
                    }
                    catch
                    {
                        dbs.DeleteDatabase();
                        //System.Diagnostics.Debug.WriteLine("DeleteDatabase");
                        using (db.DBContext dbs2 = new db.DBContext()) { dbs2.CreateDatabase(); }
                        //System.Diagnostics.Debug.WriteLine("CreateDatabase");
                    }
                }
            }
        }

        // 应用程序启动(例如，从“开始”菜单启动)时执行的代码
        // 此代码在重新激活应用程序时不执行
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            //<!-- Green 0,128,0 -->
            //<!-- LawnGreen 124,252,0 -->
            if (((SolidColorBrush)this.Resources["PhoneBackgroundBrush"]).Color == Colors.Black)
            {
                ((SolidColorBrush)this.Resources["titleColor"]).Color = Color.FromArgb(255, 124, 252, 0);
                ((SolidColorBrush)this.Resources["contentColor"]).Color = Colors.LightGray;
            }
            else
            {
                ((SolidColorBrush)this.Resources["titleColor"]).Color = Colors.Green;
                ((SolidColorBrush)this.Resources["contentColor"]).Color = Colors.DarkGray;
            }
            AppGlobalVar.readAllKey();
        }

        // 激活应用程序(置于前台)时执行的代码
        // 此代码在首次启动应用程序时不执行

        // 停用应用程序(发送到后台)时执行的代码
        // 此代码在应用程序关闭时不执行
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // 应用程序关闭(例如，用户点击“后退”)时执行的代码
        // 此代码在停用应用程序时不执行
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            // 确保所需的应用程序状态在此处保持不变。
        }

        // 导航失败时执行的代码
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // 导航已失败；强行进入调试器
                Debugger.Break();
            }
        }

        // 出现未处理的异常时执行的代码
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // 出现未处理的异常；强行进入调试器
                Debugger.Break();
            }
        }

        #region 电话应用程序初始化

        // 避免双重初始化
        private bool phoneApplicationInitialized = false;

        // 请勿向此方法中添加任何其他代码
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // 创建框架但先不将它设置为 RootVisual；这允许初始
            // 屏幕保持活动状态，直到准备呈现应用程序时。
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // 处理导航故障
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // 在下一次导航中处理清除 BackStack 的重置请求，
            RootFrame.Navigated += CheckForResetNavigation;

            // 确保我们未再次初始化
            phoneApplicationInitialized = true;
        }

        // 请勿向此方法中添加任何其他代码
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // 设置根视觉效果以允许应用程序呈现
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // 删除此处理程序，因为不再需要它
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        private void CheckForResetNavigation(object sender, NavigationEventArgs e)
        {
            // 如果应用程序收到“重置”导航，则需要进行检查
            // 以确定是否应重置页面堆栈
            if (e.NavigationMode == NavigationMode.Reset)
                RootFrame.Navigated += ClearBackStackAfterReset;
        }

        private void ClearBackStackAfterReset(object sender, NavigationEventArgs e)
        {
            // 取消注册事件，以便不再调用该事件
            RootFrame.Navigated -= ClearBackStackAfterReset;

            // 只为“新建”(向前)和“刷新”导航清除堆栈
            if (e.NavigationMode != NavigationMode.New && e.NavigationMode != NavigationMode.Refresh)
                return;

            // 为了获得 UI 一致性，请清除整个页面堆栈
            while (RootFrame.RemoveBackEntry() != null)
            {
                ; // 不执行任何操作
            }
        }

        #endregion

        // 初始化应用程序在其本地化资源字符串中定义的字体和排列方向。
        //
        // 若要确保应用程序的字体与受支持的语言相符，并确保
        // 这些语言的 FlowDirection 都采用其传统方向，ResourceLanguage
        // 应该初始化每个 resx 文件中的 ResourceFlowDirection，以便将这些值与以下对象匹配
        // 文件的区域性。例如: 
        //
        // AppResources.es-ES.resx
        //    ResourceLanguage 的值应为“es-ES”
        //    ResourceFlowDirection 的值应为“LeftToRight”
        //
        // AppResources.ar-SA.resx
        //     ResourceLanguage 的值应为“ar-SA”
        //     ResourceFlowDirection 的值应为“RightToLeft”
        //
        // 有关本地化 Windows Phone 应用程序的详细信息，请参见 http://go.microsoft.com/fwlink/?LinkId=262072。
        //
        private void InitializeLanguage()
        {
        }
    }
}