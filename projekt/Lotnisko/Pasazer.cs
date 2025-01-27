using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotnisko
{
    public class ImionaNazwiska
    {
        public static List<(string, string)> ludzie;
        static ImionaNazwiska()
        {
            ludzie = new List<(string, string)> {
            ("Jan", "Kowalski"),
            ("Anna", "Nowak"),
            ("Piotr", "Wiśniewski"),
            ("Maria", "Wójcik"),
            ("Tomasz", "Kozłowski"),
            ("Katarzyna", "Kwiatkowska"),
            ("Michał", "Zieliński"),
            ("Agnieszka", "Szymańska"),
            ("Andrzej", "Woźniak"),
            ("Elżbieta", "Dąbrowska"),
            ("Krzysztof", "Król"),
            ("Joanna", "Majewska"),
            ("Mateusz", "Olszewski"),
            ("Magdalena", "Zając"),
            ("Grzegorz", "Jaworski"),
            ("Ewa", "Pawlak"),
            ("Paweł", "Kaczmarek"),
            ("Dorota", "Sikorska"),
            ("Robert", "Nowicki"),
            ("Monika", "Adamska"),
            ("Łukasz", "Czarnecki"),
            ("Beata", "Malinowska"),
            ("Marcin", "Górski"),
            ("Iwona", "Sadowska"),
            ("Rafał", "Borkowski"),
            ("Sylwia", "Mazur"),
            ("Damian", "Sawicki"),
            ("Patrycja", "Chmielewska"),
            ("Sebastian", "Michalski"),
            ("Karolina", "Kubiak"),
            ("Wojciech", "Piotrowski"),
            ("Justyna", "Nowicka"),
            ("Adam", "Kamiński"),
            ("Izabela", "Lis"),
            ("Dariusz", "Wesołowski"),
            ("Zofia", "Makowska"),
            ("Bartłomiej", "Przybylski"),
            ("Jolanta", "Walczak")
            };
        }



    }
    public class Pasazer : IComparable<Pasazer>
    {
        string? imie;
        string? nazwisko;
        DateTime dataUrodzenia;
        public Bilet bilet;
        public List<Bagaz> bagaze; // lista bagaży danego pasażera - podręcznych i rejestrowanych ( można mieć > 1 każdego z nich)

        public Pasazer(string imie, string nazwisko, Bilet bilet, string data)
        {
            if (!DateTime.TryParseExact(data, new string[] {"dd-MM-yyyy", "yyyy-MM-dd",
                "dd-MMM-yyyy"}, null, DateTimeStyles.None, out DateTime d))
            {
                throw new FormatException("Błędny format daty urodzenia!");
            }

            bagaze = new List<Bagaz>();
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.bilet = bilet;
            dataUrodzenia = d;
        }
        public Pasazer(string imie, string nazwisko, Bilet bilet, DateTime d, List<Bagaz> bagaze)
        {
            this.bagaze = bagaze;
            this.imie = imie;
            this.nazwisko = nazwisko;
            this.bilet = bilet;
            dataUrodzenia = d;
        }
        public Pasazer(Bilet bilet, List<Bagaz> bagaze)
        {
            this.bilet = bilet;
            this.bagaze = bagaze;

            // losowe generowanie daty urodzenia
            Random rand = new Random();
            DateTime start = new DateTime(1940, 1, 1);
            int range = (DateTime.Today - start).Days;
            dataUrodzenia = start.AddDays(rand.Next(range));

            // losowe generowanie imienia i nazwiska
            int pom = rand.Next(ImionaNazwiska.ludzie.Count);
            this.imie = ImionaNazwiska.ludzie[pom].Item1;
            this.nazwisko = ImionaNazwiska.ludzie[pom].Item2;
        }

        public void WezBagaz(Bagaz bagaz) => bagaze.Add(bagaz);

        public int IleSztukBagazu(enumRodzajBagazu rodzaj) => bagaze.Where(b => b.rodzaj == rodzaj).Count();
        public override string ToString()
        {
            return $"Pasażer {imie} {nazwisko},\n" +
                $"liczba sztuk bagażu podręcznego: {IleSztukBagazu(enumRodzajBagazu.podreczny)}, \n" +
                $"liczba sztuk bagażu rejestrowanego: {IleSztukBagazu(enumRodzajBagazu.rejestrowany)}\n" +
                $"{bilet.ToString()}";
        }

        public int CompareTo(Pasazer? other)
        {
            if (other == null) return 1;

            // porównanie pasażerów wg biletów
            if (!bilet.Equals(other.bilet)) return bilet.CompareTo(other.bilet);

            // porównanie po dacie urodzenia
            return dataUrodzenia.CompareTo(other.dataUrodzenia);
        }
    }

}
