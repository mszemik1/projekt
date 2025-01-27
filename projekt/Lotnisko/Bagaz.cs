using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotnisko
{
    internal class BlednaWagaBagazuException : Exception
    {
        public BlednaWagaBagazuException() : base() { }
        public BlednaWagaBagazuException(string message) : base(message) { }
    }
    public enum enumRodzajBagazu { podreczny, rejestrowany } // rodzaje bagaży


    public class Bagaz
    {
        double waga;
        public enumRodzajBagazu rodzaj;

        public Bagaz(double waga, enumRodzajBagazu rodzaj)
        {
            Waga = waga;
            this.rodzaj = rodzaj;
        }

        // sprawdzenie czy bagaż ma właściwą wagę
        public double Waga
        {
            get => waga;
            set
            {
                if (value < 0)
                {
                    throw new BlednaWagaBagazuException($"Waga bagażu musi być dodatnią liczbą.");
                }
                waga = value;
            }
        }

        public override string ToString() => $"Bagaż {rodzaj}, waga: {waga} kg";


    }
}
