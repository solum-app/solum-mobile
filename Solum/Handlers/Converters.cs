using System;
using System.Globalization;
using Xamarin.Forms;

namespace Solum.Handlers
{
	public class TexturaPValueConverter : IValueConverter
	{
		public static TexturaPValueConverter Instance = new TexturaPValueConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var textura = (value as string);

			switch (textura) {
			case "Arenosa":
				return "18,1 a 25";
			case "Média":
				return "15,1 a 20";
			case "Argilosa":
				return "8,1 a 12";
			case "Muito argilosa":
				return "4,1 a 6";
			default:
				return "";
				break;
			}

			return value == null ? string.Empty : value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var textura = (value as string);

			switch (textura) {
			case "Arenosa":
				return "18,1 a 25";
			case "Média":
				return "15,1 a 20";
			case "Argilosa":
				return "8,1 a 12";
			case "Muito argilosa":
				return "4,1 a 6";
			default:
				return "";
				break;
			}
		}
	}

	public class CtcKValueConverter : IValueConverter
	{
		public static CtcKValueConverter Instance = new CtcKValueConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var ctc = (float)value;

			if (ctc < 4) {
				return "31 a 40";
			} else {
				return "51 a 80";
			}

			return value == null ? string.Empty : value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var ctc = (float)value;

			if (ctc < 4) {
				return "31 a 40";
			} else {
				return "51 a 80";
			}
		}
	}

	public class TexturaCtcValueConverter : IValueConverter
	{
		public static TexturaCtcValueConverter Instance = new TexturaCtcValueConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var textura = (value as string);

			switch (textura) {
			case "Arenosa":
				return "4,1 a 6,0";
			case "Média":
				return "6,1 a 9,0";
			case "Argilosa":
				return "9,1 a 13,5";
			case "Muito argilosa":
				return "12,1 a 18,0";
			default:
				return "";
				break;
			}

			return value == null ? string.Empty : value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var textura = (value as string);

			switch (textura) {
			case "Arenosa":
				return "4,1 a 6,0";
			case "Média":
				return "6,1 a 9,0";
			case "Argilosa":
				return "9,1 a 13,5";
			case "Muito argilosa":
				return "12,1 a 18,0";
			default:
				return "";
				break;
			}
		}
	}

	public class TexturaMoValueConverter : IValueConverter
	{
		public static TexturaMoValueConverter Instance = new TexturaMoValueConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var textura = (value as string);

			switch (textura) {
			case "Arenosa":
				return "1,1 a 1,5";
			case "Média":
				return "2,1 a 3,0";
			case "Argilosa":
				return "3,1 a 4,5";
			case "Muito argilosa":
				return "3,6 a 5,2";
			default:
				return "";
				break;
			}

			return value == null ? string.Empty : value.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var textura = (value as string);

			switch (textura) {
			case "Arenosa":
				return "1,1 a 1,5";
			case "Média":
				return "2,1 a 3,0";
			case "Argilosa":
				return "3,1 a 4,5";
			case "Muito argilosa":
				return "3,6 a 5,2";
			default:
				return "";
				break;
			}
		}
	}
}

