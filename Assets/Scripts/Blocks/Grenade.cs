using UnityEngine;
using System.Collections;

public class Grenade : BulletBase
{
    public float ExplosionRadius = 3f;
    public float ArcHeight = 5f;
    public float MaxDistance = 5f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float travelTime = 1f;
    private float timer;
    private Coroutine travelRoutine;
    private bool exploded = false;
    [SerializeField] protected TrailRenderer trailRenderer;
    [SerializeField] private GameObject explosionEffect;

    protected override void OnEnable()
    {
        GameObject closestZombie = FindClosestZombie();
        if(closestZombie == null)
        {
            gameObject.SetActive(false);
            return;
        }
        trailRenderer?.Clear();
        trailRenderer.enabled = true;
        base.OnEnable();
        exploded = false;

        if (closestZombie != null)
        {
            startPos = transform.position;
            targetPos = closestZombie.transform.position;
            timer = 0f;

            if (travelRoutine != null)
                StopCoroutine(travelRoutine);

            travelRoutine = StartCoroutine(TravelArc());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private IEnumerator TravelArc()
    {
        while (timer < travelTime)
        {
            timer += Time.deltaTime;
            float progress = timer / travelTime;

            Vector3 flatDirection = Vector3.Lerp(startPos, targetPos, progress);
            float heightOffset = ArcHeight * Mathf.Sin(Mathf.PI * progress);
            transform.position = flatDirection + Vector3.up * heightOffset;

            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        if (exploded) return;
        exploded = true;

        ObjectPool.Instance.SpawnEffect(EffectType.GrenadeExplode, transform.position - new Vector3(0, 0.02f, 0), Quaternion.identity);

        Collider[] hits = Physics.OverlapSphere(transform.position, ExplosionRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Zombie"))
            {
                Zombie zombie = hit.GetComponent<Zombie>();
                if (zombie != null)
                {
                    zombie.TakeDamageBase(Damage, Type);
                }
            }
        }

        trailRenderer.Clear();
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!exploded && (other.CompareTag("Zombie") || other.CompareTag("Place")))
        {
            Explode();
        }
    }

   
}
