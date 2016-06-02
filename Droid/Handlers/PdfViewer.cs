using System;
using Solum.Interfaces;
using Java.IO;
using Android.Content;
using Xamarin.Forms;
using Solum.Droid.Handlers;

[assembly: Dependency (typeof(PdfViewer))]
namespace Solum.Droid.Handlers
{
	public class PdfViewer : IPdfViewer
	{
		public void PreviewPdf (System.IO.MemoryStream stream)
		{
			string exception = string.Empty;
			string root = null;

			if (Android.OS.Environment.IsExternalStorageEmulated)
			{
				root = Android.OS.Environment.ExternalStorageDirectory.ToString();
			}
			else
				root = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			Java.IO.File myDir = new Java.IO.File(root + "/Solum");
			myDir.Mkdir();

			Java.IO.File file = new Java.IO.File(myDir, "temp.pdf");

			if (file.Exists()) 
				file.Delete();

			try
			{
				FileOutputStream outs = new FileOutputStream(file);
				outs.Write(stream.ToArray());

				outs.Flush();
				outs.Close();
			}
			catch (Exception e)
			{
				exception = e.ToString();
			}

			if (file.Exists())
			{
				Android.Net.Uri path = Android.Net.Uri.FromFile(file);
				Intent intent = new Intent(Intent.ActionView);
				intent.SetDataAndType(path, "application/pdf");
				Forms.Context.StartActivity(Intent.CreateChooser(intent, "Escolha um App"));
			}
		}
	}
}

