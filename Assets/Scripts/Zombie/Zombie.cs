using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.UIElements;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
    private EnemyData enemyData;
    public float currentHealth;
    public float maxHealth;

    [SerializeField] string enemyName;
    public float MoveSpeed = 1.5f;
    public float AttackInterval = 3.0f;
    public int Damage = 10;

    private bool isAttacking = false;
    private bool dieBool = false;

    private Animator animator;
    public Fence targetFence;
    private float checkDistance = 1f;

    public string EnemyId;

    private Tween hitTween;
    private NavMeshAgent navAgent;
    public UnityEngine.UI.Slider slider;  // UI Slider komponenti
    [SerializeField] GameObject healthBarPrefab;
    private void OnEnable()
    {
        InitializeComponents();
        LoadEnemyStats();
        SetupNavAgent();
        FindTargetWithRay();
        slider.enabled = false;
        healthBarPrefab.SetActive(false);
    }

    private void Update()
    {
        if (dieBool && !isAttacking && targetFence != null)
        {
            MoveTowardsTarget();
        }
    }

    private void InitializeComponents()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.avoidancePriority = Random.Range(40, 80);
    }

    private void LoadEnemyStats()
    {
        enemyData = GameDataService.Instance.GetEnemyById(EnemyId);
        if (enemyData == null)
        {
            Debug.LogError("EnemyData not found for ID: " + EnemyId);
            return;
        }

        currentHealth = enemyData.HP;
        maxHealth = currentHealth;
        Damage = enemyData.Damage;
    }

    private void SetupNavAgent()
    {
        navAgent.speed = MoveSpeed;
        navAgent.stoppingDistance = checkDistance;
        navAgent.updateRotation = false;
        navAgent.updatePosition = true;
    }

    private void FindTargetWithRay()
    {
        int waterLayerMask = LayerMask.GetMask("Water");
        Ray ray = new Ray(transform.position + Vector3.forward * 0.5f, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, waterLayerMask))
        {
            if (hit.collider.CompareTag("Fence"))
            {
                //Debug.Log(hit.point + " Find Fence " + (hit.point + new Vector3(0, 0, 2)));
                targetFence = hit.collider.GetComponent<Fence>();
                navAgent.SetDestination(hit.point + new Vector3(0, 0, 2));
            }
        }
    }


    private void MoveTowardsTarget()
    {
        if (targetFence != null)
        {
            navAgent.SetDestination(targetFence.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fence"))
        {
            navAgent.enabled = false;
            GetComponent<NavMeshObstacle>().enabled = true;
            isAttacking = true;
            animator.SetTrigger("Attack");
            StartCoroutine(DealDamageRepeatedly());
        }
    }

    private IEnumerator DealDamageRepeatedly()
    {
        while (isAttacking && targetFence != null)
        {
            //Debug.Log(Damage + " Damage Fence");
            targetFence.TakeDamage(Damage);
            yield return new WaitForSeconds(AttackInterval);
        }
    }

    public void TakeDamage(int damage, BulletType type,Vector3 hitPoint)
    {
        healthBarPrefab.SetActive(true);

        slider.enabled = true;

        TakeDamageBase(damage, type);
        ObjectPool.Instance.SpawnEffect(EffectType.ZombieBlood, hitPoint,Quaternion.identity);
    }
    
    public void TakeDamageBase(int damage,BulletType type)
    {
        if (type == BulletType.Grenade)
            damage *= 2;

        currentHealth -= damage;
        slider.value = currentHealth / maxHealth;

        if (!dieBool && (hitTween == null || !hitTween.IsActive() || !hitTween.IsPlaying()))
        {
            hitTween = transform.DOScale(Vector3.one * 0.6f, 0.15f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutBack)
                .OnComplete(() => transform.localScale = Vector3.one * 0.5f);
        }

        if (currentHealth <= 0)
        {
            dieBool = true;
            Die();
        }
    }
    public void Die()
    {
        healthBarPrefab.SetActive(false);

        navAgent.enabled = false;
        WaveManager.Instance.RemoveZombie(gameObject);
        //navAgent.isStopped = true;
        GetComponent<BoxCollider>().enabled = false;
        StartCoroutine(DeathCourtineZombie());
        animator.SetTrigger("Die");
        isAttacking = true;
        if (enemyName == "Trump")
        {
            ObjectPool.Instance.SpawnCoin(EffectType.Cristal, transform.position, Quaternion.identity);

            UIManager.Instance.SetCyristals(5);
        }
        else
        {
            ObjectPool.Instance.SpawnCoin(EffectType.Coin, transform.position, Quaternion.identity);

        }
        //Destroy(gameObject, 5f);
        UIManager.Instance.SetCoins(enemyData.CoinReward);
    }

    
    private IEnumerator DeathCourtineZombie()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject,1);
        StopAllCoroutines();
    }
}
