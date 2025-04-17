using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    public void ShowWin()
    {
        winScreen.SetActive(true);
    }

    public void ShowLose()
    {
        loseScreen.SetActive(true);
    }
}
