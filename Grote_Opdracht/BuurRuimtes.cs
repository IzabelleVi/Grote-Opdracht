/*  To Do
- Iza: 
    - Iteratief berekenen van Tijd
    - Iteratief berekenen van Volume

- Ilan:
    - Hoeveelheid Trips aanpassen waar nodig
- Goof:
    - Zorgen dat de functies ShiftAndereDag, voor de frequenties 1,2 en 4 een andere set aan dagen kiest, en pas als er een nieuwe plek is gevonden
      in de nieuwe set voor alle dagen, dan het bedrijf uit de vorige set (bijv voor freq 2, van maandag-donderdag naar dinsdag-vrijdag) uit alle dagen 
      verwijderen en in de nieuwe set toevoegen.
*/

namespace Grote_Opdracht
{
    internal class BuurRuimtes
    {
        static double incrementeel = 0;
        public static Random random = new Random();
        static List<DoubleLinkedList> GlobaleOphaalPatronen;
        
        public static List<DoubleLinkedList> ShiftAndereDag(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            incrementeel = 0;
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            int Dag1 = random.Next(0, huidigeOphaalpatronen.Count);
            DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[Dag1];
            int index;
            try
            {
                index = random.Next(1, ophaalPatroon.Count) - 1;
                if (index == 0 || index == ophaalPatroon.Count - 1)
                {
                    return huidigeOphaalpatronen;
                }
            }
            catch
            {
                return huidigeOphaalpatronen;
            }
            Node verplaatsbareNode = ophaalPatroon.Index(index);
            if (verplaatsbareNode == null) return huidigeOphaalpatronen;
            Bedrijf verplaatsbaarBedrijf = verplaatsbareNode.data;
            if (verplaatsbaarBedrijf.Plaats == "Stortplaats") return huidigeOphaalpatronen;

            int frequentie = verplaatsbaarBedrijf.Frequentie;
            if (frequentie == 3) return huidigeOphaalpatronen;
            List<int[]> sets = GetSetsForFrequentie(frequentie);

            foreach (var set in sets)
            {
                if (Array.Exists(set, element => element == Dag1))
                {
                    int nieuweDag1 = set[random.Next(0, set.Length)];
                    if (nieuweDag1 == Dag1) return huidigeOphaalpatronen; // Ensure we pick a different day within the set

                    DoubleLinkedList nieuweOphaalPatroon1 = huidigeOphaalpatronen[nieuweDag1];
                    int nieuwePlek1;
                    try
                    {
                        nieuwePlek1 = random.Next(1, nieuweOphaalPatroon1.Count) - 1;
                    }
                    catch
                    {
                        return huidigeOphaalpatronen;
                    }

                    if (frequentie == 4)
                    {
                        int nieuweDag2 = set[random.Next(0, set.Length)];
                        if (nieuweDag2 == Dag1 || nieuweDag2 == nieuweDag1) return huidigeOphaalpatronen; // Ensure we pick a different day within the set

                        DoubleLinkedList nieuweOphaalPatroon2 = huidigeOphaalpatronen[nieuweDag2];
                        int nieuwePlek2;
                        try
                        {
                            nieuwePlek2 = random.Next(1, nieuweOphaalPatroon2.Count) - 1;
                        }
                        catch
                        {
                            return huidigeOphaalpatronen;
                        }

                        int nieuweDag3 = set[random.Next(0, set.Length)];
                        if (nieuweDag3 == Dag1 || nieuweDag3 == nieuweDag1 || nieuweDag3 == nieuweDag2) return huidigeOphaalpatronen; // Ensure we pick a different day within the set

                        DoubleLinkedList nieuweOphaalPatroon3 = huidigeOphaalpatronen[nieuweDag3];
                        int nieuwePlek3;
                        try
                        {
                            nieuwePlek3 = random.Next(1, nieuweOphaalPatroon3.Count) - 1;
                        }
                        catch
                        {
                            return huidigeOphaalpatronen;
                        }

                        int nieuweDag4 = set[random.Next(0, set.Length)];
                        if (nieuweDag4 == Dag1 || nieuweDag4 == nieuweDag1 || nieuweDag4 == nieuweDag2 || nieuweDag4 == nieuweDag3) return huidigeOphaalpatronen; // Ensure we pick a different day within the set

                        DoubleLinkedList nieuweOphaalPatroon4 = huidigeOphaalpatronen[nieuweDag4];
                        int nieuwePlek4;
                        try
                        {
                            nieuwePlek4 = random.Next(1, nieuweOphaalPatroon4.Count) - 1;
                        }
                        catch
                        {
                            return huidigeOphaalpatronen;
                        }

                        if (insertChecker(nieuweOphaalPatroon1, verplaatsbaarBedrijf, nieuwePlek1) &&
                            insertChecker(nieuweOphaalPatroon2, verplaatsbaarBedrijf, nieuwePlek2) &&
                            insertChecker(nieuweOphaalPatroon3, verplaatsbaarBedrijf, nieuwePlek3) &&
                            insertChecker(nieuweOphaalPatroon4, verplaatsbaarBedrijf, nieuwePlek4))
                        {
                            BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                            BaseToevoegen(nieuweOphaalPatroon1, verplaatsbaarBedrijf, nieuwePlek1);
                            BaseToevoegen(nieuweOphaalPatroon2, verplaatsbaarBedrijf, nieuwePlek2);
                            BaseToevoegen(nieuweOphaalPatroon3, verplaatsbaarBedrijf, nieuwePlek3);
                            BaseToevoegen(nieuweOphaalPatroon4, verplaatsbaarBedrijf, nieuwePlek4);
                            Program.huidigeKost += incrementeel; // Iza
                            return huidigeOphaalpatronen;
                        }
                    }
                    else if (frequentie == 2)
                    {
                        int nieuweDag2 = set[random.Next(0, set.Length)];
                        if (nieuweDag2 == Dag1 || nieuweDag2 == nieuweDag1) return huidigeOphaalpatronen; // Ensure we pick a different day within the set

                        DoubleLinkedList nieuweOphaalPatroon2 = huidigeOphaalpatronen[nieuweDag2];
                        int nieuwePlek2;
                        try
                        {
                            nieuwePlek2 = random.Next(1, nieuweOphaalPatroon2.Count) - 1;
                        }
                        catch
                        {
                            return huidigeOphaalpatronen;
                        }

                        if (insertChecker(nieuweOphaalPatroon1, verplaatsbaarBedrijf, nieuwePlek1) &&
                            insertChecker(nieuweOphaalPatroon2, verplaatsbaarBedrijf, nieuwePlek2))
                        {
                            BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                            BaseToevoegen(nieuweOphaalPatroon1, verplaatsbaarBedrijf, nieuwePlek1);
                            BaseToevoegen(nieuweOphaalPatroon2, verplaatsbaarBedrijf, nieuwePlek2);
                            Program.huidigeKost += incrementeel; //Iza
                            return huidigeOphaalpatronen;
                        }
                    }
                    else if (frequentie == 1)
                    {
                        if (insertChecker(nieuweOphaalPatroon1, verplaatsbaarBedrijf, nieuwePlek1))
                        {
                            BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                            BaseToevoegen(nieuweOphaalPatroon1, verplaatsbaarBedrijf, nieuwePlek1);
                            Program.huidigeKost += incrementeel; // Iza
                            return huidigeOphaalpatronen;
                        }
                    }
                }
            }
            return huidigeOphaalpatronen;
        }

        private static List<int[]> GetSetsForFrequentie(int frequentie)
        {
            List<int[]> sets = new List<int[]>();
            switch (frequentie)
            {
                case 1:
                    sets.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
                    break;
                case 2:
                    sets.Add(new int[] { 0, 1, 2 });
                    sets.Add(new int[] { 9, 10, 11 });
                    sets.Add(new int[] { 3, 4, 5 });
                    sets.Add(new int[] { 12, 13, 14 });
                    break;
                case 3:
                    sets.Add(new int[] { 0, 1, 2 });
                    sets.Add(new int[] { 6, 7, 8 });
                    sets.Add(new int[] { 12, 13, 14 });
                    break;
                case 4:
                    sets.Add(new int[] { 0, 1, 2 });
                    sets.Add(new int[] { 3, 4, 5 });
                    sets.Add(new int[] { 6, 7, 8 });
                    sets.Add(new int[] { 9, 10, 11 });
                    sets.Add(new int[] { 3, 4, 5 });
                    sets.Add(new int[] { 6, 7, 8 });
                    sets.Add(new int[] { 9, 10, 11 });
                    sets.Add(new int[] { 12, 13, 14 });
                    break;
            }
            return sets;
        }

        public static List<DoubleLinkedList> ShiftAndereTruck(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            incrementeel = 0;

            int Dag1 = random.Next(0, huidigeOphaalpatronen.Count);
            DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[Dag1];
            int index;
            try
            {
                index = random.Next(1, ophaalPatroon.Count) - 1;
                if (index == 0 || index == ophaalPatroon.Count - 1) return huidigeOphaalpatronen;
            }
            catch
            {
                return huidigeOphaalpatronen;
            }
            Node verplaatsbareNode = ophaalPatroon.Index(index);
            Bedrijf verplaatsbaarBedrijf = verplaatsbareNode.data;
            if (verplaatsbaarBedrijf.Plaats == "Stortplaats") return huidigeOphaalpatronen;

            int frequentie = verplaatsbaarBedrijf.Frequentie;
            List<int[]> sets = GetSetsForFrequentie(frequentie);

            foreach (var set in sets)
            {
                if (Array.Exists(set, element => element == Dag1))
                {
                    int nieuweDag = set[random.Next(0, set.Length)];
                    if (nieuweDag == Dag1) return huidigeOphaalpatronen; // Ensure we pick a different day within the set

                    DoubleLinkedList nieuweOphaalPatroon = huidigeOphaalpatronen[nieuweDag];
                    int nieuwePlek;
                    try
                    {
                        nieuwePlek = random.Next(1, nieuweOphaalPatroon.Count) - 1;
                    }
                    catch
                    {
                        return huidigeOphaalpatronen;
                    }
                    if (insertChecker(nieuweOphaalPatroon, verplaatsbaarBedrijf, nieuwePlek))
                    {
                        BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                        BaseToevoegen(nieuweOphaalPatroon, verplaatsbaarBedrijf, nieuwePlek);
                        Program.huidigeKost += incrementeel; // Iza
                        return huidigeOphaalpatronen;
                    }
                }
            }
            return huidigeOphaalpatronen;
        }


        // Verschuift een bedrijf naar een andere plek binnen hetzelfde ophaalpatroon.
        public static List<DoubleLinkedList> ShiftZelfdeDag(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            incrementeel = 0;
            GlobaleOphaalPatronen = huidigeOphaalpatronen;

            int indexOphaalPatroon = random.Next(0, huidigeOphaalpatronen.Count);
            if (indexOphaalPatroon == 0 || indexOphaalPatroon == huidigeOphaalpatronen.Count - 1) return huidigeOphaalpatronen;
            DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[indexOphaalPatroon];
            int index;
            try
            {
                index = random.Next(1, ophaalPatroon.Count) - 1;
                if (index == 0 || index == ophaalPatroon.Count - 1) return huidigeOphaalpatronen;
            }
            catch
            {
                return huidigeOphaalpatronen;
            }
            Node verplaatsbareNode = ophaalPatroon.Index(index);
            Bedrijf verplaatsbaarBedrijf = verplaatsbareNode.data;
            if (verplaatsbaarBedrijf.Plaats == "Stortplaats") return huidigeOphaalpatronen;

            int nieuwePlek;
            try
            {
                nieuwePlek = random.Next(1, ophaalPatroon.Count) - 1;
            }
            catch
            {
                return huidigeOphaalpatronen;
            }
            if (insertChecker(ophaalPatroon, verplaatsbaarBedrijf, nieuwePlek))
            {
                BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                BaseToevoegen(ophaalPatroon, verplaatsbaarBedrijf, nieuwePlek);
                return huidigeOphaalpatronen;
            }

            return huidigeOphaalpatronen;
        }

        public static List<DoubleLinkedList> Add(List<DoubleLinkedList> ophaalpatronen)
        {
            if (ophaalpatronen == null || ophaalpatronen.Count == 0)
            {
                Console.WriteLine("Null or empty ophaalpatronen list.");
                return ophaalpatronen;
            }

            GlobaleOphaalPatronen = ophaalpatronen;
            if (Program.NietBezochteBedrijven == null || Program.NietBezochteBedrijven.Count == 0)
            {
                Console.WriteLine("No bedrijven to add.");
                return ophaalpatronen;
            }

            Bedrijf bedrijf = Program.NietBezochteBedrijven[random.Next(0, Program.NietBezochteBedrijven.Count)];
            incrementeel = -1000; // Iza

            bool added = false;
            switch (bedrijf.Frequentie)
            {
                case 1:
                    added = AddToRandomDays(ophaalpatronen, bedrijf);
                    break;
                case 2:
                    added = AddToSpecificDays(ophaalpatronen, bedrijf, new int[][] { new int[] { 0, 1, 2, 9, 10, 11}, new int[] { 3, 4, 5, 12, 13, 14 } });
                    break;
                case 3:
                    added = AddToSpecificDays(ophaalpatronen, bedrijf, new int[][] { new int[] { 0, 1, 2, 6, 7, 8, 12, 13, 14 } });
                    break;
                case 4:
                    added = AddToSpecificDays(ophaalpatronen, bedrijf, new int[][] { new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 } });
                    break;
            }

            if (added)
            {
                Program.NietBezochteBedrijven.Remove(bedrijf);
            }

            return ophaalpatronen;
            }
// Maandag: 012, Dinsdag 345, Woensdag 678, Donderdag 91011, Vrijdag 121314
            private static bool AddToRandomDays(List<DoubleLinkedList> ophaalpatronen, Bedrijf bedrijf) 
            {
                DoubleLinkedList rit = ophaalpatronen[random.Next(0, ophaalpatronen.Count)];
                int index = GetRandomIndex(rit);
                if (index == 0 || index == rit.Count - 1) return false;

                if (insertChecker(rit, bedrijf, index))
                {
                    BaseToevoegen(rit, bedrijf, index);
                    Program.huidigeKost += incrementeel; // Iza
                    return true;
                }
                return false;
            }

            private static bool AddToSpecificDays(List<DoubleLinkedList> ophaalpatronen, Bedrijf bedrijf, int[][] dayGroups)
            {
                foreach (var days in dayGroups)
                {
                    bool added = true;
                    foreach (int day in days)
                    {
                        DoubleLinkedList rit = ophaalpatronen[day];
                        int index = GetRandomIndex(rit);
                        if (index == 0 || index == rit.Count - 1 || index == -1 || !insertChecker(rit, bedrijf, index))
                        {
                            added = false;
                            break;
                        }
                    }

                    if (added)
                    {
                        foreach (int day in days)
                        {
                            DoubleLinkedList rit = ophaalpatronen[day];
                            int index = GetRandomIndex(rit);
                            BaseToevoegen(rit, bedrijf, index);
                             Program.huidigeKost += incrementeel; // Iza
                        }
                         return true;
                    }
                }
                
                return false;
            }


        private static int GetRandomIndex(DoubleLinkedList rit)
        {
            if (rit == null || rit.Count == 0) return -1;
            return random.Next(1, rit.Count) - 1;
        }

        public static List<DoubleLinkedList> Delete(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            // Kies een willekeurig ophaalpatroon
            DoubleLinkedList randomPatroon = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)];

            // Kies een willekeurige bedrijf in het ophaalpatroon
            int BedrijfIndex = random.Next(1, randomPatroon.Count) - 1;
            if (BedrijfIndex == 0 || BedrijfIndex == randomPatroon.Count - 1) return huidigeOphaalpatronen;

            // Haal het bedrijf en de bijbehorende node op
            Bedrijf verwijderBedrijf = randomPatroon.Index(BedrijfIndex).data;
            Node verwijderBedrijfNode = randomPatroon.Index(BedrijfIndex);
            if (verwijderBedrijf.Plaats == "Stortplaats") return huidigeOphaalpatronen;

            // Verwijder het bedrijf van alle routes gebasseerd op de frequenties
            int[][] dayGroups = null;
            switch (verwijderBedrijf.Frequentie)
            {
                case 1:
                    dayGroups = new int[][] { new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 } };
                    break;
                case 2:
                    dayGroups = new int[][] { new int[] { 0, 1, 2, 9, 10, 11}, new int[] { 3, 4, 5, 12, 13, 14 } };
                    break;
                case 3:
                    dayGroups = new int[][] { new int[] { 0, 1, 2, 6, 7, 8, 12, 13, 14 } };
                    break;
                case 4:
                    dayGroups = new int[][] { new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 } };
                    break;
            }

            // Verwijder het bedrijf van alle routes op de specifieke dagen
            foreach (var days in dayGroups)
            {
                foreach (int day in days)
                {
                    DoubleLinkedList patroon = huidigeOphaalpatronen[day];
                    Node mogelijkeVerwijderBedrijf = patroon.Find(verwijderBedrijf);
                    if (mogelijkeVerwijderBedrijf != null)
                    {
                        BaseVerwijderen(patroon, mogelijkeVerwijderBedrijf);
                        verwijderBedrijf.langsGeweest--;
                        // Update the cost by removing the travel time and adding the penalty
                        Program.huidigeKost += BerekenVerwijderKost(patroon, mogelijkeVerwijderBedrijf);
                        Program.huidigeKost += verwijderBedrijf.LedigingsDuurMinuten * verwijderBedrijf.AantContainers * 3 * 60; //penalty
                    }
                }
            }

            // Voeg het bedrijf terug toe aan de NietBezochteBedrijven lijst
            Program.NietBezochteBedrijven.Add(verwijderBedrijf);

            return huidigeOphaalpatronen;
        }

        private static double BerekenVerwijderKost(DoubleLinkedList patroon, Node verwijderBedrijfNode)
        {
            double verwijderKost = 0;

            // Bereken de kost van het verwijderen van het bedrijf
            if (verwijderBedrijfNode.previous != null && verwijderBedrijfNode.next != null)
            {
                Bedrijf prevBedrijf = verwijderBedrijfNode.previous.data;
                Bedrijf nextBedrijf = verwijderBedrijfNode.next.data;
                verwijderKost =  - BerekenReisTijd(prevBedrijf, verwijderBedrijfNode.data) - BerekenReisTijd(verwijderBedrijfNode.data, nextBedrijf) - (verwijderBedrijfNode.data.LedigingsDuurMinuten * 60);
            }

            return verwijderKost;
        }

        private static double BerekenReisTijd(Bedrijf bedrijf1, Bedrijf bedrijf2)
        {
            return Program.TijdTussenBedrijven(bedrijf1, bedrijf2);
        }

//MMMMMMMMMMMMMMKWEDSRFGHNMV<KJHYGTERFEDAFSGHJKVGCRFEWDQWSFZGRDXHCJVKGCXDZRFSEDWSEDFSZRGDXHFCJGVHCFGDFRSEDAWRFZSGTXDHCFJVGCHGTRFED
        public static bool insertChecker(DoubleLinkedList ophaalpatroon, Bedrijf bedrijf, int index) // Check of het bedrijf op de gegeven index in het ophaalpatroon past
        {
            try
            {
                Node bedrijfNode = new Node(bedrijf);
                ophaalpatroon.InsertAtIndex(bedrijfNode, index);
            }
            catch(Exception e)
            {
                Console.WriteLine($"log: {e}");
                return false;
            }

            // Controleer of het nieuwe patroon nog steeds mogelijk is
            bool ritMogelijk = CheckGrenzen(ophaalpatroon, index, bedrijf);

            if (!ritMogelijk)
            {
                ophaalpatroon.RemoveAtIndex(index);
                return false;
            }
            return true;
        }
        public static bool CheckGrenzen(DoubleLinkedList ophaalpatroon, int index, Bedrijf bedrijf)
        {
            if (bedrijf == null || ophaalpatroon == null || ophaalpatroon.Index(index) == null) // Ensure no null reference exceptions
            {
                Console.WriteLine("Null reference encountered in CheckGrenzen.");
                return false;
            }

            bool tijd = false;
            bool volume = false;

            Node nodeVorigeBedrijf = ophaalpatroon.Index(index - 1);
            if (nodeVorigeBedrijf == null || nodeVorigeBedrijf.data == null || nodeVorigeBedrijf.next == null || nodeVorigeBedrijf.next.data == null)
            {
                return false;
            }

            Bedrijf vorigeBedrijf = nodeVorigeBedrijf.data;
            //MMMMMMMMMMMMMMKWEDSRFGHNMV<KJHYGTERFEDAFSGHJKVGCRFEWDQWSFZGRDXHCJVKGCXDZRFSEDWSEDFSZRGDXHFCJGVHCFGDFRSEDAWRFZSGTXDHCFJVGCHGTRFED
            //Console.WriteLine($"Patroon nummer: {GlobaleOphaalPatronen.IndexOf(ophaalpatroon)}, Hoeveelheid rij tijden: {Program.Rijtijd.Count}");
            double huidigeRijtijd = Program.Rijtijd[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)];
        
            double tijdTussenVorigeEnVolgende = Program.TijdTussenBedrijven(vorigeBedrijf, nodeVorigeBedrijf.next.data);
            double tijdTussenVorigeEnNieuwe = Program.TijdTussenBedrijven(vorigeBedrijf, bedrijf);
            double tijdTussenNieuweEnVolgende = Program.TijdTussenBedrijven(bedrijf, nodeVorigeBedrijf.next.data);
            double ledigingsDuur = bedrijf.LedigingsDuurMinuten;

            double totaleTijd = huidigeRijtijd - tijdTussenVorigeEnVolgende + tijdTussenVorigeEnNieuwe + tijdTussenNieuweEnVolgende + ledigingsDuur;

            if (totaleTijd < 570 * 60)
                tijd = true;
            else if (570*60 < totaleTijd && totaleTijd < 630 * 60)
            {
                tijd = true;
                double penalty = (630 * 60 - totaleTijd) * 2; //Hier extra tijd *2 gedaan, kan nog nader worden bepaald
                incrementeel += penalty;
            }
                

            Program.BerekenHuidigeVolume(GlobaleOphaalPatronen);
            double volumeTotaal = (bedrijf.VolumePerContainer * bedrijf.AantContainers) + Program.Volumes[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)];
            if (volumeTotaal < 20000)
                volume = true;
            else if (volumeTotaal > 20000 && volumeTotaal < 21000)
            {
                volume = true;
                double penalty = (21000 - volumeTotaal) * 2; //Hier extra volume *2 gedaan, kan nog nader worden bepaald
                incrementeel += penalty;
            }

            double incrementeleKosten = 0; // Iza, zorgen dat dit allemaal werkt met de nieuwe kosten bijhouden
            if (ophaalpatroon.Index(index).previous != null || ophaalpatroon.Index(index).next != null)
            {
                incrementeleKosten += Program.AfstandenMatrix[ophaalpatroon.Index(index).previous.data.MatrixID, bedrijf.MatrixID]; 
                incrementeleKosten += Program.AfstandenMatrix[bedrijf.MatrixID, ophaalpatroon.Index(index).next.data.MatrixID];
                incrementeleKosten -= Program.AfstandenMatrix[ophaalpatroon.Index(index).previous.data.MatrixID, ophaalpatroon.Index(index).next.data.MatrixID];
            }
            else if (ophaalpatroon.Index(index).next != null)
            {
                incrementeleKosten += Program.AfstandenMatrix[BeginOplossing.stortPlaats.MatrixID, bedrijf.MatrixID];
                incrementeleKosten += Program.AfstandenMatrix[bedrijf.MatrixID, ophaalpatroon.Index(index).next.data.MatrixID];
                incrementeleKosten -= Program.AfstandenMatrix[BeginOplossing.stortPlaats.MatrixID, ophaalpatroon.Index(index).next.data.MatrixID];
            }
            incrementeel += incrementeleKosten;
            bool incrementeelCheck = CheckAccepteerOplossing();
            if (tijd && volume && incrementeelCheck) // Goof: Zorgen dat hier schendigen van tijd en volume wel worden toegelaten maar met een penalty
                return true;
            return false;
        }

        public static bool CheckAccepteerOplossing()
        {
            return Program.AccepteerOplossing(incrementeel);
        }

        static void BaseVerwijderen(DoubleLinkedList ophaalpatroon, Node nodeBedrijf)
        {
            if (ophaalpatroon == null || nodeBedrijf == null || nodeBedrijf.data == null)
            {
               Console.WriteLine("Null reference encountered in BaseVerwijderen.");
                return;
            }

            try
            {
               Program.NietBezochteBedrijven.Add(nodeBedrijf.data);

               double temp = 0; // Iza
               if (nodeBedrijf.previous != null && nodeBedrijf.next != null && nodeBedrijf.next.data != null)
               {
                  temp -= Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.data);
                  temp -= Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                 temp += Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.next.data);
                 temp -= nodeBedrijf.data.LedigingsDuurMinuten;
                }
                else if (nodeBedrijf.next != null && nodeBedrijf.next.data != null)
                {
                temp -= Program.TijdTussenBedrijven(BeginOplossing.stortPlaats, nodeBedrijf.data);
                temp -= Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                temp += Program.TijdTussenBedrijven(BeginOplossing.stortPlaats, nodeBedrijf.next.data);
                temp -= nodeBedrijf.data.LedigingsDuurMinuten;
                }   

                incrementeel += temp; // Iza
                Program.Rijtijd[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] += temp;

                Program.Volumes[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] -= nodeBedrijf.data.VolumePerContainer * nodeBedrijf.data.AantContainers;

                ophaalpatroon.Remove(nodeBedrijf);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception encountered in BaseVerwijderen: {e.Message}");
            }
        }

        static void BaseToevoegen(DoubleLinkedList ophaalpatroon, Bedrijf bedrijf, int index) //Iza
        {
            if (ophaalpatroon == null || bedrijf == null)
            {
                 Console.WriteLine("Null reference encountered in BaseToevoegen.");
                return;
            }

            Program.NietBezochteBedrijven.Remove(bedrijf);
            Node nodeBedrijf = new Node(bedrijf);
            ophaalpatroon.InsertAtIndex(nodeBedrijf, index);

            double temp = 0;
            if (nodeBedrijf.previous != null && nodeBedrijf.next != null && nodeBedrijf.next.data != null)
            {
                temp += Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.data);
                temp += Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                temp -= Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.next.data);
                temp += nodeBedrijf.data.LedigingsDuurMinuten;
        }
            else if (nodeBedrijf.next != null && nodeBedrijf.next.data != null)
            {
                temp += Program.TijdTussenBedrijven(BeginOplossing.stortPlaats, nodeBedrijf.data);
                temp += Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                temp -= Program.TijdTussenBedrijven(BeginOplossing.stortPlaats, nodeBedrijf.next.data);
                temp += nodeBedrijf.data.LedigingsDuurMinuten;
            }

            incrementeel += temp;
            Program.Rijtijd[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] += temp;
        }
    }
}
