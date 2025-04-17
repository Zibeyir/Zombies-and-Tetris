
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ActiveBlocks : MonoBehaviour
{
    [SerializeField] Transform[] transformsPoints;
    [SerializeField] GameObject[] blocks;
    List<GameObject> currentBlocks=new List<GameObject>();

    public Transform target; 
    public float duration = 1f; 

  
    private void Start()
    {

    }
    public void AddBlocks()
    {
        SpawnNewBlocks(3);
        transform.DOMove(target.position, duration).OnComplete(() => ActiveBlocksScript()); 
    }
    public void SpawnNewBlocks(int MaxRandom)
    {
        currentBlocks.Clear();
        for (int i = 0; i < MaxRandom; i++)
        {
            GameObject instatedBlock = Instantiate(blocks[Random.Range(0,MaxRandom)], transformsPoints[i].position, transformsPoints[i].rotation);
            instatedBlock.transform.SetParent(transform);
            currentBlocks.Add(instatedBlock);
        }
    }
    private void ActiveBlocksScript()
    {
        foreach (var block in currentBlocks) { 
            block.transform.SetParent(null);
            block.GetComponent<DraggableBlock>().enabled = true;
        }
    }
}
