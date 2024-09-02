namespace RPG_Game
{
    internal class Game
    {
        string BOLD = Console.IsOutputRedirected ? "" : "\x1B[1m";
        string RESETFORMAT = Console.IsOutputRedirected ? "" : "\x1B[0m";
        string CYAN = Console.IsOutputRedirected ? "" : "\x1B[36m";

        public void AddLocation(List<Location> Locations, Location location) { Locations.Add(location); }

        //generates location
        public void GenerateLocationWithMonsters(List<Location> Locations, string name, List<Monster> CurrentMonsters, List<Monster> DefaultMonsters, int LocationRespawnTimer)
        {
            AddLocation(
                Locations,
                new Location()
                {
                    Name = name,
                    RespawningMonsters = false,
                    DefaultMonsters = new List<Monster>(DefaultMonsters),
                    LocationRespawnTimer = LocationRespawnTimer,
                    CurrentMonsters = new List<Monster>(CurrentMonsters)
                });
        }

        public void GameMenu(Game CurrentGame, Character Player, List<Skill> PlayerSkills, List<Location> Locations)
        {
            int getChoice(string? input)
            {
                int choice;

                while (!Int32.TryParse(input, out choice) || choice < 0 || choice > 3)
                {
                    Console.WriteLine($"\n{BOLD}Invalid input! Enter a valid integer between 0 and 3.{RESETFORMAT}");
                    input = Console.ReadLine();
                }

                return choice;
            }

            Console.Clear();
            Console.WriteLine($"{BOLD}{CYAN}--- RPG GAME ---{RESETFORMAT}");
            Console.WriteLine($"\n{BOLD}1.{RESETFORMAT} View Character Details.");
            Console.WriteLine($"{BOLD}2.{RESETFORMAT} Locations.");
            Console.WriteLine($"{BOLD}3.{RESETFORMAT} Rest next to the campfire.");
            Console.WriteLine($"{BOLD}0.{RESETFORMAT} Exit.\n");

            Console.WriteLine($"{BOLD}Your choice:");

            int choice = getChoice(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    Character Character = new();
                    Character.GetPlayerData(Player, PlayerSkills);
                    GameMenu(CurrentGame, Player, PlayerSkills, Locations);
                    break;
                case 2:
                    Location Location = new();
                    Location.DisplayLocations(CurrentGame, Locations, Player, PlayerSkills);
                    break;
                case 3:
                    Player.HealPlayerCampfire(Player);
                    GameMenu(CurrentGame, Player, PlayerSkills, Locations);
                    break;
                case 0:
                    System.Environment.Exit(1);
                    break;
            }
        }

        public void StartGame(Game CurrentGame, List<Location> Locations, Character Player, List<Skill> PlayerSkills)
        {
            Player.GenerateCharacter();

            Player.GenerateSkill(
                PlayerSkills,
                name: "Melee",
                description: "Determines how hard you can hit with melee attacks.",
                level: 10,
                maxLevel: 99,
                experience: 0,
                maxExperience: 200,
                triggerChance: 1,
                scalingFactor: 0.2);

            Player.GenerateSkill(
                PlayerSkills,
                name: "Magic",
                description: "Determines how hard you can hit with magic attacks.",
                level: 10,
                maxLevel: 99,
                experience: 0,
                maxExperience: 200,
                triggerChance: 1,
                scalingFactor: 0.2);

            Player.GenerateSkill(
                PlayerSkills,
                name: "Attack",
                description: "Improves your chance of hitting a target.",
                level: 1, maxLevel: 99,
                experience: 0,
                maxExperience: 200,
                triggerChance: 0.75,
                scalingFactor: 0.15);

            Player.GenerateSkill(
                PlayerSkills,
                name: "Defense",
                description: "Increases chance to block enemy attacks.",
                level: 1,
                maxLevel: 99,
                experience: 0,
                maxExperience: 200,
                triggerChance: 0.25,
                scalingFactor: 0.25);

            Player.GenerateSkill(
                PlayerSkills,
                name: "Critical Hit",
                description: "Increases chance to deal additional damage.",
                level: 1,
                maxLevel: 25,
                experience: 0,
                maxExperience: 200,
                triggerChance: 0.05,
                scalingFactor: 0.5);

            CurrentGame.GenerateLocationWithMonsters(
                Locations,
                name: "Whispering Woods",
                CurrentMonsters: new List<Monster>([
                    new Monster() { Name = "Wolf", MaxHealth = 75, CurrentHealth = 75, Damage = 7, Experience = 30 },
                    new Monster() { Name = "Goblin", MaxHealth = 50, CurrentHealth = 50, Damage = 5, Experience = 20 },
                    new Monster() { Name = "Giant Spider", MaxHealth = 60, CurrentHealth = 60, Damage = 6, Experience = 25 },
                ]),
                DefaultMonsters: new List<Monster>([
                    new Monster() { Name = "Wolf", MaxHealth = 75, CurrentHealth = 75, Damage = 7, Experience = 30 },
                    new Monster() { Name = "Goblin", MaxHealth = 50, CurrentHealth = 50, Damage = 5, Experience = 20 },
                    new Monster() { Name = "Giant Spider", MaxHealth = 60, CurrentHealth = 60, Damage = 6, Experience = 25 },
                ]),
                LocationRespawnTimer: 30000);

            CurrentGame.GenerateLocationWithMonsters(
                Locations,
                name: "Forgotten Ruins",
                CurrentMonsters: new List<Monster>([
                    new Monster() { Name = "Skeleton", MaxHealth = 80, CurrentHealth = 80, Damage = 8, Experience = 35 },
                    new Monster() { Name = "Mummy", MaxHealth = 100, CurrentHealth = 100, Damage = 9, Experience = 40 },
                    new Monster() { Name = "Undead Warrior", MaxHealth = 120, CurrentHealth = 120, Damage = 10, Experience = 45 },
                ]),
                DefaultMonsters: new List<Monster>([
                    new Monster() { Name = "Skeleton", MaxHealth = 80, CurrentHealth = 80, Damage = 8, Experience = 35 },
                    new Monster() { Name = "Mummy", MaxHealth = 100, CurrentHealth = 100, Damage = 9, Experience = 40 },
                    new Monster() { Name = "Undead Warrior", MaxHealth = 120, CurrentHealth = 120, Damage = 10, Experience = 45 },
                ]),
                LocationRespawnTimer: 45000);

            CurrentGame.GenerateLocationWithMonsters(
                Locations,
                name: "Shadowed Caverns",
                CurrentMonsters: new List<Monster>([
                    new Monster() { Name = "Troll", MaxHealth = 150, CurrentHealth = 150, Damage = 12, Experience = 60 },
                    new Monster() { Name = "Orc", MaxHealth = 130, CurrentHealth = 130, Damage = 11, Experience = 55 },
                    new Monster() { Name = "Giant Bat", MaxHealth = 90, CurrentHealth = 90, Damage = 8, Experience = 35 },
                ]),
                DefaultMonsters: new List<Monster>([
                    new Monster() { Name = "Troll", MaxHealth = 150, CurrentHealth = 150, Damage = 12, Experience = 60 },
                    new Monster() { Name = "Orc", MaxHealth = 130, CurrentHealth = 130, Damage = 11, Experience = 55 },
                    new Monster() { Name = "Giant Bat", MaxHealth = 90, CurrentHealth = 90, Damage = 8, Experience = 35 },
                ]),
                LocationRespawnTimer: 60000);

            CurrentGame.GenerateLocationWithMonsters(
                Locations,
                name: "Dragon's Peak",
                CurrentMonsters: new List<Monster>([
                    new Monster() { Name = "Fire Drake", MaxHealth = 200, CurrentHealth = 200, Damage = 15, Experience = 100 },
                    new Monster() { Name = "Mountain Giant", MaxHealth = 250, CurrentHealth = 250, Damage = 20, Experience = 150 },
                    new Monster() { Name = "Griffin", MaxHealth = 180, CurrentHealth = 180, Damage = 14, Experience = 80 },
                ]),
                DefaultMonsters: new List<Monster>([
                    new Monster() { Name = "Fire Drake", MaxHealth = 200, CurrentHealth = 200, Damage = 15, Experience = 100 },
                    new Monster() { Name = "Mountain Giant", MaxHealth = 250, CurrentHealth = 250, Damage = 20, Experience = 150 },
                    new Monster() { Name = "Griffin", MaxHealth = 180, CurrentHealth = 180, Damage = 14, Experience = 80 },
                ]),
                LocationRespawnTimer: 75000);

            CurrentGame.GenerateLocationWithMonsters(
                Locations,
                name: "Infernal Cavern",
                CurrentMonsters: new List<Monster>([
                    new Monster() { Name = "Fire Demon", MaxHealth = 250, CurrentHealth = 250, Damage = 20, Experience = 150 },
                    new Monster() { Name = "Lava Golem", MaxHealth = 200, CurrentHealth = 200, Damage = 15, Experience = 120 },
                    new Monster() { Name = "Hell Hound", MaxHealth = 100, CurrentHealth = 100, Damage = 8, Experience = 50 },
                ]),
                DefaultMonsters: new List<Monster>([
                    new Monster() { Name = "Fire Demon", MaxHealth = 250, CurrentHealth = 250, Damage = 20, Experience = 150 },
                    new Monster() { Name = "Lava Golem", MaxHealth = 200, CurrentHealth = 200, Damage = 15, Experience = 120 },
                    new Monster() { Name = "Hell Hound", MaxHealth = 100, CurrentHealth = 100, Damage = 8, Experience = 50 },
                ]),
                LocationRespawnTimer: 120000);

            CurrentGame.GenerateLocationWithMonsters(
                Locations,
                name: "Frozen Wasteland",
                CurrentMonsters: new List<Monster>([
                    new Monster() { Name = "Ice Titan", MaxHealth = 400, CurrentHealth = 400, Damage = 30, Experience = 300 },
                    new Monster() { Name = "Frost Wyrm Alpha", MaxHealth = 250, CurrentHealth = 250, Damage = 20, Experience = 150 },
                    new Monster() { Name = "Blizzard Storm Elemental", MaxHealth = 150, CurrentHealth = 150, Damage = 15, Experience = 100 },
                    new Monster() { Name = "Frost Dragon", MaxHealth = 500, CurrentHealth = 500, Damage = 40, Experience = 400 },
                ]),
                DefaultMonsters: new List<Monster>([
                    new Monster() { Name = "Ice Titan", MaxHealth = 400, CurrentHealth = 400, Damage = 30, Experience = 300 },
                    new Monster() { Name = "Frost Wyrm Alpha", MaxHealth = 250, CurrentHealth = 250, Damage = 20, Experience = 150 },
                    new Monster() { Name = "Blizzard Storm Elemental", MaxHealth = 150, CurrentHealth = 150, Damage = 15, Experience = 100 },
                    new Monster() { Name = "Frost Dragon", MaxHealth = 500, CurrentHealth = 500, Damage = 40, Experience = 400 },
                ]),
                LocationRespawnTimer: 150000);
        }

        public void RestartGame(Game CurrentGame, List<Location> Locations, Character Player, List<Skill> PlayerSkills)
        {
            Console.Clear();

            PlayerSkills.Clear();
            Locations.Clear();

            StartGame(CurrentGame, Locations, Player, PlayerSkills);
        }
    }
}
