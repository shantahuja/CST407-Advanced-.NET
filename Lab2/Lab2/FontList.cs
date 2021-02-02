using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lab2
{
    public class FontList
    {
        public List<FavoriteFont> FavoriteFontList { get; set; } = new List<FavoriteFont>();

        public ObservableCollection<FavoriteFont> DisplayFonts { get; set; } = new ObservableCollection<FavoriteFont>();

        private string searchQuery;

        public string SearchQuery
        {
            get { return searchQuery; }
            set { searchQuery = value; FilterFonts(); }
        }

        private bool filterFavorites;

        public bool FilterFavorites
        {
            get { return filterFavorites; }
            set { filterFavorites = value; FilterFonts(); }
        }

        private bool filterComments;

        public bool FilterComments
        {
            get { return filterComments; }
            set { filterComments = value; FilterFonts(); }
        }


        public void FilterFonts()
        {
            List<FavoriteFont> filtered = FavoriteFontList.Where(x => x.FontInfo.Source.ToLower().Contains(SearchQuery.ToLower())).ToList();

            if (FilterFavorites)
            {
                filtered = filtered.Where(x => x.IsFavorite).ToList();
            }


            if (FilterComments)
            {
                filtered = filtered.Where(x => !string.IsNullOrWhiteSpace(x.Comment)).ToList();
            }

            DisplayFonts.Clear();

            filtered.ForEach(x => DisplayFonts.Add(x));
        }

        public FontList()
        {
            InitializeFonts();

            SearchQuery = "";
        }

        public void GenerateFontsToSerialize(string filePath)
        {
            List<FavoriteFont> fontsToSerialize;

            fontsToSerialize = FavoriteFontList.Where(x => x.IsFavorite || !string.IsNullOrWhiteSpace(x.Comment)).ToList();

            string json = JsonConvert.SerializeObject(fontsToSerialize, Formatting.Indented);

            File.WriteAllText(filePath, json);
        }

        public void LoadSerializedFonts(string filePath)
        {
            string json = File.ReadAllText(filePath);

            List<FavoriteFont> deserializedFonts = JsonConvert.DeserializeObject<List<FavoriteFont>>(json);

            InitializeFonts();

            foreach (FavoriteFont fontInformation in deserializedFonts)
            {
                FavoriteFont fontToUpdate = FavoriteFontList.First(x => x.FontInfo.Source == fontInformation.FontInfo.Source);
                fontToUpdate.Comment = fontInformation.Comment;
                fontToUpdate.IsFavorite = fontInformation.IsFavorite;
            }

            FilterFonts();
        }

        public void InitializeFonts()
        {
            FavoriteFontList.Clear();
            foreach (FontFamily font in Fonts.SystemFontFamilies)
            {
                FavoriteFontList.Add(new FavoriteFont()
                {
                    FontInfo = font
                });
            }
        }
    }
}
