using DAL.Model;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DAL;

namespace VkMusicSync
{
    public class TrackList : ITrackList
    {
        private MusicLoader musicLoader;
        private IList<Track> currentTrackList;

        public TrackList(MusicLoader musicLoader)
        {
            this.musicLoader = musicLoader;
            RefreshTrackList();
        }

        public IList<Track> GetList()
        {
            RefreshTrackList();

            return currentTrackList;
        }


        public Track GetNext(Track current)
        {
            var list = currentTrackList.ToList();
            var nextIndex = list.FindIndex(i => i.Equals(current)) + 1;

            if (nextIndex >= list.Count)
                nextIndex = 0;

            return list[nextIndex];
        }

        public Track GetPrevious(Track previous)
        {
            var list = currentTrackList.ToList();
            var nextIndex = list.FindIndex(i => i.Equals(previous)) - 1;

            // TODO Fails if next index < 0 but list.Count == 0
            if (nextIndex < 0 && list.Count != 0)
                nextIndex = list.Count - 1;

            return list[nextIndex];
        }

        private void RefreshTrackList()
        {
            var tracks = musicLoader.GetAllTracks();

            if (!IsRandomPlay || currentTrackList.Count != tracks.Count)
                currentTrackList = tracks;

            if (IsRandomPlay && currentTrackList.ToList().SequenceEqual(tracks))
                currentTrackList = tracks.Shuffle();
        }

        public async void CheckForUpdates()
        {
            await musicLoader.LoadAsync();
        }

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

        

        public bool IsRandomPlay { get; set; }
        

        public event EventHandler SynchronizationCompleted
        {
            add { musicLoader.Completed += value; }
            remove { musicLoader.Completed -= value; }
        }

        public event EventHandler SynchronizationStarted
        {
            add { musicLoader.SynchronizationStarted += value; }
            remove { musicLoader.SynchronizationStarted -= value; }
        }

        public event EventHandler<ProgressStatus> ProgressChanged
        {
            add { musicLoader.ProgressChanged += value; }
            remove { musicLoader.ProgressChanged -= value; }
        }

        public event EventHandler Completed
        {
            add { musicLoader.Completed += value; }
            remove { musicLoader.Completed -= value; }
        }

        public event Action<Track, List<Track>> AddedOneTrack
        {
            add { musicLoader.AddedOneTrack += value; }
            remove { musicLoader.AddedOneTrack -= value; }
        }

        public event Action<long> DeletedOneTrack
        {
            add { musicLoader.DeletedOneTrack += value; }
            remove { musicLoader.DeletedOneTrack -= value; }
        }

    }
}
