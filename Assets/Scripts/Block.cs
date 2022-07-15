using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {
    public string blockName { get; }
    public byte ID { get; }
    public byte[] textures = new byte[6];
    public bool exists { get; }

    public static Block[] blocks = new Block[256];

    public Block(string blockName, byte ID, bool exists) {
        this.blockName = blockName;
        this.ID = ID;
        this.exists = exists;
    }

    public static void LoadBlocks() {
        blocks[0] = new Block("Air", (byte) BlockTypes.AIR, false).SetTextureData(new byte[] { 0, 0, 0, 0, 0, 0 });
        blocks[1] = new Block("Bedrock", (byte) BlockTypes.BEDROCK, true).SetTextureData(new byte[] { 12, 12, 12, 12, 12, 12 });
        blocks[2] = new Block("Dirt", (byte) BlockTypes.DIRT, true).SetTextureData(new byte[] { 2, 2, 2, 2, 2, 2 });
        blocks[3] = new Block("Grass", (byte) BlockTypes.GRASS, true).SetTextureData(new byte[] { 3, 3, 2, 4, 3, 3 });
        blocks[4] = new Block("Stone", (byte) BlockTypes.STONE, true).SetTextureData(new byte[] { 1, 1, 1, 1, 1, 1 });
        
        
        
        
        
    
    }

    public Block SetTextureData(byte[] data) {
        this.textures = data;
        return this;
    }

    public static byte GetTextureData(byte ID, int faceIndex) {
        return blocks[ID].textures[faceIndex];
    }

}

public enum BlockTypes {
    AIR = 0,
    BEDROCK = 1,
    DIRT = 2,
    GRASS = 3,
    STONE = 4,
}
