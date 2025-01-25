/*  To Do
- Ilan:
    - Hoeveelheid Trips aanpassen waar nodig
- Goof:
    - Zorgen dat de functies ShiftAndereDag, voor de frequenties 1,2 en 4 een andere set aan dagen kiest, en pas als er een nieuwe plek is gevonden
      in de nieuwe set voor alle dagen, dan het bedrijf uit de vorige set (bijv voor freq 2, van maandag-donderdag naar dinsdag-vrijdag) uit alle dagen 
      verwijderen en in de nieuwe set toevoegen.
*/

using System.Formats.Asn1;
using System.Runtime.CompilerServices;

namespace Grote_Opdracht
{
    internal class BuurRuimtes
    {
        static double incrementeel = 0;
        public static Random random = new Random();
        static List<DoubleLinkedList> GlobaleOphaalPatronen;
        

        private static List<int[]> GetSetsForFrequentie(int frequentie)
        {
            List<int[]> sets = new List<int[]>();
            switch (frequentie)
            {
                case 1:
                    sets.Add(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }); // Every day
                    break;
                case 2:
                    sets.Add(new int[] { 0, 1, 2 }); //maandag
                    sets.Add(new int[] { 3, 4, 5 }); //dinsdag
                    sets.Add(new int[] { 9, 10, 11 }); //donderdag
                    sets.Add(new int[] { 12, 13, 14 }); //vrijdag
                    break;
                case 3:
                    sets.Add(new int[] { 0, 1, 2 }); // maandag
                    sets.Add(new int[] { 6, 7, 8 }); // woensdag
                    sets.Add(new int[] { 12, 13, 14 }); // vrijdag
                    break;
                case 4:
                    sets.Add(new int[] { 0, 1, 2 }); // maandag
                    sets.Add(new int[] { 3, 4, 5 }); // dinsdag
                    sets.Add(new int[] { 6, 7, 8 }); // woensdag
                    sets.Add(new int[] { 9, 10, 11 }); // donderdag
                    sets.Add(new int[] { 12, 13, 14 }); // vrijdag
                    break;
            }
            return sets;
        }

        public static List<DoubleLinkedList> ShiftAndereDag(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            int Dag1 = random.Next(0, huidigeOphaalpatronen.Count); // index oude patroon
            DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[Dag1]; // oude patroon
            int index;
            index = random.Next(1, ophaalPatroon.Count) - 1; // index bedrijf in oude patroon
            if (index == 0 || index == ophaalPatroon.Count - 1) // Kan weg als stortplaats niet in de lijst staat.
                {
                    return huidigeOphaalpatronen;
                }

            Node verplaatsbareNode = ophaalPatroon.Index(index);
            if (verplaatsbareNode == null) return huidigeOphaalpatronen;
            Bedrijf verplaatsbaarBedrijf = verplaatsbareNode.data;

            int frequentie = verplaatsbaarBedrijf.Frequentie;
            if (frequentie == 3) return huidigeOphaalpatronen;
            List<int[]> sets = GetSetsForFrequentie(frequentie);

            if (frequentie == 1)
            {
                int[] setje = sets[random.Next(0, sets.Count)]; // set van dagen
                int nieuweDag1 = setje[random.Next(0, setje.Length)]; // index nieuw ophaalpatroon
                if (nieuweDag1 == Dag1) return huidigeOphaalpatronen; // Ensure we pick a different day within the set
                DoubleLinkedList nieuweOphaalPatroon1 = huidigeOphaalpatronen[nieuweDag1]; // nieuw ophaalpatroon
                int nieuwePlek1;
                nieuwePlek1 = random.Next(1, nieuweOphaalPatroon1.Count) - 1;
                BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                BaseToevoegen(nieuweOphaalPatroon1, verplaatsbaarBedrijf, nieuwePlek1);

                return huidigeOphaalpatronen;
            }
            else if (frequentie == 4)
            {
                List<bool> aanwezigheid = new List<bool>();
                for (int i = 0; i < 15; i++)
                {
                aanwezigheid.Add(false);
                }
                foreach (int[] set in sets)
                {
                    foreach(int dag in set)
                    {
                     if (huidigeOphaalpatronen[dag].Contains(verplaatsbaarBedrijf) == true) aanwezigheid[dag] = true;
                    }
                }
                if (aanwezigheid[0] == true || aanwezigheid[1] == true || aanwezigheid[2] == true) //patroon is maandag-donderdag
                {
                    foreach (int[] set in sets) //verwijderd alle oude aanwezigheden van het bedrijf
                    {
                        foreach(int dag in set)
                        {
                        if (aanwezigheid[dag]) 
                        {
                            BaseVerwijderen(GlobaleOphaalPatronen[dag], verplaatsbareNode);
                        }
                        }
                    }
                    int nieuweRit1 = sets[1][random.Next(0, sets[1].Length)]; // nieuwe rit op dinsdag
                    int nieuweRit2 = sets[2][random.Next(0, sets[2].Length)]; // nieuwe rit op woensdag
                    int nieuweRit3 = sets[3][random.Next(0, sets[3].Length)]; // nieuwe rit op donderdag
                    int nieuweRit4 = sets[4][random.Next(0, sets[4].Length)]; // nieuwe rit op vrijdag

                    int nieuwePlek1 = random.Next(0, huidigeOphaalpatronen[nieuweRit1].Count); // nieuwe plek op maandag
                    int nieuwePlek2 = random.Next(0, huidigeOphaalpatronen[nieuweRit2].Count); // nieuwe plek op dinsdag
                    int nieuwePlek3 = random.Next(0, huidigeOphaalpatronen[nieuweRit3].Count); // nieuwe plek op woensdag
                    int nieuwePlek4 = random.Next(0, huidigeOphaalpatronen[nieuweRit4].Count); // nieuwe plek op donderdag

                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit1], verplaatsbaarBedrijf, nieuwePlek1);
                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit2], verplaatsbaarBedrijf, nieuwePlek2);
                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit3], verplaatsbaarBedrijf, nieuwePlek3);
                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit4], verplaatsbaarBedrijf, nieuwePlek4);

                    return GlobaleOphaalPatronen;
                }
                else if (aanwezigheid[12] == true || aanwezigheid[13] == true || aanwezigheid[14] == true) //patroon is dinsdag-vrijdag)
                                {
                    foreach (int[] set in sets) //verwijderd alle oude aanwezigheden van het bedrijf
                    {
                        foreach(int dag in set)
                        {
                        if (aanwezigheid[dag]) 
                        {
                            BaseVerwijderen(GlobaleOphaalPatronen[dag], verplaatsbareNode);
                        }
                        }
                    }
                    int nieuweRit1 = sets[0][random.Next(0, sets[0].Length)]; // nieuwe rit op maandag
                    int nieuweRit2 = sets[1][random.Next(0, sets[1].Length)]; // nieuwe rit op dinsdag
                    int nieuweRit3 = sets[2][random.Next(0, sets[2].Length)]; // nieuwe rit op woensdag
                    int nieuweRit4 = sets[3][random.Next(0, sets[3].Length)]; // nieuwe rit op donderdag

                    int nieuwePlek1 = random.Next(1, huidigeOphaalpatronen[nieuweRit1].Count) - 1; // nieuwe plek op dinsdag
                    int nieuwePlek2 = random.Next(1, huidigeOphaalpatronen[nieuweRit2].Count) - 1; // nieuwe plek op woensdag
                    int nieuwePlek3 = random.Next(1, huidigeOphaalpatronen[nieuweRit3].Count) - 1; // nieuwe plek op donderdag
                    int nieuwePlek4 = random.Next(1, huidigeOphaalpatronen[nieuweRit4].Count) - 1; // nieuwe plek op vrijdag

                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit1], verplaatsbaarBedrijf, nieuwePlek1);
                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit2], verplaatsbaarBedrijf, nieuwePlek2);
                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit3], verplaatsbaarBedrijf, nieuwePlek3);
                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit4], verplaatsbaarBedrijf, nieuwePlek4);
                    
                    return GlobaleOphaalPatronen;
                }
            
            }
            else if (frequentie == 2)
            {
                List<bool> aanwezigheid = new List<bool>();
                for (int i = 0; i < 15; i++)
                {
                aanwezigheid.Add(false);
                }
                foreach (int[] set in sets)
                {
                    foreach(int dag in set)
                    {
                     if (huidigeOphaalpatronen[dag].Contains(verplaatsbaarBedrijf) == true) aanwezigheid[dag] = true;
                    }
                }
                if (aanwezigheid[0] == true || aanwezigheid[1] == true || aanwezigheid[2] == true) //patroon is maandag & donderdag
                {
                    foreach (int[] set in sets) //verwijderd alle oude aanwezigheden van het bedrijf
                    {
                        foreach(int dag in set)
                        {
                        if (aanwezigheid[dag]) 
                        {
                            BaseVerwijderen(GlobaleOphaalPatronen[dag], verplaatsbareNode);
                        }
                        }
                    }
                    int nieuweRit1 = sets[1][random.Next(0, sets[1].Length)]; // nieuwe rit op dinsdag
                    int nieuweRit2 = sets[3][random.Next(0, sets[3].Length)]; // nieuwe rit op vrijdag

                    int nieuwePlek1 = random.Next(1, huidigeOphaalpatronen[nieuweRit1].Count) - 1; // nieuwe plek op dinsdag
                    int nieuwePlek2 = random.Next(1, huidigeOphaalpatronen[nieuweRit2].Count) - 1; // nieuwe plek op vrijdag

                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit1], verplaatsbaarBedrijf, nieuwePlek1);
                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit2], verplaatsbaarBedrijf, nieuwePlek2);

                    return GlobaleOphaalPatronen;
                }
                else if (aanwezigheid[3] == true || aanwezigheid[4] == true || aanwezigheid[5] == true) //patroon is dinsdag & vrijdag)
                                {
                    foreach (int[] set in sets) //verwijderd alle oude aanwezigheden van het bedrijf
                    {
                        foreach(int dag in set)
                        {
                        if (aanwezigheid[dag]) 
                        {
                            BaseVerwijderen(GlobaleOphaalPatronen[dag], verplaatsbareNode);
                        }
                        }
                    }
                    int nieuweRit1 = sets[0][random.Next(0, sets[0].Length)]; // nieuwe rit op maandag
                    int nieuweRit2 = sets[2][random.Next(0, sets[2].Length)]; // nieuwe rit op donderdag

                    int nieuwePlek1 = random.Next(1, huidigeOphaalpatronen[nieuweRit1].Count) - 1; // nieuwe plek op maandag
                    int nieuwePlek2 = random.Next(1, huidigeOphaalpatronen[nieuweRit2].Count) - 1; // nieuwe plek op donderdag

                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit1], verplaatsbaarBedrijf, nieuwePlek1);
                    BaseToevoegen(GlobaleOphaalPatronen[nieuweRit2], verplaatsbaarBedrijf, nieuwePlek2);
                    
                    return GlobaleOphaalPatronen;
                }
            }
            return huidigeOphaalpatronen;
        }

        public static List<DoubleLinkedList> ShiftZelfdeDag(List<DoubleLinkedList> huidigeOphaalpatronen) // Ilan, aanpassen voor wat de nieuwe trucks zijn?
        {
            GlobaleOphaalPatronen = huidigeOphaalpatronen;

            int Dag1 = random.Next(0, huidigeOphaalpatronen.Count); // index oude patroon
            DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[Dag1]; // oude patroon
            int index;
            index = random.Next(1, ophaalPatroon.Count) - 1; // index bedrijf in oude patroon

            Node verplaatsbareNode = ophaalPatroon.Index(index);
            Bedrijf verplaatsbaarBedrijf = verplaatsbareNode.data;

            int frequentie = verplaatsbaarBedrijf.Frequentie;
            List<int[]> sets = GetSetsForFrequentie(frequentie);

            foreach (var set in sets)
            {
                if (Array.Exists(set, element => element == Dag1))
                {
                    int nieuweDag = set[random.Next(0, set.Length)];
                    if (nieuweDag == Dag1) return huidigeOphaalpatronen; // Andere dag pakken

                    DoubleLinkedList nieuweOphaalPatroon = GlobaleOphaalPatronen[nieuweDag];
                    int nieuwePlek = random.Next(1, nieuweOphaalPatroon.Count) - 1; // nieuwe plek op de zelfde dag, andere trip
                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                    BaseToevoegen(nieuweOphaalPatroon, verplaatsbaarBedrijf, nieuwePlek);
                    return huidigeOphaalpatronen;
                }
            return GlobaleOphaalPatronen;
            }
            return huidigeOphaalpatronen;
        }


        public static List<DoubleLinkedList> Add(List<DoubleLinkedList> ophaalpatronen) // Ilan, Aanpassen voor de trips
        {
            if (Program.NietBezochteBedrijven == null || Program.NietBezochteBedrijven.Count == 0)
            {
                Console.WriteLine("No bedrijven to add.");
                return ophaalpatronen;
            }

            Bedrijf bedrijf = Program.NietBezochteBedrijven[random.Next(0, Program.NietBezochteBedrijven.Count)];
            switch (bedrijf.Frequentie)
            {
                case 1:
                    Console.WriteLine("Added bedrijf to random days freq 1");
                    GlobaleOphaalPatronen = AddToRandomDays(ophaalpatronen, bedrijf);
                    break;
                case 2:
                Console.WriteLine("Added bedrijf to specific days, frequentie 2");
                    GlobaleOphaalPatronen = AddToSpecificDays(ophaalpatronen, bedrijf);
                    break;
                case 3:
                    Console.WriteLine("Added bedrijf to specific days, frequentie 3");
                    GlobaleOphaalPatronen = AddToSpecificDays(ophaalpatronen, bedrijf);
                    break;
                case 4:
                    Console.WriteLine("Added bedrijf to specific days, frequentie 4");
                    GlobaleOphaalPatronen = AddToSpecificDays(ophaalpatronen, bedrijf);
                    break;
            }
            Program.NietBezochteBedrijven.Remove(bedrijf);
            return GlobaleOphaalPatronen;
        }

        public static List<DoubleLinkedList> AddToRandomDays(List<DoubleLinkedList> ophaalpatronen, Bedrijf bedrijf) 
        {
            DoubleLinkedList rit = ophaalpatronen[random.Next(0, ophaalpatronen.Count)];
            int index = GetRandomIndex(rit);
            BaseToevoegen(rit, bedrijf, index);
            return ophaalpatronen;
        }

        public static List<DoubleLinkedList> AddToSpecificDays(List<DoubleLinkedList> ophaalpatronen, Bedrijf bedrijf)
        {
            int frequentie = bedrijf.Frequentie;
            List<int[]> sets = GetSetsForFrequentie(frequentie);

            if (frequentie == 1)
            { Console.WriteLine("Frequentie 1"); // Randomly choose between the two sets of arrays
                int[] set = sets[random.Next(0, sets.Count)]; // set van dagen

                int nieuweDag1 = set[random.Next(0, set.Length)]; // index nieuw ophaalpatroon
                DoubleLinkedList nieuweOphaalPatroon1 = ophaalpatronen[nieuweDag1]; // nieuw ophaalpatroon
                int nieuwePlek1 = random.Next(1, nieuweOphaalPatroon1.Count) - 1;
                BaseToevoegen(nieuweOphaalPatroon1, bedrijf, nieuwePlek1);

                return ophaalpatronen;
            }
            else if (frequentie == 2)
            {   
                // Randomly choose between the two sets of arrays
                bool chooseFirstSet = random.Next(0, 2) == 0;

                int nieuweRit1, nieuweRit2;
                if (chooseFirstSet)
                {
                    // maandag & donderdag
                    nieuweRit1 = sets[0][random.Next(0, sets[0].Length)];
                    nieuweRit2 = sets[2][random.Next(0, sets[2].Length)];
                }
                else
                {
                    // disndag & vrijdag
                    nieuweRit1 = sets[1][random.Next(0, sets[1].Length)];
                    nieuweRit2 = sets[3][random.Next(0, sets[3].Length)];
                }

                int nieuwePlek1 = random.Next(1, ophaalpatronen[nieuweRit1].Count) - 1; // nieuwe plek op de eerste dag
                int nieuwePlek2 = random.Next(1, ophaalpatronen[nieuweRit2].Count) - 1; // nieuwe plek op de tweede dag
                BaseToevoegen(GlobaleOphaalPatronen[nieuweRit1], bedrijf, nieuwePlek1);
                BaseToevoegen(GlobaleOphaalPatronen[nieuweRit2], bedrijf, nieuwePlek2);

                return ophaalpatronen;
            }
            else if (frequentie == 3)
            {
                int nieuweRit1 = sets[0][random.Next(0, sets[0].Length)]; // nieuwe rit op maandag
                int nieuweRit2 = sets[1][random.Next(0, sets[1].Length)]; // nieuwe rit op woensdag
                int nieuweRit3 = sets[2][random.Next(0, sets[2].Length)]; // nieuwe rit op vrijdag

                int nieuwePlek1 = random.Next(1, ophaalpatronen[nieuweRit1].Count) - 1; // nieuwe plek op maandag
                int nieuwePlek2 = random.Next(1, ophaalpatronen[nieuweRit2].Count) - 1; // nieuwe plek op woensdag
                int nieuwePlek3 = random.Next(1, ophaalpatronen[nieuweRit3].Count) - 1; // nieuwe plek op vrijdag

                BaseToevoegen(GlobaleOphaalPatronen[nieuweRit1], bedrijf, nieuwePlek1);
                BaseToevoegen(GlobaleOphaalPatronen[nieuweRit2], bedrijf, nieuwePlek2);
                BaseToevoegen(GlobaleOphaalPatronen[nieuweRit3], bedrijf, nieuwePlek3);

                return ophaalpatronen;
            }
            else if (frequentie == 4)
            {
                bool chooseFirstSet = random.Next(0, 2) == 0;
                int nieuweTrip1, nieuweTrip2, nieuweTrip3, nieuweTrip4;
                if (chooseFirstSet)
                {
                    // maandag, dinsdag, woensdag, donderdag
                    nieuweTrip1 = sets[0][random.Next(0, sets[0].Length)]; // nieuwe rit op maandag
                    nieuweTrip2 = sets[1][random.Next(0, sets[1].Length)]; // nieuwe rit op dinsdag
                    nieuweTrip3 = sets[2][random.Next(0, sets[2].Length)]; // nieuwe rit op woensdag
                    nieuweTrip4 = sets[3][random.Next(0, sets[3].Length)]; // nieuwe rit op donderdag
                }
                else
                {
                    // disndag, woensdag, donderdag, vrijdag
                    nieuweTrip1 = sets[1][random.Next(0, sets[1].Length)]; // nieuwe rit op dinsdag
                    nieuweTrip2 = sets[2][random.Next(0, sets[3].Length)]; // nieuwe rit op woensdag
                    nieuweTrip3 = sets[3][random.Next(0, sets[2].Length)]; // nieuwe rit op donderdag
                    nieuweTrip4 = sets[4][random.Next(0, sets[3].Length)]; // nieuwe rit op vrijdag
                }

                int nieuwePlek1 = random.Next(1, ophaalpatronen[nieuweTrip1].Count) - 1; // nieuwe plek op maandag/dinsdag
                int nieuwePlek2 = random.Next(1, ophaalpatronen[nieuweTrip2].Count) - 1; // nieuwe plek op dinsdag/woensdag
                int nieuwePlek3 = random.Next(1, ophaalpatronen[nieuweTrip3].Count) - 1; // nieuwe plek op woensdag/donderdag
                int nieuwePlek4 = random.Next(1, ophaalpatronen[nieuweTrip4].Count) - 1; // nieuwe plek op donderdag/vrijdag

                BaseToevoegen(GlobaleOphaalPatronen[nieuweTrip1], bedrijf, nieuwePlek1);
                BaseToevoegen(GlobaleOphaalPatronen[nieuweTrip2], bedrijf, nieuwePlek2);
                BaseToevoegen(GlobaleOphaalPatronen[nieuweTrip3], bedrijf, nieuwePlek3);
                BaseToevoegen(GlobaleOphaalPatronen[nieuweTrip4], bedrijf, nieuwePlek4);

                return ophaalpatronen;
            }
           return ophaalpatronen;
        }

        private static int GetRandomIndex(DoubleLinkedList rit)
        {
            if (rit == null || rit.Count == 0) return -1;
            return random.Next(0, rit.Count);
        }

        public static List<DoubleLinkedList> Delete(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            DoubleLinkedList rit = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)]; // Random rit
            int index = GetRandomIndex(rit);
            Node node = rit.Index(index);
            if (node == null) return huidigeOphaalpatronen;
            Bedrijf bedrijf = node.data;

            int frequentie = bedrijf.Frequentie;
            List<int[]> sets = GetSetsForFrequentie(frequentie);

            if (frequentie == 1)
            {
                BaseVerwijderen(rit, node);
                return huidigeOphaalpatronen;
            }
            else if (frequentie == 2)
            {   
                List<bool> aanwezigheid = new List<bool>();
                for (int i = 0; i < 15; i++)
                {
                aanwezigheid.Add(false);
                }
                foreach (int[] set in sets)
                {
                    foreach(int dag in set)
                    {
                     if (huidigeOphaalpatronen[dag].Contains(bedrijf) == true) aanwezigheid[dag] = true;
                    }
                }
                if (aanwezigheid[0] == true || aanwezigheid[1] == true || aanwezigheid[2] == true) //patroon is maandag & donderdag
                {
                    foreach (int[] set in sets) //verwijderd alle oude aanwezigheden van het bedrijf
                    {
                        foreach(int dag in set)
                        {
                        if (aanwezigheid[dag]) 
                        {
                            BaseVerwijderen(GlobaleOphaalPatronen[dag], node);
                        }
                        }
                    }
                    return GlobaleOphaalPatronen;
                }
                else if (aanwezigheid[3] == true || aanwezigheid[4] == true || aanwezigheid[5] == true) //patroon is dinsdag & vrijdag)
                {
                    foreach (int[] set in sets) //verwijderd alle oude aanwezigheden van het bedrijf
                    {
                        foreach(int dag in set)
                        {
                        if (aanwezigheid[dag]) 
                        {
                            BaseVerwijderen(GlobaleOphaalPatronen[dag], node);
                        }
                        }
                    }
                    return GlobaleOphaalPatronen;
                }
            }
            else if (frequentie == 3)
            {
                List<bool> aanwezigheid = new List<bool>();
                for (int i = 0; i < 9; i++)
                {
                aanwezigheid.Add(false);
                }
                foreach (int[] set in sets)
                {
                    foreach(int dag in set)
                    {
                     if (huidigeOphaalpatronen[dag].Contains(bedrijf) == true) aanwezigheid[dag] = true;
                    }
                }
                foreach (int[] set in sets) //verwijderd alle aanwezigheden van het bedrijf
                {
                    foreach(int dag in set)
                    {
                        if (aanwezigheid[dag]) 
                        {
                            BaseVerwijderen(GlobaleOphaalPatronen[dag], node);
                        }
                    }
                }
                return GlobaleOphaalPatronen;
            }
            else if (frequentie == 4)
            {

                List<bool> aanwezigheid = new List<bool>();
                for (int i = 0; i < 15; i++)
                {
                aanwezigheid.Add(false);
                }
                foreach (int[] set in sets)
                {
                    foreach(int dag in set)
                    {
                     if (huidigeOphaalpatronen[dag].Contains(bedrijf) == true) aanwezigheid[dag] = true;
                    }
                }
                if (aanwezigheid[0] == true || aanwezigheid[1] == true || aanwezigheid[2] == true) //patroon is maandag-donderdag
                {
                    foreach (int[] set in sets) //verwijderd alle aanwezigheden van het bedrijf
                    {
                        foreach(int dag in set)
                        {
                        if (aanwezigheid[dag]) 
                        {
                            BaseVerwijderen(GlobaleOphaalPatronen[dag], node);
                        }
                        }
                    }
                    return GlobaleOphaalPatronen;
                }
                else if (aanwezigheid[12] == true || aanwezigheid[13] == true || aanwezigheid[14] == true) //patroon is dinsdag-vrijdag)
                                {
                    foreach (int[] set in sets) //verwijderd alle oude aanwezigheden van het bedrijf
                    {
                        foreach(int dag in set)
                        {
                        if (aanwezigheid[dag]) 
                        {
                            BaseVerwijderen(GlobaleOphaalPatronen[dag], node);
                        }
                        }
                    }
                    return GlobaleOphaalPatronen;
                }
            
            }
           return GlobaleOphaalPatronen;
        }

        static void BaseVerwijderen(DoubleLinkedList ophaalpatroon, Node nodeBedrijf)
        {
            Program.NietBezochteBedrijven.Add(nodeBedrijf.data);
            double incrementeel = 0;
            double incrementeelVolume = 0;
            if (nodeBedrijf.previous != null && nodeBedrijf.next != null)
            {
                incrementeel -= Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.data);
                incrementeel -= Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                incrementeel += Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.next.data);
                incrementeel -= nodeBedrijf.data.LedigingsDuurMinuten;
                incrementeelVolume -= nodeBedrijf.data.VolumePerContainer * nodeBedrijf.data.AantContainers;
            }
            else if (nodeBedrijf.previous == null) //eerste bedrijf
            {
            incrementeel -= Program.TijdTussenBedrijven(Program.stortPlaats, nodeBedrijf.data);
            incrementeel -= Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
            incrementeel += Program.TijdTussenBedrijven(Program.stortPlaats, nodeBedrijf.next.data);
            incrementeel -= nodeBedrijf.data.LedigingsDuurMinuten;
            incrementeelVolume -= nodeBedrijf.data.VolumePerContainer * nodeBedrijf.data.AantContainers;
            }   
            else if (nodeBedrijf.next == null) //laatste bedrijf
            {
            incrementeel -= Program.TijdTussenBedrijven(nodeBedrijf.data, Program.stortPlaats);
            incrementeel -= Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.previous.data);
            incrementeel += Program.TijdTussenBedrijven(nodeBedrijf.previous.data,Program.stortPlaats);
            incrementeel -= nodeBedrijf.data.LedigingsDuurMinuten;
            incrementeelVolume -= nodeBedrijf.data.VolumePerContainer * nodeBedrijf.data.AantContainers;
            }   
            Program.incrementeleVolume = incrementeelVolume;
            Program.incrementeleTijd = incrementeel;

            ophaalpatroon.Remove(nodeBedrijf);
        }

        static void BaseToevoegen(DoubleLinkedList ophaalpatroon, Bedrijf bedrijf, int index) //Iza
        {
            Program.NietBezochteBedrijven.Remove(bedrijf);
            Node nodeBedrijf = new Node(bedrijf);
            ophaalpatroon.InsertAtIndex(nodeBedrijf, index);

            double incrementeel = 0;
            double incrementeelVolume = 0;

            if (nodeBedrijf.previous != null && nodeBedrijf.next != null)
            {
                incrementeel += Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.data);
                incrementeel += Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                incrementeel -= Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.next.data);
                incrementeel += nodeBedrijf.data.LedigingsDuurMinuten;
                incrementeelVolume += nodeBedrijf.data.VolumePerContainer * nodeBedrijf.data.AantContainers;
            }
            else if (nodeBedrijf.previous == null) //eerste bedrijf
            {
            incrementeel += Program.TijdTussenBedrijven(Program.stortPlaats, nodeBedrijf.data);
            incrementeel += Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
            incrementeel -= Program.TijdTussenBedrijven(Program.stortPlaats, nodeBedrijf.next.data);
            incrementeel += nodeBedrijf.data.LedigingsDuurMinuten;
            incrementeelVolume += nodeBedrijf.data.VolumePerContainer * nodeBedrijf.data.AantContainers;
            }   
            else if (nodeBedrijf.next == null) //laatste bedrijf
            {
            incrementeel += Program.TijdTussenBedrijven(nodeBedrijf.data, Program.stortPlaats);
            incrementeel += Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.previous.data);
            incrementeel -= Program.TijdTussenBedrijven(nodeBedrijf.previous.data,Program.stortPlaats);
            incrementeel += nodeBedrijf.data.LedigingsDuurMinuten;
            incrementeelVolume += nodeBedrijf.data.VolumePerContainer * nodeBedrijf.data.AantContainers;
            }   
            Program.incrementeleVolume = incrementeelVolume;
            Program.incrementeleTijd = incrementeel;
        }
    }
}
