using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grote_Opdracht
{
    internal class BuurRuimtes
    {
        static double incrementeel = 0;
        public static Random random = new Random();
        static List<DoubleLinkedList> GlobaleOphaalPatronen;
        
        public static List<DoubleLinkedList> ShiftAndereDag(List<DoubleLinkedList> huidigeOphaalpatronen) //Om een bedrijf van de ene dag naar de andere te verplaatsen
        {
            incrementeel = 0;
            GlobaleOphaalPatronen = huidigeOphaalpatronen; //clonen, uiteindelijk iets anders voor bedenken

            int tries = 0;
            while (tries < 10) // 10 keer random bedrijf pakken en dan honderd keer kijken of die past
            {
                int Dag1 = random.Next(0, huidigeOphaalpatronen.Count); // kies een random rit
                DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)]; // kies een random ophaalpatroon
                int index;
                try
                {
                    index = random.Next(1, ophaalPatroon.Count) - 1; // kies een random bedrijf
                    if (index == 0 || index == ophaalPatroon.Count - 1) 
                    {
                        tries++;
                        continue;
                    }
                }
                catch
                {
                    continue; // voor als er geen bedrijven zijn
                }
                Node verplaatsbareNode = ophaalPatroon.Index(index); // de node die we gaan verplaatsen
                if (verplaatsbareNode == null) continue; // Check of de node wel iets is
                Bedrijf verplaatsbaarBedrijf = verplaatsbareNode.data;
                if (verplaatsbaarBedrijf.Plaats == "Stortplaats") continue; // overslaan als het de stortplaats is

                int tries2 = 0;
                while (tries2 < 100) // honderd keer proberen te verplaatsen
                {
                    DoubleLinkedList nieuweOphaalPatroon = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)]; // Kies het nieuwe ophaalpatroon waar je hem naar verplaatst
                    int nieuwePlek;
                    try
                    {
                        nieuwePlek = random.Next(1, nieuweOphaalPatroon.Count) - 1;  // Kies de plek waar je hem neerzet
                    }
                    catch
                    {
                        break; // voor als er geen bedrijven zijn
                    }
                    if (insertChecker(nieuweOphaalPatroon, verplaatsbaarBedrijf, nieuwePlek)) // Check of het mogelijk is
                    {
                        BaseVerwijderen(ophaalPatroon, verplaatsbareNode); // Verwijder het bedrijf uit het oude ophaalpatroon
                        BaseToevoegen(nieuweOphaalPatroon, verplaatsbaarBedrijf, nieuwePlek); // Voeg het bedrijf toe aan het nieuwe ophaalpatroon
                        Program.huidigeKost += incrementeel;
                        return huidigeOphaalpatronen;
                    }
                    tries2++;
                }
                tries++;
            }

            return huidigeOphaalpatronen;
        }

        public static List<DoubleLinkedList> ShiftAndereTruck(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            incrementeel = 0;
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            int tries = 0;
            while (tries < 11) // 10 keer random bedrijf pakken en dan honderd keer kijken of die past
            {
                tries++;
                int Dag1 = random.Next(0, huidigeOphaalpatronen.Count);
                int dag2;
                if (Dag1 == 14)
                    dag2 = Dag1 - 1;
                else if (Dag1 % 2 == 0)
                    dag2 = Dag1 + 1;
                else
                    dag2 = Dag1 - 1;
                DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[Dag1];
                int index;
                try
                {
                    index = random.Next(1, ophaalPatroon.Count) - 1;
                    if (index == 0 || index == ophaalPatroon.Count - 1) continue;
                }
                catch
                {
                    continue;
                }
                Node verplaatsbareNode = ophaalPatroon.Index(index);
                Bedrijf verplaatsbaarBedrijf = verplaatsbareNode.data;
                if (verplaatsbaarBedrijf.Plaats == "Stortplaats") continue;
                int tries2 = 0;
                while (tries2 < 100)
                {
                    DoubleLinkedList nieuweOphaalPatroon;
                    if (random.Next(0,2) == 1)
                        nieuweOphaalPatroon = huidigeOphaalpatronen[Dag1];
                    else
                        nieuweOphaalPatroon = huidigeOphaalpatronen[dag2];
                    int nieuwePlek;
                    try
                    {
                        nieuwePlek = random.Next(1, nieuweOphaalPatroon.Count) - 1;
                    }
                    catch
                    {
                        continue;
                    }
                    if (insertChecker(nieuweOphaalPatroon, verplaatsbaarBedrijf, nieuwePlek))
                    {
                        BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                        BaseToevoegen(nieuweOphaalPatroon, verplaatsbaarBedrijf, nieuwePlek);
                        Program.huidigeKost += incrementeel;
                        return huidigeOphaalpatronen;
                    }
                    tries2++;
                }

                
            }
            Console.WriteLine("Shift andere truck is niet gelukt");
            return huidigeOphaalpatronen;
        }

        /// Verschuift een bedrijf naar een andere plek binnen hetzelfde ophaalpatroon.
        public static List<DoubleLinkedList> ShiftZelfdeDag(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            incrementeel = 0;
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            int tries = 0;
            while (tries < 10) // 10 keer random bedrijf pakken en dan honderd keer kijken of die past
            {
                int indexOphaalPatroon = random.Next(0, huidigeOphaalpatronen.Count);
                if (indexOphaalPatroon == 0 || indexOphaalPatroon == huidigeOphaalpatronen.Count - 1) continue;
                DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[indexOphaalPatroon];
                int index;
                try
                {
                    index = random.Next(1, ophaalPatroon.Count) - 1;
                    if (index == 0 || index == ophaalPatroon.Count - 1) continue;
                }
                catch
                {
                    continue;
                }
                Node verplaatsbareNode = ophaalPatroon.Index(index);
                Bedrijf verplaatsbaarBedrijf = verplaatsbareNode.data;
                if (verplaatsbaarBedrijf.Plaats == "Stortplaats") continue;
                int tries2 = 0;
                while (tries2 < 100)
                {
                    int nieuwePlek;
                    try
                    {
                        nieuwePlek = random.Next(1, ophaalPatroon.Count) - 1;
                    }
                    catch
                    {
                        break;
                    }
                    if (insertChecker(ophaalPatroon, verplaatsbaarBedrijf, nieuwePlek))
                    {
                        BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                        BaseToevoegen(ophaalPatroon, verplaatsbaarBedrijf, nieuwePlek);
                        return huidigeOphaalpatronen;
                    }
                    tries2++;
                }

                tries++;
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
            incrementeel = -1000;

            switch (bedrijf.Frequentie)
            {
                case 1:
                    AddToRandomDays(ophaalpatronen, bedrijf, 1, 100);
                    break;
                case 2:
                    AddToSpecificDays(ophaalpatronen, bedrijf, new int[][] { new int[] { 0, 1, 6, 7 }, new int[] { 2, 3, 8, 9 } }, 10);
                    break;
                case 3:
                    AddToSpecificDays(ophaalpatronen, bedrijf, new int[][] { new int[] { 0, 1, 4, 5, 8, 9 } }, 300);
                    break;
                case 4:
                    AddToSpecificDays(ophaalpatronen, bedrijf, new int[][] { new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new int[] { 2, 3, 4, 5, 6, 7, 8, 9 } }, 10);
                    break;
            }

            return ophaalpatronen;
        }

        private static void AddToRandomDays(List<DoubleLinkedList> ophaalpatronen, Bedrijf bedrijf, int frequency, int maxTries)
        {
            for (int i = 0; i < maxTries; i++)
            {
                DoubleLinkedList rit = ophaalpatronen[random.Next(0, ophaalpatronen.Count)];
                int index = GetRandomIndex(rit);
                if (index == 0 || index == rit.Count - 1) continue;

                if (insertChecker(rit, bedrijf, index))
                {
                    BaseToevoegen(rit, bedrijf, index);
                    Program.huidigeKost += incrementeel;
                    return;
                }
            }
        }

        private static void AddToSpecificDays(List<DoubleLinkedList> ophaalpatronen, Bedrijf bedrijf, int[][] dayGroups, int maxTries)
        {
            for (int tries = 0; tries < maxTries; tries++)
            {
                foreach (var days in dayGroups)
                {
                    DoubleLinkedList rit = ophaalpatronen[random.Next(days[0], days[1] + 1)];
                    int index = GetRandomIndex(rit);
                    if (index == 0 || index == rit.Count - 1 || index == -1) continue;
                    if (insertChecker(rit, bedrijf, index))
                    {
                        BaseToevoegen(rit, bedrijf, index);
                        Program.huidigeKost += incrementeel;
                        return;
                    }
                }
            }
        }

        private static int GetRandomIndex(DoubleLinkedList rit)
        {
            if (rit == null || rit.Count == 0) return -1;
            return random.Next(1, rit.Count) - 1;
        }

        public static List<DoubleLinkedList> Delete(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            incrementeel = 1000;
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            // Kies een willekeurig ophaalpatroon
            DoubleLinkedList randomPatroon = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)];

            // Kies een willekeurige bedrijf in het ophaalpatroon
            int BedrijfIndex = random.Next(1, randomPatroon.Count) - 1;
            if (BedrijfIndex == 0 || BedrijfIndex == randomPatroon.Count - 1) return huidigeOphaalpatronen;

            // Haal het bedrijf en de bijbehorende node op
            Bedrijf verwijderBedrijf = randomPatroon.Index(BedrijfIndex).data;
            Node verwijderBedrijfNode = randomPatroon.Index(BedrijfIndex);
            //Console.WriteLine(randomPatroon.Count + " <- hoeveelheid bedrijven Delete");
            if (verwijderBedrijf.Plaats == "Stortplaats") return huidigeOphaalpatronen;
            // Gebaseerd op de frequentie van het bedrijf, bepaal welke andere dagen moeten worden verwijderd
            if (verwijderBedrijf.Frequentie == 1)
            {
                    // Een keer per week
                BaseVerwijderen(randomPatroon, verwijderBedrijfNode);
                verwijderBedrijf.langsGeweest--;
            } 
            else
            {
                // Twee keer per week
                foreach (var patroon in huidigeOphaalpatronen)
                {
                    Node mogelijkeVerwijderBedrijf = patroon.Find(verwijderBedrijf);
                    if (mogelijkeVerwijderBedrijf!= null)
                    {
                        BaseVerwijderen(patroon, mogelijkeVerwijderBedrijf);
                        
                    }
                }

            }
            Program.huidigeKost += incrementeel;
            return huidigeOphaalpatronen;

        }

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
            if (bedrijf == null || ophaalpatroon == null || ophaalpatroon.Index(index) == null) // Zorg ervoor dat er geen null reference exceptions zijn
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
            double totaleTijd = Program.Rijtijd[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] - Program.TijdTussenBedrijven(vorigeBedrijf, nodeVorigeBedrijf.next.data) + Program.TijdTussenBedrijven(vorigeBedrijf, bedrijf) + Program.TijdTussenBedrijven(bedrijf, nodeVorigeBedrijf.next.data) + bedrijf.LedigingsDuurMinuten;

            if (totaleTijd < 570 * 60)
                tijd = true;

            Program.BerekenHuidigeVolume(GlobaleOphaalPatronen);
            if ((bedrijf.VolumePerContainer * bedrijf.AantContainers) + Program.Volumes[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] < 20000)
                volume = true;
            

            double incrementeleKosten = 0;
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
            if (tijd && volume && incrementeelCheck)
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

               double temp = 0;
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

                incrementeel += temp;
                Program.Rijtijd[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] += temp;

                Program.Volumes[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] -= nodeBedrijf.data.VolumePerContainer * nodeBedrijf.data.AantContainers;

                ophaalpatroon.Remove(nodeBedrijf);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception encountered in BaseVerwijderen: {e.Message}");
            }
        }

        static void BaseToevoegen(DoubleLinkedList ophaalpatroon, Bedrijf bedrijf, int index)
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

        public static List<DoubleLinkedList> Toevoegen(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            Bedrijf bedrijf1 = Program.NietBezochteBedrijven[random.Next(0, Program.NietBezochteBedrijven.Count)];


            int x = 0;
            while (x < 1000)
            {
                Bedrijf bedrijf = Program.bedrijven[random.Next(0, Program.bedrijven.Count)];
                if (bedrijf.langsGeweest < bedrijf.Frequentie)
                {
                    Node nieuwBedrijfsNode = new Node(bedrijf);
                    Bedrijf nieuwBedrijf = nieuwBedrijfsNode.data;
                    DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)];
                    int index = random.Next(0, ophaalPatroon.Count);
                    if (index == 0 || index == ophaalPatroon.Count - 1) continue;
                    if (insertChecker(ophaalPatroon, nieuwBedrijf, index))
                    {
                        ophaalPatroon.InsertAtIndex(nieuwBedrijfsNode, index);
                        bedrijf.langsGeweest++;
                        return huidigeOphaalpatronen;
                    }
                }
                x++;
            }
            return huidigeOphaalpatronen;
        }

        public static List<DoubleLinkedList> Remove(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            int x = 0;
            while (x < 1000)
            {
                Bedrijf bedrijf = Program.bedrijven[random.Next(0, Program.bedrijven.Count)];
                if (bedrijf.langsGeweest > bedrijf.Frequentie)
                {
                    foreach (DoubleLinkedList ophaalpatroon in huidigeOphaalpatronen)
                    {
                        Node node = ophaalpatroon.Find(bedrijf);
                        if (node != null)
                        {
                            ophaalpatroon.Remove(node);
                            bedrijf.langsGeweest--;
                            x++;
                        }
                    }
                }
                x++;
            }
            return huidigeOphaalpatronen;
        }
    }
}

