using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletType Type;
    public float Speed = 10f;
    public int Damage = 20;

    private Transform targetZombie;

    private void OnEnable()
    {
        FindClosestZombie();
    }

    private void Update()
    {
        if (targetZombie != null)
        {
            Vector3 dir = (targetZombie.position - transform.position).normalized;
            transform.Translate(dir * Speed * Time.deltaTime , Space.World);
        }
        else
        {
            transform.Translate(Vector3.up * Speed * Time.deltaTime); // əgər zombi yoxdursa yuxarı getsin
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();
            if (zombie != null)
            {
                zombie.TakeDamage(Damage, Type);
            }
            gameObject.SetActive(false);
        }
    }

    private void FindClosestZombie()
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

        if (closest != null)
            targetZombie = closest.transform;
    }
}
public enum BulletType
{
    Pistol,
    Grenade,
    Shotgun,
    Rocket
}