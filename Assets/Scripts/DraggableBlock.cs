using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    public GridCell currentGridCell;
    public bool isInGrid = false;
    public float moveSpeed = 25f;

    private Vector3 targetPosition;
    private Vector3 target;
    private Vector3 targetBase;
    public bool targetBool;
    [SerializeField] Vector3 offset;
    private float fixedZ;

    public BlockPartsCheck[] blockPartsChecks;
    private void Start()
    {
        targetBool = true;
       
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
        {
            Debug.Log("Removed");

            currentGridCell.RemoveDraggableBlock();
        }
        currentGridCell = newCell;
        targetPosition = currentGridCell.GetComponent<Renderer>().bounds.center;
        isInGrid = true;
    }

    public void SetGridStatus(bool status)
    {
        isInGrid = status;

    }

    private void FixedUpdate()
    {
        if (isInGrid)
        {
            target = new Vector3(targetPosition.x, targetPosition.y, fixedZ) + offset;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            Debug.Log(Vector3.Distance(transform.position, target));

            if (Vector3.Distance(transform.position, target) < 0.005f)
            {
                Debug.Log(Vector3.Distance(transform.position, target));
                if (targetBool)
                {
                    targetBool = false;
                    CheckGridStation();
                }
                else
                {
                    targetBool = true;
                }
               
                    
            }
           
        }
    }


    public void CheckGridStation()
    {
        if (AllPartsOverValidGrids())
        {
            if (targetBase != targetPosition)
            {
                targetBase = targetPosition;
                targetBool = true;
            }
        }
        else
        {
            if (targetPosition != targetBase)
            {
                targetPosition = targetBase;
                targetBool = true;
            }
        }
    }


    public bool AllCellTouchCell()
    {
        foreach (var checker in blockPartsChecks)
        {
            if (!checker.CellIsFull()) {
                Debug.Log("false");
                return false;
            }
        }
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
