using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Zombie : MonoBehaviour
{
    private EnemyData enemyData;
    public float currentHealth;

    public float MoveSpeed = 1.5f;
    public float AttackInterval = 1.0f;
    public int Damage = 10;

    private bool isAttacking = false;
    private Animator animator;
    private Fence targetFence;
    private float checkDistance = 1f;

    public string EnemyId;
    private void Start()
    {
        animator = GetComponent<Animator>();
        targetFence = GameObject.FindGameObjectWithTag("Fence")?.GetComponent<Fence>();

        // Load data from JSON (via GameDataService)
        enemyData = GameDataService.Instance.GetEnemyById(EnemyId);
        if (enemyData == null)
        {
            Debug.LogError("EnemyData not found for ID: " + EnemyId);
            return;
        }

        currentHealth = enemyData.HP;
    }

    private void Update()
    {
        if (!isAttacking && targetFence != null)
        {

            if (!IsBlockedAhead())
            {
                transform.Translate(Vector3.forward * MoveSpeed * Time.deltaTime);

            }
        }
    }
    private bool IsBlockedAhead()
    {
        // Önünde başka bir zombi var mı diye kontrol eder
        Ray ray = new Ray(transform.position + Vector3.forward * 0.5f, transform.forward);
        return Physics.Raycast(ray, checkDistance, LayerMask.GetMask("Zombie"));
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
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        StopAllCoroutines();
        animator.SetTrigger("Die");
        //isAttacking = false;
        Destroy(gameObject, 5f);
        //WaveManager.Instance.OnZombieKilled();
    }
}
