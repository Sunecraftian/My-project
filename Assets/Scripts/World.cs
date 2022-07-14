using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    public static readonly int worldSize = 3;
    public static int worldSizeInVoxels { get { return worldSize * Chunk.chunkSize; } }


    public Material material;

    // public int seed;
    public float scale;
    // public int octaves;

    // public float persistence;
    // [Range(0,1)]
    // public float lacunarity;
    public float offset;

    public bool autoUpdate;

    public static Chunk[,] chunks = new Chunk[worldSize, worldSize];


    private void Start() {
        GenerateWorld();
    }
    private void Update() {
        
    }


    public void GenerateWorld() {
        BlockManager.LoadBlockData();
        TextureManager.LoadTextureData();

        this.transform.position = new Vector3(0, 0, 0);

        for (int x = 0; x < worldSize; x++) {
            for (int z = 0; z < worldSize; z++) {
                chunks[x, z] = new Chunk(this, x, z);
            } 
        }
    }

    public byte GetVoxel(Vector3 pos) {
        int y = Mathf.FloorToInt(pos.y);
        
        if (!IsVoxelInWorld(pos)) return (byte) BlockTypes.AIR; // If Voxel is NOT in world
        if (y == 0) return (byte) BlockTypes.BEDROCK;
        
        
        

        int terrainHeight = Mathf.FloorToInt(NoiseGenerator.OGet2DPerlin(new Vector2(pos.x, pos.z), offset, scale) * Chunk.chunkHeight);


        if (y == terrainHeight) 
            return 3;
        else if (y > terrainHeight)
            return 0;
        else {
            return 2; 
        }    

        // if (y == terrainHeight) 
        //     return (byte) BlockTypes.DIRT;
        // else if (y > terrainHeight) 
        //     return (byte) BlockTypes.AIR;
        // else if (y < terrainHeight)
        //     return (byte) BlockTypes.STONE;
        // else 
        //     return (byte) BlockTypes.GRASS;
            
    }


    public bool IsVoxelInWorld(Vector3 pos) {
        Vector3 worldBounds = new Vector3(worldSizeInVoxels, Chunk.chunkHeight, worldSizeInVoxels); // TODO : Change to Noise Height at pos.y

        // If Voxel is inside worldbounds - return yes (true)
        if (pos.x >= 0 && pos.x < worldBounds.x &&
            pos.y >= 0 && pos.y < worldBounds.y &&
            pos.z >= 0 && pos.z < worldBounds.z) return true;
        return false;
    }

    


    public class Chunk {
        public static readonly int chunkSize = 8;
        public static readonly int chunkHeight = 16;

        int xCoord { get; set; } 
        int zCoord { get; set; } 

        World world;

        GameObject chunk;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        byte[,,] voxelMap = new byte[chunkSize, chunkHeight, chunkSize];

        int vertexIndex = 0;


        public Chunk(World world, int xCoord, int zCoord) {
            this.world = world;
            this.xCoord = xCoord;
            this.zCoord = zCoord;

            chunk = new GameObject("Chunk " + xCoord + ", " + zCoord);
            meshRenderer = chunk.AddComponent<MeshRenderer>();
            meshFilter = chunk.AddComponent<MeshFilter>();

            chunk.transform.SetParent(world.transform);
            chunk.transform.position = chunkPos + new Vector3(xCoord * chunkSize, 0, zCoord * chunkSize); ;

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


        void FillVoxelMap() {
            for (int y = 0; y < Chunk.chunkHeight; y++) {
                for (int x = 0; x < Chunk.chunkSize; x++) {
                    for (int z = 0; z < Chunk.chunkSize; z++) {
                        voxelMap[x,y,z] = world.GetVoxel(new Vector3(x, y, z) + chunkPos);
                    } 
                }
            }
        }


        void GenerateChunk() {
            for (int y = 0; y < Chunk.chunkHeight; y++) {
                for (int x = 0; x < Chunk.chunkSize; x++) {
                    for (int z = 0; z < Chunk.chunkSize; z++) {
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
            Vector3 chunkBounds = new Vector3(chunkSize, chunkHeight, chunkSize);

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

    


}

