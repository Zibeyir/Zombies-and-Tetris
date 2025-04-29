using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Collections.Generic;

public class UIFader : MonoBehaviour
{
    private List<Graphic> graphics = new List<Graphic>();
    private Button button;
    private void Start()
    {
        button =GetComponent<Button>();
        graphics.AddRange(GetComponentsInChildren<Image>(includeInactive: true));
        graphics.AddRange(GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true));
        FadeOutAll(.1f);

    }
    public void FadeINOutAll(bool boolButton, float duration = 0.5f)
    {
        if (boolButton) FadeInAll();
        else FadeOutAll();
    }
    public void FadeOutAll(float duration = 0.5f)
    {
        foreach (var g in graphics)
        {
            g.DOFade(0f, duration);
        }
        button.interactable = false;
    }

    public void FadeInAll(float duration = 0.5f)
    {
        foreach (var g in graphics)
        {
            g.DOFade(1f, duration);
        }
        button.interactable = true;

    }
}
