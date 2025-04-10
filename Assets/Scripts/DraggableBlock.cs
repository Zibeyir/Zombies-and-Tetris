using UnityEngine;

public class DraggableBlock : MonoBehaviour
{
    public GridCell[] currentGridCell=new GridCell[4];
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
        currentGridCell = new GridCell[4];
        blockPartsChecks = GetComponentsInChildren<BlockPartsCheck>();
        foreach (var part in blockPartsChecks)
            part.draggableBlock = this;
        fixedZ = transform.position.z;
        targetPosition = transform.position;
        targetBase = targetPosition;
    }

    public void SetGridCell(GridCell newCell)
    {
        if (currentGridCell[0] != null)
            currentGridCell[0].RemoveDraggableBlock();

        currentGridCell[0] = newCell;
        targetPosition = currentGridCell[0].GetComponent<Renderer>().bounds.center;
        isInGrid = true;
    }

    public void SetGridStatus(bool status)
    {
        Debug.Log("SetGridStatus");
        isInGrid = status;

    }

    private void FixedUpdate()
    {
        if (isInGrid)
        {
            target = new Vector3(targetPosition.x, targetPosition.y, fixedZ) + offset;
            transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target) < 0.03f)
            {
                if (targetBool)
                {
                    targetBool = false;
                    CheckGridStation();
                }
               
                    
            }
            else
            {
                targetBool = true;
            }
        }
    }


    public void CheckGridStation()
    {
        if (AllPartsOverValidGrids())
        {
            targetBase = targetPosition; Debug.Log("targetBase");

        }
        else
        {
            targetPosition = targetBase; Debug.Log("targetPosition");

        }
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
