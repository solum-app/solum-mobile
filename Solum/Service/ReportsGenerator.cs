using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Color = Syncfusion.Drawing.Color;

namespace Solum.Service
{
    public class ReportsGenerator
    {
		private readonly Color _black = Color.FromArgb(255, 52, 52, 52);
		private readonly Color _gray = Color.FromArgb(255, 155, 155, 155);
		private readonly Color _grayLight = Color.FromArgb(255, 241, 241, 241);
		private readonly Color _green = Color.FromArgb(255, 63, 170, 88);
		private readonly Color _white = Color.FromArgb(255, 255, 255, 255);

        public async Task<MemoryStream> GeneratePDFReport(Analise analise) {
			
			var document = new PdfDocument();
            var page = new PdfPage();

			var infos = document.DocumentInformation;
			infos.Author = "Sydy Tecnologia";
			infos.CreationDate = DateTime.Now;
			infos.Creator = "Solum";
            infos.Title = $"Análise {analise.Identificacao}";
			infos.Subject = "Relatório";

			var prefs = document.ViewerPreferences;
			prefs.PageMode = PdfPageMode.UseAttachments;
			document.PageSettings.Margins.All = 0;
            page = document.Pages.Add();
            var g = page.Graphics;

			PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);

            await DrawHeader(g, "Relatório de Interpretação da Analise", page, analise);

            var argila = analise.Argila;
            var silte = analise.Silte;
			var textura = Interpretador.Textura(argila, silte);
			g.DrawString("Textura: " + textura, subHeadingFont, new PdfSolidBrush(_black), new PointF(25, 130));
			g.DrawString("Propriedade", subHeadingFont, new PdfSolidBrush(_black), new PointF(25, 160));
			g.DrawString("Valor atual", subHeadingFont, new PdfSolidBrush(_black), new PointF(175, 160));
			g.DrawString("Nível adequado", subHeadingFont, new PdfSolidBrush(_black), new PointF(325, 160));
			g.DrawString("Classe", subHeadingFont, new PdfSolidBrush(_black), new PointF(475, 160));


			//pH
            var valorAtual = analise.PotencialHidrogenico.ToString("F", CultureInfo.InvariantCulture);
			var valorAdequado = "4,81 a 5,50";
            var classe = NivelConverter(Interpretador.NivelPotencialHidrogenico(analise.PotencialHidrogenico));
            var y = BodyContent(g, "pH (CaCl2)", valorAtual, valorAdequado, classe, 180, _grayLight, page);

			//Fósforo
            valorAtual = analise.Fosforo.ToString("F", CultureInfo.InvariantCulture);
			valorAdequado = TexturaPConverter(textura);
            classe = NivelConverter(Interpretador.NiveFosforo(analise.Fosforo, textura));

            y = BodyContent(g, "P", valorAtual, valorAdequado, classe, y, _white, page);

			//K
            valorAtual = analise.Potassio.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = CtcKConverter(analise.CTC);
            classe = NivelConverter(Interpretador.NivelPotassio(analise.Potassio, analise.CTC));

            y = BodyContent(g, "K", valorAtual, valorAdequado, classe, y, _grayLight, page);

			//Ca
            valorAtual = analise.Calcio.ToString("F", CultureInfo.InvariantCulture);
			valorAdequado = "1,50 a 7,00";
            classe = NivelConverter(Interpretador.NivelCalcio(analise.Calcio));

            y = BodyContent(g, "Ca", valorAtual, valorAdequado, classe, y, _white, page);

			//Mg
            valorAtual = analise.Magnesio.ToString("F", CultureInfo.InvariantCulture);
			valorAdequado = "0,50 a 2,00";
            classe = NivelConverter(Interpretador.NivelMagnesio(analise.Magnesio));

            y = BodyContent(g, "Mg", valorAtual, valorAdequado, classe, y, _grayLight, page);

			//M.O.
            valorAtual = analise.MateriaOrganica.ToString("F", CultureInfo.InvariantCulture);
			valorAdequado = TexturaMoConverter(textura);
            classe = NivelConverter(Interpretador.NivelMateriaOrganica(analise.MateriaOrganica, textura));

            y = BodyContent(g, "M.O.", valorAtual, valorAdequado, classe, y, _white, page);

			//CTC(T)
            valorAtual = analise.CTC.ToString("F", CultureInfo.InvariantCulture);
			valorAdequado = TexturaCTCConverter(textura);
            classe = NivelConverter(Interpretador.NivelCtc(analise.CTC, textura));

            y = BodyContent(g, "CTC(T)", valorAtual, valorAdequado, classe, y, _grayLight, page);

			//V(%)
            valorAtual = analise.V.ToString("F", CultureInfo.InvariantCulture) + "%";
			valorAdequado = "35,01 a 60,00";
            classe = NivelConverter(Interpretador.NivelV(analise.V));

            y = BodyContent(g, "V(%)", valorAtual, valorAdequado, classe, y, _white, page);

			//m(%)
            valorAtual = analise.M.ToString("F", CultureInfo.InvariantCulture) + "%";
			valorAdequado = "Baixo";
            classe = NivelConverter(Interpretador.NivelM(analise.M));

            y = BodyContent(g, "m%", valorAtual, valorAdequado, classe, y, _grayLight, page);

			//Ca/K
            valorAtual = analise.CaK.ToString("F", CultureInfo.InvariantCulture);
			valorAdequado = "14,01 a 25,00";
            classe = NivelConverter(Interpretador.NivelCalcioPotassio(analise.CaK));

            y = BodyContent(g, "Ca/K", valorAtual, valorAdequado, classe, y, _white, page);

			//Mg/K
            valorAtual = analise.MgK.ToString("F", CultureInfo.InvariantCulture);
			valorAdequado = "4,01 a 15,00";
            classe = NivelConverter(Interpretador.NivelMagnesioPotassio(analise.MgK));

            y = BodyContent(g, "Mg/K", valorAtual, valorAdequado, classe, y, _grayLight, page);

			PdfFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);
			//Texto
			g.DrawString(
				"Fonte das tabelas: Sousa, D.M.G. de; Lobato, E. Cerrado – Correção do solo e adubação. 2ª ed. (2004).\n" +
				"pH (CaCl2) – solução de Cloreto de Cálcio 0,01 M na proporção 1:2,5\n" +
				"P e K (mg/dm³) - Extrator: solução Mehlich 1 (HCl 0,05 N e H2SO4 0,025 N)\n" +
				"Ca, Mg e Al (cmolc/dm³) – Extrator: solução de Cloreto de Potássio 1N (KCl) \n" +
				"H (cmolc/dm³) – Extrator: Solução de Acetato de Cálcio\n" +
				"M.O. (Matéria Orgânica) (g/dm³) – Extrator: Oxidação com Bicromato de Potássio e determinação colorimétrica \n" +
				"Areia, Silte e Argila (g/kg) – Extrator: dispersante NaOH e determinação por densímetro\n" +
				"CTC (T) (cmolc/dm³) – Capacidade de Troca de Cátions \n" +
				"V (%) – Porcentagem de Saturação por bases\n" +
				"m (%) – Porcentagem de Saturação por alumínio\n\n" +
				"Observações:\n" +
				"A amostragem de solo não é de responsabilidade do laboratório e nem da empresa que gerou o aplicativo Solum.\n" +
				"Este laudo não tem fins jurídicos", textFont, new PdfSolidBrush(_black), new PointF(20, y + 50));

			DrawFooter(g);


			var txf = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            if (analise.HasCalagem)
			{
                page = document.Pages.Add();
                g = page.Graphics;

                await DrawHeader(g, "Relatório de Recomendações", page, analise);
				var s = "Recomendação de Calagem";
				y = 130;
				g.DrawString(s, txf, new PdfSolidBrush(_black), new PointF(25, y));
				y += 30;
				g.DrawString("V2", subHeadingFont, new PdfSolidBrush(_black), new PointF(25, y));
				g.DrawString("PRNT", subHeadingFont, new PdfSolidBrush(_black), new PointF(175, y));
				g.DrawString("Profundidade", subHeadingFont, new PdfSolidBrush(_black), new PointF(325, y));
				g.DrawString("Quantidade", subHeadingFont, new PdfSolidBrush(_black), new PointF(475, y));

				y += 20;
				g.DrawRectangle(new PdfSolidBrush(_gray),
                                new RectangleF(20, y, page.Graphics.ClientSize.Width - 40, 30));
				g.DrawRectangle(new PdfSolidBrush(_grayLight),
                                new RectangleF(21, y + 1, page.Graphics.ClientSize.Width - 42, 28));

				y += 9;
                g.DrawString($"{analise.V2} %", textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString($"{analise.Prnt} %", textFont, new PdfSolidBrush(_black), new PointF(175, y));
                g.DrawString($"{analise.Profundidade} cm", textFont, new PdfSolidBrush(_black), new PointF(325, y));
                var calagem = Calculador.CalcularCalcario(analise.Prnt, analise.V2, analise.CTC, analise.V, analise.Profundidade);
                g.DrawString($"{calagem} kg/ha", textFont, new PdfSolidBrush(_black),
					new PointF(475, y));
			}
            if (analise.HasCorretiva)
			{
				y += 50;
				g.DrawString("Recomendação de Adubagem Corretiva", txf, new PdfSolidBrush(_black), new PointF(25, y));

				y += 30;
				g.DrawString("P2O5", subHeadingFont, new PdfSolidBrush(_black), new PointF(25, y));
				g.DrawString("K2O", subHeadingFont, new PdfSolidBrush(_black), new PointF(175, y));

				y += 20;
				g.DrawRectangle(new PdfSolidBrush(_gray),
					new RectangleF(20, y, page.Graphics.ClientSize.Width - 40, 30));
				g.DrawRectangle(new PdfSolidBrush(_grayLight),
					new RectangleF(21, y + 1, page.Graphics.ClientSize.Width - 42, 28));

				y += 9;
                var p2o5 = Calculador.CalcularP2O5(analise.Argila, analise.Silte, analise.Fosforo);
                var k2o = Calculador.CalcularK2O(analise.Argila, analise.Silte, analise.Potassio, analise.CTC);
                g.DrawString($"{p2o5} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString($"{k2o} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(175, y));
			}

            if (analise.HasSemeadura)
			{
				y += 50;
				g.DrawString("Recomendação de Adubagem de Semeadura", txf, new PdfSolidBrush(_black),
					new PointF(25, y));

				y += 30;
				g.DrawString("Cultura", subHeadingFont, new PdfSolidBrush(_black), new PointF(25, y));
				g.DrawString("Expectativa", subHeadingFont, new PdfSolidBrush(_black), new PointF(125, y));
				g.DrawString("N", subHeadingFont, new PdfSolidBrush(_black), new PointF(225, y));
				g.DrawString("P2O5", subHeadingFont, new PdfSolidBrush(_black), new PointF(325, y));
				g.DrawString("K2O", subHeadingFont, new PdfSolidBrush(_black), new PointF(475, y));

				y += 20;
				g.DrawRectangle(new PdfSolidBrush(_gray),
                                new RectangleF(20, y, page.Graphics.ClientSize.Width - 40, 30));
				g.DrawRectangle(new PdfSolidBrush(_grayLight),
                                new RectangleF(21, y + 1, page.Graphics.ClientSize.Width - 42, 28));

				y += 9;
                Enum.TryParse(analise.Cultura, out Cultura cultura);
                var interpreter = SemeaduraInjector.GetInstance(cultura);
                var n = interpreter.QuanidadeNitrogenio(analise.Expectativa, Nivel.Adequado);
                var p2o5 = interpreter.QuantidadeFosforo(analise.Expectativa, Interpretador.NiveFosforo(analise.Fosforo, Interpretador.Textura(analise.Argila, analise.Silte)));
                var k2o = interpreter.QuantidadePotassio(analise.Expectativa, Interpretador.NivelPotassio(analise.Potassio, analise.CTC));

                g.DrawString(analise.Cultura, textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString($"{analise.Expectativa} t/ha", textFont, new PdfSolidBrush(_black), new PointF(125, y));
				g.DrawString($"{n} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(225, y));
                g.DrawString($"{p2o5} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(325, y));
                g.DrawString($"{k2o} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(475, y));
			}

            if (analise.HasCobertura)
			{
				y += 50;
				g.DrawString("Recomendação da Adubagem de Cobertura", txf, new PdfSolidBrush(_black),
					new PointF(25, y));
				y += 30;
				g.DrawString("Cultura", subHeadingFont, new PdfSolidBrush(_black), new PointF(25, y));
				g.DrawString("Expectativa", subHeadingFont, new PdfSolidBrush(_black), new PointF(125, y));
				g.DrawString("N", subHeadingFont, new PdfSolidBrush(_black), new PointF(225, y));
				g.DrawString("P2O5", subHeadingFont, new PdfSolidBrush(_black), new PointF(325, y));
				g.DrawString("K2O", subHeadingFont, new PdfSolidBrush(_black), new PointF(475, y));

				y += 20;
				g.DrawRectangle(new PdfSolidBrush(_gray),
					new RectangleF(20, y, page.Graphics.ClientSize.Width - 40, 30));
				g.DrawRectangle(new PdfSolidBrush(_grayLight),
					new RectangleF(21, y + 1, page.Graphics.ClientSize.Width - 42, 28));

				y += 9;

				Enum.TryParse(analise.Cultura, out Cultura cultura);
                var interpreter = CoberturaInjector.GetInstance(cultura);
				var n = interpreter.QuanidadeNitrogenio(analise.Expectativa);
				var p2o5 = interpreter.QuantidadeFosforo(analise.Expectativa);
				var k2o = interpreter.QuantidadePotassio(analise.Expectativa);

                g.DrawString(analise.Cultura, textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString($"{analise.Expectativa} t/ha", textFont, new PdfSolidBrush(_black), new PointF(125, y));
				g.DrawString($"{n} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(225, y));
                g.DrawString($"{p2o5} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(325, y));
                g.DrawString($"{k2o} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(475, y));
			}

			g.DrawString("Observações:\n" +
						 "A amostragem de solo não é de responsabilidade do laboratório e nem da empresa que gerou o aplicativo Solum.\n" +
						 "Este laudo não tem fins jurídicos", new PdfStandardFont(PdfFontFamily.Helvetica, 10),
				new PdfSolidBrush(_black), new PointF(20, y + 50));

			DrawFooter(g);
         	var stream = new MemoryStream();
			document.Save(stream);
			document.Close(true);
            return stream;
        }

		private void DrawImage(PdfGraphics page)
		{
			var assembly = typeof(App).GetTypeInfo().Assembly;
			var imgStream = assembly.GetManifestResourceStream("Solum.ic_solum.jpg");
			page.DrawImage(PdfImage.FromStream(imgStream), 20, 20, 72, 72);
		}

        private async Task DrawHeader(PdfGraphics pg, string title, PdfPage page, Analise analise)
		{
			DrawImage(pg);

			PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
			PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
			PdfFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            pg.DrawString(title, headerFont, new PdfSolidBrush(_black), new PointF(110, 20));
            var talhao = await AzureService.Instance.FindTalhaoAsync(analise.TalhaoId);
			var fazenda = await AzureService.Instance.FindFazendaAsync(talhao.FazendaId);
			pg.DrawString(fazenda.Nome, subHeadingFont, new PdfSolidBrush(_black), new PointF(110, 52));
			pg.DrawString("Talhão " + talhao.Nome, textFont, new PdfSolidBrush(_black), new PointF(110, 74));
            pg.DrawString($"{analise.DataCalculoSemeadura:dd/MM/yyyy}", textFont, new PdfSolidBrush(_black), new PointF(page.Graphics.ClientSize.Width - 75, 25));
            pg.DrawRectangle(new PdfSolidBrush(_green), new RectangleF(0, 105, page.Graphics.ClientSize.Width, 5));
		}

		private void DrawFooter(PdfGraphics page)
		{
			PdfFont footerBoldFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
			PdfFont footerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);


			page.DrawRectangle(new PdfSolidBrush(_green),
				new RectangleF(0, page.ClientSize.Height - 80, page.ClientSize.Width, 5));
			page.DrawString("Base de cálculo: Bioma Cerrado", footerFont, new PdfSolidBrush(_black),
				new PointF(20, page.ClientSize.Height - 60));
			page.DrawString("Relatório gerado pelo aplicativo Solum", footerBoldFont, new PdfSolidBrush(_black),
				new PointF(20, page.ClientSize.Height - 40));
			page.DrawString("Desenvolvido por Sydy Tecnologia", footerFont, new PdfSolidBrush(_black),
				new PointF(page.ClientSize.Width - 175, page.ClientSize.Height - 60));

			var linkAnnot = new PdfTextWebLink();
			linkAnnot.Url = "http://www.sydy.com.br";
			linkAnnot.Text = "www.sydy.com.br";
			linkAnnot.Font = footerBoldFont;
			linkAnnot.Brush = new PdfSolidBrush(_black);
			linkAnnot.DrawTextWebLink(page, new PointF(page.ClientSize.Width - 105, page.ClientSize.Height - 40));
		}

		private float BodyContent(PdfGraphics pg, string propriedade, string valorAtual, string valorAdequado,
                                  string classe, float yPosition, Color color, PdfPage page)
		{
			PdfFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

			pg.DrawRectangle(new PdfSolidBrush(_gray),
				new RectangleF(20, yPosition, page.Graphics.ClientSize.Width - 40, 30));
			pg.DrawRectangle(new PdfSolidBrush(color),
				new RectangleF(21, yPosition + 1, page.Graphics.ClientSize.Width - 42, 28));

			pg.DrawString(propriedade, textFont, new PdfSolidBrush(_black), new PointF(25, yPosition + 9));
			pg.DrawString(valorAtual, textFont, new PdfSolidBrush(_black), new PointF(175, yPosition + 9));
			pg.DrawString(valorAdequado, textFont, new PdfSolidBrush(_black), new PointF(325, yPosition + 9));
			pg.DrawString(classe, textFont, new PdfSolidBrush(_black), new PointF(475, yPosition + 9));

			return yPosition + 29;
		}

		private string TexturaPConverter(Textura textura)
		{
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

		private string CtcKConverter(float ctc)
		{
			return ctc < 4 ? "30,01 a 40,00" : "50,01 a 80,00";
		}

		private string TexturaCTCConverter(Textura textura)
		{
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

		private string TexturaMoConverter(Textura textura)
		{
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

		private string NivelConverter(Nivel nivel)
		{
			switch (nivel)
			{
				case Nivel.MuitoBaixo:
					return "Muito Baixo";
				case Nivel.Baixo:
					return "Baixo";
				case Nivel.Adequado:
					return "Adequado";
				case Nivel.Medio:
					return "Médio";
				case Nivel.Alto:
					return "Alto";
				case Nivel.MuitoAlto:
					return "Muito Alto";
				case Nivel.Nenhum:
					return "";
				default:
					return "";
			}
		}
    }
}
