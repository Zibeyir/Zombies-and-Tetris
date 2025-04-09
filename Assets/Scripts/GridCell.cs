using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public DraggableBlock draggableBlock;
    public bool BlockBool= false;
    public void GetDraggableBlock(DraggableBlock _draggableBlock)
    {
        if (draggableBlock != null) return;

        draggableBlock = _draggableBlock;
        draggableBlock.GetGridObject(this);
    }
    public void RemoveGraggableBlock()
    {
        draggableBlock=null;
    }
}
