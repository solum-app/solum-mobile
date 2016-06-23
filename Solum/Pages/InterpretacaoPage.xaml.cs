
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
using Solum.Handlers;

namespace Solum.Pages
{
	public partial class InterpretacaoPage : ContentPage
	{
		PdfDocument doc;
		PdfPage page;
		Syncfusion.Drawing.Color green = Syncfusion.Drawing.Color.FromArgb (255, 63, 170, 88);
		Syncfusion.Drawing.Color black = Syncfusion.Drawing.Color.FromArgb(255, 52, 52, 52);
		Syncfusion.Drawing.Color white = Syncfusion.Drawing.Color.FromArgb(255, 255, 255, 255);
		Syncfusion.Drawing.Color gray = Syncfusion.Drawing.Color.FromArgb(255, 155, 155, 155);
		Syncfusion.Drawing.Color grayLight = Syncfusion.Drawing.Color.FromArgb (255, 241, 241, 241);

		Analise analise;

		public InterpretacaoPage (Analise analise, bool isVisualizacao)
		{
			this.analise = analise;
			BindingContext = new InterpretacaoViewModel (Navigation, analise);
			Init (isVisualizacao);
		}

		public InterpretacaoPage (Analise analise, Analise realmAnalise, bool isVisualizacao)
		{
			this.analise = analise;
			BindingContext = new InterpretacaoViewModel (Navigation, analise, realmAnalise);
			Init (isVisualizacao);
		}

		void Init(bool isVisualizacao) {
			InitializeComponent ();

			ToolbarItem item;

			if (isVisualizacao) {
				item = new ToolbarItem {
					Icon = "ic_pdf",
				};
				item.Clicked += OnPdfTapped;
			} else {
				item = new ToolbarItem {
					Icon = "ic_save",
				};
				item.Clicked += OnSalvarTapped;
			}

			this.ToolbarItems.Add (item);
		}

		async void OnSalvarTapped (object sender, EventArgs e)
		{
			var action = await DisplayActionSheet (null, "Cancelar", null, "Salvar", "Salvar e exportar");

			if (action == "Salvar") {
				(BindingContext as InterpretacaoViewModel).SalvarAnalise ();
				await Navigation.PopToRootAsync ();
			} else if (action == "Salvar e exportar") {
				GeneratePdf ();
				(BindingContext as InterpretacaoViewModel).SalvarAnalise ();
				await Navigation.PopToRootAsync ();
			}
		}

		void OnPdfTapped(object sender, EventArgs e)
		{
			GeneratePdf ();
		}

		void GeneratePdf()
		{
			doc = new PdfDocument();
			doc.PageSettings.Margins.All = 0;
			page = doc.Pages.Add();
			PdfGraphics g = page.Graphics;

			var assembly = typeof (App).GetTypeInfo ().Assembly;
			Stream imgStream = assembly.GetManifestResourceStream ("Solum.ic_solum.jpg");
			g.DrawImage (PdfImage.FromStream (imgStream), 20, 20, 72, 72);

			PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
			PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 14);
			PdfFont textFont = new PdfStandardFont (PdfFontFamily.Helvetica, 12);

			//Header 
			g.DrawString ("Relatório de iterpretação de análise de solo", headerFont, new PdfSolidBrush (black), new PointF (110, 20));
			g.DrawString (analise.Fazenda, subHeadingFont, new PdfSolidBrush (black), new PointF (110, 52));
			g.DrawString ("Talhão: " + analise.Talhao, textFont, new PdfSolidBrush (black), new PointF (110, 74));
			g.DrawString (String.Format ("{0:dd/MM/yyyy}", analise.Data), textFont, new PdfSolidBrush (black), new PointF (page.Graphics.ClientSize.Width - 80, 25));
			g.DrawRectangle (new PdfSolidBrush (green), new RectangleF (0, 105, page.Graphics.ClientSize.Width, 5));

			g.DrawString ("Propriedade", subHeadingFont, new PdfSolidBrush (black), new PointF (25, 150));
			g.DrawString ("Valor atual", subHeadingFont, new PdfSolidBrush (black), new PointF (175, 150));
			g.DrawString ("Nível adequado", subHeadingFont, new PdfSolidBrush (black), new PointF (325, 150));
			g.DrawString ("Classe", subHeadingFont, new PdfSolidBrush (black), new PointF (475, 150));


			//pH (CaCl2)
			var valorAtual = (BindingContext as InterpretacaoViewModel).Analise.Ph.ToString ();
			var valorAdequado = "4,9 a 5,5";
			var classe = (BindingContext as InterpretacaoViewModel).InterpretacaoPh;

			var y = BodyContent (g, "pH (CaCl2)", valorAtual, valorAdequado, classe, 180, grayLight);

			//P
			valorAtual = (BindingContext as InterpretacaoViewModel).Analise.P.ToString ();
			valorAdequado = TexturaPConverter ((BindingContext as InterpretacaoViewModel).InterpretacaoTextura);
			classe = (BindingContext as InterpretacaoViewModel).InterpretacaoP;

			y = BodyContent (g, "P", valorAtual, valorAdequado, classe, y, white);

			//Ca
			valorAtual = (BindingContext as InterpretacaoViewModel).Analise.Ca.ToString ();
			valorAdequado = "1,5 a 7,0";
			classe = (BindingContext as InterpretacaoViewModel).InterpretacaoCa;

			y = BodyContent (g, "Ca", valorAtual, valorAdequado, classe, y, grayLight);

			//Mg
			valorAtual = (BindingContext as InterpretacaoViewModel).Analise.Mg.ToString ();
			valorAdequado = "0,5 a 2,0";
			classe = (BindingContext as InterpretacaoViewModel).InterpretacaoMg;

			y = BodyContent (g, "Mg", valorAtual, valorAdequado, classe, y, white);

			//V(%)
			valorAtual = (BindingContext as InterpretacaoViewModel).Analise.V.ToString ();
			valorAdequado = "36 a 60";
			classe = (BindingContext as InterpretacaoViewModel).InterpretacaoV;

			y = BodyContent (g, "V(%)", valorAtual, valorAdequado, classe, y, grayLight);

			//CTC(T)
			valorAtual = (BindingContext as InterpretacaoViewModel).Analise.CTC.ToString ();
			valorAdequado = TexturaCTCConverter ((BindingContext as InterpretacaoViewModel).InterpretacaoTextura);
			classe = (BindingContext as InterpretacaoViewModel).InterpretacaoCtc;

			y = BodyContent (g, "CTC(T)", valorAtual, valorAdequado, classe, y, white);

			//Matéria Orgânica
			valorAtual = (BindingContext as InterpretacaoViewModel).Analise.MateriaOrganica.ToString ();
			valorAdequado = TexturaMoConverter ((BindingContext as InterpretacaoViewModel).InterpretacaoTextura);
			classe = (BindingContext as InterpretacaoViewModel).InterpretacaoMo;

			y = BodyContent (g, "Matéria Orgânica", valorAtual, valorAdequado, classe, y, grayLight);

			//Ca/K
			valorAtual = (BindingContext as InterpretacaoViewModel).Analise.CaK.ToString ();
			valorAdequado = "15 a 25";
			classe = (BindingContext as InterpretacaoViewModel).InterpretacaoCaK;

			y = BodyContent (g, "Ca/K", valorAtual, valorAdequado, classe, y, white);

			//Mg/K
			valorAtual = (BindingContext as InterpretacaoViewModel).Analise.MgK.ToString ();
			valorAdequado = "5 a 15";
			classe = (BindingContext as InterpretacaoViewModel).InterpretacaoMgK;

			y = BodyContent (g, "Mg/K", valorAtual, valorAdequado, classe, y, grayLight);

			//m%
			valorAtual = (BindingContext as InterpretacaoViewModel).Analise.M.ToString ();
			valorAdequado = "-";
			classe = (BindingContext as InterpretacaoViewModel).InterpretacaoM;

			y = BodyContent (g, "m%", valorAtual, valorAdequado, classe, y, white);

			PdfFont footerBoldFont = new PdfStandardFont (PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
			PdfFont footerFont = new PdfStandardFont (PdfFontFamily.Helvetica, 10);


			//Footer
			g.DrawRectangle (new PdfSolidBrush (green), new RectangleF (0, page.Graphics.ClientSize.Height - 80, page.Graphics.ClientSize.Width, 5));
			g.DrawString ("Base de cálculo: Região Centro-Oeste", footerFont, new PdfSolidBrush (black), new PointF (20, page.Graphics.ClientSize.Height - 60));
			g.DrawString ("Relatório gerado pelo aplicativo Solum", footerBoldFont, new PdfSolidBrush (black), new PointF (20, page.Graphics.ClientSize.Height - 40));
			g.DrawString ("Desenvolvido por Sydy Tecnologia", footerFont, new PdfSolidBrush (black), new PointF (page.Graphics.ClientSize.Width - 175, page.Graphics.ClientSize.Height - 60));
			PdfTextWebLink linkAnnot = new PdfTextWebLink ();
			linkAnnot.Url = "http://www.sydy.com.br";
			linkAnnot.Text = "www.sydy.com.br";
			linkAnnot.Font = footerBoldFont;
			linkAnnot.Brush = new PdfSolidBrush (black);
			linkAnnot.DrawTextWebLink (page, new PointF (page.Graphics.ClientSize.Width - 105, page.Graphics.ClientSize.Height - 40));

			MemoryStream stream = new MemoryStream();
			doc.Save(stream);
			doc.Close(true);

			DependencyService.Get<IPdfViewer>().PreviewPdf(stream);
		}

		private float BodyContent (PdfGraphics pg, string propriedade, string valorAtual, string valorAdequado, string classe, float yPosition, Syncfusion.Drawing.Color color)
		{
			PdfFont textFont = new PdfStandardFont (PdfFontFamily.Helvetica, 12);

			pg.DrawRectangle (new PdfSolidBrush (gray), new RectangleF (20, yPosition, page.Graphics.ClientSize.Width - 40, 30));
			pg.DrawRectangle (new PdfSolidBrush (color), new RectangleF (21, yPosition + 1, page.Graphics.ClientSize.Width - 42, 28));

			pg.DrawString (propriedade, textFont, new PdfSolidBrush (black), new PointF (25, yPosition + 8));
			pg.DrawString (valorAtual, textFont, new PdfSolidBrush (black), new PointF (175, yPosition + 8));
			pg.DrawString (valorAdequado, textFont, new PdfSolidBrush (black), new PointF (325, yPosition + 8));
			pg.DrawString (classe, textFont, new PdfSolidBrush (black), new PointF (475, yPosition + 8));

			return yPosition + 29;
		}

		string TexturaPConverter(string textura){
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

		string TexturaCTCConverter(string textura){
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

		string TexturaMoConverter(string textura){
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
}

