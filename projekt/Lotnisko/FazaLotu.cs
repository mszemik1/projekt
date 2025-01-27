using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lotnisko
{
    internal class FazaLotu
    {
        private List<Lot> loty; // Lista lotów
        private Lotnisko lotnisko; // Referencja do lotniska
        private readonly TimeSpan fazaRuchu = new TimeSpan(0, 15, 0); // Przesunięcie czasu o 15 minut
    }

}
public static class LosowanieCzasu
{
    private static readonly Random Random = new Random();

    public static TimeSpan LosujCzasMinuty(TimeSpan min, TimeSpan max)
    {
        if (min > max)
            throw new ArgumentException("Minimalny czas nie może być większy od maksymalnego.");

        // Oblicz liczbę minut w przedziale
        int minMinuty = (int)min.TotalMinutes;
        int maxMinuty = (int)max.TotalMinutes;

        // Losuj liczbę minut
        int losoweMinuty = Random.Next(minMinuty, maxMinuty + 1);

        // Przekonwertuj liczbę minut na TimeSpan
        return TimeSpan.FromMinutes(losoweMinuty);
    }
}
