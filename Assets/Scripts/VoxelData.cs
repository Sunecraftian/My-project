using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData {
    public static readonly int worldSize = 2;
    public static readonly int chunkSize = 1;
    public static readonly int chunkHeight = 4;
    public static int worldSizeInVoxels { get { return worldSize * chunkSize; } }


    public static readonly int AtlasSize = 4;
    public static float normalizedAtlasSize { get { return 1f / (float)AtlasSize; } }

    public static readonly Vector3[] voxelVertices = new Vector3[8] {
        new Vector3(0.0f, 0.0f, 0.0f), // Bottom Left Back
        new Vector3(1.0f, 0.0f, 0.0f), // Bottom Right Back
        new Vector3(1.0f, 1.0f, 0.0f), // Top Right Back
        new Vector3(0.0f, 1.0f, 0.0f), // Top Left Back
        new Vector3(0.0f, 0.0f, 1.0f), // Bottom Left Front
        new Vector3(1.0f, 0.0f, 1.0f), // Bottom Right Front
        new Vector3(1.0f, 1.0f, 1.0f), // Top Right Front
        new Vector3(0.0f, 1.0f, 1.0f)  // Top Left Front
    };


    public static readonly int[,] voxelTriangles = new int[6, 4] {

        {0, 3, 1, 2}, // Back Face
        {5, 6, 4, 7}, // Front Face
        {1, 5, 0, 4}, // Down Face
        {3, 7, 2, 6}, // Top Face
        {4, 7, 0, 3}, // Left Face
        {1, 2, 5, 6}, // Right Face

    };

    public static readonly Vector2[] voxelUVs = new Vector2[4] {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 1.0f)
    };

    public static Vector3[] faceChecks = new Vector3[6] {
        Vector3.back,    // Back Face  :  0.0f,  0.0f, -1.0f
        Vector3.forward, // Front Face :  0.0f,  0.0f,  1.0f
        Vector3.down,    // Down Face  :  0.0f, -1.0f,  0.0f
        Vector3.up,      // Top Face   :  0.0f,  1.0f,  0.0f
        Vector3.left,    // Left Face  : -1.0f,  0.0f,  0.0f
        Vector3.right,   // Right Face :  1.0f,  0.0f,  0.0f
    };
    
}
