using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace ActivityMonitor.View
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        MainView mainView;

        protected override void OnStartup(StartupEventArgs e)
        {
            CloseIfBeforeStarted();
            mainView = new MainView();
        }

        void CloseIfBeforeStarted()
        {
            var ps = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName)
                .OrderBy(q => q.StartTime);
            if (ps.Count() > 1)
            {
                SetForegroundWindow(ps.FirstOrDefault().MainWindowHandle);
                Shutdown();
            }

        }

       
    }
}
