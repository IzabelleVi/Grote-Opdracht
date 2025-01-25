using System.Diagnostics;

namespace Grote_Opdracht
{

    class Node //Dit verwijst naar een bedrijf binnen onze doublelinked list
    {
        public Bedrijf data;
        public Node next;
        public Node previous;

        public Node(Bedrijf data)
        {
            this.data = data;
            next = null;
            previous = null;
        }
    }
    class DoubleLinkedList // Dit is de doublelinked list
    {
        public Node head;
        public Node tail;
        public int count;

        public DoubleLinkedList()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public int Count //Hoeveelheid items in de list
        {
            get { return count; }
        }

        public void AddFirst(Node node) // Voeg een nieuw items vooraan toe
        {
            if (head == null)
            {
                head = node;
                tail = node;
                count++;
            }
            else
            {
                node.next = head;
                head.previous = node;
                head = node;
                count++;
            }
        }

        public void AddLast(Node node) // Voeg een item achteraan toe
        {
            if (head == null)
            {
                head = node;
                tail = node;
                count++;
            }
            else
            {
                node.previous = tail;
                tail.next = node;
                tail = node;
                count++;
            }
        }

        public void RemoveFirst() // Verwijder het eerste item
        {
            if (head == null)
            {
                return;
            }
            else
            {
                head = head.next;
                head.previous = null;
                count--;
            }
        }


        public void RemoveLast() // Verwijder het laatste item
        {
            if (head == null)
            {
                return;
            }
            else
            {
                tail = tail.previous;
                tail.next = null;
                count--;
            }
        }

        public void Remove(Node node) // Verwijder een gespecificeerde node (dit kost O(n) tijd)
        {
            if (head == null)
            {
                return;
            }
            else
            {
                if (node == head)
                {
                    RemoveFirst();
                }
                else if (node == tail)
                {
                    RemoveLast();
                }
                else
                {
                    node.previous.next = node.next;
                    node.next.previous = node.previous;
                    count--;
                }
            }
        }

        public void Clear() // De list leeg maken
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void Print() // Print de hele list
        {
            Node current = head;
            while (current != null)
            {
                Console.WriteLine(current.data.Order);
                current = current.next;
            }
        }

        public Node Find(Bedrijf data) // Zoek een bedrijf binnen de lijst. Kost O(n) tijd
        {
            Node current = head;
            while (current != null)
            {
                if (current.data == data)
                {
                    return current;
                }
                current = current.next;
            }
        return null;
        }

        public Node Index(int index) //returned het bedrijf dat op de gegeven index in de lijst zit. Kost O(n) tijd.
        {
            int i = 0;
            Node current = head;
            while (current != null)
            {
                if (i == index)
                {
                    return current;
                }
                current = current.next;
                i++;
            }
            return null;
        }


        public void VerplaatsBedrijf(Node node, int index) // Verwijderd en herplaatst het bedrijf in een gegeven index. Kost O(n) tijd.
        {
            Remove(node);
            InsertAtIndex(node, index);
        }
        
        public void FindAndReplace(Bedrijf data, Bedrijf newData) // Verwijderd een bedrijf en zet op die plek in de index een nieuw, gegeven bedrijf.
        {
            Node current = head;
            while (current != null)
            {
                if (current.data == data)
                {
                    current.data = newData;
                    break; //stopt met loopen zodra het bedrijf geswappt is
                }
                current = current.next;
            }
        }

        public void WisselBedrijven(Node node1, Node node2) // Wissel 2 gegeven bedrijven binnen de list.
        {
            if (node1 == node2) return; // Geen actie nodig als het dezelfde node is

            // Houd referenties bij voor omliggende nodes
            Node prev1 = node1.previous;
            Node next1 = node1.next;
            Node prev2 = node2.previous;
            Node next2 = node2.next;

            // Update de omliggende nodes van node1
            if (prev1 != null) prev1.next = node2;
            if (next1 != null) next1.previous = node2;

            // Update de omliggende nodes van node2
            if (prev2 != null) prev2.next = node1;
            if (next2 != null) next2.previous = node1;

            // Wissel de previous en next van node1 en node2
            node1.previous = prev2;
            node1.next = next2;
            node2.previous = prev1;
            node2.next = next1;

            // Update head en tail indien nodig
            if (node1 == head) head = node2;
            else if (node2 == head) head = node1;

            if (node1 == tail) tail = node2;
            else if (node2 == tail) tail = node1;
        }


        public void InsertAtIndex(Node node, int index) //Voegt bedrijf toe in gegeven index. Kost O(n/2)
        {
            if (index < 0 || index > count)
            {
                tail.next = node;
                node.previous = tail;
                tail = node;
                count++;
            }

            if (index == 0) AddFirst(node);
            else if (index == count) AddLast(node);
            else
            {
                Node current;
                if (index < count / 2)
                {
                    current = head;
                    for (int i = 0; i < index - 1; i++)
                        current = current.next;
                }
                else
                {
                    current = tail.previous;
                    for (int i = count - 1; i > index; i--)
                        current = current.previous;
                }

                node.next = current.next;
                node.previous = current;

                current.next.previous = node;
                current.next = node;
                count++;
            }
        }

        public void RemoveAtIndex(int index) // Verwijderd bedrijf van gegeven Index 
        {
            if (index < 0 || index >= count) throw new ArgumentOutOfRangeException();

            if (index == 0)
            {
                RemoveFirst();
            }
            else if (index == Count - 1)
            {
                RemoveLast();
            }
            else
            {
                Node current = head;
                for (int i = 0; i < index; i++)
                    current = current.next;

                if (current.previous != null)
                {
                    current.previous.next = current.next;
                }
                if (current.next != null)
                {
                    current.next.previous = current.previous;
                }
                count--;
            }
        }
        
        public bool Contains(Bedrijf data) // Check of een bedrijf in de list zit. Kost O(n) tijd.
        {
            Node current = head;
            while (current != null)
            {
                if (current.data == data)
                {
                    return true;
                }
                current = current.next;
            }
            return false;
        }
    }
            
}
