namespace ProjektPraktyka_MiniDziennik
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Pages.StronaGlownaPage), typeof(Pages.StronaGlownaPage));
            Routing.RegisterRoute(nameof(Pages.ListaWpisowPage), typeof(Pages.ListaWpisowPage));
            Routing.RegisterRoute(nameof(Pages.DodajEdytujWpisPage), typeof(Pages.DodajEdytujWpisPage));
            Routing.RegisterRoute(nameof(Pages.StatystykiWagiPage), typeof(Pages.StatystykiWagiPage));
            Routing.RegisterRoute(nameof(Pages.SzczegolyWpisuPage), typeof(Pages.SzczegolyWpisuPage));

        }
    }
}
