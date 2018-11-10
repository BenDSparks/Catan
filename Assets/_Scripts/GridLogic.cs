using UnityEngine;
using System;
using System.Collections.Generic;

public class GridLogic : MonoBehaviour
{
    private Board board;
    public GameObject tilePrefab;
    public GameObject settlementPrefab;
    public GameObject tokenPrefab;
    public GameObject settlementGameObject;
    private Tile[,] tiles;
    private Settlement[,] settlements;
    

    private int gridWidth = 7;
    private int gridHeight = 7;
    private int settlementCount = 22;
    private int roadCount = 22;


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
        CreateHexGrid();
        CreateSettlementGrid();
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

    Vector3 CalcWorldPosTiles(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float y = startPos.y - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, y, -1);
    }

    //Vector3 CalcWorldPosSettlements(Vector2 gridPos) {
    //    float offset = 0;
    //    if (gridPos.y % 2 != 0) {
    //        offset = hexHeight / 2;
    //    }

    //    float x = startPos.x + gridPos.x * hexWidth + offset;
    //    float y = startPos.y - gridPos.y * hexHeight * 0.75f;


    //    return new Vector3(x, y, -3);
    //}

    //tile Logic
    void CreateHexGrid()
    {
        tiles = new Tile[gridWidth, gridWidth];
        settlements = new Settlement[22, 22];

        int tileIndex = 0;
        int numberTokenIndex = 0;


        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                //the partOfTheBoard is inversed to be eaiser to write boards
                if(board.partOfTheBoard[y,x]){
                 
                    //create tile
                    GameObject tile = (GameObject)Instantiate(tilePrefab);
                    tile.transform.parent = this.transform;

                    TileData tileData = tile.GetComponent<TileData>();

                    //set tile position
                    Vector2 gridPos = new Vector2(x, y);
                    tile.transform.position = CalcWorldPosTiles(gridPos);
                    tileData.setPosition(x, y);

                    tile.transform.name = "Hexagon (" + x + "," + y + ")";
                    

                    tiles[x, y] = new Tile(tile,x,y);

                    //place the settlements above and below
                    PlaceTopSettlement(tiles[x, y]);
                    PlaceBottomSettlement(tiles[x, y]);

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
                    tileData.setResourceType(board.resourceList[tileIndex]);

                    tileIndex++;
                    //print("Hexagon: " + x + "," + y + ") " + tileData.getTokenNumber() + " " + tileData.getResourceType());
                }
                else{
                    GameObject tile = (GameObject)Instantiate(tilePrefab);
                    tile.transform.parent = this.transform;

                    tiles[x, y] = new Tile(tile, x, y);


                    TileData tileData = tile.GetComponent<TileData>();


                    Vector2 gridPos = new Vector2(x, y);
                    tile.transform.position = CalcWorldPosTiles(gridPos);
                    tile.transform.name = "Hexagon (" + x + "," + y + ") Water";

                    tileData.setPosition(x, y);
                    tileData.setResourceType(ResourceType.Water);

                }

               


            }
        }
      




    }
    //Settlement Logic
    void CreateSettlementGrid() {
        
        for (int x = 0; x < settlementCount; x++) {
            for (int y = 0; y < settlementCount; y++) {
                settlements[x, y] = new Settlement(null, x, y);
            }
        }

    }

    void PlaceTopSettlement(Tile tile) {
        GameObject settlement = (GameObject)Instantiate(settlementPrefab);
        settlement.transform.parent = settlementGameObject.transform;

        //set tile position
        Vector2 gridPos = new Vector2(tile.x,tile.y);
        Vector3 worldPos = CalcWorldPosTiles(gridPos);

        worldPos.y = worldPos.y + (hexHeight / 2);

        settlement.transform.position = worldPos;
        //tile.transform.position = CalcWorldPosTiles(gridPos);
        


        //TODO set settlement into the grid for data purposes

    }

    void PlaceBottomSettlement(Tile tile) {
        GameObject settlement = (GameObject)Instantiate(settlementPrefab);
        settlement.transform.parent = settlementGameObject.transform;

        //set tile position
        Vector2 gridPos = new Vector2(tile.x, tile.y);
        Vector3 worldPos = CalcWorldPosTiles(gridPos);

        worldPos.y = worldPos.y - (hexHeight / 2);

        settlement.transform.position = worldPos;
    }

    public Tile[] GetNeighbors(Tile tile) {
        int x = tile.x;
        int y = tile.y;
        int offset = y % 2 == 0 ? -1 : 1;
        return new[] {
            //right tile
            tiles[x+1,y],
            //left tile
            tiles[x-1,y],
            tiles[x,y+1],
            tiles[x,y-1],
            tiles[x+offset,y+1],
            tiles[x+offset,y-1]

        };
    }

    public void checkIfNextToWater(int x, int y) {
        Tile[] surroundingTiles = GetNeighbors(tiles[x, y]);

        for (int i = 0; i < surroundingTiles.Length; i++) {
            TileData tileData = surroundingTiles[i].gameObject.GetComponent<TileData>();

    print("(" + surroundingTiles[i].x + "," + surroundingTiles[i].y + ") : " + tileData.getResourceType());
        }
    }

    public void testDelete(int x, int y) {
        Tile[] surroundingTiles = GetNeighbors(tiles[x, y]);

        for (int i = 0; i < surroundingTiles.Length; i++) {
            TileData tileData = surroundingTiles[i].gameObject.GetComponent<TileData>();

            print("(" + surroundingTiles[i].x + "," + surroundingTiles[i].y + ") : " + tileData.getResourceType());

            Destroy(surroundingTiles[i].gameObject);
        }
    }

    //public IEnumerable<Tile> GetNeighbors(Tile tile) {
    //    var x = tile.getX; var y = tile.getY;
    //    var offset = x % 2 == 0 ? +1 : -1;
    //    return new[]
    //    {
    //        Hexes[x,y+1],
    //        Hexes[x,y-1],
    //        Hexes[x+1,y],
    //        Hexes[x-1,y],
    //        Hexes[x+1,y+offset],
    //        Hexes[x-1,y+offset],
    //    };
    //}

}