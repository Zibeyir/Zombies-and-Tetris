using System;

public static class GameEvents
{
    public static Action<float,int> OnWaveStarted;
    public static Action<float> OnWaveCompleted;
    public static Action<float> OnFenceHPChanged;
    public static Action<int> OnCoinChanged;
    public static Action OnGameWon;
    public static Action OnGameLost;
    public static Action ActiveBlockButton;

    //public static Action UIFader;
}
