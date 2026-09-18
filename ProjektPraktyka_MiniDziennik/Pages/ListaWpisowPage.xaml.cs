using System;
using System.Collections.Generic;
using System.Text;

namespace ProjektPraktyka_MiniDziennik.Pages
{
    [QueryProperty(nameof(Kategoria), "kategoria")]
    public partial class ListaWpisowPage : ContentPage
    {
        public string Kategoria
        {
            set
            {
                KategoriaLabel.Text = value;
                LadowanieDanych(value);
            }
        }

        public ListaWpisowPage()
        {
            InitializeComponent();
        }

        private void LadowanieDanych(string kategoria)
        {
            if (kategoria == "WAGA")
            {
                WpisyListView.ItemsSource = new List<WpisModel>
                {
                    new WpisModel { Wartosc = "78,5 kg", DataWpisu = "Dzisiaj, 07:10" },
                    new WpisModel { Wartosc = "78,9 kg", DataWpisu = "Wczoraj, 19:30" }
                };
            }
            else if (kategoria == "NOTATKI")
            {
                WpisyListView.ItemsSource = new List<WpisModel>
                {
                    new WpisModel { Wartosc = "Trening zrobiony", DataWpisu = "Wczoraj, 18:00" },
                    new WpisModel { Wartosc = "Kupić odżywkę białkową", DataWpisu = "3 dni temu" }
                };
            }
        }

        private async void OnWstecz_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    public class WpisModel
{
    public string Wartosc { get; set; }
    public string DataWpisu { get; set; }
}

}
