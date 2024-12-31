using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessLogic
{
    public enum EndReason
    {
        Checkmate,
        Stalemate, // van la thua chu khong hoa giong nhu co vua
        InsufficientMaterial,
        ThreefoldRepetition,
        FiftyMoveRule,
        DrawAgreed,
        Resignation,
        TimeForfeit,
        Abandoned,
        IllegalMove,
        PlayerDisconnected,
        Unknown
    }
}
