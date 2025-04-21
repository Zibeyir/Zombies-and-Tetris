using System.Collections;
using UnityEngine;

public class ObjectFalseTime : MonoBehaviour
{
    [SerializeField] private float time=2;
    void OnEnable()
    {
        StartCoroutine(DisableAfterSeconds(time));
    }
    private IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}


