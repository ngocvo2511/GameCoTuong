using System.Windows.Media;
using System.Windows.Media.Imaging;
using ChessLogic;

namespace ChessUI
{
    public class Images
    {
        private static readonly Dictionary<PieceType, ImageSource> redSources = new()
        {
            {PieceType.Cannon, LoadImage("Assets/Images/PhaoDo.png") },
            {PieceType.Chariot, LoadImage("Assets/Images/XeDo.png") },
            {PieceType.General, LoadImage("Assets/Images/TuongDo.png") },
            {PieceType.Advisor, LoadImage("Assets/Images/SiDo.png") },
            {PieceType.Elephant, LoadImage("Assets/Images/TinhDo.png") },
            {PieceType.Soldier, LoadImage("Assets/Images/TotDo.png") },
            {PieceType.Horse, LoadImage("Assets/Images/MaDo.png") },
        };

        private static readonly Dictionary<PieceType, ImageSource> blackSources = new()
        {
            {PieceType.Cannon, LoadImage("Assets/Images/PhaoXanh.png") },
            {PieceType.Chariot, LoadImage("Assets/Images/XeXanh.png") },
            {PieceType.General, LoadImage("Assets/Images/TuongXanh.png") },
            {PieceType.Advisor, LoadImage("Assets/Images/SiXanh.png") },
            {PieceType.Elephant, LoadImage("Assets/Images/TinhXanh.png") },
            {PieceType.Soldier, LoadImage("Assets/Images/TotXanh.png") },
            {PieceType.Horse, LoadImage("Assets/Images/MaXanh.png") },
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
