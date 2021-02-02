using Lab2;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Server
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        string GetString();

        [OperationContract]
        void SetString(string set);

        [OperationContract]
        void SendFontData(List<FavoriteFont> fontData);

        [OperationContract]
        List<FavoriteFont> ReceiveFontData();
    }

    [ServiceBehavior(InstanceContextMode=InstanceContextMode.Single)]
    public class WCFServiceContract : IService
    {
        private const string fontDataPath = "C:/Users/shant/source/repos/Labs/Lab5/fontData.json";

        public List<FavoriteFont> favoriteFonts = new List<FavoriteFont>();

        string example;
        public string GetString()
        {
            Console.WriteLine("GetString Accessed!");
            return example;
        }

        public void SetString(string set)
        {
            example = set;
        }

        public void SendFontData(List<FavoriteFont> fontData)
        {
            string json = JsonConvert.SerializeObject(fontData, Formatting.Indented);

            File.WriteAllText(fontDataPath, json);

            favoriteFonts = fontData;
        }

        public List<FavoriteFont> ReceiveFontData()
        {
            return favoriteFonts;
        }

    }
}
