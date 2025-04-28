using UnityEngine;
using DG.Tweening;

public class CoinMovement : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float riseHeight = 3f;      // Yuxarı qalxma hündürlüyü
    public float sidewaysDistance = 1f; // Sağa/sola azca sürüşmə
    public float riseDuration = 0.5f;  // Yuxarı qalxma müddəti
    public float fallDuration = 0.4f;  // Aşağı düşmə müddəti
    public float flyDuration = 0.8f;   // UI-a uçuş müddəti
    public float fadeDelay = 0.3f;     // Fade-in gecikmə

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void FlyToUI(RectTransform uiTarget, Vector3 startPosition)
    {
        transform.position = startPosition;
        gameObject.SetActive(true);
        transform.rotation = Quaternion.Euler(-43, 0, 0);

        // Rəngi tam görünən et
        var color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;

        // 1) Yuxarı qalxır
        Vector3 riseTarget = startPosition + new Vector3(0, riseHeight, 0);
        transform.DOMove(riseTarget, riseDuration).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // 2) Sağa ya sola azca sürüşərək aşağı düşür
                float randomX = Random.Range(-1f, 1f) * sidewaysDistance;
                Vector3 fallTarget = riseTarget + new Vector3(randomX, -riseHeight * 0.5f, 0);

                transform.DOMove(fallTarget, fallDuration).SetEase(Ease.InQuad)
                    .OnComplete(() =>
                    {
                        // 3) Sonra UI target-a uçur
                        Vector3 uiWorldPosition = Camera.main.ScreenToWorldPoint(uiTarget.position);
                        uiWorldPosition.z = 0; // 2D üçün

                        transform.DOMove(uiWorldPosition, flyDuration).SetEase(Ease.InQuad);

                        // 4) Fade-i bir az gecikdiririk
                        spriteRenderer.DOFade(0f, flyDuration)
                            .SetDelay(fadeDelay)
                            .OnComplete(() =>
                            {
                                gameObject.SetActive(false);
                            });
                    });
            });
    }
}
