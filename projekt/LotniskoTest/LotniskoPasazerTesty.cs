using lotnisko;

namespace LotniskoTest
{
    [TestClass]
    public class LotniskoTest
    {
        [TestMethod]
        public void TworzenieLotniska()
        {
            Lotnisko lotnisko = new("lotnisko1", 1);
            Assert.IsNotNull(lotnisko);
        }
        [TestMethod]
        public void DodawanieSamolotu()
        {
            Lotnisko lotniska = new("321", 1);
            LiniaLotnicza L1 = new("Lufthansa", 40, 20, false);
            Boeing737 s1 = new(1000, L1);
            lotniska.DodajSamolot(s1);
            Assert.IsTrue(lotniska.Przyloty.Contains(s1));
        }
        [TestMethod]
        public void SortujLotyPoGodzinie()
        {
            Lotnisko lotnisko = new("321", 1);
            Samolot samolot1 = new Boeing737(1000, new LiniaLotnicza("Lufthansa", 40, 20, false))
            {
                lot = new Lot("Berlin", 2.5, 200, "Planowany")
            };

            Samolot samolot2 = new Boeing737(1200, new LiniaLotnicza("Ryanair", 30, 15, true))
            {
                lot = new Lot("Warszawa", 1.5, 150, "Planowany")
            };

            Samolot samolot3 = new Boeing737(1500, new LiniaLotnicza("Air France", 50, 25, true))
            {
                lot = new Lot("Paryż", 3, 300, "Przylot")
            };
            lotnisko.Odloty.Add(samolot1);
            lotnisko.Odloty.Add(samolot2);
            lotnisko.Odloty.Add(samolot3);

            lotnisko.Przyloty.Add(samolot3);
            lotnisko.Przyloty.Add(samolot1);
            lotnisko.Przyloty.Add(samolot2);


            lotnisko.SortujLotyPoGodzinieWylotu();


            var odloty = lotnisko.Odloty.Select(s => s.lot.godzinaWylotu).ToList();
            Assert.IsTrue(odloty.SequenceEqual(odloty.OrderBy(x => x)), "Odloty nie zostały poprawnie posortowane po godzinach wylotu.");

            var przyloty = lotnisko.Przyloty.Select(s => s.lot.godzinaPrzylotu).ToList();
            Assert.IsTrue(przyloty.SequenceEqual(przyloty.OrderBy(x => x)), "Przyloty nie zostały poprawnie posortowane po godzinach przylotu.");
        }
        [TestMethod]
        public void LosowanieSamolotow()
        {
            Lotnisko lotnisko = new("KRK", 3);
            int liczbaSamolotow = 5;
            bool czyPrzyloty = true;

            var samoloty = lotnisko.LosujSamoloty(liczbaSamolotow, czyPrzyloty);

            Assert.AreEqual(liczbaSamolotow, samoloty.Count, "Nie wygenerowano poprawnej liczby samolotów.");

            foreach (var samolot in samoloty)
            {
                Assert.IsTrue(
                    samolot is Airbus320 || samolot is Boeing737 || samolot is Embrayer195 || samolot is Airbus380,
                    $"Samolot typu {samolot.GetType().Name} nie jest poprawnym typem."
                );

                Assert.AreEqual(czyPrzyloty, samolot.lot.faza == "Przylot",
                    "Faza lotu nie jest zgodna z oczekiwanym typem (Przylot/Odlot).");
            }
        }
    }

    
    [TestClass]
    public class PasazerTest
    {
        [TestMethod]
        public void WezBagaz()
        {
            Lot lot = new Lot("Berlin", 2.5, 500.00, "Przylot");
            Bilet bilet = new Bilet(EnumKlasa.premium, 500.00, lot);
            Pasazer pasazer = new Pasazer("Jan", "Kowalski", bilet, "01-01-1980");
            Bagaz bagaz = new Bagaz(10, enumRodzajBagazu.podreczny);

            pasazer.WezBagaz(bagaz);

            Assert.AreEqual(1, pasazer.IleSztukBagazu(enumRodzajBagazu.podreczny), "Pasażer powinien mieć 1 bagaż podręczny.");
        }
        [TestMethod]
        public void IleSztukBagazu()
        {
            Lot lot = new Lot("Berlin", 2.5, 500.00, "Przylot");
            Bilet bilet = new Bilet(EnumKlasa.premium, 500.00, lot);
            Pasazer pasazer = new Pasazer("Anna", "Nowak", bilet, "15-05-1990");
            Bagaz bagaz1 = new Bagaz(5, enumRodzajBagazu.podreczny);
            Bagaz bagaz2 = new Bagaz(20, enumRodzajBagazu.rejestrowany);
            pasazer.WezBagaz(bagaz1);
            pasazer.WezBagaz(bagaz2);

            Assert.AreEqual(1, pasazer.IleSztukBagazu(enumRodzajBagazu.podreczny), "Pasażer powinien mieć 1 bagaż podręczny.");
            Assert.AreEqual(1, pasazer.IleSztukBagazu(enumRodzajBagazu.rejestrowany), "Pasażer powinien mieć 1 bagaż rejestrowany.");
        }
        [TestMethod]
        public void CompareTo()
        {
            Lot lot1 = new Lot("Nowy Jork", 10.0, 1000.00, "Lot przygotowania");
            Lot lot2 = new Lot("Rzym", 2.5, 500.00, "Przylot");

            Bilet biletVIP = new Bilet(EnumKlasa.pierwsza, 1000.00, lot1);
            Bilet biletEconomy = new Bilet(EnumKlasa.ekonomiczna, 300.00, lot2);

            Pasazer pasazer1 = new Pasazer("Jan", "Kowalski", biletVIP, "01-01-1980");
            Pasazer pasazer2 = new Pasazer("Anna", "Nowak", biletEconomy, "15-05-1990");
            Pasazer pasazer3 = new Pasazer("Marek", "Lewandowski", biletVIP, "10-10-1975");

            Assert.IsTrue(pasazer1.CompareTo(pasazer2) < 0, "Pasażer z biletem VIP powinien mieć wyższy priorytet niż pasażer z biletem Economy.");
            Assert.IsTrue(pasazer2.CompareTo(pasazer1) > 0, "Pasażer z biletem Economy powinien mieć niższy priorytet niż pasażer z biletem VIP.");

            Assert.IsTrue(pasazer1.CompareTo(pasazer3) > 0, "Starszy pasażer (Marek) z biletem VIP powinien mieć wyższy priorytet.");
            Assert.IsTrue(pasazer3.CompareTo(pasazer1) < 0, "Młodszy pasażer (Jan) z biletem VIP powinien mieć niższy priorytet.");
        }
    }
}
