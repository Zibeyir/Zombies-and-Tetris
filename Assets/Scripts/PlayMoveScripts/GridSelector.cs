using System.Collections.Generic;
using UnityEngine;

public class GridSelector : MonoBehaviour
{
    [SerializeField] private string selectableTag = "SelectedBlock";
    [SerializeField] private float yTouchOffset = 0.5f;
  
    public static float TimeSpeed = 1;
    private Camera mainCam;
    private GameObject selectedObject;
    private DraggableBlock draggableBlock;
    private GridCell currentCell;
    private bool isDragging = false;
    public LayerMask layerMask;


    public List<Transform> blocks = new List<Transform>();
    public List<Weapon> weapons = new List<Weapon>();
    Transform seledcetObjectTransform=null;
    Weapon seledcetObjectWeapon = null;
    Vector3 selectedPos;
    WeaponType selectedType;
    int selectedLevel;
    float sqrDist;


    private void Start()
    {
        int layerMask = ~1 << LayerMask.NameToLayer("Wall");
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
            if (Physics.Raycast(ray, out RaycastHit hit,100,layerMask) && hit.collider.CompareTag(selectableTag))
            {
                Time.timeScale = .2f;
                //TimeSpeed = .2f;
                selectedObject = hit.collider.gameObject;
                selectedObject.transform.parent = null;
                draggableBlock = selectedObject.GetComponent<DraggableBlock>();
                yTouchOffset = selectedObject.GetComponent<BoxCollider>().size.z;
                selectedObject.GetComponent<Collider>().enabled = false;
                seledcetObjectTransform = selectedObject.transform;
                seledcetObjectWeapon = selectedObject.GetComponent<Weapon>();
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

        if (Physics.Raycast(ray, out RaycastHit hit, 100, layerMask))
        {
            currentCell = hit.collider.GetComponent<GridCell>();
            if (currentCell != null&&draggableBlock.AllCellTouchCell())
            {
                //Debug.Log("DontMove");

                currentCell.GetDraggableBlock(draggableBlock);
            }
            else
            {
                //Debug.Log("Move");
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
            selectedObject.GetComponent<Collider>().enabled = true;

        selectedObject = null;
        draggableBlock = null;
        isDragging = false;
        currentCell = null;
    }
    void CheckMerge()
    {
        blocks = _GameTimeData.Instance.CurrentBlocks;
        weapons = _GameTimeData.Instance.CurrentBlocksWeapons;

        selectedPos = seledcetObjectTransform.position;
        selectedType = seledcetObjectWeapon._WeaponType;
        selectedLevel = seledcetObjectWeapon.WeaponLevel;

        for (int i = 0; i < blocks.Count; i++)
        {
            if (blocks[i] == seledcetObjectTransform)
                continue;

            sqrDist = Vector3.SqrMagnitude(blocks[i].position - selectedPos);
            if (sqrDist > 0.25f)
                continue;

            if (weapons[i]._WeaponType != selectedType || weapons[i].WeaponLevel != selectedLevel)
                continue;

            Debug.Log("MergeDistance");

            seledcetObjectWeapon.MergeBlockAndDestroy();
            weapons[i].MergeBlockAndLevelUp();
            isDragging = false;

            weapons.Remove(seledcetObjectWeapon);
            blocks.Remove(seledcetObjectTransform);
            _GameTimeData.Instance.ActiveButtonBlocks.Remove(seledcetObjectTransform);
            Time.timeScale = 1;

            break;
        }
    }

    public void AddBlocks(Transform transform, Weapon draggableBlock)
    {
        //transformsBlocks.Add(transform);
        //WeaponBlocksforMerge.Add(draggableBlock);
    }
    private Ray GetPointerRay(float yOffset)
    {
        Ray baseRay = Input.touchCount > 0
            ? mainCam.ScreenPointToRay(Input.GetTouch(0).position)
            : mainCam.ScreenPointToRay(Input.mousePosition);

        Vector3 offsetOrigin = baseRay.origin + mainCam.transform.forward * yOffset;
       

        return new Ray(offsetOrigin, baseRay.direction);
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