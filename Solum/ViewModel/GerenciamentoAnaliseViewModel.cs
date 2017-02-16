﻿using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using Realms;
using Solum.Handlers;
using Solum.Interfaces;
using Solum.Models;
using Solum.Pages;
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
        public GerenciamentoAnaliseViewModel(INavigation navigation, Analise analise) : base(navigation)
        {
            _realm = Realm.GetInstance();
            Analise = _realm.Find<Analise>(analise.Id);
            PageTitle = Analise.Identificacao;
            UpdateValues();
        }

        #region Private Properties

        private ICommand _showRecomendacaoCalagemPageCommand;
        private ICommand _showCoberturaPageCommand;
        private ICommand _showCorretivaPageCommand;
        private ICommand _showInterpretacaoPageCommand;
        private ICommand _showSemeaduraPageCommand;
        private ICommand _generatePdfCommand;

        private bool _wasInterpreted;
        private bool _hasCalagemCalculation;
        private bool _hasCorretivaCalculation;
        private bool _hasSemeaduraCalculation;
        private bool _hasCoberturaCalculation;
        private bool _isGeneratingReport;
        private string _pageTitle;

        private DateTimeOffset? _interpretacaoDate;
        private DateTimeOffset? _calagemDate;
        private DateTimeOffset? _corretivaDate;
        private DateTimeOffset? _semeaduraDate;
        private DateTimeOffset? _coberturaDate;

        private Analise _analise;
        private readonly Realm _realm;

        private PdfDocument _doc;
        private PdfPage _page;

        private readonly Color _black = Color.FromArgb(255, 52, 52, 52);
        private readonly Color _gray = Color.FromArgb(255, 155, 155, 155);
        private readonly Color _grayLight = Color.FromArgb(255, 241, 241, 241);
        private readonly Color _green = Color.FromArgb(255, 63, 170, 88);
        private readonly Color _white = Color.FromArgb(255, 255, 255, 255);

        #endregion

        #region Binding Properties

        public string PageTitle
        {
            get { return _pageTitle; }
            set { SetPropertyChanged(ref _pageTitle, value); }
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
            get
            {
                return _interpretacaoDate.HasValue
                    ? $"Realizada em  {_interpretacaoDate.Value:dd/MM/yy}"
                    : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _interpretacaoDate, DateTimeOffset.Parse(value)); }
        }

        public string CalagemDate
        {
            get
            {
                return _calagemDate.HasValue ? $"Realizada em {_calagemDate.Value:dd/MM/yy}" : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _calagemDate, DateTimeOffset.Parse(value)); }
        }

        public string CorretivaDate
        {
            get
            {
                return _corretivaDate.HasValue ? $"Realizada em {_corretivaDate.Value:dd/MM/yy}" : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _corretivaDate, DateTimeOffset.Parse(value)); }
        }

        public string SemeaduraDate
        {
            get
            {
                return _semeaduraDate.HasValue ? $"Realizada em {_semeaduraDate.Value:dd/MM/yy}" : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _semeaduraDate, DateTimeOffset.Parse(value)); }
        }

        public string CoberturaDate
        {
            get
            {
                return _coberturaDate.HasValue ? $"Realizada em {_coberturaDate.Value:dd/MM/yy}" : "Não realizada ainda";
            }
            set { SetPropertyChanged(ref _coberturaDate, DateTimeOffset.Parse(value)); }
        }

        #endregion

        #region Commands

        public ICommand ShowInterpretacaoPageCommand
            => _showInterpretacaoPageCommand ?? (_showInterpretacaoPageCommand = new Command(ShowInterpretacaoPage));

        public ICommand ShowRecomendacaoCalagemPageCommand
            => _showRecomendacaoCalagemPageCommand ??
               (_showRecomendacaoCalagemPageCommand = new Command(ShowRecomendacaoCalagemPage));

        public ICommand ShowCorretivaPageCommand
            => _showCorretivaPageCommand ?? (_showCorretivaPageCommand = new Command(ShowCorretivaPage));

        public ICommand ShowSemeaduraPageCommand
            => _showSemeaduraPageCommand ?? (_showSemeaduraPageCommand = new Command(ShowSemeaduraPage));

        public ICommand ShowCoberturaPageCommand
            => _showCoberturaPageCommand ?? (_showCoberturaPageCommand = new Command(ShowCoberturaPage));

        public ICommand GeneratePdfCommand => _generatePdfCommand ?? (_generatePdfCommand = new Command(GeneratePdf));

        #endregion

        #region Functions

        private async void ShowInterpretacaoPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                await Navigation.PushAsync(new InterpretacaoPage(Analise));
                IsBusy = false;
            }
        }

        private async void ShowCalagemPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if (WasInterpreted)
                    await Navigation.PushAsync(new CalagemPage(Analise.Id));
                else
                    "Você precisa realizar a interpretação primeiro".ToDisplayAlert(MessageType.Aviso);
                IsBusy = false;
            }
        }

        private async void ShowRecomendacaoCalagemPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                var wasCalculated = Analise.DataCalculoCalagem.HasValue;
                if (wasCalculated)
                {
                    await Navigation.PushAsync(new RecomendaCalagemPage(Analise.Id));
                    IsBusy = false;
                }
                else
                {
                    IsBusy = false;
                    ShowCalagemPage();
                }
            }
        }

        private async void ShowCorretivaPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if (HasCalagemCalculation)
                    await Navigation.PushAsync(new AdubacaoCorretivaPage(Analise.Id));
                else
                    "Você precisa executar o cálculo para recomendação de calagem primeiro".ToDisplayAlert(
                        MessageType.Aviso);
                IsBusy = false;
            }
        }

        private async void ShowSemeaduraPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if (HasCorretivaCalculation)
                    if (HasSemeaduraCalculation)
                    {
                        var semeadura = _realm.All<Semeadura>().FirstOrDefault(x => x.AnaliseId.Equals(Analise.Id));
                        await Navigation.PushAsync(new RecomendaSemeaduraPage(Analise.Id, semeadura.Expectativa,
                            semeadura.Cultura));
                    }
                    else
                    {
                        await Navigation.PushAsync(new SemeaduraPage(Analise.Id));
                    }
                else
                    "Você precisa executar o cálculo da recomendação corretiva primeiro".ToDisplayAlert(
                        MessageType.Aviso);
                IsBusy = false;
            }
        }

        private async void ShowCoberturaPage()
        {
            if (!IsBusy)
            {
                IsBusy = true;
                if (HasSemeaduraCalculation)
                    await Navigation.PushAsync(new AdubacaoCoberturaPage(Analise.Id));
                else
                    "Você precisa executar a recomendação de semeadura primeiro".ToDisplayAlert(MessageType.Aviso);
                IsBusy = false;
            }
        }

        public void UpdateValues()
        {
            Analise = _realm.Find<Analise>(Analise.Id);
            if (Analise.DataInterpretacao != null)
            {
                InterpretacaoDate = Analise.DataInterpretacao.ToString();
                WasInterpreted = true;
            }
            if (Analise.DataCalculoCalagem != null)
            {
                CalagemDate = Analise.DataCalculoCalagem.ToString();
                HasCalagemCalculation = true;
            }
            if (Analise.DataCalculoCorretiva != null)
            {
                CorretivaDate = Analise.DataCalculoCorretiva.ToString();
                HasCorretivaCalculation = true;
            }
            if (Analise.DataCalculoSemeadura != null)
            {
                SemeaduraDate = Analise.DataCalculoSemeadura.ToString();
                HasSemeaduraCalculation = true;
            }
            if (Analise.DataCalculoCobertura != null)
            {
                CoberturaDate = Analise.DataCalculoCobertura.ToString();
                HasCoberturaCalculation = true;
            }
        }


        private void GeneratePdf()
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

            DrawHeader(g, "Relatório de Interpretação da Analise");


            var argila = Analise.Argila;
            var silte = Analise.Silte;
            var textura = InterpretaHandler.InterpretaTextura(argila, silte);
            g.DrawString("Textura: " + textura, subHeadingFont, new PdfSolidBrush(_black), new PointF(25, 130));
            g.DrawString("Propriedade", subHeadingFont, new PdfSolidBrush(_black), new PointF(25, 160));
            g.DrawString("Valor atual", subHeadingFont, new PdfSolidBrush(_black), new PointF(175, 160));
            g.DrawString("Nível adequado", subHeadingFont, new PdfSolidBrush(_black), new PointF(325, 160));
            g.DrawString("Classe", subHeadingFont, new PdfSolidBrush(_black), new PointF(475, 160));


            //pH
            var valorAtual = Analise.PotencialHidrogenico.ToString("F", CultureInfo.InvariantCulture);
            var valorAdequado = "4,81 a 5,50";
            var classe = InterpretaHandler.InterpretaPh(Analise.PotencialHidrogenico);
            var y = BodyContent(g, "pH (CaCl2)", valorAtual, valorAdequado, classe, 180, _grayLight);

            //Fósforo
            valorAtual = Analise.Fosforo.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = TexturaPConverter(textura);
            classe = InterpretaHandler.InterpretaP(Analise.Fosforo, textura);

            y = BodyContent(g, "P", valorAtual, valorAdequado, classe, y, _white);

            //K
            valorAtual = Analise.Potassio.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = CtcKConverter(_analise.CTC);
            classe = InterpretaHandler.InterpretaK(Analise.Potassio, Analise.CTC);

            y = BodyContent(g, "K", valorAtual, valorAdequado, classe, y, _grayLight);

            //Ca
            valorAtual = Analise.Calcio.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = "1,50 a 7,00";
            classe = InterpretaHandler.InterpretaCa(Analise.Calcio);

            y = BodyContent(g, "Ca", valorAtual, valorAdequado, classe, y, _white);

            //Mg
            valorAtual = Analise.Magnesio.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = "0,50 a 2,00";
            classe = InterpretaHandler.InterpretaMg(Analise.Magnesio);

            y = BodyContent(g, "Mg", valorAtual, valorAdequado, classe, y, _grayLight);

            //M.O.
            valorAtual = Analise.MateriaOrganica.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = TexturaMoConverter(textura);
            classe = InterpretaHandler.InterpretaMo(Analise.MateriaOrganica, textura);

            y = BodyContent(g, "M.O.", valorAtual, valorAdequado, classe, y, _white);

            //CTC(T)
            valorAtual = Analise.CTC.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = TexturaCTCConverter(textura);
            classe = InterpretaHandler.InterpretaCtc(Analise.CTC, textura);

            y = BodyContent(g, "CTC(T)", valorAtual, valorAdequado, classe, y, _grayLight);

            //V(%)
            valorAtual = Analise.V.ToString("F", CultureInfo.InvariantCulture) + "%";
            valorAdequado = "35,01 a 60,00";
            classe = InterpretaHandler.InterpretaV(Analise.V);

            y = BodyContent(g, "V(%)", valorAtual, valorAdequado, classe, y, _white);

            //m(%)
            valorAtual = Analise.M.ToString("F", CultureInfo.InvariantCulture) + "%";
            valorAdequado = "Baixo";
            classe = InterpretaHandler.InterpretaM(Analise.M);

            y = BodyContent(g, "m%", valorAtual, valorAdequado, classe, y, _grayLight);

            //Ca/K
            valorAtual = Analise.CaK.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = "14,01 a 25,00";
            classe = InterpretaHandler.InterpretaCaK(Analise.CaK);

            y = BodyContent(g, "Ca/K", valorAtual, valorAdequado, classe, y, _white);

            //Mg/K
            valorAtual = Analise.MgK.ToString("F", CultureInfo.InvariantCulture);
            valorAdequado = "4,01 a 15,00";
            classe = InterpretaHandler.InterpretaMgK(Analise.MgK);

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

                DrawHeader(g, "Relatório de Recomendações");
                var s = "Recomendação de Calagem";
                y = 130;
                g.DrawString(s, txf, new PdfSolidBrush(_black), new PointF(25, y));
                y += 30;
                g.DrawString("V2 (%)", subHeadingFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString("PRNT", subHeadingFont, new PdfSolidBrush(_black), new PointF(175, y));
                g.DrawString("Profundidade", subHeadingFont, new PdfSolidBrush(_black), new PointF(325, y));
                g.DrawString("Quantidade", subHeadingFont, new PdfSolidBrush(_black), new PointF(475, y));

                y += 20;
                g.DrawRectangle(new PdfSolidBrush(_gray),
                    new RectangleF(20, y, _page.Graphics.ClientSize.Width - 40, 30));
                g.DrawRectangle(new PdfSolidBrush(_grayLight),
                    new RectangleF(21, y + 1, _page.Graphics.ClientSize.Width - 42, 28));

                y += 9;
                var calagem = _realm.All<Calagem>().FirstOrDefault(c => c.AnaliseId.Equals(Analise.Id));
                g.DrawString(calagem.V2.ToString(), textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString(calagem.Prnt.ToString(), textFont, new PdfSolidBrush(_black), new PointF(175, y));
                g.DrawString(calagem.Profundidade.ToString(), textFont, new PdfSolidBrush(_black), new PointF(325, y));
                var calagemvm = new RecomendacaoCalagemViewModel(Navigation, calagem.Id);
                g.DrawString(calagemvm.QuantidadeCal.ToString(), textFont, new PdfSolidBrush(_black), new PointF(475, y));
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
                var corretivavm = new AdubacaoCorretivaViewModel(Navigation, _analise.Id);
                g.DrawString(corretivavm.P2O5, textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString(corretivavm.K2O, textFont, new PdfSolidBrush(_black), new PointF(175, y));
            }

            if (HasSemeaduraCalculation)
            {
                y += 50;
                g.DrawString("Recomendação de Adubagem de Semeadura", txf, new PdfSolidBrush(_black), new PointF(25, y));

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
                var semeadura = _realm.All<Semeadura>().FirstOrDefault(s => s.AnaliseId.Equals(Analise.Id));
                var semeaduravm = new RecomendacaoSemeaduraViewModel(Navigation, _analise.Id, semeadura.Expectativa,
                    semeadura.Cultura);
                g.DrawString(semeadura.Cultura, textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString(semeadura.Expectativa.ToString(), textFont, new PdfSolidBrush(_black), new PointF(125, y));
                g.DrawString(semeaduravm.N, textFont, new PdfSolidBrush(_black), new PointF(225, y));
                g.DrawString(semeaduravm.P205, textFont, new PdfSolidBrush(_black), new PointF(325, y));
                g.DrawString(semeaduravm.K20, textFont, new PdfSolidBrush(_black), new PointF(475, y));
            }

            if (HasCoberturaCalculation)
            {
                y += 50;
                g.DrawString("Recomendação da Adubagem de Cobertura", txf, new PdfSolidBrush(_black), new PointF(25, y));
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
                var cobertura = _realm.All<Semeadura>().FirstOrDefault(s => s.AnaliseId.Equals(Analise.Id));
                var coberturavm = new AdubacaoCoberturaViewModel(Navigation, _analise.Id);
                g.DrawString(cobertura.Cultura, textFont, new PdfSolidBrush(_black), new PointF(25, y));
                g.DrawString(cobertura.Expectativa.ToString(), textFont, new PdfSolidBrush(_black), new PointF(125, y));
                g.DrawString(coberturavm.N, textFont, new PdfSolidBrush(_black), new PointF(225, y));
                g.DrawString(coberturavm.P2O5, textFont, new PdfSolidBrush(_black), new PointF(325, y));
                g.DrawString(coberturavm.K2O, textFont, new PdfSolidBrush(_black), new PointF(475, y));
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

        private void DrawHeader(PdfGraphics page, string title)
        {
            DrawImage(page);

            PdfFont headerFont = new PdfStandardFont(PdfFontFamily.Helvetica, 16, PdfFontStyle.Bold);
            PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.Helvetica, 12, PdfFontStyle.Bold);
            PdfFont textFont = new PdfStandardFont(PdfFontFamily.Helvetica, 10);

            page.DrawString(title, headerFont, new PdfSolidBrush(_black), new PointF(110, 20));
            page.DrawString(_analise.Talhao.Fazenda.ToString(), subHeadingFont, new PdfSolidBrush(_black),
                new PointF(110, 52));
            page.DrawString("Talhão " + _analise.Talhao, textFont, new PdfSolidBrush(_black), new PointF(110, 74));
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

        #endregion
    }
}