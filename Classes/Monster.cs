using System.Numerics;
using System.Timers;

namespace RPG_Game
{
    internal class Monster
    {
        public string Name { get; set; }
        public double MaxHealth { get; set; }
        public double CurrentHealth { get; set; }
        public int Damage { get; set; }
        public double Experience { get; set; }

        public void SetName(string name) { Name = name; }
        public void SetMaxHealth(double health) { MaxHealth = health; }
        public void SetCurrentHealth(double health) { CurrentHealth = health; }
        public void SetDamage(int damage) { Damage = damage; }
        public void SetExperience(int experience) { Experience = experience; }

        public double MonsterAttack()
        {
            Random rnd = new();
            return rnd.Next(1, Damage);
        }

        //respawns monsters on a timer
        public void RespawnMonsters(Location location)
        {
            if (location.RespawningMonsters == true)
            {
                System.Timers.Timer timer;
                timer = new System.Timers.Timer();
                timer.Elapsed += OnTimerElapsed;
                timer.Interval = location.LocationRespawnTimer;
                timer.Enabled = true;

                void OnTimerElapsed(object sender, ElapsedEventArgs e)
                {
                    timer.Stop();
                    timer.Elapsed -= OnTimerElapsed;
                    timer = null;

                    //check if CurrentMonsters is empty
                    if (location.CurrentMonsters.Count == 0)
                    {
                        //respawn all monsters by copying DefaultMonsters
                        location.CurrentMonsters.AddRange(location.DefaultMonsters.Select(m => new Monster()
                        {
                            Name = m.Name,
                            MaxHealth = m.MaxHealth,
                            CurrentHealth = m.CurrentHealth,
                            Damage = m.Damage,
                            Experience = m.Experience,
                        }));
                    }

                    location.SetRespawningMonsters(false);
                }
            }
        }
    }
}
