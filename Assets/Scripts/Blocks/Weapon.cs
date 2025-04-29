using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;


public class Weapon : MonoBehaviour
{
    public BulletType BulletType;
    public Transform FirePoint;
    public WeaponType _WeaponType;
    private Weapon mergeWeapon;
    [SerializeField] FollowObject followObject;
    [SerializeField] private ParticleSystem? particleSystemFire;
    DraggableBlock draggableBlock;
    public int WeaponLevel = 0;
    int damage;
    public string Name;
    public int[] DamageByLevel = new int[5];
    int WeaponLevelfromUpgrade;
    float healthBlock;
    float healthBlockMax;
    public Building _building;
    public bool MoveBool=false;
    public bool MoveBoolGreen = false;
    public float sqrDist=10;
    public LayerMask layerMask;

    private void Start()
    {
        layerMask = LayerMask.GetMask("Block");

        healthBlock = 30;
        healthBlockMax = 30;
        draggableBlock = GetComponent<DraggableBlock>();
        DamageByLevel = GameDataService.Instance.GetWeapon(_WeaponType).Damages;
        WeaponLevelfromUpgrade = (GameDataService.Instance.GetWeapon(_WeaponType).Level - 1)*5 ;
        //WeaponDamage = 
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ball")){
            --healthBlock;
            if (healthBlock<=0)
            {
                _GameTimeData.Instance.ActiveButtonBlocks.Remove(_building.transform);
                Destroy(_building.gameObject);

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
    private void FixedUpdate()
    {
        if (!MoveBool) return;

        // Rayın başlanğıc nöqtəsi və istiqaməti
        RaycastHit hit;
        Ray ray = new Ray(transform.position-new Vector3(0,0.3f,0), -transform.forward);  // Bu, obyektdən irəliləyən bir raydır

        // Ray ilə obyektə baxmaq
        if (Physics.Raycast(ray, out hit, sqrDist, layerMask)) // `sqrDist` məsafəni təyin edirik
        {

            // Rayın toxunduğu obyektin tagını yoxlayırıq
            if (hit.collider.CompareTag("Selectedblock") && hit.collider.gameObject != gameObject)
            {
                Debug.Log("Weapon Hit");

                mergeWeapon = hit.collider.GetComponent<Weapon>();

                // Obyektin növü və səviyyəsi eynidirsə, merge etmək
                if (mergeWeapon._WeaponType == _WeaponType && mergeWeapon.WeaponLevel == WeaponLevel)
                {
                    Debug.Log("Weapon Merge");
                    mergeWeapon.MergeBlockAndLevelUp();
                    MergeBlockAndDestroy();
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        // Gizmos ilə rayı çəkmək
        if (MoveBool) // Rayı yalnız obyekt hərəkət etdikdə göstəririk
        {
            Ray ray = new Ray(transform.position - new Vector3(0, 0.5f, 0), -transform.forward); // Rayın başlanğıcı və istiqaməti
            Gizmos.color = Color.red; // Rayın rəngi (qırmızı)
            Gizmos.DrawRay(ray.origin, ray.direction * sqrDist); // Rayın başlanğıc nöqtəsindən təyin olunmuş məsafə qədər çəkilməsi
        }
    }

    private void Fire()
    {
        if (BulletType==BulletType.Shotgun)
        {
            ObjectPool.Instance.SpawnBullet(BulletType, FirePoint.position, FirePoint.rotation, DamageByLevel[WeaponLevel]+ WeaponLevelfromUpgrade);

        }
        else
        {
            ObjectPool.Instance.SpawnBullet(BulletType, FirePoint.position, FirePoint.rotation, DamageByLevel[WeaponLevel]+ WeaponLevelfromUpgrade);

        }
    }
    public void SetBuilding(Building building)
    {
        _building = building;
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
        Time.timeScale = 1;

        _GameTimeData.Instance.ActiveButtonBlocks.Remove(_building.transform);

        Destroy(_building.gameObject);
        followObject.gameObject.SetActive(false);
        if (draggableBlock.allCells != null)
        {
            foreach (GridCell g in draggableBlock.allCells)
            {
                g.RemoveDraggableBlock();
            }
        }
        GridMaterial.Instance.CellsAllMaterial();
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
