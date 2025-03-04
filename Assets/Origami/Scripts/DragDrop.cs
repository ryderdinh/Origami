using UnityEngine;

public class DragDropSnap : MonoBehaviour
{
    public float cellSize = 1f;             // Phải khớp với cellSize của GridManager
    public float snapOverlapScale = 1.0f;     // Có thể dùng để điều chỉnh vùng kiểm tra (nếu cần)
    
    private Vector3 offset;
    private float zCoord;
    private Vector3 initialPosition;        // Vị trí ban đầu trước khi kéo

    void Start()
    {
        // Đăng ký vị trí ban đầu trong grid
        Vector2Int cell = GridManager.Instance.GetCellFromPosition(transform.position);
        GridManager.Instance.OccupyCell(cell, gameObject);
        Debug.Log("Start: initial position = " + transform.position + " | Cell = " + cell);
    }

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
        initialPosition = transform.position;
        Debug.Log("OnMouseDown: initialPosition = " + initialPosition + " | offset = " + offset);

        // Giải phóng ô hiện tại để không tự phát hiện chồng lấn khi kiểm tra
        Vector2Int currentCell = GridManager.Instance.GetCellFromPosition(transform.position);
        GridManager.Instance.VacateCell(currentCell);
        Debug.Log("OnMouseDown: Vacated cell = " + currentCell);
    }

    void OnMouseDrag()
    {
        Vector3 newPos = GetMouseWorldPos() + offset;
        transform.position = newPos;
        // Debug.Log("OnMouseDrag: new position = " + newPos);

        // Tính toán vị trí snap dự kiến khi thả
        Vector3 predictedSnapPos = GetPredictedSnapPosition(newPos);
        Vector2Int predictedCell = GridManager.Instance.GetCellFromPosition(predictedSnapPos);
        bool canSnap = !GridManager.Instance.IsCellOccupied(predictedCell);
        Debug.Log("Predicted snap position = " + predictedSnapPos + " | Predicted cell = " + predictedCell + " | Can snap? " + canSnap);
    }

    void OnMouseUp()
    {
        Debug.Log("OnMouseUp: position before snap = " + transform.position);
        SnapToGrid();
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    
    // Tính toán vị trí snap dự kiến dựa trên vị trí hiện tại
    Vector3 GetPredictedSnapPosition(Vector3 currentPos)
    {
        float offsetValue = cellSize / 2f; // Điều chỉnh nếu cần căn chỉnh cạnh
        float snappedX = Mathf.Round((currentPos.x - offsetValue) / cellSize) * cellSize + offsetValue;
        float snappedY = Mathf.Round((currentPos.y - offsetValue) / cellSize) * cellSize + offsetValue;
        return new Vector3(snappedX, snappedY, currentPos.z);
    }

    void SnapToGrid()
    {
        Vector3 pos = transform.position;
        float offsetValue = cellSize / 2f;
        float snappedX = Mathf.Round((pos.x - offsetValue) / cellSize) * cellSize + offsetValue;
        float snappedY = Mathf.Round((pos.y - offsetValue) / cellSize) * cellSize + offsetValue;
        Vector3 snappedPos = new Vector3(snappedX, snappedY, pos.z);
        // Debug.Log("SnapToGrid: calculated snappedPos = " + snappedPos);

        // Tính toán ô đích dựa trên vị trí snap
        Vector2Int targetCell = GridManager.Instance.GetCellFromPosition(snappedPos);
        // Debug.Log("SnapToGrid: targetCell = " + targetCell);

        bool occupied = GridManager.Instance.IsCellOccupied(targetCell);
        // Debug.Log("SnapToGrid: Is targetCell occupied? " + occupied);

        if (occupied)
        {
            Debug.Log("SnapToGrid: Cell occupied, reverting to initialPosition = " + initialPosition);
            // Nếu ô đã bị chiếm, revert về vị trí ban đầu và đánh dấu lại ô ban đầu
            transform.position = initialPosition;
            Vector2Int initialCell = GridManager.Instance.GetCellFromPosition(initialPosition);
            GridManager.Instance.OccupyCell(initialCell, gameObject);
        }
        else
        {
            Debug.Log("SnapToGrid: Snap successful to cell = " + targetCell);
            // Snap thành công, đặt vị trí mới và đánh dấu ô target
            transform.position = snappedPos;
            GridManager.Instance.OccupyCell(targetCell, gameObject);
        }
    }
}