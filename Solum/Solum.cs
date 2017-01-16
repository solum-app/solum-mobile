using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Realms;
using Solum.Effects;
using Solum.Models;
using Solum.Pages;
using Solum.Service;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Solum
{
    public class App : Application
    {
        public App()
        {
            var r = Realm.GetInstance();
            if(!r.All<Estado>().Any())
                CarregarEstadoCidades(r);
            var isUsuarioLogado = VerificaLogin();
            if (!isUsuarioLogado)
            {
                var color = Color.Black;
                if (Device.OS == TargetPlatform.Android)
                    DependencyService.Get<IStatusBarColor>().SetColor(color);
                MainPage = new NavigationPage(new LoginPage())
                {
                    BackgroundColor = Color.Transparent,
                    BarTextColor = Color.Black
                };
            }
            else
            {
                MainPage = new RootPage();
            }
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

            var config = new CsvConfiguration() {Delimiter = ";", HasHeaderRecord = true, IgnoreHeaderWhiteSpace = true, TrimHeaders = true, TrimFields = true, Encoding = Encoding.UTF8};
            var estadoReader = new CsvReader(new StreamReader(estadoStream), config);
            estadoReader.Configuration.RegisterClassMap<EstadoCsvMapper>();
            

            var cidadeReader = new CsvReader(new StreamReader(cidadeStream), config);
            cidadeReader.Configuration.RegisterClassMap<CidadeCsvMapper>();
            
            var estados = estadoReader.GetRecords<Estado>().AsQueryable().OrderBy(x => x.Nome).ToList();
            var cidades = cidadeReader.GetRecords<Cidade>().AsQueryable().OrderBy(x => x.EstadoId).ToList();

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
            Map(s => s.Id).Name("Id");
            Map(s => s.Nome).Name("Nome");
            Map(s => s.Uf).Name("Uf");
        }
    }

    public sealed class CidadeCsvMapper : CsvClassMap<Cidade>
    {
        public CidadeCsvMapper()
        {
            Map(s => s.Id).Name("Id");
            Map(s => s.Nome).Name("Nome");
            Map(s => s.EstadoId).Name("EstadoId");
        }
    }
}