using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    public TextAsset jsonFile;            // Gán file JSON trong Inspector
    public GameObject puzzlePiecePrefab;  // Prefab của Puzzle Piece
    public Transform piecesParent;        // Khu vực chứa các mảnh ghép (PuzzlePiecesPanel)

    private LevelData level;
    
    void Awake() {
        // Parse file JSON thành đối tượng LevelData
        level = JsonUtility.FromJson<LevelData>(jsonFile.text);
    }

    // Hàm trả về grid size từ dữ liệu level
    public int GetGridSize() {
        return level.grid_size;
    }
    void Start() {
        // Duyệt qua từng polygon trong level
        foreach (var poly in level.polygons) {
            // Khởi tạo mảnh ghép từ prefab
            GameObject piece = Instantiate(puzzlePiecePrefab, piecesParent);
            
            // Tính toán vị trí ban đầu dựa trên bounds
            int x = poly.bounds[0];
            int y = poly.bounds[1];
            int width = poly.bounds[2];
            int height = poly.bounds[3];
            // Ví dụ: đặt vị trí theo tọa độ (x, y)
            piece.transform.position = new Vector2(x, y);
            
            // Tạo danh sách điểm từ triangle_points
            List<Vector2> points = new List<Vector2>();
            for (int i = 0; i < poly.triangle_points.Count; i += 2) {
                points.Add(new Vector2(poly.triangle_points[i], poly.triangle_points[i + 1]));
            }
            
            // Tạo Mesh từ các điểm
            Mesh mesh = CreateMesh(points);
            MeshFilter mf = piece.GetComponent<MeshFilter>();
            if (mf != null) {
                mf.mesh = mesh;
            }
            
            // Cấu hình collider với các điểm (nếu sử dụng PolygonCollider2D)
            PolygonCollider2D pc = piece.GetComponent<PolygonCollider2D>();
            if (pc != null) {
                pc.points = points.ToArray();
            }
        }
    }
    
    // Hàm tạo Mesh từ danh sách các điểm
    Mesh CreateMesh(List<Vector2> points) {
        Mesh mesh = new Mesh();
        Vector3[] vertices = new Vector3[points.Count];
        for (int i = 0; i < points.Count; i++) {
            vertices[i] = new Vector3(points[i].x, points[i].y, 0);
        }
        
        // Giả sử: các điểm tạo thành các tam giác nối tiếp (mỗi 3 điểm tạo thành 1 tam giác)
        int triangleCount = points.Count / 3;
        int[] triangles = new int[triangleCount * 3];
        for (int i = 0; i < triangleCount; i++) {
            triangles[i * 3] = i * 3;
            triangles[i * 3 + 1] = i * 3 + 1;
            triangles[i * 3 + 2] = i * 3 + 2;
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
