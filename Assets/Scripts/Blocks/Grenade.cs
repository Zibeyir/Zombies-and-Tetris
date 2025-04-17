using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grenade : MonoBehaviour
{
    public BulletType Type;
    public float Speed = 10f;
    public int Damage = 50;
    public float ExplosionRadius = 3f;
    public float ArcHeight = 5f;
    public float MaxDistance = 5f;

    private Vector3 startPos;
    private Vector3 targetPos;
    private float travelTime = 1f;
    private float timer;
    private Coroutine travelRoutine;
    private bool exploded = false;
    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private GameObject explosionEffect;

    private void OnEnable()
    {
        trailRenderer.enabled = true;
        exploded = false;
        GameObject closestZombie = FindClosestZombie();
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
    private void OnTriggerEnter(Collider other)
    {
        if (!exploded && (other.CompareTag("Zombie") || other.CompareTag("Place")))
        {
            Explode();
        }
    }
    private IEnumerator TravelArc()
    {
        while (timer < travelTime)
        {
            timer += Time.deltaTime;
            float progress = timer / travelTime;

            Vector3 flatDirection = Vector3.Lerp(startPos, targetPos, progress);
            float heightOffset = ArcHeight * Mathf.Sin(Mathf.PI * progress); // yarım dairəvi trayektoriya
            transform.position = flatDirection + Vector3.up * heightOffset;

            yield return null;
        }

        //Explode();
    }

    private void Explode()
    {
        if (exploded) return;
        exploded = true;
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

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

    private GameObject FindClosestZombie()
    {
        float minDist = float.MaxValue;
        GameObject closest = null;

        foreach (var zombie in WaveManager.Instance.GetActiveZombies())
        {
            if (zombie != null)
            {
                float dist = Vector3.Distance(transform.position, zombie.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = zombie;
                }
            }
        }

        return closest;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, ExplosionRadius);
    }
}
