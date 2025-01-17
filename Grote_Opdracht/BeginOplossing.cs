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

        public static Bedrijf stortPlaats = new Bedrijf {
            Order = 0,
            Plaats = "Stortplaats",
            LedigingsDuurMinuten = DISPOSAL_TIME_SECONDS,
            MatrixID = 287
        };

        public static List<DoubleLinkedList> WillekeurigeBeginOplossing()
        {
            // Initialize 10 routes (2 trucks per day for 5 days).
            List<DoubleLinkedList> routes = Enumerable.Range(0, 10).Select(_ => new DoubleLinkedList()).ToList(); // Ilan; dit moet aangepast worden voor trips

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

                    foreach (int day in days) { // Ilan; dit moet aangepast worden voor trips
                        int truckIndex = day * 2 + random.Next(0, 2); // Choose one of two trucks for the day.
                        TryAssign(truckIndex, bedrijf, false);
                    }
                }
            }

            // Ensure all routes start and end at the disposal site.
            foreach (DoubleLinkedList route in routes) {
                AddDisposalSiteToRoute(route); // Ilan; disposal maakt niet meer uit
            }

            return routes;

            void TryAssign(int truckIndex, Bedrijf bedrijf, bool retry) // could eventually return a bool to look if operation was succesful
            {
                if (AssignToRoute(routes[truckIndex], bedrijf, remainingVisits)) return;

                if (AssignToRoute(routes[truckIndex], stortPlaats, null)) {
                    AssignToRoute(routes[truckIndex], bedrijf, remainingVisits); }// Ilan: Disposal wordt niet meer gebruikt
                else if (retry) return;
                else {
                    int differentDayTruck; // Get the difference in number of the other truck, 1 or -1
                    if (truckIndex % 2 == 0) differentDayTruck = 1;
                    else differentDayTruck = -1;
                    TryAssign(truckIndex + differentDayTruck, bedrijf, true);
                }
                // Iza, tijd en capaciteit berekenen
            }
        }

        private static bool AssignToRoute(DoubleLinkedList route, Bedrijf bedrijf, Dictionary<Bedrijf, int>? remainingVisits) // Ilan, wederom de trips & disposal die niet meer hoeft
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

        private static void AddDisposalSiteToRoute(DoubleLinkedList route) // Ilan, mag weg
        {
            if (route.head == null) {
                route.AddFirst(new Node(stortPlaats));
                route.AddLast(new Node(stortPlaats));
            } else {
                route.AddFirst(new Node(stortPlaats));
                route.AddLast(new Node(stortPlaats));
            }
        }

        private static List<int> GetValidDaysForFrequency(int frequency, Random random) // Ilan, trips
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
