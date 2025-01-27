using lotnisko;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace guiLotnisko
{
    
    public partial class MainWindow : Window
    {
        public Lotnisko lotniskoKrakow { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            ZaktualizujAktualnyCzas();
            lotniskoKrakow = new Lotnisko("Kraków", 50);
            OdświeżLoty();
        }

        private void OdświeżLoty()
        {
            PrzylotyList.ItemsSource = null;
            PrzylotyList.ItemsSource = lotniskoKrakow.Przyloty.Select(s => s.lot).ToList();

            OdlotyList.ItemsSource = null;
            OdlotyList.ItemsSource = lotniskoKrakow.Odloty.Select(s => s.lot).ToList();

            SzczegolyTextBox.Text = string.Empty;

        }

        private void PrzesunCzasButton_Click(object sender, RoutedEventArgs e)
        {
            lotniskoKrakow.RuchCzasu();
            OdświeżLoty();
            ZaktualizujAktualnyCzas();
        }

        private void ZaktualizujAktualnyCzas()
        {
            string czas = AktualnyCzas.aktualnyCzas.ToString(@"hh\:mm");
            AktualnyCzasTextBlock.Text = czas;
        }

        private void PrzylotyList_Click(object sender, SelectionChangedEventArgs e)
        {   
            if (PrzylotyList.SelectedIndex >= 0)
            {
                int wybranyIndex = PrzylotyList.SelectedIndex;
                Samolot wybranySamolot = lotniskoKrakow.Przyloty[wybranyIndex];
                SzczegolyTextBox.Text = wybranySamolot.ToString();
            }
            else
            {
                SzczegolyTextBox.Text = "Nie wybrano lotu.";
            }
        }



        private void OdlotyList_Click(object sender, SelectionChangedEventArgs e)
        {
            if (OdlotyList.SelectedIndex >= 0)
            {
                int wybranyIndex = OdlotyList.SelectedIndex;
                Samolot wybranySamolot = lotniskoKrakow.Odloty[wybranyIndex];
                SzczegolyTextBox.Text = wybranySamolot.ToString();
                //SzczegolyTextBox.Text = wybranySamolot.ZaladunekBagazy(true);
                
            }
            else
            {
                SzczegolyTextBox.Text = "Nie wybrano lotu.";
            }
        }

        private void WyzerujButton_Click(object sender, RoutedEventArgs e)
        {
            OdświeżLoty();
            SzczegolyTextBox.Text = string.Empty;
        }


        private void SzczegolyWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (OdlotyList.SelectedIndex >= 0)
            {
                int wybranyIndex = OdlotyList.SelectedIndex;
                Samolot wybranySamolot = lotniskoKrakow.Odloty[wybranyIndex];

                SzczegolyWindow szczegolyWindow = new SzczegolyWindow(wybranySamolot, this);
                szczegolyWindow.Show();
            }

            else if (PrzylotyList.SelectedIndex >= 0)
            {
                int wybranyIndex = PrzylotyList.SelectedIndex;
                Samolot wybranySamolot = lotniskoKrakow.Przyloty[wybranyIndex];

                SzczegolyWindow szczegolyWindow = new SzczegolyWindow(wybranySamolot, this);
                szczegolyWindow.Show();
            }

            else
            {
                MessageBox.Show("Nie wybrano lotu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }


        }

       
        private void UsunLotButton_Click(Object sender, RoutedEventArgs e)
        {
            if (PrzylotyList.SelectedIndex >= 0)
            {
                int wybranyIndex = PrzylotyList.SelectedIndex;
                Samolot wybranySamolot = lotniskoKrakow.Przyloty[wybranyIndex];

                //Przekierować można tylko samolot, który nie wylądował
                if (wybranySamolot.lot?.faza == "Przylot")
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Czy na pewno chcesz przekierować wybrany lot?",
                        "Potwierdzenie przekierowania",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        lotniskoKrakow.Przyloty.RemoveAt(wybranyIndex);
                        OdświeżLoty();

                        MessageBox.Show($"Lot został przekierowany do Katowic.", "Przekierowano lot", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Nie można przekierować lotu, który jest na lotnisku", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Nie wybrano przylotu.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


           



    }

    }



