namespace RPG_Game
{
    internal class Skill
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Level { get; set; }
        public int MaxLevel {  get; set; }
        public double Experience { get; set; }
        public double MaxExperience { get; set; }
        public double TriggerChance { get; set; }
        public double ScalingFactor { get; set; }

        string BOLD = Console.IsOutputRedirected ? "" : "\x1B[1m";
        string RESETFORMAT = Console.IsOutputRedirected ? "" : "\x1B[0m";
        string YELLOW = Console.IsOutputRedirected ? "" : "\x1B[33m";

        public void SetName(string name) { Name = name; }
        public void SetDescription(string description) { Description = description; }
        public void SetLevel(int level) { Level = level; }
        public void SetMaxLevel(int maxLevel) { MaxLevel = maxLevel; }
        public void SetExperience(double experience) { Experience = experience; }
        public void SetMaxExperience(double maxExperience) { MaxExperience = maxExperience; }
        public void SetTriggerChance(double triggerChance) { TriggerChance = triggerChance; }
        public void SetScalingFactor(double scalingFactor) { ScalingFactor = scalingFactor; }

        public void GiveExperience(Skill usedSkill, double experience)
        {
            double experienceToGive = experience;

            //given + current experience exceeds the limit
            if (Experience + experienceToGive >= MaxExperience)
            {
                //experience required to level up
                double remindingExperience = MaxExperience - Experience;

                Console.WriteLine($"{BOLD}{usedSkill.Name} has gained {YELLOW}{Math.Round(remindingExperience, 2)}{RESETFORMAT}{BOLD} experience.");

                //level up
                SetLevel(++Level);
                Console.WriteLine("\nYour skill has leveled up!");
                Console.WriteLine($"{BOLD}{usedSkill.Name} advanced to level {YELLOW}{Level}{RESETFORMAT}{BOLD}.");
                Console.WriteLine($"\nPress Enter to continue...{RESETFORMAT}");
                Console.ReadLine();

                //reset experience
                SetExperience(0);

                //calculate and set new experience limit (consider using double for MaxExperience)
                SetMaxExperience(MaxExperience * 1.15);

                //increase the trigger chance
                SetTriggerChance(TriggerChance + (TriggerChance * ScalingFactor / 4));

                double experienceToGiveAfterLevelUp = experienceToGive - remindingExperience;

                if (experienceToGiveAfterLevelUp > 0)
                {
                    //award remaining experience after level up
                    GiveExperience(new Skill { Name = usedSkill.Name }, experienceToGiveAfterLevelUp);
                }
            }
            else
            {
                //award experience without exceeding the limit
                SetExperience(Experience + experienceToGive);
                Console.WriteLine($"{BOLD}{usedSkill.Name} has gained {YELLOW}{Math.Round(experienceToGive, 2)}{RESETFORMAT}{BOLD} experience. {RESETFORMAT}");
            }
        }
    }
}
