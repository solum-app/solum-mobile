using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Solum
{
	public partial class SobrePage : ContentPage
	{
		public SobrePage()
		{
			InitializeComponent();

			Task.Run(() => LoadHtml ());
		}

		void LoadHtml(){
			var assembly = typeof(SobrePage).GetTypeInfo().Assembly;
			Stream stream = assembly.GetManifestResourceStream("Solum.Resources.sobre.html");
			var html = "";
			using (var reader = new StreamReader(stream))
			{
				html = reader.ReadToEnd();
			}

			webView.Source = new HtmlWebViewSource()
			{
				Html = html
			};
		}

	}
}

