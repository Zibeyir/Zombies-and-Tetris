using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMaterial : MonoBehaviour
{
    public static GridMaterial Instance { get; private set; }
    [SerializeField] Material material;
    [SerializeField] MeshRenderer[] meshRenderer;
    Color color;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        color = material.color;
    }
    private void Start()
    {
        CellsAllMaterial();
    }
    public void CellsAllMaterial()
    {
        for (int i = 0; i < meshRenderer.Length; i++)
        {
            meshRenderer[i].material.color = color;
        }
    }
}
