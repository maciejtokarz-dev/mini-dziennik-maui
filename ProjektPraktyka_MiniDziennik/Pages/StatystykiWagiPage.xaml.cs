using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;

namespace ProjektPraktyka_MiniDziennik.Pages;

public partial class StatystykiWagiPage : ContentPage
{
    public ISeries[] SerieWykresu { get; set; }

    public Axis[] OsX { get; set; }

    public Axis[] OsY { get; set; }

    public StatystykiWagiPage()
    {
        InitializeComponent();

        SerieWykresu =
        [
            new LineSeries<DateTimePoint>
            {
                Values =
                [
                    new DateTimePoint(new DateTime(2026, 9, 10), 69.2),
                    new DateTimePoint(new DateTime(2026, 9, 11), 68.9),
                    new DateTimePoint(new DateTime(2026, 9, 12), 69.1),
                    new DateTimePoint(new DateTime(2026, 9, 13), 68.7),
                    new DateTimePoint(new DateTime(2026, 9, 14), 68.8),
                    new DateTimePoint(new DateTime(2026, 9, 15), 68.5),
                    new DateTimePoint(new DateTime(2026, 9, 16), 68.7),
                    new DateTimePoint(new DateTime(2026, 9, 17), 70)
                ],

                Fill = null,

                Stroke = new SolidColorPaint(
                    new SKColor(240, 122, 94))
                {
                    StrokeThickness = 3
                },

                GeometryFill = new SolidColorPaint(
                    new SKColor(240, 122, 94)),

                GeometryStroke = new SolidColorPaint(
                    SKColors.White)
                {
                    StrokeThickness = 2
                },

                GeometrySize = 9
            }
        ];

        OsX =
        [
            new DateTimeAxis(
                TimeSpan.FromDays(2),
                data => data.ToString("dd.MM"))
        ];

        OsY =
        [
            new Axis
            {
                MinLimit = 68,
                MaxLimit = 70,

                MinStep = 0.5,

                Labeler = value => value.ToString("0.0")
            }
        ];

        BindingContext = this;
    }
}