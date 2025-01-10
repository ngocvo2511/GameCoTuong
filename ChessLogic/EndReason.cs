namespace ChessLogic
{
    public enum EndReason
    {
        Checkmate,
        Stalemate,
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
