using Grote_Opdracht;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;


// Class die wordt gebruikt om een bedrijf te representeren
class Bedrijf //functioneel
{
    public int Order { get; set; }
    public string Plaats { get; set; }
    public int Frequentie { get; set; }
    public int AantContainers { get; set; }
    public double VolumePerContainer { get; set; }
    public double LedigingsDuurMinuten { get; set; }
    public int MatrixID { get; set; }
    public int XCoordinaat { get; set; }
    public int YCoordinaat { get; set; }

    public int langsGeweest = 0;
    public int[] dagen;
    public Bedrijf()
    {
        dagen = new int[Frequentie];
    }
    
}

class Program //functioneel
{
    public static List<Bedrijf> bedrijven;
    public static int[,] AfstandenMatrix;
    static long maxIteraties = 10000000000000; // Aantal iteraties voor Simulated Annealing
    static double Complete_Rijtijd = 0;
    static double temperatuur;
    public static double huidigeKost;
    public static List<Bedrijf> orgineleBedrijven;

    static void Main()
    {
        bedrijven = InlezenBedrijfsData("Orderbestand.txt");
        AfstandenMatrix = InlezenAfstandenData("AfstandenMatrix.txt");

        // Optimalisatiealgoritme oproepen
        (List<DoubleLinkedList> beste, double prijs) = OptimaliseerOphaalpatronen();

        // Resultaten weergeven
        ToonResultaten(beste, prijs);
        Console.ReadLine();
    }

    static List<Bedrijf> InlezenBedrijfsData(string bestandsnaam) //functioneel
    {
        List<Bedrijf> bedrijven = new List<Bedrijf>();

        try
        {
            string[] regels = File.ReadAllLines(bestandsnaam).Skip(1).ToArray(); // Overslaan van de eerste regel met de kolomnamen

            foreach (string regel in regels)
            {
                string[] gegevens = regel.Split(';');

                Bedrijf bedrijf = new Bedrijf
                {
                    Order = int.Parse(gegevens[0]),
                    Plaats = gegevens[1],
                    Frequentie = Convert.ToInt32(gegevens[2][0]) - 48,
                    AantContainers = int.Parse(gegevens[3]),
                    VolumePerContainer = double.Parse(gegevens[4]),
                    LedigingsDuurMinuten = double.Parse(gegevens[5]),
                    MatrixID = int.Parse(gegevens[6]),
                    XCoordinaat = int.Parse(gegevens[7]),
                    YCoordinaat = int.Parse(gegevens[8])
                };
                bedrijven.Add(bedrijf);
            }
            Console.WriteLine($"Aantal bedrijven: {bedrijven.Count}");
            Console.WriteLine($"Succesvol ingelezen");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij inlezen bedrijfsgegevens: {ex.Message}");
        }
        orgineleBedrijven = new List<Bedrijf>(bedrijven);

        return bedrijven;
    }

    static int[,] InlezenAfstandenData(string bestandsnaam) //functioneel
    {
        int[,] afstandenMatrix = new int[bedrijven.Count, bedrijven.Count];
        try
        {
            string[] regels = File.ReadAllLines(bestandsnaam).Skip(1).ToArray(); // Overslaan van de eerste regel met de kolomnamen

            foreach (string regel in regels)
            {
                string[] gegevens = regel.Split(';');
                afstandenMatrix[int.Parse(gegevens[0]), int.Parse(gegevens[1])] = int.Parse(gegevens[3]);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fout bij inlezen afstandsgegevens: {ex.Message}");
        }
        return afstandenMatrix;
    }

    public static (List<DoubleLinkedList> besteOphaalPatronen, double huidigeKost) OptimaliseerOphaalpatronen()
    {
        double allerBesteKost = 1000000000;
        List<DoubleLinkedList> allerBesteOphaalpatronen = new List<DoubleLinkedList>();
        int x = 1;
        while (x < 100000) // Hoeveelheid itteraties dat we het programma opnieuw starten met een nieuwe begin oplossing
        {

            // Simulated Annealing-parameters
            double initieleTemperatuur = 100.0;
            double afkoelingsfactor = 0.95;

            // Parameters op 0 zetten, bij eerste oplossing)
            if (x == 1)
            {
                Clean_bedrijven();
                Complete_Rijtijd = 0;
            }

            // Genereren van een beginoplossing
            List<DoubleLinkedList> huidigeOphaalpatronen = BeginOplossing.WillekeurigeBeginOplossing();

            // Evalueren van de kost van de huidige oplossing
            huidigeKost = BerekenTotaleKost(huidigeOphaalpatronen);

            // Simulated Annealing-algoritme
            int iteratie = 0;
            temperatuur = initieleTemperatuur;

            while (iteratie < maxIteraties && temperatuur > 0.1)
            {
                // Genereren van een buuroplossing
                BuurRuimteBepalen(huidigeOphaalpatronen);

                // Koel het systeem af
                if (iteratie % 10 == 0)
                    temperatuur *= afkoelingsfactor;

                // Incrementeer de iteratie
                iteratie++;
            }
            // Checken of betere oplossing gevonden is.
            if (huidigeKost < allerBesteKost)
            {
                allerBesteKost = huidigeKost;
                allerBesteOphaalpatronen = huidigeOphaalpatronen;
            }

            // Elke 100 itteraties de huidige beste oplossing printen
            x++;
            if (x % 100 == 0)
            {
                Console.WriteLine($"{x}: incr:{allerBesteKost}, tot = {BerekenTotaleKost(allerBesteOphaalpatronen)}");
            }
            
        }
        // Print & returned de beste oplossing
        ToonResultaten(allerBesteOphaalpatronen, allerBesteKost);
        return (allerBesteOphaalpatronen, allerBesteKost);
        
    }

    static List<DoubleLinkedList> RittenSamenvoegen(List<DoubleLinkedList> allerBesteOphaalpatronen) //  Wordt niet gebruikt
    {
        for (int i = 0; i < 15; i++)
        {
            Console.WriteLine($"Rit {i + 1}: {BeginOplossing.tijden[i]} sec (max = {600 * 60}");
        }
        int dag = 0;
        for (int i = 10; i < 15; i++)
        {
            if (BeginOplossing.tijden[dag] + BeginOplossing.tijden[i] < 600 * 60)
            {
                allerBesteOphaalpatronen[dag].AddList(allerBesteOphaalpatronen[i]);
                BeginOplossing.tijden[dag] += BeginOplossing.tijden[i];
            }
            else if (BeginOplossing.tijden[dag + 1] + BeginOplossing.tijden[i] < 600 * 60)
            {
                allerBesteOphaalpatronen[dag + 1].AddList(allerBesteOphaalpatronen[i]);
                BeginOplossing.tijden[dag + 1] += BeginOplossing.tijden[i];
            }
            else
            {
                Console.WriteLine($"Fout bij samenvoegen: rit {i} past niet in dag {dag}");
                throw new Exception("Fout bij samenvoegen");
            }
        }
        
        allerBesteOphaalpatronen.RemoveRange(10, 5);

        return allerBesteOphaalpatronen;
    }   

    static List<DoubleLinkedList> GenereerWillekeurigeOphaalpatronen() // wordt niet gebruikt.
    {
        Random random = new Random();
        List<DoubleLinkedList> willekeurigeOphaalpatronen = new List<DoubleLinkedList>();
        Bedrijf stortPlaats = new Bedrijf(); stortPlaats.Order = 0; stortPlaats.MatrixID = 287; // Ga ervan uit dat de stortplaats bekend is
        int Dag = 1;

        while (Dag < 6) // 5 dagen in de week
        {
            int truck = 1;
            while (truck < 3) // 2 trucks
            {
                DoubleLinkedList huidigeOphaalpatroon = new DoubleLinkedList();
                double totaleTijd = 0;
                double vrachtwagenInhoud = 0;
                Bedrijf vorigBedrijf = stortPlaats; // Begin bij de stortplaats

                while (totaleTijd < 570 * 60) // Tijdslimiet van 9.5 uur (570 minuten)
                {
                    int randomBedrijfIndex = random.Next(0, bedrijven.Count);
                    Bedrijf huidigBedrijf = bedrijven[randomBedrijfIndex];
                    Node temp_node = new Node(huidigBedrijf);

                    // controlleert de tijdslimiet
                    if (totaleTijd + TijdTussenBedrijven(vorigBedrijf, stortPlaats) > 550 * 60 &&
                        vorigBedrijf != stortPlaats)
                    {
                        totaleTijd += TijdTussenBedrijven(vorigBedrijf, stortPlaats);
                        totaleTijd += 30 * 60; //lossen
                        vrachtwagenInhoud = 0;
                        Node stort_node = new Node(stortPlaats);
                        huidigeOphaalpatroon.AddLast(stort_node);
                        
                    }
                    // Controleer of het bedrijf nog niet voldoende is bezocht en of het afval in de vrachtwagen past
                    if (huidigBedrijf.langsGeweest < huidigBedrijf.Frequentie &&
                        vrachtwagenInhoud + huidigBedrijf.AantContainers * huidigBedrijf.VolumePerContainer < 20000)
                    {
                        vrachtwagenInhoud += huidigBedrijf.AantContainers * huidigBedrijf.VolumePerContainer;
                        totaleTijd += huidigBedrijf.LedigingsDuurMinuten + TijdTussenBedrijven(vorigBedrijf, huidigBedrijf);
                        huidigeOphaalpatroon.AddLast(temp_node);
                        huidigBedrijf.langsGeweest++;
                        vorigBedrijf = huidigBedrijf;
                    }
                    else if (vrachtwagenInhoud + huidigBedrijf.AantContainers * huidigBedrijf.VolumePerContainer > 20000)
                    {
                        // Vrachtwagen is vol
                        totaleTijd += TijdTussenBedrijven(vorigBedrijf, stortPlaats);
                        totaleTijd += 30 * 60; //lossen
                        vrachtwagenInhoud = 0;
                        Node stort_node = new Node(stortPlaats);
                        huidigeOphaalpatroon.AddLast(stort_node);
                        vorigBedrijf = stortPlaats;
                    }

                    
                }
                Complete_Rijtijd += totaleTijd; // zodat we aan bij kostenberekening niet extra hoeven op te tellen
                willekeurigeOphaalpatronen.Add(huidigeOphaalpatroon);
                truck++;
            }
            Dag++;
        }

        return willekeurigeOphaalpatronen;
    }

    public static double TijdTussenBedrijven(Bedrijf bedrijf1, Bedrijf bedrijf2) //functioneel, returned de tijd tussen 2 bedrijven
    {
        int matrixID1 = bedrijf1.MatrixID;
        int matrixID2 = bedrijf2.MatrixID;

        return AfstandenMatrix[matrixID1, matrixID2];
    }

    static List<DoubleLinkedList> BuurRuimteBepalen(List<DoubleLinkedList> OphaalPatroon) //functioneel
    {
        // Bepaald wat we gaan wisselene in het huidige ophaalpatroon, shift een bedrijf tussen andere dagen, trucks, 
        // op een andere plek binnen de zelfde dag of voegt een bedrijf toe of verwijderd een bedrijf. switch is hier overigens
        // niet optimaal, omdat niet elke oplossing even goed werkt, denk dat shift tussen andere dag, truck & zelfde dag het belangrijkst
        // zijn zodra er een redelijke "being oplossing is, en dat add & delete alleen in het begin nodig zijn.
        Random random = new Random();

        int rand = random.Next(1, 5);
        switch (rand)
        {
            case 1:
                return BuurRuimtes.ShiftAndereDag(OphaalPatroon);
                break;
            case 2:
                return BuurRuimtes.ShiftAndereTruck(OphaalPatroon);
                break;
            case 3:
                return BuurRuimtes.ShiftZelfdeDag(OphaalPatroon);
                break;
            case 4:
                if(BeginOplossing.bedrijvenlijst_nog_niet.Count > 0)
                    return BuurRuimtes.Add(OphaalPatroon);
                return OphaalPatroon;
                break;
            case 5:
                return BuurRuimtes.Delete(OphaalPatroon);
                break;
        }
        return null;

    }

    static void ToonResultaten(List<DoubleLinkedList> besteOphaalpatronen, double besteKost) // niet functioneel
    {
        //resultaten moeten in de volgende manier worden weergegeven:
            // vrachtwagen/dag/nummer op lijst/bedrijf id(stort heeft hier 0); bijv 1/4/13/0 (vrachtwagen 1 gaat op donderdag als 13e adres storten)
        //dus dit moet ff worden gefixed
        int indexer = 0;
        while(indexer < besteOphaalpatronen.Count)
        {
            int dag = indexer/2 + 1;
            int vrachtwagen = indexer % 2 + 1;
            int adres_nummer = 1;
            Node tail = besteOphaalpatronen[indexer].tail;
            Node node = besteOphaalpatronen[indexer].head;
            double totaalVolume = 0;
            while(node != besteOphaalpatronen[indexer].tail)
            {
                totaalVolume += node.data.AantContainers * node.data.VolumePerContainer;
                if(node.data.Order == 0)
                    totaalVolume = 0;
                Console.WriteLine($"{vrachtwagen};{dag};{adres_nummer};{node.data.Order}");
                adres_nummer++;
                node = node.next;
            }
            indexer++;
        }
        

        Console.WriteLine($"Totale kost: {besteKost}");
        for (int i = 0; i < 15; i++)
        {
            Console.WriteLine($"Rit {i + 1}: {BeginOplossing.tijden[i]} sec (max = {600*60}");
        }
        Console.WriteLine($"nog te bezoeken: {BeginOplossing.bedrijvenlijst_nog_niet.Count}");
    }

    public static bool AccepteerOplossing(double incrementeel) // functioneel
    {
        // Als de buuroplossing beter is, accepteer deze altijd
        if (incrementeel <= 0)
        {
            huidigeKost += incrementeel;
            return true;

        }

        // Anders, accepteer met een bepaalde kans op basis van het temperatuurverschil
        double kans = Math.Exp(incrementeel / temperatuur);
        if(new Random().NextDouble() < kans)
        {
            huidigeKost += incrementeel;
            return true;
        }
        return false;
        
    }

    static double BerekenTotaleKost(List<DoubleLinkedList> ophaalpatronen) //functioneel
    {
        double totale_kost = 0;
        // de tijd:
        for (int i = 0; i < BeginOplossing.tijden.Length; i++)
            totale_kost += BeginOplossing.tijden[i];

        // penalties:
        totale_kost += BeginOplossing.bedrijvenlijst_nog_niet.Count *1000; // penalty voor niet bezochte bedrijven

        return totale_kost;
    }

    static void Clean_bedrijven() //functioneel
    {
        // Zet bij alle bedrijven langsGeweest op 0
        foreach (Bedrijf bedrijf in bedrijven)
        {
            bedrijf.langsGeweest = 0;
        }
    }   

    public static bool Check_Legit(DoubleLinkedList OphaalPatroon) //functioneel
    {
        Bedrijf stortPlaats = new Bedrijf(); stortPlaats.Order = 0; stortPlaats.MatrixID = 287; // Ga ervan uit dat de stortplaats bekend is
        double volume = 0;
        double tijd = 0;

        Node node = OphaalPatroon.head;
        while(node != null)
        {
            volume += node.data.AantContainers * node.data.VolumePerContainer;
            if (node == OphaalPatroon.head)
                tijd += node.data.LedigingsDuurMinuten + TijdTussenBedrijven(stortPlaats, node.data);
            else
                tijd += node.data.LedigingsDuurMinuten + TijdTussenBedrijven(node.previous.data, node.data);
            if (volume > 20000)
            {
                Console.WriteLine("Fout: vrachtwagen te vol");
                return false;
            }
            if (tijd > 570*60)
            {
                Console.WriteLine("Fout: tijdslimiet overschreden");
                return false;
            }
            node = node.next;
        }
        return true;
    }
}
