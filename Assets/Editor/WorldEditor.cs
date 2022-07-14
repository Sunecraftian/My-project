using System;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(World))]
public class WorldEditor : Editor {
    public override void OnInspectorGUI() {
        World world = (World) target;
        GameObject a = GameObject.Find("World");


        if (DrawDefaultInspector()){
            if (world.autoUpdate) {
                
                while(a.transform.childCount > 0) {
                    DestroyImmediate(a.transform.GetChild(0).gameObject);
                }
                world.GenerateWorld();
            }
        }

        if (GUILayout.Button("Generate")) {
            while(a.transform.childCount > 0) {
                DestroyImmediate(a.transform.GetChild(0).gameObject);
            }
            world.GenerateWorld();
        }

        if (GUILayout.Button("Delete")) {
            while(a.transform.childCount > 0) {
                    DestroyImmediate(a.transform.GetChild(0).gameObject);
                }
        }

        // if (GUILayout.Button("Test")) {
            
        // }

    }

    
}