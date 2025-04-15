using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))] // Add NavMeshAgent component
public class Zombie : MonoBehaviour
{
    private EnemyData enemyData;
    public float currentHealth;

    public float MoveSpeed = 1.5f;
    public float AttackInterval = 1.0f;
    public int Damage = 10;

    private bool isAttacking = false;
    private bool dieBool = false;

    private Animator animator;
    private Fence targetFence;
    private float checkDistance = 1f;

    public string EnemyId;

    private Tween hitTween;
    private Rigidbody rb;
    private NavMeshAgent navAgent; // Reference to the NavMeshAgent
    
    private void OnEnable()
    {
        dieBool = true;
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();
        navAgent = GetComponent<NavMeshAgent>(); // Initialize NavMeshAgent
        targetFence = GameObject.FindGameObjectWithTag("Fence")?.GetComponent<Fence>();
        navAgent.avoidancePriority = Random.Range(40, 80);
        enemyData = GameDataService.Instance.GetEnemyById(EnemyId);
        if (enemyData == null)
        {
            Debug.LogError("EnemyData not found for ID: " + EnemyId);
            return;
        }

        currentHealth = enemyData.HP;
        //rb.isKinematic = false; // Enable physics

        navAgent.speed = MoveSpeed; // Set the move speed for NavMeshAgent
        navAgent.stoppingDistance = checkDistance; // Set distance when the agent stops moving towards the target
        navAgent.updateRotation = false; // Disable rotation, so the zombie doesn't spin
        navAgent.updatePosition = true; // Allow position updates via NavMeshAgent
    }

    private void Update()
    {
        if (dieBool&&!isAttacking && targetFence != null)
        {
            MoveTowardsTarget();
        }
    }

    private void MoveTowardsTarget()
    {
        if (targetFence != null)
        {
            navAgent.SetDestination(targetFence.transform.position); // Set the destination to the target (the fence)
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Fence"))
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
            StartCoroutine(DealDamageRepeatedly());
        }
    }

    private IEnumerator DealDamageRepeatedly()
    {
        while (isAttacking && targetFence != null)
        {
            targetFence.TakeDamage(Damage);
            yield return new WaitForSeconds(AttackInterval);
        }
    }

    public void TakeDamage(int damage, BulletType type)
    {
        if (type == BulletType.Grenade)
            damage *= 2;

        currentHealth -= damage;

        if (!dieBool && (hitTween == null || !hitTween.IsActive() || !hitTween.IsPlaying()))
        {
            Debug.Log("TakeDamage");
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
        StopAllCoroutines();
        animator.SetTrigger("Die");
        isAttacking = true;
        Destroy(gameObject, 5f);
    }
}
