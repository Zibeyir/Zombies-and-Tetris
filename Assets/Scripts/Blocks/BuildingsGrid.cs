using UnityEngine;

public class BuildingsGrid : MonoBehaviour
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    public float cellSize = 1f;

    private Building[,] grid;
    public Building flyingBuilding;
    private Camera mainCamera;
    public bool available;

    int x;
    int y;

    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
    }

    private void Start()
    {
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
    }

    private void Update()
    {
        if (IsInputDown())
        {
            Ray ray = mainCamera.ScreenPointToRay(GetInputPosition());
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var building = hit.collider.GetComponent<Building>();
                if (building != null)
                {
                    Debug.Log("Building clicked: " + building.name);
                    RemoveBuildingFromGrid(building);
                    flyingBuilding = building;
                    return;
                }
            }
        }

        if (flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, transform.position);
            Ray dragRay = mainCamera.ScreenPointToRay(GetInputPosition());

            if (groundPlane.Raycast(dragRay, out float distance))
            {
                Vector3 worldPosition = dragRay.GetPoint(distance);
                x = Mathf.RoundToInt((worldPosition.x - transform.position.x) / cellSize);
                y = Mathf.RoundToInt((worldPosition.z - transform.position.z) / cellSize);

                available = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) available = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) available = false;
                if (available && IsPlaceTaken(x, y)) available = false;

                flyingBuilding.transform.position = transform.position + new Vector3(x * cellSize, 0, y * cellSize);
                flyingBuilding.SetTransparent(available);

                if (IsInputUp())
                {
                    if (available)
                    {
                        Debug.Log("Building placed at: " + x + ", " + y);
                        PlaceFlyingBuilding(x, y);
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
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null) return true;
            }
        }
        return false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }

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
