namespace ChessLogic
{
    public class Counting
    {
        private readonly Dictionary<PieceType, int> redCount = new();
        private readonly Dictionary<PieceType, int> blackCount = new();

        public int TotalCount { get; private set; }

        public Counting()
        {
            foreach (PieceType pieceType in Enum.GetValues(typeof(PieceType)))
            {
                redCount[pieceType] = 0;
                blackCount[pieceType] = 0;
            }
        }

        public void Increment(Player color, PieceType type)
        {
            if (color == Player.Red)
            {
                redCount[type]++;
            }
            else if(color == Player.Black)
            {
                blackCount[type]++;
            }

            TotalCount++;
        }

        public int Red(PieceType type)
        {
            return redCount[type];
        }

        public int Black(PieceType type)
        {
            return blackCount[type];
        }

        public int CountingAttackPieces(Player color)
        {
            if (color == Player.Red)
            {
                return redCount[PieceType.Chariot] + redCount[PieceType.Cannon] + redCount[PieceType.Horse] + redCount[PieceType.Soldier];
            }
            else if(color == Player.Black)
            {
                return blackCount[PieceType.Chariot] + blackCount[PieceType.Cannon] + blackCount[PieceType.Horse] + blackCount[PieceType.Soldier];
            }
            else
            {
                return 0;
            }
        }

        
    }
}
