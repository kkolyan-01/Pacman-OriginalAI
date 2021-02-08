using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMapTools : MonoBehaviour
{
    private int _countTiles;
    private Tilemap _tilemap;
    private void Awake()
    {
        _countTiles = 0;
    }

    private void Start()
    {
        _tilemap = GetComponent<Tilemap>();
        CountTiles();
        GameManager.singlton.allPeletsCount = _countTiles;
    }

    private void CountTiles()
    {
        BoundsInt bounds = _tilemap.cellBounds;
        TileBase[] allTiles = _tilemap.GetTilesBlock(bounds);
        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    _countTiles++;
                }
            }
        }
    }
}
