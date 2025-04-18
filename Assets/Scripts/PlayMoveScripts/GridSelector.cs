using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectedblock";
    [SerializeField] private float yTouchOffset = 0.5f;
    [SerializeField] private float raycastDistance = 100f;
    [SerializeField] private float mergeDistanceThreshold = 0.25f;

    public static float TimeSpeed = 1; // Consider removing if unused
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

    private void Start()
    {
        layerMask = ~(1 << LayerMask.NameToLayer("Wall")); // Fixed layerMask assignment
        mainCam = Camera.main;
    }

    private void Update()
    {
        HandleSelection();
        HandleDragging();
        HandleRelease();
    }

    private void HandleSelection()
    {
        if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0) && !isDragging)
        {
            Ray ray = GetPointerRay(0);
            if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, layerMask) && hit.collider.CompareTag(selectableTag))
            {
                Time.timeScale = 0.2f;
                selectedObject = hit.collider.gameObject;
                selectedObject.transform.parent = null; // Consider if this is necessary
                draggableBlock = selectedObject.GetComponent<DraggableBlock>();
                yTouchOffset = selectedObject.GetComponent<BoxCollider>().size.z;
                ToggleCollider(selectedObject, false);
                selectedObjectTransform = selectedObject.transform;
                selectedObjectWeapon = selectedObject.GetComponent<Weapon>();
                isDragging = true;
                currentCell = null;
            }
        }
    }

    private void HandleDragging()
    {
        if (!isDragging || !(Input.GetMouseButton(0) || Input.touchCount > 0))
            return;

        Ray ray = GetPointerRay(yTouchOffset);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, layerMask))
        {
            currentCell = hit.collider.GetComponent<GridCell>();
            if (currentCell != null && draggableBlock.AllCellTouchCell())
            {
                currentCell.GetDraggableBlock(draggableBlock);
            }
            else
            {
                draggableBlock.SetGridStatus(false);
                selectedObject.transform.position = new Vector3(hit.point.x, selectedObject.transform.position.y, hit.point.z);
            }
            CheckMerge();
        }
    }

    private void HandleRelease()
    {
        if (!isDragging || !(Input.GetMouseButtonUp(0) || (Input.touchCount == 0 && Input.touchSupported)))
            return;

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

            Debug.Log("MergeDistance");

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

    private Ray GetPointerRay(float yOffset)
    {
        Ray baseRay = Input.touchCount > 0
            ? mainCam.ScreenPointToRay(Input.GetTouch(0).position)
            : mainCam.ScreenPointToRay(Input.mousePosition);

        Vector3 offsetOrigin = baseRay.origin + mainCam.transform.forward * yOffset;
        return new Ray(offsetOrigin, baseRay.direction);
    }

    private void ToggleCollider(GameObject obj, bool isEnabled)
    {
        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = isEnabled;
    }

    private void ResetSelection()
    {
        selectedObject = null;
        draggableBlock = null;
        isDragging = false;
        currentCell = null;
    }
}

public enum WeaponType
{
    Ak47,
    Pistol,
    Grenade,
    Firegun,
    Shotgun,
    Snaper
}