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
        // Bütün Image və TextMeshProUGUI komponentlərini topla
        graphics.AddRange(GetComponentsInChildren<Image>(includeInactive: true));
        graphics.AddRange(GetComponentsInChildren<TextMeshProUGUI>(includeInactive: true));
    }
    public void FadeINOutAll(bool boolButton, float duration = 0.5f)
    {
        foreach (var g in graphics)
        {
            g.DOFade(0f, duration);
        }
        button.interactable = false;
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
