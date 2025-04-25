using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPartsCheck : MonoBehaviour
{
    public Weapon weapon = null;
    

    private void OnEnable()
    {
    }

    public bool IsOverValidGrid()
    {
        //Vector3 rayOrigin = transform.position + rayOffset;
        //Ray ray = new Ray(rayOrigin, rayDirection);

        //if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
        //{
        //    GridCell cell = hit.collider.GetComponent<GridCell>();
        //    checkCellBool = (cell != null && cell.draggableBlock == _draggableBlock);
        //    Debug.Log($" Cell tapıldı: {cell.name}, draggableBlock: {cell.draggableBlock}");

        //    if (checkCellBool)
        //    {
        //        Debug.Log("IsOverValidGrid");
        //        //if (ownCcell != null && cell.draggableBlock != draggableBlock)
        //        //    ownCcell.RemoveDraggableBlock();

        //        ownCcell = cell;
        //            return true;
        //        }
        //        else
        //        {
        //            Debug.Log("------IsOverValidGrid");
        //            return false;

        //        }

        //}
        //Debug.Log("IsOverValidGrid   false");

        return false;
    }

    public bool CellIsFull()
    {
        //Vector3 rayOrigin = transform.position + rayOffset;
        //Ray ray = new Ray(rayOrigin, rayDirection);

        //if (Physics.Raycast(ray, out RaycastHit hit, rayLength, layerMask))
        //{
        //    return hit.collider.GetComponent<GridCell>() != null;
        //}

        return false;
    }

    // ✅ Gizmos ilə rayı göstər
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Vector3 rayOrigin = transform.position + rayOffset;
    //    Gizmos.DrawRay(rayOrigin, rayDirection.normalized * rayLength);
    //}
}
