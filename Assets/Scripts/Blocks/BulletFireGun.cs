using UnityEngine;

public class BulletFireGun : BulletBase
{

    private void OnEnable()
    {
        base.OnEnable();

        GameObject closest = FindClosestZombie();
        moveDirection = (closest != null)
            ? (closest.transform.position - transform.position).normalized
            : -Vector3.forward;

        transform.forward = -moveDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            var zombie = other.GetComponent<Zombie>();
            if (zombie != null)
            {
                Vector3 hitPoint = GetAdjustedHitPoint(other);
                ObjectPool.Instance.SpawnEffect(EffectType.ShotgunTouchExplode, transform.position, Quaternion.identity);
                ObjectPool.Instance.SpawnEffect(EffectType.ShotgunTouchExplodeFire, transform.position, Quaternion.identity);
                zombie.TakeDamage(Damage, Type, hitPoint);
            }
            DisableSelf();

        }
        else if (other.CompareTag("Boss"))
        {
            var boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                Vector3 hitPoint = GetAdjustedHitPoint(other);
                ObjectPool.Instance.SpawnEffect(EffectType.ShotgunTouchExplode, transform.position, Quaternion.identity);
                ObjectPool.Instance.SpawnEffect(EffectType.ShotgunTouchExplodeFire, transform.position, Quaternion.identity);
                boss.TakeDamage(Damage, Type, hitPoint);
            }
            DisableSelf();

        }

        // Hər iki halda da bulleti deaktiv et
    }
}
