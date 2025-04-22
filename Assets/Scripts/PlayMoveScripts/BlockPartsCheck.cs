using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPartsCheck : MonoBehaviour
{

    public DraggableBlock draggableBlock=null;
    public Vector3 boxHalfExtents;
    public Vector3 boxOffset = Vector3.zero;
    public GridCell ownCcell=null;
    public GridCell PastCcell=null;
    public LayerMask layerMask;

    public bool checkCellBool=false;
    float sizeBoxCollider;
    private void OnEnable()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Wall"));

        sizeBoxCollider = 0.2f;
        boxHalfExtents = new Vector3(sizeBoxCollider, sizeBoxCollider, sizeBoxCollider);
    }
   
    public bool IsOverValidGrid()
    {
        Vector3 boxCenter = transform.position + boxOffset;
        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.identity, layerMask);

        foreach (var hitz in hits)
        {
            GridCell cell = hitz.GetComponent<GridCell>();
            checkCellBool = (cell != null && (cell.draggableBlock == null || cell.draggableBlock == draggableBlock));
            if (checkCellBool)
            {
                if (ownCcell != null) ownCcell.RemoveDraggableBlock();
                ownCcell = cell;
                 
            }
            return checkCellBool;

        }

        return false;
       
    }

    public bool CellIsFull()
    {
        Vector3 boxCenter = transform.position + boxOffset;
        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfExtents, Quaternion.identity, layerMask);

        foreach (var hit in hits)
        {
            if (hit.GetComponent<GridCell>() != null)
                return true;
        }

        return false;
    }



}
