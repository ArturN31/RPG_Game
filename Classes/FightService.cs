namespace RPG_Game
{
    internal class FightService
    {
        string BOLD = Console.IsOutputRedirected ? "" : "\x1B[1m";
        string RESETFORMAT = Console.IsOutputRedirected ? "" : "\x1B[0m";
        string YELLOW = Console.IsOutputRedirected ? "" : "\x1B[33m";
        string RED = Console.IsOutputRedirected ? "" : "\x1B[31m";
        string BLUE = Console.IsOutputRedirected ? "" : "\x1B[34m";

        public void BattleStats(Monster monster, Character Player, List<Skill> PlayerSkills)
        {
            Console.Clear();
            Console.WriteLine("\n{0}", $"{BOLD}{RED}--- " + monster.Name + $" ---{RESETFORMAT}");
            Console.WriteLine("{0,-15} {1}/{2}", $"{BOLD}Health:{RESETFORMAT}     ", Math.Round(monster.CurrentHealth, 2), monster.MaxHealth);
            Console.WriteLine("{0,-15} {1}-{2}", $"{BOLD}Damage:{RESETFORMAT}     ", 1, monster.Damage);
            Console.WriteLine("{0,-15} {1}", $"{BOLD}Experience:{RESETFORMAT} ", monster.Experience);

            Console.WriteLine("\n{0}", $"{BOLD}{BLUE}--- " + Player.Name + $" ---{RESETFORMAT}");
            Console.WriteLine("{0,-15} {1}", $"{BOLD}Level:{RESETFORMAT}      ", Player.Level);
            Console.WriteLine("{0,-15} {1}/{2}", $"{BOLD}Experience:{RESETFORMAT} ", Math.Round(Player.Experience, 2), Math.Round(Player.MaxExperience, 2));
            Console.WriteLine("{0,-15} {1}/{2}", $"{BOLD}Health:{RESETFORMAT}     ", Math.Round(Player.CurrentHealth, 2), Math.Round(Player.MaxHealth));
            Console.WriteLine("{0,-15} {1}/{2}", $"{BOLD}Mana:{RESETFORMAT}       ", Math.Round(Player.CurrentMana, 2), Math.Round(Player.MaxMana));
            Console.WriteLine();


            PlayerSkills.ForEach(skill => { 
                if (skill.Name == "Melee") 
                    Console.WriteLine("{0,-15} {1}-{2}", $"{BOLD}Melee Damage:{RESETFORMAT} ", "1", skill.Level); });

            PlayerSkills.ForEach(skill => {
                if (skill.Name == "Magic")
                    Console.WriteLine("{0,-15} {1}-{2}", $"{BOLD}Magic Damage:{RESETFORMAT} ", skill.Level * 2 / 2, skill.Level * 2);
            });

            PlayerSkills.ForEach(skill => { 
                if (skill.Name == "Attack") 
                    Console.WriteLine("{0,-15} {1}%", $"{BOLD}Atk Chance:{RESETFORMAT}   ", Math.Round(skill.TriggerChance * 100, 2)); });

            PlayerSkills.ForEach(skill => { 
                if (skill.Name == "Defense") 
                    Console.WriteLine("{0,-15} {1}%", $"{BOLD}Def Chance:{RESETFORMAT}   ", Math.Round(skill.TriggerChance * 100, 2)); });

            PlayerSkills.ForEach(skill => { 
                if (skill.Name == "Critical Hit") 
                    Console.WriteLine("{0,-15} {1}%", $"{BOLD}Crit Chance:{RESETFORMAT}  ", Math.Round(skill.TriggerChance * 100, 2)); });
        }



        void PlayerDamage(Monster monster, Character Player, List<Skill> PlayerSkills, string TypeOfAttack)
        {
            //magic attack and player has no mana required for attack
            if (TypeOfAttack == "Magic" && Player.CurrentMana <= 30)
            {
                Console.WriteLine($"\n{BOLD}Mana: {Player.CurrentMana}/{Player.MaxMana}");
                Console.WriteLine($"\n{BOLD}The magic is fading from your veins.");
                Console.WriteLine($"Find a nearby campfire to rekindle your magical spark.{RESETFORMAT}");
            }

            //magic attack and player has mana required for attack
            if (TypeOfAttack == "Magic" && Player.CurrentMana >= 30)
            {
                Player.DealDamage(PlayerSkills, monster, TypeOfAttack);
                Player.SetCurrentMana(Player.CurrentMana - 30);
            }

            if (TypeOfAttack == "Melee")
            {
                Player.DealDamage(PlayerSkills, monster, TypeOfAttack);
            }
        }

        void MonsterDamage(Monster monster, Game CurrentGame, List<Location> Locations, Character Player, List<Skill> PlayerSkills)
        {
            bool BlockedMonstersAttack(List<Skill> PlayerSkills)
            {
                Skill defense = new();
                PlayerSkills.ForEach(skill => {
                    if (skill.Name == "Defense")
                        defense = new()
                        {
                            Name = skill.Name,
                            Description = skill.Description,
                            Level = skill.Level,
                            MaxLevel = skill.MaxLevel,
                            Experience = skill.Experience,
                            MaxExperience = skill.MaxExperience,
                            TriggerChance = skill.TriggerChance,
                            ScalingFactor = skill.ScalingFactor
                        };
                });

                Random rnd = new();

                if (defense == null) return false;
                return rnd.NextDouble() < defense.TriggerChance;
            }

            if (BlockedMonstersAttack(PlayerSkills)) {
                Console.WriteLine($"\n{BOLD}{BLUE}{Player.Name}{RESETFORMAT}{BOLD} has blocked {RED}{monster.Name}{RESETFORMAT}{BOLD} attack.\n");

                //give experience to defense
                PlayerSkills.ForEach(skill =>
                {
                    if (skill.Name == "Defense")
                    {
                        double attackExperience = monster.Damage * skill.ScalingFactor;
                        skill.GiveExperience(skill, attackExperience);
                    }
                });
            }
            else
            {
                //deal damage to the player
                double monsterDamage = monster.MonsterAttack();
                Player.SetCurrentHealth(Player.CurrentHealth - monsterDamage);
                Console.WriteLine($"\n{BOLD}{RED}{monster.Name}{RESETFORMAT}{BOLD} dealt {YELLOW}{Math.Round(monsterDamage, 2)}{RESETFORMAT}{BOLD} damage to {BLUE}{Player.Name}{RESETFORMAT}{BOLD}.{RESETFORMAT}");

                if (Player.CurrentHealth <= 0)
                {
                    Player.PlayerDied(CurrentGame, Locations, Player, PlayerSkills);
                }
            }
        }

        void MonsterKilled(Monster monster, int locationChoice, Game CurrentGame, List<Location> locations, Character Player, List<Skill> PlayerSkills)
        {
            Location Location = new();

            Player.GiveExperience(monster);

            // remove the monster from location
            Location location = locations[locationChoice - 1];
            location.CurrentMonsters.Remove(monster);

            // check if any monsters remain
            if (location.CurrentMonsters.Count > 0)
            {
                Location.DisplayLocationDetails(locationChoice, CurrentGame, locations, Player, PlayerSkills); //continue exploring location
            }
            else
            {
                Monster Monster = new();
                location.SetRespawningMonsters(true);
                Monster.RespawnMonsters(location); //respawn monsters

                Console.Clear();
                Console.WriteLine($"{BOLD}{BLUE}{Player.Name}{RESETFORMAT}{BOLD} has killed all monsters in {RED}{location.Name}{RESETFORMAT}{BOLD}.");
                Console.WriteLine($"Press Enter to continue...{RESETFORMAT}");
                Console.ReadLine();

                Location.DisplayLocations(CurrentGame, locations, Player, PlayerSkills); //navigate to locations
            }
        }

        public void Fight(Monster randomMonster, int locationChoice, Game CurrentGame, List<Location> locations, Character Player, List<Skill> PlayerSkills)
        {
            int getChoice(string? input)
            {
                int choice; 

                while (!Int32.TryParse(input, out choice) || choice < 0 || choice > 2)
                {
                    Console.WriteLine($"\n{BOLD}Invalid input! Enter a valid integer between 0 and 2.{RESETFORMAT}");
                    input = Console.ReadLine();
                }

                return choice;
            }

            BattleStats(randomMonster, Player, PlayerSkills);

            Console.WriteLine($"\n{BOLD}Choose what to do:");
            Console.WriteLine($"1.{RESETFORMAT} Melee Attack.");
            Console.WriteLine($"{BOLD}2.{RESETFORMAT} Magic Attack.");
            Console.WriteLine($"{BOLD}0.{RESETFORMAT} Run Away.\n");

            Console.WriteLine($"{BOLD}Your choice:{RESETFORMAT}");

            int choice = getChoice(Console.ReadLine());

            //go back
            if (choice == 0)
            {
                Location Location = new();
                Location.DisplayLocationDetails(locationChoice, CurrentGame, locations, Player, PlayerSkills);
            }

            //melee attack
            if (choice == 1)
            {
                PlayerDamage(randomMonster, Player, PlayerSkills, "Melee");
                MonsterDamage(randomMonster, CurrentGame, locations, Player, PlayerSkills);

                Console.WriteLine($"\n{BOLD}Press Enter key to continue...{RESETFORMAT}");
                Console.ReadLine();

                //if monsters health is <= 0
                if (randomMonster.CurrentHealth <= 0)
                {
                    MonsterKilled(randomMonster, locationChoice, CurrentGame, locations, Player, PlayerSkills);
                }
                else
                {
                    //monster is alive = continue the fight
                    Fight(randomMonster, locationChoice, CurrentGame, locations, Player, PlayerSkills);
                }
            }

            //magic attack
            if (choice == 2)
            {
                PlayerDamage(randomMonster, Player, PlayerSkills, "Magic");
                MonsterDamage(randomMonster, CurrentGame, locations, Player, PlayerSkills);

                Console.WriteLine($"\n{BOLD}Press Enter key to continue...{RESETFORMAT}");
                Console.ReadLine();

                //if monsters health is <= 0
                if (randomMonster.CurrentHealth <= 0)
                {
                    MonsterKilled(randomMonster, locationChoice, CurrentGame, locations, Player, PlayerSkills);
                }
                else
                {
                    //monster is alive = continue the fight
                    Fight(randomMonster, locationChoice, CurrentGame, locations, Player, PlayerSkills);
                }
            }
        }
    }
}
