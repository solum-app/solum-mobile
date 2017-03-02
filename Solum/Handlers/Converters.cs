using System;
using System.Globalization;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.Handlers
{
    public class IverseBoolConverter : IValueConverter
    {
        public static IverseBoolConverter Instance = new IverseBoolConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool) value;
        }
    }

    public class ToUpperConverter : IValueConverter
    {
        public static ToUpperConverter Instance = new ToUpperConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            return text?.ToUpper();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = value as string;
            return text?.ToLowerInvariant();
        }
    }

    public class TexturaPValueConverter : IValueConverter
    {
        public static TexturaPValueConverter Instance = new TexturaPValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textura = (Textura) value;

            switch (textura)
            {
                case Textura.Arenosa:
                    return "18,01 a 25,00";
                case Textura.Media:
                    return "15,01 a 20,00";
                case Textura.Argilosa:
                    return "8,01 a 12,00";
                case Textura.MuitoArgilosa:
                    return "4,01 a 6,00";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textura = (Textura) value;

            switch (textura)
            {
                case Textura.Arenosa:
                    return "18,01 a 25,00";
                case Textura.Media:
                    return "15,01 a 20,00";
                case Textura.Argilosa:
                    return "8,01 a 12,00";
                case Textura.MuitoArgilosa:
                    return "4,01 a 6,00";
                default:
                    return "";
            }
        }
    }

    public class CtcKValueConverter : IValueConverter
    {
        public static CtcKValueConverter Instance = new CtcKValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ctc = (float) value;

            if (ctc < 4)
                return "30,01 a 40,00";
            return "50,01 a 80,00";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ctc = (float) value;

            if (ctc < 4)
                return "30,01 a 40,00";
            return "50,01 a 80,00";
        }
    }

    public class TexturaCtcValueConverter : IValueConverter
    {
        public static TexturaCtcValueConverter Instance = new TexturaCtcValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textura = (Textura) value;

            switch (textura)
            {
                case Textura.Arenosa:
                    return "4,01 a 6,00";
                case Textura.Media:
                    return "6,01 a 9,00";
                case Textura.Argilosa:
                    return "9,01 a 13,50";
                case Textura.MuitoArgilosa:
                    return "12,01 a 18,00";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textura = (Textura) value;

            switch (textura)
            {
                case Textura.Arenosa:
                    return "4,01 a 6,00";
                case Textura.Media:
                    return "6,01 a 9,00";
                case Textura.Argilosa:
                    return "9,01 a 13,50";
                case Textura.MuitoArgilosa:
                    return "12,01 a 18,00";
                default:
                    return "";
            }
        }
    }

    public class TexturaMoValueConverter : IValueConverter
    {
        public static TexturaMoValueConverter Instance = new TexturaMoValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textura = (Textura) value;

            switch (textura)
            {
                case Textura.Arenosa:
                    return "10,01 a 15,00";
                case Textura.Media:
                    return "20,01 a 30,00";
                case Textura.Argilosa:
                    return "30,01 a 45,00";
                case Textura.MuitoArgilosa:
                    return "35,01 a 52,00";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var textura = (Textura) value;

            switch (textura)
            {
                case Textura.Arenosa:
                    return "10,01 a 15,00";
                case Textura.Media:
                    return "20,01 a 30,00";
                case Textura.Argilosa:
                    return "30,01 a 45,00";
                case Textura.MuitoArgilosa:
                    return "35,01 a 52,00";
                default:
                    return "";
            }
        }
    }

    public class PhCorValueConverter : IValueConverter
    {
        public static PhCorValueConverter Instance = new PhCorValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Nivel nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Alto:
                    return "#E57373";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Baixo:
                    return "#81D4FA";
                case Nivel.MuitoBaixo:
                    return "#CE93D8";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Alto:
                    return "#E57373";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Baixo:
                    return "#81D4FA";
                case Nivel.MuitoBaixo:
                    return "#CE93D8";
                default:
                    return "#000000";
            }
        }
    }

    public class PCorValueConverter : IValueConverter
    {
        public static PCorValueConverter Instance = new PCorValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.MuitoBaixo:
                    return "#E57373";
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.MuitoBaixo:
                    return "#E57373";
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }
    }

    public class KCorValueConverter : IValueConverter
    {
        public static KCorValueConverter Instance = new KCorValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }
    }

    public class CaMgCorValueConverter : IValueConverter
    {
        public static KCorValueConverter Instance = new KCorValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }
    }

    public class CaMgKCorValueConverter : IValueConverter
    {
        public static CaMgKCorValueConverter Instance = new CaMgKCorValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }
    }

    public class CtcCorValueConverter : IValueConverter
    {
        public static CtcCorValueConverter Instance = new CtcCorValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }
    }

    public class MoCorValueConverter : IValueConverter
    {
        public static MoCorValueConverter Instance = new MoCorValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#FFCC80";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                default:
                    return "#000000";
            }
        }
    }

    public class VCorValueConverter : IValueConverter
    {
        public static VCorValueConverter Instance = new VCorValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#E57373";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                case Nivel.MuitoAlto:
                    return "#CE93D8";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#E57373";
                case Nivel.Medio:
                    return "#FFF59D";
                case Nivel.Adequado:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                case Nivel.MuitoAlto:
                    return "#CE93D8";
                default:
                    return "#000000";
            }
        }
    }

    public class MCorValueConverter : IValueConverter
    {
        public static MCorValueConverter Instance = new MCorValueConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                case Nivel.MuitoAlto:
                    return "#CE93D8";
                default:
                    return "#000000";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var nivel = (Nivel) value;

            switch (nivel)
            {
                case Nivel.Baixo:
                    return "#81C784";
                case Nivel.Alto:
                    return "#81D4FA";
                case Nivel.MuitoAlto:
                    return "#CE93D8";
                default:
                    return "#000000";
            }
        }
    }
}