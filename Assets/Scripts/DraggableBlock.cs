using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggableBlock : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private BlockType blockType;
    [SerializeField] private Image blockImage;
    [SerializeField] private Text coinValueText;

    private Camera mainCamera;
    private GameObject previewBlock;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    private const float Z_POSITION = 0f; // Keep all objects at the same Z position for 2D gameplay

    private void Awake()
    {
        mainCamera = Camera.main;
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Initialize UI if blockType is already set
        if (blockType != null)
        {
            UpdateVisuals();
        }
    }

    public void SetBlockType(BlockType type)
    {
        blockType = type;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (blockType != null)
        {
            if (blockImage != null && blockType.uiIcon != null)
            {
                blockImage.sprite = blockType.uiIcon;
            }
            if (coinValueText != null)
            {
                coinValueText.text = blockType.coinValue.ToString();
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin drag");
        if (!GameManager.Instance.IsGameActive() || blockType == null) return;

        originalPosition = transform.position;

        // Create preview block
        if (blockType.prefab != null)
        {
            previewBlock = Instantiate(blockType.prefab);
            previewBlock.GetComponent<Block>().enabled = false; // Disable block behavior during preview

            // Make it semi-transparent
            foreach (Renderer renderer in previewBlock.GetComponentsInChildren<Renderer>())
            {
                Material previewMaterial = new Material(renderer.material);
                Color color = previewMaterial.color;
                color.a = 0.5f;
                previewMaterial.color = color;
                renderer.material = previewMaterial;
            }

            // Disable physics during drag
            if (previewBlock.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.isKinematic = true;
            }
        }

        // Make UI element semi-transparent during drag
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (previewBlock == null) return;

        // Convert mouse position to world position
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(
            new Vector3(eventData.position.x, eventData.position.y, -mainCamera.transform.position.z)
        );
        mouseWorldPosition.z = Z_POSITION;

        // Snap to grid
        Vector3 snappedPosition = GridManager.Instance.SnapToGrid(mouseWorldPosition);
        snappedPosition.z = Z_POSITION;

        // Update preview position
        if (GridManager.Instance.IsPositionValid(snappedPosition))
        {
            previewBlock.transform.position = snappedPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;

        if (previewBlock != null)
        {
            Vector3 finalPosition = previewBlock.transform.position;
            finalPosition.z = Z_POSITION;

            if (GridManager.Instance.IsPositionValid(finalPosition))
            {
                // Check for overlapping blocks
                Collider[] colliders = Physics.OverlapBox(
                    finalPosition,
                    previewBlock.GetComponent<BoxCollider>().size / 2,
                    Quaternion.identity
                );

                bool canPlace = colliders.Length <= 1; // Only the preview block itself

                if (canPlace)
                {
                    // Create actual block
                    GameObject actualBlock = Instantiate(blockType.prefab, finalPosition, Quaternion.identity);

                    // Enable physics
                    if (actualBlock.TryGetComponent<Rigidbody>(out var rb))
                    {
                        rb.isKinematic = false;
                    }
                }
            }

            // Clean up preview
            Destroy(previewBlock);
        }
    }
}