using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace lotnisko
{
    public abstract class Samolot
    {
        public string? model; // np A320, B737
        public int wagaPustegoSamolotu;
        public int maxLiczbaPasazerow; // maksymalna liczba pasażerów która może być na pokładzie
        List<Pasazer> pasazerowie;
        public int maxPaliwo; // maksymalna pojemność zbiorników paliwa (nie w litrach tylko w kg paliwa)
        double stanPaliwa; // ile kg paliwa jest obecnie
        List<Bagaz> bagaze;
        public int maxLacznaWaga; // maksymalna waga dopuszczalna, żeby samolot wystartował
        public LiniaLotnicza liniaLotnicza;
        public double spalaniePaliwa; // średnie spalanie paliwa - kg/h lotu
        public Lot? lot;
        public EnumKategoria kategoria;

        public Lot Lot { get; set; }
        public List<Pasazer> Pasazerowie
        {
            get { return pasazerowie; }
            set { pasazerowie = value; }
        }

        public object MiastoPrzylotu { get; set; }

        public Samolot(double stanPaliwa, LiniaLotnicza liniaLotnicza, Lot lot)
        {
            this.liniaLotnicza = liniaLotnicza;
            this.stanPaliwa = stanPaliwa;
            pasazerowie = new List<Pasazer>();
            bagaze = new List<Bagaz>();
            this.lot = lot;
        }

        public Samolot(double stanPaliwa, LiniaLotnicza liniaLotnicza)
        {
            this.liniaLotnicza = liniaLotnicza;
            this.stanPaliwa = stanPaliwa;
            pasazerowie = new List<Pasazer>();
            bagaze = new List<Bagaz>();
        }

        // liczba pasażerów
        public int LiczbaPasazerow() => pasazerowie.Count;

        // liczba pasażerów danej klasy
        public int LiczbaPasazerowKlasa(EnumKlasa klasa, List<Pasazer>? lista = null)
        {
            if (lista == null) return pasazerowie.Where(p => p.bilet.klasa == klasa).Count();
            else
            {
                return lista.Where(p => p.bilet.klasa == klasa).Count();
            }

        }

        // liczba bagaży danego rodzaju
        public int LiczbaBagazy(enumRodzajBagazu rodzaj, List<Bagaz>? listaBagazy = null)
        {
            if (listaBagazy == null) return bagaze.Where(b => b.rodzaj == rodzaj).Count();
            else
            {
                return listaBagazy.Where(b => b.rodzaj == rodzaj).Count();
            }
        }


        // waga wszystkich bagaży w samolocie
        public double WagaBagazy() => bagaze.Sum(b => b.Waga);

        // Całkowita waga samolotu - przyjmujemy że pasażer waży 72 kg
        public double WagaCalkowita() => wagaPustegoSamolotu + WagaBagazy() + stanPaliwa + LiczbaPasazerow() * 72;

        //Tankowanie - waga potrzebnego paliwa ustalona na podstawie spalania i czasu lotu oraz wagi samolotu z pasażerami i bagażami
        public void Tankowanie()
        {
            double potrzebnePaliwo = 1.25 * lot.czasLotu * spalaniePaliwa * (WagaCalkowita() - stanPaliwa) / wagaPustegoSamolotu;

            if (potrzebnePaliwo > stanPaliwa)
            {
                // jeśli potrzeba więcej paliwa, niż wynosi pojemność zbiorników
                if (potrzebnePaliwo > maxPaliwo)
                {
                    Console.WriteLine($"Dotankowano {Math.Round(maxPaliwo - stanPaliwa, 2)} kg paliwa.");
                    stanPaliwa = maxPaliwo;
                }
                else
                {
                    Console.WriteLine($"Dotankowano {Math.Round(potrzebnePaliwo - stanPaliwa, 2)} kg paliwa.");
                    stanPaliwa = potrzebnePaliwo;
                }
            }

        }

        // Boarding - na pokład wchodzą pasażerowie, równocześnie do samolotu ładowany jest ich bagaż
        // pod warunkiem, że jego waga jest nie większa, niż limit ustalony przez linię lotniczą
        public void Boarding(List<Pasazer> listaPasazerow, bool verbose = false)
        {
            // Sortujemy listę pasażerów wg priorytetu wejścia na pokład
            listaPasazerow.Sort();

            int roznica = listaPasazerow.Count - maxLiczbaPasazerow;
            StringBuilder sb = new StringBuilder();

            // jeśli bilet kupiło więcej pasażerów niż jest miejsc
            if (roznica > 0)
            {
                pasazerowie.AddRange(listaPasazerow.GetRange(0, maxLiczbaPasazerow));
                sb.AppendLine($" Z powodu overbookingu na pokład nie weszło {roznica} pasażerów, w tym:");
                // lista pasażerów, którzy nie weszli na pokład
                List<Pasazer> nieWeszliNaPoklad = listaPasazerow.GetRange(maxLiczbaPasazerow, roznica);

                // wypisanie liczby pasażerów, którzy nie weszli na pokład
                nieWeszliNaPoklad.Select(p => p.bilet.klasa).Distinct().ToList().
                    ForEach(kl => sb.AppendLine($"{LiczbaPasazerowKlasa(kl, nieWeszliNaPoklad)}: klasa {kl}"));
            }
            // jeśli liczba kupionych biletów nie przekracza liczby miejsc
            else
            {
                pasazerowie.AddRange(listaPasazerow);
            }
            sb.AppendLine($"Na pokład weszło {pasazerowie.Count} pasażerów, w tym:");

            // wypisanie, ile pasażerów danej klasy weszło na pokład
            pasazerowie.Select(p => p.bilet.klasa).Distinct().ToList().ForEach(kl => sb.AppendLine($"{LiczbaPasazerowKlasa(kl)}: klasa {kl}"));

            if (verbose) Console.WriteLine(sb.ToString());
        }

        // dodawanie bagaży pojedynczego pasażera
        public void DodajBagazePasazera(Pasazer p)
        {
            bagaze.AddRange(p.bagaze.Where(b => b.rodzaj == enumRodzajBagazu.rejestrowany &
            b.Waga <= liniaLotnicza.maxWagaBagazuRejestrowanego));

            bagaze.AddRange(p.bagaze.Where(b => b.rodzaj == enumRodzajBagazu.podreczny &
            b.Waga <= liniaLotnicza.maxWagaBagazuPodrecznego));
        }

        // sortowanie pasażerów
        public void SortujPasazerow()
        {
            pasazerowie.Sort();
        }

        // funkcja ładuje do samolotu bagaże wszystkich pasażerów, którzy są w samolocie, albo bagaże konkretnego pasażera (wtedy podajemy go jako argument)
        public void ZaladunekBagazy(bool verbose = false)
        {
            List<Bagaz> niezaladowaneBagaze = new List<Bagaz>();

            foreach (Pasazer p in pasazerowie)
            {
                // dodajemy wszystkie bagaże rejestrowane każdego pasażera, których waga nie przekracza limitu
                bagaze.AddRange(p.bagaze.Where(b => b.rodzaj == enumRodzajBagazu.rejestrowany &
                b.Waga <= liniaLotnicza.maxWagaBagazuRejestrowanego));

                // zbyt ciężkich nie ładujemy
                niezaladowaneBagaze.AddRange(p.bagaze.Where(b => b.rodzaj == enumRodzajBagazu.rejestrowany &
                b.Waga > liniaLotnicza.maxWagaBagazuRejestrowanego));

                // to samo dla bagaży podręcznych
                bagaze.AddRange(p.bagaze.Where(b => b.rodzaj == enumRodzajBagazu.podreczny &
                b.Waga <= liniaLotnicza.maxWagaBagazuPodrecznego));

                niezaladowaneBagaze.AddRange(p.bagaze.Where(b => b.rodzaj == enumRodzajBagazu.podreczny &
                b.Waga > liniaLotnicza.maxWagaBagazuPodrecznego));
            }
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Załadowano {LiczbaBagazy(enumRodzajBagazu.rejestrowany)} bagaży rejestrowanych i " +
                $"{LiczbaBagazy(enumRodzajBagazu.podreczny)} podręcznych.");
            if (niezaladowaneBagaze.Count > 0)
            {
                sb.AppendLine($"Z powodu przekroczenia limitów wagi nie załadowano " +
                $"{LiczbaBagazy(enumRodzajBagazu.rejestrowany, niezaladowaneBagaze)} bagaży rejestrowanych i " +
                $"{LiczbaBagazy(enumRodzajBagazu.podreczny, niezaladowaneBagaze)} podręcznych.");
            }

            // Wypisanie info o  bagażach 
            if (verbose) Console.WriteLine(sb.ToString());

        }


        // deboarding - samolot przylatujący
        public void Deboarding()
        {
            pasazerowie.Clear();
        }

        public void RozladunekBagazy()
        {
            bagaze.Clear();
        }
        public override string ToString()
        {
            return $"{model}, {liniaLotnicza.nazwa}, na pokładzie jest: {LiczbaPasazerow()} pasażerów.\n" +
                $"Stan paliwa: {Math.Round(stanPaliwa, 2)} kg, łączna waga: {Math.Round(WagaCalkowita(), 2)} kg\n" +
                $"Liczba bagaży: {LiczbaBagazy(enumRodzajBagazu.podreczny)} podręczne i " +
            $"{LiczbaBagazy(enumRodzajBagazu.rejestrowany)} rejestrowane";
        }

        // funkcja losująca - losuje pasażerów wraz z ich bagażami i biletami
        // jeśli czyPrzylot = true, to wstawia pasażerów i ich bagaże do samolotu, jeśli false - trzeba użyć w tym celu metody boarding
        public List<Pasazer> LosujPasazerow(bool czyPrzylot = false)
        {
            double cena = lot.bazowaCena;

            List<Pasazer> losowaniPasazerowie = new List<Pasazer>();

            Random rand = new Random();
            // generujemy losowo liczbę pasażerów, dopuszczamy overbooking 
            int liczbaPasazerow = rand.Next((int)Math.Floor(maxLiczbaPasazerow / 2.0)) + (int)(maxLiczbaPasazerow / 2.0) + 3;

            for (int i = 0; i < liczbaPasazerow; i++)
            {
                EnumKlasa klasa;
                // losowanie klasy - w przypadku tanich linii lotniczych pomijamy ten krok i ustawiamy klasę ekonomiczną
                if (liniaLotnicza.czyTaniaLinia) { klasa = EnumKlasa.ekonomiczna; }
                else
                {
                    int pomocnicza = rand.Next(101);
                    if (pomocnicza < 86) { klasa = EnumKlasa.ekonomiczna; }
                    else if (pomocnicza < 95) { klasa = EnumKlasa.premium; }
                    else if (pomocnicza < 99) { klasa = EnumKlasa.biznes; }
                    else { klasa = EnumKlasa.pierwsza; }
                }

                // losowanie ceny biletu - mnożenie przez liczbę odpowiadającą danej klasie w EnumKlasa zapewnia,
                // że ceny dla poszczególnych klas są różne
                double losowanaCenaBiletu = (cena + rand.Next((int)Math.Floor(cena / 5)) - cena / 10) * (int)klasa + rand.NextDouble();

                // tworzymy bilet
                Bilet bilet = new(klasa, losowanaCenaBiletu, lot);

                // lista bagaży pasażera 
                List<Bagaz> losowaneBagaze = new List<Bagaz>();

                // losowanie liczby bagaży podręcznych 
                int ileBagazyPodrecznych = (int)Math.Round(rand.NextDouble() + rand.NextDouble()); // liczba z przedziału [1,2]
                for (int j = 0; j < ileBagazyPodrecznych; j++)
                {
                    // losowanie wag bagaży
                    double losowanaWaga = rand.Next((int)Math.Floor(liniaLotnicza.maxWagaBagazuPodrecznego / 2)) +
                        +liniaLotnicza.maxWagaBagazuPodrecznego / 2.25 + 1 + rand.NextDouble(); // dopuszczamy zbyt ciężki bagaż podręczny
                    Bagaz bagaz = new(losowanaWaga, enumRodzajBagazu.podreczny);
                    losowaneBagaze.Add(bagaz);
                }

                // losowanie liczby bagaży rejestrowanych
                int ileBagazyRejestrowanych = (int)Math.Round(rand.NextDouble() + rand.NextDouble());
                for (int j = 0; j < ileBagazyRejestrowanych; j++)
                {
                    // losowanie wag bagaży
                    double losowanaWaga = rand.Next((int)Math.Floor(liniaLotnicza.maxWagaBagazuRejestrowanego / 2)) +
                        +liniaLotnicza.maxWagaBagazuRejestrowanego / 2 + 2; // dopuszczamy zbyt ciężki bagaż rejestrowany
                    Bagaz bagaz = new(losowanaWaga, enumRodzajBagazu.rejestrowany);
                    losowaneBagaze.Add(bagaz);
                }

                // tworzymy pasażera 
                Pasazer pasazer = new(bilet, losowaneBagaze);
                losowaniPasazerowie.Add(pasazer);
            }
            return losowaniPasazerowie;
        }

        // funkcja losująca - przypisuje samolotowi lot
        public void LosujLot(bool czyPrzylot = true)
        {
            // lista destynacji wraz z ich czasami lotów i bazowymi cenami biletów
            List<(string, double, int)> miasta = new() { ("Paryż", 2, 1000), ( "Londyn", 2.25, 1200 ), ( "Berlin", 1.25, 756),
                ( "Rzym", 1.75, 1232), ( "Sztokholm", 2.5, 1576 ),( "Barcelona", 3.15, 1400 ), ( "Madryd", 3.5, 1654 ),( "Warszawa", 0.5, 387) };

            Random rnd = new Random();
            int pom = rnd.Next(miasta.Count);

            string miasto = miasta[pom].Item1;
            double czasLotu = miasta[pom].Item2;
            double bazowaCena = miasta[pom].Item3;

            if (!liniaLotnicza.czyTaniaLinia)
            {
                bazowaCena *= 1.3;
            }

            //przechowujemy informację o poprzednim locie, po to aby nowy lot miał tę samą bramkę, co poprzedni (samolot cały czas stoi przy tej samej bramce)
            Lot? poprzedniLot = lot;

            // lot generowany jest dla samolotu podchodzącego do lądowania w Krakowie
            if (czyPrzylot)
            {
                lot = new Lot(miasto, czasLotu, bazowaCena, "Przylot");
            }
            // przypisanie nowego lotu samolotowi stojącemu na stanowisku postojowym po zakończonym locie do Krakowa
            else
            {
                lot = new Lot(miasto, czasLotu, bazowaCena, "Przygotowanie do lotu", poprzedniLot.bramka, poprzedniLot.opoznienie);
            }
        }
    }


    // duży samolot - kat. C (A<B<C<D)
    public class Airbus320 : Samolot
    {
        public Airbus320(double stanPaliwa, LiniaLotnicza liniaLotnicza, Lot lot)
            : base(stanPaliwa, liniaLotnicza, lot)
        {
            model = "A320";
            wagaPustegoSamolotu = 40150; // w kg
            maxLiczbaPasazerow = 180;
            maxPaliwo = 23800; // w kg
            maxLacznaWaga = 77000; // w kg
            spalaniePaliwa = 2500; // kg/h lotu
            kategoria = EnumKategoria.C;
        }

        public Airbus320(double stanPaliwa, LiniaLotnicza liniaLotnicza)
    : base(stanPaliwa, liniaLotnicza)
        {
            model = "A320";
            wagaPustegoSamolotu = 40150; // w kg
            maxLiczbaPasazerow = 180;
            maxPaliwo = 23800; // w kg
            maxLacznaWaga = 77000; // w kg
            spalaniePaliwa = 2500; // kg/h lotu
            kategoria = EnumKategoria.C;
        }
    }

    // duży samolot - kategoria C
    public class Boeing737 : Samolot
    {
        public Boeing737(double stanPaliwa, LiniaLotnicza liniaLotnicza, Lot lot)
            : base(stanPaliwa, liniaLotnicza, lot)
        {
            model = "B737";
            wagaPustegoSamolotu = 41140; // w kg 
            maxLiczbaPasazerow = 215;
            maxPaliwo = 20800; // w kg
            maxLacznaWaga = 78245; // w kg
            spalaniePaliwa = 2268; // kg / h lotu
            kategoria = EnumKategoria.C;
        }
        public Boeing737(double stanPaliwa, LiniaLotnicza liniaLotnicza)
            : base(stanPaliwa, liniaLotnicza)
        {
            model = "B737";
            wagaPustegoSamolotu = 41140; // w kg 
            maxLiczbaPasazerow = 215;
            maxPaliwo = 20800; // w kg
            maxLacznaWaga = 78245; // w kg
            spalaniePaliwa = 2268; // kg / h lotu
            kategoria = EnumKategoria.C;
        }
    }

    // mały samolot - kat. B
    public class Embrayer195 : Samolot
    {
        public Embrayer195(double stanPaliwa, LiniaLotnicza liniaLotnicza, Lot lot)
            : base(stanPaliwa, liniaLotnicza, lot)
        {
            model = "E195";
            wagaPustegoSamolotu = 21620; // w kg 
            maxLiczbaPasazerow = 112;
            maxPaliwo = 20800; // w kg
            maxLacznaWaga = 50790; // w kg
            spalaniePaliwa = 2041; // kg / h lotu
            kategoria = EnumKategoria.B;
        }
        public Embrayer195(double stanPaliwa, LiniaLotnicza liniaLotnicza)
            : base(stanPaliwa, liniaLotnicza)
        {
            model = "E195";
            wagaPustegoSamolotu = 21620; // w kg 
            maxLiczbaPasazerow = 112;
            maxPaliwo = 20800; // w kg
            maxLacznaWaga = 50790; // w kg
            spalaniePaliwa = 2041; // kg / h lotu
            kategoria = EnumKategoria.B;
        }
    }



    // bardzo duży samolot - kat. D
    public class Airbus380 : Samolot
    {
        public Airbus380(double stanPaliwa, LiniaLotnicza liniaLotnicza, Lot lot)
            : base(stanPaliwa, liniaLotnicza, lot)
        {
            model = "A380";
            wagaPustegoSamolotu = 276800; // w kg 
            maxLiczbaPasazerow = 509;
            maxPaliwo = 320000; // w kg
            maxLacznaWaga = 575000; // w kg
            spalaniePaliwa = 11500; // kg / h lotu
            kategoria = EnumKategoria.D;
        }

        public Airbus380(double stanPaliwa, LiniaLotnicza liniaLotnicza)
    : base(stanPaliwa, liniaLotnicza)
        {
            model = "A380";
            wagaPustegoSamolotu = 276800; // w kg 
            maxLiczbaPasazerow = 509;
            maxPaliwo = 320000; // w kg
            maxLacznaWaga = 575000; // w kg
            spalaniePaliwa = 11500; // kg / h lotu
            kategoria = EnumKategoria.D;
        }
    }


}
