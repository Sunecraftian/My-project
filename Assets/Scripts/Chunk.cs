using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk{
    
    World world;

    GameObject chunk;
    MeshRenderer renderer;
    MeshFilter filter;
    
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    public byte[,,] blockMap = new byte[VoxelData.chunkSize, VoxelData.chunkHeight, VoxelData.chunkSize];

    int vertexIndex = 0;


    public Chunk(World world, Vector2 pos) {
        this.world = world;

        chunk = new GameObject("Chunk " + pos.x + ", " + pos.y);
        renderer = chunk.AddComponent<MeshRenderer>();
        filter = chunk.AddComponent<MeshFilter>();
        renderer.material = world.material;

        chunk.transform.SetParent(world.transform);
        chunk.transform.position = chunkPos + new Vector3(pos.x * VoxelData.chunkSize, 0, pos.y * VoxelData.chunkSize); ;


        FillBlockMap();
        GenerateChunk();
        CreateMesh();
    }

    public Vector3 chunkPos { 
        get {
            return chunk.transform.position;
        }
    }

    void FillBlockMap() {
        for (int y = 0; y < VoxelData.chunkHeight; y++) {
            for (int x = 0; x < VoxelData.chunkSize; x++) {
                for (int z = 0; z < VoxelData.chunkSize; z++) {
                    blockMap[x,y,z] = world.GetVoxel(new Vector3(x, y, z) + chunkPos);
                } 
            }
        }
    }


    void GenerateChunk() {
        for (int y = 0; y < VoxelData.chunkHeight; y++) {
            for (int x = 0; x < VoxelData.chunkSize; x++) {
                for (int z = 0; z < VoxelData.chunkSize; z++) {
                    if (Block.blocks[blockMap[x, y, z]].exists) {
                        GenerateVoxel(new Vector3(x, y, z));
                    }
                } 
            }
        }
    }
    

    void GenerateVoxel(Vector3 pos) {
        for (int i = 0; i < 6; i++) {
            if(!CheckNeighbor(pos + VoxelData.faceChecks[i])){
                CreateVertices(i, pos);
                AddTexture(blockMap[(int)pos.x, (int)pos.y, (int)pos.z], i);
                CreateTriangles(vertexIndex);
                vertexIndex += 4;
            }
        } 
    }

    bool CheckNeighbor(Vector3 pos) { 
        if(!IsVoxelInChunk(pos)){ // If Voxel is NOT in chunk
            return Block.blocks[world.GetVoxel(pos + chunkPos)].exists;
        }
        return Block.blocks[blockMap[(int)pos.x, (int)pos.y, (int)pos.z]].exists;
    }

    bool IsVoxelInChunk(Vector3 pos) {
        Vector3 chunkBounds = new Vector3(VoxelData.chunkSize, VoxelData.chunkHeight, VoxelData.chunkSize);

        // If Voxel is inside chunkbounds - return yes (true)
        if (pos.x >= 0 && pos.x < chunkBounds.x &&
            pos.y >= 0 && pos.y < chunkBounds.y &&
            pos.z >= 0 && pos.z < chunkBounds.z) return true;
        return false;
    }

    void CreateVertices(int i, Vector3 pos) {
        vertices.Add(VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 0]] + pos);
        vertices.Add(VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 1]] + pos);
        vertices.Add(VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 2]] + pos);
        vertices.Add(VoxelData.voxelVertices[VoxelData.voxelTriangles[i, 3]] + pos);
    }

    void AddTexture(byte ID, int faceIndex) {
        int face = Block.GetTextureData(ID, faceIndex);

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

    void CreateTriangles(int vertexIndex) {
        triangles.Add(vertexIndex + 0);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 3);
    }

    void CreateMesh() {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        filter.mesh = mesh;
    }
}

public static class VoxelData {
    public static readonly int worldSize = 2;
    public static readonly int chunkSize = 3;
    public static readonly int chunkHeight = 10;
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