using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Fence : MonoBehaviour
{
    public static float HP = 100;
    public static float HPMax = 100;

    [Header("Child Settings")]
    public static List<MeshRenderer> childRenderers = new List<MeshRenderer>();
    public static int childrenCount;
    public static float healthPerChild;
    public static float previousHealth;
    public static int previousChildIndex;
    public static int currentChildIndex;
    public  List<MeshRenderer> childRenderersP = new List<MeshRenderer>();
    private void Start()
    {
        InitializeChildren();
    }

    private void InitializeChildren()
    {
        childRenderers.Clear();
        foreach (Transform child in transform)
        {
            MeshRenderer rend = child.GetComponent<MeshRenderer>();
            if (rend != null)
            {
                childRenderers.Add(rend);
            }
        }
        childRenderersP = childRenderers;
        childrenCount = childRenderers.Count;
        if (childrenCount == 0)
        {
            Debug.LogWarning("No child renderers found!");
        }
        else
        {
            healthPerChild = HPMax / childrenCount;
        }
    }
    public void GetHP(int hp)
    {
        HPMax = hp;
        HP = hp;
        Debug.Log(hp + " HP");
    }
    public void TakeDamage(int amount)
    {
        float oldHP = HP; // Əvvəlki HP-ni saxla

        HP = Mathf.Clamp(HP - amount, 0, HPMax); // HP dəyişir (azalırsa +, artırsa - olacaq)

        GameEvents.OnFenceHPChanged?.Invoke(HP / HPMax);

        previousChildIndex = Mathf.FloorToInt(oldHP / healthPerChild);
        currentChildIndex = Mathf.FloorToInt(HP / healthPerChild);

        if (currentChildIndex < previousChildIndex) // HP azalıbsa -> FadeOut
        {
            for (int i = previousChildIndex - 1; i >= currentChildIndex; i--)
            {
                if (i >= 0 && i < childRenderersP.Count)
                {
                    FadeOutChild(childRenderersP[i]);
                }
            }
        }
        else if (currentChildIndex > previousChildIndex) // HP artıbsa -> FadeIn
        {
            for (int i = previousChildIndex; i < currentChildIndex; i++)
            {
                if (i >= 0 && i < childRenderersP.Count)
                {
                    FadeInChild(childRenderersP[i]);
                }
            }
        }

        if (HP <= 0)
        {
            GameEvents.OnGameLost?.Invoke();
            LevelManager.Instance.GameOver();
        }
    }


    public void FadeOutChild(Renderer rend)
    {
        Debug.Log("FadeOutChild");
        if (rend == null) return;

        Material mat = rend.materials[0];
        Color originalColor = mat.GetColor("_BaseColor");  // BURDA BaseColor istifadə et
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        Sequence seq = DOTween.Sequence();
        seq.Append(mat.DOColor(transparentColor, "_BaseColor", .2f)) // Sönür
           .Append(mat.DOColor(originalColor, "_BaseColor", .2f))    // Yenidən yanır
           .Append(mat.DOColor(transparentColor, "_BaseColor", .2f)) // Sönür
           .Append(mat.DOColor(originalColor, "_BaseColor", .2f))    // Yenidən yanır
           .Append(mat.DOColor(transparentColor, "_BaseColor", .2f)) // Tamamilə sönür
           .OnComplete(() => rend.enabled = false); // düz
    }

    public void FadeInChild(Renderer rend)
    {
        Debug.Log("FadeInChild");
        if (rend == null) return;

        rend.enabled = true; // Önəmli: əvvəldən görünməzdirsə, açmaq lazımdır

        Material mat = rend.materials[0];
        Color originalColor = mat.GetColor("_BaseColor");
        Color transparentColor = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        // Əvvəlcə tam şəffaf olsun
        mat.SetColor("_BaseColor", transparentColor);

        // Sonra görünməyə başlasın
        mat.DOColor(originalColor, "_BaseColor", 0.5f);
    }
}
