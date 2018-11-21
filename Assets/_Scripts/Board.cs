using UnityEngine;

public class Board
{

    //height and width of grid
    private int width;
    private int height;
    private int tileCount;

    //bool saying if this tile is water or not
    public int[,] partOfTheBoard;

    //list of resources to randomize
    public ResourceType[] resourceList;

    public PortType[] ports;

    //number tokens for tile numbers
    public int[] numberTokens;

    public int TileCount {
        get {
            return tileCount;
        }

        set {
            tileCount = value;
        }
    }

  

    public int getHeight() {
        return height;
    }

    public int getWidth() {
        return width;
    }

    

    public Board(int boardNumber)
    {
        if (boardNumber == 1)
        {
            width = 7;
            height = 7;
            tileCount = 19;

            //{
            //    {-1,-1,-1,-1,-1,-1,-1},
            //    {-1,-1,0,1,2,-1,-1},
            //    {-1,-1,11,12,13,3,-1},
            //    {-1,10,17,18,14,4,-1},
            //    {-1,-1,9,16,15,5,-1},
            //    {-1,-1,8,7,6,-1,-1},
            //    {-1,-1,-1,-1,-1,-1,-1}
            //};
            //counter clockwise like in the catan rule book
            partOfTheBoard = new int[,]
            {
                {-1,-1,-1,-1,-1,-1,-1},
                {-1,-1,8,7,6,-1,-1},
                {-1,-1,9,16,15,5,-1},
                {-1,10,17,18,14,4,-1},
                {-1,-1,11,12,13,3,-1},
                {-1,-1,0,1,2,-1,-1},
                {-1,-1,-1,-1,-1,-1,-1}
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
            ports = new PortType[] {
                PortType.Brick,
                PortType.Wood,
                PortType.Sheep,
                PortType.Wheat,
                PortType.Ore,
                PortType.threeToOne,
                PortType.threeToOne,
                PortType.threeToOne,
                PortType.threeToOne
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

