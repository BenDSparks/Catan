using UnityEngine;
using System;

public class Grid : MonoBehaviour
{
    private Board board;
    public GameObject tilePrefab;
    private Tile[,] tiles;
    public GameObject tokenPrefab;

    public int gridWidth = 5;
    public int gridHeight = 5;


    float hexWidth = 1.732f;
    float hexHeight = 2.0f;
    public float gap = 0.0f;



    public Material brickMat;
    public Material woodMat;
    public Material sheepMat;
    public Material wheatMat;
    public Material oreMat;
    public Material desertMat;
    public Material waterMat;


    Vector3 startPos;

    void Start()
    {
        board = new Board(1);
        AddGap();
        CalcStartPos();
        CreateGrid();
    }

    void AddGap()
    {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    void CalcStartPos()
    {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = hexWidth / 2;

        float x = -hexWidth * (gridWidth / 2) - offset;
        float y = hexHeight * 0.75f * (gridHeight / 2);

        startPos = new Vector3(x, y, -1);
    }

    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float y = startPos.y - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, y, -1);
    }

    void CreateGrid()
    {
        tiles = new Tile[gridWidth, gridWidth];
        int tileIndex = 0;
        int numberTokenIndex = 0;


        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {

                //the partOfTheBoard is inversed to be eaiser to write boards
                if(board.partOfTheBoard[y,x]){
                    //print("Board " + x + ", " + y);

                    //create tile
                    GameObject tile = (GameObject)Instantiate(tilePrefab);
                    tile.transform.parent = this.transform;

                    TileData tileData = tile.GetComponent<TileData>();


                    Vector2 gridPos = new Vector2(x, y);
                    tile.transform.position = CalcWorldPos(gridPos);
                    tile.transform.name = "Hexagon (" + x + "," + y + ")";

                    tileData.setPosition(x, y);

                    tiles[x, y] = new Tile(tile);


                    //if not the desert and water
                    if(board.resourceList[tileIndex] != ResourceType.Desert && board.resourceList[tileIndex] != ResourceType.Water){
                        //set the resource number
                        tiles[x, y].resourceNumber = board.numberTokens[numberTokenIndex];
                        //create a token
                        GameObject token = (GameObject)Instantiate(tokenPrefab);
                        tile.transform.parent = tiles[x, y].gameObject.transform;
                        token.transform.position = tile.transform.position;

                        tileData.setTokenNumber(board.numberTokens[numberTokenIndex]);


                        numberTokenIndex++;
                    }

                    MeshRenderer meshRenderer = tile.GetComponent<MeshRenderer>();
                    switch(board.resourceList[tileIndex]){
                        case ResourceType.Brick:
                            meshRenderer.material = brickMat;
                            break;
                        case ResourceType.Wood:
                            meshRenderer.material = woodMat;
                            break;
                        case ResourceType.Sheep:
                            meshRenderer.material = sheepMat;
                            break;
                        case ResourceType.Wheat:
                            meshRenderer.material = wheatMat;
                            break;
                        case ResourceType.Ore:
                            meshRenderer.material = oreMat;
                            break;
                        case ResourceType.Desert:
                            meshRenderer.material = desertMat;
                            break;
                        case ResourceType.Water:
                            meshRenderer.material = waterMat;
                            break;
                        default:
                            meshRenderer.material = waterMat;
                            break;
                    }




                    tileIndex++;
                }
            }
        }




        //Print half the grid hight and width
        //print("grid height/2: " + (gridHeight / 2));
        //print("grid width/2: " + (gridWidth / 2));

        //test changing the material
        //MeshRenderer meshRendererTest = tiles[2, 2].gameObject.GetComponent<MeshRenderer>();
        //meshRendererTest.material = brickMat;

        //test destroying a gameobject from a tile array
        //Destroy(tiles[1, 4].gameObject);
        //Destroy(tiles[2, 4].gameObject);
        //Destroy(tiles[3, 4].gameObject);

    }

}