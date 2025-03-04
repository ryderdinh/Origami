using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance; // Singleton để dễ truy cập
    public float cellSize = 1f;         // Kích thước mỗi ô
    // Dictionary theo dạng: key là tọa độ ô, value là GameObject chiếm ô đó
    private Dictionary<Vector2Int, GameObject> gridOccupancy = new Dictionary<Vector2Int, GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Chuyển đổi vị trí thế giới sang tọa độ ô
    public Vector2Int GetCellFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / cellSize);
        int y = Mathf.RoundToInt(position.y / cellSize);
        return new Vector2Int(x, y);
    }

    // Kiểm tra ô đã có piece hay chưa
    public bool IsCellOccupied(Vector2Int cell)
    {
        return gridOccupancy.ContainsKey(cell);
    }

    // Đánh dấu ô được chiếm bởi piece
    public void OccupyCell(Vector2Int cell, GameObject piece)
    {
        gridOccupancy[cell] = piece;
    }

    // Giải phóng ô
    public void VacateCell(Vector2Int cell)
    {
        if (gridOccupancy.ContainsKey(cell))
        {
            gridOccupancy.Remove(cell);
        }
    }
}