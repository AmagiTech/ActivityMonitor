namespace ActivityMonitor.ViewModel.Utilities
{
    public class ActivitySettings
    {
        private int m_RefreshTimeOfAllProcessesInSeconds = 2;
        public int RefreshTimeOfAllProcessesInSeconds
        {
            get
            {
                return m_RefreshTimeOfAllProcessesInSeconds;
            }
            set
            {
                if (m_RefreshTimeOfAllProcessesInSeconds != value)
                    m_RefreshTimeOfAllProcessesInSeconds = value;
            }
        }

        private int m_DumpCurrentActivityTimeInSeconds = 5;
        public int DumpCurrentActivityTimeInSeconds
        {
            get
            {
                return m_DumpCurrentActivityTimeInSeconds;
            }
            set
            {
                if (m_DumpCurrentActivityTimeInSeconds != value)
                    m_DumpCurrentActivityTimeInSeconds = value;
            }
        }
    }
}
