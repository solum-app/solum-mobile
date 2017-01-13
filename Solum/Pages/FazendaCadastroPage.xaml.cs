using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Solum.Models;
using Xamarin.Forms;

namespace Solum.Pages
{
    public partial class FazendaCadastroPage : ContentPage
    {
        public FazendaCadastroPage()
        {
            InitializeComponent();
        }

        public FazendaCadastroPage(Fazenda fazenda)
        {
            InitializeComponent();
        }
    }
}
