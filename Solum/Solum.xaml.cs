using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Realms;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Solum
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();


            var r = Realm.GetInstance();
            if (!r.All<Estado>().Any())
                CarregarEstadoCidades(r);

            var isUsuarioLogado = VerificaLogin();
            if (!isUsuarioLogado)
                MainPage = new NavigationPage(new LoginPage())
                {
                    BackgroundColor = Color.Transparent,
                    BarTextColor = Color.Black
                };
            else
                MainPage = new RootPage();
        }

        private bool VerificaLogin()
        {
            var loggedUser = Realm.GetInstance().All<Usuario>().FirstOrDefault();
            return loggedUser != default(Usuario);
        }

        private void CarregarEstadoCidades(Realm realm)
        {
            var assembly = typeof(SyncService).GetTypeInfo().Assembly;

            var estadoStream = assembly.GetManifestResourceStream("Solum.Estados.csv");
            var cidadeStream = assembly.GetManifestResourceStream("Solum.Cidades.csv");

            var config = new CsvConfiguration
            {
                Delimiter = ";",
                HasHeaderRecord = true,
                IgnoreHeaderWhiteSpace = true,
                TrimHeaders = true,
                TrimFields = true,
                Encoding = Encoding.UTF8
            };
            var estadoReader = new CsvReader(new StreamReader(estadoStream), config);
            estadoReader.Configuration.RegisterClassMap<EstadoCsvMapper>();


            var cidadeReader = new CsvReader(new StreamReader(cidadeStream), config);
            cidadeReader.Configuration.RegisterClassMap<CidadeCsvMapper>();

            var estados = new Collection<Estado>();
            var cidades = new Collection<Cidade>();

            while (estadoReader.Read())
                estados.Add(estadoReader.GetRecord<Estado>());
            while(cidadeReader.Read())
                cidades.Add(cidadeReader.GetRecord<Cidade>());

            using (var trans = realm.BeginWrite())
            {
                foreach (var e in estados)
                {
                    realm.Add(e, true);
                    var result = cidades.Where(x => x.EstadoId.Equals(e.Id));
                    foreach (var c in result)
                    {
                        c.Estado = e;
                        realm.Add(c, true);
                    }
                }
                trans.Commit();
            }
        }
    }

    public sealed class EstadoCsvMapper : CsvClassMap<Estado>
    {
        public EstadoCsvMapper()
        {
            Map(s => s.Id).Index(0);
            Map(s => s.Nome).Index(1);
            Map(s => s.Uf).Index(2);
        }
    }

    public sealed class CidadeCsvMapper : CsvClassMap<Cidade>
    {
        public CidadeCsvMapper()
        {
            Map(s => s.Id).Index(0);
            Map(s => s.EstadoId).Index(1);
            Map(s => s.Nome).Index(2);
        }
    }
}