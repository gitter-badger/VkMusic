using DAL.Model;
using DAL.Repository.Abstract;
using System.Collections.Generic;

namespace DAL.Repository.XMLRepository
{
    public class TrackRepository : BaseXMLRepository<Track>, ITrackRepository
    {
        public TrackRepository(string repositoryPath)
            : base(repositoryPath)
        {
        }

        public void SaveTracks(IList<Track> tracks)
            => SaveCollection(tracks);
        
        public IList<Track> GetTracks()
            => GetCollection();
        
    }
}
