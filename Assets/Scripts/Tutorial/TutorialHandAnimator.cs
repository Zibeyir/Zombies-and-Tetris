using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TutorialHandAnimator : MonoBehaviour
{
    public RectTransform handImage;
    public float moveDistance = 100f;
    public float duration = 1f;

    public Canvas canvas;
    private Tweener currentTween;

    void Awake()
    {
        canvas = handImage.GetComponentInParent<Canvas>();
        handImage.gameObject.SetActive(false); // başlanğıcda gizli olsun
    }

    /// <summary>
    /// El animasiyasını göstərmək və obyektə yerləşdirmək
    /// </summary>
    public void ShowHand(Transform target3DObject)
    {
        Debug.Log("Null target3DObject");

        if (target3DObject == null)
        {
            HideHand();
            return;
        }
        Debug.Log("ShowHand");
        Vector3 screenPos = Camera.main.WorldToScreenPoint(target3DObject.position);

        Vector2 uiPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main,
            out uiPos
        );

        handImage.anchoredPosition = uiPos;
        handImage.gameObject.SetActive(true);

        // Əvvəlki animasiyanı sil
        currentTween?.Kill();

        // Yeni animasiyanı başlat
        Vector2 endPos = uiPos + new Vector2(0, moveDistance+100);
        currentTween = handImage.DOAnchorPos(endPos, duration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    /// <summary>
    /// El animasiyasını gizlətmək
    /// </summary>
    public void HideHand()
    {
        currentTween?.Kill();
        currentTween = null;
        handImage.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
