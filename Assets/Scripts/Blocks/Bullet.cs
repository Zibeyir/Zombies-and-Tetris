using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletType Type;
    public float Speed = 10f;
    public int Damage = 20;

    private void Update()
    {
        transform.Translate(Vector3.up * Speed * Time.deltaTime);
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
            Debug.Log("BulletFired");
            gameObject.SetActive(false); // Pool-a geri qaytar
        }
    }
}
public enum BulletType
{
    Pistol,
    Grenade,
    Shotgun,
    Rocket
}
