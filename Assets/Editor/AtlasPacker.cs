using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class AtlasPacker : EditorWindow {

    int blockSize = 16; // Block Size in pixels
    int atlasSizeInBlocks; 
    int atlasSize;

    Object[] rawTextures; 
    List<Texture2D> sortedTextures = new List<Texture2D>(); 
    Texture2D atlas;

    [MenuItem("RPGBuilders/AtlasPacker")]
    public static void ShowWindow() {
        var window = GetWindow<AtlasPacker>();
        window.titleContent = new GUIContent("AtlasPacker");
        window.Show();
    }

    private void OnGUI() {


        GUILayout.Label("Atlas Packer", EditorStyles.boldLabel);

        blockSize = EditorGUILayout.IntField("Block Size", blockSize);

        if (GUILayout.Button("Load Textures")) {
            LoadTextures();
            PackAtlas();
        }

        if (GUILayout.Button("Clear Textures")) {
            atlas = new Texture2D(atlasSize, atlasSize);
            Debug.Log("AtlasPacker : Cleared Textures");
        }

        if (GUILayout.Button("Save Atlas")) {
            byte[] bytes = atlas.EncodeToPNG();

            try {
                File.WriteAllBytes(Application.dataPath + "/Resources/Textures/Atlas.png", bytes);

                Debug.Log("AtlasPacker : Saved Atlas");
            } catch {
                Debug.Log("AtlasPacker : Could not save atlas");
            }
        }

        GUILayout.Label(atlas);
        
    }

    void LoadTextures() {
        sortedTextures.Clear();
        rawTextures = Resources.LoadAll("Textures", typeof(Texture2D));

        int i = 0;
        foreach (Object texture in rawTextures) {
            Texture2D t = (Texture2D)texture;
            if (t.width == blockSize && t.height == blockSize) {
                sortedTextures.Add(t);
                i++;
            } else {
                Debug.Log("AtlasPacker : Texture " + t.name + " is not of the correct size");
            }
        }
        
        Debug.Log("AtlasPacker : " + sortedTextures.Count + " textures successfully loaded");
    }

    void PackAtlas() {
        atlasSizeInBlocks = nextPowOfTwo(sortedTextures.Count);


        atlasSize = atlasSizeInBlocks * blockSize;
        atlas = new Texture2D(atlasSize, atlasSize);
        Color[] pixels = new Color[atlasSize * atlasSize];

        for (int x = 0; x < atlasSize; x++) {
            for (int y = 0; y < atlasSize; y++) {
                
                // Get current block
                int currentBlockX = x / blockSize;
                int currentBlockY = y / blockSize;

                int index = currentBlockY * atlasSizeInBlocks + currentBlockX;

                // Get pixel in current block
                int currentPixelX = x - currentBlockX * blockSize;
                int currentPixelY = y - currentBlockY * blockSize;

                if (index < sortedTextures.Count) {
                    pixels[(atlasSize - y - 1) * atlasSize + x] = sortedTextures[index].GetPixel(x, blockSize - y - 1);
                } else {
                    pixels[(atlasSize - y - 1) * atlasSize + x] = new Color(0f, 0f, 0f, 0f);

                }
            }
        }

        atlas.SetPixels(pixels);
        atlas.Apply();
    }

    static int nextPowOfTwo(float num) {
        int n = Mathf.CeilToInt(Mathf.Sqrt(num));
         
        n--;
        n |= n >> 1;
        n |= n >> 2;
        n |= n >> 4;
        n |= n >> 8;
        n |= n >> 16;
        n++;

        return n;
    }
}