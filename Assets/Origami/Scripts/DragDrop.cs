using UnityEngine;

public class DragDropSnap : MonoBehaviour
{
    public float cellSize = 1f; // Phải khớp với cellSize của GridManager
    private Vector3 offset;
    private float zCoord;
    private Vector3 initialPosition; // Vị trí ban đầu trước khi kéo

    void Start()
    {
        // Đăng ký vị trí ban đầu trong grid
        Vector2Int cell = GridManager.Instance.GetCellFromPosition(transform.position);
        GridManager.Instance.OccupyCell(cell, gameObject);
    }

    void OnMouseDown()
    {
        zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
        offset = transform.position - GetMouseWorldPos();
        initialPosition = transform.position;
        // Khi bắt đầu kéo, giải phóng ô mà piece đang chiếm để không tự phát hiện chồng lấn
        Vector2Int currentCell = GridManager.Instance.GetCellFromPosition(transform.position);
        GridManager.Instance.VacateCell(currentCell);
    }

    void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + offset;
    }

    void OnMouseUp()
    {
        SnapToGrid();
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void SnapToGrid()
    {
        Vector3 pos = transform.position;
        float offsetValue = cellSize / 2f; // Nếu bạn muốn căn chỉnh cạnh (chứ không phải trung tâm) của piece
        float snappedX = Mathf.Round((pos.x - offsetValue) / cellSize) * cellSize + offsetValue;
        float snappedY = Mathf.Round((pos.y - offsetValue) / cellSize) * cellSize + offsetValue;
        Vector3 snappedPos = new Vector3(snappedX, snappedY, pos.z);
        
        // Tính toán ô đích dựa trên vị trí snap
        Vector2Int targetCell = GridManager.Instance.GetCellFromPosition(snappedPos);

        // Kiểm tra xem ô target đã có piece khác chưa
        if (GridManager.Instance.IsCellOccupied(targetCell))
        {
            // Nếu ô đã bị chiếm, revert về vị trí ban đầu và đánh dấu lại ô ban đầu
            transform.position = initialPosition;
            Vector2Int initialCell = GridManager.Instance.GetCellFromPosition(initialPosition);
            GridManager.Instance.OccupyCell(initialCell, gameObject);
        }
        else
        {
            // Snap thành công, đặt vị trí mới và đánh dấu ô target
            transform.position = snappedPos;
            GridManager.Instance.OccupyCell(targetCell, gameObject);
        }
    }
}