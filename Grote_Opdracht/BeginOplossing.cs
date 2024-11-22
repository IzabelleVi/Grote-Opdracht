using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grote_Opdracht
{
    internal class BeginOplossing
    {
        public static Random random = new Random();

        public static List<DoubleLinkedList> ophaalpatronen = new List<DoubleLinkedList>();

        // Maak een lijst met alle bedrijven die nog opgehaald moeten worden
        public static List<Bedrijf> bedrijvenlijst_nog_niet = new List<Bedrijf>(Program.bedrijven);

        // Maak arrays om bij te houden of de ritten vol zitten
        public static bool[] ritMogelijkheid = new bool[15];
        public static double[] tijden = new double[15]; 
        public static double[] volumes = new double[15];

        // stortplaats
        public static Bedrijf stortPlaats = new Bedrijf();
        public static List<DoubleLinkedList> WillekeurigeBeginOplossing()
        {
            bedrijvenlijst_nog_niet = new List<Bedrijf>(Program.orgineleBedrijven);
            Random random = new Random();
            for(int i = 0; i < 15; i++)
                ritMogelijkheid[i] = true;
                
            stortPlaats.Order = 0; stortPlaats.MatrixID = 287; stortPlaats.Plaats = "Stortplaats"; stortPlaats.LedigingsDuurMinuten = 30 * 60;
            ophaalpatronen.Clear();


            // Maak 15 ritten aan:
            for (int i = 0; i < 15; i++)
            {
                ophaalpatronen.Add(new DoubleLinkedList());
            }
            while (true)
            {
                Bedrijf bedrijf = bedrijvenlijst_nog_niet[random.Next(0, bedrijvenlijst_nog_niet.Count)];
                switch (bedrijf.Frequentie)
                {
                    case 1:
                        bedrijvenlijst_nog_niet.Remove(bedrijf);
                        int rit = random.Next(0, 15);

                        if (Check_Possible(rit, bedrijf))
                            Toevoegen(bedrijf, rit);

                        else
                        {
                            rit = random.Next(0, 15);
                            for (int i = 0; i < 10; i++)
                            {
                                if (Check_Possible(rit, bedrijf))
                                {
                                    Toevoegen(bedrijf, rit);
                                    break;
                                }
                                else
                                    rit = random.Next(0, 15);
                            }
                        }
                        break;
                    case 2:

                        bedrijvenlijst_nog_niet.Remove(bedrijf);
                        // ma-do of di-vr
                        int temp = (random.Next(0, 2)) * 2;
                        Toevoegen(bedrijf, random.Next(0, 2) + temp);
                        Toevoegen(bedrijf, random.Next(6, 8) + temp);

                        break;
                    case 3:

                        bedrijvenlijst_nog_niet.Remove(bedrijf);
                        // ma-wo-vr
                        Toevoegen(bedrijf, random.Next(0, 2));
                        Toevoegen(bedrijf, random.Next(4, 6));
                        Toevoegen(bedrijf, random.Next(8, 10));
                        break;
                    case 4:

                        bedrijvenlijst_nog_niet.Remove(bedrijf);
                        // ma-di-wo-do of di-wo-do-vr
                        int temp2 = (random.Next(0, 2)) * 2;
                        Toevoegen(bedrijf, random.Next(0, 2) + temp2);
                        Toevoegen(bedrijf, random.Next(2, 4) + temp2);
                        Toevoegen(bedrijf, random.Next(4, 6) + temp2);
                        Toevoegen(bedrijf, random.Next(6, 8) + temp2);
                        break;
                    case 5:

                        bedrijvenlijst_nog_niet.Remove(bedrijf);
                        // ma-di-wo-do-vr
                        Toevoegen(bedrijf, random.Next(0, 2));
                        Toevoegen(bedrijf, random.Next(2, 4));
                        Toevoegen(bedrijf, random.Next(4, 6));
                        Toevoegen(bedrijf, random.Next(6, 8));
                        Toevoegen(bedrijf, random.Next(8, 10));


                        break;

                }
                if (ritMogelijkheid.All(mogelijkheid => mogelijkheid == false))
                    break;
            }

            // dit doet me ongelooflijk veel pijn
            Toevoegen(stortPlaats, 0);
            Toevoegen(stortPlaats, 1);
            Toevoegen(stortPlaats, 2);
            Toevoegen(stortPlaats, 3);
            Toevoegen(stortPlaats, 4);
            Toevoegen(stortPlaats, 5);
            Toevoegen(stortPlaats, 6);
            Toevoegen(stortPlaats, 7);
            Toevoegen(stortPlaats, 8);
            Toevoegen(stortPlaats, 9);
            Toevoegen(stortPlaats, 10);
            Toevoegen(stortPlaats, 11);
            Toevoegen(stortPlaats, 12);
            Toevoegen(stortPlaats, 13);
            Toevoegen(stortPlaats, 14);
            return ophaalpatronen;
        }

        public static bool Check_Possible(int rit, Bedrijf bedrijf)
        {
            bool tijd = false;
            bool volume = false;

            if (ophaalpatronen[rit].tail == null)
                return true;
            Bedrijf vorigeBedrijf = ophaalpatronen[rit].tail.data;

            // check of het binnen de tijd past
            if (Program.AfstandenMatrix[vorigeBedrijf.MatrixID,bedrijf.MatrixID] + tijden[rit] + bedrijf.LedigingsDuurMinuten < 570*60)
                tijd = true;

            // check of afval past
            if ((bedrijf.VolumePerContainer*bedrijf.AantContainers) + volumes[rit] < 20000)
                volume = true;


            if (tijd && volume)
                return true;
            else
            {
                ritMogelijkheid[rit] = false;
                return false;
            }
                
        }

        public static void Toevoegen(Bedrijf bedrijf, int rit)
        {
            Node bedrijfNode = new Node(bedrijf);
            ophaalpatronen[rit].AddLast(bedrijfNode);
            Node vorigBedrijf = bedrijfNode.previous;
            if(vorigBedrijf == null)
            {

                tijden[rit] += Program.TijdTussenBedrijven(stortPlaats, bedrijf) + bedrijf.LedigingsDuurMinuten;
                volumes[rit] += bedrijf.VolumePerContainer * bedrijf.AantContainers;
            }
            else
            {
                tijden[rit] += Program.TijdTussenBedrijven(vorigBedrijf.data, bedrijf) + bedrijf.LedigingsDuurMinuten;
                volumes[rit] += bedrijf.VolumePerContainer * bedrijf.AantContainers;
            }
                
        }
    }
}
