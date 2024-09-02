namespace RPG_Game
{
    internal class HealingService
    {
        public double HealingAmount { get; set; }

        public void CampfireMendingHealth(Character player)
        {
            if (player.CurrentHealth < player.MaxHealth)
            {
                player.CurrentHealth += HealingAmount;
                if (player.CurrentHealth > player.MaxHealth)
                {
                    player.CurrentHealth = player.MaxHealth;
                }
            }
        }

        public void CampfireMendingMana(Character player)
        {
            if (player.CurrentMana < player.MaxMana)
            {
                player.CurrentMana += HealingAmount;
                if (player.CurrentMana > player.MaxMana)
                {
                    player.CurrentMana = player.MaxMana;
                }
            }
        }
    }
}
