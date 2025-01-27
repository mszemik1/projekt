using lotnisko;

internal class Program
{
    static void Main(string[] args)
    {
        // zakładamy że nasze lotnisko jest w Krakowie 

        //// tworzymy linie lotnicze
        //linialotnicza wizzair = new("wizz air", 10, 20, true);
        //linialotnicza ryanair = new("ryanair", 11, 22, true);
        //linialotnicza lot = new("lot", 8, 26, false);

        //// tworzymy loty - przylatujące - trzeba podać miasto z którego wyleciał, czas lotu i cenę
        //lot lot1 = new("warszawa", 0.5, 357);
        //lot lot2 = new("londyn", 2.25, 1200);
        //lot lot3 = new("paryż", 2.15, 1325);

        //// tworzymy samoloty
        //airbus320 airbus1 = new(4000, wizzair, lot1);
        //boeing737 boeing1 = new(3000, ryanair, lot2);
        //boeing737 boeing2 = new(2000, lot, lot3);

        //console.writeline(boeing2.tostring());

        //// tworzymy bilety
        //bilet bilet1 = new(enumklasa.ekonomiczna, 400, lot1);
        //bilet bilet2 = new(enumklasa.ekonomiczna, 843.32, lot2);


        //// tworzymy pasażerów
        //pasazer pasazer1 = new("adam", "kowalski", bilet1, "2002-12-02");
        //pasazer pasazer2 = new("joanna", "nowak", bilet2, "1998-04-04");

        //// tworzymy bagaże i dodajemy do pasażerów
        //bagaz b1 = new(6.5, enumrodzajbagazu.podreczny);
        //bagaz b2 = new(15.67, enumrodzajbagazu.rejestrowany);
        //bagaz b3 = new(12, enumrodzajbagazu.podreczny);
        //bagaz b4 = new(17, enumrodzajbagazu.rejestrowany);
        //pasazer1.wezbagaz(b1);
        //pasazer1.wezbagaz(b2);
        //pasazer1.wezbagaz(b3);

        //pasazer2.wezbagaz(b1);
        //pasazer2.wezbagaz(b2);
        //pasazer2.wezbagaz(b4);

        //console.writeline(pasazer1.tostring());
        //console.writeline(pasazer2.tostring());

        //// lista pasażerów, którzy wejdą na pokład
        //list<pasazer> pasazerowie = new() { pasazer1, pasazer2 };


        //// lista pasażerów - wykorzystanie funkcji losującej
        //list<pasazer> losowanipasazerowie = boeing2.losujpasazerow();
        //list<pasazer> losowanipasazerowie2 = airbus1.losujpasazerow();

        //// boarding
        //boeing2.boarding(losowanipasazerowie);
        //boeing2.tankowanie();

        //console.writeline(boeing2.tostring());

        //// deboarding

        //boeing2.deboarding();
        //console.writeline(boeing2.tostring());

        //console.writeline(airbus1.tostring());
        //airbus1.boarding(losowanipasazerowie2);
        //airbus1.tankowanie();
        //console.writeline(airbus1.tostring());
        // dodanie lotniska



        // dodanie kolejnych lotów przylatujących i wypisanie listy
        //Lot lot4 = new("Amsterdam", 1.5, 1287);
        //Lot lot5 = new("Barcelona", 2.25, 1657);


        // konsekwencja tego co w klasie lotnisko: musimy dodawać Samolot z lotem


        //krakowLotnisko.DodajLot(lot1);
        //krakowLotnisko.DodajLot(lot2);
        //krakowLotnisko.DodajLot(lot3);
        //krakowLotnisko.DodajLot(lot4);
        //krakowLotnisko.DodajLot(lot5);

        //Lotnisko krakowLotnisko = new Lotnisko("Kraków");
        //krakowLotnisko.DodajSamolot(airbus1);
        //krakowLotnisko.DodajSamolot(boeing1);

        // losowanie samolotów, można sobie wylosować i przyloty i wyloty, ale w praktyce tylko przyloty 
        //krakowLotnisko.LosujSamoloty(20);

        // jeśli czyPrzyloty= false to losujemy wyloty
        //krakowLotnisko.LosujSamoloty(20, false);

        Lotnisko krakowLotnisko = new Lotnisko("Kraków", 20);
        for (int i = 0; i < 10; i++)
        {
            krakowLotnisko.RuchCzasu();
            krakowLotnisko.WyswietlLoty();
            
            Console.WriteLine("");
            Console.WriteLine("########################");
        }


        krakowLotnisko.SortujLotyPoGodzinieWylotu();


        //krakowLotnisko.RuchCzasu();
        //krakowLotnisko.RuchCzasu();
        //krakowLotnisko.RuchCzasu();
        //krakowLotnisko.RuchCzasu();
        //krakowLotnisko.RuchCzasu();
    }
}
