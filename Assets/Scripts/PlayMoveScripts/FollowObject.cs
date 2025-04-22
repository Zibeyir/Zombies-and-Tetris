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
        materialWeapon.material = GameDataService.Instance.WeaponMaterials[0];

    }
    private void Start()
    {
        targetParent = transform.parent;
        transform.parent = null;
    }
    public void SetTransparency(float level, float max)
    {
        // Sərhəd: 0 (tam şəffaf) ilə 1 (tam görünən) arasında xətti azalma
        float alpha = Mathf.Clamp01(level / max); // 0-10 arası səviyyəni 0.0 - 1.0 aralığına çevirir

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        Material mat = renderer.material;

        Color baseColor = mat.GetColor("_BaseColor");
        baseColor.a = alpha;
        mat.SetColor("_BaseColor", baseColor);
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
    public void TouchBallScale(float breakLevel, float max)
    {
        SetTransparency(breakLevel,max);
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