using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    // El tilemap de la lógica
    [SerializeField] private Tilemap map;

    [SerializeField] private List<TileData> tileDatas;

    private Dictionary<TileBase, TileData> dataFromTiles;

    private void Awake()
    {
        dataFromTiles = new Dictionary<TileBase, TileData>();

        foreach (var tileData in tileDatas)
            foreach (var tile in tileData.tiles)
                dataFromTiles.Add(tile, tileData);

        //Para comprobar
        //foreach (var data in dataFromTiles)
        //    Debug.Log(data);
            
    }

    // Update is called once per frame
    void Update()
    {
        // Prueba para comprobar la identificación de tiles
        
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = map.WorldToCell(mousePosition);

            TileBase clickedTile = map.GetTile(gridPosition);
            Debug.Log("At position " + gridPosition + " there is a " + clickedTile);

            //float walkingSpeed = dataFromTiles[clickedTile].walkingSpeed;
            //if(dataFromTiles[clickedTile].fall)
            //Debug.Log("Caes");

        }
    }

    public float GetTileWalkingSpeed(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);

        if (tile == null) return 1f;

        float walkingSpeed = dataFromTiles[tile].walkingSpeed;
        return walkingSpeed;
    }

    public TileData GetTileInfo(Vector2 worldPosition)
    {
        Vector3Int gridPosition = map.WorldToCell(worldPosition);
        TileBase tile = map.GetTile(gridPosition);

        if (tile == null) return null;

        return dataFromTiles[tile];
       
    }
}
