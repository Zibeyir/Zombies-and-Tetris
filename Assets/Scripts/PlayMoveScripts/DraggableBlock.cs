using System.Collections.Generic;
using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    [SerializeField] Building buildingPrefab;
    public GridCell currentGridCell;

    public GridCell targetGridCell;
    public List<GridCell> allCells;
    public bool isInGrid = false;
    public bool isInGridFirstTime = false;

    public float moveSpeed = 25f;

   
   
    public BlockPartsCheck[] blockPartsChecks;
    public Building _building;
    private void OnEnable()
    {
        buildingPrefab = GetComponent<Building>();
        //targetBool = true;
        //allCells=new List<GridCell>();
        // blockPartsChecks = GetComponentsInChildren<BlockPartsCheck>();
        //foreach (var part in blockPartsChecks)
        //    part._draggableBlock = this;
        //fixedZ = transform.position.y;
        //targetPosition = transform.position;
        //targetBase = transform.position;
    }

    public void SetGridCell(GridCell newCell)
    {
        //targetGridCell=newCell;
        //Debug.Log("SetGridCell");
        //currentGridCell = newCell;
        //targetPosition = currentGridCell.GetComponent<Renderer>().bounds.center;

        //isInGrid = true;
        //targetBool = true;
        ////if (newCell.draggableBlock == this) Debug.Log("newCell.draggableBlock == this");
        ////if (newCell.draggableBlock.currentGridCell != currentGridCell) Debug.Log("newCell.draggableBlock.currentGridCell!=currentGridCell");

        ////currentGridCell.RemoveDraggableBlock();
        //if (newCell.draggableBlock == this)
        //{
           
        //}
       


    }
    public void SetBuilding(Building building)
    {
        _building=building;
    }

    public void SetGridStatus(bool status)
    {
   
    }

    private void Update()
    {
        //if (!isInGrid ) return;
        ////Debug.Log("DraggableUpdate");
        //if (LastBool)
        //{
        //    target = new Vector3(targetBase.x, fixedZ, targetBase.z) + offset;
        //    transform.position = target;
        //}
        //else
        //{
        //    target = new Vector3(targetPosition.x, fixedZ, targetPosition.z) + offset;
        //    transform.position = target;
        //    if (transform.position == target && targetBool)
        //    {
        //        targetBool = false;
        //        CheckGridStation();
        //    }
        //}
            

        
    }


    public void CheckGridStation()
    {
        //if (AllPartsOverValidGrids())
        //{
        //    Debug.Log($"Draggabel True Cell tapıldı: {currentGridCell.name}, draggableBlock: {currentGridCell.draggableBlock}");

        //    Debug.Log("CheckGridStation "+currentGridCell.gameObject.name);
        //    targetBase = currentGridCell.GetComponent<Renderer>().bounds.center;
        //    ;
        //    isInGridFirstTime = true;
        //    targetGridCell = currentGridCell;
        //}
        //else
        //{
        //    Debug.Log("CheckGridStation " + currentGridCell.gameObject.name);

        //    isInGridFirstTime = false;

        //    //targetPosition = targetBase;

        //}
    }

    //public void LastCheckforCells()
    //{
    //    if (targetGridCell != null)
    //    {
            
    //        allCells.Clear();

    //        foreach (var checker in blockPartsChecks)
    //        {
    //            if (checker.ownCcell!=null)
    //            {
    //                allCells.Add(checker.ownCcell);
    //                checker.ownCcell.GetDraggableBlockfromParts(this);

    //            }
    //        }
    //    }
    //}
    //private void OnDestroy()
    //{
    //    if (allCells != null)
    //    {
    //        foreach (var checker in allCells) checker.RemoveDraggableBlock();
    //    }
    //}
    //public bool AllCellTouchCell()
    //{
    //    foreach (var checker in blockPartsChecks)
    //    {
    //        if (!checker.CellIsFull()) {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    public bool AllPartsOverValidGrids()
    {
        //foreach (var checker in blockPartsChecks)
        //{
        //    if (!checker.IsOverValidGrid())
        //        return false;
        //}
        return buildingPrefab.ActiveCell;
    }

}
