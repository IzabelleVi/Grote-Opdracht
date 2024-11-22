using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grote_Opdracht
{

    class Node
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
    class DoubleLinkedList
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

        public int Count
        {
            get { return count; }
        }

        public void AddFirst(Node node)
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

        public void AddLast(Node node)
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

        public void RemoveFirst()
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

        public void RemoveLast()
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

        public void Remove(Node node)
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

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void Print()
        {
            Node current = head;
            while (current != null)
            {
                Console.WriteLine(current.data);
                current = current.next;
            }
        }

        public Node Find(Bedrijf data)
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

        public Node Index(int index)
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

        public void Insert(int index, Node node)
        {
            int i = 0;
            Node current = head;
            if (index == 0)
            {
                AddFirst(node);
            }
            else if (index == count)
            {
                AddLast(node);
            }
            else
            {
                while (current != null)
                {
                    if (i == index)
                    {
                        node.next = current;
                        node.previous = current.previous;
                        current.previous.next = node;
                        current.previous = node;
                        count++;
                        break;
                    }
                    current = current.next;
                    i++;
                }
            }
            
        }

        public void VerplaatsBedrijf(Node node, int index)
        {
            Remove(node);
            Insert(index, node);
        }
        
        public void FindAndReplace(Bedrijf data, Bedrijf newData)
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

        public void WisselBedrijven(Node node1, Node node2)
        {
            Bedrijf temp = node1.data;
            node1.data = node2.data;
            node2.data = temp;
        }

        public void AddList(DoubleLinkedList extraRit)
        {
            Node node = extraRit.head;
            while(node != extraRit.tail)
            {
                AddLast(node);
                node = node.next;
                count++;
            }
            AddLast(node);
            count++;
        }

        public void InsertAtIndex(Node newNode, int index)
        {
            if (index == 0)
            {
                AddFirst(newNode);
            }
            else if (index == Count)
            {
                AddLast(newNode);
            }
            else
            {
                // Voeg toe in het midden van de lijst
                Node current = head;
                for (int i = 0; i < index - 1; i++)
                    current = current.next;

                newNode.next = current.next;
                newNode.previous = current;

                current.next.previous = newNode;
                current.next = newNode;
            }
        }

        public void RemoveAtIndex(int index)
        {
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

                current.previous.next = current.next;
                current.next.previous = current.previous;
            }
        }
    }
            
}

