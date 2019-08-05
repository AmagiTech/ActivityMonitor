using ActivityMonitor.ViewModel;
using ActivityMonitor.ViewModel.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ActivityMonitor.View
{
    public partial class MainView : Window
    {
        MainViewModel model;
        private readonly NotifyIcon ni;
        public MainView()
        {
            InitializeComponent();
            var settings = new ActivitySettings()
            {
                DumpCurrentActivityTimeInSeconds = Properties.Settings.Default.DumpCurrentActivityTimeInSeconds,

                RefreshTimeOfAllProcessesInSeconds = Properties.Settings.Default.RefreshTimeOfAllProcessesInSeconds
            };
            model = new MainViewModel(settings);
            this.DataContext = model;
            ni = new System.Windows.Forms.NotifyIcon();
            CreateTrayMenu();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            WindowState = WindowState.Minimized;
            base.OnClosing(e);
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                this.Hide();
            base.OnStateChanged(e);
        }

        void CreateTrayMenu()
        {

            ni.Icon = new System.Drawing.Icon("Assets/monitoring.ico");
            ni.Visible = true;
            ni.DoubleClick += (e, s) =>
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            };
            ni.ContextMenu = new System.Windows.Forms.ContextMenu();
            ni.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Show", (e, s) =>
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }));
            ni.ContextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Exit", (e, s) =>
            {
                model.AddCurrentActivityLog(new Core.Activity(Process.GetCurrentProcess()));
                model.SaveCurrentActivityLogSession();
                
                System.Windows.Application.Current.Shutdown();
            }));
        }
    }
}
