using System.Collections.Generic;
using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    public GridCell currentGridCell;
    public List<GridCell> allCells;
    public bool isInGrid = false;
    public float moveSpeed = 25f;

    private Vector3 targetPosition;
    private Vector3 target;
    private Vector3 targetBase;
    public bool targetBool;
    public bool LastBool=false;
    [SerializeField] Vector3 offset;
    private float fixedZ;

    public BlockPartsCheck[] blockPartsChecks;
    private void Start()
    {
        targetBool = true;
        allCells=new List<GridCell>();
         blockPartsChecks = GetComponentsInChildren<BlockPartsCheck>();
        foreach (var part in blockPartsChecks)
            part.draggableBlock = this;
        fixedZ = transform.position.z;
        targetPosition = transform.position;
        targetBase = targetPosition;
    }

    public void SetGridCell(GridCell newCell)
    {
        if (currentGridCell != null)
            currentGridCell.RemoveDraggableBlock();
        if (newCell.draggableBlock == this)
        {
            Debug.Log("Remove all cells");
           
            currentGridCell = newCell;
            targetPosition = currentGridCell.GetComponent<Renderer>().bounds.center;
            isInGrid = true;
            targetBool = true;
        }
       


    }


    public void SetGridStatus(bool status)
    {
        LastBool=status;
        if (LastBool) LastCheckforCells();
        isInGrid = status;
    }

    private void FixedUpdate()
    {
        if (!isInGrid ) return;

        target = new Vector3(targetPosition.x, targetPosition.y, fixedZ) + offset;
        transform.position = target;

        if (Vector3.Distance(transform.position, target) < 0.01f && targetBool)
        {
            targetBool = false;
            CheckGridStation();
        }
    }


    public void CheckGridStation()
    {
        if (AllPartsOverValidGrids())
        {
            targetBase = targetPosition;
            Debug.Log("AllCheck");
           
        }
        else
        {
            
            targetPosition = targetBase;

        }
    }

    public void LastCheckforCells()
    {
        Debug.Log("LastCheckforCells");
        if (currentGridCell!=null)
        {
            
            allCells.Clear();

            foreach (var checker in blockPartsChecks)
            {
                if (checker.ownCcell!=null)
                {
                    allCells.Add(checker.ownCcell);
                    checker.ownCcell.GetDraggableBlockfromParts(this);
                    Debug.Log("LastCheckforCells 3");

                }
            }
            Debug.Log(allCells.Count+ " LastCheckforCells");
        }
    }
    public bool AllCellTouchCell()
    {
        foreach (var checker in blockPartsChecks)
        {
            if (!checker.CellIsFull()) {
                Debug.Log("false+ All");
                return false;
            }
        }
        Debug.Log("true+ All");

        return true;
    }
    public bool AllPartsOverValidGrids()
    {
        foreach (var checker in blockPartsChecks)
        {
            if (!checker.IsOverValidGrid())
                return false;
        }
        return true;
    }

}
