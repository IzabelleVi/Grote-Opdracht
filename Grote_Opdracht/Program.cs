/*  To Do
- Iza: 
    - Check_Legit
- Ilan:
    - Hoeveelheid Trips aanpassen waar nodig
    - Temperatuur aanpassen zodat deze elke nieuwe random start hoger wordt
    - En er voor zorgen dat de temperatuur niet te snel afkoelt, dus bijvoorbeeld maar elke 20 of 50 iteraties afkoelen
- Goof:
    - Het toelaten van schendingen van zowel het volume als de tijd beperking
    - Accepteer oplossing aanpassen zodat er een penalty wordt toegevoegd aan de kost & op basis hiervan wordt geaccepteerd
- Algemeen:
    - Zorgen dat de buurruimtes Toevoegen vooral in het begin en in het einde gebruikt worden
    - Zorgen dat de buurruimtes ShiftAndereDag, ShiftAndereTruck en ShiftZelfdeDag vooral in het midden gebruikt worden & dus niet random
*/

using Grote_Opdracht;

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
    static long maxIteraties = 100000; // Aantal iteraties voor Simulated Annealing
    public static List<double> Rijtijden = new List<double>(); //Iza
    public static List<double> Volumes = new List<double>(); // Iza
    static double temperatuur;
    public static double incrementeleTijd; 
    public static double incrementeleVolume; 
    public static double huidigeKost; //Iza
    public static double totale_kost;
    public static List<Bedrijf> NietBezochteBedrijven = new List<Bedrijf>();
    
    public static Bedrijf stortPlaats = new Bedrijf {
        Order = 0,
        Plaats = "Stortplaats",
        LedigingsDuurMinuten = 30 * 60, // 30 minutes to dispose of waste.
        MatrixID = 287
    };

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

        double allerBesteKost = double.MaxValue;
        List<DoubleLinkedList> allerBesteOphaalpatronen = new List<DoubleLinkedList>();
        int x = 1;
        Random r = new Random();
        while (x < 1000) // Number of iterations to restart the program with a new initial solution
        {
            // Simulated Annealing parameters
            double initieleTemperatuur = 100.0; // Ilan, deze moet bij elke restart van het algoritme verhoogd worden, zodat er meer geexploreerd wordt
            double afkoelingsfactor = 0.95;
            double temperatuur = initieleTemperatuur;

            // Reset the NietBezochteBedrijven list, totale rijtijd en het totale volume
            NietBezochteBedrijven = new List<Bedrijf>(bedrijven);
            Rijtijden.Clear(); 
            for (int i = 0; i < 15; i++)
            {
                Rijtijden.Add(0);
            }
            Volumes.Clear(); 
            for (int i = 0; i < 15; i++) 
            {
                Volumes.Add(0);
            }

            // Generate an initial solution
            List<DoubleLinkedList> huidigeOphaalpatronen = BeginOplossing.WillekeurigeBeginOplossing();
            double huidigeKost = BerekenTotaleKost(huidigeOphaalpatronen); // berekent huidige tijd & volume en gebasseerd daarop de kost + penalty
            Console.WriteLine($"Initiele kost: {huidigeKost} Iteratie: {x}");

            int iteratie = 0;
            while (temperatuur > 1 && iteratie < maxIteraties) // Ilan, hier dus weer zorgen dat de temperatuur niet te snel 1 bereikt, en er voor zorgen dat bij elke nieuwe random start iteratie dit langer duurt.
            {
                // Generate a neighbor solution
                List<DoubleLinkedList> buurOphaalpatronen = BuurRuimteBepalen(huidigeOphaalpatronen);
                double buurKost = BerekenTotaleKost(buurOphaalpatronen); // Iza, misschien nieuwe functie voor berekenen van buurkost incrementeel
                //Console.WriteLine($"Buurkost: {buurKost}");

                // Acceptance criteria
                if (buurKost < huidigeKost || Math.Exp((huidigeKost - buurKost) / temperatuur) > r.NextDouble())
                {
                    huidigeOphaalpatronen = buurOphaalpatronen;
                    huidigeKost = buurKost;
                }

                // Update the best solution found
                if (huidigeKost < allerBesteKost)
                {
                    allerBesteKost = huidigeKost;
                    allerBesteOphaalpatronen = new List<DoubleLinkedList>(huidigeOphaalpatronen);
                }

                // Cool down
                temperatuur *= afkoelingsfactor; // Ilan, dit dus niet elke ronde maar misschien elke 20 iteraties pas doen, zodat er meer geexploreerd wordt
                iteratie++;
            }
            x++;
        }
        // Return the best solution found
        return (allerBesteOphaalpatronen, allerBesteKost);
    }


    public static double TijdTussenBedrijven(Bedrijf? bedrijf1, Bedrijf? bedrijf2) //functioneel, returned de tijd tussen 2 bedrijven
    {
        if (bedrijf1 == null || bedrijf2 == null) return 0;
        int matrixID1 = bedrijf1.MatrixID;
        int matrixID2 = bedrijf2.MatrixID;

        return AfstandenMatrix[matrixID1, matrixID2];
    }

    static List<DoubleLinkedList> BuurRuimteBepalen(List<DoubleLinkedList> OphaalPatroon) //functioneel
    {
        // Bepaald wat we gaan wisselene in het huidige ophaalpatroon, shift een bedrijf tussen andere dagen, trucks, 
        // op een andere plek binnen de zelfde dag of voegt een bedrijf toe of verwijderd een bedrijf. switch is hier overigens
        // niet optimaal, omdat niet elke oplossing even goed werkt, denk dat shift tussen andere dag, truck & zelfde dag het belangrijkst
        // zijn zodra er een redelijke "begin oplossing is, en dat add & delete alleen in het begin nodig zijn.
        Random random = new Random();

        int rand = random.Next(1, 4);
        switch (rand)
        {
            case 1:
            //Console.WriteLine("ShiftAndereDag");
                return BuurRuimtes.ShiftAndereDag(OphaalPatroon);
            case 2:
            //Console.WriteLine("Shift op de Zelfde Dag");
                return BuurRuimtes.ShiftZelfdeDag(OphaalPatroon);
            case 3:
            //Console.WriteLine("Add een bedrijd");
                if(NietBezochteBedrijven.Count > 0)
                    return BuurRuimtes.Add(OphaalPatroon);
                return OphaalPatroon;
            case 4:
            //Console.WriteLine("Delete een bedrijf");
                return BuurRuimtes.Delete(OphaalPatroon);
        }
        return null;
    }

    static void ToonResultaten(List<DoubleLinkedList> besteOphaalpatronen, double besteKost)
    {
        //resultaten moeten in de volgende manier worden weergegeven:
        // vrachtwagen/dag/nummer op lijst/bedrijf id(stort heeft hier 0); bijv 1/4/13/0 (vrachtwagen 1 gaat op donderdag als 13e adres storten)
        //dus dit moet ff worden gefixed
        int[] truck1Trips = { 0, 3, 6, 9, 12 }; // Indexes van de trips die door truck 1 worden gereden, rest is truck 2

        for (int tripIndex = 0; tripIndex < besteOphaalpatronen.Count; tripIndex++)
        {
            int dag = (tripIndex / 3) + 1; // Determine the day based on the trip index
            int vrachtwagen = truck1Trips.Contains(tripIndex) ? 1 : 2; // Determine the truck based on the trip index
            int adres_nummer = 1;
            Node node = besteOphaalpatronen[tripIndex].head;

            while (node != besteOphaalpatronen[tripIndex].tail)
            {
                Console.WriteLine($"{vrachtwagen};{dag};{adres_nummer};{0}"); // Ilan, zorgen dat dit werkt met de nieuwe trips structuur
                adres_nummer++;
                node = node.next;
            }
            Console.WriteLine($"{vrachtwagen};{dag};{adres_nummer};0");
        }
        Console.WriteLine($"Totale kost: {besteKost}");
        Console.WriteLine($"nog te bezoeken bedrijven: {NietBezochteBedrijven.Count}");
    }

    public static bool AccepteerOplossing(double incrementeel) // Goof, hier kun je de tijd en volume schendingen wel accepteren maar er een penalty aan toe voegen.
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

    static double BerekenTotaleTijd(List<DoubleLinkedList> ophaalpatronen) // Iza
    {
        double totalenTijd = 0;
        // Loop door elk patroon
        int k = 0;
        foreach (var patroon in ophaalpatronen)
        {
            Node bedrijf = patroon.head;
            // Loop door elk bedrijf in het patroon en bereken de tijd tussen de bedrijven
            for (int i = 0; i < ophaalpatronen.Count-1; i++)
            {
                if (bedrijf.next != null)
                {
                    Rijtijden[k] += TijdTussenBedrijven(bedrijf.data, bedrijf.next.data);
                    Rijtijden[k] += bedrijf.data.LedigingsDuurMinuten;
                }
                else
                {
                    Rijtijden[k] += 30*60; //Toevoegen van sorttijd aan het eind van het patroon
                    break;
                }
                bedrijf = bedrijf.next;
            }
            k++;
        }
        for (int i = 0; i < Rijtijden.Count; i++)
        {
            totalenTijd += Rijtijden[i];

        }
        return totalenTijd;
    }

    public static List<double> BerekenHuidigeVolume(List<DoubleLinkedList> ophaalpatronen) // Iza
    {
        int k = 0;
        foreach (var patroon in ophaalpatronen)
        {
            Node bedrijf = patroon.head;
            double volume = 0;
            for (int i = 0; i < ophaalpatronen.Count-1; i++)
            {
                volume += bedrijf.data.AantContainers * bedrijf.data.VolumePerContainer;
                bedrijf = bedrijf.next;
            }
            Volumes[k] = volume;
            k++;
        }
        return Volumes;
    }

    static double BerekenTotaleKost(List<DoubleLinkedList> ophaalpatronen) //Iza
    {
        // de tijd:
        totale_kost = BerekenTotaleTijd(ophaalpatronen);
        // het volume:
        BerekenHuidigeVolume(ophaalpatronen);

        foreach (Bedrijf bedrijf in NietBezochteBedrijven) // penalty niet bezochte bedrijven
        {
            totale_kost += bedrijf.LedigingsDuurMinuten * bedrijf.AantContainers * 3 * 60;
        }

        foreach (double Rijtijd in Rijtijden) // Penalty rijtijd overschreden
        {
            if(Rijtijd > 600*60)
            {
                totale_kost += (Rijtijd - 600*60) * 10;
            }
        }

        foreach (double Volume in Volumes) // penalty volume overschreden
        {
            if(Volume > 20000)
            {
                totale_kost += (Volume - 20000) * 10;
            }
        }
        return totale_kost;
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
            if (volume > 20000) // Goof, hier kun je weer de schendingen toelaten
            {
                Console.WriteLine("Fout: vrachtwagen te vol");
                return false;
            }
            if (tijd > 570*60) // Goof, en hier
            {
                Console.WriteLine("Fout: tijdslimiet overschreden");
                return false;
            }
            node = node.next;
        }
        return true;
    }
}
