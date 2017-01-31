using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using Solum.Interfaces;
using Solum.Models;
using Solum.ViewModel;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Xamarin.Forms;
using Color = Syncfusion.Drawing.Color;

namespace Solum.Pages
{
    public partial class InterpretacaoPage : ContentPage
    {
        public InterpretacaoPage(INavigation navigation, Analise analise)
        {
            InitializeComponent();
          //  this.analise = analise;
          BindingContext = new InterpretacaoViewModel(navigation, analise);
          NavigationPage.SetBackButtonTitle(this, "Voltar");
        }

        //private async void OnSalvarTapped(object sender, EventArgs e)
        //{
        //    var action = await DisplayActionSheet(null, "Cancelar", null, "Salvar", "Salvar e exportar");

        //    if (action == "Salvar")
        //    {
        //        (BindingContext as InterpretacaoViewModel).SalvarAnalise();
        //        await Navigation.PopToRootAsync();
        //    }
        //    else if (action == "Salvar e exportar")
        //    {
        //        GeneratePdf();
        //        (BindingContext as InterpretacaoViewModel).SalvarAnalise();
        //        await Navigation.PopToRootAsync();
        //    }
        //}

        private void OnPdfTapped(object sender, EventArgs e)
        {
            GeneratePdf();
        }

        private void GeneratePdf()
        {
            doc = new PdfDocument();
            doc.PageSettings.Margins.All = 0;
            page = doc.Pages.Add();
            var g = page.Graphics;

            var assembly = typeof(App).GetTypeInfo().Assembly;
            var imgStream = assembly.GetManifestResourceStream("Solum.ic_solum.jpg");
            g.DrawImage(PdfImage.FromStream(imgStream), 20, 20, 72, 72);

            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            //Header 
            g.DrawString("Relatório de Interpretação de Análise de Solo", headerFont, new PdfSolidBrush(black),
                new PointF(110, 20));
            g.DrawString(analise.Talhao.Fazenda.Nome, subHeadingFont, new PdfSolidBrush(black), new PointF(110, 52));
            g.DrawString("Talhão " + analise.Talhao, textFont, new PdfSolidBrush(black), new PointF(110, 74));
            g.DrawString(string.Format("{0:dd/MM/yyyy}", analise.Data), textFont, new PdfSolidBrush(black),
                new PointF(page.Graphics.ClientSize.Width - 75, 25));
            g.DrawRectangle(new PdfSolidBrush(green), new RectangleF(0, 105, page.Graphics.ClientSize.Width, 5));

            var textura = (BindingContext as InterpretacaoViewModel).InterpretacaoTextura;
            g.DrawString("Textura: " + textura, subHeadingFont, new PdfSolidBrush(black), new PointF(25, 130));

            g.DrawString("Propriedade", subHeadingFont, new PdfSolidBrush(black), new PointF(25, 160));
            g.DrawString("Valor atual", subHeadingFont, new PdfSolidBrush(black), new PointF(175, 160));
            g.DrawString("Nível adequado", subHeadingFont, new PdfSolidBrush(black), new PointF(325, 160));
            g.DrawString("Classe", subHeadingFont, new PdfSolidBrush(black), new PointF(475, 160));


            //pH
            var valorAtual = (BindingContext as InterpretacaoViewModel).Analise.PotencialHidrogenico.ToString("F",
                CultureInfo.InvariantCulture);
            var valorAdequado = "4,81 a 5,50";
            var classe = (BindingContext as InterpretacaoViewModel).InterpretacaoPh;

            var y = BodyContent(g, "pH (CaCl2)", valorAtual, valorAdequado, classe, 180, grayLight);

            //P
            valorAtual = (BindingContext as InterpretacaoViewModel).Analise.Fosforo.ToString("F",
                CultureInfo.InvariantCulture);
            valorAdequado = TexturaPConverter((BindingContext as InterpretacaoViewModel).InterpretacaoTextura);
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoP;

            y = BodyContent(g, "P", valorAtual, valorAdequado, classe, y, white);

            //K
            valorAtual = (BindingContext as InterpretacaoViewModel).Analise.Potassio.ToString("F",
                CultureInfo.InvariantCulture);
            valorAdequado = CtcKConverter(analise.CTC);
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoK;

            y = BodyContent(g, "K", valorAtual, valorAdequado, classe, y, grayLight);

            //Ca
            valorAtual = (BindingContext as InterpretacaoViewModel).Analise.Calcio.ToString("F",
                CultureInfo.InvariantCulture);
            valorAdequado = "1,50 a 7,00";
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoCa;

            y = BodyContent(g, "Ca", valorAtual, valorAdequado, classe, y, white);

            //Mg
            valorAtual = (BindingContext as InterpretacaoViewModel).Analise.Magnesio.ToString("F",
                CultureInfo.InvariantCulture);
            valorAdequado = "0,50 a 2,00";
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoMg;

            y = BodyContent(g, "Mg", valorAtual, valorAdequado, classe, y, grayLight);

            //M.O.
            valorAtual = (BindingContext as InterpretacaoViewModel).Analise.MateriaOrganica.ToString("F",
                CultureInfo.InvariantCulture);
            valorAdequado = TexturaMoConverter((BindingContext as InterpretacaoViewModel).InterpretacaoTextura);
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoMo;

            y = BodyContent(g, "M.O.", valorAtual, valorAdequado, classe, y, white);

            //CTC(T)
            valorAtual = (BindingContext as InterpretacaoViewModel).Analise.CTC.ToString("F",
                CultureInfo.InvariantCulture);
            valorAdequado = TexturaCTCConverter((BindingContext as InterpretacaoViewModel).InterpretacaoTextura);
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoCtc;

            y = BodyContent(g, "CTC(T)", valorAtual, valorAdequado, classe, y, grayLight);

            //V(%)
            valorAtual =
                (BindingContext as InterpretacaoViewModel).Analise.V.ToString("F", CultureInfo.InvariantCulture) + "%";
            valorAdequado = "35,01 a 60,00";
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoV;

            y = BodyContent(g, "V(%)", valorAtual, valorAdequado, classe, y, white);

            //m(%)
            valorAtual =
                (BindingContext as InterpretacaoViewModel).Analise.M.ToString("F", CultureInfo.InvariantCulture) + "%";
            valorAdequado = "Baixo";
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoM;

            y = BodyContent(g, "m%", valorAtual, valorAdequado, classe, y, grayLight);

            //Ca/K
            valorAtual = (BindingContext as InterpretacaoViewModel).Analise.CaK.ToString("F",
                CultureInfo.InvariantCulture);
            valorAdequado = "14,01 a 25,00";
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoCaK;

            y = BodyContent(g, "Ca/K", valorAtual, valorAdequado, classe, y, white);

            //Mg/K
            valorAtual = (BindingContext as InterpretacaoViewModel).Analise.MgK.ToString("F",
                CultureInfo.InvariantCulture);
            valorAdequado = "4,01 a 15,00";
            classe = (BindingContext as InterpretacaoViewModel).InterpretacaoMgK;

            y = BodyContent(g, "Mg/K", valorAtual, valorAdequado, classe, y, grayLight);

            //Texto
            g.DrawString(
                "Fonte das tabelas: Sousa, D.M.G. de; Lobato, E. Cerrado – Correção do solo e adubação. 2ª ed. (2004).\npH (CaCl2) – solução de Cloreto de Cálcio 0,01 M na proporção 1:2,5\nP e K (mg/dm³) - Extrator: solução Mehlich 1 (HCl 0,05 N e H2SO4 0,025 N)\nCa, Mg e Al (cmolc/dm³) – Extrator: solução de Cloreto de Potássio 1N (KCl) \nH (cmolc/dm³) – Extrator: Solução de Acetato de Cálcio\nM.O. (Matéria Orgânica) (g/dm³) – Extrator: Oxidação com Bicromato de Potássio e determinação colorimétrica \nAreia, Silte e Argila (g/kg) – Extrator: dispersante NaOH e determinação por densímetro\nCTC (T) (cmolc/dm³) – Capacidade de Troca de Cátions \nV (%) – Porcentagem de Saturação por bases\nm (%) – Porcentagem de Saturação por alumínio\n\nObservações:\nConsulte um Engenheiro Agrônomo para recomendação de calagem e adubação.\nA amostragem de solo não é de responsabilidade do laboratório e nem da empresa que gerou o aplicativo Solum.\nEste laudo não tem fins jurídicos",
                textFont, new PdfSolidBrush(black), new PointF(20, y + 50));

            PdfFont footerBoldFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);
            PdfFont footerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            //Footer
            g.DrawRectangle(new PdfSolidBrush(green),
                new RectangleF(0, page.Graphics.ClientSize.Height - 80, page.Graphics.ClientSize.Width, 5));
            g.DrawString("Base de cálculo: Bioma Cerrado", footerFont, new PdfSolidBrush(black),
                new PointF(20, page.Graphics.ClientSize.Height - 60));
            g.DrawString("Relatório gerado pelo aplicativo Solum", footerBoldFont, new PdfSolidBrush(black),
                new PointF(20, page.Graphics.ClientSize.Height - 40));
            g.DrawString("Desenvolvido por Sydy Tecnologia", footerFont, new PdfSolidBrush(black),
                new PointF(page.Graphics.ClientSize.Width - 175, page.Graphics.ClientSize.Height - 60));
            var linkAnnot = new PdfTextWebLink();
            linkAnnot.Url = "http://www.sydy.com.br";
            linkAnnot.Text = "www.sydy.com.br";
            linkAnnot.Font = footerBoldFont;
            linkAnnot.Brush = new PdfSolidBrush(black);
            linkAnnot.DrawTextWebLink(page,
                new PointF(page.Graphics.ClientSize.Width - 105, page.Graphics.ClientSize.Height - 40));

            var stream = new MemoryStream();
            doc.Save(stream);
            doc.Close(true);

            DependencyService.Get<IPdfViewer>().PreviewPdf(stream);
        }

        private float BodyContent(PdfGraphics pg, string propriedade, string valorAtual, string valorAdequado,
            string classe, float yPosition, Color color)
        {
            PdfFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            pg.DrawRectangle(new PdfSolidBrush(gray),
                new RectangleF(20, yPosition, page.Graphics.ClientSize.Width - 40, 30));
            pg.DrawRectangle(new PdfSolidBrush(color),
                new RectangleF(21, yPosition + 1, page.Graphics.ClientSize.Width - 42, 28));

            pg.DrawString(propriedade, textFont, new PdfSolidBrush(black), new PointF(25, yPosition + 9));
            pg.DrawString(valorAtual, textFont, new PdfSolidBrush(black), new PointF(175, yPosition + 9));
            pg.DrawString(valorAdequado, textFont, new PdfSolidBrush(black), new PointF(325, yPosition + 9));
            pg.DrawString(classe, textFont, new PdfSolidBrush(black), new PointF(475, yPosition + 9));

            return yPosition + 29;
        }

        private string TexturaPConverter(string textura)
        {
            switch (textura)
            {
                case "Arenosa":
                    return "18,01 a 25,00";
                case "Média":
                    return "15,01 a 20,00";
                case "Argilosa":
                    return "8,01 a 12,00";
                case "Muito argilosa":
                    return "4,01 a 6,00";
                default:
                    return "";
            }
        }

        private string CtcKConverter(float ctc)
        {
            if (ctc < 4)
                return "30,01 a 40,00";
            return "50,01 a 80,00";
        }

        private string TexturaCTCConverter(string textura)
        {
            switch (textura)
            {
                case "Arenosa":
                    return "4,01 a 6,00";
                case "Média":
                    return "6,01 a 9,00";
                case "Argilosa":
                    return "9,01 a 13,50";
                case "Muito argilosa":
                    return "12,01 a 18,00";
                default:
                    return "";
            }
        }

        private string TexturaMoConverter(string textura)
        {
            switch (textura)
            {
                case "Arenosa":
                    return "10,01 a 15,00";
                case "Média":
                    return "20,01 a 30,00";
                case "Argilosa":
                    return "30,01 a 45,00";
                case "Muito argilosa":
                    return "35,01 a 52,00";
                default:
                    return "";
            }
        }

        #region Private Properties

        private PdfDocument doc;
        private PdfPage page;

        private readonly Color black = Color.FromArgb(255, 52, 52, 52);
        private readonly Color gray = Color.FromArgb(255, 155, 155, 155);
        private readonly Color grayLight = Color.FromArgb(255, 241, 241, 241);
        private readonly Color green = Color.FromArgb(255, 63, 170, 88);
        private readonly Color white = Color.FromArgb(255, 255, 255, 255);

        private readonly Analise analise;

        #endregion
    }
}