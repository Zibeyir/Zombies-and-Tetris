using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private Vector2 gridOffset = Vector2.zero;
    [SerializeField] private Vector2Int gridDimensions = new Vector2Int(10, 20);
    [SerializeField] private float gridHeight = 0.1f;

    private static GridManager instance;
    public static GridManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GridManager>();
            }
            return instance;
        }
    }

    private void Start()
    {
        //CreateGridVisualization();
    }

    public Vector3 SnapToGrid(Vector3 worldPosition)
    {
        float x = Mathf.Round((worldPosition.x - gridOffset.x) / gridSize) * gridSize + gridOffset.x;
        float y = Mathf.Round((worldPosition.y - gridOffset.y) / gridSize) * gridSize + gridOffset.y;

        return new Vector3(x, y, 0);
    }

    public bool IsPositionValid(Vector3 worldPosition)
    {
        int gridX = Mathf.RoundToInt((worldPosition.x - gridOffset.x) / gridSize);
        int gridY = Mathf.RoundToInt((worldPosition.y - gridOffset.y) / gridSize);

        return gridX >= 0 && gridX < gridDimensions.x &&
               gridY >= 0 && gridY < gridDimensions.y;
    }

    private void CreateGridVisualization()
    {
        GameObject gridParent = new GameObject("GridVisualization");
        gridParent.transform.parent = transform;

        for (int x = 0; x <= gridDimensions.x; x++)
        {
            GameObject line = new GameObject($"GridLine_X_{x}");
            line.transform.parent = gridParent.transform;

            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.gray;
            lr.endColor = Color.gray;

            Vector3 start = new Vector3(x * gridSize + gridOffset.x, gridOffset.y, 0);
            Vector3 end = new Vector3(x * gridSize + gridOffset.x, gridDimensions.y * gridSize + gridOffset.y, 0);
            lr.SetPositions(new Vector3[] { start, end });
        }

        for (int y = 0; y <= gridDimensions.y; y++)
        {
            GameObject line = new GameObject($"GridLine_Y_{y}");
            line.transform.parent = gridParent.transform;

            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.gray;
            lr.endColor = Color.gray;

            Vector3 start = new Vector3(gridOffset.x, y * gridSize + gridOffset.y, 0);
            Vector3 end = new Vector3(gridDimensions.x * gridSize + gridOffset.x, y * gridSize + gridOffset.y, 0);
            lr.SetPositions(new Vector3[] { start, end });
        }
    }
}