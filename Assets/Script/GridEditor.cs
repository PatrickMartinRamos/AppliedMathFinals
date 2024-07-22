using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(GridManager))]
public class GridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GridManager gridGenerator = (GridManager)target;
        if (GUILayout.Button("Generate Grid"))
        {
            gridGenerator.GenerateGrid();
        }
        if (GUILayout.Button("Generate Wall"))
        {
            gridGenerator.GenerateWall();
        }
        if (GUILayout.Button("Clear Wall"))
        {
            gridGenerator.ClearWall();
        }
        if (GUILayout.Button("Clear Grid"))
        {
            gridGenerator.ClearGrid();
        }
        if (GUILayout.Button("Clear Target"))
        {
            gridGenerator.ClearTarget();
        }

    }

    [MenuItem("Tools/Grid/Generate Grid")]
    public static void GenerateGrid()
    {
        GridManager gridGenerator = FindObjectOfType<GridManager>();
        if (gridGenerator != null)
        {
            gridGenerator.GenerateGrid();
        }
        else
        {
            Debug.LogError("No Grid Generator Found in Scene");
        }
    }
}
