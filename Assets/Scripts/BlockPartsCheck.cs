using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPartsCheck : MonoBehaviour
{

    public DraggableBlock draggableBlock;
    public bool IsOverValidGrid()
    {

        Ray ray = new Ray( transform.position, -Vector3.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            GridCell cell = hit.collider.GetComponent<GridCell>();
            Debug.Log(cell != null && (cell.draggableBlock == null || cell.draggableBlock == draggableBlock));

            return cell != null && (cell.draggableBlock==null||cell.draggableBlock==draggableBlock);
        }
        return false;
    }
    private void Update()
    {
        Debug.DrawRay(transform.position, -Vector3.forward*4 , Color.red);

    }

}
