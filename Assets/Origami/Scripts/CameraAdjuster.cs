using UnityEngine;

public class CameraSetup : MonoBehaviour
{ // Số cột của Grid 
    public float cellSize = 1f;  // Kích thước mỗi ô trong Grid

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        int gridSize = levelManager.GetGridSize();

        // Tính toán kích thước tổng của Grid
        float gridWidth = gridSize * cellSize;
        float gridHeight = gridSize * cellSize;

        // Đặt Camera nhìn vào giữa Grid
        // Nếu grid được vẽ từ (0,0) đến (gridWidth, gridHeight)
        Vector3 gridCenter = new Vector3(0, 0, -10f);
        mainCamera.transform.position = gridCenter;

        // Tính toán và điều chỉnh Orthographic Size của Camera
        float screenRatio = (float)Screen.width / Screen.height;
        float targetSize = Mathf.Max(gridWidth / (2f * screenRatio), gridHeight / 2f);
        mainCamera.orthographicSize = targetSize;
    }
}