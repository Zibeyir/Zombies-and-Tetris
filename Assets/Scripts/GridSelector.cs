using UnityEngine;

public class GridSelector : MonoBehaviour
{
    private Camera mainCam;
    private GameObject selectedObject = null;
    private Vector3 lastValidPosition;
    private bool isDragging = false;

    public string selectableTag = "SelectedBlock";
    [SerializeField] float yTouchDistance;

    public GridCell pastCell;
    public GridCell currentCell;
    public DraggableBlock draggableBlock;
    void Start()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && !isDragging)
        {
            Ray ray = GetPointerRay();
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag(selectableTag))
                {
                    Debug.Log("Cell False7");
                    selectedObject = hit.collider.gameObject;
                    draggableBlock=selectedObject.GetComponent<DraggableBlock>();
                    lastValidPosition = selectedObject.transform.position;
                    selectedObject.GetComponent<Collider>().enabled = false;
                    isDragging = true;

                    pastCell = null;
                    currentCell = null;
                }
            }
        }

        if (isDragging && (Input.GetMouseButton(0) || Input.touchCount > 0))
        {
            Ray ray = GetPointerRay();
            RaycastHit[] hits = Physics.RaycastAll(ray);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                currentCell = hit.collider.GetComponent<GridCell>();
                if (currentCell != null)
                {
                    
                    currentCell.GetDraggableBlock(draggableBlock);
                }
                else
                {
                    draggableBlock.ExitFromGameScene(); 
                    selectedObject.transform.position = new Vector3(hit.point.x,hit.point.y,selectedObject.transform.position.z);
                 
                }
            }
        }

        if (isDragging && (Input.GetMouseButtonUp(0) || (Input.touchCount == 0 && Input.touchSupported)))
        {
           
            selectedObject.GetComponent<Collider>().enabled = true;

            selectedObject = null;
            isDragging = false;
            
            draggableBlock = null;
        }
    }

    private Ray GetPointerRay()
    {
        if (Input.touchCount > 0)
            return mainCam.ScreenPointToRay(Input.GetTouch(0).position);
        else
            return mainCam.ScreenPointToRay(Input.mousePosition);
    }
}
