using DG.Tweening;
using UnityEngine;
using DG.Tweening;

public class FollowObject : MonoBehaviour
{
    private Transform targetParent; 
    public float followSpeedZ = 20f;
    private Tween hitTween;

    private void Start()
    {
        targetParent = transform.parent;
        transform.parent = null;
    }

    public void TouchBallScale()
    {
        if (hitTween == null || !hitTween.IsActive() || !hitTween.IsPlaying())
        {
            
            hitTween = transform.DOScale(Vector3.one * 1.1f, 0.1f)
            .SetLoops(2, LoopType.Yoyo)
            .SetEase(Ease.OutBack)
            .OnComplete(() => transform.localScale = Vector3.one * 1);
        }
    }
    private void Update()
    {
        if (targetParent == null) return;

        transform.position = Vector3.Lerp(transform.position, targetParent.position, followSpeedZ * Time.unscaledDeltaTime);

    }
}
