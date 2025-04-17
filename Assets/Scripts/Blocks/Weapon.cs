using DG.Tweening;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public BulletType BulletType;
    public Transform FirePoint;
    
    [SerializeField] FollowObject followObject;
    ObjectPool objectPool;

  
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball")){
            Fire();
            followObject.TouchBallScale();
        }
    }
    private void Fire()
    {
        ObjectPool.Instance.SpawnFromPool(BulletType, FirePoint.position, FirePoint.rotation);
    }
}
public enum BulletType
{
    Pistol,
    Grenade,
    Shotgun,
    Rocket,
    ZombieBoold
}