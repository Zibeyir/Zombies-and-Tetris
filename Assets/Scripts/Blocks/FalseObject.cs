using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalseObject : MonoBehaviour
{

    [SerializeField] private float timeFalse = 5;
    void OnEnable()
    {
        StartCoroutine(SetActiveFalseTime());
    }

    private IEnumerator SetActiveFalseTime()
    {  
        yield return new WaitForSeconds(timeFalse);
        gameObject.SetActive(false);       
    }

}
