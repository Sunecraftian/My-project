using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour {
    public Material material;
    public bool autoUpdate;


    // public int seed;
    public float scale;
    // public int octaves;

    // public float persistence;
    // [Range(0,1)]
    // public float lacunarity;
    public float offset;


    public static Chunk[,] chunks = new Chunk[VoxelData.worldSize, VoxelData.worldSize];


    private void Start() {
        GenerateWorld();
    }
    private void Update() {
        
    }


    public void GenerateWorld() {
        Block.LoadBlocks();

        this.transform.position = new Vector3(0, 0, 0);

        for (int x = 0; x < VoxelData.worldSize; x++) {
            for (int z = 0; z < VoxelData.worldSize; z++) {
                chunks[x, z] = new Chunk(this, new Vector2(x,z));
                // Debug.Log(Mathf.PerlinNoise(x*scale+offset,z*scale+offset));
            } 
        }

        
        // byte[,,] temp1 = chunks[0, 0].GetVoxelMap();
        // byte[,,] temp2 = chunks[1, 0].GetVoxelMap();



        // Debug.Log(temp1[0,0,0] + " " + temp2[0,0,0]);
        // Debug.Log(temp1[0,1,0] + " " + temp2[0,1,0]);
        // Debug.Log(temp1[0,2,0] + " " + temp2[0,2,0]);
        // Debug.Log(temp1[0,3,0] + " " + temp2[0,3,0]);
    }

    public byte GetVoxel(Vector3 pos) {
        int y = Mathf.FloorToInt(pos.y);
        int max = VoxelData.chunkHeight/2;
        int baseline = max/2;
        float noise = Mathf.PerlinNoise(pos.x * scale + offset, pos.y * scale + offset);
        int height = (int)((max * noise) + baseline);
        
        Debug.Log(System.String.Join(", ", new string[]{y.ToString(), noise.ToString(), height.ToString()}));

        if (!IsVoxelInWorld(pos)) return (byte) BlockTypes.AIR; // If Voxel is NOT in world
        if (y == 0) return (byte) BlockTypes.BEDROCK;

        if (y > height) return (byte) BlockTypes.AIR;
        else if (y == height) return (byte) BlockTypes.GRASS;

        
        return (byte) BlockTypes.DIRT;
        
            
    }

    // public byte GetVoxel(Vector3 pos) {
    //     int y = Mathf.FloorToInt(pos.y);
        
    //     if (!IsVoxelInWorld(pos)) return (byte) BlockTypes.AIR; // If Voxel is NOT in world
    //     if (y == 0) return (byte) BlockTypes.BEDROCK;
        
        
        

    //     int terrainHeight = Mathf.FloorToInt(NoiseGenerator.OGet2DPerlin(new Vector2(pos.x, pos.z), offset, scale) * VoxelData.chunkHeight);

    //     // if (y == 3) Debug.Log("is 3: " + pos);
    //     // Debug.Log(Chunk.voxelMap.Length);


    //     if (y == terrainHeight) 
    //         return 4;
    //     else if (y > terrainHeight)
    //         return 0;
    //     else {
    //         return 0; 
    //     }    

    //     // if (y == terrainHeight) 
    //     //     return (byte) BlockTypes.DIRT;
    //     // else if (y > terrainHeight) 
    //     //     return (byte) BlockTypes.AIR;
    //     // else if (y < terrainHeight)
    //     //     return (byte) BlockTypes.STONE;
    //     // else 
    //     //     return (byte) BlockTypes.GRASS;
            
    // }


    public bool IsVoxelInWorld(Vector3 pos) {
        Vector3 worldBounds = new Vector3(VoxelData.worldSizeInVoxels, VoxelData.chunkHeight, VoxelData.worldSizeInVoxels); // TODO : Change to Noise Height at pos.y

        // If Voxel is inside worldbounds - return yes (true)
        if (pos.x >= 0 && pos.x < worldBounds.x &&
            pos.y >= 0 && pos.y < worldBounds.y &&
            pos.z >= 0 && pos.z < worldBounds.z) return true;
        return false;
    }
}
