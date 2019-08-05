using ActivityMonitor.Core;
using ActivityMonitor.ViewModel.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ActivityMonitor.ViewModel
{
    public class MainViewModel : ViewModelBase
    {


        ActivitySettings activitySettings;

        public MainViewModel(ActivitySettings activitySettings)
        {
            this.activitySettings = activitySettings;
            CurrentActivityLog = IOUtilities.LoadJson().ToList();
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Application.Current.Dispatcher
                                   .BeginInvoke(new Action(() =>
                                   IOUtilities.WriteToJsonFile(CurrentActivityLog)));
                    Thread.Sleep(this.activitySettings.DumpCurrentActivityTimeInSeconds * 1000);
                }
            });
        }


        private Activity m_CurrentProcess;
        public Activity CurrentProcess
        {
            get
            {
                if (m_CurrentProcess == null)
                {
                    m_CurrentProcess = Activity.CurrentProcess;
                    m_CurrentProcess.ActivityChanged += (s, e) =>
                    {
                        AddCurrentActivityLog(s);
                        NotifyPropertyChanged("CurrentProcess");
                    };
                }
                return m_CurrentProcess;
            }
        }

        private ObservableCollection<Activity> m_AllProcesses;
        public ObservableCollection<Activity> AllProcesses
        {
            get
            {
                if (m_AllProcesses == null)
                {
                    Task.Factory.StartNew(() =>
                    {
                        m_AllProcesses = new ObservableCollection<Activity>();
                        while (true)
                        {
                            Application.Current.Dispatcher
                                     .BeginInvoke(new Action(() =>
                                     this.m_AllProcesses.Clear()));
                            foreach (var p in Activity.Processes)
                            {
                                Application.Current.Dispatcher
                                    .BeginInvoke(new Action(() =>
                                    this.m_AllProcesses.Add(p)));
                            }
                            NotifyPropertyChanged("AllProcesses");
                            Thread.Sleep(this.activitySettings.RefreshTimeOfAllProcessesInSeconds * 1000);
                        }
                    });
                }
                return m_AllProcesses;
            }
        }


        public List<ActivityLog> CurrentActivityLog { get; }

        public void AddCurrentActivityLog(Activity activity)
        {        
            CurrentActivityLog.Add(new ActivityLog(activity, AllProcesses.Select(q => q.ProcessName).ToArray()));
        }

        public void SaveCurrentActivityLogSession() => IOUtilities.WriteToJsonFile(CurrentActivityLog);

    }
}
