using UnityEngine;
using System.Collections;

public abstract class BulletBase : MonoBehaviour
{
    public BulletType Type;
    public float Speed = 10f;
    public int Damage = 20;

    protected Vector3 moveDirection;
    protected Coroutine disableRoutine;
    //[SerializeField] protected TrailRenderer trailRenderer;
    Vector3 hitPoint;
    protected virtual void OnEnable()
    {
        //trailRenderer?.Clear();
        //trailRenderer.enabled = true;

        if (disableRoutine != null)
            StopCoroutine(disableRoutine);

        disableRoutine = StartCoroutine(DisableAfterSeconds(3f));
    }
    public void Initiliaze(int damage)
    {
        Damage = damage;
    }

    protected virtual void Update()
    {
        transform.Translate(moveDirection * Speed * Time.deltaTime, Space.World);
    }

    protected virtual IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        DisableSelf();
    }

    protected virtual void DisableSelf()
    {
        if (disableRoutine != null)
        {
            StopCoroutine(disableRoutine);
            disableRoutine = null;
        }

        //trailRenderer?.Clear();
        gameObject.SetActive(false);
    }

    protected GameObject FindClosestZombie()
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

    protected Vector3 GetAdjustedHitPoint(Collider other)
    {
        Vector3 direction = (other.transform.position - transform.position).normalized;
        Vector3 surfacePoint = other.ClosestPoint(transform.position);
        return surfacePoint + direction * 0.2f;
    }
}
