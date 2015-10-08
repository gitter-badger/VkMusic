using System;
using System.Windows;
using Microsoft.Practices.Unity;
using VkMusicSync;
using WpfUI;

namespace VkMusic.UI
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        [Dependency]
        public ITrackList MusicWorker { get; set; }

        [Dependency]
        public MusicPlayer MusicPlayer { get; set; }

        [Dependency]
        public AutorunHelper AutorunHelper { get; set; }

        MainWindow mainWindow;

        VkMusic.UI.PlayerWindow playerWindow;

        

        public SettingsWindow()
        {
            InitializeComponent();

            DependencyUtility.BuildUp(this);

            autoRunMenuItem.IsChecked = AutorunHelper.IsAutorunRegistered;

            playerWindow = new VkMusic.UI.PlayerWindow();
            MyNotifyIcon.BeforeShowTrayPopup += MyNotifyIcon_BeforeShowTrayPopup; ;

            MusicWorker.SynchronizationStarted += MusicWorker_SynchronizationStarted;
            MusicWorker.SynchronizationCompleted += MusicWorker_SynchronizationCompleted;

            mainWindow = new MainWindow();

            MusicPlayer.MusicPlayed += () => startPauseItem.Header = "Пауза";
            MusicPlayer.MusicPaused += () => startPauseItem.Header = "Старт";

            playerWindow.Topmost = alwaysOnTopItem.IsChecked = Properties.Settings.Default.AlwaysOnTop;
        }

        private void MyNotifyIcon_BeforeShowTrayPopup(object sender, EventArgs e)
        {
            playerWindow.Show();
            playerWindow.Activate();
        }

        private void MusicWorker_SynchronizationCompleted(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                syncItem.Header = "Синхронизировать";
            });
        }

        private void MusicWorker_SynchronizationStarted(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                syncItem.Header = "Идёт синхронизация...";
            });
        }

        private void quitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void settingsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Show();
        }

        private void autoRunMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AutorunHelper.RegisterUnregisterAutorun();    
        }

        private void syncItem_Click(object sender, RoutedEventArgs e)
        {
            MusicWorker.CheckForUpdates();
            (new ProgressWindow("Синхронизация...", MusicWorker)).Show();
        }

        private void startPauseItem_Click(object sender, RoutedEventArgs e)
        {
            if (MusicPlayer.CurrentlyPlaying)
                MusicPlayer.Pause();
            else
                MusicPlayer.Play();
        }

        private void alwaysOnTopItem_Click(object sender, RoutedEventArgs e)
        {
            playerWindow.Topmost = Properties.Settings.Default.AlwaysOnTop = alwaysOnTopItem.IsChecked;
            Properties.Settings.Default.Save();
        }
    }
}
