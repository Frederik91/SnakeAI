namespace GameLogic
{
    public class Settings
    {
        public bool DontSleep { get; set; }

        // dimensions of the circles.
        public int Width { get; set; } = 12;

        public int Height { get; set; } = 12;
        // how fast the player is moving.
        public int Speed { get; set; } = 10;
        // total score of the current game.
        public int Score { get; set; }
        // how much points is to be added for food eaten.
        public int Points { get; } = 10;
        // determines if game will end.
        public bool GameOver { get; set; }
        // number of seconds, how long the game lasted.
        public int Duration { get; set; }

        // directions for snake movement.

        public Settings()
        {
            GameOver = false;
        }
    }
}
