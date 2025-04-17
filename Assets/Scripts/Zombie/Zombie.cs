using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : MonoBehaviour
{
    private EnemyData enemyData;
    public float currentHealth;

    public float MoveSpeed = 1.5f;
    public float AttackInterval = 3.0f;
    public int Damage = 10;

    private bool isAttacking = false;
    private bool dieBool = false;

    private Animator animator;
    private Fence targetFence;
    private float checkDistance = 1f;

    public string EnemyId;

    private Tween hitTween;
    private NavMeshAgent navAgent;

    private void OnEnable()
    {
        InitializeComponents();
        LoadEnemyStats();
        SetupNavAgent();
        FindTargetWithRay();
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
        TakeDamageBase(damage, type);
        ObjectPool.Instance.SpawnFromPool(BulletType.ZombieBoold, hitPoint,Quaternion.identity);
    }
    
    public void TakeDamageBase(int damage,BulletType type)
    {
        if (type == BulletType.Grenade)
            damage *= 2;

        currentHealth -= damage;

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
        navAgent.enabled = false;
        StartCoroutine(DeathCourtineZombie());
        animator.SetTrigger("Die");
        isAttacking = true;
        WaveManager.Instance.RemoveZombie(gameObject);
        //Destroy(gameObject, 5f);
        UIManager.Instance.SetCoins(5);
    }
    private IEnumerator DeathCourtineZombie()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject,1);
        StopAllCoroutines();
    }
}
