using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace lotnisko
{
    class BlednaGodzinaException : Exception
    {
        public BlednaGodzinaException() : base() { }
        public BlednaGodzinaException(string message) : base(message) { }

    }
    public class Lot
    {
        public TimeSpan? godzinaWylotu;
        public TimeSpan? godzinaPrzylotu;
        public string miastoWylotu;
        public string miastoPrzylotu;
        public double czasLotu;
        public double bazowaCena;
        public Bramka? bramka;
        public string faza;
        public TimeSpan czasDoOdlotu;
        public int opoznienie;

        // <to powinno być w klasie lotnisko>
        private static readonly TimeSpan MinCzas = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan MaxCzas = TimeSpan.FromMinutes(14);

        public TimeSpan czasNaLotnisku = new TimeSpan(2, 0, 0);



        //<>


        // konstruktor parametryczny do generowania samolotu w fazie przylotu lub przygotowania do lotu
        public Lot(string miasto, double czasLotu, double bazowaCena, string faza, Bramka? bramka = null, int opoznienie = 0)
        {
            if (bramka is not null)
            {
                this.bramka = bramka;
            }

            this.opoznienie = opoznienie;

            if (faza == "Przylot")
            {
                godzinaPrzylotu = AktualnyCzas.aktualnyCzas + LosowanieCzasu.LosujCzasMinuty(MinCzas, MaxCzas);
                czasDoOdlotu = TimeSpan.FromMinutes(135);
                miastoWylotu = miasto;
                miastoPrzylotu = "Kraków";
                this.czasLotu = czasLotu;
                this.bazowaCena = bazowaCena;
            }
            // przygotowanie do lotu
            else
            {
                godzinaWylotu = AktualnyCzas.aktualnyCzas + TimeSpan.FromMinutes(75) + TimeSpan.FromMinutes((double)opoznienie);
                czasDoOdlotu = TimeSpan.FromMinutes(90) + TimeSpan.FromMinutes((double)opoznienie);
                miastoWylotu = "Kraków";
                miastoPrzylotu = miasto;
                this.czasLotu = czasLotu;
                this.bazowaCena = bazowaCena;
            }
            this.faza = faza;
        }

        public override string ToString()
        {
            if (faza == "Przylot" | faza == "Lot zakończony")
            {
                return $"Lot {miastoWylotu}-Kraków, przyleci o: {godzinaPrzylotu:hh\\:mm}, faza lotu: {faza}";
            }
            else if (faza == "Kołowanie do bramki")
            {
                if (bramka is null)
                {
                    return $"Lot {miastoWylotu}-Kraków, przyleciał o: {godzinaPrzylotu:hh\\:mm}, faza lotu: {faza}; oczekiwanie na wolną bramkę, opóźnienie: {opoznienie} min";
                }
                else return $"Lot {miastoWylotu}-Kraków, przyleciał o: {godzinaPrzylotu:hh\\:mm}, faza lotu: {faza} {bramka.kategoria}{bramka.numer}";

            }
            else if (faza == "Przygotowanie do lotu" | faza == "Boarding")
            {
                return $"Lot Kraków-{miastoPrzylotu}, godzina wylotu: {godzinaWylotu:hh\\:mm}, " +
                       $"czas do odlotu:{(czasDoOdlotu.Hours == 0 ? "" : $" {czasDoOdlotu.Hours}h")}" +
                       $"{(czasDoOdlotu.Minutes == 0 ? "" : $" {czasDoOdlotu.Minutes} min")}" +
                       $", faza lotu: {faza}, bramka: {bramka.kategoria}{bramka.numer}" +
                       $"{(opoznienie != 0 ? $", opóźnienie: {opoznienie} min" : "")}";
            }
            else if (faza == "Kołowanie na pas")
            {
                return $"Lot Kraków-{miastoPrzylotu}, godzina wylotu: {godzinaWylotu:hh\\:mm}, " +
                    $"faza lotu: {faza}{(opoznienie != 0 ? $" ,opóźnienie: {opoznienie} min" : "")}";
            }
            else 
            {
                return $"Lot Kraków-{miastoPrzylotu}, wyleciał o: {godzinaWylotu:hh\\:mm}" +
                    $"{(opoznienie != 0 ? $" ,opóźnienie: {opoznienie} min" : "")}";
            }


        }
    }
}
