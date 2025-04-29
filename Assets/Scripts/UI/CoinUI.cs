using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI cyrstalText;

    public static float counPercent;
    public float coinValue;
    public int cyrstalValue;
    private void Start()
    {
        coinValue = 90;
        coinText.text = coinValue.ToString();
        cyrstalValue = LevelManager.Instance._SaveData.Cristal;
        cyrstalText.text = cyrstalValue.ToString();
    }
    public void UpdateCoins(int amount)
    {
        float bonus = amount * (counPercent / 100f);
        float totalAmount = amount + bonus;

        // coinValue-ya əlavə edirik
        coinValue += totalAmount;

        // Text-i int kimi yuvarlaq yazırıq
        coinText.text = Mathf.RoundToInt(coinValue).ToString();

        UIManager.Instance.ActivateButtonForSpawnBlocks(Mathf.RoundToInt(coinValue));
    }
    public void UpdateCrystals(int amount)
    {
        cyrstalValue = (LevelManager.Instance._SaveData.Cristal += amount);
        cyrstalText.text = cyrstalValue.ToString();
        //UIManager.Instance.ActivateButtonForSpawnBlocks(cyrstalValue);

    }
}
   

