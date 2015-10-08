using DAL.Model;
using System.Collections.Generic;

namespace DAL.Repository.Abstract
{
    public interface ITrackRepository
    {
        void SaveTracks(IList<Track> tracks);

        IList<Track> GetTracks();
    }
}
