using Newtonsoft.Json;
using PluginBaseLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BingLibrary
{
    public class BingPlugin : PluginBase
    {
        private const string URL = @"https://www.bing.com/HPImageArchive.aspx?format=js&idx=0&n=8&mkt=en-US";

        public override Photo[] GetPhotoUrl()
        {
            BingData data = DeserializeData<BingData>(GetAPIData(URL));

            return data.images.Select(x =>
            {
                BitmapImage image = new BitmapImage(new Uri(("http://www.bing.com" + x.url)));
                return new Photo()
                {
                    URL = "http://www.bing.com" + x.url,
                    Metadata = new List<PhotoMetadata>()
                    {
                        new PhotoMetadata()
                        {
                            Title = "Title",
                            Description = x.title
                        },
                        new PhotoMetadata()
                        {
                            Title = "Copyright",
                            Description = x.copyright
                        },
                        new PhotoMetadata()
                        {
                            Title = "URL",
                            Description = "http://www.bing.com" + x.url
                        }
                    }
                };
            }).ToArray();
        }
    }
}
