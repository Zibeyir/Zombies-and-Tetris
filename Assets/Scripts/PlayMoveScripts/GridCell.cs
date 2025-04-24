using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public DraggableBlock draggableBlock=null;
    public bool BlockBool= false;
    public void GetDraggableBlock(DraggableBlock _draggableBlock)
    {
       // if (draggableBlock != null) return;

       //// draggableBlock = _draggableBlock;
       // BlockBool = true;
       // //draggableBlock.SetGridCell(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Selectedblock"))
        {
            DraggableBlock _draggableBlock = other.GetComponent<DraggableBlock>();
            if (_draggableBlock != null)
            {
                if (draggableBlock != null) return;
                draggableBlock = _draggableBlock;
                BlockBool = true;
               // draggableBlock.SetGridCell(this);
                //GetDraggableBlock(_draggableBlock);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Selectedblock"))
        {
            DraggableBlock _draggableBlock = other.GetComponent<DraggableBlock>();
            if (_draggableBlock != null&&_draggableBlock== draggableBlock)
            {
                draggableBlock = null;
                BlockBool = false;
            }
        }
    }
    public void GetDraggableBlockfromParts(DraggableBlock _draggableBlock)
    {
        
        //draggableBlock = _draggableBlock;
        //BlockBool = true;
    }
    public void RemoveDraggableBlock()
    {
        //draggableBlock=null;
        //BlockBool = false;

    }
}
