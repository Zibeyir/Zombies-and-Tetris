using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{
    public DraggableBlock draggableBlock=null;
    public bool BlockBool= false;

    Material cellMaterial;
    Renderer cellRenderer;
    BlockPartsCheck blockPartsCheck;
    BlockPartsCheck _blockPartsCheck;
    Color cellColor;
    private void Start()
    {
        cellRenderer = GetComponent<Renderer>();
        cellMaterial = cellRenderer.material;
        cellColor = cellMaterial.color;
    }
    public void GetDraggableBlock(DraggableBlock _draggableBlock)
    {
       // if (draggableBlock != null) return;

       //// draggableBlock = _draggableBlock;
       // BlockBool = true;
       // //draggableBlock.SetGridCell(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Blockpart"))
        {
            blockPartsCheck = other.GetComponent<BlockPartsCheck>();
            if (blockPartsCheck != null)
            {Debug.Log(this.name+" Blockpart: " + blockPartsCheck.weapon.name);
                cellRenderer.material.color = blockPartsCheck.weapon.MoveBoolGreen ? Color.green : Color.red;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Blockpart"))
        {
            BlockPartsCheck _blockPartsCheck = other.GetComponent<BlockPartsCheck>();
            if (_blockPartsCheck != null&& _blockPartsCheck==blockPartsCheck)
            {
                cellRenderer.material.color = cellColor;
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
