using DG.Tweening;
using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{
    public BulletType BulletType;
    public Transform FirePoint;
    private Tween hitTween;
    [SerializeField] FollowObject followObject;
   
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
