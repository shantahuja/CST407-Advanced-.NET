using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Lab2
{
    public static class ExtensionMethod
    {
        public static FontServiceReference.FavoriteFont ConvertTo(this FavoriteFont favoriteFont)
        {
            FontServiceReference.FavoriteFont fontServiceReference = new FontServiceReference.FavoriteFont();

            fontServiceReference.Comment = favoriteFont.Comment;

            fontServiceReference.IsFavorite = favoriteFont.IsFavorite;

            fontServiceReference.FontName = favoriteFont.FontName;

            return fontServiceReference;
        }

        public static FavoriteFont ConvertBack(this FontServiceReference.FavoriteFont favoriteFont)
        {
            FavoriteFont fontServiceReference = new FavoriteFont();

            fontServiceReference.Comment = favoriteFont.Comment;

            fontServiceReference.IsFavorite = favoriteFont.IsFavorite;

            fontServiceReference.FontName = favoriteFont.FontName;

            fontServiceReference.FontInfo = Fonts.SystemFontFamilies.FirstOrDefault(x => x.Source == fontServiceReference.FontName);

            return fontServiceReference;
        }
    }
}
