using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Solum.Pages
{
	public partial class SobrePage : ContentPage
	{
		public SobrePage ()
		{
			InitializeComponent ();

			LoadHtml ();
		}

		async Task LoadHtml ()
		{
			var html = "";

			try {
				var assembly = typeof (SobrePage).GetTypeInfo ().Assembly;
				Stream stream = assembly.GetManifestResourceStream ("Solum.Resources.sobre.html");

				using (var reader = new StreamReader (stream)) {
					html = await reader.ReadToEndAsync ();
				}

				Device.BeginInvokeOnMainThread (() => {
					webView.Source = new HtmlWebViewSource () {
						Html = html
					};
				});

			} catch (Exception) {
				
			}
		}
	}
}

