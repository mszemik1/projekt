using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotnisko
{
    public enum EnumKlasa { ekonomiczna = 1, premium = 2, biznes = 3, pierwsza = 4 }
    public class Bilet : IComparable<Bilet>, IEquatable<Bilet>
    {
        public EnumKlasa klasa;
        double cena;
        Lot lot;

        public Bilet(EnumKlasa klasa, double cena, Lot lot)
        {
            this.klasa = klasa;
            this.cena = cena;
            this.lot = lot;
        }

        public override string ToString()
        {
            return $"Bilet, klasa {klasa}, cena biletu: {cena:c}";
        }

        public int CompareTo(Bilet? other)
        {
            if (other == null) return 1;

            // porównanie biletów wg klasy
            if (klasa != other.klasa)
            {
                return -1 * klasa.CompareTo(other.klasa); // chcemy, żeby wyższa klasa była najpierw na liście
            }

            // w obrębie jednej klasy, porównujemy bilety wg ceny
            return -1 * cena.CompareTo(other.cena);
        }

        public bool Equals(Bilet? other)
        {
            // implementacja interfejsu IEquatable - uznajemy, że bilety są te same, jeśli mają tę samą cenę i klasę
            return other.klasa == klasa && other.cena == cena;
        }
    }
}