using DAL.Model;
using System;
using System.Collections.Generic;

namespace VkMusicSync
{
    public interface ITrackList: IProgressable
    {
        IList<Track> GetList();

        Track GetNext(Track current);

        Track GetPrevious(Track previous);

        void CheckForUpdates();

        event Action<Track, List<Track>> AddedOneTrack;
        event Action<long> DeletedOneTrack;

        event EventHandler SynchronizationCompleted;
        event EventHandler SynchronizationStarted;

        bool IsRandomPlay { get; set; }
    }
}
