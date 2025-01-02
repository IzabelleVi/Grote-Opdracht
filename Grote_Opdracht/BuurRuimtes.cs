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
            int tries = 0;
            while (tries < 10) // 10 keer random bedrijf pakken en dan honderd keer kijken of die past
            {
                int Ritnummer;
                DoubleLinkedList ophaalPatroon = DoubleLinkedList.KiesRandomOphaalPatroon(huidigeOphaalpatronen, out Ritnummer); // Kies een random ophaalpatroon om een bedrijf uit te wisselen
                int index;
                try
                {
                    index = random.Next(1, ophaalPatroon.Count -2); // Kies een random bedrijf in het ophaalpatroon die niet de stortplaats zijn
                }
                catch
                {
                    continue;
                }
                Node verplaatsbareNode = ophaalPatroon.Index(index);
                Bedrijf verplaatsbaarBedrijf = verplaatsbareNode.data;
                if (verplaatsbaarBedrijf == BeginOplossing.stortPlaats)
                {
                    tries++;
                    continue;
                }

                // Kies een random ophaalpatroon om het bedrijf in te plaatsen, dat binnen de juiste dagen past
                switch (verplaatsbaarBedrijf.Frequentie)
                {
                case 1:
                    for(int i = 0; i < 100; i++) // 100 keer proberen te plaatsen in nieuw ophaalpatroon
                    {
                        DoubleLinkedList rit = huidigeOphaalpatronen[random.Next(0, huidigeOphaalpatronen.Count)];
                        if (rit == ophaalPatroon){continue;}

                        int index2;
                        try
                        {
                            index2 = random.Next(1, rit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                        }
                        catch
                        {
                            break;
                        }
                        if (insertChecker(rit, verplaatsbaarBedrijf, index2))
                        {
                            BaseToevoegen(rit, verplaatsbaarBedrijf, index2);
                            BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                            Program.huidigeKost += incrementeel;
                            //Console.WriteLine("Add(1) is gelukt");
                            return huidigeOphaalpatronen;
                        }
                    }
                    break;
                case 2:
                    //for (int tries2 = 0; tries2 < 100; tries2++) // 100 keren proberen te plaatsen in nieuw ophaalpatroon
                    //ma-do (0,1 ; 6,7) of di-vr (2,3 ; 8,9)
                    if (Ritnummer == 0 || Ritnummer == 1 || Ritnummer == 6 || Ritnummer == 7) // Zit nu in maandag & donderdag, proberen te zetten in  dinsdag & vrijdag
                    {
                        int temp = random.Next(2, 4) ;
                        DoubleLinkedList eersteRit = huidigeOphaalpatronen[temp]; // Kies een random bus op dinsdag
                        temp = random.Next(2, 4) ;
                        DoubleLinkedList tweedeRit = huidigeOphaalpatronen[temp + 6]; // Kies een random bus op vrijdag
                        for (int tries2 = 0; tries2 < 100; tries2++) // 100 keren proberen te plaatsen in nieuw ophaalpatroon
                        {
                            int indexEersteRit;
                            int indexTweedeRit;
                            try
                            {
                                indexEersteRit = random.Next(1, eersteRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                                indexTweedeRit = random.Next(1, tweedeRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                            }
                            catch
                            {
                            break;
                            }

                            if (insertChecker(eersteRit, verplaatsbaarBedrijf, indexEersteRit) && insertChecker(tweedeRit, verplaatsbaarBedrijf, indexTweedeRit))
                            {
                                // Als mogelijk, voeg toe aan nieuwe ophaalpatroon & verwijder uit het oude
                                BaseToevoegen(eersteRit, verplaatsbaarBedrijf, indexEersteRit);
                                BaseToevoegen(tweedeRit, verplaatsbaarBedrijf, indexTweedeRit);
                                if(Ritnummer == 0 || Ritnummer == 1) // Patroon is maandag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee donderdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[6], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[7], verplaatsbareNode);
                                    }
                                }
                                else // Patroon is donderdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee maandag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[0], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[1], verplaatsbareNode);
                                    }
                                }
                                Program.huidigeKost += incrementeel;
                                //Console.WriteLine("Add(2) is gelukt");
                                return huidigeOphaalpatronen;
                            }
                        }
                    }
                    if (Ritnummer == 2 || Ritnummer == 3 || Ritnummer == 8 || Ritnummer == 9) // Zit nu in dinsdag & vrijdag, proberen te zetten in maandag & donderdag
                    {
                        int temp = random.Next(0, 2) ;
                        DoubleLinkedList eersteRit = huidigeOphaalpatronen[temp]; // Kies een random bus op maandag
                        temp = random.Next(0, 2) ;
                        DoubleLinkedList tweedeRit = huidigeOphaalpatronen[temp + 6]; // Kies een random bus op donderdag
                        for (int tries2 = 0; tries2 < 100; tries2++) // 100 keren proberen te plaatsen in nieuw ophaalpatroon
                        {
                            int indexEersteRit;
                            int indexTweedeRit;
                            try
                            {
                                indexEersteRit = random.Next(1, eersteRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                                indexTweedeRit = random.Next(1, tweedeRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                            }
                            catch
                            {
                            break;
                            }

                            if (insertChecker(eersteRit, verplaatsbaarBedrijf, indexEersteRit) && insertChecker(tweedeRit, verplaatsbaarBedrijf, indexTweedeRit))
                            {
                                BaseToevoegen(eersteRit, verplaatsbaarBedrijf, indexEersteRit);
                                BaseToevoegen(tweedeRit, verplaatsbaarBedrijf, indexTweedeRit);
                                if(Ritnummer == 2 || Ritnummer == 3) // Patroon is dinsdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee vrijdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[8], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[9], verplaatsbareNode);
                                    }
                                }
                                else // Patroon is vrijdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee dinsdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[2], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[3], verplaatsbareNode);
                                    }
                                }
                                Program.huidigeKost += incrementeel;
                                //Console.WriteLine("Add(2) is gelukt");
                                return huidigeOphaalpatronen;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Bedrijf wordt op de verkeerde dag opgehaald");
                    }
                    break;
                case 3: // Kan niet naar andere dag verplaats worden, dagen zijn vast
                    break;
                case 4: 
                // Maandag, Dinsdag, Woensdag, Donderdag (0,1 ; 2,3 ; 4,5 ; 6,7) of Dinsdag, Woensdag, Donderdag, Vrijdag (2,3 ; 4,5 ; 6,7 ; 8,9)
                    if (Ritnummer == 0 || Ritnummer == 1 || Ritnummer == 2 || Ritnummer == 3 || Ritnummer == 4 || Ritnummer == 5 || Ritnummer == 6 || Ritnummer == 7) // Zit nu in maandag, dinsdag, woensdag of donderdag, proberen te zetten in dinsdag, woensdag, donderdag of vrijdag
                    {
                        int temp = random.Next(2, 4) ;
                        DoubleLinkedList eersteRit = huidigeOphaalpatronen[temp]; // Kies een random bus op dinsdag
                        temp = random.Next(2, 4) ;
                        DoubleLinkedList tweedeRit = huidigeOphaalpatronen[temp + 2]; // Kies een random bus op woensdag
                        temp = random.Next(2, 4) ;
                        DoubleLinkedList derdeRit = huidigeOphaalpatronen[temp + 4]; // Kies een random bus op donderdag
                        temp = random.Next(2, 4) ;
                        DoubleLinkedList vierdeRit = huidigeOphaalpatronen[temp + 6]; // Kies een random bus op vrijdag
                        for (int tries2 = 0; tries2 < 100; tries2++) // 100 keren proberen te plaatsen in nieuw ophaalpatroon
                        {
                            int indexEersteRit;
                            int indexTweedeRit;
                            int indexDerdeRit;
                            int indexVierdeRit;
                            try
                            {
                                indexEersteRit = random.Next(1, eersteRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                                indexTweedeRit = random.Next(1, tweedeRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                                indexDerdeRit = random.Next(1, derdeRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                                indexVierdeRit = random.Next(1, vierdeRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                            }
                            catch
                            {
                                break;
                            }

                            if (insertChecker(eersteRit, verplaatsbaarBedrijf, indexEersteRit) && insertChecker(tweedeRit, verplaatsbaarBedrijf, indexTweedeRit) && insertChecker(derdeRit, verplaatsbaarBedrijf, indexDerdeRit) && insertChecker(vierdeRit, verplaatsbaarBedrijf, indexVierdeRit))
                            {
                                BaseToevoegen(eersteRit, verplaatsbaarBedrijf, indexEersteRit);
                                BaseToevoegen(tweedeRit, verplaatsbaarBedrijf, indexTweedeRit);
                                BaseToevoegen(derdeRit, verplaatsbaarBedrijf, indexDerdeRit);
                                BaseToevoegen(vierdeRit, verplaatsbaarBedrijf, indexVierdeRit);
                                if(Ritnummer == 0 || Ritnummer == 1) // Patroon is maandag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee dinsdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[2], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[3], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee woensdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[4], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[5], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee donderdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[6], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[7], verplaatsbareNode);
                                    }
                                }
                                else if(Ritnummer == 2 || Ritnummer == 3) // Patroon is dinsdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee maandag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[0], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[1], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee woensdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[4], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[5], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee donderdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[6], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[7], verplaatsbareNode);
                                    }
                                }
                                else if(Ritnummer == 4 || Ritnummer == 5) // Patroon is woensdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee maandag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[0], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[1], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee dinsdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[2], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[3], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee donderdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[6], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[7], verplaatsbareNode);
                                    }
                                }
                                else // Patroon is donderdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee maandag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[0], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[1], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee dinsdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[2], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[3], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee woensdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[4], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[5], verplaatsbareNode);
                                    }
                                }
                                Program.huidigeKost += incrementeel;
                                //Console.WriteLine("Add(4) is gelukt");
                                return huidigeOphaalpatronen;
                            }
                        }
                    }
                    else if (Ritnummer == 2 || Ritnummer == 3 || Ritnummer == 4 || Ritnummer == 5 || Ritnummer == 6 || Ritnummer == 7 || Ritnummer == 8 || Ritnummer == 9) // Patroon is dinsdag, woensdag, donderdag, vrijdag
                    {
                        int temp = random.Next(0, 2) ;
                        DoubleLinkedList eersteRit = huidigeOphaalpatronen[temp]; // Kies een random bus op maandag
                        temp = random.Next(0, 2) ;
                        DoubleLinkedList tweedeRit = huidigeOphaalpatronen[temp + 2]; // Kies een random bus op dinsdag
                        temp = random.Next(0, 2) ;
                        DoubleLinkedList derdeRit = huidigeOphaalpatronen[temp + 4]; // Kies een random bus op woensdag
                        temp = random.Next(0, 2) ;
                        DoubleLinkedList vierdeRit = huidigeOphaalpatronen[temp + 6]; // Kies een random bus op donderdag
                        for (int tries2 = 0; tries2 < 100; tries2++) // 100 keren proberen te plaatsen in nieuw ophaalpatroon
                        {
                            int indexEersteRit;
                            int indexTweedeRit;
                            int indexDerdeRit;
                            int indexVierdeRit;
                            try
                            {
                                indexEersteRit = random.Next(1, eersteRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                                indexTweedeRit = random.Next(1, tweedeRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                                indexDerdeRit = random.Next(1, derdeRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                                indexVierdeRit = random.Next(1, vierdeRit.Count -2); //Zoek nieuwe plek die niet de eerste of laatste is
                            }
                            catch
                            {
                            break;
                            }

                            if (insertChecker(eersteRit, verplaatsbaarBedrijf, indexEersteRit) && insertChecker(tweedeRit, verplaatsbaarBedrijf, indexTweedeRit) && insertChecker(derdeRit, verplaatsbaarBedrijf, indexDerdeRit) && insertChecker(vierdeRit, verplaatsbaarBedrijf, indexVierdeRit))
                            {
                                BaseToevoegen(eersteRit, verplaatsbaarBedrijf, indexEersteRit);
                                BaseToevoegen(tweedeRit, verplaatsbaarBedrijf, indexTweedeRit);
                                BaseToevoegen(derdeRit, verplaatsbaarBedrijf, indexDerdeRit);
                                BaseToevoegen(vierdeRit, verplaatsbaarBedrijf, indexVierdeRit);
                                
                                if(Ritnummer == 2 || Ritnummer == 3) // Patroon is dinsdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee woensdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[4], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[5], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee donderdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[6], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[7], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee vrijdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[8], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[9], verplaatsbareNode);
                                    }
                                }
                                else if(Ritnummer == 4 || Ritnummer == 5) // Patroon is woensdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee dinsdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[2], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[3], verplaatsbareNode);
                                    }
                                    try // Probeer uit een van de twee donderdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[6], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[7], verplaatsbareNode);
                                    }
                                    try // Probeer uit een van de twee vrijdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[8], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[9], verplaatsbareNode);
                                    }
                                }
                                else if (Ritnummer == 6 || Ritnummer == 7)// Patroon is donderdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);

                                    try // Probeer uit een van de twee dinsdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[2], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[3], verplaatsbareNode);
                                    }
                                    try // Probeer uit een van de twee woensdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[4], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[5], verplaatsbareNode);
                                    }
                                    try // Probeer uit een van de twee vrijdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[8], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[9], verplaatsbareNode);
                                    }
                                }
                                else // Patroon is vrijdag
                                {
                                    BaseVerwijderen(ophaalPatroon, verplaatsbareNode);
                                    try // Probeer uit een van de twee dinsdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[2], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[3], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee woensdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[4], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[5], verplaatsbareNode);
                                    }

                                    try // Probeer uit een van de twee donderdag bussen te verwijderen
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[6], verplaatsbareNode);
                                    }
                                    catch
                                    {
                                        BaseVerwijderen(huidigeOphaalpatronen[7], verplaatsbareNode);
                                    }
                                }
                                Program.huidigeKost += incrementeel;
                                //Console.WriteLine("Add(4) is gelukt");
                                return huidigeOphaalpatronen;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Bedrijf wordt op de verkeerde dag opgehaald");
                    }
                    break;
                }
            }
            //Console.WriteLine("Shift andere dag is niet gelukt");
            return huidigeOphaalpatronen;
        }
        public static List<DoubleLinkedList> ShiftAndereTruck(List<DoubleLinkedList> huidigeOphaalpatronen)
        {
            GlobaleOphaalPatronen = huidigeOphaalpatronen;
            incrementeel = 0;
            int tries = 0;
            while (tries <= 10) // 10 times try to pick a random bedrijf and then 100 times check if it fits
            {
                tries++;
                int Dag1 = random.Next(0, huidigeOphaalpatronen.Count);
                int dag2 = (Dag1 % 2 == 0) ? Dag1 + 1 : Dag1 - 1;

                if (dag2 < 0 || dag2 >= huidigeOphaalpatronen.Count)
                {
                    continue; // Skip als dag twee negatief of te groot
                }

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
                    DoubleLinkedList nieuweOphaalPatroon = (random.Next(0, 2) == 1) ? GlobaleOphaalPatronen[Dag1] : GlobaleOphaalPatronen[dag2];
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
            //Console.WriteLine("Shift andere truck is niet gelukt");
            return huidigeOphaalpatronen;
        }

        // Verschuift een bedrijf naar een andere plek binnen hetzelfde ophaalpatroon.
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

            bool added = false;
            switch (bedrijf.Frequentie)
            {
                case 1:
                    added = AddToRandomDays(ophaalpatronen, bedrijf, 100);
                    break;
                case 2:
                    added = AddToSpecificDays(ophaalpatronen, bedrijf, new int[][] { new int[] { 0, 1, 6, 7 }, new int[] { 2, 3, 8, 9 } }, 10);
                    break;
                case 3:
                    added = AddToSpecificDays(ophaalpatronen, bedrijf, new int[][] { new int[] { 0, 1, 4, 5, 8, 9 } }, 300);
                    break;
                case 4:
                    added = AddToSpecificDays(ophaalpatronen, bedrijf, new int[][] { new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new int[] { 2, 3, 4, 5, 6, 7, 8, 9 } }, 10);
                    break;
            }

            if (added)
            {
                Program.NietBezochteBedrijven.Remove(bedrijf);
            }

            return ophaalpatronen;
            }
// Maandag: 01, Dinsdag 23, Woensdag 45, Donderdag 67, Vrijdag 89
            private static bool AddToRandomDays(List<DoubleLinkedList> ophaalpatronen, Bedrijf bedrijf, int maxTries)
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
                        return true;
                    }
                }
                return false;
            }

            private static bool AddToSpecificDays(List<DoubleLinkedList> ophaalpatronen, Bedrijf bedrijf, int[][] dayGroups, int maxTries)
            {
                for (int tries = 0; tries < maxTries; tries++)
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
                                Program.huidigeKost += incrementeel;
                            }
                            return true;
                        }
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
                    dayGroups = new int[][] { new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 } };
                    break;
                case 2:
                    dayGroups = new int[][] { new int[] { 0, 1, 6, 7 }, new int[] { 2, 3, 8, 9 } };
                    break;
                case 3:
                    dayGroups = new int[][] { new int[] { 0, 1, 4, 5, 8, 9 } };
                    break;
                case 4:
                    dayGroups = new int[][] { new int[] { 0, 1, 2, 3, 4, 5, 6, 7 }, new int[] { 2, 3, 4, 5, 6, 7, 8, 9 } };
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
