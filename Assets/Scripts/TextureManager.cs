using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureManager {

    public static byte[][] Data = new byte[256][];

    public static void LoadTextureData() {
        // Back - Front - Down - Top - Left - Right //
        Data[0] = new byte[] {9, 9, 9, 9, 9, 9};       // AIR
        Data[1] = new byte[] {11, 11, 11, 11, 11, 11}; // BEDROCK
        Data[2] = new byte[] {1, 1, 1, 1, 1, 1};       // DIRT
        Data[3] = new byte[] {2, 2, 1, 3, 2, 2};       // GRASS
        Data[4] = new byte[] {0, 0, 0, 0, 0, 0};       // STONE

    }

    public static void AddTexture(byte ID, int faceIndex, List<Vector2> uvs) {
        int face = TextureManager.Data[ID][faceIndex];

        float y = face / VoxelData.AtlasSize;
        float x = face - (y * VoxelData.AtlasSize);

        x *= VoxelData.normalizedAtlasSize;
        y *= VoxelData.normalizedAtlasSize;

        y = 1f - y - VoxelData.normalizedAtlasSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.normalizedAtlasSize));
        uvs.Add(new Vector2(x + VoxelData.normalizedAtlasSize, y));
        uvs.Add(new Vector2(x + VoxelData.normalizedAtlasSize, y + VoxelData.normalizedAtlasSize));
    }

}
