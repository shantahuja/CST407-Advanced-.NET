using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PluginBaseLibrary;
using System.IO;
using System.Windows.Media.Imaging;

namespace RedditLibrary
{
    public class RedditPlugin : PluginBase
    {
        private const string URL = @"https://www.reddit.com/r/EarthPorn/top/.json";

        public override Photo[] GetPhotoUrl()
        {
            RedditRootobject data = DeserializeData<RedditRootobject>(GetAPIData(URL));

            return data.data.children.Select(x =>
            {
                BitmapImage image = new BitmapImage(new Uri(x.data.url));
                return new Photo()
                {
                    URL = x.data.url,
                    Metadata = new List<PhotoMetadata>()
                    {
                        new PhotoMetadata()
                        {
                            Title = "Title",
                            Description = x.data.title
                        },
                        new PhotoMetadata()
                        {
                            Title = "Author",
                            Description = x.data.author
                        },
                        new PhotoMetadata()
                        {
                            Title = "Created at",
                            Description = DateTimeOffset.FromUnixTimeSeconds(x.data.created_utc).DateTime.ToString() + " PST"
                        },
                        new PhotoMetadata()
                        {
                            Title = "Upvote count",
                            Description = x.data.ups.ToString()
                        },
                        new PhotoMetadata()
                        {
                            Title = "URL",
                            Description = x.data.url
                        }
                    }
                };
            }).ToArray();
        }
    }
}
