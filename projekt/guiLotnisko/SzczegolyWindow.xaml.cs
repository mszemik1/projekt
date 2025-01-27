using lotnisko;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace guiLotnisko
{
  
    public partial class SzczegolyWindow : Window
    {
        private Samolot samolot;
        private MainWindow mainWindow;
        public ListBox pasazerowieList;
      

        public SzczegolyWindow(Samolot samolot, MainWindow window)
        {
            InitializeComponent();
            this.samolot = samolot;
            mainWindow = window;


            if (samolot.Pasazerowie != null && samolot.Pasazerowie.Any())
            {
                PasazerowieList.ItemsSource = samolot.Pasazerowie;
            }
            else
            {
                PasazerowieList.ItemsSource = new List<string> { "Brak pasażerów" };
            }

        }

        public Lotnisko lotniskoKrakow { get; private set; }

        private void UsunPasazeraButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasazerowieList.SelectedIndex >= 0)
            {

                int wybranyIndex = PasazerowieList.SelectedIndex;
                samolot.Pasazerowie.RemoveAt(wybranyIndex);

                PasazerowieList.ItemsSource = null;  
                PasazerowieList.ItemsSource = samolot.Pasazerowie;
                mainWindow.SzczegolyTextBox.Text = samolot.ToString();

                MessageBox.Show("Pasażer został usunięty.", "Usunięto pasażera", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Nie wybrano pasażera do usunięcia.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DodajPasazeraButton_Click(object sender, RoutedEventArgs e)
        {
            if (samolot.Pasazerowie.Count < samolot.maxLiczbaPasazerow)
            {
                DodajPasazeraWindow dodajPasazeraWindow = new DodajPasazeraWindow(samolot);
                dodajPasazeraWindow.ShowDialog();
                PasazerowieList.ItemsSource = null;
                PasazerowieList.ItemsSource = samolot.Pasazerowie;
                mainWindow.SzczegolyTextBox.Text = samolot.ToString();
            }
            else
            {
                MessageBox.Show("Nie można dodać pasażera, ponieważ wszystkie miejsca w samolocie są zajęte.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

    }
}
