using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int width, height;
    public int Walls;
    public GameObject tilePrefab;

    public GameObject[,] grid;
    private playerMovement _playerMovement;

    private void Awake()
    {
        _playerMovement = FindObjectOfType<playerMovement>();
    }

    private void Update()
    {
        targeteTile();
    }

    #region Generate Grid
    public void GenerateGrid()
    {

        int tileCount = 0;
        if (tilePrefab == null)
        {
            Debug.LogError("No Tile Prefab Assigned");
            return;
        }
        //loop through the grid position x = width
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                //calculate the position for each tile
                Vector3 position = new Vector3(x, y, 0);
                //instaciate the cube at the calculated position
                GameObject newTile = Instantiate(tilePrefab, position, Quaternion.identity);
                newTile.transform.parent = transform;
                newTile.tag = "Tile";
                newTile.name = $"Tile_{tileCount}";
                tileCount++;
                foreach (Transform child in newTile.transform)
                {
                    child.gameObject.tag = "Tile";
                }

            }
        }


    }
    #endregion

    #region Generate Wall
    public void GenerateWall()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        // Check if there are enough tiles to place walls
        if (tiles.Length < Walls)
        {
            Debug.LogError("Not enough tiles to place walls.");
            return;
        }

        // Randomly select Walls number of tiles
        List<int> selectedIndices = new List<int>();
        while (selectedIndices.Count < Walls)
        {
            int randomIndex = Random.Range(0, tiles.Length);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
            }
        }

        // Change color and tag of selected tiles
        foreach (int index in selectedIndices)
        {
            GameObject tileObject = tiles[index];
            Tile tileComponent = tileObject.GetComponent<Tile>();

            if (tileComponent != null)
            {
                tileComponent.wakable = false; // Mark as not walkable
                tileObject.tag = "Wall";

                SpriteRenderer spriteRenderer = tileObject.GetComponentInChildren<SpriteRenderer>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = Color.red;
                }
            }
            else
            {
               // Debug.LogWarning("Tile component not found on GameObject: " + tileObject.name);
            }
        }
    }
    #endregion

    #region Clear Grid / Clear Wall
    public void ClearGrid()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
        {
            foreach (GameObject tile in tiles)
            {

                DestroyImmediate(tile);
            }
        }
    }

    public void ClearWall()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");

        foreach (GameObject wall in walls)
        {
            // Reset the tile properties
            Tile tileComponent = wall.GetComponent<Tile>();
            if (tileComponent != null)
            {
                tileComponent.wakable = true; // Reset to walkable
            }

            // Reset visuals (color) if necessary
            SpriteRenderer wallRend = wall.GetComponentInChildren<SpriteRenderer>();
            if (wallRend != null)
            {
                wallRend.color = Color.white; // Reset color to default
            }

            // Reset tag
            wall.tag = "Tile";
        }
    }

    public void ClearTarget()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Target");
        {
            foreach (GameObject wall in walls)
            {
                SpriteRenderer wallRend = wall.GetComponentInChildren<SpriteRenderer>();
                wallRend.color = Color.white;
                wall.tag = "Tile";
            }
        }

    }
    #endregion

    #region select target tile
    public void targeteTile()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null)
            {
                GameObject clickedTile = hit.collider.gameObject;
                if (clickedTile.CompareTag("Wall"))
                {
                    Debug.Log("Cannot select a wall tile.");
                    return; // Exit the method early
                }

                // Reset previous target tile if any
                ResetPreviousTarget();

                // Set new tile as target
                clickedTile.tag = "Target";
                SpriteRenderer targetColor = clickedTile.GetComponent<SpriteRenderer>();
                targetColor.color = Color.green;

                Tile tileComponent = clickedTile.GetComponent<Tile>();
                if (tileComponent != null)
                {
                    _playerMovement.targetTile = tileComponent;
                    _playerMovement.MoveToTarget(); //trigger movement immediately
                }
            }
        }
    }


    private void ResetPreviousTarget()
    {
        GameObject[] previousTargets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject tile in previousTargets)
        {
            tile.tag = "Tile";
            SpriteRenderer tileRenderer = tile.GetComponent<SpriteRenderer>();//Get Renderer
            tileRenderer.color = Color.white; // Set back to original color
        }
    }
    #endregion


    #region Assign Material
    //public void AssignMaterial()
    //    GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
    //    Material material = Resources.Load<Material>("Tile");

    //    foreach (GameObject tile in tiles)
    //    {
    //        tile.GetComponent<MeshRenderer>().material = material;
    //    }
    //}
    //#endregion

    //#region Assign Tile
    //public void AssignTileScript()
    //{
    //    GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
    //    foreach (GameObject tile in tiles)
    //    {
    //        tile.AddComponent<Tile>();
    //    }
    //}
    #endregion
}




