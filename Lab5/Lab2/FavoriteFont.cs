using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lab2
{
    [DataContract]
    public class FavoriteFont
    {
        [IgnoreDataMember]
        public FontFamily FontInfo { get; set; }

        [DataMember]
        public bool IsFavorite { get; set; }

        [DataMember]
        public String Comment { get; set; }

        [DataMember]
        public string FontName { get; set; }

    }
}
