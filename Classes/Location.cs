using System.Timers;

namespace RPG_Game
{
    internal class Location
    {
        public string Name { get; set; }
        public bool RespawningMonsters { get; set; }
        public int LocationRespawnTimer { get; set; }
        public List<Monster> DefaultMonsters { get; set; }
        public List<Monster> CurrentMonsters { get; set; }

        string BOLD = Console.IsOutputRedirected ? "" : "\x1B[1m";
        string RESETFORMAT = Console.IsOutputRedirected ? "" : "\x1B[0m";
        string RED = Console.IsOutputRedirected ? "" : "\x1B[31m";
        string CYAN = Console.IsOutputRedirected ? "" : "\x1B[36m";

        public void SetName(string name) { Name = name; }
        public void SetRespawningMonsters(bool respawningMonsters) { RespawningMonsters = respawningMonsters; }
        public void SetLocationRespawnTimer(int locationRespawnTimer) { LocationRespawnTimer = locationRespawnTimer; }
        public void SetDefaultMonsters(List<Monster> monsters) { DefaultMonsters = monsters; }
        public void SetCurrentMonsters(List<Monster> monsters) { CurrentMonsters = monsters; }

        //displays details of chosen location
        //allows to fight monsters and go back
        public void DisplayLocationDetails(int locationChoice, Game CurrentGame, List<Location> locations, Character Player, List<Skill> PlayerSkills)
        {
            int getChoice(string? input)
            {
                int choice;

                while (!Int32.TryParse(input, out choice) || choice < 0 || choice > 1)
                {
                    Console.WriteLine($"\n{BOLD}Invalid input! Enter a valid integer between 0 and 1.{RESETFORMAT}");
                    input = Console.ReadLine();
                }

                return choice;
            }

            Location location = locations[locationChoice - 1];

            if (location.RespawningMonsters &&
                location.CurrentMonsters.Count == 0)
            {
                Console.Clear();
                Console.WriteLine($"{BOLD}No Monsters Available. Respawning...");
                Console.WriteLine($"\nPress Enter to continue... {RESETFORMAT}");
                Console.ReadLine();
                DisplayLocations(CurrentGame, locations, Player, PlayerSkills);
            }
            else
            {
                Console.Clear();

                string line = "";

                for (int iterator = 0; iterator < location.Name.Length; iterator++ ) line += "-";

                line += "------";

                Console.WriteLine($"{BOLD}{RED}" + line);
                Console.WriteLine("   " + location.Name + "   ");
                Console.WriteLine(line + $"{RESETFORMAT}");
                Console.WriteLine($"\n{BOLD}List of Monsters:{RESETFORMAT}");

                int i = 0;
                foreach (Monster monster in location.CurrentMonsters)
                {
                    i++;
                    Console.WriteLine($"\n{BOLD}{i}. Monster:{RESETFORMAT}    {monster.Name}");
                    Console.WriteLine($"{BOLD}   Health:{RESETFORMAT}     {Math.Round(monster.CurrentHealth, 2)}/{Math.Round(monster.MaxHealth, 2)}");
                    Console.WriteLine($"{BOLD}   Strength:{RESETFORMAT}   {monster.Damage}");
                    Console.WriteLine($"{BOLD}   Experience:{RESETFORMAT} {monster.Experience}");
                }

                Console.WriteLine($"\n{BOLD}Choose action:");
                Console.WriteLine("1. Tread on (Initiates figh with random monster).");
                Console.WriteLine("0. Exit.\n");

                Console.WriteLine($"Your choice:{RESETFORMAT}");

                int choice = getChoice(Console.ReadLine());

                //intiate fight
                if (choice != 0)
                {
                    //if there are any monsters in the location
                    if (location.CurrentMonsters.Count > 0)
                    {
                        //get random monster
                        Random rnd = new();
                        int index = rnd.Next(location.CurrentMonsters.Count);
                        Monster randomMonster = location.CurrentMonsters[index];

                        //start the fight
                        FightService fightService = new();
                        fightService.Fight(randomMonster, locationChoice, CurrentGame, locations, Player, PlayerSkills);
                    }
                }
                else
                {
                    DisplayLocations(CurrentGame, locations, Player, PlayerSkills);
                }
            }
        }

        //outputs locations
        public void DisplayLocations(Game CurrentGame, List<Location> Locations, Character Player, List<Skill> PlayerSkills)
        {
            int getChoice(string? input)
            {
                int choice;

                while (!Int32.TryParse(input, out choice) || choice < 0 || choice > Locations.Count)
                {
                    Console.WriteLine($"\n{BOLD}Invalid input! Enter a valid integer between 0 and {Locations.Count}.{RESETFORMAT}");
                    input = Console.ReadLine();
                }

                return choice;
            }

            Console.Clear();
            Console.WriteLine($"{BOLD}{CYAN}--- Locations ---\n");

            int index = 0;
            foreach (Location location in Locations)
            {
                index++;
                string formatString = "{0}. {1}{2}";
                string monsterText = location.CurrentMonsters.Count == 0 ? $" - no monsters (respawn takes {location.LocationRespawnTimer / 1000}s)." : ".";
                Console.WriteLine(string.Format(formatString, $"{BOLD}" + index, $"{RESETFORMAT}" + location.Name, monsterText));
            }

            Console.WriteLine($"{BOLD}0.{RESETFORMAT} Exit.");
            Console.WriteLine($"\n{BOLD}Choose current location:");

            Console.WriteLine($"Your choice:{RESETFORMAT}");

            int choice = getChoice(Console.ReadLine());

            if (choice != 0)
            {
                if (Locations[choice - 1].RespawningMonsters &&
                    Locations[choice - 1].CurrentMonsters.Count == 0)
                {
                    Console.WriteLine($"\n{BOLD}No Monsters Available. Respawning...");
                    Console.WriteLine($"Press Enter to continue...{RESETFORMAT}");
                    Console.ReadLine();
                    DisplayLocations(CurrentGame, Locations, Player, PlayerSkills);
                }
                else
                {
                    DisplayLocationDetails(choice, CurrentGame, Locations, Player, PlayerSkills);
                }
            }
            else
            {
                Game game = new();
                game.GameMenu(CurrentGame, Player, PlayerSkills, Locations);
            }
        }
    }
}
