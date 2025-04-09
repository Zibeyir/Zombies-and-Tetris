using UnityEngine;
using UnityEngine.UI;

public class BlockTypeButton : MonoBehaviour
{
    public BlockType blockType;
    [SerializeField] private Image blockIcon;
    [SerializeField] private Text coinValueText;

    private void Start()
    {
        if (blockType != null)
        {
            if (blockIcon != null && blockType.uiIcon != null)
            {
                blockIcon.sprite = blockType.uiIcon;
            }

            if (coinValueText != null)
            {
                coinValueText.text = blockType.coinValue.ToString();
            }
        }
    }
}