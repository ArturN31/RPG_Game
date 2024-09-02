using RPG_Game;

Console.WindowHeight = 60;
Console.WindowWidth = 120;

RPG_Game.Game CurrentGame = new();
List<Location> Locations = [];
Character Player = new();
List<Skill> PlayerSkills = [];

CurrentGame.StartGame(CurrentGame, Locations, Player, PlayerSkills);

CurrentGame.GameMenu(CurrentGame, Player, PlayerSkills, Locations);