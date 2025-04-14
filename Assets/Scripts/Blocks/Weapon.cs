using UnityEngine;

public class Weapon : MonoBehaviour
{
    public BulletType BulletType;
    public Transform FirePoint;

  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball")){
            Fire();
        }
    }
    private void Fire()
    {
        ObjectPool.Instance.SpawnFromPool(BulletType, FirePoint.position, FirePoint.rotation);
    }
}
