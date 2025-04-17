using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFalseTime : MonoBehaviour
{
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(DisableAfterSeconds(2));
    }
    private IEnumerator DisableAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameObject.SetActive(false);
    }
}


