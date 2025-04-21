using Unity.VisualScripting;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    public void ShowWin()
    {
        winScreen.SetActive(true);
    }

    public void NextLevel() {
        winScreen.SetActive(false);
        LevelManager.Instance.NextLevel();
    }
    public void LoseLevel()
    {
        loseScreen.SetActive(false);
        LevelManager.Instance.NextLevel();
    }
    public void ShowLose()
    {
        loseScreen.SetActive(true);
    }
}
