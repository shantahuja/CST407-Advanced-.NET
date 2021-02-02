using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PluginBaseLibrary
{
    public class PhotoMetadata
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }

    public class Photo
    {
        public Photo[] PhotoArray { get; set; }
        public string URL { get; set; }

        public List<PhotoMetadata> Metadata { get; set; }
    }

    public abstract class PluginBase
    {
        protected virtual string GetAPIData(string url)
        {
            using (WebClient client = new WebClient())
            {
                string json = client.DownloadString(url);

                return json;
            }
        }

        protected T DeserializeData<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public abstract Photo[] GetPhotoUrl();

        public Photo[] Photos
        {
            get
            {
                return GetPhotoUrl();
            }
        }
    }
}
