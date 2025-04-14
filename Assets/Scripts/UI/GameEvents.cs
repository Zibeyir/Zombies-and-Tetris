using System;

public static class GameEvents
{
    public static Action<int> OnWaveStarted;
    public static Action<int> OnWaveCompleted;
    public static Action<int> OnFenceHPChanged;
    public static Action OnGameWon;
    public static Action OnGameLost;
}
