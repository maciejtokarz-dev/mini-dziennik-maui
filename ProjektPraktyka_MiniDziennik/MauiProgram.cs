using LiveChartsCore.SkiaSharpView.Maui;
using Microsoft.Extensions.Logging;
using ProjektPraktyka_MiniDziennik.Services;
using ProjektPraktyka_MiniDziennik.ViewModels;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace ProjektPraktyka_MiniDziennik
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseSkiaSharp() // To trzeba było dodać
                .UseLiveCharts() // To trzeba było dodać
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<WpisService>();

            builder.Services.AddTransient<DodajEdytujWpisViewModel>();
            builder.Services.AddTransient<ListaWpisowViewModel>();
            builder.Services.AddTransient<Pages.StatystykiWagiPage>();
            builder.Services.AddTransient<ViewModels.StatystykiWagiViewModel>();
            builder.Services.AddTransient<SzczegolyWpisuViewModel>();
            builder.Services.AddTransient<StronaGlownaViewModel>();

            return builder.Build();
        }
    }
}
