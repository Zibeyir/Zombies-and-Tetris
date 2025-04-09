using UnityEngine;
using UnityEngine.EventSystems;

public class BlockSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnHeight = 0.5f;

    private void Start()
    {
        if (spawnPoint == null)
        {
            spawnPoint = transform;
        }
    }

    public void SpawnBlock(BlockType blockType)
    {
        if (blockType == null || blockType.prefab == null) return;

        Vector3 spawnPosition = spawnPoint.position + Vector3.up * spawnHeight;
        GameObject block = Instantiate(blockType.prefab, spawnPosition, Quaternion.identity);

        // Enable physics
        if (block.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.isKinematic = false;
        }
    }
}