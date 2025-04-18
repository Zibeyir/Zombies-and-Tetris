using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeBlocks : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Merge"))
        {
            Debug.Log("Merge");
        }
    }
}
