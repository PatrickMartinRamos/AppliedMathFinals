using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour
{
    public GameObject wallPrefab; // Reference to the wall prefab

    public void CreateWalls()
    {
        if (wallPrefab == null)
        {
            Debug.LogError("No Wall Prefab Assigned");
            return;
        }

        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        foreach (GameObject tile in tiles)
        {
            Vector3 tilePosition = tile.transform.position;

            // Randomly decide which walls to create
            if (Random.value > 0.5f) CreateWall(tile, new Vector3(tilePosition.x - 0.5f, tilePosition.y, 0)); // Left
            if (Random.value > 0.5f) CreateWall(tile, new Vector3(tilePosition.x + 0.5f, tilePosition.y, 0)); // Right
            if (Random.value > 0.5f) CreateWall(tile, new Vector3(tilePosition.x, tilePosition.y + 0.5f, 0)); // Top
            if (Random.value > 0.5f) CreateWall(tile, new Vector3(tilePosition.x, tilePosition.y - 0.5f, 0)); // Bottom
        }
    }

    private void CreateWall(GameObject parentTile, Vector3 position)
    {
        GameObject newWall = Instantiate(wallPrefab, position, Quaternion.identity);
        newWall.transform.parent = parentTile.transform; // Make the wall a child of the tile
        newWall.tag = "Wall";
        newWall.name = "Wall";
    }

    public void ClearWalls()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach (GameObject wall in walls)
        {
            DestroyImmediate(wall);
        }
    }
}
