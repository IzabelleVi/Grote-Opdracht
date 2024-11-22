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
        
        public static List<DoubleLinkedList> ShiftAndereDag(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            incrementeel = 0;
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            int tries = 0;
            while (tries < 10) // 10 keer random bedrijf pakken en dan honderd keer kijken of die past
            {
                int Dag1 = random.Next(0, huidigeOphaalpatronen.Count);
                DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)];
                int index;
                try
                {
                    index = random.Next(1, ophaalPatroon.Count) - 1;
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
                    DoubleLinkedList nieuweOphaalPatroon = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)];
                    int nieuwePlek;
                    try
                    {
                        nieuwePlek = random.Next(1, nieuweOphaalPatroon.Count) - 1;
                    }
                    catch
                    {
                        break;
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

        public static List<DoubleLinkedList> ShiftZelfdeDag(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            incrementeel = 0;
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            int tries = 0;
            while (tries < 10) // 10 keer random bedrijf pakken en dan honderd keer kijken of die past
            {
                int indexOphaalPatroon = random.Next(0, huidigeOphaalpatronen.Count);
                DoubleLinkedList ophaalPatroon = huidigeOphaalpatronen[indexOphaalPatroon];
                int index;
                try
                {
                    index = random.Next(1, ophaalPatroon.Count) - 1;
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

        public static List<DoubleLinkedList> Add (List<DoubleLinkedList> ophaalpatronen)
        {
            GlobaleOphaalPatronen = ophaalpatronen;
            Bedrijf bedrijf = BeginOplossing.bedrijvenlijst_nog_niet[random.Next(0, BeginOplossing.bedrijvenlijst_nog_niet.Count)];
            incrementeel = -1000;
            switch (bedrijf.Frequentie)
            {
                //elke dag kan
                case 1:
                    for(int i = 0; i < 100; i++)
                    {
                        DoubleLinkedList rit = ophaalpatronen[random.Next(0, ophaalpatronen.Count)];
                        int index;
                        try
                        {
                            index = random.Next(1, rit.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        if (insertChecker(rit, bedrijf, index))
                        {
                            BaseToevoegen(rit, bedrijf, index);
                            Program.huidigeKost += incrementeel;

                            return ophaalpatronen;
                        }
                    }
                    break;
                case 2:
                    for (int tries2 = 0; tries2 < 10; tries2++)
                    {
                        //ma-do of di-vr
                        int temp = (random.Next(0, 2)) * 2;

                        DoubleLinkedList rit = ophaalpatronen[random.Next(0, 2) + temp];
                        int index;
                        try
                        {
                            index = random.Next(1, rit.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        int tries21 = 0;
                        while (!insertChecker(rit, bedrijf, index) && tries21 < 50)
                        {
                            rit = ophaalpatronen[random.Next(0, 2)];
                            index = random.Next(1, rit.Count) - 1;
                            tries21++;
                        }

                        DoubleLinkedList rit2 = ophaalpatronen[random.Next(6, 8) + temp];
                        int index2;
                        try
                        {
                            index2 = random.Next(1, rit2.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        int tries22 = 0;
                        while (!insertChecker(rit2, bedrijf, index2) && tries22 < 50)
                        {
                            rit2 = ophaalpatronen[random.Next(6, 8) + temp];
                            index2 = random.Next(1, rit2.Count) - 1;
                            tries22++;
                        }
                        if (insertChecker(rit, bedrijf, index) && insertChecker(rit2, bedrijf, index2))
                        {
                            BaseToevoegen(rit, bedrijf, index);
                            BaseToevoegen(rit2, bedrijf, index2);
                            Program.huidigeKost += incrementeel;
                            return ophaalpatronen;
                        }
                        
                    
                    }
                    break;
                //ma-wo-vr
                case 3:
                    int tries = 0;
                    while (tries < 300) 
                    {
                        DoubleLinkedList rit = ophaalpatronen[random.Next(0, 2)];
                        int index;
                        try
                        {
                            index = random.Next(1, rit.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        while (!insertChecker(rit, bedrijf, index) && tries<100)
                        {
                            rit = ophaalpatronen[random.Next(0, 2)];
                            index = random.Next(1, rit.Count) - 1;
                            tries++;
                        }

                        DoubleLinkedList rit2 = ophaalpatronen[random.Next(4, 6)];
                        int index2;
                        try
                        {
                            index2 = random.Next(1, rit2.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        while (!insertChecker(rit2, bedrijf, index2) && tries < 200)
                        {
                            rit2 = ophaalpatronen[random.Next(4, 6)];
                            index2 = random.Next(1, rit2.Count) - 1;
                            tries++;
                        }

                        DoubleLinkedList rit3 = ophaalpatronen[random.Next(8, 10)];
                        int index3;
                        try
                        {
                            index3 = random.Next(1, rit3.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        while (!insertChecker(rit3, bedrijf, index3) && tries < 300)
                        {
                            rit3 = ophaalpatronen[random.Next(8, 10)];
                            index3 = random.Next(1, rit3.Count) - 1;
                            tries++;
                        }
                        if(insertChecker(rit, bedrijf, index) && insertChecker(rit2, bedrijf, index2) && insertChecker(rit3, bedrijf, index3))
                        {
                            BaseToevoegen(rit, bedrijf, index);
                            BaseToevoegen(rit2, bedrijf, index2);
                            BaseToevoegen(rit3, bedrijf, index3);
                            Program.huidigeKost += incrementeel;
                            return ophaalpatronen;
                        }
                    }
                    break;
                //ma-di-wo-do  of  di-wo-do-vr
                case 4:
                    for (int tries4 = 0; tries4 < 10; tries4++)
                    {
                        //ma-do of di-vr
                        int temp = (random.Next(0, 2)) * 2;

                        DoubleLinkedList rit = ophaalpatronen[random.Next(0, 2) + temp];
                        int index;
                        try
                        {
                            index = random.Next(1, rit.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        int tries41 = 0;
                        while (!insertChecker(rit, bedrijf, index) && tries41 < 50)
                        {
                            rit = ophaalpatronen[random.Next(0, 2)];
                            index = random.Next(1, rit.Count) - 1;
                            tries41++;
                        }

                        DoubleLinkedList rit2 = ophaalpatronen[random.Next(2, 4) + temp];
                        int index2;
                        try
                        {
                            index2 = random.Next(1, rit2.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        int tries42 = 0;
                        while (!insertChecker(rit2, bedrijf, index2) && tries42 < 50)
                        {
                            rit2 = ophaalpatronen[random.Next(4, 6) + temp];
                            index2 = random.Next(1, rit2.Count) - 1;
                            tries42++;
                        }

                        DoubleLinkedList rit3 = ophaalpatronen[random.Next(4, 6) + temp];
                        int index3;
                        try
                        {
                            index3 = random.Next(1, rit3.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        int tries43 = 0;
                        while (!insertChecker(rit3, bedrijf, index3) && tries43 < 50)
                        {
                            rit3 = ophaalpatronen[random.Next(8, 10) + temp];
                            index3 = random.Next(1, rit3.Count) - 1;
                            tries43++;
                        }
                        DoubleLinkedList rit4 = ophaalpatronen[random.Next(6, 8) + temp];
                        int index4;
                        try
                        {
                            index4 = random.Next(1, rit4.Count) - 1;
                        }
                        catch
                        {
                            break;
                        }
                        int tries44 = 0;
                        while (!insertChecker(rit4, bedrijf, index4) && tries44 < 50)
                        {
                            rit4 = ophaalpatronen[(random.Next(12, 14) + temp )%15];
                            index4 = random.Next(1, rit4.Count) - 1;
                            tries44++;
                        }
                        if (insertChecker(rit, bedrijf, index) && insertChecker(rit2, bedrijf, index2) && insertChecker(rit3, bedrijf, index3) && insertChecker(rit4, bedrijf, index4))
                        {
                            BaseToevoegen(rit, bedrijf, index);
                            BaseToevoegen(rit2, bedrijf, index2);
                            BaseToevoegen(rit3, bedrijf, index3);
                            BaseToevoegen(rit4, bedrijf, index4);
                            Program.huidigeKost += incrementeel;
                            return ophaalpatronen;
                        }
                    }
                    break;
            }
            return ophaalpatronen;
        }

        public static List<DoubleLinkedList> Delete(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            incrementeel = 1000;
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            // Kies een willekeurig ophaalpatroon
            DoubleLinkedList randomPatroon = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)];

            // Kies een willekeurige bedrijf in het ophaalpatroon
            int BedrijfIndex = random.Next(1, randomPatroon.Count) - 1;

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

        public static bool insertChecker(DoubleLinkedList ophaalpatroon, Bedrijf bedrijf, int index)
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
            bool tijd = false;
            bool volume = false;

            if (index == 0)
            {
                return true;
            }
            else
            {
                // Anders, controleer de vorige node en het nieuwe bedrijf op tijd en volume
                Node nodeVorigeBedrijf = ophaalpatroon.Index(index - 1);
                Bedrijf vorigeBedrijf = nodeVorigeBedrijf.data;

                // Controleer of het binnen de tijd past

                double totaleTijd = BeginOplossing.tijden[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] - Program.TijdTussenBedrijven(vorigeBedrijf, nodeVorigeBedrijf.next.data) + Program.TijdTussenBedrijven(vorigeBedrijf, bedrijf) + Program.TijdTussenBedrijven(bedrijf, nodeVorigeBedrijf.next.data) + bedrijf.LedigingsDuurMinuten;

                if (totaleTijd < 570 * 60)
                    tijd = true;

                // Controleer of afval past
                if ((bedrijf.VolumePerContainer * bedrijf.AantContainers) + BeginOplossing.volumes[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] < 20000)
                    volume = true;
            }

            //incrementele kosten berekenenen:
            double incrementeleKosten = 0;
            if (ophaalpatroon.Index(index).previous != null)
            {
                incrementeleKosten += Program.AfstandenMatrix[ophaalpatroon.Index(index).previous.data.MatrixID, bedrijf.MatrixID];
                incrementeleKosten += Program.AfstandenMatrix[bedrijf.MatrixID, ophaalpatroon.Index(index).next.data.MatrixID];
                incrementeleKosten -= Program.AfstandenMatrix[ophaalpatroon.Index(index).previous.data.MatrixID, ophaalpatroon.Index(index).next.data.MatrixID];
            }
            else
            {
                incrementeleKosten += Program.AfstandenMatrix[BeginOplossing.stortPlaats.MatrixID, bedrijf.MatrixID];
                incrementeleKosten += Program.AfstandenMatrix[bedrijf.MatrixID, ophaalpatroon.Index(index).next.data.MatrixID];
                incrementeleKosten -= Program.AfstandenMatrix[BeginOplossing.stortPlaats.MatrixID, ophaalpatroon.Index(index).next.data.MatrixID];
            }

            incrementeel += incrementeleKosten;
            bool incrementeelCheck = CheckAccepteerOplossing();
            if (tijd && volume && incrementeelCheck)
                return true;
            else
            {
                return false;
            }
        }

        public static bool CheckAccepteerOplossing()
        {
            return Program.AccepteerOplossing(incrementeel);
        }

        static void BaseVerwijderen(DoubleLinkedList ophaalpatroon, Node nodeBedrijf)
        {

                try
                {
                    BeginOplossing.bedrijvenlijst_nog_niet.Add(nodeBedrijf.data);

                    //tijd aanpassen
                    if (nodeBedrijf.previous != null)
                    {
                        double temp = 0;
                        temp = temp - Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.data);
                        temp = temp - Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                        temp = temp + Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.next.data);
                        temp = temp + nodeBedrijf.data.LedigingsDuurMinuten;

                        incrementeel += temp;

                        BeginOplossing.tijden[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] += temp;
                    }
                    else
                    {
                        double temp = 0;
                        temp = temp - Program.TijdTussenBedrijven(BeginOplossing.stortPlaats, nodeBedrijf.data);
                        temp = temp - Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                        temp = temp + Program.TijdTussenBedrijven(BeginOplossing.stortPlaats, nodeBedrijf.next.data);
                        temp = temp + nodeBedrijf.data.LedigingsDuurMinuten;
                        incrementeel += temp;

                        BeginOplossing.tijden[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] += temp;
                    }
                    //volume aanpassen
                    BeginOplossing.volumes[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] -= nodeBedrijf.data.VolumePerContainer * nodeBedrijf.data.AantContainers;

                    ophaalpatroon.Remove(nodeBedrijf);
                }
                catch (Exception e)
                {

                }

        }

        static void BaseToevoegen(DoubleLinkedList ophaalpatroon, Bedrijf bedrijf, int index)
        {
            BeginOplossing.bedrijvenlijst_nog_niet.Remove(bedrijf);
            Node nodeBedrijf = new Node(bedrijf);
            ophaalpatroon.Insert(index, nodeBedrijf);

            //tijden aanpassen
            if (nodeBedrijf.previous != null)
            {
                double temp = 0;
                temp = temp + Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.data);
                temp = temp + Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                temp = temp - Program.TijdTussenBedrijven(nodeBedrijf.previous.data, nodeBedrijf.next.data);
                temp = temp - nodeBedrijf.data.LedigingsDuurMinuten;

                incrementeel += temp;

                BeginOplossing.tijden[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] += temp;
            }
            else
            {
                double temp = 0;
                temp = temp + Program.TijdTussenBedrijven(BeginOplossing.stortPlaats, nodeBedrijf.data);
                temp = temp + Program.TijdTussenBedrijven(nodeBedrijf.data, nodeBedrijf.next.data);
                temp = temp - Program.TijdTussenBedrijven(BeginOplossing.stortPlaats, nodeBedrijf.next.data);
                temp = temp - nodeBedrijf.data.LedigingsDuurMinuten;

                incrementeel += temp;

                BeginOplossing.tijden[GlobaleOphaalPatronen.IndexOf(ophaalpatroon)] += temp;
            }

        }

        public static List<DoubleLinkedList> Toevoegen(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            Bedrijf bedrijf1 = BeginOplossing.bedrijvenlijst_nog_niet[random.Next(0, BeginOplossing.bedrijvenlijst_nog_niet.Count)];


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
                    if (insertChecker(ophaalPatroon, nieuwBedrijf, index))
                    {
                        ophaalPatroon.Insert(index, nieuwBedrijfsNode);
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

