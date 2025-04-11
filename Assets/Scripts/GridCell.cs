using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public DraggableBlock draggableBlock=null;
    public bool BlockBool= false;
    public void GetDraggableBlock(DraggableBlock _draggableBlock)
    {
        if (draggableBlock != null&&draggableBlock!= _draggableBlock) return;

        Debug.Log("draggable Cell");
        draggableBlock = _draggableBlock;
        BlockBool = true;
        draggableBlock.SetGridCell(this);
    }
    public void GetDraggableBlockfromParts(DraggableBlock _draggableBlock)
    {                
        //draggableBlock = _draggableBlock;
    }
    public void RemoveDraggableBlock()
    {
        draggableBlock=null;
        BlockBool = false;

    }
}
