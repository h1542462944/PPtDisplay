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
                Start_Fresh?.Invoke();
                PPts.Clear();
                Inventories_RemoveAllItem?.Invoke();
                //这一层{try}是为了防止异步导致的foreach发生集合已修改而产生的异常.
                try
                {
                    foreach (var folder in App.Inventory.Inventories)
                    {
                        //这一层{try}是为了排除内部错误.
                        try
                        {
                            if (Directory.Exists(folder))
                            {
                                DirectoryInfo d = new DirectoryInfo(folder);

                                foreach (var item in d.GetFiles("*", SearchOption.AllDirectories))
                                {
                                    if ((item.Extension == ".ppt" || item.Extension == ".pptx") && (item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                                    {
                                        if (!PPts.Contains(item.FullName))
                                        {
                                            PPts.Add(item.FullName);
                                            Inventories_AddItem?.Invoke(item.FullName);
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                catch (Exception)
                {
                }
                End_Fresh?.Invoke();
            });
        }
        public static async Task<bool> IsFolderHasPPt(string folder)
        {
            return await Task.Run(() =>
            {

                if (Directory.Exists(folder))
                {
                    DirectoryInfo d = new DirectoryInfo(folder);
                    try
                    {
                        foreach (var item in d.GetFiles("*", SearchOption.AllDirectories))
                        {
                            if ((item.Extension == ".ppt" || item.Extension == ".pptx") && (item.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden)
                            {
                                return true;
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }

                return false;
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
        public static List<string> PPts { get; set; } = new List<string>();
        internal static readonly Inventory Inventory = new Inventory();
        public static PPtWatcher PPtWatcher { get; set; } = new PPtWatcher();
        public static Size ScreenSize => new Size(SystemParameters.PrimaryScreenWidth, SystemParameters.PrimaryScreenHeight);
        public static event Action Inventories_RemoveAllItem;
        public static event Action<string> Inventories_AddItem;
        public static event Action Start_Fresh;
        public static event Action End_Fresh;
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
            if (App.PPtWatcher.Application == null)
            {
                App.GotoMainWindow();
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
