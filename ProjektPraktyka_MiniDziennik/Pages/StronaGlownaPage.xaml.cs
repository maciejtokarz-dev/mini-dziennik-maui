using System;
using System.Collections.Generic;
using ProjektPraktyka_MiniDziennik.Pages;
using System.Text;

namespace ProjektPraktyka_MiniDziennik.Pages
{
    public partial class StronaGlownaPage : ContentPage
    {
        public StronaGlownaPage()
        {
            InitializeComponent();
        }

        private async void OnDodajWpisClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(DodajEdytujWpisPage));
        }

        private async void OnWagaCard_Clicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(ListaWpisowPage)}?kategoria=WAGA");
        }
        private async void OnNotatkiCard_Clicked(object sender, TappedEventArgs e)
        {
            await Shell.Current.GoToAsync($"{nameof(ListaWpisowPage)}?kategoria=NOTATKI");
        }
    }

}
