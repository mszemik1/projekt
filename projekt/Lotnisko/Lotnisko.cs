using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace lotnisko
{
    // kategoria bramki - D to największa bramka, A- najmniejsza
    public enum EnumKategoria { A, B, C, D }
    public class Bramka
    {
        public int numer;
        public bool czyZajety;
        public EnumKategoria kategoria;

        public Bramka(int numer, EnumKategoria kategoria)
        {
            this.numer = numer;
            czyZajety = false;
            this.kategoria = kategoria;
        }
    }
    public class AktualnyCzas
    {
        public static TimeSpan aktualnyCzas = new TimeSpan(6, 0, 0);
    }



    public class Lotnisko
    {
        string nazwa;
        List<Bramka> bramki;
        List<Samolot> odloty;
        List<Samolot> przyloty;
        List<Samolot> historia;

        public List<Samolot> Przyloty
        {
            get { return przyloty; }
            set { przyloty = value; }
        }
        public List<Samolot> Odloty
        {
            get { return odloty; }
            set { odloty = value; }
        }



        public Lotnisko(string nazwa, int ileBramek)
        {
            this.nazwa = nazwa;
            bramki = new List<Bramka>();
            odloty = new List<Samolot>();
            przyloty = new List<Samolot>();
            historia = new List<Samolot>();



            for (int i = 0; i < ileBramek; i++)
            {
                if (i % 2 == 0) bramki.Add(new Bramka(i + 1, EnumKategoria.D));
                else if (i % 3 == 0) bramki.Add(new Bramka(i + 1, EnumKategoria.C));
                else if (i % 5 == 0) bramki.Add(new Bramka(i + 1, EnumKategoria.B));
                else bramki.Add(new Bramka(i + 1, EnumKategoria.A));
            }
        }

        public void DodajSamolot(Samolot s)
        {
            przyloty.Add(s);
        }

        

        public void SortujLotyPoGodzinieWylotu()
        {
            odloty = odloty.OrderBy(s => s.lot.godzinaWylotu).ToList();
            przyloty = przyloty.OrderBy(s => s.lot.godzinaPrzylotu).ToList();
        }


        public void WyswietlLoty()
        {
            Console.WriteLine($"Arrivals / Przyloty {nazwa}:\n");
            foreach (var s in przyloty)
            {
                Console.WriteLine(s.lot);
            }
            Console.WriteLine($"Departures/ Odloty {nazwa}:\n");
            foreach (var s in odloty)
            {
                Console.WriteLine(s.lot);
                //Console.WriteLine(s);
            }
            Console.WriteLine($"Odleciały z {nazwa}:\n");
            foreach (var s in historia)
            {
                Console.WriteLine(s.lot);
            }
        }

        // funkcja losująca samoloty przylatujące
        public List<Samolot> LosujSamoloty(int liczbaSamolotow, bool czyPrzyloty = true)
        {
            // nie powinienem tworzyć obiektów - linii- tutaj, ale koncepcyjnie nie wiem jak inaczej
            LiniaLotnicza wizzair = new("Wizz Air", 10, 20, true);
            LiniaLotnicza ryanair = new("Ryanair", 11, 22, true);
            LiniaLotnicza LOT = new("LOT", 8, 26, false);
            LiniaLotnicza Lufthansa = new("Lufthansa", 9, 23, false);

            List<LiniaLotnicza> linie = new() { wizzair, ryanair, LOT, Lufthansa };

            for (int i = 0; i < liczbaSamolotow; i++)
            {
                Random rnd = new Random();
                int pom2 = rnd.Next(21); // losowanie modelu samolotu

                LiniaLotnicza linia = linie[rnd.Next(linie.Count)]; // losowanie linii lotniczej


                double stanPaliwa = rnd.NextDouble() + rnd.Next(1000) + 1000;
                if (pom2 < 6)
                {
                    Airbus320 samolot = new(stanPaliwa, linia);
                    samolot.LosujLot(czyPrzyloty);
                    samolot.Boarding(samolot.LosujPasazerow(true));
                    samolot.ZaladunekBagazy();
                    przyloty.Add(samolot);
                }
                else if (pom2 < 12)
                {
                    Boeing737 samolot = new Boeing737(stanPaliwa, linia);
                    samolot.LosujLot(czyPrzyloty);
                    samolot.Boarding(samolot.LosujPasazerow(true));
                    samolot.ZaladunekBagazy();
                    przyloty.Add(samolot);
                }
                else if (pom2 <18)
                {
                    Embrayer195 samolot = new Embrayer195(stanPaliwa, linia);
                    samolot.LosujLot(czyPrzyloty);
                    samolot.Boarding(samolot.LosujPasazerow(true));
                    samolot.ZaladunekBagazy();
                    przyloty.Add(samolot);
                }
                else
                {
                    Airbus380 samolot = new Airbus380(stanPaliwa, linia);
                    samolot.LosujLot(czyPrzyloty);
                    samolot.Boarding(samolot.LosujPasazerow());
                    samolot.ZaladunekBagazy();
                    przyloty.Add(samolot);
                }
                // zakładam że to co losuję to są samoloty przylatujące
            }
            return przyloty;


        }

        public void RuchCzasu()
        {
            AktualnyCzas.aktualnyCzas += new TimeSpan(0, 15, 0);
            TimeSpan aktualnyCzas = AktualnyCzas.aktualnyCzas;
            Console.WriteLine($"Aktualny czas: {aktualnyCzas:hh\\:mm}");
            LosujSamoloty(new Random().Next(4) + 1, true);

            List<Samolot> doPrzeniesieniaDoOdlotow = new List<Samolot>();
            List<Samolot> doUsunieciaZOdlotow = new List<Samolot>();

            foreach (Samolot s in przyloty)
            {

                TimeSpan czasDoOdlotu = s.lot.czasDoOdlotu;

                if (czasDoOdlotu > TimeSpan.FromMinutes(120))
                {
                    s.lot.faza = "Przylot";
                }
                else if (czasDoOdlotu <= TimeSpan.FromMinutes(120) && czasDoOdlotu > TimeSpan.FromMinutes(105))
                {
                    s.lot.faza = "Kołowanie do bramki";

                    // szukamy pierwszej wolnej bramki tak dużej, żeby "zmieścił się" samolot
                    Bramka? wolnaBramka = bramki.FirstOrDefault(b => !b.czyZajety & (int)b.kategoria >= (int)s.kategoria);

                    // jeśli wszystkie bramki są zajęte
                    if (wolnaBramka is null)
                    {
                        int opoznienie = (aktualnyCzas - s.lot.godzinaPrzylotu.Value).Minutes;
                        TimeSpan czasOpoznienia = new TimeSpan(0, opoznienie, 0);
                        s.lot.opoznienie += opoznienie;
                        s.lot.czasDoOdlotu += czasOpoznienia;
                        s.lot.godzinaPrzylotu += czasOpoznienia;
                    }

                    // znaleziono wolną bramkę
                    else
                    {
                        s.lot.bramka = wolnaBramka;

                        // przypisujemy bramkę do lotu - lot ją zajmuje
                        wolnaBramka.czyZajety = true;
                    }

                }
                else if (czasDoOdlotu <= TimeSpan.FromMinutes(105) && czasDoOdlotu > TimeSpan.FromMinutes(90))
                {
                    s.lot.faza = "Lot zakończony";

                    s.Deboarding();
                    s.RozladunekBagazy();
                    doPrzeniesieniaDoOdlotow.Add(s); // Dodaj do listy tymczasowej
                }
                s.lot.czasDoOdlotu -= TimeSpan.FromMinutes(15);
            }

            foreach (Samolot s in doPrzeniesieniaDoOdlotow)
            {
                przyloty.Remove(s);
                odloty.Add(s);
            }


            foreach (Samolot s in odloty)
            {
                TimeSpan czasDoOdlotu = s.lot.czasDoOdlotu;
                if (czasDoOdlotu <= TimeSpan.FromMinutes(90) && czasDoOdlotu > TimeSpan.FromMinutes(75))
                {
                    s.lot.faza = "Przygotowanie do lotu";
                    s.LosujLot(false);
                }

                else if (czasDoOdlotu <= TimeSpan.FromMinutes(75) && czasDoOdlotu > TimeSpan.FromMinutes(30))
                {
                    s.lot.faza = "Boarding";
                    // jeśli nie ma pasażerów na pokładzie, to losujemy ich, pasażerowie wchodzą na pokład, ładujemy bagaże i tankujemy samolot
                    if (s.LiczbaPasazerow() == 0)
                    {
                        s.Boarding(s.LosujPasazerow(false));
                        s.ZaladunekBagazy();
                        s.Tankowanie();
                    }
                }
                else if (czasDoOdlotu <= TimeSpan.FromMinutes(30) && czasDoOdlotu > TimeSpan.FromMinutes(15))
                {
                    s.lot.faza = "Kołowanie na pas";

                    // zwalniamy bramkę, którą wcześniej zajmował samolot
                    s.lot.bramka.czyZajety = false;


                    s.lot.bramka = null;

                }
                else if (czasDoOdlotu <= TimeSpan.FromMinutes(15) && czasDoOdlotu >= TimeSpan.FromMinutes(0))
                {
                    s.lot.faza = "Odleciał";
                }
                else
                {
                    czasDoOdlotu -= new TimeSpan(0, 15, 0);
                    doUsunieciaZOdlotow.Add(s);
                }
                s.lot.czasDoOdlotu -= TimeSpan.FromMinutes(15);

            }


            foreach (Samolot s in doUsunieciaZOdlotow)
            {
                odloty.Remove(s);
                historia.Add(s);
            }
            SortujLotyPoGodzinieWylotu();
        }

    }
}
