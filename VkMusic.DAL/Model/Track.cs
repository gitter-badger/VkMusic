using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DAL.Model
{
    [Serializable()]
    public class Track: IEquatable<Track>, ICloneable
    {
        [XmlAttribute("TrackID", DataType = "long")]
        public long Id { get; set; }

        [DefaultValue(-1)]  // looks like a hack
        [XmlAttribute("TrackOwnerID", DataType = "long")]
        public long OwnerId { get; set; }

        [XmlAttribute("TrackArtist", DataType = "string")]
        public string Artist { get; set; }

        [XmlAttribute("TrackTitle", DataType = "string")]
        public string Title { get; set; }

        [XmlAttribute("TrackDuration", DataType = "int")]
        public int Duration { get; set; }

        [XmlAttribute("CreatedDateTime", DataType = "date")]
        public DateTime CreatedDateTime { get; set; }

        public bool Equals(Track other)
        {
            if (other == null)
                return false;
            return other.Id == Id;
        }

        public object Clone()
            => MemberwiseClone();

    }
}
