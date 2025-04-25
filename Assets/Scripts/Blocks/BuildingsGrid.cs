using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    public float cellSize = 1f;

    private Building[,] grid;
    public Building flyingBuilding;
    private Camera mainCamera;
    public bool available;
    public bool firstAvailable;

    // Yeni dəyişənləri burada təyin edirik
    int x;
    int y;
    Vector2Int offset;  // Bu dəyişəni təyin edirik
    Vector3 touchOffset = new Vector3(0, 0.5f, 0); // Bu dəyişəni birbaşa təyin etdik
    int width, height, gridX, gridY;

    int xLast;
    int yLast;
    Vector2Int offsetLast;

    public LayerMask gridLayerMask; // Grid layer mask

    private Ray dragRay;
    private RaycastHit hit;
    private Vector3 worldPosition;
    private Plane groundPlane;

    // Yeni dəyişən: son yerləşmə yeri
    private Vector3 lastPosition;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
        groundPlane = new Plane(Vector3.up, transform.position); // Plane-ni bir dəfə təyin edirik
    }

    private void Start()
    {
        gridLayerMask = 1 << LayerMask.NameToLayer("Grid");

        // Başlanğıcda gridin x:2-4, y:4-5 aralığını blok kimi göstər
        for (int i = 2; i <= 5; i++)
        {
            for (int j = 0; j <= 1; j++)
            {
                grid[i, j] = new GameObject("BlockedCell").AddComponent<Building>();
            }
        }
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (flyingBuilding != null)
        {
            Destroy(flyingBuilding.gameObject);
        }

        flyingBuilding = Instantiate(buildingPrefab);

        // Başlanğıcda son yer təyin edilir
        lastPosition = flyingBuilding.transform.position;
    }

    private void Update()
    {
        if (flyingBuilding != null)
        {
            // Drag ray-ini yalnız bir dəfə təyin edirik
            dragRay = mainCamera.ScreenPointToRay(GetInputPosition());

            if (groundPlane.Raycast(dragRay, out float distance))
            {
                worldPosition = dragRay.GetPoint(distance);
                x = Mathf.RoundToInt((worldPosition.x - transform.position.x) / cellSize);
                y = Mathf.RoundToInt((worldPosition.z - transform.position.z) / cellSize);

                // True hüceyrələrinin orta nöqtəsini tapırıq
                offset = flyingBuilding.GetCenterOffset();

                // Binanın ortasında yerləşməsini təmin edirik
                available = !IsPlaceTaken(x - offset.x, y - offset.y);

                // Binanı ortalayıb yuxarıya yerləşdiririk
                flyingBuilding.transform.position = transform.position + new Vector3(
                    (x - offset.x) * cellSize,
                    .8f,
                    (y - offset.y) * cellSize
                ) + touchOffset;
                if(available)
                {
                    lastPosition = flyingBuilding.transform.position;
                    xLast = x;
                    yLast = y;
                    offsetLast = offset;
                }


                // Binanın şəffaflıq vəziyyətini təyin edirik
                flyingBuilding.SetTransparent(available);

                // Əgər sağ klik edilmişsə və yer boşdursa, binanı yerləşdiririk
                if (IsInputUp())
                {
                    Time.timeScale = 1;

                    flyingBuilding._Weapon.MoveBool = false;

                    if (available)
                    {
                        flyingBuilding.transform.position = transform.position + new Vector3(
                    (x - offset.x) * cellSize,
                    0,
                    (y - offset.y) * cellSize) + touchOffset;
                        PlaceFlyingBuilding(x - offset.x, y - offset.y);

                        // Yerləşdikdən sonra son yeri saxlayırıq
                    }
                    else
                    {
                        flyingBuilding.transform.position = lastPosition - new Vector3(0, .8f, 0);

                        if (firstAvailable)
                        {
                            // Əgər yer boş deyilsə, binanı geri qaytarırıq
                            PlaceFlyingBuilding(xLast - offsetLast.x, yLast - offsetLast.y);

                        }

                        // Əgər yer boşdursa, binanı geri qaytarırıq
                        flyingBuilding = null;
                    }
                    firstAvailable = false;

                }
            }
        }
        else
        {
            if (IsInputDown())
            {
                dragRay = mainCamera.ScreenPointToRay(GetInputPosition());

                if (Physics.Raycast(dragRay, out hit, Mathf.Infinity, gridLayerMask))
                {
                    var building = hit.collider.GetComponent<Building>();
                    if (building != null)
                    {
                        Time.timeScale = 0.4f;
                        firstAvailable = true;
                        RemoveBuildingFromGrid(building);
                        flyingBuilding = building;
                        flyingBuilding._Weapon.MoveBool = true;
                        lastPosition = flyingBuilding.transform.position+ new Vector3(0, .8f, 0);
                        return;
                    }
                }
            }
        }
    }

    private void RemoveBuildingFromGrid(Building building)
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                if (grid[x, y] == building)
                {
                    grid[x, y] = null;
                }
            }
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        width = flyingBuilding.Width;
        height = flyingBuilding.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!flyingBuilding.Shape[y, x]) continue;

                gridX = placeX + x;
                gridY = placeY + y;

                if (gridX < 0 || gridX >= GridSize.x || gridY < 0 || gridY >= GridSize.y)
                    return true;

                if (grid[gridX, gridY] != null)
                    return true;
            }
        }
        return false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        width = flyingBuilding.Width;
        height = flyingBuilding.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (!flyingBuilding.Shape[y, x]) continue;

                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }
        //flyingBuilding._Weapon.MoveBool = false;

        flyingBuilding.SetNormal();
        flyingBuilding = null;
    }

    private bool IsInputDown()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButtonDown(0);
#elif UNITY_ANDROID || UNITY_IOS
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
#else
        return false;
#endif
    }

    private bool IsInputUp()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.GetMouseButtonUp(0);
#elif UNITY_ANDROID || UNITY_IOS
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended;
#else
        return false;
#endif
    }

    private Vector3 GetInputPosition()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        return Input.mousePosition;
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
            return Input.GetTouch(0).position;
        else
            return Vector3.negativeInfinity;
#else
        return Vector3.negativeInfinity;
#endif
    }

    private void OnDrawGizmos()
    {
        if (grid == null) grid = new Building[GridSize.x, GridSize.y];

        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                Vector3 center = transform.position + new Vector3(x * cellSize, 0, y * cellSize);
                Gizmos.color = grid[x, y] == null ? Color.gray : Color.red;
                Gizmos.DrawWireCube(center, Vector3.one * cellSize);
            }
        }
    }
}
