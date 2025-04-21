using UnityEngine;

public class Fence : MonoBehaviour
{
    public float HP = 100;
    public float HPMax = 100;

    public void GetHP(int hp)
    {
        HPMax = hp;
        HP = hp;
        Debug.Log(hp + " HP");
    }
    public void TakeDamage(int amount)
    {
        
        if (HP <= 0)
        {
            GameEvents.OnGameLost?.Invoke();
            LevelManager.Instance.GameOver();
        }
        else
        {
            HP -= amount;
            //Debug.Log(HP / HPMax);
            GameEvents.OnFenceHPChanged?.Invoke(HP / HPMax);
        }
    }

}
