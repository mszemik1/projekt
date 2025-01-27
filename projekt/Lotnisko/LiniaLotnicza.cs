using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotnisko
{
    public class LiniaLotnicza
    {
        public string nazwa;
        public double maxWagaBagazuPodrecznego;
        public double maxWagaBagazuRejestrowanego;
        public bool czyTaniaLinia;

        public LiniaLotnicza(string nazwa, double maxWagaBagazuPodrecznego, double maxWagaBagazuRejestrowanego, bool czyTaniaLinia)
        {
            this.nazwa = nazwa;
            this.maxWagaBagazuPodrecznego = maxWagaBagazuPodrecznego;
            this.maxWagaBagazuRejestrowanego = maxWagaBagazuRejestrowanego;
            this.czyTaniaLinia = czyTaniaLinia;
        }
    }
}
