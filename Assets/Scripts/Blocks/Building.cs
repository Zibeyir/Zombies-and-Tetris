using UnityEngine;

public class Building : MonoBehaviour
{
    // Inspector'da göstəriləcək hüceyrə vəziyyətləri
    public bool Cell00 = false;
    public bool Cell01 = false;
    public bool Cell02 = false;
    public bool Cell10 = false;
    public bool Cell11 = false;
    public bool Cell12 = false;

    public int x;
    public int y;
    public bool[,] Shape;

    public bool ActiveCell;

    public int Width;
    public int Height;
    public Vector3 Vector3offset;

    public DraggableBlock _DraggableBlock;
    public Weapon _Weapon;
    private void OnEnable()
    {
        UpdateShape();
        Height = Shape.GetLength(0); // Yəni satır sayını
        Width = Shape.GetLength(1);  // Sütun sayını
        ActiveCell = false;
        PrintShape();

        //Weapon weapon = gameObject.GetComponentInChildren<Weapon>(); // DraggableBlock'a Building'i təyin edirik
        if (_Weapon != null)
        {
            if (_Weapon._WeaponType == WeaponType.Shotgun)
            {
                transform.position -= new Vector3(0.8f, 0, 0); // Yalnız Shotgun üçün
            }
        }
    }

    private void UpdateShape()
    {
        // Dinamik olaraq Shape'ı yeniləyirik
        Shape = new bool[2, 3] // [y, x]
        {
            { Cell00, Cell01 ,Cell11},
            { Cell02, Cell10 ,Cell12}
        };
    }
    public Vector2Int GetCenterOffset()
    {
        int width = Shape.GetLength(1);
        int height = Shape.GetLength(0);

        Vector2 total = Vector2.zero;
        int count = 0;

        // "True" olan hüceyrələri tapırıq və orta nöqtəni hesablayırıq
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Shape[y, x])
                {
                    total += new Vector2(x, y);
                    count++;
                }
            }
        }

        if (count == 0)
            return Vector2Int.zero;

        Vector2 average = total / count;
        return new Vector2Int(Mathf.RoundToInt(average.x), Mathf.RoundToInt(average.y));
    }
    public void PrintShape()
    {
        // Hər bir elementini çap edirik
        for (int y = 0; y < Height; y++)
        {
            string row = "";  // Satırı toplayırıq
            for (int x = 0; x < Width; x++)
            {
                row += Shape[y, x] ? "1 " : "0 "; // True/False dəyərləri çap edirik
            }
        }
    }

    public void SetTransparent(bool available)
    {
        if (available)
        {
            ActiveCell = true;
        }
        _Weapon.MoveBoolGreen = available;
    }

    public void SetNormal()
    {
        // Use this to set visuals if needed
    }

    private void Update()
    {
        // Real-time updates: we update the shape whenever a change occurs
        UpdateShape();  // Make sure to always have the latest shape
    }

    private void OnDrawGizmos()
    {
        if (Shape == null) return;

        // Gizmos üçün doğru indekləmə: [y, x] - [satır, sütun]
        for (int y = 0; y < Height; y++) // Y istiqamətində (satır)
        {
            for (int x = 0; x < Width; x++) // X istiqamətində (sütun)
            {
                if (!Shape[y, x]) continue; // Yalnız true olan hüceyrələri göstəririk

                // Gizmos ilə bu hüceyrə göstərilir
                Gizmos.color = ((x + y) % 2 == 0) ? new Color(0.88f, 0f, 1f, 0.3f) : new Color(1f, 0.68f, 0f, 0.3f);
                Gizmos.DrawCube(transform.position + new Vector3(x, 0, y), new Vector3(1, 0.1f, 1)); // Hüceyrəni göstərmək
            }
        }
    }
}
