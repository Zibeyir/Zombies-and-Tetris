using UnityEngine;

public class Bullet : BulletBase
{
    private void OnEnable()
    {
        base.OnEnable();
       
        
        moveDirection = (closest != null)
            ? (closest.transform.position - transform.position).normalized
            : -Vector3.forward;

        transform.forward = -moveDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.TakeDamage(Damage, Type, GetAdjustedHitPoint(other));
            }

            DisableSelf();
        }
        else if (other.CompareTag("Boss"))
        {
            Boss boss = other.GetComponent<Boss>();
            if (boss != null)
            {
                boss.TakeDamage(Damage, Type, GetAdjustedHitPoint(other));
                //ObjectPool.Instance.SpawnEffect(EffectType.ShotgunTouchExplode, transform.position, Quaternion.identity);
                //ObjectPool.Instance.SpawnEffect(EffectType.ShotgunTouchExplodeFire, transform.position, Quaternion.identity);
            }

            DisableSelf();
        }
    }

    public void GetSkillsBullet() { } // Hələki boş
}
