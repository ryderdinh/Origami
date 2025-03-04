using UnityEngine;

public class GridDrawer : MonoBehaviour {
    public float cellSize = 1f;
    public Color gridColor = Color.black;
    public float lineWidth = 0.05f;
    
    private int gridSize = 10;

    void Start() {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        gridSize = levelManager.GetGridSize();
        DrawCenteredGrid();
    }

    void DrawCenteredGrid() {
        float gridWidth = gridSize * cellSize;
        float gridHeight = gridSize * cellSize;

        // Vẽ các đường dọc
        for (int i = 0; i <= gridSize; i++) {
            GameObject line = new GameObject("LineVertical_" + i);
            line.transform.parent = transform;
            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.startColor = gridColor;
            lr.endColor = gridColor;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.useWorldSpace = false;
            lr.material = new Material(Shader.Find("Sprites/Default"));

            // Tọa độ cục bộ đối với parent đã có pivot tại (0,0)
            float xPos = i * cellSize - gridWidth / 2f;
            lr.SetPosition(0, new Vector3(xPos, -gridHeight / 2f, 0));
            lr.SetPosition(1, new Vector3(xPos, gridHeight / 2f, 0));
        }

        // Vẽ các đường ngang
        for (int j = 0; j <= gridSize; j++) {
            GameObject line = new GameObject("LineHorizontal_" + j);
            line.transform.parent = transform;
            LineRenderer lr = line.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.startColor = gridColor;
            lr.endColor = gridColor;
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.useWorldSpace = false;
            lr.material = new Material(Shader.Find("Sprites/Default"));

            float yPos = j * cellSize - gridHeight / 2f;
            lr.SetPosition(0, new Vector3(-gridWidth / 2f, yPos, 0));
            lr.SetPosition(1, new Vector3(gridWidth / 2f, yPos, 0));
        }
    }
}