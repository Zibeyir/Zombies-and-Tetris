using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaskObject : MonoBehaviour
{
    [SerializeField] int customRenderQueue = 3001;

    void Start()
    {
        var rend = GetComponent<Renderer>();
        if (rend != null && rend.material != null)
        {
            rend.material.renderQueue = customRenderQueue;
        }
    }
}
