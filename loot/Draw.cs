using System;
using System.Collections.Generic;

namespace loot
{
    class Draw
    {
        public static void DrawInventory(Pointer pointer, List<Item> inventory)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n-----Inventory-----" + " Selected: " + (pointer.Position + 1) + " of " + inventory.Count);
            for (int i = 0; i < inventory.Count; i++)
            {
                if (i == pointer.Position)
                {
                    Console.Write("> ");
                }
                Console.Write(inventory[i].Name);
                if (inventory[i].ID == Program.player.Equipped.ID)
                {
                    Console.Write(" (e)");
                }
                Console.Write("\n");
            }

            Console.WriteLine("-------------------");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("w/s: Up/Down \t e: Use item \t q: Close inventory");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ResetColor();
        }

        public static void SelectInventory(Pointer pointer, List<Item> inventory)
        {
            ConsoleKeyInfo selector;
            DrawInventory(pointer, inventory);
            selector = Console.ReadKey();

            switch (selector.Key.ToString().ToLower())
            {
                case "w":
                    if (pointer.Position > 0)
                    {
                        Console.Clear();
                        pointer.Position--;
                    }

                    SelectInventory(pointer, inventory);
                    break;
                case "s":
                    if (pointer.Position < (inventory.Count - 1))
                    {
                        Console.Clear();
                        pointer.Position++;
                    }

                    SelectInventory(pointer, inventory);
                    break;
                case "e":
                    Console.Clear();
                    break;
                case "q":
                    Console.Clear();
                    pointer.Position = -1;
                    break;
                default:
                    SelectInventory(pointer, inventory);
                    break;
            }
        }

        public static void DrawPlayerInventory(Pointer pointer)
        {
            Console.Clear();
            Console.WriteLine("Health: " + Program.player.Health + "/" + Program.player.MaxHealth + "\t" +
                "Experience: " + Program.player.Experience + "/" + Program.metaStats.NextLevel + "\t" +
                "Equipped: " + Program.player.Equipped.Name);

            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("\n-----Inventory-----" + " Selected: " + (pointer.Position + 1) + " of " + Program.player.Inventory.Count);
            for (int i = 0; i < Program.player.Inventory.Count; i++)
            {
                if (i == pointer.Position)
                {
                    Console.Write("> ");
                }
                Console.Write(Program.player.Inventory[i].Name);
                if (Program.player.Inventory[i].ID == Program.player.Equipped.ID)
                {
                    Console.Write(" (e)");
                }
                Console.Write("\n");
            }

            Console.WriteLine("-------------------");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("w/s: Up/Down \t e: Use item \t q: Close inventory");
            Console.ForegroundColor = ConsoleColor.White;
            Console.ResetColor();
        }
    }
}