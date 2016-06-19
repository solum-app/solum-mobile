
using Xamarin.Forms;
using Solum.Models;
using Solum.ViewModel;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Drawing;
using Syncfusion.Pdf.Interactive;
using System.IO;
using Solum.Interfaces;
using System.Reflection;
using System;

namespace Solum.Pages
{
	public partial class InterpretacaoPage : ContentPage
	{
		PdfDocument doc;
		PdfPage page;
		Syncfusion.Drawing.Color headerColor = Syncfusion.Drawing.Color.FromArgb(255, 71, 201, 173);

		Syncfusion.Drawing.Color black = Syncfusion.Drawing.Color.FromArgb(255, 0, 0, 0);
		Syncfusion.Drawing.Color white = Syncfusion.Drawing.Color.FromArgb(255, 255, 255, 255);
		Syncfusion.Drawing.Color violet = Syncfusion.Drawing.Color.FromArgb(255, 151, 108, 174);

		public InterpretacaoPage (Analise analise)
		{
			BindingContext = new InterpretacaoViewModel (Navigation, analise);
			InitializeComponent ();

			var item = new ToolbarItem ("Salvar", "ic_save", ()=> GeneratePdf());
			this.ToolbarItems.Add (item);
			var item2 = new ToolbarItem("PDF", "ic_pdf", () => GeneratePdf());
			this.ToolbarItems.Add(item2);
		}

		void GeneratePdf()
		{
			doc = new PdfDocument();
			doc.PageSettings.Margins.All = 0;
			page = doc.Pages.Add();
			PdfGraphics g = page.Graphics;

			PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 35);
			PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16);
			g.DrawRectangle(new PdfSolidBrush(headerColor), new RectangleF(0, 0, page.Graphics.ClientSize.Width, page.Graphics.ClientSize.Height));
			g.DrawRectangle(new PdfSolidBrush(white), new RectangleF(0, 100, page.Graphics.ClientSize.Width, page.Graphics.ClientSize.Height - 150));
			g.DrawString("Enterprise", headerFont, new PdfSolidBrush(violet), new PointF(10, 20));
			g.DrawString("Reporting Solutions", subHeadingFont, new PdfSolidBrush(black), new PointF(15, 70));

			g.DrawString(String.Format("{0:dd/MM/yyyy}", DateTime.Today), subHeadingFont, new PdfSolidBrush(white), new PointF(page.Graphics.ClientSize.Width - 100, 20));

//			PdfLayoutResult result = HeaderPoints("Develop cloud-ready reporting applications in as little as 20% of the time.", 15);
//			result = HeaderPoints("Proven, reliable platform thousands of users over the past 10 years.", result.Bounds.Bottom + 15);
//			result = HeaderPoints("Microsoft Excel, Word, Adobe PDF, RDL display and editing.", result.Bounds.Bottom + 15);
//			result = HeaderPoints("Why start from scratch? Rely on our dependable solution frameworks", result.Bounds.Bottom + 15);

			PdfLayoutResult result = BodyContent("Deployment-ready framework tailored to your needs.", 100);
			result = BodyContent("Our architects and developers have years of reporting experience.", result.Bounds.Bottom + 25);
			result = BodyContent("Solutions available for web, desktop, and mobile applications.", result.Bounds.Bottom + 25);
			result = BodyContent("Backed by our end-to-end product maintenance infrastructure.", result.Bounds.Bottom + 25);
			result = BodyContent("The quickest path from concept to delivery.", result.Bounds.Bottom + 25);

			PdfPen redPen = new PdfPen(PdfBrushes.Red, 2);
			g.DrawLine(redPen, new PointF(40, result.Bounds.Bottom + 92), new PointF(40, result.Bounds.Bottom + 145));
			float headerBulletsXposition = 40;
			var txtElement = new PdfTextElement("The Experts");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
			txtElement.Draw(page, new RectangleF(headerBulletsXposition + 5, result.Bounds.Bottom + 90, 450, 200));

			PdfPen violetPen = new PdfPen(PdfBrushes.Violet, 2);
			g.DrawLine(violetPen, new PointF(headerBulletsXposition + 280, result.Bounds.Bottom + 92), new PointF(headerBulletsXposition + 280, result.Bounds.Bottom + 145));
			txtElement = new PdfTextElement("Accurate Estimates");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
			result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 290, result.Bounds.Bottom + 90, 450, 200));

			txtElement = new PdfTextElement("A substantial number of .NET reporting applications use our frameworks");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
			result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 5, result.Bounds.Bottom + 5, 250, 200));


			txtElement = new PdfTextElement("Given our expertise, you can expect estimates to be accurate.");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
			result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 290, result.Bounds.Y, 250, 200));


			PdfPen greenPen = new PdfPen(PdfBrushes.Green, 2);
			g.DrawLine(greenPen, new PointF(40, result.Bounds.Bottom + 32), new PointF(40, result.Bounds.Bottom + 85));

			txtElement = new PdfTextElement("Product Licensing");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
			txtElement.Draw(page, new RectangleF(headerBulletsXposition + 5, result.Bounds.Bottom + 30, 450, 200));

			PdfPen bluePen = new PdfPen(PdfBrushes.Blue, 2);
			g.DrawLine(bluePen, new PointF(headerBulletsXposition + 280, result.Bounds.Bottom + 32), new PointF(headerBulletsXposition + 280, result.Bounds.Bottom + 85));
			txtElement = new PdfTextElement("About Syncfusion");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 20);
			result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 290, result.Bounds.Bottom + 30, 450, 200));

			txtElement = new PdfTextElement("Solution packages can be combined with product licensing for great cost savings.");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
			result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 5, result.Bounds.Bottom + 5, 250, 200));


			txtElement = new PdfTextElement("Syncfusion has more than 7,000 customers including large financial institutions and Fortune 100 companies.");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11, PdfFontStyle.Regular);
			result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 290, result.Bounds.Y, 250, 200));

			var assembly = typeof(App).GetTypeInfo().Assembly;
			Stream imgStream = assembly.GetManifestResourceStream("Solum.teste.jpg");
			
			g.DrawImage(PdfImage.FromStream(imgStream), 280, 600, 300, 170);

			g.DrawString("All trademarks mentioned belong to their owners.", new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Italic), PdfBrushes.White, new PointF(10, g.ClientSize.Height - 30));
			PdfTextWebLink linkAnnot = new PdfTextWebLink();
			linkAnnot.Url = "http://www.sydy.com.br";
			linkAnnot.Text = "www.sydy.com.br";
			linkAnnot.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 8, PdfFontStyle.Italic);
			linkAnnot.Brush = PdfBrushes.White;
			linkAnnot.DrawTextWebLink(page, new PointF(g.ClientSize.Width - 100, g.ClientSize.Height - 30));
			MemoryStream stream = new MemoryStream();
			doc.Save(stream);
			doc.Close(true);

			DependencyService.Get<IPdfViewer>().PreviewPdf(stream);
		}

		private PdfLayoutResult BodyContent(string text, float yPosition)
		{
			float headerBulletsXposition = 35;
			PdfTextElement txtElement = new PdfTextElement("3");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.ZapfDingbats, 16);
			txtElement.Brush = new PdfSolidBrush(violet);
			txtElement.StringFormat = new PdfStringFormat();
			txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
			txtElement.StringFormat.LineLimit = true;
			txtElement.Draw(page, new RectangleF(headerBulletsXposition, yPosition, 320, 100));

			txtElement = new PdfTextElement(text);
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 17);
			txtElement.Brush = new PdfSolidBrush(white);
			txtElement.StringFormat = new PdfStringFormat();
			txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
			txtElement.StringFormat.LineLimit = true;
			PdfLayoutResult result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 25, yPosition, 450, 130));
			return result;
		}

		private PdfLayoutResult HeaderPoints(string text, float yPosition)
		{
			float headerBulletsXposition = 220;
			PdfTextElement txtElement = new PdfTextElement("l");
			txtElement.Font = new PdfStandardFont(PdfFontFamily.ZapfDingbats, 10);
			txtElement.Brush = new PdfSolidBrush(violet);
			txtElement.StringFormat = new PdfStringFormat();
			txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
			txtElement.StringFormat.LineLimit = true;
			txtElement.Draw(page, new RectangleF(headerBulletsXposition, yPosition, 300, 100));

			txtElement = new PdfTextElement(text);
			txtElement.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 11);
			txtElement.Brush = new PdfSolidBrush(white);
			txtElement.StringFormat = new PdfStringFormat();
			txtElement.StringFormat.WordWrap = PdfWordWrapType.Word;
			txtElement.StringFormat.LineLimit = true;
			PdfLayoutResult result = txtElement.Draw(page, new RectangleF(headerBulletsXposition + 20, yPosition, 320, 100));
			return result;
		}
	}
}

