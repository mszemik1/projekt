using lotnisko;
using System.Collections.Generic;

namespace LotniskoTest
{
    [TestClass]
    public class SamolotTest
    {
        private LiniaLotnicza liniaLotnicza;
        private Lot lot;
        private List<Pasazer> pasazerowie;
        private Samolot samolot;

        [TestInitialize]
        public void Setup()
        {
            liniaLotnicza = new LiniaLotnicza("TestLine",  20, 30, true);
            lot = new Lot("Pary¿", 2, 1000, "Przylot");
            samolot = new Airbus320(10000, liniaLotnicza, lot); 
            pasazerowie = new List<Pasazer>(); 
        }

        [TestMethod]
        public void LiczbaPasazerow()
        {
        
            pasazerowie.Add(new Pasazer(new Bilet(EnumKlasa.ekonomiczna, 500, lot), new List<Bagaz>()));
            pasazerowie.Add(new Pasazer(new Bilet(EnumKlasa.biznes, 1500, lot), new List<Bagaz>()));

            samolot.Boarding(pasazerowie);

            int liczbaPasazerow = samolot.LiczbaPasazerow();
            Assert.AreEqual(2, liczbaPasazerow); 
        }

        [TestMethod]
        public void Boarding()
        {
            pasazerowie.Add(new Pasazer(new Bilet(EnumKlasa.ekonomiczna, 500, lot), new List<Bagaz>()));
            samolot.Boarding(pasazerowie);

            Assert.AreEqual(1, samolot.LiczbaPasazerow()); 
        }

        [TestMethod]
        public void ZaladunekBagazy()
        {
            pasazerowie.Add(new Pasazer(new Bilet(EnumKlasa.ekonomiczna, 500, lot),
                new List<Bagaz> { new Bagaz(15, enumRodzajBagazu.rejestrowany) }));

            samolot.Boarding(pasazerowie);
            samolot.ZaladunekBagazy();

            Assert.AreEqual(1, samolot.LiczbaBagazy(enumRodzajBagazu.rejestrowany)); 
        }

        [TestMethod]
        public void Deboarding()
        {
            pasazerowie.Add(new Pasazer(new Bilet(EnumKlasa.ekonomiczna, 500, lot), new List<Bagaz>()));
            samolot.Boarding(pasazerowie);

            samolot.Deboarding();

            Assert.AreEqual(0, samolot.LiczbaPasazerow()); 
        }

        [TestMethod]
        public void LosujPasazerow()
        {
            var losowiPasazerowie = samolot.LosujPasazerow();
            Assert.IsTrue(losowiPasazerowie.Count > 0); 
        }

        [TestMethod]
        public void LosujLot()
        {
            samolot.LosujLot();
            Assert.IsNotNull(samolot.lot); 
        }
    }
};