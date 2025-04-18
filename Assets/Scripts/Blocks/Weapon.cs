using DG.Tweening;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public BulletType BulletType;
    public Transform FirePoint;
    public WeaponType _WeaponType;
    [SerializeField] FollowObject followObject;
    [SerializeField] private ParticleSystem? particleSystemFire;
    DraggableBlock draggableBlock;
    public int WeaponLevel = 0;



    private void Start()
    {
        draggableBlock = new DraggableBlock();
    }
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

    public void MergeBlockAndLevelUp()
    {
        WeaponLevel++;
    }

    public void MergeBlockAndDestroy()
    {
        followObject.gameObject.SetActive(false);
        //foreach (GridCell g in draggableBlock.allCells) {
        //    g.RemoveDraggableBlock();
        //}
        Destroy(gameObject);
    }
}
