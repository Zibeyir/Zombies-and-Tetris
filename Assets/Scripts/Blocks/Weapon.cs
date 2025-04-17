using DG.Tweening;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public BulletType BulletType;
    public Transform FirePoint;
    
    [SerializeField] FollowObject followObject;
    [SerializeField] private ParticleSystem? particleSystemFire;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ball")){
            Fire();
            followObject.TouchBallScale();
            if (particleSystemFire != null&&!particleSystemFire.isPlaying) particleSystemFire.Play();
        }
    }
    private void Fire()
    {
        if (BulletType==BulletType.Shotgun)
        {
            ObjectPool.Instance.SpawnFromPool(BulletType, FirePoint.position, FirePoint.rotation);

        }
        else
        {
            ObjectPool.Instance.SpawnFromPool(BulletType, FirePoint.position, FirePoint.rotation);

        }
    }
}
