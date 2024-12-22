using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            
