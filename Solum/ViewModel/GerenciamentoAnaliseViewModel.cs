using System.Windows.Input;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.ViewModel
{
    public class GerenciamentoAnaliseViewModel : BaseViewModel
    {
        public GerenciamentoAnaliseViewModel(INavigation navigation, Analise analise) : base(navigation)
        {
            _analise = analise;
        }

        private ICommand _abrirTelaInterpretacaoCommand;
        private ICommand _abrirTelaCalagemCommand;
        private ICommand _abrirTelaCorretivaCommand;
        private ICommand _abrirTelaSemeaduraCommand;
        private ICommand _abrirTelaCoberturaCommand;

        private Analise _analise;

    }
}