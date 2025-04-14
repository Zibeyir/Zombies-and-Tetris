using UnityEngine;

public class Fence : MonoBehaviour
{
    public int HP = 100;

    public void TakeDamage(int amount)
    {
        HP -= amount;
        GameEvents.OnFenceHPChanged?.Invoke(HP);
        if (HP <= 0)
        {
            GameEvents.OnGameLost?.Invoke();
            LevelManager.Instance.GameOver();
        }
    }

}
