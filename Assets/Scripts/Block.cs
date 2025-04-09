using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class Block : MonoBehaviour
{
    [Header("Block Settings")]
    [SerializeField] private int coinValue = 10;

    [Header("Visual Effects")]
    [SerializeField] private GameObject coinTextPrefab;
    [SerializeField] private GameObject hitEffectPrefab;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material hitMaterial;

    private Vector3 startPosition;
    private float hoverOffset;
    private Renderer blockRenderer;
    private Rigidbody rb;

    private void Start()
    {
        startPosition = transform.position;
        blockRenderer = GetComponent<Renderer>();
        rb = GetComponent<Rigidbody>();

        // Configure Rigidbody for arcade-style physics
        if (rb != null)
        {
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezePositionZ |
                           RigidbodyConstraints.FreezeRotationX |
                           RigidbodyConstraints.FreezeRotationY;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
        }

        // Add slight random rotation
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-10f, 10f));
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            GameManager.Instance.AddCoins(coinValue);

            StartCoroutine(ShowHitEffect());

            if (coinTextPrefab != null)
            {
                GameObject floatingText = Instantiate(coinTextPrefab,
                    transform.position + Vector3.up,
                    Quaternion.identity);
                Text textComponent = floatingText.GetComponent<Text>();
                if (textComponent != null)
                {
                    textComponent.text = $"+{coinValue}";
                }
                Destroy(floatingText, 1f);
            }

            if (hitEffectPrefab != null)
            {
                Instantiate(hitEffectPrefab, collision.contacts[0].point, Quaternion.identity);
            }
        }
    }

    private System.Collections.IEnumerator ShowHitEffect()
    {
        if (blockRenderer != null && hitMaterial != null)
        {
            Material originalMaterial = blockRenderer.material;
            blockRenderer.material = hitMaterial;
            yield return new WaitForSeconds(0.1f);
            blockRenderer.material = originalMaterial;
        }
    }
}