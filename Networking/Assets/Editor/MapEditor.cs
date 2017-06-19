using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (TileMapGenerator))]
public class MapEditor : Editor {

    public override void OnInspectorGUI ()
    {
        base.OnInspectorGUI();

        TileMapGenerator map = target as TileMapGenerator;

        map.GenerateMap();
    }
}
