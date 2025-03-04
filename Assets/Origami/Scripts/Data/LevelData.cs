using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData {
    public long timestamp;
    public int grid_size;         // Giá trị grid_size sẽ được lấy từ file JSON
    public List<PolygonData> polygons;
}

[Serializable]
public class PolygonData {
    public List<int> bounds;         // [x, y, width, height]
    public List<int> triangle_points;
}