using Microsoft.Win32;

namespace VkMusicSync
{
    // TODO maybe move to UI, VkMusicSync doesn't really need this class
    public class AutorunHelper
    {
        private readonly string _executableFilePath;
        private readonly RegistryKey _runRegistryKey;

        public AutorunHelper(string executableFilePath)
        {
            _executableFilePath = executableFilePath;
            _runRegistryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        }

        
        public void RegisterUnregisterAutorun()
        {
            if (!IsAutorunRegistered)
                _runRegistryKey.SetValue(AppPaths.AppName, _executableFilePath);
            else
                _runRegistryKey.DeleteValue(AppPaths.AppName, false);
        }

        public bool IsAutorunRegistered
        {
            get
            {
                if (_runRegistryKey.GetValue(AppPaths.AppName) == null)
                    return false;

                return _runRegistryKey.GetValue(AppPaths.AppName).ToString() == _executableFilePath;
            }
        }
    }
}
