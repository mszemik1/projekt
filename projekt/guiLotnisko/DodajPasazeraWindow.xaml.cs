using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using lotnisko;

namespace guiLotnisko
{
    public partial class DodajPasazeraWindow : Window
    {
        private Samolot samolot;
        private ObservableCollection<Bagaz> bagaze;


        public DodajPasazeraWindow(Samolot samolot)
        {
            InitializeComponent();
            bagaze = new ObservableCollection<Bagaz>();
            BagazeList.ItemsSource = bagaze;
            this.samolot = samolot;
            if (samolot.liniaLotnicza.czyTaniaLinia)
            {
                KlasaComboBox.Items.Clear();
                KlasaComboBox.Items.Add("ekonomiczna");
            }
            KlasaComboBox.Text = "ekonomiczna";
            CenaLotuBox.Text = $"{samolot.lot.bazowaCena:c}";

            TypBagazuComboBox.Text = "rejestrowany";
            detaleLotu.Text = $"Lot {samolot.lot.miastoWylotu}-{samolot.lot.miastoPrzylotu}, {samolot.liniaLotnicza.nazwa}";
        }



        private void KlasaComboBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            AktualizacjaCeny();
        }


        // Dodawanie pasażera
        private void DodajPasazeraButton_Click(object sender, RoutedEventArgs e)
        {
            if (samolot.Pasazerowie.Count >= samolot.maxLiczbaPasazerow)
            {
                MessageBox.Show("Nie można dodać pasażera, ponieważ wszystkie miejsca w samolocie są zaajęte", "Uwaga", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string imie = ImieTextBox.Text;
            string nazwisko = NazwiskoTextBox.Text;
            EnumKlasa klasa = (EnumKlasa)Enum.Parse(typeof(EnumKlasa), KlasaComboBox.Text);
            if (!string.IsNullOrEmpty(imie) && !string.IsNullOrEmpty(nazwisko))
            {
                if (!DataUrodzeniaDatePicker.SelectedDate.HasValue)
                {
                    MessageBox.Show("Nie wybrano daty urodzenia pasażera.", "Brak daty urodzenia", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                DateTime data = DataUrodzeniaDatePicker.SelectedDate.Value;
                if (data > DateTime.Today)
                {
                    MessageBox.Show("Błędna data urodzenia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (BagazeList.Items.Count == 0)
                {
                    MessageBoxResult result = MessageBox.Show("Nie dodano bagaży. Czy chcesz kontynuować?",
                                               "Brak bagaży",
                                               MessageBoxButton.YesNo,
                                               MessageBoxImage.Information);
                    if (result == MessageBoxResult.No) return;
                }
                string cena = CenaLotuBox.Text;
                if(!double.TryParse(cena.Replace(" ", "").Replace(",", ".").Replace("zł", ""), NumberStyles.Any, CultureInfo.InvariantCulture, out double cenaBiletu) || cenaBiletu <=0)
                {
                    MessageBox.Show("Niepoprawna cena!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Bilet bilet = new Bilet(klasa, cenaBiletu, samolot.lot);
                List<Bagaz> bagazeList = BagazeList.Items.Cast<Bagaz>().ToList();
                Pasazer nowyPasazer = new Pasazer(imie, nazwisko, bilet, data, bagazeList);
                samolot.Pasazerowie.Add(nowyPasazer);
                samolot.SortujPasazerow();
                samolot.DodajBagazePasazera(nowyPasazer);
                MessageBox.Show("Pomyślnie dodano pasażera.", "Informacja", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Wpisz imię i nazwisko pasażera", "Błędne dane", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AnulujButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Czy na pewno chcesz przerwać dodawanie pasażera?", "Ostrzeżenie", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            Close();
        }

        private void DodajBagazButton_Click(object sender, RoutedEventArgs e)
        {
            string waga = WagaTextBox.Text.Trim();
            waga = waga.Replace(",", ".");
            enumRodzajBagazu typ = (enumRodzajBagazu)Enum.Parse(typeof(enumRodzajBagazu), TypBagazuComboBox.Text);

            if (double.TryParse(waga, NumberStyles.Any, CultureInfo.InvariantCulture, out double wagaBagazu))
            {
                if (wagaBagazu <= 0)
                {
                    MessageBox.Show("Niepoprawna waga! Wpisz dodatnią liczbę.", "Błędna waga bagażu.", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Sprawdzenie, czy bagaż nie jest zbyt ciężki
                if (typ == enumRodzajBagazu.podreczny && wagaBagazu > samolot.liniaLotnicza.maxWagaBagazuPodrecznego)
                {
                    MessageBox.Show($"Nie można dodać zbyt ciężkiego bagażu podręcznego. Maksymalna waga dopuszczona przez linię {samolot.liniaLotnicza.nazwa} wynosi {samolot.liniaLotnicza.maxWagaBagazuPodrecznego} kg.", "Zbyt ciężki bagaż", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (typ == enumRodzajBagazu.rejestrowany && wagaBagazu > samolot.liniaLotnicza.maxWagaBagazuRejestrowanego)
                {
                    MessageBox.Show($"Nie można dodać zbyt ciężkiego bagażu rejestrowanego. Maksymalna waga dopuszczona przez linię {samolot.liniaLotnicza.nazwa} wynosi {samolot.liniaLotnicza.maxWagaBagazuRejestrowanego} kg.", "Zbyt ciężki bagaż", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    bagaze.Add(new Bagaz(wagaBagazu, typ));
                    AktualizacjaCeny();
                }
            }
            else
            {
                MessageBox.Show("Niepoprawna waga!", "Błędna waga bagażu.", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void UsunBagazButton_Click(object sender, RoutedEventArgs e)
        {
            if (BagazeList.SelectedIndex >= 0)
            {
                int wybranyIndex = BagazeList.SelectedIndex;
                bagaze.RemoveAt(wybranyIndex);
                AktualizacjaCeny();
            }
            else
            {
                MessageBox.Show("Nie wybrano bagażu do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void AktualizacjaCeny()
        {
            double cena = samolot.lot.bazowaCena;
            string klasa = KlasaComboBox.Text;


            double cenaMnożnik = klasa switch
            {
                "premium" => 1.5,
                "biznes" => 2,
                "pierwsza" => 3,
                _ => 1
            };
            cena *= cenaMnożnik;

            if (bagaze.Count == 0) return;
            foreach (Bagaz bagaz in bagaze)
            {
                if (bagaz.rodzaj == enumRodzajBagazu.podreczny)
                {
                    cena += 8.5 * bagaz.Waga;
                }
                else { cena += 16.2 * bagaz.Waga; }
            }
            CenaLotuBox.Text = $"{cena:c}";
        }
    }

    
}

