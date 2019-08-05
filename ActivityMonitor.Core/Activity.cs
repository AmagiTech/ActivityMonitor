using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace ActivityMonitor.Core
{
    public class Activity
    {
        #region HelperMethods
        [DllImport("user32.dll")]
        protected static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        protected static extern IntPtr GetForegroundWindow();

        protected static Process GetActiveProcessFileName(IntPtr hwnd)
        {
            uint pid;
            GetWindowThreadProcessId(hwnd, out pid);
            return Process.GetProcessById((int)pid);
        }
        #endregion

        public int Id { get; set; } = 0;
        public IntPtr Hwnd { get; private set; }
        public string FileName { get; private set; }
        public string ModuleName { get; private set; }
        public string Title { get; private set; }
        public string ProcessName { get; private set; }
        public DateTime ActivationTime { get; private set; }
        private Process m_ActiveProcess { get; set; }

        private Process ActiveProcess {
            get {
                return m_ActiveProcess;
            }
            set {

                if ((m_ActiveProcess == null) ||
                    (value.Id > 0) && (m_ActiveProcess.Id != value.Id))

                {
                    ActivityChanging?.Invoke(this, EventArgs.Empty);
                    m_ActiveProcess = value;
                    ActivationTime = DateTime.Now;
                    Id = value.Id;
                    FileName = value.MainModule.FileName;
                    ModuleName = value.MainModule.ModuleName;
                    Title = value?.MainWindowTitle;
                    ProcessName = value.ProcessName;
                    ActivityChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public static IEnumerable<Activity> Processes =>

           Process.GetProcesses().Where(q => !string.IsNullOrWhiteSpace(q.MainWindowTitle))
           .Select(p => new Activity(p));

        public event Action<Activity, EventArgs> ActivityChanged;
        public event Action<Activity, EventArgs> ActivityChanging;

        public Activity(Process process)
        {
            ActiveProcess = process;
        }

        private static Activity m_CurrentProcess;
        public static Activity CurrentProcess {
            get {
                if (m_CurrentProcess == null)
                {
                    m_CurrentProcess =
                        new Activity(GetActiveProcessFileName(GetForegroundWindow()));
                    Task.Factory.StartNew(() =>
                    {
                        while (true)
                        {
                            var _hwnd = GetForegroundWindow();
                            if (_hwnd != IntPtr.Zero && _hwnd != m_CurrentProcess.Hwnd)
                            {
                                m_CurrentProcess.ActiveProcess = GetActiveProcessFileName(_hwnd);
                                m_CurrentProcess.Hwnd = _hwnd;
                            }
                            Thread.Sleep(500);
                        }
                    });
                }
                return m_CurrentProcess;
            }
        }
    }
}