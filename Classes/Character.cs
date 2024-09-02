using System.Threading;

namespace RPG_Game
{
    internal class Character
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public double MaxExperience { get; set; }
        public double Experience { get; set; }
        public double MaxHealth { get; set; }
        public double CurrentHealth { get; set; }
        public double MaxMana { get; set; }
        public double CurrentMana { get; set; }

        string BOLD = Console.IsOutputRedirected ? "" : "\x1B[1m";
        string RESETFORMAT = Console.IsOutputRedirected ? "" : "\x1B[0m";
        string YELLOW = Console.IsOutputRedirected ? "" : "\x1B[33m";
        string BLUE = Console.IsOutputRedirected ? "" : "\x1B[34m";
        string RED = Console.IsOutputRedirected ? "" : "\x1B[31m";
        string CYAN = Console.IsOutputRedirected ? "" : "\x1B[36m";

        public void SetName(string name) { Name = name; }
        public void SetLevel(int level) { Level = level; }
        public void SetMaxExperience(double maxExperience) { MaxExperience = maxExperience; }
        public void SetExperience(double experience) { Experience = experience; }
        public void SetMaxHealth(double health) { MaxHealth = health; }
        public void SetCurrentHealth(double health) { CurrentHealth = health; }
        public void SetMaxMana(double mana) { MaxMana = mana; }
        public void SetCurrentMana(double mana) { CurrentMana = mana; }

        //prepares initial Player data
        public void GenerateCharacter()
        {
            Console.WriteLine($"{BOLD}{CYAN}--- RPG GAME ---{RESETFORMAT}");

            Console.WriteLine($"\n{BOLD}Enter character name:{RESETFORMAT}");

            string? nameInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(nameInput))
            {
                SetName(nameInput);
                SetLevel(1);
                SetExperience(990);
                SetMaxExperience(1000);
                SetMaxHealth(200);
                SetCurrentHealth(200);
                SetMaxMana(200);
                SetCurrentMana(200);
            }
            else GenerateCharacter();
        }

        void AddSkill(List<Skill> PlayerSkills, Skill skill) { PlayerSkills.Add(skill); }

        public void GenerateSkill(List<Skill> PlayerSkills,string name, string description, int level, int maxLevel, double experience, double maxExperience, double triggerChance, double scalingFactor)
        {
            AddSkill(PlayerSkills,
                new()
                {
                    Name = name,
                    Description = description,
                    Level = level,
                    MaxLevel = maxLevel,
                    Experience = experience,
                    MaxExperience = maxExperience,
                    TriggerChance = triggerChance,
                    ScalingFactor = scalingFactor
                });
        }

        //outputs Player data
        public void GetPlayerData(Character Player, List<Skill> PlayerSkills)
        {
            Console.Clear();
            Console.WriteLine($"{BOLD}{BLUE}--- Character Details ---{RESETFORMAT}");
            Console.WriteLine($"\n{BOLD}Name:   {RESETFORMAT}{Player.Name}");
            Console.WriteLine($"{BOLD}Level:  {RESETFORMAT}{Player.Level}");
            Console.WriteLine($"{BOLD}Exp:    {RESETFORMAT}{Math.Round(Player.Experience, 2)}/{Math.Round(Player.MaxExperience, 2)}");
            Console.WriteLine($"{BOLD}Health: {RESETFORMAT}{Math.Round(Player.CurrentHealth, 2)}/{Math.Round(Player.MaxHealth, 2)}");
            Console.WriteLine($"{BOLD}Mana:   {RESETFORMAT}{Math.Round(Player.CurrentMana, 2)}/{Math.Round(Player.MaxMana, 2)}");

            GetPlayerSkills(PlayerSkills);

            Console.WriteLine($"\n{BOLD}Press any key to continue...{RESETFORMAT}");
            Console.ReadLine();
        }

        public void GetPlayerSkills(List<Skill> PlayerSkills)
        {
            Console.WriteLine($"\n{BOLD}{BLUE}--- Battle Skills ---{RESETFORMAT}");
            PlayerSkills.ForEach(skill =>
            {
                Console.WriteLine($"\n{BOLD}Name:   {RESETFORMAT}{skill.Name}");
                Console.WriteLine($"{BOLD}Level:  {RESETFORMAT}{skill.Level}/{skill.MaxLevel}");
                Console.WriteLine($"{BOLD}Exp:    {RESETFORMAT}{Math.Round(skill.Experience, 2)}/{Math.Round(skill.MaxExperience, 2)}");
                Console.WriteLine($"{BOLD}Desc:   {RESETFORMAT}{skill.Description}");
                Console.WriteLine($"{BOLD}Chance: {RESETFORMAT}{skill.TriggerChance * 100}%");
            });
        }

        bool DidAttack(List<Skill> PlayerSkills)
        {
            Skill attack = new();
            PlayerSkills.ForEach(skill => { 
                if (skill.Name == "Attack") 
                    attack = new() { 
                        Name = skill.Name, 
                        Description = skill.Description, 
                        Level = skill.Level, 
                        MaxLevel = skill.MaxLevel, 
                        Experience = skill.Experience, 
                        MaxExperience = skill.MaxExperience, 
                        TriggerChance = skill.TriggerChance, 
                        ScalingFactor = skill.ScalingFactor }; });

            Random rnd = new();

            if (attack == null) return false;
            return rnd.NextDouble() < attack.TriggerChance;
        }

        bool IsCriticalHit(List<Skill> PlayerSkills)
        {
            Skill crit = new();
            PlayerSkills.ForEach(skill => { 
                if (skill.Name == "Critical Hit") 
                    crit = new() { 
                        Name = skill.Name, 
                        Description = skill.Description, 
                        Level = skill.Level, 
                        MaxLevel = skill.MaxLevel, 
                        Experience = skill.Experience, 
                        MaxExperience = skill.MaxExperience, 
                        TriggerChance = skill.TriggerChance, 
                        ScalingFactor = skill.ScalingFactor }; });
            
            Random rnd = new();

            if (crit == null) return false;
            return rnd.NextDouble() < crit.TriggerChance;
        }

        public (double, bool) PlayerAttack(List<Skill> PlayerSkills, string TypeOfAttack)
        {
            Skill AttackType = new();
            PlayerSkills.ForEach(skill => { 
                if (skill.Name == TypeOfAttack)
                    AttackType = new() { 
                        Name = skill.Name, 
                        Description = skill.Description, 
                        Level = skill.Level, 
                        MaxLevel = skill.MaxLevel, 
                        Experience = skill.Experience, 
                        MaxExperience = skill.MaxExperience, 
                        TriggerChance = skill.TriggerChance, 
                        ScalingFactor = skill.ScalingFactor }; });
            
            Random rnd = new();

            if (DidAttack(PlayerSkills))
            {
                //critical attack
                if (IsCriticalHit(PlayerSkills))
                {
                    switch (TypeOfAttack)
                    {
                        case "Melee":
                            double MeleeDamageDealt = rnd.Next(1, AttackType.Level);
                            double MeleeCriticalDamage = MeleeDamageDealt * 1.5;
                            return (MeleeCriticalDamage, true);
                        case "Magic":
                            int MagicAttack = AttackType.Level * 2;
                            double MagicDamageDealt = rnd.Next(MagicAttack / 2, MagicAttack);
                            double MagicCriticalDamage = MagicDamageDealt * 1.5;
                            return (MagicCriticalDamage, true);
                        default:
                            //handle case where TypeOfAttack is not "Melee" or "Magic"
                            return (0, false);
                    }
                }
                else
                {
                    switch (TypeOfAttack)
                    {
                        case "Melee":
                            double MeleeDamageDealt = rnd.Next(1, AttackType.Level);
                            return (MeleeDamageDealt, false);
                        case "Magic":
                            int MagicAttack = AttackType.Level * 2;
                            double MagicDamageDealt = rnd.Next(MagicAttack / 2, MagicAttack);
                            return (MagicDamageDealt, false);
                        default:
                            //handle case where TypeOfAttack is not "Melee" or "Magic"
                            return (0, false);
                    }
                }
            }
            else return (0, false);   
        }

        public void DealDamage(List<Skill> PlayerSkills, Monster Monster, string TypeOfAttack)
        {
            (double, bool) playerDamage = PlayerAttack(PlayerSkills, TypeOfAttack);

            if (playerDamage.Item1 > 0)
            {
                //player attacks
                if (playerDamage.Item2)
                {
                    //deal critical damage to the monster
                    Monster.SetCurrentHealth(Monster.CurrentHealth - playerDamage.Item1);
                    Console.WriteLine($"\n{BOLD}{BLUE}{Name}{RESETFORMAT}{BOLD} has dealt {YELLOW}{Math.Round(playerDamage.Item1, 2)}{RESETFORMAT}{BOLD} {TypeOfAttack} damage (Critical Attack) to {RED}{Monster.Name}{RESETFORMAT}{BOLD}.{RESETFORMAT}\n");

                    //give experience to attack
                    PlayerSkills.ForEach(skill =>
                    {
                        if (skill.Name == "Critical Hit")
                        {
                            double critExperience = playerDamage.Item1 * skill.ScalingFactor;
                            skill.GiveExperience(skill, critExperience);
                        }
                    });
                }
                else
                {
                    //deal damage to the monster
                    Monster.SetCurrentHealth(Monster.CurrentHealth - playerDamage.Item1);
                    Console.WriteLine($"\n{BOLD}{BLUE}{Name}{RESETFORMAT}{BOLD} has dealt {YELLOW}{Math.Round(playerDamage.Item1, 2)}{RESETFORMAT}{BOLD} {TypeOfAttack} damage to {RED}{Monster.Name}{RESETFORMAT}{BOLD}.{RESETFORMAT}\n");
                }

                //give experience to attack
                PlayerSkills.ForEach(skill =>
                {
                    if (skill.Name == "Attack")
                    {
                        double attackExperience = playerDamage.Item1 * skill.ScalingFactor;
                        skill.GiveExperience(skill, attackExperience);
                    }
                });

                //give experience to damage
                PlayerSkills.ForEach(skill =>
                {
                    if (skill.Name == TypeOfAttack)
                    {
                        double attackTypeExperience = playerDamage.Item1 * skill.ScalingFactor;
                        skill.GiveExperience(skill, attackTypeExperience);
                    }
                });
            }
            else
            {
                //player missed the attack
                Console.WriteLine($"\n{BOLD}{BLUE}{Name}{RESETFORMAT}{BOLD} has missed a {TypeOfAttack} attack!{RESETFORMAT}");
            }
        }

        public void PlayerDied(Game CurrentGame, List<Location> Locations, Character Player, List<Skill> PlayerSkills)
        {
            Console.Clear();
            Console.WriteLine($"\n{BOLD}{BLUE}{Name}{RESETFORMAT}{BOLD} has fallen in battle. {BOLD}{BLUE}{Name}{RESETFORMAT}{BOLD} your journey ends here.");
            Console.WriteLine($"But fear not, for a new adventure awaits. Rise again!{RESETFORMAT}");

            Console.WriteLine("\nPress 'R' to restart or 'E' to exit.");
            char choice = Console.ReadKey().KeyChar;

            while(choice != Convert.ToChar("R") || choice != Convert.ToChar("r") || choice != Convert.ToChar("E") || choice != Convert.ToChar("e"))
            {
                if (choice == 'R' || choice == 'r')
                {
                    //restart the game
                    CurrentGame.RestartGame(CurrentGame, Locations, Player, PlayerSkills);
                    CurrentGame.GameMenu(CurrentGame, Player, PlayerSkills, Locations);
                }
                else if (choice == 'E' || choice == 'e')
                {
                    //exit the game
                    Environment.Exit(0);
                }
            }
        }

        public void GiveExperience(Monster randomMonster)
        {
            double experienceToGive = randomMonster.Experience;

            //award experience and check for level up
            if (Experience + experienceToGive >= MaxExperience)
            {
                //calculate remaining experience after exceeding the limit
                double remainingExperience = Experience + experienceToGive - MaxExperience;

                //award experience to reach the limit
                SetExperience(MaxExperience);

                Console.WriteLine($"{BOLD}{BLUE}{Name}{RESETFORMAT}{BOLD} has gained {YELLOW}{Math.Round(experienceToGive - remainingExperience, 2)}{RESETFORMAT}{BOLD} experience by killing {randomMonster.Name}.{RESETFORMAT}");

                //level up
                SetLevel(++Level);
                Console.WriteLine($"\n{BOLD}{BLUE}{Name}{RESETFORMAT}{BOLD} has leveled up!");
                Console.WriteLine($"Advanced to level {YELLOW}{Level}{RESETFORMAT}{BOLD}.");
                Console.WriteLine($"\nPress any key to continue...{RESETFORMAT}");
                Console.ReadLine();

                //reset experience
                SetExperience(0);

                //calculate and set new experience limit (consider using double for MaxExperience)
                SetMaxExperience(MaxExperience * 1.15);

                //award remaining experience after level up
                GiveExperience(new Monster { Experience = remainingExperience, Name = randomMonster.Name });
            }
            else
            {
                //award experience without exceeding the limit
                SetExperience(Experience + experienceToGive);
                Console.WriteLine($"{BOLD}{BLUE}{Name}{RESETFORMAT}{BOLD} has gained {YELLOW}{Math.Round(experienceToGive, 2)}{RESETFORMAT}{BOLD} experience after killing {randomMonster.Name}.");
                Console.WriteLine($"\nPress any key to continue...{RESETFORMAT}");
                Console.ReadLine();
            }
        }

        public void HealPlayerCampfire(Character Player)
        {
            double remainingHealth = Player.MaxHealth - Player.CurrentHealth;
            double remainingMana = Player.MaxMana - Player.CurrentMana;

            if (Player.CurrentHealth < Player.MaxHealth || Player.CurrentMana < Player.MaxMana)
            {
                Console.Clear();
                Console.WriteLine($"{BOLD}The crackling flames of your makeshift campfire dance before you {BOLD}{BLUE}{Name}{RESETFORMAT}{BOLD}.");
                Console.WriteLine("Their warmth seeps into your bones, soothing your wounds and replenishing your mana.");
                Console.WriteLine($"Slowly, your health and mana begin to mend.{RESETFORMAT}");

                // Create a healer
                HealingService healer = new() { HealingAmount = 5 };

                while (Player.CurrentHealth < Player.MaxHealth || Player.CurrentMana < Player.MaxMana)
                {
                    // Heal health and mana simultaneously if both need healing
                    if (remainingHealth > 0 && remainingMana > 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"{BOLD}A warm, soothing sensation flows through your body as your injuries mend.");
                        Console.WriteLine("The arcane currents around you dance and swirl, nourishing your inner power.");
                        Console.WriteLine($"Health: {Math.Round(Player.CurrentHealth, 2)}/{Math.Round(Player.MaxHealth, 2)}");
                        Console.WriteLine($"Mana: {Math.Round(Player.CurrentMana, 2)}/{Math.Round(Player.MaxMana, 2)}{RESETFORMAT}");

                        healer.CampfireMendingHealth(Player);
                        healer.CampfireMendingMana(Player);
                        remainingHealth -= healer.HealingAmount;
                        remainingMana -= healer.HealingAmount;
                    }
                    // Heal health if it needs healing
                    else if (remainingHealth > 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"{BOLD}A warm, soothing sensation flows through your body as your injuries mend.");
                        Console.WriteLine($"Health: {Math.Round(Player.CurrentHealth, 2)}/{Math.Round(Player.MaxHealth, 2)}{RESETFORMAT}");

                        healer.CampfireMendingHealth(Player);
                        remainingHealth -= healer.HealingAmount;
                    }
                    // Heal mana if it needs healing
                    else if (remainingMana > 0)
                    {
                        Console.Clear();
                        Console.WriteLine($"{BOLD}The arcane currents around you dance and swirl, nourishing your inner power.");
                        Console.WriteLine($"Mana: {Math.Round(Player.CurrentMana, 2)}/{Math.Round(Player.MaxMana, 2)}{RESETFORMAT}");

                        healer.CampfireMendingMana(Player);
                        remainingMana -= healer.HealingAmount;
                    }

                    Thread.Sleep(1000);
                }

                EndHealingNotification(Player);
            }
            else
            {
                EndHealingNotification(Player);
            }
        }

        private void EndHealingNotification(Character Player)
        {
            Console.Clear();
            Console.WriteLine($"{BOLD}The campfire offers its warmth, but your health is already overflowing.");
            Console.WriteLine($"Health: {Math.Round(Player.CurrentHealth, 2)}/{Math.Round(Player.MaxHealth, 2)}");
            Console.WriteLine($"Mana: {Math.Round(Player.CurrentMana, 2)}/{Math.Round(Player.MaxMana, 2)}");

            Console.WriteLine("\nMaybe it's time for a different adventure?");
            Console.WriteLine($"Press Enter to continue...{RESETFORMAT}");
            Console.ReadLine();
        }
    }
}
