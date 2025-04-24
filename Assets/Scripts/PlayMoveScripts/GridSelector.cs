using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectedblock";
    [SerializeField] private float yTouchOffset = 0.5f;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private float mergeDistanceThreshold = 0.25f;

    public static float TimeSpeed = 1;
    private Camera mainCam;
    private GameObject selectedObject;
    private DraggableBlock draggableBlock;
    private GridCell currentCell;
    private bool isDragging = false;
    public LayerMask layerMask;

    public List<Transform> blocks = new List<Transform>();
    public List<Weapon> weapons = new List<Weapon>();
    private Transform selectedObjectTransform = null;
    private Weapon selectedObjectWeapon = null;
    private Vector3 selectedPos;
    private WeaponType selectedType;
    private int selectedLevel;
    private float sqrDist;

    public LayerMask wallLayer;
    public LayerMask selectionMask;
    [SerializeField] private float sphereRadius = 0.05f; // inspector'dan idarə oluna bilən radius
    private Vector3 lastHitPoint; // overlap mərkəzini yadda saxlamaq üçün
    private void Start()
    {
        mainCam = Camera.main;
        wallLayer = 1 << LayerMask.NameToLayer("Plane");
        selectionMask = Physics.DefaultRaycastLayers; // Bütün layere atmaq üçün
    }

    private void Update()
    {
        //HandleSelection();
        //HandleDragging();

        //HandleRelease();
    }
  
    private void HandleSelection()
    {
        if (!isDragging && IsPointerDown())
        {
            if (TryGetPointerPosition(out Vector3 screenPos))
            {
                Ray ray = GetPointerRay(screenPos, 0f);

                // Yalnız Wall layer-ə baxırıq
                if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, selectionMask))
                {
                    if (hit.collider.CompareTag(selectableTag))
                    {
                        SelectObject(hit.collider.gameObject);
                        return;
                    }
                }
                lastHitPoint = hit.point;
                // Əgər ray heç nə tapmasa, dairə ilə yoxla (bütün layere)
                //Vector3 origin;
                //if (mainCam != null)
                //    origin = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 1.0f));
                //else
                //    origin = screenPos;

                Collider[] colliders = Physics.OverlapSphere(hit.point, sphereRadius, layerMask);
                foreach (var col in colliders)
                {
                    if (col.CompareTag(selectableTag))
                    {
                        SelectObject(col.gameObject);
                        return;
                    }
                }
            }
        }
    }

    private void SelectObject(GameObject obj)
    {
        Time.timeScale = 0.3f;

        selectedObject = obj;
        selectedObject.transform.parent = null;

        draggableBlock = selectedObject.GetComponent<DraggableBlock>();
        yTouchOffset = selectedObject.GetComponent<BoxCollider>().size.z;

        ToggleCollider(selectedObject, false);

        selectedObjectTransform = selectedObject.transform;
        selectedObjectWeapon = selectedObject.GetComponent<Weapon>();
        isDragging = true;
        currentCell = null;
    }
   

    private void HandleDragging()
    {
        if (!isDragging || !IsPointerHeld())
            return;

        if (TryGetPointerPosition(out Vector3 screenPos))
        {
            Ray ray = GetPointerRay(screenPos, yTouchOffset);

            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, wallLayer))
            {
                lastHitPoint = hit.point; // gizmo çəkiliş nöqtəsini yadda saxla

                Collider[] colliders = Physics.OverlapSphere(hit.point, sphereRadius, selectionMask);

                GridCell foundCell = null;
                foreach (var col in colliders)
                {
                    if (col.CompareTag("Cell"))
                    {
                        foundCell = col.GetComponent<GridCell>();
                        break;
                    }
                }

                if (foundCell != null  )
                {
                    //Debug.Log("foundCell != null");
                    //currentCell = foundCell;
                    //draggableBlock.SetGridCell(currentCell);
                    //currentCell.GetDraggableBlock(draggableBlock);
                }
                else
                {
                    //Debug.Log("foundCell != true");

                    //draggableBlock.SetGridStatus(false);
                    //selectedObject.transform.position = new Vector3(hit.point.x, selectedObject.transform.position.y, hit.point.z);
                }

                CheckMerge();
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (isDragging && selectedObject != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(lastHitPoint, sphereRadius);
        }
    }


    private void HandleRelease()
    {
        if (!isDragging || !IsPointerReleased())
            return;
        Debug.Log("HandleRelease");
        Time.timeScale = 1;

        draggableBlock.SetGridStatus(true);

        if (selectedObject != null)
            ToggleCollider(selectedObject, true);

        ResetSelection();
    }

    private void CheckMerge()
    {
        blocks = _GameTimeData.Instance.CurrentBlocks;
        weapons = _GameTimeData.Instance.CurrentBlocksWeapons;

        selectedPos = selectedObjectTransform.position;
        selectedType = selectedObjectWeapon._WeaponType;
        selectedLevel = selectedObjectWeapon.WeaponLevel;

        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == selectedObjectTransform)
                continue;

            sqrDist = Vector3.SqrMagnitude(blocks[i].position - selectedPos);
            if (sqrDist > mergeDistanceThreshold)
                continue;

            if (weapons[i]._WeaponType != selectedType || weapons[i].WeaponLevel != selectedLevel)
                continue;

            selectedObjectWeapon.MergeBlockAndDestroy();
            weapons[i].MergeBlockAndLevelUp();

            isDragging = false;

            weapons.Remove(selectedObjectWeapon);
            blocks.Remove(selectedObjectTransform);
            _GameTimeData.Instance.ActiveButtonBlocks.Remove(selectedObjectTransform);

            Time.timeScale = 1;
            break;
        }
    }

    private void ToggleCollider(GameObject obj, bool isEnabled)
    {
        Collider[] collider = obj.GetComponents<Collider>();
        foreach (var col in collider)
        {
           // if (col != null)
              //  col.isTrigger = !isEnabled;
        }   
    }

    private void ResetSelection()
    {
        selectedObject = null;
        draggableBlock = null;
        isDragging = false;
        currentCell = null;
    }

    private bool TryGetPointerPosition(out Vector3 screenPosition)
    {
        if (Input.touchCount > 0)
        {
            screenPosition = Input.GetTouch(0).position;
            return true;
        }
        else if (Input.GetMouseButton(0))
        {
            screenPosition = Input.mousePosition;
            return true;
        }

        screenPosition = Vector3.zero;
        return false;
    }

    private Ray GetPointerRay(Vector3 screenPosition, float yOffset)
    {
        Ray baseRay = mainCam.ScreenPointToRay(screenPosition);
        Vector3 offsetOrigin = baseRay.origin + mainCam.transform.up * yOffset;
        return new Ray(offsetOrigin, baseRay.direction);
    }

    private bool IsPointerDown()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0);
    }

    private bool IsPointerHeld()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0);
    }

    private bool IsPointerReleased()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetMouseButtonUp(0);
    }
}

public enum WeaponType
{
    Ak,
    Pistol,
    Grenade,
    Firegun,
    Shotgun,
    Snaper
}
