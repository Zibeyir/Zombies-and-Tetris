using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using System.Collections;

[RequireComponent(typeof(Animator), typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoBehaviour
{
    protected EnemyData enemyData;
    protected float currentHealth;
    protected bool isAttacking = false;
    protected bool isDead = false;

    protected Animator animator;
    protected NavMeshAgent navAgent;
    protected Fence targetFence;
    protected float checkDistance = 1f;

    protected Tween hitTween;
    public string EnemyId;

    public float MoveSpeed = 1.5f;
    public float AttackInterval = 3.0f;
    public int Damage = 10;

    protected virtual void OnEnable()
    {
        InitializeComponents();
        LoadEnemyStats();
        SetupNavAgent();
        FindTargetWithRay();
    }

    protected virtual void InitializeComponents()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.avoidancePriority = Random.Range(40, 80);
    }

    protected virtual void LoadEnemyStats()
    {
        enemyData = GameDataService.Instance.GetEnemyById(EnemyId);
        if (enemyData == null)
        {
            Debug.LogError("EnemyData not found for ID: " + EnemyId);
            return;
        }
        currentHealth = enemyData.HP;
    }

    protected virtual void SetupNavAgent()
    {
        navAgent.speed = MoveSpeed;
        navAgent.stoppingDistance = checkDistance;
        navAgent.updateRotation = false;
        navAgent.updatePosition = true;
    }

    protected virtual void FindTargetWithRay()
    {
        int waterLayerMask = LayerMask.GetMask("Water");
        Ray ray = new Ray(transform.position + Vector3.forward * 0.5f, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 50f, waterLayerMask) && hit.collider.CompareTag("Fence"))
        {
            targetFence = hit.collider.GetComponent<Fence>();
            navAgent.SetDestination(hit.point + new Vector3(0, 0, 2));
        }
    }

    protected virtual void Update()
    {
        if (!isDead && !isAttacking && targetFence != null)
        {
            navAgent.SetDestination(targetFence.transform.position);
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
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

    protected virtual IEnumerator DealDamageRepeatedly()
    {
        while (isAttacking && targetFence != null)
        {
            targetFence.TakeDamage(Damage);
            yield return new WaitForSeconds(AttackInterval);
        }
    }

    public virtual void TakeDamage(int damage, BulletType type, Vector3 hitPoint)
    {
        TakeDamageBase(damage, type);
        SpawnBloodEffect(hitPoint);
    }

    public virtual void TakeDamageBase(int damage, BulletType type)
    {
        if (type == BulletType.Grenade)
            damage *= 2;

        currentHealth -= damage;

        if (!isDead && (hitTween == null || !hitTween.IsActive()))
        {
            hitTween = transform.DOScale(Vector3.one * 0.6f, 0.15f)
                .SetLoops(2, LoopType.Yoyo)
                .SetEase(Ease.OutBack)
                .OnComplete(() => transform.localScale = Vector3.one * 0.5f);
        }

        if (currentHealth <= 0 && !isDead)
        {
            isDead = true;
            Die();
        }
    }

    protected virtual void SpawnBloodEffect(Vector3 hitPoint)
    {
        ObjectPool.Instance.SpawnEffect(EffectType.ZombieBlood, hitPoint, Quaternion.identity);
    }

    protected abstract void Die();
}
