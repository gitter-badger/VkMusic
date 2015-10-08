using DAL.Repository.Abstract;
using DAL.Repository.XMLRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using VkMusicSync;

namespace WpfUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        MediaPlayer mediaPlayer = new MediaPlayer();

        DispatcherTimer timer = new DispatcherTimer();

        ITrackList musicWorker;

        MusicPlayer musicPlayer;

        MusicLoader musicLoader;

        private string tokenID;
        private long userID;

        Mutex single = new Mutex(true, "VkMusicSync");
        Mutex mutexShow = new Mutex(false, "VkMusicSync-ShowPlayer");

        DispatcherTimer fastTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1), IsEnabled = true };

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            if (!single.WaitOne(0, true))
            {
                if (mutexShow.WaitOne(10000))
                {
                    Thread.Sleep(1000);
                    mutexShow.ReleaseMutex();
                }
                this.Shutdown();
                return;
            }



            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            var appId = 4159456;

            var authWindow = new AuthorizeWindow(appId);

            authWindow.ShowDialog();

            if (authWindow.UserID <= 0)
                Current.Shutdown(1488);

            tokenID = authWindow.TokenID;
            userID = authWindow.UserID;

            Prepare();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.ExceptionObject.ToString());
        }

        private void Prepare()
        {

            var directory = Directory.CreateDirectory(AppPaths.AppPath);

            

            musicLoader = new MusicLoader(directory.FullName, tokenID, userID);

            DependencyUtility.RegisterInstance(musicLoader);
            DependencyUtility.RegisterType<MusicPlayer>();

            DependencyUtility.RegisterInstance(new AutorunHelper(System.Reflection.Assembly.GetExecutingAssembly().Location));

            var progressWindow = new ProgressWindow("Синхронизация", musicLoader);
            progressWindow.Show();



            musicWorker = DependencyUtility.Resolve<TrackList>();

            musicPlayer = DependencyUtility.Resolve<MusicPlayer>();
            musicLoader.LoadAsync().ContinueWith((t) => Dispatcher.Invoke(firstSynchronizationCompleted));

            var settingsWindow = new SettingsWindow();

            fastTimer.Tick += (s, e) =>
            {
                if (!mutexShow.WaitOne(0, true))
                    musicPlayer.InvokeShow();
                else
                    mutexShow.ReleaseMutex();
            };
        }

        void firstSynchronizationCompleted()
        {
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            musicWorker.CheckForUpdates();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            WpfUI.Properties.Settings.Default.Save();
            try
            {
                single.ReleaseMutex();
            }
            catch
            { }
            base.OnExit(e);
        }


    }
}
