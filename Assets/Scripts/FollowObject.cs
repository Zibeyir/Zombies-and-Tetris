using UnityEngine;

public class FollowObject : MonoBehaviour
{
    private Transform targetParent; 
    public float followSpeed = 20f; 

    private void Start()
    {
        targetParent = transform.parent;
        transform.parent = null;
    }

    private void Update()
    {
        if (targetParent == null) return;

        transform.position = Vector3.Lerp(transform.position, targetParent.position, followSpeed * Time.deltaTime);
    }
}
