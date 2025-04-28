using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIPrefabController : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI upgradeText;
    public TextMeshProUGUI upgradeBuyText;
    public Image upgradeImage;

    public GameObject gameObject;
    public GameObject BuyPart;

    public Button upgradeButton;

    // This will be called to set the upgrade description and button functionality
    public void SetUpgradeData(string description, int crystalCost, UnityEngine.Events.UnityAction onClickAction,Sprite sprite)
    {
        if (upgradeText != null)
        {
            upgradeText.text = description;
            upgradeBuyText.text = crystalCost.ToString();
            upgradeImage.sprite = sprite;
        }
        else
            Debug.LogError("Text component is missing!");

        if (upgradeButton != null)
            upgradeButton.onClick.AddListener(onClickAction);
        else
            Debug.LogError("Button component is missing!");
    }
    public void StartNextWave()
    {
        WaveManager.Instance.StartWavesAfterUpgrade();
        gameObject.SetActive(false);
    }
}
