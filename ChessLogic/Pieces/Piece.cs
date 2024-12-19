using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public abstract class Piece
    {
        public Player bottomPlayer;
        public abstract PieceType Type { get; }
        public abstract Player Color { get; }
        public bool HasMoved { get; set; } = false;
        public abstract Piece Copy();

        public abstract IEnumerable<Move> GetMoves(Position from, Board board);

        public virtual bool CanCaptureOpponentGeneral(Position from, Board board)
        {
            return GetMoves(from, board).Any(move =>
            {
                Piece piece = board[move.ToPos];
                return piece != null && piece.Type == PieceType.General;
            });
        }
        public override string ToString()
        {
            switch (Type)
            {
                case PieceType.General: return (Color == Player.Black) ? "bG" : "rG";
                case PieceType.Advisor: return (Color == Player.Black) ? "bA" : "rA";
                case PieceType.Chariot: return (Color == Player.Black) ? "bCh" : "rCh";
                case PieceType.Cannon: return (Color == Player.Black) ? "bC" : "rC";
                case PieceType.Elephant: return (Color == Player.Black) ? "bE" : "rE";
                case PieceType.Horse: return (Color == Player.Black) ? "bH" : "rH";
                default: return (Color == Player.Black) ? "bS" : "rS";
            }
        }
    }
}
