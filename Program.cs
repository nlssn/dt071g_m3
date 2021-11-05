/* 
 * Johannes Nilsson
 * joni1307@student.miun.se
 * HT2021 / DT071G / Moment 3
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Moment3
{
    // Class: GuestBook
    public class GuestBook
    {
        // Properties
        private List<Entry> entries = new List<Entry>(); /* List to store all of the entries */
        private string filePath = @"guestbook.data";

        // Constructor
        /* Fetches the available entries from data file. If there's no file, one is created */
        public GuestBook()
        {
            if (File.Exists(filePath))
            {
                IFormatter formatter = new BinaryFormatter();
                Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                while (stream.Position < stream.Length)
                {
                    Entry newEntry = (Entry)formatter.Deserialize(stream);
                    entries.Add(newEntry);
                }

                stream.Close();
            }
            else
            {
                File.Create(filePath);
            }
        }

        // Methods
        /* addEntry - Adds a new entry to the list, then marshall to save the updated list */
        public Entry addEntry(Entry entry)
        {
            entries.Add(entry);
            marshall();
            return entry;
        }

        /* delEntry - Delete a post from the list, then marshall to save the updated list */
        public int delEntry(int index)
        {
            entries.RemoveAt(index);
            marshall();
            return index;
        }

        /* getEntries - Get all of the entries in the list */
        public List<Entry> getEntries()
        {
            return entries;
        }

        /* marshall - Serialize and write the list of entries to the data file */
        private void marshall()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            foreach(Entry entry in entries)
            {
                formatter.Serialize(stream, entry);
            }

            stream.Close();
        }
    }

    // Class: Entry
    [Serializable]
    public class Entry
    {
        private string author;
        private string message;

        public string Author
        {
            set { this.author = value; }
            get { return this.author; }
        }
        public string Message
        {
            set { this.message = value; }
            get { return this.message; }
        }
    }

    // The main program
    class Program
    {
        static void Main()
        {
            /* Create a new GuestBook */
            GuestBook guestBook = new GuestBook();

            while (true)
            {
                /* Display the main menu */
                Console.Clear();
                Console.WriteLine("J O H A N N E S   G Ä S T B O K\n\n");
                Console.WriteLine("1. Skriv i gästboken");
                Console.WriteLine("2. Ta bort inlägg\n");
                Console.WriteLine("X. Avsluta");

                /* If there are any entries in the guestBook, display them */
                if (guestBook.getEntries().Count > 0)
                {
                    Console.WriteLine(); /* Spacer */
                    for (int i = 0; i < guestBook.getEntries().Count; i++)
                    {
                        Entry entry = guestBook.getEntries()[i];
                        Console.WriteLine($"[{i}] {entry.Author} - {entry.Message}");
                    }
                }

                /* Wait for the user to pick an option from the main menu */
                string input = Console.ReadLine().Trim().ToLower();
                Console.Clear();

                switch (input)
                {
                    case "1":
                        /* Create a new entry*/
                        Entry newEntry = new Entry();

                        /* Keep asking for a name until the user provides one 
                         * Show an error if the string is null/empty
                         */
                        while (true)
                        {
                            try
                            {
                                Console.WriteLine("Ange ditt namn:");
                                newEntry.Author = Console.ReadLine().Trim();
                                
                                if (String.IsNullOrEmpty(newEntry.Author))
                                {
                                    throw new FormatException("Du måste ange ett namn!");
                                }

                                Console.Clear();
                                break; /* Break the while loop */
                            } catch (FormatException ex)
                            {
                                Console.Clear();
                                Console.WriteLine(ex.Message);
                            }
                        }

                        /* Keep asking for a message until the user provides one
                         * Show an error if the string is null/empty
                         */
                        while (true)
                        {
                            try
                            {
                                Console.WriteLine("Ange ditt meddelande:");
                                newEntry.Message = Console.ReadLine().Trim();

                                if (String.IsNullOrEmpty(newEntry.Message))
                                {
                                    throw new FormatException("Du måste ange ett meddelande!");
                                }

                                Console.Clear();
                                break; /* Break the while loop */
                            } catch (FormatException ex)
                            {
                                Console.Clear();
                                Console.WriteLine(ex.Message);
                            }
                        }

                        /* Add the entry to ghe guestBook */
                        guestBook.addEntry(newEntry);

                        break;
                    case "2":
                        /* If there are no entries in the guestBook, stop the user from entering delete mode */
                        if (guestBook.getEntries().Count <= 0)
                        {
                            Console.WriteLine("Det finns inga inlägg.\nTyck på valfri tangent för att komma till huvudmenyn.");
                            Console.ReadLine();

                            Console.Clear();
                            break;
                        } else
                        {
                            /* Keep asking for a entry id until the user provides one
                             * Give an error if the int is empty or larger/smaller than the length of the guestBook list
                             */
                            while (true)
                            {
                                try
                                {
                                    Console.WriteLine("Ange vilket inlägg du vill radera:");
                                    string index = Console.ReadLine().Trim();

                                    guestBook.delEntry(Convert.ToInt32(index));

                                    Console.Clear();
                                    break; /* Break the while loop */
                                }
                                catch (Exception ex)
                                {
                                    Console.Clear();

                                    if (ex is ArgumentOutOfRangeException)
                                    {
                                        Console.WriteLine("Detta ID finns inte!");
                                    }

                                    if (ex is FormatException)
                                    {
                                        Console.WriteLine("Du måste ange ett ID!");
                                    }

                                }
                            }
                        }

                        break;
                    case "x":
                        /* Exit the program*/
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }
}
