using Newtonsoft.Json;
using PluginBaseLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PetFinderLibrary
{
    public class AccessToken
    {
        public string BearerToken { get; set; }
        public DateTime Expiration { get; set; }
    }
    public class PetFinderPlugin : PluginBase
    {
        private const string AccessTokenPath = "token.json";
        private string GetBearerToken()
        {

            // open a webclient
            // add properties to webclient for the form to be posted to the URL privded
            // encapsulate the response in a string 
            // - the response will be an object that contains an "access_token" parameter

            // first case: no token saved, get token
            // second case: token is saved, but expired. get token
            // third case: token saved, not expired. do not get token, return the old token.

            if (!File.Exists(AccessTokenPath))
            {
                AccessToken newToken = GenerateBearerToken();
                return newToken.BearerToken;
            }

            else 
            {
                string accessToken = File.ReadAllText(AccessTokenPath);

                //             settings = JsonConvert.DeserializeObject<AppSettings>(json);

                AccessToken tokenObject = JsonConvert.DeserializeObject<AccessToken>(accessToken);

                if (DateTime.Now > tokenObject.Expiration)
                {
                    AccessToken newToken = GenerateBearerToken();
                    return newToken.BearerToken;
                }
                else
                {
                    return tokenObject.BearerToken;
                }
            }
        }

        private AccessToken GenerateBearerToken()
        {
            PetFinderAuthorization response;

            using (WebClient client = new WebClient())
            {
                const string authorizationUrl = "https://api.petfinder.com/v2/oauth2/token";
                client.Headers.Add(HttpRequestHeader.ContentType, "application/x-www-form-urlencoded");
                System.Collections.Specialized.NameValueCollection parameters = new System.Collections.Specialized.NameValueCollection();
                parameters.Add("grant_type", "client_credentials");
                parameters.Add("client_id", "7ZWH39bKGmiPUvjjgxHNNhbcWWjHDMvUdWUdt3jfHCwLPhX5FH");
                parameters.Add("client_secret", "ifrFLGoWEggOBs24yTqav8y8GPSmWSWPiIEF7pFC");
                //            BingData data = DeserializeData<BingData>(GetAPIData(URL));
                string authorizationResponse = Encoding.UTF8.GetString(client.UploadValues(authorizationUrl, parameters));
                response = DeserializeData<PetFinderAuthorization>(authorizationResponse);
            }

            DateTime expiration = DateTime.Now.AddSeconds(response.expires_in);
            string accessToken = response.access_token;

            AccessToken temporaryToken = new AccessToken();

            temporaryToken.BearerToken = accessToken;
            temporaryToken.Expiration = expiration;


            //string json = JsonConvert.SerializeObject(settings, Formatting.Indented);

            //File.WriteAllText(settings.AppSettingsPath, json);

            string tokenObject = JsonConvert.SerializeObject(temporaryToken, Formatting.Indented);

            File.WriteAllText(AccessTokenPath, tokenObject);

            return temporaryToken;
        }

        protected override string GetAPIData(string url)
        {
            //const string url = @"http://cst407.azurewebsites.net/Lab6/GetMissionDetails";
            //WebClient webClient = new WebClient();
            //string responsejson = webClient.DownloadString(url + "?id=" + EmpID);
            // Bearer eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiJ9.eyJhdWQiOiI3WldIMzliS0dtaVBVdmpqZ3hITk5oYmNXV2pIRE12VWRXVWR0M2pmSEN3TFBoWDVGSCIsImp0aSI6IjViNTc5NmFlNDQyOWE0YTVjNDNiZmEzMmU4OTcxYjE5OTlkYmE1ZGUwN2YzODhjODM0NDUxMDg4MjYxNWU1Y2VkNDIxOWU4MzI5Y2RmNGZhIiwiaWF0IjoxNTkwNjk3ODY2LCJuYmYiOjE1OTA2OTc4NjYsImV4cCI6MTU5MDcwMTQ2Niwic3ViIjoiIiwic2NvcGVzIjpbXX0.A1fWVYX5IS24z4QrFH0Z_EDDubjjfqaaXfGQfJ68Oos74LM5r0Lrf0yPASE1IpOhFiYMUPbvPynIgWGEFfSdiZecj5t-MJWbtoTHx13trRdiY3ab6aKc2jWRsyXKziLp5ifwqoFRWkMSLx2FT9FdVn0Ybo28Z2odR92WgTbJKt8blh9UAv4xCRwoBxnqHRt3osKLrtsRTZsOJeMYPKGIgoTTsdyCkD3slVFOZH3X4M8cMvA8ll_R5PudEocA-m8v9Wo5y3XW2t8H7Gjvtcx1s1e1EoAQB8QIpZig0NMS98W_Q3t79wIWj7TG1HBay6E0XBPbfbpuRjCjGqdXOh0MsA
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("Authorization", "Bearer " + GetBearerToken());
                string json = client.DownloadString(url);

                return json;
            }
        }

        private const string URL = @"https://api.petfinder.com/v2/animals?type=dog";

        public override Photo[] GetPhotoUrl()
        {
            PetFinderRootobject data = DeserializeData<PetFinderRootobject>(GetAPIData(URL));


            List<Photo> photoListObject = new List<Photo>();

            foreach (PetFinderAnimal animal in data.animals.Where(x => x.photos.Length > 0))
            {
                Photo tempPhoto = new Photo();
                List<Photo> photoList = new List<Photo>();



                foreach (PetFinderPhoto photo in animal.photos)
                {
                    Photo tempPhotoMany = new Photo();
                    tempPhotoMany.URL = photo.full;
                    photoList.Add(tempPhotoMany);

                }
                tempPhoto.URL = animal.photos.FirstOrDefault()?.full;
                tempPhoto.PhotoArray = photoList.ToArray();
                tempPhoto.Metadata = new List<PhotoMetadata>()
                {
                    new PhotoMetadata()
                    {
                        Title = "Species",
                        Description = animal.species
                    },
                    new PhotoMetadata()
                    {
                        Title = "Primary Breed",
                        Description = animal.breeds.primary
                    },
                    new PhotoMetadata()
                    {
                        Title = "Age",
                        Description = animal.age
                    },
                    new PhotoMetadata()
                    {
                        Title = "Gender",
                        Description = animal.gender
                    },
                    new PhotoMetadata()
                    {
                        Title = "URL",
                        Description = animal.url
                    }
                };

                photoListObject.Add(tempPhoto);
            }
            return photoListObject.ToArray();
        }
    }
}
