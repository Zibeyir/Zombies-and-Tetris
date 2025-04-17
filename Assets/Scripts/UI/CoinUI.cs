using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CoinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    float coinValue;
    public void UpdateCoins(int amount)
    {
        coinValue += amount;
        coinText.text = coinValue.ToString();
    }
}
