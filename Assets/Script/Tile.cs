using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<Tile> neighbors;
   // public bool walkable = true;
    public Tile cameFrom;
    public bool wakable = true;

    private void Start()
    {
        FindNeighbors();
    }

    public void FindNeighbors()
    {
        //ResetNeighbors();

        CheckTile(Vector2.up);
        CheckTile(Vector2.down);
        CheckTile(Vector2.right);
        CheckTile(Vector2.left);
    }

    //private void ResetNeighbors()
    //{
    //    neighbors = new List<Tile>();
    //}

    public void CheckTile(Vector2 direction)
    {
        Vector2 currentPos = transform.position;
        Vector2 checkPos = currentPos + direction;
        Vector2 halfExtents = new Vector2(0.5f, 0.5f); // Adjust as needed

        Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPos, halfExtents, 0f);
        foreach (Collider2D collider in colliders)
        {
            Tile tile = collider.GetComponent<Tile>();

            if (tile != null)
            {
                RaycastHit2D hit = Physics2D.Raycast(tile.transform.position, Vector2.up, 1);
                if (collider.gameObject.tag == "Wall")
                {
                    tile.wakable = false; // Mark as not walkable if it's a wall
                }
                else if (collider.gameObject.tag == "Tile")
                {
                    neighbors.Add(tile);
                    tile.wakable = true; // Mark as walkable if it's a regular tile
                }
            }
        }

        // Debug.Log("Tile at " + currentPos + " has " + neighbors.Count + " neighbors.");
    }
}