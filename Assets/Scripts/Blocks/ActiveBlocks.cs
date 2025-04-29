
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ActiveBlocks : MonoBehaviour
{
    [SerializeField] Transform[] transformsPoints;
     public static List<GameObject> blocks;
     public List<Transform> currentBlocksA=new List<Transform>();

    public Transform target; 
    public float duration = 1f;
    public static int OpenedBlockCount;
    bool ActivedButtonOpened=false;
    GameObject instatedBlock;
    [SerializeField] TutorialStarter tutorialStarter;
   // [SerializeField] GameObject[] TestDerstroys;
    //GridSelector gridSelector;
    private void Start()
    {
        //gridSelector = FindAnyObjectByType<GridSelector>();
        GetOpenedBlocks();
       StartCoroutine(StartTime());
    }

    IEnumerator StartTime()
    {
        yield return new WaitForSeconds(1);
        ActivedButtonOpened = false;
        SpawnNewBlocks(3);

        transform.DOMove(target.position, duration).OnComplete(() => ActiveBlocksScript());
    }
    public void ActivetedBlockButtonforSpawn(int coin)
    {
        //Debug.Log("ActiveButtonFunc "+(coin >= LevelManager.Instance.BlockPrice)+" "+ BlocksInGrid());
        if (coin >= LevelManager.Instance.BlockPrice && BlocksInGrid()) {
            ActivedButtonOpened = true;

            UIManager.Instance.ActivatedBlockButton(true);                  
        }
    }
    public static void GetOpenedBlocks()
    {
        blocks = GameDataService.Instance.GetActiveWeapons();
    }
    private bool BlocksInGrid()
    {
        currentBlocksA = _GameTimeData.Instance.ActiveButtonBlocks;


        foreach (var block in currentBlocksA)
        {
            if (!block.GetComponent<Building>().ActiveCell) return false;
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
            instatedBlock = Instantiate(blocks[Random.Range(0, blocks.Count)], transformsPoints[i].position, Quaternion.identity);
            instatedBlock.transform.SetParent(transform);
            _GameTimeData.Instance.CurrentBlocks.Add(instatedBlock.transform);
            _GameTimeData.Instance.CurrentBlocksWeapons.Add(instatedBlock.GetComponentInChildren<Weapon>());
            //_GameTimeData.Instance.CurrentBlocksWeapons.Add(instatedBlock.GetComponentInChildren<Weapon>());

            _GameTimeData.Instance.ActiveButtonBlocks.Add(instatedBlock.transform);

           
            //_GameTimeData.Instance.CurrentBlocks.Add(instatedBlock.transform);
            //_GameTimeData.Instance.CurrentBlocksWeapons.Add(instatedBlock.GetComponentInChildren<Weapon>());
            //_GameTimeData.Instance.ActiveButtonBlocks.Add(instatedBlock.transform);
            //gridSelector.AddBlocks(instatedBlock.transform,instatedBlock.GetComponent<Weapon>());
        }
    }
    private void ActiveBlocksScript()
    {
        Debug.Log("ActiveBlocksScript");
        tutorialStarter.StartHandutorial(transformsPoints[1]);

        currentBlocksA = _GameTimeData.Instance.ActiveButtonBlocks;
        foreach (var block in currentBlocksA) { 
            block.transform.SetParent(null);
            block.GetComponentInChildren<DraggableBlock>().enabled = true;
        }
    }
}
