/*  To Do
- Ilan:
    - Hoeveelheid Trips aanpassen waar nodig
    - Op veel plekken wordt rekening gehouden met de disposal site, dat hoeft dus ook niet meer
- Iza:
    - Gelijk de tijd en volumes uitrekenen.
*/


namespace Grote_Opdracht
{
    internal class BeginOplossing
    {
        private const int MAX_TRUCK_CAPACITY = 100000; // Maximum capacity in liters.
        private const int MAX_TRUCK_TIME_SECONDS = 720 * 60; // 720 minutes per day in seconds.
        private const int DISPOSAL_TIME_SECONDS = 30 * 60; // 30 minutes to dispose of waste.

        public static List<DoubleLinkedList> WillekeurigeBeginOplossing()
        {
            // Initialize 10 routes (2 trucks per day for 5 days).
            List<DoubleLinkedList> routes = Enumerable.Range(0, 15).Select(_ => new DoubleLinkedList()).ToList();

            Random random = new Random();

            // Group businesses by "Plaats" for better clustering.
            var groupedBusinesses = Program.bedrijven
                .GroupBy(b => b.Plaats)
                .ToDictionary(g => g.Key, g => new Queue<Bedrijf>(g.ToList()));

            // Shuffle the dictionary to ensure a semi-random solution
            groupedBusinesses.OrderBy(x => random.Next()).ToDictionary(g => g.Key, g => g.Value);

            // Keep track of remaining visits for each business.
            var remainingVisits = Program.bedrijven.ToDictionary(b => b, b => b.Frequentie);

            // Schedule businesses onto truck routes.
            foreach (var group in groupedBusinesses) {
                foreach (var bedrijf in group.Value) {
                    // Assign days based on frequency.
                    List<int> days = GetValidDaysForFrequency(bedrijf.Frequentie, random);

                    foreach (int day in days) {
                        int truckIndex = day * 3 + random.Next(0, 3); // Choose one of two trucks for the day.
                        TryAssign(truckIndex, bedrijf, 0);
                    }
                }
            }

            return routes;

            void TryAssign(int truckIndex, Bedrijf bedrijf, int tries) // could eventually return a bool to look if operation was succesful
            {
                if (AssignToRoute(routes[truckIndex], bedrijf, remainingVisits)) return;

                else if (tries == 2) return;
                else {
                    int differentDayTruck = (truckIndex / 3) + ((truckIndex + 1) % 3);
                    TryAssign(differentDayTruck, bedrijf, tries + 1);
                }
            }
        }

        private static bool AssignToRoute(DoubleLinkedList route, Bedrijf bedrijf, Dictionary<Bedrijf, int>? remainingVisits) // Iza, nieuwe berekeningen
        {
            double currentVolume = 0;
            double currentTime = 0;

            // Calculate current route metrics.
            Node? currentNode = route.head;
            while (currentNode != null) {
                if (currentNode.data == stortPlaats) currentVolume = 0;
                else currentVolume += currentNode.data.VolumePerContainer * currentNode.data.AantContainers;
                currentTime += Program.TijdTussenBedrijven(currentNode.previous?.data ?? stortPlaats, currentNode.data);
                currentNode = currentNode.next;
            }

            // Simulate adding this business to the route.
            double additionalVolume = bedrijf.VolumePerContainer * bedrijf.AantContainers;
            double additionalTime = Program.TijdTussenBedrijven(route.tail?.data ?? stortPlaats, bedrijf);

            // Simulate going to the stortplaats at the end of the day
            additionalTime += Program.TijdTussenBedrijven(bedrijf, stortPlaats) + DISPOSAL_TIME_SECONDS;

            if (currentVolume + additionalVolume <= MAX_TRUCK_CAPACITY &&
                currentTime + additionalTime <= MAX_TRUCK_TIME_SECONDS) {
                route.AddLast(new Node(bedrijf));

                // null if place we add is stortplaats
                if (remainingVisits != null) {
                    remainingVisits[bedrijf]--;
                    Program.NietBezochteBedrijven.Remove(bedrijf); // nodig?
                }
                return true;
            }

            return false;
        }

        private static List<int> GetValidDaysForFrequency(int frequency, Random random)
        {
            return frequency switch {
                1 => new List<int> { random.Next(0, 5) }, // Any one day.
                2 => random.Next(0, 2) == 0 ? new List<int> { 0, 3 } : new List<int> { 1, 4 }, // Mon-Thu or Tue-Fri.
                3 => new List<int> { 0, 2, 4 }, // Mon-Wed-Fri.
                4 => new List<int> { 0, 1, 2, 3 }.OrderBy(_ => random.Next()).Take(4).ToList(), // Any 4 days.
                _ => throw new ArgumentException($"Invalid frequency: {frequency}")
            };
        }
    }
}
