using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTime : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(DisableAfterSeconds(3));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            Zombie zombie = other.GetComponent<Zombie>();
            if (zombie != null)
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;

                // Collider üzərindəki ən yaxın nöqtəni tap
                Vector3 surfacePoint = other.ClosestPoint(transform.position);

                // Daxilə doğru azca irəli get (mesh tərəfə)
                Vector3 adjustedPoint = surfacePoint + direction * 0.2f; // istəsən 0.1f-0.3f dəyiş

                //zombie.TakeDamage(Damage, Type, adjustedPoint);
            }

        }
    }
    private IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}
