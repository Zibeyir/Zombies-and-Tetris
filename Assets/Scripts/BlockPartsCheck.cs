using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPartsCheck : MonoBehaviour
{

    public DraggableBlock draggableBlock;
    public Vector3 boxHalfExtents;
    public Vector3 boxOffset = Vector3.zero;
    private void OnEnable()
    {
        boxHalfExtents = new Vector3(0.3f, 0.3f, 0.3f);
    }
    //public bool IsOverValidGrid()
    //{

    //    Ray ray = new Ray( transform.position, -Vector3.forward);
    //    if (Physics.Raycast(ray, out RaycastHit hit, 20f))
    //    {
    //        GridCell cell = hit.collider.GetComponent<GridCell>();
    //        Debug.Log(cell != null && (cell.draggableBlock == null || cell.draggableBlock == draggableBlock));

    //        return cell != null && (cell.draggableBlock==null||cell.draggableBlock==draggableBlock);
    //    }
    //    return false;
    //}
    public bool IsOverValidGrid()
    {
        Vector3 boxCenter = transform.position + boxOffset;
        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfExtents);

        foreach (var hitz in hits)
        {
            GridCell cell = hitz.GetComponent<GridCell>();

            return cell != null && (cell.draggableBlock == null || cell.draggableBlock == draggableBlock);

        }

        return false;
       
    }

    public bool CellIsFull()
    {
        Vector3 boxCenter = transform.position + boxOffset;
        Collider[] hits = Physics.OverlapBox(boxCenter, boxHalfExtents);

        foreach (var hit in hits)
        {
            if (hit.GetComponent<GridCell>() != null)
                return true;
        }

        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 boxCenter = transform.position + boxOffset;
        Gizmos.DrawWireCube(boxCenter, boxHalfExtents * 2f); // 2x çünki halfExtents-dir
    }

    private void Update()
    {
        Debug.DrawRay(transform.position, -Vector3.forward*4 , Color.red);

    }

}
