using PluginBaseLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace LocalLibrary
{
    public class LocalPlugin : PluginBase
    {
        private const string path = @"C:\Users\shant\OneDrive\Pictures\lion designs";

        FileInfo fileInfo = new FileInfo(path);
        public override Photo[] GetPhotoUrl()
        {
            string[] files = Directory.GetFiles(path, "*.jpg");

            return files.Select(x =>
            {
                BitmapImage image = new BitmapImage(new Uri(path + @"\" + Path.GetFileName(x)));
                return new Photo()
                {
                    URL = x,
                    Metadata = new List<PhotoMetadata>()
                    {
                        new PhotoMetadata()
                        {
                            Title = "Filename",
                            Description = Path.GetFileName(x)
                        },
                        new PhotoMetadata()
                        {
                            Title = "Created at",
                            Description = fileInfo.CreationTime.ToString()
                        },
                        new PhotoMetadata()
                        {
                            Title = "Filepath",
                            Description = path + @"\" + Path.GetFileName(x)
                        },
                        new PhotoMetadata()
                        {
                            Title = "Height",
                            Description = image.Height.ToString()
                        },
                        new PhotoMetadata()
                        {
                            Title = "Width",
                            Description = image.Width.ToString()
                        }
                    }
                };
            }).ToArray();
        }
    }
}
