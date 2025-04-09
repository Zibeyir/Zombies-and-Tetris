using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class DraggableBlock : MonoBehaviour
{
    public GridCell gridCell;
    public bool gridCellEnter=false;
    public DraggableBlock draggableBlock;
    public Transform gridCellPosition;
    public float timeMove=5;
    float positionZ;

    Vector3 targetPosition;
    private void Start()
    {
        positionZ=transform.position.z;
    }
    public void GetGridObject(GridCell _gridCell)
    {
        if(gridCell!=null) gridCell.RemoveGraggableBlock();

        gridCell = _gridCell;
        gridCellPosition = _gridCell.gameObject.transform;
        gridCellEnter = true;
        targetPosition = gridCellPosition.GetComponent<Renderer>().bounds.center;
    }
    public void ExitFromGameScene()
    {
        gridCellEnter = false;
    }
    private void FixedUpdate()
    {
        if (gridCellEnter) {

            transform.position = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetPosition.y, positionZ), 50*Time.deltaTime);
        
        }
    }

}