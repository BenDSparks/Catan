using UnityEngine;

public class Board
{

    //height and width of grid
    private int width;
    private int height;

    //bool saying if this tile is water or not
    public bool[,] partOfTheBoard;

    //list of resources to randomize
    public ResourceType[] resourceList;

    //number tokens for tile numbers
    public int[] numberTokens;



    public Board(int boardNumber)
    {
        if (boardNumber == 1)
        {
            width = 7;
            height = 7;
            partOfTheBoard = new bool[,]
            {
                {false,false,false,false,false,false,false},
                {false,false,true,true,true,false,false},
                {false,false,true,true,true,true,false},
                {false,true,true,true,true,true,false},
                {false,false,true,true,true,true,false},
                {false,false,true,true,true,false,false},
                {false,false,false,false,false,false,false}
            };
            resourceList = new ResourceType[] {
                ResourceType.Brick,
                ResourceType.Brick,
                ResourceType.Brick,
                ResourceType.Wood,
                ResourceType.Wood,
                ResourceType.Wood,
                ResourceType.Wood,
                ResourceType.Sheep,
                ResourceType.Sheep,
                ResourceType.Sheep,
                ResourceType.Sheep,
                ResourceType.Wheat,
                ResourceType.Wheat,
                ResourceType.Wheat,
                ResourceType.Wheat,
                ResourceType.Ore,
                ResourceType.Ore,
                ResourceType.Ore,
                ResourceType.Desert
            };

            ShuffleArray(resourceList);

            numberTokens = new int[] { 5, 2, 6, 3, 8, 10, 9, 12, 11, 4, 8, 10, 9, 4, 5, 6, 3, 11 };

            
        }

    }

    void ShuffleArray(ResourceType[] resources){

        for (int i = 0; i < resources.Length; i++){

            ResourceType tempResource = resources[i];
            int shuffleArray = Random.Range(0, resources.Length);
            resources[i] = resources[shuffleArray];
            resources[shuffleArray] = tempResource;


        }



    }
    
}

