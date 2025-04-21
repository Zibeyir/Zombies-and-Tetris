using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI cyrstalText;

    public int coinValue;
    public int cyrstalValue;
    private void Start()
    {
        coinValue = LevelManager.Instance._SaveData.Coins;
        coinText.text = coinValue.ToString();

        Debug.Log("Coin Value: " + coinValue);
    }
    public void UpdateCoins(int amount)
    {
        coinValue = (LevelManager.Instance._SaveData.Coins += amount);
        coinText.text = coinValue.ToString();
        UIManager.Instance.ActivateButtonForSpawnBlocks(coinValue);

    }
    public void UpdateCrystals(int amount)
    {
        cyrstalValue = (LevelManager.Instance._SaveData.Cristal += amount);
        cyrstalText.text = cyrstalValue.ToString();
        UIManager.Instance.ActivateButtonForSpawnBlocks(cyrstalValue);

    }
}
