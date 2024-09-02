Developed to revisit C#

### Character:
Attributes:
- Name
- Level
- MaxExperience - Experience cap for current level.
- Experience - Current experience.
- MaxHealth - Health cap.
- CurrentHealth
- MaxMana - Mana Cap.
- CurrentMana

Functionality:
- Generate Character with default attributes.
- Add Skill.
- Display Character attributes.
- Display Character skills.
- Melee attack.
- Magic attack - stronger than melee however, it is mana based.
- Attack skill with trigger chance based on the attack level. Essentially, character can miss the attack.
- Critical Hit skill with trigger chance based on the critical hit level.
- Defense skill with trigger chance based on the defense level. Allowing the character to block attacks.
- Character Death.
- Give Experience to character. Allowing to level up.
- Heal health and mana next to campfire.

### Skill:
Attributes:
- Name
- Description - Explains the purpose.
- Level
- MaxLevel - Level cap.
- Experience - Current experience.
- MaxExperience - Experience cap for current level.
- TriggerChance - Chance to trigger the effect.
- ScalingFactor - Experience scaling factor.

Functionality:
- Give experience to any of the skills.

### List Of Character Skills:
Attributes:
- Damage - Determines how hard you can hit.
- Attack - Improves your chance of hitting a target.
- Defense - Increases chance to block enemy attacks.
- Critical Hit - Increases chance to deal additional damage.

### Monster:
Attributes:
- Name
- MaxHealth - Monsters maximum health.
- CurrentHealth - Current health.
- Damage - Determines how hard a monster can hit.
- Experience - Amount of Experience that is given to the player after monster is defeated.

Functionality:
- Monster attack.
- Respawn monsters in location.

### Location:
Attributes:
- Name
- RespawningMonsters - Flag for monster respawn.
- DefaultLocationRespawnTimer - Default respawn time for monsters in the location.
- CurrentLocationRespawnTimer - Current respawn time for monsters in the location.
- DefaultMonsters - Default list of monsters that are used to respawn current monsters.
- CurrentMonsters - Monsters that are currently present/alive in the location.

Functionality:
- Display all Locations.
- Display Location details.

### List of Locations with monsters:
Whispering Woods
- Wolf (HP: 75/75, Damage: 7, Experience: 30)
- Goblin (HP: 50/50, Damage: 5, Experience: 20)
- Giant Spider (HP: 60/60, Damage: 6, Experience: 25)

Forgotten Ruins
- Skeleton (HP: 80/80, Damage: 8, Experience: 35)
- Mummy (HP: 100/100, Damage: 9, Experience: 40)
- Undead Warrior (HP: 120/120, Damage: 10, Experience: 45)

Shadowed Caverns
- Troll (HP: 150/150, Damage: 12, Experience: 60)
- Orc (HP: 130/130, Damage: 11, Experience: 55)
- Giant Bat (HP: 90/90, Damage: 8, Experience: 35)

Dragon's Peak
- Fire Drake (HP: 200/200, Damage: 15, Experience: 100)
- Mountain Giant (HP: 250/250, Damage: 20, Experience: 150)
- Griffin (HP: 180/180, Damage: , Experience: )

Infernal Cavern
- Fire Demon (HP: 250/250, Damage: 20, Experience: 150)
- Lava Golem (HP: 200/200, Damage: 15, Experience: 120)
- Hell Hound (HP: 100/100, Damage: 8, Experience: 50)

Frozen Wasteland
- Ice Titan (HP: 400/400, Damage: 30, Experience: 300)
- Frost Wyrm Alpha (HP: 250/250, Damage: 20, Experience: 150)
- Blizzard Storm Elemental (HP: 150/150, Damage: 15, Experience: 100)
- Frost Dragon (HP: 500/500, Damage: 40, Experience: 400)

### Healing Service:
Attributes:
- HealingAmount - Determines the amount of health given to the player

Functionality:
- CampfireMendingHealth - restores health by HealingAmount
- CampfireMendingMana - restores mana by HealingAmount

### Fight Service:
Functionality:
- Displays Battle Stats (Monsters attributes along with Player attributes and skills).
- Handles Players Damage (Player may miss or not, if not then there is a chance for a crtical hit).
- Handles Monster Damage (Monsters attack may be blocked or not).
- Gives experience to skills and character.

### Game:
Functionality:
- Generate Location with Monsters.
- Display Game Menu.
- Start Game.
- Restart Game.