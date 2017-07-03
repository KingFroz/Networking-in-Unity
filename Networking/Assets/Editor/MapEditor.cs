using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (TileMapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI ()
    {
        TileMapGenerator map = target as TileMapGenerator;

        //Returns a bool
        //True if a value has been updated in Inspector
        //False otherwise
        if (DrawDefaultInspector()) {
            map.GenerateMap();
        }

        //GUI Button
        if (GUILayout.Button("Generate Map")) {
            map.GenerateMap();
        }
    }
}
