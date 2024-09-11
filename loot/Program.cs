using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.XPath;
using System.Reflection;

namespace loot
{
    class Program
    {

        public static IDictionary<int, int> levels = new Dictionary<int, int>()
        {
            {1, 0}, {2, 300}, {3, 900}, {4, 2700}, {5, 6500}, {6, 14000}, {7, 23000}, {8, 34000}, {9, 48000}, {10, 64000}
        };

        public static Player player = new Player();
        public static SaveData saveData = new SaveData();
        public static MetaStatistics metaStats = new MetaStatistics();

        public static Alchemist Alchemist = new Alchemist();
        public static Blacksmith Blacksmith = new Blacksmith();

        public static Pointer pointer = new Pointer();

        //Main 
        static void Main(string[] args)
        {
            MainMenu();
        }

        //Fancy menu screen
        public static void MainMenu()
        {
            Console.Clear();
            player.Health = 15;
            player.MaxHealth = 15;
            player.Inventory.Clear();
            Weapon starterWeapon = new Weapon("Iron Shortsword", 10, 1);
            player.Inventory.Add(starterWeapon);
            player.Equipped = starterWeapon;
            GameMaster.ObtainGold(10000);
            string title = "==============================================\n" +
                           "||||||||||||||||||||||||||||||||||||||||||||||\n" +
                           "==============================================\n\n" +
                           "  L           OOO          OOO       TTTTTTT  \n" +
                           "  L          O   O        O   O         T     \n" +
                           "  L         O     O      O     O        T     \n" +
                           "  L         O     O      O     O        T     \n" +
                           "  L          O   O        O   O         T     \n" +
                           "  LLLLLL      OOO          OOO          T     \n\n" +
                           "==============================================\n" +
                           "||||||||||||||||||||||||||||||||||||||||||||||\n" +
                           "==============================================\n";

            Console.WriteLine(title);
            Prompt.PromptMenu();
        }
    }

    class Prompt
    {
        //Asks the player what they want to do.
        public static void PromptUser()
        {
            if (Program.player.Health >= 1)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1: View Inventory\n" +
                                  "2: Explore\n" +
                                  "3: View Stats\n" +
                                  "4: Leave Dungeon\n" +
                                  "5: Save Game\n" +
                                  "6: Exit Game\n");
                Console.ResetColor(); ;
                string input = Console.ReadLine();

                //Show the inventory, and ask player if they want to use an item.
                switch (input.ToLower())
                {
                    //Show the player's inventory, ask if they want to use an item
                    case "1":
                        int activeSelection = 0;
                        Draw.SelectInventory(Program.pointer, Program.player.Inventory);
                        if (activeSelection == -1)
                        {
                            PromptUser();
                        }
                        else if (Program.player.Inventory[activeSelection] is object)
                        {
                            Program.player.Inventory[activeSelection].Use();
                        }
                        PromptUser();
                        break;
                    //Explore the dungeon
                    case "2":
                        Console.WriteLine("\nYou explore the dungeon further.\n");
                        Program.metaStats.ExplorationsLasted++;
                        Program.metaStats.CurrentDepth++;
                        Random rand = new Random();
                        int chance = rand.Next(100);

                        if (chance >= 0 && chance <= 25)
                            GameMaster.FindChest();
                        else if (chance >= 26 && chance <= 44)
                            GameMaster.FindEnemy();
                        else if (chance >= 45 && chance <= 50)
                            GameMaster.FindTrap();
                        else
                            GameMaster.FindNothing();

                        PromptUser();
                        break;
                    //Show player stats
                    case "3":
                        Console.Clear();
                        Console.WriteLine("You are " + Program.player.Name);
                        Console.WriteLine("\nExplorations lasted: " + Program.metaStats.ExplorationsLasted +
                                          "\nEnemies slain: " + Program.metaStats.EnemiesSlain +
                                          "\nPotions drank: " + Program.metaStats.PotionsDrank +
                                          "\nCrystals used: " + Program.metaStats.CrystalsUsed +
                                          "\nGold obtained: " + Program.metaStats.GoldObtained +
                                          "\nPlayer level: " + Program.player.Level + "\n");
                        PromptUser();
                        break;
                    case "4":
                        double calculateHealth(double health, double maxHealth)
                        {
                            double calculated = (health / maxHealth) * 100;
                            return calculated;
                        }
                        double healthPercentage = calculateHealth(Program.player.Health, Program.player.MaxHealth);
                        Console.Clear();

                        if (healthPercentage > 69 && Program.metaStats.CurrentDepth < 360)
                        {
                            Console.WriteLine("You are " + Program.metaStats.CurrentDepth + " minutes away from the exit.\n" +
                                          "Based on your health, you will survive the journey back.\n");

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Will you leave? (y/n)");
                            Console.ResetColor(); ;

                            string travelchoice = Console.ReadLine();

                            if (travelchoice.ToLower() == "y")
                            {
                                Program.metaStats.CurrentDepth = 0;
                                Console.Clear();
                                Program.player.Location = "town";
                                PromptTown();
                            }
                            else
                            {
                                PromptUser();
                            }
                        }
                        else if (healthPercentage > 69 || Program.metaStats.CurrentDepth > 360)
                        {
                            Console.WriteLine("You are " + Program.metaStats.CurrentDepth + " minutes away from the exit.\n" +
                                          "Based on your depth, you may starve to death.\n");

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Will you leave? (y/n)");
                            Console.ResetColor(); ;

                            string travelchoice = Console.ReadLine();

                            if (travelchoice.ToLower() == "y")
                            {
                                //Change this
                                Console.Clear();
                                Program.metaStats.CurrentDepth = 0;
                                PromptTown();
                            }
                            else
                            {
                                PromptUser();
                            }
                        }
                        else if (healthPercentage <= 69 && Program.metaStats.CurrentDepth <= 10)
                        {
                            Console.WriteLine("You are " + Program.metaStats.CurrentDepth + " minutes away from the exit.\n" +
                                          "You are wounded, but the exit is close by.\n");

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Will you leave? (y/n)");
                            Console.ResetColor(); ;

                            string travelchoice = Console.ReadLine();

                            if (travelchoice.ToLower() == "y")
                            {
                                Console.Clear();
                                Program.metaStats.CurrentDepth = 0;
                                PromptTown();
                            }
                            else
                            {
                                PromptUser();
                            }
                        }
                        else if (healthPercentage <= 69)
                        {
                            Console.WriteLine("You are " + Program.metaStats.CurrentDepth + " minutes away from the exit.\n" +
                                          "Based on your health, you have a " + healthPercentage + "% chance to survive.\n");

                            Console.ForegroundColor = ConsoleColor.Cyan;
                            Console.WriteLine("Will you leave? (y/n)");
                            Console.ResetColor(); ;

                            string travelchoice = Console.ReadLine();

                            if (travelchoice.ToLower() == "y")
                            {
                                Random random = new Random();
                                int chance2 = random.Next(100);

                                if (chance2 > healthPercentage)
                                {
                                    Console.Clear();
                                    Console.WriteLine("Unfortunately, you have succumbed to your wounds.");
                                    Player.Die();
                                }
                                else
                                {
                                    Console.Clear();
                                    Program.metaStats.CurrentDepth = 0;
                                    PromptTown();
                                }
                            }
                            else
                            {
                                PromptUser();
                            }
                        }
                        break;
                    //Save the game
                    case "5":
                        Console.WriteLine("You scribble your journey into your journal...");
                        try
                        {
                            Program.saveData.playerData = Program.player;
                            Program.saveData.meta = Program.metaStats;
                            SaveLoadManager.SavePlayer(Program.saveData, "PLDATA.dat");
                            Console.WriteLine("Success! Press enter to continue.");
                            Console.Read();
                            PromptUser();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("... But were unable to! " +
                                "\n" + ex.Message);
                            Console.Read();
                        }
                        break;
                    //Return to menu
                    case "6":
                        Program.MainMenu();
                        break;
                    //Unknown
                    default:
                        Console.Clear();
                        PromptUser();
                        break;
                }
            }
            else
            {
                Player.Die();
            }
        }

        //This method is used when a player is in combat.
        public static void PromptBattle(Enemy enemy)
        {
            //old
            //int value = 0;
            //Program.itemDamage.TryGetValue(Program.player.Equipped, out value);

            if (enemy.Health > 0) //Only prompt battle if enemy's HP is greater than 0
            {
                if (Program.player.Health <= 0)
                    Player.Die();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\nWhat do you do?\n" +
                                  "1: Attack enemy\n" +
                                  "2: Run Away\n");
                Console.ResetColor(); ;

                string choice = Console.ReadLine();

                if (choice.ToLower() == "1")
                {
                    Console.Clear();
                    Random rand = new Random();
                    int chance = rand.Next(20);

                    if (chance > 10)
                    {
                        int damage = GameMaster.RollDice(6);
                        Console.WriteLine("You hit your enemy for " + (damage) + " damage\n");
                        enemy.Health -= (damage + Program.player.Equipped.BaseDamage);

                        chance = rand.Next(100);

                        if (enemy.Health <= 0)
                            Enemy.Die();

                        if (chance > 70)
                        {
                            damage = GameMaster.RollDice(6);
                            if (damage != 0)
                            {
                                Console.WriteLine("The enemy hits you for " + damage + " damage.");
                                Program.player.Health -= damage;
                            }
                            else
                            {
                                Console.WriteLine("The enemy rolled a critical fail!");
                            }

                            PromptBattle(enemy);
                        }
                        else
                        {
                            Console.WriteLine("You evade the enemy's attack.");
                            PromptBattle(enemy);
                        }
                    }
                    else
                    {
                        Console.WriteLine("The enemy dodges your attack.\n");

                        chance = rand.Next(100);

                        if (chance > 70)
                        {
                            int damage = GameMaster.RollDice(6);
                            Console.WriteLine("The enemy hits you for " + damage + " damage.");
                            Program.player.Health -= damage;
                            PromptBattle(enemy);
                        }
                        else
                        {
                            Console.WriteLine("You evade the enemy's attack.");
                            PromptBattle(enemy);
                        }
                    }
                }
                else if (choice.ToLower() == "2")
                {
                    Console.Clear();
                    Random rand = new Random();
                    int chance = rand.Next(100);

                    if (chance > 50)
                    {
                        Console.WriteLine("You successfully run away.\n");
                        PromptUser();
                    }
                    else
                    {
                        int damage = GameMaster.RollDice(6);
                        Console.WriteLine("You failed to escape!");
                        Console.WriteLine("The enemy hits you for " + damage + " damage.\n");
                        Program.player.Health -= damage;
                        PromptBattle(enemy);
                    }
                }
                else
                {
                    PromptBattle(enemy);
                }
            }
            else //If the enemy has 0 health upon prompting battle
            {
                Enemy.Die();
            }
        }

        //Handles menu selection
        public static void PromptMenu()
        {
            Console.WriteLine("n: New Game");
            Console.WriteLine("c: Continue");
            Console.WriteLine("e: Exit\n");

            ConsoleKeyInfo menuChoice = Console.ReadKey();

            switch (menuChoice.KeyChar.ToString().ToLower())
            {
                //New Game
                case "n":
                    Console.Clear();
                    GenerateCharacter();
                    break;
                //Exit game
                case "e":
                    Environment.Exit(0);
                    break;
                case "c":
                    Console.Clear();
                    try
                    {
                        var loadedPlayer = SaveLoadManager.LoadData("PLDATA.dat");
                        GameMaster.CopyData(loadedPlayer, Program.saveData);
                        Program.player = Program.saveData.playerData;
                        Program.metaStats = Program.saveData.meta;
                        PromptUser();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Press enter to continue");
                        Console.Read();
                    }
                    break;
                //The program will break if this isn't here.
                case "":
                    Console.Clear();
                    Program.MainMenu();
                    break;
                //Unknown selection
                default:
                    Console.WriteLine("\nThat feature hasn't been put in yet!\n");
                    PromptMenu();
                    break;
            }
        }

        //Handles the character generation
        private static void GenerateCharacter()
        {
            string occupation;
            string reason;

            Random rand = new Random();

            Console.Write("You wake with a cold sweat, sunlight streaming from a window." +
                "\nWhatever dream you has is forgotten without a trace." +
                "\nAs you shake away the haze of sleep you start to speak." +
                "\nMy name is...: ");
            string name = Console.ReadLine();

            Program.player.Name = name;
            occupation = Generation.occupation[rand.Next(Generation.occupation.Count)];
            reason = Generation.reason[rand.Next(Generation.reason.Count)];

            Console.WriteLine("\nYou were a " + occupation + " named " + name + " who traveled from far away.");
            Console.WriteLine("You moved to Easthallow " + reason + ".\n");

            Console.WriteLine("Loose lips in taverns spill stories of a dungeon where" +
            "\nbaubles and treasures of unknown quantity await the brave and strong." +
            "\nScraping the bottom of your gold bag yields a meager blade." +
            "\nHaving nothing to lose, you hold your sword close and enter The Dungeon.\n");

            Program.player.Location = "dungeon";
            PromptUser();
        }

        //Prompt the user for what shop they want 
        public static void PromptTown()
        {
            Console.WriteLine("The town is full of people going in and out of shops.\n");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1: Blacksmith\n" +
                              "2: Alchemist Shop\n" +
                              "3: Enter The Dungeon");
            Console.ResetColor(); ;

            string choice = Console.ReadLine();

            switch (choice.ToLower())
            {
                case "1":
                    Console.Clear();
                    Program.player.Location = "blacksmith";
                    PromptBlacksmith();
                    break;
                case "2":
                    Console.Clear();
                    Program.player.Location = "alchemist";
                    PromptAlchemist();
                    break;
                case "3":
                    Console.Clear();
                    Program.player.Location = "dungeon";
                    Console.WriteLine("You arrive back at The Dungeon.\n");
                    PromptUser();
                    break;
                default:
                    Console.Clear();
                    PromptTown();
                    break;
            }
        }

        //Prompt the user what they want to do in the blacksmith 
        public static void PromptBlacksmith()
        {
            Console.WriteLine("Swords of all shapes and sizes line the walls of the blacksmith.\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1) Buy item\n" +
                              "2) Sell item\n" +
                              "3) Leave shop\n");
            Console.ResetColor(); ;

            string choice = Console.ReadLine();

            switch (choice.ToLower())
            {
                case "1":
                    Draw.SelectInventory(Program.pointer, Program.Blacksmith.Inventory);
                    if (Program.pointer.Position == -1)
                    {
                        Program.pointer.Position = 0;
                        PromptBlacksmith();
                    }
                    else if (Program.Blacksmith.Inventory.Contains(Program.Blacksmith.Inventory[Program.pointer.Position]))
                    {
                        Item selectedItem = Program.Blacksmith.Inventory[Program.pointer.Position];
                        Console.Clear();
                        Console.WriteLine("\"Hmm, you know what? I'll charge you " + selectedItem.Value + " for my " + selectedItem.Name + "\"");

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nDo you accept? (y/n)");
                        Console.ResetColor(); ;

                        string input = Console.ReadLine();

                        if (input.ToLower() == "y")
                        {
                            if (Program.player.Gold < selectedItem.Value)
                            {
                                Console.Clear();
                                Console.WriteLine("\"Come back when you get more gold for it.\"\n");
                                PromptTown();
                            }
                            else //TODO: Remove bought item
                            {
                                Program.player.Inventory.Add(selectedItem);
                                Console.Clear();
                                Console.WriteLine("\"It's a done deal.\"\n");
                                Program.player.Gold -= selectedItem.Value;
                                Program.Blacksmith.Gold += selectedItem.Value;
                                PromptBlacksmith();
                            }
                        }
                        else if (input.ToLower() == "n")
                        {
                            Console.Clear();
                            Console.WriteLine("\"Another time perhaps?\"\n");
                            PromptBlacksmith();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("\"It's a simple \'Yes\' or \'No\'.\"");
                            PromptBlacksmith();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\"I don't sell that item.\"\n");
                        PromptBlacksmith();
                    }
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("I ain't buyin' right now.");
                    PromptBlacksmith();
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("\"Good day.\"\n");
                    PromptTown();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("\"I don't know about that.\"\n");
                    PromptBlacksmith();

                    break;
            }
        }

        //Prompt the user what they want to do in the alchemist's shop 
        public static void PromptAlchemist()
        {

            Console.WriteLine("In the corner, a cauldron bubbles as the smell of ingredients invades your nose.\n");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("1) Buy item\n" +
                              "2) Sell item\n" +
                              "3) Leave shop\n");
            Console.ResetColor(); ;

            string choice = Console.ReadLine();

            switch (choice.ToLower())
            {
                case "1":
                    Draw.SelectInventory(Program.pointer, Program.Alchemist.Inventory);
                    if (Program.pointer.Position == -1)
                    {
                        Program.pointer.Position = 0;
                        PromptAlchemist();
                    }
                    else if (Program.Alchemist.Inventory.Contains(Program.Alchemist.Inventory[Program.pointer.Position]))
                    {
                        Item selectedItem = Program.Alchemist.Inventory[Program.pointer.Position];
                        Console.Clear();
                        Console.WriteLine("\"Hmm, you know what? I'll charge you " + selectedItem.Value + " for my " + selectedItem.Name + "\"");

                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("\nDo you accept? (y/n)");
                        Console.ResetColor(); ;

                        string input = Console.ReadLine();

                        if (input.ToLower() == "y")
                        {
                            if (Program.player.Gold < selectedItem.Value)
                            {
                                Console.Clear();
                                Console.WriteLine("\"Come back when you get more gold for it.\"\n");
                                PromptTown();
                            }
                            else //TODO: Remove bought item
                            {
                                Program.player.Inventory.Add(selectedItem);
                                Console.Clear();
                                Console.WriteLine("\"It's a done deal.\"\n");
                                Program.player.Gold -= selectedItem.Value;
                                Program.Alchemist.Gold += selectedItem.Value;
                                PromptAlchemist();
                            }
                        }
                        else if (input.ToLower() == "n")
                        {
                            Console.Clear();
                            Console.WriteLine("\"Another time perhaps?\"\n");
                            PromptAlchemist();
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("\"It's a simple \'Yes\' or \'No\'.\"");
                            PromptAlchemist();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("\"I don't sell that item.\"\n");
                        PromptAlchemist();
                    }
                    break;

                case "2":
                    Console.Clear();
                    Console.WriteLine("I ain't buyin' right now.");
                    PromptAlchemist();
                    break;
                case "3":
                    Console.Clear();
                    Console.WriteLine("\"Good day.\"\n");
                    PromptTown();
                    break;

                default:
                    Console.Clear();
                    Console.WriteLine("\"I don't know about that.\"\n");
                    PromptAlchemist();

                    break;
            }
        }
    }

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

    class Pointer
    {
        public Pointer()
        {
            Position = 0;
        }

        public int Position { get; set; }
    }

    class GameMaster
    {
        public static void ObtainGold()
        {
            Random rand = new Random();
            int chance = rand.Next(10);
            int totalGold = chance + (1 * Program.player.Level);
            Program.player.Gold += totalGold;
            Program.metaStats.GoldObtained += totalGold;
            Console.WriteLine("You've obtained " + totalGold + " gold.\n");
        }

        public static void ObtainGold(int amount)
        {
            Program.player.Gold += amount;
            Program.metaStats.GoldObtained += amount;
            Console.WriteLine("You've obtained " + amount + " gold.\n");
        }

        public static int RollDice(int sides)
        {
            Random rand = new Random();
            int chance = rand.Next(sides);
            return chance++;
        }

        public static void CopyData<T>(T source, T destination)
        {
            try
            {
                var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var property in properties)
                {
                    if (property.CanWrite)
                    {
                        var value = property.GetValue(source);
                        property.SetValue(destination, value);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press enter to continue");
                Console.Read();
            }
        }

        //This method activates if the player finds a chest while exploring
        public static void FindChest()
        {
            Random rand = new Random();
            int chance = rand.Next(100);
            Console.Clear();

            //Health Crystal
            if (chance >= 0 && chance <= 5)
            {
                Crystal crystal = new Crystal();
                crystal.Name = "Health Crystal";
                crystal.Value = 100;
                Console.WriteLine("You find a health crystal.\n");
                Program.player.Inventory.Add(crystal);
            }
            //Potion
            else if (chance >= 11 && chance <= 40)
            {
                Potion potion = new Potion();
                potion.Name = "Potion";
                potion.Value = 50;
                Console.WriteLine("You find a potion.\n");
                Program.player.Inventory.Add(potion);
            }
            //Gold
            else
            {
                int chestGold = (rand.Next(5) + Program.player.Level);
                Console.WriteLine("You found " + chestGold + " gold.\n");
                Program.player.Gold += chestGold;
            }
        }

        //This method activates if the player finds an enemy while exploring
        public static void FindEnemy()
        {
            Enemy enemy = new Enemy();
            Console.Clear();
            Random rand = new Random();
            int chance = rand.Next(30);

            InitiateCombat(enemy);
        }

        //This method activates if the player encounters a trap.
        public static void FindTrap()
        {
            Console.Clear();
            Console.WriteLine("You accidentally trip a wire trap! Arrows shoot out and hit you for 2 health.\n");
            Program.player.Health -= 2;
        }

        //This method activates if the player finds nothing while adventuring
        public static void FindNothing()
        {
            Console.Clear();
            Console.WriteLine("You delve further into the dungeon.\n");
            Prompt.PromptUser();
        }

        //This method activated when the player fights an enemy.
        public static void InitiateCombat(Enemy enemy)
        {
            Console.WriteLine("You start a fight with an enemy " + enemy.Name + "!");

            Random rand = new Random();
            int chance = rand.Next(50);

            if (chance >= 0 && chance <= 25)
            {
                Console.WriteLine("Your natural speed lets you attack first.");
                int value = 0;
                value = Program.player.Equipped.BaseDamage;
                Console.WriteLine("You hit your enemy for " + value + " damage!");
                enemy.Health -= value;
                Prompt.PromptBattle(enemy);
            }
            else
            {
                Console.WriteLine("The enemy's speed gives it an advantage.");
                Console.WriteLine("You take 1 point of damage\n");
                Program.player.Health--;

                if (Program.player.Health <= 0)
                    Player.Die();

                Prompt.PromptBattle(enemy);
            }
        }

        //This method activates when the player obtains exp
        public static void ObtainEXP()
        {
            Console.WriteLine("You obtained 50 EXP.\n");
            Program.player.Experience += 50;

            if (Program.player.Experience >= Program.metaStats.NextLevel)
            {
                Console.WriteLine("Congratulations! You have leveled up.\n");
                Program.player.Level++;
                Program.metaStats.NextLevel = Program.levels[Program.player.Level + 1];
                Prompt.PromptUser();
            }
        }

    }
}