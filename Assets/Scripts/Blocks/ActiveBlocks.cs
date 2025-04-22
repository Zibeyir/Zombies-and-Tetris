
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ActiveBlocks : MonoBehaviour
{
    [SerializeField] Transform[] transformsPoints;
    [SerializeField] GameObject[] blocks;
     public List<Transform> currentBlocksA=new List<Transform>();

    public Transform target; 
    public float duration = 1f;

    bool ActivedButtonOpened=false;
    //GridSelector gridSelector;
    private void Start()
    {
        //gridSelector = FindAnyObjectByType<GridSelector>();
    }


    public void ActivetedBlockButtonforSpawn(int coin)
    {
        //Debug.Log("ActiveButtonFunc "+(coin >= LevelManager.Instance.BlockPrice)+" "+ BlocksInGrid());
        if (coin >= LevelManager.Instance.BlockPrice && BlocksInGrid()) {
            ActivedButtonOpened = true;

            UIManager.Instance.ActivatedBlockButton(true);                  
        }
    }
    private bool BlocksInGrid()
    {
        currentBlocksA = _GameTimeData.Instance.ActiveButtonBlocks;

        foreach (var block in currentBlocksA) {
            if (!block.GetComponent<DraggableBlock>().isInGridFirstTime) return false;
        }
        return true;
    }
    public void AddBlocks()
    {
        ActivedButtonOpened = false;
        SpawnNewBlocks(3);
        UIManager.Instance.SetCoins(-50);

        transform.DOMove(target.position, duration).OnComplete(() => ActiveBlocksScript()); 
    }
    public void SpawnNewBlocks(int MaxRandom)
    {
        _GameTimeData.Instance.ActiveButtonBlocks.Clear();
        for (int i = 0; i < MaxRandom; i++)
        {
            GameObject instatedBlock = Instantiate(blocks[Random.Range(0,6)], transformsPoints[i].position, transformsPoints[i].rotation);
            instatedBlock.transform.SetParent(transform);
            _GameTimeData.Instance.CurrentBlocks.Add(instatedBlock.transform);
            _GameTimeData.Instance.CurrentBlocksWeapons.Add(instatedBlock.GetComponent<Weapon>());
            _GameTimeData.Instance.ActiveButtonBlocks.Add(instatedBlock.transform);
            //gridSelector.AddBlocks(instatedBlock.transform,instatedBlock.GetComponent<Weapon>());
        }
    }
    private void ActiveBlocksScript()
    {
        currentBlocksA = _GameTimeData.Instance.ActiveButtonBlocks;
        foreach (var block in currentBlocksA) { 
            block.transform.SetParent(null);
            block.GetComponent<DraggableBlock>().enabled = true;
        }
    }
}
