using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Input;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Interactive;
using Xamarin.Forms;
using Color = Syncfusion.Drawing.Color;

namespace Solum.ViewModel
{
    public class GerenciamentoAnaliseViewModel : BaseViewModel
    {
        private readonly Color _black = Color.FromArgb(255, 52, 52, 52);
        private readonly Color _gray = Color.FromArgb(255, 155, 155, 155);
        private readonly Color _grayLight = Color.FromArgb(255, 241, 241, 241);
        private readonly Color _green = Color.FromArgb(255, 63, 170, 88);
        private readonly Color _white = Color.FromArgb(255, 255, 255, 255);
        private Analise _analise;
        private string _calagemDate;
        private string _coberturaDate;
        private string _corretivaDate;
        private PdfDocument _doc;
        private ICommand _generatePdfCommand;
        private bool _hasCalagemCalculation;
        private bool _hasCoberturaCalculation;
        private bool _hasCorretivaCalculation;
        private bool _hasSemeaduraCalculation;
        private string _interpretacaoDate;
        private bool _isGeneratingReport;
        private PdfPage _page;
        private string _semeaduraDate;
        private ICommand _showCoberturaPageCommand;
        private ICommand _showCorretivaPageCommand;
        private ICommand _showInterpretacaoPageCommand;
        private ICommand _showRecomendacaoCalagemPageCommand;
        private ICommand _showSemeaduraPageCommand;
        private bool _wasInterpreted;

		public GerenciamentoAnaliseViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
			AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(
				task =>
				  {
					  Analise = task.Result;
					  UpdateValues(Analise);
				  }
			);
        }

        public Analise Analise
        {
			get { return _analise; }
			set { SetPropertyChanged(ref _analise, value); }
        }

        public bool WasInterpreted
        {
			get { return _wasInterpreted; }
			set { SetPropertyChanged(ref _wasInterpreted, value); }
        }

        public bool HasCalagemCalculation
        {
			get { return _hasCalagemCalculation; }
			set { SetPropertyChanged(ref _hasCalagemCalculation, value); }
        }

        public bool HasCorretivaCalculation
        {
			get { return _hasCorretivaCalculation; }
			set { SetPropertyChanged(ref _hasCorretivaCalculation, value); }
        }

        public bool HasSemeaduraCalculation
        {
			get { return _hasSemeaduraCalculation; }
			set { SetPropertyChanged(ref _hasSemeaduraCalculation, value); }
        }

        public bool HasCoberturaCalculation
        {
			get { return _hasCoberturaCalculation; }
			set { SetPropertyChanged(ref _hasCoberturaCalculation, value); }
        }

        public bool IsGeneratingReport
        {
			get { return _isGeneratingReport; }
			set { SetPropertyChanged(ref _isGeneratingReport, value); }
        }

        public string InterpretacaoDate
        {
			get { return _interpretacaoDate; }
			set { SetPropertyChanged(ref _interpretacaoDate, value); }
        }

        public string CalagemDate
        {
			get { return _calagemDate; }
			set { SetPropertyChanged(ref _calagemDate, value); }
        }

        public string CorretivaDate
        {
			get { return _corretivaDate; }
			set { SetPropertyChanged(ref _corretivaDate, value); }
        }

        public string SemeaduraDate
        {
			get { return _semeaduraDate; }
			set { SetPropertyChanged(ref _semeaduraDate, value); }
        }

        public string CoberturaDate
        {
			get { return _coberturaDate; }
			set { SetPropertyChanged(ref _coberturaDate, value); }
        }


        public ICommand ShowInterpretacaoPageCommand
			=> _showInterpretacaoPageCommand ?? (_showInterpretacaoPageCommand = new Command(async ()=> await ShowInterpretacaoPage()));

        public ICommand ShowRecomendacaoCalagemPageCommand
			=> _showRecomendacaoCalagemPageCommand ?? (_showRecomendacaoCalagemPageCommand = new Command((async ()=> await ShowRecomendacaoCalagemPage())));

        public ICommand ShowCorretivaPageCommand
			=> _showCorretivaPageCommand ?? (_showCorretivaPageCommand = new Command((async ()=> await ShowCorretivaPage())));

        public ICommand ShowSemeaduraPageCommand
			=> _showSemeaduraPageCommand ?? (_showSemeaduraPageCommand = new Command((async ()=> await ShowSemeaduraPage())));

        public ICommand ShowCoberturaPageCommand
			=> _showCoberturaPageCommand ?? (_showCoberturaPageCommand = new Command((async ()=> await ShowCoberturaPage())));

         public ICommand GeneratePdfCommand 
			=> _generatePdfCommand ?? (_generatePdfCommand = new Command((async ()=> await GeneratePdf())));


		private async Task ShowInterpretacaoPage()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new InterpretacaoPage(Analise.Id));
                IsBusy = false;
            }
        }

		private async Task ShowCalagemPage()
        {
            if (!WasInterpreted)
            {
                "Você precisa realizar a interpretação primeiro".ToDisplayAlert(MessageType.Aviso);
                return;
            }
            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new CalagemPage(Analise.Id));
                IsBusy = false;
            }
        }

		private async Task ShowRecomendacaoCalagemPage()
        {
            if (IsNotBusy)
            {
                IsBusy = true;
                if (HasCalagemCalculation)
                {
                    await Navigation.PushAsync(new RecomendacaoCalagemPage(Analise.Id, Analise.V2, Analise.Prnt,
                        Analise.Profundidade, false));
                    IsBusy = false;
                }
                else
                {
                    IsBusy = false;
                    await ShowCalagemPage();
                }
            }
        }

		private async Task ShowCorretivaPage()
        {
            if (!HasCalagemCalculation)
            {
                "Você precisa executar o cálculo para recomendação de calagem primeiro"
                    .ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new AdubacaoCorretivaPage(Analise.Id, !Analise.HasCorretiva));
                IsBusy = false;
            }
        }

		private async Task ShowSemeaduraPage()
        {
            if (!HasCorretivaCalculation)
            {
                "Você precisa executar o cálculo da recomendação corretiva primeiro".ToDisplayAlert(MessageType.Aviso);
                return;
            }
            if (IsNotBusy)
            {
                IsBusy = true;
                if (HasSemeaduraCalculation)
                {
                    Cultura c;
                    Enum.TryParse(Analise.Cultura, out c);
                    await Navigation.PushAsync(
                        new RecomendacaoSemeaduraPage(Analise.Id, Analise.Expectativa, c, !Analise.HasSemeadura));
                }
                else
                {
                    await Navigation.PushAsync(new SemeaduraPage(Analise.Id));
                }
                IsBusy = false;
            }
        }

		private async Task ShowCoberturaPage()
        {
            if (!HasSemeaduraCalculation)
            {
                "Você precisa executar a recomendação de semeadura primeiro".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            if (IsNotBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new AdubacaoCoberturaPage(Analise.Id, !Analise.HasCobertura));
                IsBusy = false;
            }
        }

		public void RefreshValues()
		{
			if (Analise != null)
			{
				var analiseId = Analise.Id;
				AzureService.Instance.FindAnaliseAsync(analiseId).ContinueWith(
					task =>
					  {
						  Analise = task.Result;
						  UpdateValues(Analise);
					  }
				);
			}
		}

		public void UpdateValues(Analise analise)
		{
			WasInterpreted = analise.WasInterpreted;
			HasCalagemCalculation = analise.HasCalagem;
			HasCorretivaCalculation = analise.HasCorretiva;
			HasSemeaduraCalculation = analise.HasSemeadura;
			HasCoberturaCalculation = analise.HasCobertura;
			InterpretacaoDate = analise.WasInterpreted ? $"Realizada em  {analise.DataInterpretacao:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
			CalagemDate = analise.HasCalagem ? $"Realizada em {analise.DataCalculoCalagem:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
			CorretivaDate = analise.HasCorretiva ? $"Realizada em {analise.DataCalculoCorretiva:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
			SemeaduraDate = analise.HasSemeadura ? $"Realizada em {analise.DataCalculoSemeadura:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
			CoberturaDate = analise.HasCobertura ? $"Realizada em {analise.DataCalculoCobertura:dd/MM/yy HH:mm:ss}" : "Não realizada ainda";
		}

		private async Task GeneratePdf()
        {
            IsGeneratingReport = true;

            if (!WasInterpreted)
            {
                "Você precisa ao menos realizar a interpretação dos dados".ToDisplayAlert(MessageType.Aviso);
                return;
            }

            _doc = new PdfDocument();
            var infos = _doc.DocumentInformation;
            infos.Author = "Sydy Tecnologia";
            infos.CreationDate = DateTime.Now;
            infos.Creator = "Solum";
            infos.Title = $"Análise {Analise.Identificacao}";
            infos.Subject = "Relatório";

            var prefs = _doc.ViewerPreferences;
            prefs.PageMode = PdfPageMode.UseAttachments;
            _doc.PageSettings.Margins.All = 0;
            _page = _doc.Pages.Add();
            var g = _page.Graphics;

            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10, PdfFontStyle.Bold);

            await DrawHeader(g, "Relatório de Interpretação da Analise");


            var argila = Analise.Argila;
            var silte = Analise.Silte;
            var textura = Interpretador.Textura(argila, silte);
            g.DrawString("Textura: " + textura, subHeadingFont, new PdfSolidBrush(_black), new PointF(25, 130));
            g.DrawString("Propriedade", subHeadingFont, new PdfSolidBrush(_black), new PointF(25, 160));
            g.DrawString("Valor atual", subHeadingFont, new PdfSolidBrush(_black), new PointF(175, 160));
            g.DrawString("Nível adequado", subHeadingFont, new PdfSolidBrush(_black), new PointF(325, 160));
            g.DrawString("Classe", subHeadingFont, new PdfSolidBrush(_black), new PointF(475, 160));


            //pH
            var valorAtual = Analise.PotencialHidrogenico.ToString("F", CultureInfo.InvariantCulture);
            var valorAdequado = "4,81 a 5,50";
            var classe = NivelConverter(Interpretador.NivelPotencialHidrogenico(Analise.PotencialHidrogenico));
            var y = BodyContent(g, "pH (CaCl2)", valorAtual, valorAdequado, classe, 180, _grayLight);

            //Fósforo
            valorAtual = Analise.Fosforo.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = TexturaPConverter(textura);
            classe = NivelConverter(Interpretador.NiveFosforo(Analise.Fosforo, textura));

            y = BodyContent(g, "P", valorAtual, valorAdequado, classe, y, _white);

            //K
            valorAtual = Analise.Potassio.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = CtcKConverter(_analise.CTC);
            classe = NivelConverter(Interpretador.NivelPotassio(Analise.Potassio, Analise.CTC));

            y = BodyContent(g, "K", valorAtual, valorAdequado, classe, y, _grayLight);

            //Ca
            valorAtual = Analise.Calcio.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = "1,50 a 7,00";
            classe = NivelConverter(Interpretador.NivelCalcio(Analise.Calcio));

            y = BodyContent(g, "Ca", valorAtual, valorAdequado, classe, y, _white);

            //Mg
            valorAtual = Analise.Magnesio.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = "0,50 a 2,00";
            classe = NivelConverter(Interpretador.NivelMagnesio(Analise.Magnesio));

            y = BodyContent(g, "Mg", valorAtual, valorAdequado, classe, y, _grayLight);

            //M.O.
            valorAtual = Analise.MateriaOrganica.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = TexturaMoConverter(textura);
            classe = NivelConverter(Interpretador.NivelMateriaOrganica(Analise.MateriaOrganica, textura));

            y = BodyContent(g, "M.O.", valorAtual, valorAdequado, classe, y, _white);

            //CTC(T)
            valorAtual = Analise.CTC.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = TexturaCTCConverter(textura);
            classe = NivelConverter(Interpretador.NivelCtc(Analise.CTC, textura));

            y = BodyContent(g, "CTC(T)", valorAtual, valorAdequado, classe, y, _grayLight);

            //V(%)
            valorAtual = Analise.V.ToString("F", CultureInfo.InvariantCulture) + "%";
            valorAdequado = "35,01 a 60,00";
            classe = NivelConverter(Interpretador.NivelV(Analise.V));

            y = BodyContent(g, "V(%)", valorAtual, valorAdequado, classe, y, _white);

            //m(%)
            valorAtual = Analise.M.ToString("F", CultureInfo.InvariantCulture) + "%";
            valorAdequado = "Baixo";
            classe = NivelConverter(Interpretador.NivelM(Analise.M));

            y = BodyContent(g, "m%", valorAtual, valorAdequado, classe, y, _grayLight);

            //Ca/K
            valorAtual = Analise.CaK.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = "14,01 a 25,00";
            classe = NivelConverter(Interpretador.NivelCalcioPotassio(Analise.CaK));

            y = BodyContent(g, "Ca/K", valorAtual, valorAdequado, classe, y, _white);

            //Mg/K
            valorAtual = Analise.MgK.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = "4,01 a 15,00";
            classe = NivelConverter(Interpretador.NivelMagnesioPotassio(Analise.MgK));

            y = BodyContent(g, "Mg/K", valorAtual, valorAdequado, classe, y, _grayLight);

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
            if (HasCalagemCalculation)
            {
                _page = _doc.Pages.Add();
                g = _page.Graphics;

                await DrawHeader(g, "Relatório de Recomendações");
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
                    new RectangleF(20, y, _page.Graphics.ClientSize.Width - 40, 30));
                g.DrawRectangle(new PdfSolidBrush(_grayLight),
                    new RectangleF(21, y + 1, _page.Graphics.ClientSize.Width - 42, 28));

                y += 9;
                g.DrawString($"{Analise.V2} %", textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString($"{Analise.Prnt} %", textFont, new PdfSolidBrush(_black), new PointF(175, y));
                g.DrawString($"{Analise.Profundidade} cm", textFont, new PdfSolidBrush(_black), new PointF(325, y));
                var calagemvm = new RecomendacaoCalagemViewModel(Navigation, Analise.Id, Analise.V2, Analise.Prnt,
                    Analise.Profundidade, false);
                g.DrawString($"{calagemvm.QuantidadeCal} kg/ha", textFont, new PdfSolidBrush(_black),
                    new PointF(475, y));
            }
            if (HasCorretivaCalculation)
            {
                y += 50;
                g.DrawString("Recomendação de Adubagem Corretiva", txf, new PdfSolidBrush(_black), new PointF(25, y));

                y += 30;
                g.DrawString("P2O5", subHeadingFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString("K2O", subHeadingFont, new PdfSolidBrush(_black), new PointF(175, y));

                y += 20;
                g.DrawRectangle(new PdfSolidBrush(_gray),
                    new RectangleF(20, y, _page.Graphics.ClientSize.Width - 40, 30));
                g.DrawRectangle(new PdfSolidBrush(_grayLight),
                    new RectangleF(21, y + 1, _page.Graphics.ClientSize.Width - 42, 28));

                y += 9;
                var corretivavm = new AdubacaoCorretivaViewModel(Navigation, _analise.Id, false);
                g.DrawString($"{corretivavm.P2O5} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString($"{corretivavm.K2O} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(175, y));
            }

            if (HasSemeaduraCalculation)
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
                    new RectangleF(20, y, _page.Graphics.ClientSize.Width - 40, 30));
                g.DrawRectangle(new PdfSolidBrush(_grayLight),
                    new RectangleF(21, y + 1, _page.Graphics.ClientSize.Width - 42, 28));

                y += 9;
                Cultura c;
                Enum.TryParse(Analise.Cultura, out c);
                var semeaduravm =
                    new RecomendacaoSemeaduraViewModel(Navigation, _analise.Id, Analise.Expectativa, c, false);
                g.DrawString(Analise.Cultura, textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString($"{Analise.Expectativa} t/ha", textFont, new PdfSolidBrush(_black), new PointF(125, y));
                g.DrawString($"{semeaduravm.N} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(225, y));
                g.DrawString($"{semeaduravm.P205} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(325, y));
                g.DrawString($"{semeaduravm.K20} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(475, y));
            }

            if (HasCoberturaCalculation)
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
                    new RectangleF(20, y, _page.Graphics.ClientSize.Width - 40, 30));
                g.DrawRectangle(new PdfSolidBrush(_grayLight),
                    new RectangleF(21, y + 1, _page.Graphics.ClientSize.Width - 42, 28));

                y += 9;
                var coberturavm = new AdubacaoCoberturaViewModel(Navigation, _analise.Id, false);
                g.DrawString(Analise.Cultura, textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString($"{Analise.Expectativa} t/ha", textFont, new PdfSolidBrush(_black), new PointF(125, y));
                g.DrawString($"{coberturavm.N} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(225, y));
                g.DrawString($"{coberturavm.P2O5} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(325, y));
                g.DrawString($"{coberturavm.K2O} kg/ha", textFont, new PdfSolidBrush(_black), new PointF(475, y));
            }

            g.DrawString("Observações:\n" +
                         "A amostragem de solo não é de responsabilidade do laboratório e nem da empresa que gerou o aplicativo Solum.\n" +
                         "Este laudo não tem fins jurídicos", new PdfStandardFont(PdfFontFamily.Helvetica, 10),
                new PdfSolidBrush(_black), new PointF(20, y + 50));

            DrawFooter(g);

            IsGeneratingReport = false;
            var stream = new MemoryStream();
            _doc.Save(stream);
            _doc.Close(true);

            DependencyService.Get<IPdfViewer>().PreviewPdf(stream);
        }

        private void DrawImage(PdfGraphics page)
        {
            var assembly = typeof(App).GetTypeInfo().Assembly;
            var imgStream = assembly.GetManifestResourceStream("Solum.ic_solum.jpg");
            page.DrawImage(PdfImage.FromStream(imgStream), 20, 20, 72, 72);
        }

        private async Task DrawHeader(PdfGraphics page, string title)
        {
            DrawImage(page);

            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            page.DrawString(title, headerFont, new PdfSolidBrush(_black), new PointF(110, 20));
            var talhao = await  AzureService.Instance.FindTalhaoAsync(_analise.TalhaoId);
            var fazenda = await AzureService.Instance.FindFazendaAsync(talhao.FazendaId);
            page.DrawString(fazenda.Nome, subHeadingFont, new PdfSolidBrush(_black),
                new PointF(110, 52));
            page.DrawString("Talhão " + talhao.Nome, textFont, new PdfSolidBrush(_black), new PointF(110, 74));
            page.DrawString($"{_analise.DataCalculoSemeadura:dd/MM/yyyy}", textFont, new PdfSolidBrush(_black),
                new PointF(_page.Graphics.ClientSize.Width - 75, 25));
            page.DrawRectangle(new PdfSolidBrush(_green), new RectangleF(0, 105, _page.Graphics.ClientSize.Width, 5));
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
            string classe, float yPosition, Color color)
        {
            PdfFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            pg.DrawRectangle(new PdfSolidBrush(_gray),
                new RectangleF(20, yPosition, _page.Graphics.ClientSize.Width - 40, 30));
            pg.DrawRectangle(new PdfSolidBrush(color),
                new RectangleF(21, yPosition + 1, _page.Graphics.ClientSize.Width - 42, 28));

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