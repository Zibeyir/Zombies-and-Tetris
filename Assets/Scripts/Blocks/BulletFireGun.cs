using UnityEngine;
using System.Collections;

public class BulletFireGun : MonoBehaviour
{
    public BulletType Type;
    public float Speed = 10f;
    public int Damage = 20;

    private Vector3 moveDirection;
    private Coroutine disableRoutine;
    [SerializeField] private TrailRenderer trailRenderer;

    private void OnEnable()
    {
        FindDirectionToClosestZombie();
        transform.forward = -moveDirection; 

        //trailRenderer.enabled = true;

        if (disableRoutine != null)
            StopCoroutine(disableRoutine);

        disableRoutine = StartCoroutine(DisableAfterSeconds(3f));
    }


    private void Update()
    {
        transform.Translate(moveDirection * Speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();
            if (zombie != null)
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;

                // Collider üzərindəki ən yaxın nöqtəni tap
                Vector3 surfacePoint = other.ClosestPoint(transform.position);

                // Daxilə doğru azca irəli get (mesh tərəfə)
                Vector3 adjustedPoint = surfacePoint + direction * 0.2f; // istəsən 0.1f-0.3f dəyiş
                ObjectPool.Instance.SpawnFromPool(BulletType.ShortGunTouchExpole, transform.position, Quaternion.identity);
                ObjectPool.Instance.SpawnFromPool(BulletType.ShortGunTouchExpoleFire, transform.position, Quaternion.identity);

                zombie.TakeDamage(Damage, Type, adjustedPoint);
            }

            DisableSelf();
        }
    }

    private void FindDirectionToClosestZombie()
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

        moveDirection = (closest != null)
            ? (closest.transform.position - transform.position).normalized
            : -Vector3.forward;
    }

    private IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DisableSelf();
    }

    private void DisableSelf()
    {
        //trailRenderer.Clear();
        //trailRenderer.enabled = false;
        if (disableRoutine != null)
        {
            StopCoroutine(disableRoutine);
            disableRoutine = null;
        }
        

        gameObject.SetActive(false);
    }
}

