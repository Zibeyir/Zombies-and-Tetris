using DG.Tweening;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private float followSpeedZ = 20f;
    [SerializeField] private float touchScaleFactor = 1.1f;
    [SerializeField] private float mergeScaleFactor = 1.2f;
    [SerializeField] private float touchScaleDuration = 0.1f;
    [SerializeField] private float mergeScaleDuration = 0.2f;
    [SerializeField] private ParticleSystem mergeParticleSystem;

    private Transform targetParent;
    private Tween hitTween;
    private MeshRenderer materialWeapon;
    private void OnEnable()
    {
        materialWeapon = GetComponent<MeshRenderer>();

    }
    private void Start()
    {
        targetParent = transform.parent;
        transform.parent = null;
        materialWeapon.material = GameDataService.Instance.WeaponMaterials[0];
    }

    public void GetMaterialWeapon(int index)
    {
        if (index < GameDataService.Instance.WeaponMaterials.Count)
        {
            materialWeapon.material = GameDataService.Instance.WeaponMaterials[index];
        }
        else
        {
            Debug.Log("Index out of range for WeaponMaterials.");
        }
    }
    public void TouchBallScale()
    {
        PlayScaleTween(touchScaleFactor, touchScaleDuration);
    }

    public void MergeBallScale()
    {
        PlayScaleTween(mergeScaleFactor, mergeScaleDuration);
        if(!mergeParticleSystem.isPlaying)
        mergeParticleSystem.gameObject.SetActive(true);
    }

    private void PlayScaleTween(float scaleFactor, float duration)
    {
        if (hitTween == null || !hitTween.IsActive() || !hitTween.IsPlaying())
        {
            hitTween = transform.DOScale(Vector3.one * scaleFactor, duration)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutBack);
        }
    }

    private void Update()
    {
        if (targetParent == null)
        {
            Debug.LogWarning("Target parent is null. Object will stop following.");
            return;
        }

        transform.position = Vector3.Lerp(transform.position, targetParent.position, followSpeedZ * Time.unscaledDeltaTime);
    }
}