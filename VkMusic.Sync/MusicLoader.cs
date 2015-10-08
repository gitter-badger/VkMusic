using DAL.Model;
using DAL.Repository.Abstract;
using DAL.Repository.XMLRepository;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DAL;
using VkNet;

namespace VkMusicSync
{
    public class MusicLoader : IProgressable
    {

        string syncDirectory;
        long userId;
        VkApi api;

        ConcurrentSortedList<int, Track> tracks;

        private RemoteAudioSizeHelper remoteAudioSizeHelper;

        [Dependency]
        public ITrackRepository TrackRepository { get; set; }

        #region
        public event Action<Track, List<Track>> AddedOneTrack;
        public event Action<long> DeletedOneTrack;

        public void OnAddedOneTrack(Track track, List<Track> list)
        {
            if (AddedOneTrack != null)
                AddedOneTrack(track, list);
        }

        public void OnDeletedOneTrack(long trackId)
        {
            if (DeletedOneTrack != null)
                DeletedOneTrack(trackId);
        }

        #endregion

        #region event handlers
        public event EventHandler<ProgressStatus> ProgressChanged;
        public event EventHandler Completed;
        public event EventHandler SynchronizationStarted;

        #endregion

        public MusicLoader(string syncDir, string token, long userId)
        {
            DependencyUtility.BuildUp(this);

            syncDirectory = syncDir;
            this.userId = userId;

            api = new VkApi();
            api.Authorize(token, userId);
            api.Invoke("stats.trackVisitor", new Dictionary<string, string>(), true); // статистика посещаемости приложения

            remoteAudioSizeHelper = new RemoteAudioSizeHelper(Path.Combine(AppPaths.SettingsPath, "sizes.dat"));

            tracks = new ConcurrentSortedList<int, Track>();

            int key = 0;
            foreach (var track in TrackRepository.GetTracks())
                tracks.Add(key++, track);
        }

        public bool IsLoading 
            => Monitor.IsEntered(musicLoadLock);

        object musicLoadLock = new object();
        DateTime lastLoadTime;



        public async Task LoadAsync()
        {
            await Task.Factory.StartNew(Load);
        }

        private void Load()
        {

            if (!Monitor.TryEnter(musicLoadLock))
                return;

            OnSynchronizationStarted();

            var audios = api.Audio.Get(userId);

            OnProgressChanged(new ProgressStatus { TotalCount = audios.Count });

            var addedList = new List<Track>();

            var trackIDs = tracks.Values.Select(t => t.Id).ToList();
            var tracksCount = tracks.Count;

            tracks = new ConcurrentSortedList<int, Track>();

            int loadedTracksCount = 0;

            var tracksDictionary = audios.ToDictionary(audio => audio, audio => new Track().Update(audio));

            var trackIndexes = tracksDictionary.Values.Select((s, i) => new { s, i }).ToDictionary(x => x.s, x => x.i);

            Parallel.ForEach(audios, new ParallelOptions { MaxDegreeOfParallelism = 4 }, (audio) =>
            {
                var track = tracksDictionary[audio];

                var fileName = track.GetFilePath();

                var fi = new FileInfo(fileName);
                if (remoteAudioSizeHelper.GetSize(audio) > 0)
                {
                    if (!trackIDs.Contains(audio.Id))
                    {
                        addedList.Add(track);
                    }
                    if (!fi.Exists || fi.Length < remoteAudioSizeHelper.GetSize(audio))
                    {
                        TrySaveFileFromUrl(audio.Url.ToString(), fileName);
                    }

                    tracks.Add(trackIndexes[track], track);

                    OnAddedOneTrack(track, tracks.Values.ToList());

                    Interlocked.Increment(ref loadedTracksCount);

                    OnProgressChanged(new ProgressStatus { TotalCount = audios.Count, DoneCount = loadedTracksCount });
                }

            });

            remoteAudioSizeHelper.Save();

            TrackRepository.SaveTracks(tracks.Values.ToList());
            // TODO do we really need deleted list?
            var deletedList = GetDeletedTrackIDs();

            OnCompleted();

            lastLoadTime = DateTime.UtcNow;

            Monitor.Exit(musicLoadLock);
        }

        // TODO this function really deletes files together with getting their trackids
        private List<long> GetDeletedTrackIDs()
        {
            var deletedList = new List<long>();
            DirectoryInfo di = new DirectoryInfo(syncDirectory);
            var files = di.EnumerateFiles("*.mp3");
            foreach (var file in files)
            {
                try
                {
                    var trackIDstr = file.Name.Substring(0, file.Name.IndexOf(".mp3"));
                    var deletedTrackID = long.Parse(trackIDstr);
                    if (!tracks.Values.Select(t => t.Id).Contains(deletedTrackID))
                    {
                        try
                        {
                            file.Delete();
                            deletedList.Add(deletedTrackID);
                            OnDeletedOneTrack(deletedTrackID);
                        }
                        catch
                        {
                        }
                    }
                }
                catch (FormatException)
                {
                    file.Delete();
                }
            }
            return deletedList;
        }

        public IList<Track> GetAllTracks()
            => tracks.Values.Clone();

        public bool Cancel()
        {
            throw new NotImplementedException();
        }

        public bool Cancelable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private void TrySaveFileFromUrl(string url, string physicalPath)
        {
            try
            {
                SaveFileFromUrl(url, physicalPath);
            }
            catch (WebException ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void SaveFileFromUrl(string url, string physicalPath)
        {
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(url, physicalPath);
            }
        }


        public void OnProgressChanged(ProgressStatus status)
        {
            if (ProgressChanged != null)
                ProgressChanged(this, status);
        }

        public void OnCompleted()
        {
            if (Completed != null)
                Completed(this, EventArgs.Empty);
        }

        public void OnSynchronizationStarted()
        {
            if (SynchronizationStarted != null)
                SynchronizationStarted(this, EventArgs.Empty);
        }




    }

}
