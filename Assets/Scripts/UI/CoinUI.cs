using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    public int coinValue;
    public void UpdateCoins(int amount)
    {
        coinValue = (LevelManager.Instance._SaveData.Coins += amount);
        coinText.text = coinValue.ToString();
        UIManager.Instance.ActivateButtonForSpawnBlocks(coinValue);

    }

}
