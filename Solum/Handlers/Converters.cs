﻿using System;
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
			}
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
			}
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
				return "11 a 15";
			case "Média":
				return "21 a 30";
			case "Argilosa":
				return "31 a 45";
			case "Muito argilosa":
				return "36 a 52";
			default:
				return "";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var textura = (value as string);

			switch (textura) {
			case "Arenosa":
				return "11 a 15";
			case "Média":
				return "21 a 30";
			case "Argilosa":
				return "31 a 45";
			case "Muito argilosa":
				return "36 a 52";
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
			var nivel = (value as string);

			switch (nivel) {
			case "Acidez alta":
				return "#F44336";
			case "Acidez média":
				return "#FFF176";
			case "Acidez adequada":
				return "#8BC34A";
			case "Acidez baixa":
				return "#4DD0E1";
			case "Acidez muito baixa":
				return "#0097A7";
			default:
				return "#000000";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Acidez alta":
				return "#F44336";
			case "Acidez média":
				return "#FFF176";
			case "Acidez adequada":
				return "#8BC34A";
			case "Acidez baixa":
				return "#4DD0E1";
			case "Acidez muito baixa":
				return "#0097A7";
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
			var nivel = (value as string);

			switch (nivel) {
			case "Muito baixo":
				return "#F44336";
			case "Baixo":
				return "#FF9800";
			case "Médio":
				return "#FFF176";
			case "Adequado":
				return "#8BC34A";
			case "Alto":
				return "#4DD0E1";
			default:
				return "#000000";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Muito baixo":
				return "#F44336";
			case "Baixo":
				return "#FF9800";
			case "Médio":
				return "#FFF176";
			case "Adequado":
				return "#8BC34A";
			case "Alto":
				return "#4DD0E1";
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
			var nivel = (value as string);

			switch (nivel) {
			case "Baixo":
				return "#FF9800";
			case "Médio":
				return "#FFF176";
			case "Adequado":
				return "#8BC34A";
			case "Alto":
				return "#4DD0E1";
			default:
				return "#000000";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Baixo":
				return "#FF9800";
			case "Médio":
				return "#FFF176";
			case "Adequado":
				return "#8BC34A";
			case "Alto":
				return "#4DD0E1";
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
			var nivel = (value as string);

			switch (nivel) {
			case "Baixo":
				return "#FF9800";
			case "Adequado":
				return "#8BC34A";
			case "Alto":
				return "#4DD0E1";
			default:
				return "#000000";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Baixo":
				return "#FF9800";
			case "Adequado":
				return "#8BC34A";
			case "Alto":
				return "#4DD0E1";
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
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#FF9800";
			case "Média":
				return "#FFF176";
			case "Adequada":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
			default:
				return "#000000";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#FF9800";
			case "Média":
				return "#FFF176";
			case "Adequada":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
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
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#FF9800";
			case "Média":
				return "#FFF176";
			case "Adequada":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
			default:
				return "#000000";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#FF9800";
			case "Média":
				return "#FFF176";
			case "Adequada":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
			default:
				return "";
			}
		}
	}

	public class MoCorValueConverter : IValueConverter
	{
		public static MoCorValueConverter Instance = new MoCorValueConverter();
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#FF9800";
			case "Média":
				return "#FFF176";
			case "Adequada":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
			default:
				return "#000000";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#FF9800";
			case "Média":
				return "#FFF176";
			case "Adequada":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
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
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#F44336";
			case "Média":
				return "#FFF176";
			case "Adequada":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
			case "Muito alta":
				return "#0097A7";
			default:
				return "#000000";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#F44336";
			case "Média":
				return "#FFF176";
			case "Adequada":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
			case "Muito alta":
				return "#0097A7";
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
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
			case "Muito alta":
				return "#0097A7";
			default:
				return "#000000";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var nivel = (value as string);

			switch (nivel) {
			case "Baixa":
				return "#8BC34A";
			case "Alta":
				return "#4DD0E1";
			case "Muito alta":
				return "#0097A7";
			default:
				return "#000000";
			}
		}
	}
}

