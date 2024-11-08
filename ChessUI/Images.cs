using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessLogic;

namespace ChessUI
{
    public class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> redSources = new()
        {
            {PieceType.Cannon, LoadImage("Assets/PhaoDo.png") },
            {PieceType.Chariot, LoadImage("Assets/XeDo.png") },
            {PieceType.General, LoadImage("Assets/TuongDo.png") },
            {PieceType.Advisor, LoadImage("Assets/SiDo.png") },
            {PieceType.Elephant, LoadImage("Assets/TinhDo.png") },
            {PieceType.Soldier, LoadImage("Assets/TotDo.png") },
            {PieceType.Horse, LoadImage("Assets/MaDo.png") },
        };

        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            {PieceType.Cannon, LoadImage("Assets/PhaoXanh.png") },
            {PieceType.Chariot, LoadImage("Assets/XeXanh.png") },
            {PieceType.General, LoadImage("Assets/TuongXanh.png") },
            {PieceType.Advisor, LoadImage("Assets/SiXanh.png") },
            {PieceType.Elephant, LoadImage("Assets/TinhXanh.png") },
            {PieceType.Soldier, LoadImage("Assets/TotXanh.png") },
            {PieceType.Horse, LoadImage("Assets/MaXanh.png") },
        };
        private static ImageSource LoadImage(string filePath)
        {
            return new BitmapImage(new Uri(filePath, UriKind.Relative));
        }
        public static ImageSource GetImage(Player color, PieceType type)
        {
            return color switch
            {
                Player.Red => redSources[type],
                Player.Black => blackSources[type],
                _ => null
            };
        }

        public static ImageSource GetImage(Piece piece)
        {
            if(piece == null) return null;
            return GetImage(piece.Color, piece.Type);
        }
    }
}
