using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotnisko
{
    public class ZlaKategoriaStanowiskaException : Exception
    {
        public ZlaKategoriaStanowiskaException() : base() { }
        public ZlaKategoriaStanowiskaException(string message) : base(message) { }
    }
    public class PasStartowy
    {
        string kierunek;
        int dlugosc;
        bool czyZajety;

        public PasStartowy(string kierunek, int dlugosc)
        {
            this.kierunek = kierunek;
            this.dlugosc = dlugosc;
            this.czyZajety = false;
        }
    }

}
