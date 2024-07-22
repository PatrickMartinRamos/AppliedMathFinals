using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class playerMovement : MonoBehaviour
{
    public Tile targetTile; 

    private Stack<Tile> pathStack = new Stack<Tile>();
    private Tile currentTile;

    private void Start()
    {
        currentTile = GetCurrentTile();
        MoveToTarget();
    }

    private Tile GetCurrentTile()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);

        foreach (Collider2D collider in colliders)
        {
            Tile tile = collider.GetComponent<Tile>();
            if (tile != null && tile.wakable)
            {
                // Check if player is inside this tile
                if (collider.OverlapPoint(transform.position))
                {
                    return tile;
                }
            }
        }

        // If no walkable tile found, find the nearest walkable tile
        //Tile nearestWalkableTile = FindNearestWalkableTile();
        //if (nearestWalkableTile != null)
        //{
        //    return nearestWalkableTile;
        //}

        return null; // Return null or handle case when player is not on any tile
    }

    private Tile FindNearestWalkableTile()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1.0f);

        foreach (Collider2D collider in colliders)
        {
            Tile tile = collider.GetComponent<Tile>();
            if (tile != null && tile.wakable)
            {
                return tile;
            }
        }

        return null; // No walkable tile found nearby
    }

    public void MoveToTarget()
    {
        pathStack.Clear(); // Clear previous path
        DFS(currentTile, targetTile);
        StartCoroutine(MoveAlongPath());

        // Check if pathStack is empty after DFS
        if (pathStack.Count == 0)
        {
            Debug.Log("No valid path found.");
            return; // Exit the method if no path found
        }
    }

    private void DFS(Tile current, Tile target)
    {
        Stack<Tile> stack = new Stack<Tile>();
        List<Tile> visited = new List<Tile>();

        stack.Push(current);
        visited.Add(current);

        while (stack.Count > 0)
        {
            Tile node = stack.Pop();

            if (node == target)
            {
                // Found the target, reconstruct path
                while (node != current)
                {
                    pathStack.Push(node);
                    node = node.cameFrom;
                }
                pathStack.Push(current);
                break;
            }

            foreach (Tile neighbor in node.neighbors)
            {
                if (!visited.Contains(neighbor) && neighbor.wakable)
                {
                    visited.Add(neighbor);
                    stack.Push(neighbor);
                    neighbor.cameFrom = node;
                }
            }
        }
    }

    private IEnumerator MoveAlongPath()
    {
        while (pathStack.Count > 0)
        {
            Tile nextTile = pathStack.Pop();
            //Debug.Log("Moving to: " + nextTile.gameObject.name); 
            transform.position = nextTile.transform.position;

            yield return new WaitForSeconds(0.5f);
        }

        //Debug.Log("Reached target tile!"); 
    }
}
