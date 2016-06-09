using System;
using Solum.Interfaces;
using System.IO;
using UIKit;
using QuickLook;
using System.Threading.Tasks;
using Foundation;
using Solum.iOS.Handelers;
using Xamarin.Forms;

[assembly: Dependency (typeof(PdfViewer))]
namespace Solum.iOS.Handelers
{
	public class PdfViewer : IPdfViewer
	{
		public void PreviewImage(string path){

			QLPreviewController previewController= new QLPreviewController();             

			previewController.DataSource=new ImageQLPreviewControllerDataSource(path); 

			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(previewController, true, null);

		}

		public void PreviewPdf(MemoryStream stream){

			QLPreviewController previewController = new QLPreviewController();             

			UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(previewController, true, null);

			var dataSource =  SaveFile (stream);

			previewController.DataSource = new ImageQLPreviewControllerDataSource(dataSource); 
		}

		private static string SaveFile(MemoryStream stream) {

			string path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			string filePath = Path.Combine(path, "temp.pdf");
			try
			{
				FileStream fileStream = File.Open(filePath, FileMode.Create);
				stream.Position = 0;
				stream.CopyTo(fileStream);
				fileStream.Flush();
				fileStream.Close();
			}
			catch (Exception)
			{
				filePath = "Não foi possível exibir este arquivo";
			}

			return filePath;
		}
	}

	public class ImageQLPreviewControllerDataSource : QLPreviewControllerDataSource { 

		string path;

		public ImageQLPreviewControllerDataSource(string path){
			this.path = path;
		}

		public override nint PreviewItemCount (QLPreviewController controller) {
			return 1;
		}

		public override IQLPreviewItem GetPreviewItem (QLPreviewController controller, nint index)
		{
			NSUrl url = NSUrl.FromFilename (path);
			return new QlItem ("Relatório", url);
		}
	}

	public class QlItem : QLPreviewItem 
	{ 
		string title; 
		NSUrl uri; 

		public QlItem(string title, NSUrl uri) 
		{ 
			this.title = title; 
			this.uri = uri; 
		} 

		public override string ItemTitle { 
			get { return title; } 
		} 

		public override NSUrl ItemUrl { 
			get { return uri; } 
		} 
	}
}

