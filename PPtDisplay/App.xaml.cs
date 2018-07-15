using PPtDisplay.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PPtDisplay
{
    /// <summary>
    /// App.xaml的交互逻辑.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 以异步方式加载文件夹相关内容.
        /// </summary>
        /// <returns></returns>
        public static async Task LoadStorageAsync()
        {
            await Task.Run(() =>
            {
                if (!IsStorageLoaded)
                {
                    if (!Directory.Exists(LocalCache))
                    {
                        Directory.CreateDirectory(LocalCache);
                    }
                    if (!Directory.Exists(DataCache))
                    {
                        Directory.CreateDirectory(DataCache);
                    }
                }
                IsStorageLoaded = true;
            });

        }
        /// <summary>
        /// 从文件中以异步方式加载设置.
        /// </summary>
        /// <returns></returns>
        public static async Task LoadWindowSettingsAsync()
        {
            await Window.Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    Window.XSerializer.DeSerialize();
                }
                catch (Exception)
                {
                    Window.IsFullScreen = false;
                }
            });
        }
        public static async Task SaveWindowSettingsAsync()
        {
            await Task.Run(() => Window.XSerializer.Serialize());
        }
        public static async Task LoadUserSettingsAsync()
        {

            await Task.Run(() =>
            {
                foreach (var folder in App.Inventory.Inventorys)
                {
                    if (Directory.Exists(folder))
                    {
                        DirectoryInfo d = new DirectoryInfo(folder);
                        foreach (var item in d.GetFiles("*", SearchOption.AllDirectories))
                        {
                            if ((item.Extension == ".ppt" || item.Extension == ".pptx") && (item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                            {
                                PPts.Add(item.FullName);
                            }
                        }
                    }
                }
            });
        }
        public static async Task SaveUserSettingsAsync()
        {
            await Inventory.SaveAsync();
        }
        public static bool IsStorageLoaded { get; private set; } = false;
        public static string LocalCache => AppDomain.CurrentDomain.BaseDirectory + @"LocalCache\";
        /// <summary>
        /// 获取程序的主窗体.
        /// </summary>
        public static MainWindow Window { get; set; }
        public static DisplayWindow DisplayWindow { get; set; } 
        public static string WindowSettingsPath => LocalCache + "WindowSettings.xml";
        public static string DataCache => LocalCache + @"Data\";
        public static List<string> PPts { get; set; } = new List<string>();
        internal static readonly Inventory Inventory = new Inventory();
        public static PPtWatcher PPtWatcher { get; set; } = new PPtWatcher();
        public static Size ScreenSize => new Size(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);

        static App()
        {
            PPtWatcher.PPtApplictionChanged += PPtWatcher_PPtApplictionChanged;
            PPtWatcher.PPtPresentationChanged += PPtWatcher_PPtPresentationChanged;
        }

        private static void PPtWatcher_PPtPresentationChanged(object sender, PPtPresentationChangedEventArgs e)
        {
            if (e.IsActivate == true)
            {
                App.GotoDisplayWindow();
                App.PPtWatcher.Display();
            }
        }
        private static void PPtWatcher_PPtApplictionChanged(object sender, EventArgs e)
        {
            if (App.PPtWatcher.Application== null)
            {
                App.PPtWatcher.ExitDisplay();
            }
        }
        /// <summary>
        /// 转到<see cref="PPtDisplay.DisplayWindow"/>并开始放映.
        /// </summary>
        public static void GotoDisplayWindow()
        {
            App.Window.Dispatcher.Invoke(() => 
            {
                if (Window.Visibility == Visibility.Visible)
                {
                    Window.Hide();
                    if (DisplayWindow != null)
                    {
                        DisplayWindow.Close();
                    }
                    DisplayWindow = new DisplayWindow();
                    DisplayWindow.Show();
                }   
            });
        }
        /// <summary>
        /// 转到<see cref="MainWindow"/>并开始放映.
        /// </summary>
        public static void GotoMainWindow()
        {
            if (Window.Visibility != Visibility.Visible)
            {
                DisplayWindow.Close();
                Window.Show();
            }
        }
    }
}
