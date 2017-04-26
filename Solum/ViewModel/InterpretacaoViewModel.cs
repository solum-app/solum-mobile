using System;
using Solum.Handlers;
using Solum.Models;
using Solum.Service;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class InterpretacaoViewModel : BaseViewModel
    {
        private Analise _analise;
        private DateTimeOffset _date;
        private string _fazendaName;
        private string _interpretacaoCa;
        private string _interpretacaoCaK;
        private string _interpretacaoCtc;
        private string _interpretacaoK;
        private string _interpretacaoM;
        private string _interpretacaoMg;
        private string _interpretacaoMgK;
        private string _interpretacaoMo;
        private string _interpretacaoP;
        private string _interpretacaoPh;
        private string _interpretacaoTextura;
        private string _interpretacaoV;

        private Nivel _nivelCa;
        private Nivel _nivelCaK;
        private Nivel _nivelCtc;
        private Nivel _nivelK;
        private Nivel _nivelM;
        private Nivel _nivelMg;
        private Nivel _nivelMgK;
        private Nivel _nivelMo;
        private Nivel _nivelP;
        private Nivel _nivelPh;
        private Nivel _nivelV;
        private string _talhaoName;
        private Textura _textura;

        public InterpretacaoViewModel(INavigation navigation, string analiseId) : base(navigation)
        {
            AzureService.Instance.FindAnaliseAsync(analiseId)
                .ContinueWith(t =>
                {
                    Analise = t.Result;
                    Talhao talhao;
                    Fazenda fazenda;
                    AzureService.Instance.FindTalhaoAsync(Analise.TalhaoId)
                        .ContinueWith(tt =>
                        {
                            talhao = tt.Result;
                            AzureService.Instance.FindFazendaAsync(talhao.FazendaId)
                                .ContinueWith(ttt =>
                                {
                                    fazenda = ttt.Result;
                                    FazendaName = fazenda.Nome;
                                });

                            TalhaoName = talhao.Nome;
                        });

                    Date = Analise.DataRegistro;

                    Textura = Interpretador.Textura(Analise.Argila, Analise.Silte);
                    NivelPh = Interpretador.NivelPotencialHidrogenico(Analise.PotencialHidrogenico);
                    NivelP = Interpretador.NiveFosforo(Analise.Fosforo, Textura);
                    NivelK = Interpretador.NivelPotassio(Analise.Potassio, Analise.CTC);
                    NivelCa = Interpretador.NivelCalcio(Analise.Calcio);
                    NivelMg = Interpretador.NivelMagnesio(Analise.Magnesio);
                    NivelMo = Interpretador.NivelMateriaOrganica(Analise.MateriaOrganica, Textura);
                    NivelCtc = Interpretador.NivelCtc(Analise.CTC, Textura);
                    NivelV = Interpretador.NivelV(Analise.V);
                    NivelM = Interpretador.NivelM(Analise.M);
                    NivelCaK = Interpretador.NivelCalcioPotassio(Analise.CaK);
                    NivelMgk = Interpretador.NivelMagnesioPotassio(Analise.MgK);

                    InterpretacaoTextura = TexturaConverter(Textura);
                    InterpretacaoPh = NivelPhConverter(NivelPh);
                    InterpretacaoP = NivelConverter(NivelP);
                    InterpretacaoK = NivelConverter(NivelK);
                    InterpretacaoCa = NivelConverter(NivelCa);
                    InterpretacaoMg = NivelConverter(NivelMg);
                    InterpretacaoCaK = NivelConverter(NivelCaK);
                    InterpretacaoMgK = NivelConverter(NivelMgk);
                    InterpretacaoM = NivelConverter(NivelM);
                    InterpretacaoV = NivelConverter(NivelV);
                    InterpretacaoCtc = NivelConverter(NivelCtc);
                    InterpretacaoMo = NivelConverter(NivelMo);


                    Analise.DataInterpretacao = DateTimeOffset.Now;
                    Analise.WasInterpreted = true;
                    AzureService.Instance.UpdateAnaliseAsync(Analise);
                });
        }


        public Nivel NivelCa
        {
            get => _nivelCa;
            set => SetPropertyChanged(ref _nivelCa, value);
        }

        public Nivel NivelCaK
        {
            get => _nivelCaK;
            set => SetPropertyChanged(ref _nivelCaK, value);
        }

        public Nivel NivelCtc
        {
            get => _nivelCtc;
            set => SetPropertyChanged(ref _nivelCtc, value);
        }

        public Nivel NivelK
        {
            get => _nivelK;
            set => SetPropertyChanged(ref _nivelK, value);
        }

        public Nivel NivelM
        {
            get => _nivelM;
            set => SetPropertyChanged(ref _nivelM, value);
        }

        public Nivel NivelMg
        {
            get => _nivelMg;
            set => SetPropertyChanged(ref _nivelMg, value);
        }

        public Nivel NivelMgk
        {
            get => _nivelMgK;
            set => SetPropertyChanged(ref _nivelMgK, value);
        }

        public Nivel NivelMo
        {
            get => _nivelMo;
            set => SetPropertyChanged(ref _nivelMo, value);
        }

        public Nivel NivelP
        {
            get => _nivelP;
            set => SetPropertyChanged(ref _nivelP, value);
        }

        public Nivel NivelPh
        {
            get => _nivelPh;
            set => SetPropertyChanged(ref _nivelPh, value);
        }

        public Nivel NivelV
        {
            get => _nivelV;
            set => SetPropertyChanged(ref _nivelV, value);
        }

        public Textura Textura
        {
            get => _textura;
            set => SetPropertyChanged(ref _textura, value);
        }

        public Analise Analise
        {
            get => _analise;
            set => SetPropertyChanged(ref _analise, value);
        }

        public string FazendaName
        {
            get => _fazendaName;
            set => SetPropertyChanged(ref _fazendaName, value);
        }

        public string TalhaoName
        {
            get => _talhaoName;
            set => SetPropertyChanged(ref _talhaoName, value);
        }

        public DateTimeOffset Date
        {
            get => _date;
            set => SetPropertyChanged(ref _date, value);
        }

        public string InterpretacaoTextura
        {
            get => _interpretacaoTextura;
            set => SetPropertyChanged(ref _interpretacaoTextura, value);
        }

        public string InterpretacaoPh
        {
            get => _interpretacaoPh;
            set => SetPropertyChanged(ref _interpretacaoPh, value);
        }

        public string InterpretacaoP
        {
            get => _interpretacaoP;
            set => SetPropertyChanged(ref _interpretacaoP, value);
        }

        public string InterpretacaoK
        {
            get => _interpretacaoK;
            set => SetPropertyChanged(ref _interpretacaoK, value);
        }

        public string InterpretacaoCa
        {
            get => _interpretacaoCa;
            set => SetPropertyChanged(ref _interpretacaoCa, value);
        }

        public string InterpretacaoMg
        {
            get => _interpretacaoMg;
            set => SetPropertyChanged(ref _interpretacaoMg, value);
        }

        public string InterpretacaoCaK
        {
            get => _interpretacaoCaK;
            set => SetPropertyChanged(ref _interpretacaoCaK, value);
        }

        public string InterpretacaoMgK
        {
            get => _interpretacaoMgK;
            set => SetPropertyChanged(ref _interpretacaoMgK, value);
        }

        public string InterpretacaoM
        {
            get => _interpretacaoM;
            set => SetPropertyChanged(ref _interpretacaoM, value);
        }

        public string InterpretacaoV
        {
            get => _interpretacaoV;
            set => SetPropertyChanged(ref _interpretacaoV, value);
        }

        public string InterpretacaoCtc
        {
            get => _interpretacaoCtc;
            set => SetPropertyChanged(ref _interpretacaoCtc, value);
        }

        public string InterpretacaoMo
        {
            get => _interpretacaoMo;
            set => SetPropertyChanged(ref _interpretacaoMo, value);
        }

        private string NivelPhConverter(Nivel nivel)
        {
            switch (nivel)
            {
                case Nivel.MuitoBaixo:
                    return "Acidez Muito Baixa";
                case Nivel.Baixo:
                    return "Acidez Baixa";
                case Nivel.Medio:
                    return "Acidez Média";
                case Nivel.Adequado:
                    return "Acidez Adequeada";
                case Nivel.Alto:
                    return "Acidez Alta";
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

        private string TexturaConverter(Textura textura)
        {
            switch (textura)
            {
                case Textura.Arenosa:
                    return "Arenosa";
                case Textura.Media:
                    return "Média";
                case Textura.Argilosa:
                    return "Argilosa";
                case Textura.MuitoArgilosa:
                    return "Muito Argilosa";
                default:
                    return "";
            }
        }
    }
}