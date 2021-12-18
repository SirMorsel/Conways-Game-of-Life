using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TileMapVisualizer : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase tileLivingCell;
    [SerializeField] private TileBase tileLivingSpaceFrame;

    private static TileMapVisualizer _instance;
    public static TileMapVisualizer Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void CreateLivingAreaFrame(int width, int height)
    {
        for (int y = 0; y <= height; y++) // Left Frame
        {
            tilemap.SetTile(new Vector3Int(0, y, 0), tileLivingSpaceFrame);
        }
        for (int y = 0; y <= height; y++) // Right Frame
        {
            tilemap.SetTile(new Vector3Int(width + 1, y, 0), tileLivingSpaceFrame);
        }
        for (int x = 0; x <= width; x++) // Bottom Frame
        {
            tilemap.SetTile(new Vector3Int(x, 0, 0), tileLivingSpaceFrame);
        }
        for (int x = 0; x <= width + 1; x++)
        {
            tilemap.SetTile(new Vector3Int(x, height + 1, 0), tileLivingSpaceFrame);
        }
    }

    public void PaintLivingCellTile(Vector3Int position, bool isAlive)
    {
        if (isAlive)
        {
            tilemap.SetTile(position, tileLivingCell);
        }
        else
        {
            tilemap.SetTile(position, null);
        }
        
    }

    public void ClearTiles()
    {
        tilemap.ClearAllTiles();
    }

    public Tilemap GetTileMap()
    {
        return tilemap;
    }
}
