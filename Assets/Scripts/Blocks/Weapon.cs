using DG.Tweening;
using Unity.VisualScripting;
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
    int damage;
    public string Name;
    public int[] DamageByLevel = new int[5];
    float healthBlock;
    float healthBlockMax;

    private void Start()
    {
        healthBlock = 30;
        healthBlockMax = 30;
        draggableBlock = GetComponent<DraggableBlock>();
        DamageByLevel = GameDataService.Instance.GetWeapon(_WeaponType).Damages;

    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ball")){
            --healthBlock;
            if (healthBlock<=0)
            {
                followObject.gameObject.SetActive(false);
                MergeBlockAndDestroy();
                _GameTimeData.Instance.CurrentBlocks.Remove(this.gameObject.transform);
                _GameTimeData.Instance.CurrentBlocksWeapons.Remove(this);
                _GameTimeData.Instance.ActiveButtonBlocks.Remove(this.gameObject.transform);

            }
            Fire();
            //Debug.Log("BallDamege "+ healthBlock);
            followObject.TouchBallScale(healthBlock, healthBlockMax);
            if (particleSystemFire != null&&!particleSystemFire.isPlaying) particleSystemFire.Play();
        }
     
    }


    private void Fire()
    {
        if (BulletType==BulletType.Shotgun)
        {
            ObjectPool.Instance.SpawnBullet(BulletType, FirePoint.position, FirePoint.rotation, DamageByLevel[WeaponLevel]);

        }
        else
        {
            ObjectPool.Instance.SpawnBullet(BulletType, FirePoint.position, FirePoint.rotation, DamageByLevel[WeaponLevel]);

        }
    }

    public void MergeBlockAndLevelUp()
    {
        healthBlock = healthBlockMax;
        followObject.MergeBallScale();
        ++WeaponLevel;
        followObject.GetMaterialWeapon(WeaponLevel);
    }

    public void MergeBlockAndDestroy()
    {
        followObject.gameObject.SetActive(false);
        if (draggableBlock.allCells != null)
        {
            foreach (GridCell g in draggableBlock.allCells)
            {
                g.RemoveDraggableBlock();
            }
        }
       
        Destroy(draggableBlock.gameObject);
    }
    public int GetDamageByLevel(int level)
    {
        if (level <= 0 || level > DamageByLevel.Length)
        {
            Debug.LogWarning($"Invalid weapon level: {level} for {Name}");
            return DamageByLevel[0]; // default Level 1 damage
        }

        return DamageByLevel[level - 1]; // level 1 = index 0
    }
}
