namespace ChessLogic
{
    public enum Player
    {
        None,
        Red,
        Black
    }
    public static class PlayerExtensions
    {
        public static Player Opponent(this Player player)
        {
            return player switch
            {
                Player.Black => Player.Red,
                Player.Red => Player.Black,
                _ => Player.None,
            };
        }
    }
}
