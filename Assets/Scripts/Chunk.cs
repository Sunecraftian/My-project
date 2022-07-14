using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk{
    

    public int xCoord { get; set; } 
    public int zCoord { get; set; } 

    

    World world;

    GameObject chunk;
    MeshRenderer meshRenderer;
    MeshFilter meshFilter;
    
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    public byte[,,] voxelMap = new byte[VoxelData.chunkSize, VoxelData.chunkHeight, VoxelData.chunkSize];

    int vertexIndex = 0;


    public Chunk(World world, int xCoord, int zCoord) {
        this.world = world;
        this.xCoord = xCoord;
        this.zCoord = zCoord;

        chunk = new GameObject("Chunk " + xCoord + ", " + zCoord);
        meshRenderer = chunk.AddComponent<MeshRenderer>();
        meshFilter = chunk.AddComponent<MeshFilter>();

        chunk.transform.SetParent(world.transform);
        chunk.transform.position = chunkPos + new Vector3(xCoord * VoxelData.chunkSize, 0, zCoord * VoxelData.chunkSize); ;

        meshRenderer.material = world.material;

        FillVoxelMap();
        GenerateChunk();
        CreateMesh();
    }

    public Vector3 chunkPos { 
        get {
            return chunk.transform.position;
        }
    }

    public byte[,,] GetVoxelMap() {
        return voxelMap;
    }


    void FillVoxelMap() {
        for (int y = 0; y < VoxelData.chunkHeight; y++) {
            for (int x = 0; x < VoxelData.chunkSize; x++) {
                for (int z = 0; z < VoxelData.chunkSize; z++) {
                    voxelMap[x,y,z] = world.GetVoxel(new Vector3(x, y, z) + chunkPos);
                } 
            }
        }
    }


    void GenerateChunk() {
        for (int y = 0; y < VoxelData.chunkHeight; y++) {
            for (int x = 0; x < VoxelData.chunkSize; x++) {
                for (int z = 0; z < VoxelData.chunkSize; z++) {
                    // if (BlockManager.blocks[voxelMap[x, y, z]].id != 0) GenerateVoxel(new Vector3(x, y, z));
                    if (BlockManager.blocks[voxelMap[x, y, z]].exists) {
                        GenerateVoxel(new Vector3(x, y, z));
                    }
                } 
            }
        }
    }
    

    void GenerateVoxel(Vector3 pos) {
        for (int i = 0; i < 6; i++) {
            if(!CheckNeighbor(pos + VoxelData.faceChecks[i])){
                if (BlockManager.blocks[voxelMap[(int)pos.x, (int)pos.y, (int)pos.z]].exists) {
                
                    CreateVertices(i, pos);
                    TextureManager.AddTexture(world.GetVoxel(pos), i, uvs);
                    CreateTriangles(vertexIndex);
                    vertexIndex += 4;
                
                }
            }
        } 
    }

    bool CheckNeighbor(Vector3 pos) { 
        if(!IsVoxelInChunk(pos)){ // If Voxel is NOT in chunk
            return BlockManager.blocks[world.GetVoxel(pos + chunkPos)].exists;
        }
        return BlockManager.blocks[voxelMap[(int) pos.x,  (int) pos.y,  (int) pos.z ]].exists;
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

        meshFilter.mesh = mesh;
    }
}