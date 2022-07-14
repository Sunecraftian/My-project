using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockManager {

    public static Block[] blocks = new Block[256];
 
    public static void LoadBlockData() {
        blocks[0] = new Block("Air", BlockTypes.AIR, false);
        blocks[1] = new Block("Bedrock", BlockTypes.BEDROCK, true);
        blocks[2] = new Block("Dirt", BlockTypes.DIRT, true);
        blocks[3] = new Block("Grass", BlockTypes.GRASS, true);
        blocks[4] = new Block("Stone", BlockTypes.STONE, true);
        
    }

    public static Block GetBlockByType(BlockTypes blockType) {
        return blocks[(int)blockType];
    }

}




public class Block {
    public string name { get; set; }
    public BlockTypes id { get; set; }
    public bool exists { get; set; }

    public Block(string name, BlockTypes blockTypes, bool exists) {
        this.name = name;
        this.id = blockTypes;        
        this.exists = exists;
    }
}

public enum BlockTypes {
    AIR = 0,
    BEDROCK = 1,
    DIRT = 2,
    GRASS = 3,
    STONE = 4,


}


