﻿using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class GridLogic : MonoBehaviour
{
    private Board board;
    public GameObject tilePrefab;
    public GameObject settlementPrefab;
    public GameObject roadPrefab;
    public GameObject tokenPrefab;
    public GameObject settlementSpotsGameObject;
    public GameObject roadSpotsGameObject;
    private Tile[,] tiles;
    private Settlement[,] settlements;
    private Road[,] roads;



    private int gridWidth;
    private int gridHeight;
    private Tile[] tokenOrder;
    private int settlementWidth;
    private int roadCount;


    private float hexWidth = 1.732f;
    private float hexHeight = 2.0f;
    public float gap = 0.0f;

    private bool isStartingPhase;


    public Material brickMat;
    public Material woodMat;
    public Material sheepMat;
    public Material wheatMat;
    public Material oreMat;
    public Material desertMat;
    public Material waterMat;

    public Material highlightMat;


    Vector3 startPos;

    void Start()
    {
        board = new Board(1);
        tokenOrder = new Tile[board.TileCount];
        gridHeight = board.getHeight();
        gridWidth = board.getWidth();
        if(gridWidth >= gridHeight) {
            settlementWidth = 2 * gridWidth + 2;
            roadCount = 2 * gridWidth + 2;
        }
        else {
            settlementWidth = 2 * gridHeight + 2;
            roadCount = 2 * gridWidth + 2;
        }
        
        AddGap();
        CalcStartPos();
        CreateHexGrid();
        PlaceNumberTokens();
        //CreateSettlementGrid();
        PlaceSettlements();
        PlaceRoads();

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

    //tile Logic
    void CreateHexGrid()
    {
        tiles = new Tile[gridWidth, gridWidth];
        
        int tileIndex = 0;

        for (int y = 0; y < gridHeight; y++) {
            for (int x = 0; x < gridWidth; x++) {
                //the partOfTheBoard is inversed to be eaiser to write boards
                if(board.partOfTheBoard[y,x] != -1){
                    
                    //create tile
                    GameObject tileGameObject = (GameObject)Instantiate(tilePrefab);
                    tileGameObject.transform.parent = this.transform;

                    TileData tileData = tileGameObject.GetComponent<TileData>();

                    //set tile position
                    Vector2 gridPos = new Vector2(x, y);
                    tileGameObject.transform.position = CalcWorldPosTiles(gridPos);
                    tileData.setPosition(x, y);

                    tileGameObject.transform.name = "Hexagon (" + x + "," + y + ") " + board.resourceList[tileIndex];
                    

                    tiles[x, y] = new Tile(tileGameObject,x,y,board.resourceList[tileIndex]);
                    tokenOrder[board.partOfTheBoard[y, x]] = tiles[x, y];

                  

                    MeshRenderer meshRenderer = tileGameObject.GetComponentInChildren<MeshRenderer>();
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
                }
                //tile is water
                else{
                    GameObject tile = (GameObject)Instantiate(tilePrefab);
                    tile.transform.parent = this.transform;

                    tiles[x, y] = new Tile(tile, x, y, ResourceType.Water);

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
   
    void PlaceNumberTokens() {
        int numberTokenIndex = 0;
        for (int i = 0; i < tokenOrder.Length; i++) {
            
            if (tokenOrder[i].resourceType != ResourceType.Desert) {
                int x = tokenOrder[i].x;
                int y = tokenOrder[i].y;
                //place number token
                //set the resource number

                //place tokennumber in tiledata
                TileData tileData = tokenOrder[i].gameObject.GetComponent<TileData>();
                tileData.setTokenNumber(board.numberTokens[numberTokenIndex]);

                tokenOrder[i].resourceNumber = board.numberTokens[numberTokenIndex];
                print("(" + x + "," + y + ") " + tokenOrder[i].resourceNumber + " " + tokenOrder[i].resourceType);
                //create a token
                GameObject token = (GameObject)Instantiate(tokenPrefab);
                token.transform.position = tokenOrder[i].gameObject.transform.position;
                token.transform.parent =tokenOrder[i].gameObject.transform;

                TextMesh textMesh = token.GetComponentInChildren<TextMesh>();
                textMesh.text = board.numberTokens[numberTokenIndex].ToString();



                if (board.numberTokens[numberTokenIndex] == 6 || board.numberTokens[numberTokenIndex] == 8) {
                    //make font red
                    textMesh.color = Color.red;
                }

                //tileData.setTokenNumber(board.numberTokens[numberTokenIndex]);


                numberTokenIndex++;
            }
        }
    }

    private void PlaceSettlements() {
        settlements = new Settlement[settlementWidth,settlementWidth];

        //iterate through tiles
        for (int x = 0; x < tiles.GetLength(0); x++) {
            for (int y = 0; y < tiles.GetLength(1); y++) {
                

                //if the tile is not water place top and bottom settlements
                if (tiles[x, y].resourceType != ResourceType.Water) {
                    
                    PlaceTopSettlement(tiles[x, y]);
                    PlaceBottomSettlement(tiles[x, y]);
                }
                //if the tile is water, look top left, top right, bottom left, bottom right and fill settlements in as needed.
                else {
                    Tile[] surroundingTiles = GetNeighbors(x,y);
                    //top left tile is there and it isnt water
                    if (surroundingTiles[(int)TileDirection.TopLeft] != null && surroundingTiles[(int)TileDirection.TopLeft].resourceType != ResourceType.Water) {
                        PlaceTopSettlement(tiles[x, y]);
                    }
                    //top right
                    else if (surroundingTiles[(int)TileDirection.TopRight] != null && surroundingTiles[(int)TileDirection.TopRight].resourceType != ResourceType.Water) {
                        PlaceTopSettlement(tiles[x, y]);
                    }
                    //bottom right
                    else if (surroundingTiles[(int)TileDirection.BottomRight] != null && surroundingTiles[(int)TileDirection.BottomRight].resourceType != ResourceType.Water) {
                        PlaceBottomSettlement(tiles[x, y]);
                    }
                    //bottom left
                    else if (surroundingTiles[(int)TileDirection.BottomLeft] != null && surroundingTiles[(int)TileDirection.BottomLeft].resourceType != ResourceType.Water) {
                        PlaceBottomSettlement(tiles[x, y]);
                    }
                }
            }
        }
    }

    private void PlaceTopSettlement(Tile tile) {
        int tileX = tile.x;
        int tileY = tile.y;
        int offset = tileY % 2;
        int x = (2 * tileX) + 1 + offset;
        int y = tileY;
        //print("Placing settlement around tile (" + x + "," + y + ")");
        //create game object, place in hierarchy, set name
        GameObject settlement = (GameObject)Instantiate(settlementPrefab);
        SettlementData settlementData = settlement.GetComponent<SettlementData>();
        settlementData.setPosition(x, y);

        settlement.transform.parent = settlementSpotsGameObject.transform;
        settlement.transform.name = "Settlement (" + x + "," + y + ")";

        //set settlement position
        Vector2 gridPos = new Vector2(tileX,tileY);
        Vector3 worldPos = CalcWorldPosTiles(gridPos);
        worldPos.y = worldPos.y + (hexHeight / 2);
        settlement.transform.position = worldPos;

        //tile.transform.position = CalcWorldPosTiles(gridPos);
        settlements[x, y] = new Settlement(settlement,x,y);

        

    }

    private void PlaceBottomSettlement(Tile tile) {
        int tileX = tile.x;
        int tileY = tile.y;
        int offset = tileY % 2;
        int x = (2 * tileX) + 1 + offset;
        int y = tileY + 1;
        //print("Placing settlement around tile (" + x + "," + y + ")");
        //create game object, place in hierarchy, set name


        GameObject settlement = (GameObject)Instantiate(settlementPrefab);
        SettlementData settlementData = settlement.GetComponent<SettlementData>();
        settlementData.setPosition(x, y);

        settlement.transform.parent = settlementSpotsGameObject.transform;
        settlement.transform.name = "Settlement (" + x + "," + y + ")";

        //set settlement position
        Vector2 gridPos = new Vector2(tileX, tileY);
        Vector3 worldPos = CalcWorldPosTiles(gridPos);
        worldPos.y = worldPos.y - (hexHeight / 2);
        settlement.transform.position = worldPos;


        settlements[x, y] = new Settlement(settlement, x, y);



    }

    private void PlaceRoads() {
        roads = new Road[roadCount, roadCount];

        for (int x = 0; x < tiles.GetLength(0); x++) {
            for (int y = 0; y < tiles.GetLength(1); y++) {

                if (tiles[x,y].resourceType != ResourceType.Water) {
                    PlaceRoadsOnAllSides(tiles[x,y]);
                }
                else {
                    Tile[] surroundingTiles = GetNeighbors(x, y);

                    if (surroundingTiles[(int)TileDirection.Left] != null && surroundingTiles[(int)TileDirection.Left].resourceType != ResourceType.Water) {
                        PlaceRoadsOnCertainlSides(tiles[x, y], TileDirection.Left);
                    }
                    //if (surroundingTiles[(int)TileDirection.TopLeft] != null && surroundingTiles[(int)TileDirection.TopLeft].resourceType != ResourceType.Water) {
                    //    PlaceRoadsOnCertainlSides(tiles[x, y], TileDirection.TopLeft);
                    //}
                    if (surroundingTiles[(int)TileDirection.BottomRight] != null && surroundingTiles[(int)TileDirection.BottomRight].resourceType != ResourceType.Water) {
                        PlaceRoadsOnCertainlSides(tiles[x, y], TileDirection.BottomRight);
                    }
                    if (surroundingTiles[(int)TileDirection.BottomLeft] != null && surroundingTiles[(int)TileDirection.BottomLeft].resourceType != ResourceType.Water) {
                        PlaceRoadsOnCertainlSides(tiles[x, y], TileDirection.BottomLeft);
                    }
                }


                //Tile[] surroundingTiles = GetNeighbors(x, y);
                
            }
        }

    }

    private void PlaceRoadsOnAllSides(Tile tile) {
        int tileX = tile.x;
        int tileY = tile.y;
        int offset = tileY % 2;
        int xLeft = 2 * tileX + offset;
        int yLeft = 2 * tileY + 1;
        //int xTopLeft = 2 * tileX + offset;
        //int yTopLeft = 2 * tileY;
        int xBottomRight = 2 * tileX + 1 + offset;
        int yBottomRight = 2 * tileY + 2;
        int xBottomLeft = 2 * tileX + offset;
        int yBottomLeft = 2 * tileY + 2;

        //left
        //      roads[2 * x + offset, 2 * y + 1],
        //    //top left
        //    roads[2 * x + offset, 2 * y],
        //    //top right
        //    roads[2 * x + 1 + offset, 2 * y],
        //    //right
        //    roads[2 * x + 2 + offset, 2 * y + 1],
        //    //bottom right
        //    roads[2 * x + 1 + offset, 2 * y + 2],
        //    //bottom left
        //    roads[2 * x + offset, 2 * y + 2]

        //left
        GameObject road = (GameObject)Instantiate(roadPrefab);
        road.transform.parent = roadSpotsGameObject.transform;

        //road.transform.parent = tile.gameObject.transform;
        road.transform.name = "Road (" + xLeft + "," + yLeft + ")";

        Vector2 gridPos = new Vector2(tileX, tileY);
        Vector3 worldPos = CalcWorldPosTiles(gridPos);
        worldPos.x = worldPos.x - ((hexWidth / 2)+(gap/2));
        road.transform.position = worldPos;
        

        roads[xLeft, yLeft] = new Road(road, xLeft, yLeft);

        ////top left
        //road = (GameObject)Instantiate(roadPrefab);
        //road.transform.parent = roadSpotsGameObject.transform;
        ////road.transform.parent = tile.gameObject.transform;

        //road.transform.name = "Road (" + xTopLeft + "," + yTopLeft + ")";

        //gridPos = new Vector2(tileX, tileY);
        //worldPos = CalcWorldPosTiles(gridPos);
        //worldPos.x = worldPos.x - (hexWidth / 4) - (gap/2);
        //worldPos.y = worldPos.y + (hexHeight /3) + (gap/2);
        //road.transform.position = worldPos;
        //road.transform.Rotate(0, 0, -60, Space.Self);
        //roads[xTopLeft, yTopLeft] = new Road(road, xTopLeft, yTopLeft);

        //top left
        road = (GameObject)Instantiate(roadPrefab);
        road.transform.parent = roadSpotsGameObject.transform;
        //road.transform.parent = tile.gameObject.transform;

        road.transform.name = "Road (" + xBottomRight + "," + yBottomRight + ")";

        gridPos = new Vector2(tileX, tileY);
        worldPos = CalcWorldPosTiles(gridPos);
        worldPos.x = worldPos.x + ((hexWidth / 4) - (gap / 2));
        worldPos.y = worldPos.y - (hexHeight / 3) - (gap );
        road.transform.position = worldPos;
        road.transform.Rotate(0, 0, -60, Space.Self);
        roads[xBottomRight, yBottomRight] = new Road(road, xBottomRight, yBottomRight);


        //bottom left
        road = (GameObject)Instantiate(roadPrefab);
        road.transform.parent = roadSpotsGameObject.transform;
        //road.transform.parent = tile.gameObject.transform;
        road.transform.Rotate(0, 0, 60, Space.Self);
        road.transform.name = "Road (" + xBottomLeft + "," + yBottomLeft + ")";

        gridPos = new Vector2(tileX, tileY);
        worldPos = CalcWorldPosTiles(gridPos);
        worldPos.x = worldPos.x - (hexWidth / 4) - (gap / 2);
        worldPos.y = worldPos.y - (hexHeight / 3)  - (gap);
        road.transform.position = worldPos;
        roads[xBottomLeft, yBottomLeft] = new Road(road, xBottomLeft, yBottomLeft);


        
    }

    private void PlaceRoadsOnCertainlSides(Tile tile, TileDirection tileDirection) {
        int tileX = tile.x;
        int tileY = tile.y;
        int offset = tileY % 2;
        //int xLeft = (2 * tileX) + offset;
        //int yLeft = 2 * tileY + 1;
        ////int xTopLeft = 2 * tileX + offset;
        ////int yTopLeft = 2 * tileY;
        //int xBottomRight = 2 * tileX + offset;
        //int yBottomRight = 2 * tileY + 2;
        //int xBottomLeft = 2 * tileX + 1 + offset;
        //int yBottomLeft = 2 * tileY + 2;


        //left
        if(tileDirection == TileDirection.Left) {
            int xLeft = 2 * tileX + offset;
            int yLeft = 2 * tileY + 1;
            GameObject road = (GameObject)Instantiate(roadPrefab);
            road.transform.parent = roadSpotsGameObject.transform;
            road.transform.name = "Road (" + xLeft + "," + yLeft + ")";

            Vector2 gridPos = new Vector2(tileX, tileY);
            Vector3 worldPos = CalcWorldPosTiles(gridPos);
            worldPos.x = worldPos.x - (hexWidth / 2);
            road.transform.position = worldPos;
            roads[xLeft, yLeft] = new Road(road, xLeft, yLeft);
        }

        ////top left
        //if (tileDirection == TileDirection.TopLeft) {          
        //    GameObject road = (GameObject)Instantiate(roadPrefab);
        //    road.transform.parent = roadSpotsGameObject.transform;
        //    road.transform.name = "Road (" + xLeft + "," + yLeft + ")";
      
        //    Vector2 gridPos = new Vector2(tileX, tileY);
        //    Vector3 worldPos = CalcWorldPosTiles(gridPos);
        //    worldPos.x = worldPos.x - (hexWidth / 4);
        //    worldPos.y = worldPos.y + (hexHeight / 3);
        //    road.transform.position = worldPos;
        //    road.transform.Rotate(0, 0, -60, Space.Self);
        //    roads[xTopLeft, yTopLeft] = new Road(road, xTopLeft, yTopLeft);
        //}
        

        //bottom right
        if (tileDirection == TileDirection.BottomRight) {
            int xBottomRight = 2 * tileX + 1 + offset;
            int yBottomRight = 2 * tileY + 2;
            GameObject road = (GameObject)Instantiate(roadPrefab);
            road.transform.parent = roadSpotsGameObject.transform;
            road.transform.name = "Road (" + xBottomRight + "," + yBottomRight + ")";

            Vector2 gridPos = new Vector2(tileX, tileY);
            Vector3 worldPos = CalcWorldPosTiles(gridPos);
            worldPos.x = worldPos.x + ((hexWidth / 4) - (gap / 2));
            worldPos.y = worldPos.y - (hexHeight / 3) - (gap );
            road.transform.position = worldPos;
            road.transform.Rotate(0, 0, -60, Space.Self);
            roads[xBottomRight, yBottomRight] = new Road(road, xBottomRight, yBottomRight);
        }


        //bottom left
        if (tileDirection == TileDirection.BottomLeft) {
            int xBottomLeft = 2 * tileX + offset;
            int yBottomLeft = 2 * tileY + 2;
            GameObject road = (GameObject)Instantiate(roadPrefab);
            road.transform.parent = roadSpotsGameObject.transform;
            road.transform.name = "Road (" + xBottomLeft + "," + yBottomLeft + ")";

            Vector2 gridPos = new Vector2(tileX, tileY);
            Vector3 worldPos = CalcWorldPosTiles(gridPos);
            worldPos.x = worldPos.x - (hexWidth / 4);
            worldPos.y = worldPos.y - (hexHeight / 3) - (gap);
            road.transform.position = worldPos;
            road.transform.Rotate(0, 0, 60, Space.Self);
            roads[xBottomLeft, yBottomLeft] = new Road(road, xBottomLeft, yBottomLeft);
        }
        
    }

    public Tile[] GetNeighbors(Tile tile) {
        return GetNeighbors(tile.x, tile.y);
    }

    public Tile[] GetNeighbors(int x, int y) {

        //if center tiles in the grid
        if (x > 0 && x < gridWidth-1 && y > 0 && y < gridHeight-1) {
            //even y
            if (y % 2 == 0) {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    tiles[x-1,y-1],
                    //top right
                    tiles[x,y-1],
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    tiles[x,y+1],
                    //bottom left
                    tiles[x-1,y+1]
                };
            }
            //odd y
            else {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    tiles[x,y-1],
                    //top right
                    tiles[x+1,y-1],
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    tiles[x+1,y+1],
                    //bottom left
                    tiles[x,y+1]
                };
            }
        }
        //top left of the grid
        else if (x == 0 && y == 0) {
            if (y % 2 == 0) {
                return new[] {
                    //left tile
                    null,
                    //top left
                    null,
                    //top right
                    null,
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    tiles[x,y+1],
                    //bottom left
                    null
                };
            }
            //odd y
            else {
                return new[] {
                    //left tile
                    null,
                    //top left
                    null,
                    //top right
                    null,
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    tiles[x+1,y+1],
                    //bottom left
                    tiles[x,y+1]
                };
            }
        }
        //top right
        else if (x == gridWidth - 1 && y == 0) {
            //even y
            if (y % 2 == 0) {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    null,
                    //top right
                    null,
                    //right tile
                    null,
                    //bottom right
                    tiles[x,y+1],
                    //bottom left
                    tiles[x-1,y+1]
                };
            }
            //odd y
            else {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    null,
                    //top right
                    null,
                    //right tile
                    null,
                    //bottom right
                    null,
                    //bottom left
                    tiles[x,y+1]
                };
            }
        }
        //bottom right
        else if (x == gridWidth - 1 && y == gridHeight - 1) {
            //even y
            if (y % 2 == 0) {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    tiles[x-1,y-1],
                    //top right
                    tiles[x,y-1],
                    //right tile
                    null,
                    //bottom right
                    null,
                    //bottom left
                    null
                };
            }
            //odd y
            else {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    tiles[x,y-1],
                    //top right
                    null,
                    //right tile
                    null,
                    //bottom right
                    null,
                    //bottom left
                    null,
                };
            }
        }
        //bottom left
        else if (x == 0 && y == gridHeight - 1) {
            //even y
            if (y % 2 == 0) {
                return new[] {
                    //left tile
                    null,
                    //top left
                    null,
                    //top right
                    tiles[x,y-1],
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    null,
                    //bottom left
                    null
                };
            }
            //odd y
            else {
                return new[] {
                    //left tile
                    null,
                    //top left
                    tiles[x,y-1],
                    //top right
                    tiles[x+1,y-1],
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    null,
                    //bottom left
                    null
                };
            }
        }
        //top row
        else if (x > 0 && x < gridWidth - 1 && y == 0) {
            if (y % 2 == 0) {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    null,
                    //top right
                    null,
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    tiles[x,y+1],
                    //bottom left
                    tiles[x-1,y+1]
                };
            }
            //odd y
            else {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    null,
                    //top right
                    null,
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    tiles[x+1,y+1],
                    //bottom left
                    tiles[x,y+1]
                };
            }
        }
        //right column
        else if (x == gridWidth - 1 && y > 0 && y < gridHeight - 1) {
            if (y % 2 == 0) {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    tiles[x-1,y-1],
                    //top right
                    tiles[x,y-1],
                    //right tile
                    null,
                    //bottom right
                    tiles[x,y+1],
                    //bottom left
                    tiles[x-1,y+1]
                };
            }
            //odd y
            else {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    tiles[x,y-1],
                    //top right
                    null,
                    //right tile
                    null,
                    //bottom right
                    null,
                    //bottom left
                    tiles[x,y+1]
                };
            }
        }
        //bottom row
        else if (x > 0 && x < gridWidth - 1 && y == gridHeight - 1) {
            if (y % 2 == 0) {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    tiles[x-1,y-1],
                    //top right
                    tiles[x,y-1],
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    null,
                    //bottom left
                    null
                };
            }
            //odd y
            else {
                return new[] {
                    //left tile
                    tiles[x-1,y],
                    //top left
                    tiles[x,y-1],
                    //top right
                    tiles[x+1,y-1],
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    null,
                    //bottom left
                    null
                };
            }
        }
        //left column
        else if (x == 0 && y > 0 && y < gridHeight - 1) {
            if (y % 2 == 0) {
                return new[] {
                    //left tile
                    null,
                    //top left
                    null,
                    //top right
                    tiles[x,y-1],
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    tiles[x,y+1],
                    //bottom left
                    null
                };
            }
            //odd y
            else {
                return new[] {
                    //left tile
                    null,
                    //top left
                    tiles[x,y-1],
                    //top right
                    tiles[x+1,y-1],
                    //right tile
                    tiles[x+1,y],
                    //bottom right
                    tiles[x+1,y+1],
                    //bottom left
                    tiles[x,y+1]
                };
            }
        }
        else {
            return new[] {
                //left tile
                tiles[x,y],
                //top left
                tiles[x,y],
                //top right
                tiles[x,y],
                //right tile
                tiles[x,y],
                //bottom right
                tiles[x,y],
                //bottom left
                tiles[x,y]
            };
        }
    }

    public void checkIfNextToWater(int x, int y) {
        Tile[] surroundingTiles = GetNeighbors(tiles[x, y]);
        for (int i = 0; i < surroundingTiles.Length; i++) {
            TileData tileData = surroundingTiles[i].gameObject.GetComponent<TileData>();
            print("(" + surroundingTiles[i].x + "," + surroundingTiles[i].y + ") : " + tileData.getResourceType());
        }
    }

    public void highlightSurroundingRoads(int x, int y) {
        Road[] surroundingRoads = GetRoads(settlements[x, y]);
        for (int i = 0; i < surroundingRoads.Length; i++) {
            print("(" + surroundingRoads[i].getX() + "," + surroundingRoads[i].getY() + ") ");

            MeshRenderer meshRenderer = surroundingRoads[i].gameObject.GetComponentInChildren<MeshRenderer>();
            meshRenderer.material = highlightMat;
        }
    }

    public void testDelete(int x, int y) {
        StartCoroutine(DeleteTilesCoroutine(x, y));
    }

    IEnumerator DeleteTilesCoroutine(int x, int y) {
        Tile[] surroundingTiles = GetNeighbors(tiles[x, y]);
        for (int i = 0; i < surroundingTiles.Length; i++) {
            TileData tileData = surroundingTiles[i].gameObject.GetComponent<TileData>();

            print("(" + surroundingTiles[i].x + "," + surroundingTiles[i].y + ") : " + tileData.getResourceType());

            Destroy(surroundingTiles[i].gameObject);
            yield return new WaitForSeconds(1);

        }
    }

    public Settlement[] GetSettlements(Tile tile) {
        int x = tile.x;
        int y = tile.y;

        var offset = y % 2;

        //return each vertex from north eastern point clockwise

        return new[]
        {
            //top left
            settlements[2*x+offset,y],
            //top
            settlements[2*x+1+offset,y],
            //top right
            settlements[2*x+2+offset,y],
            //bottom right
            settlements[2*x+2+offset,y+1],
            //bottom
            settlements[2*x+1+offset,y+1],
            //bottom left
            settlements[2*x+offset,y+1]


        };

    }

    public Road[] GetRoads(int x, int y) {
        int offset = y % 2;
        return new[]
        {
            //left
            roads[2*x+offset,2*y+1],
            //top left
            roads[2*x+offset,2*y],
            //top right
            roads[2*x+1+offset,2*y],
            //right
            roads[2*x+2+offset,2*y+1],
            //bottom right
            roads[2*x+1+offset,2*y+2],
            //bottom left
            roads[2*x+offset,2*y+2]
        };

    }

    public Settlement[] GetEnds(Road road) {
        int x = road.getX();
        int y = road.getY();
        if (y % 2 == 0)
            return new[]
            {
                settlements[x,y/2],
                settlements[x+1,y/2]
            };
        else
            return new[]
            {
                settlements[x,(y-1)/2],
                settlements[x,(y+1)/2]
            };
    }

    public Road[] GetRoads(Settlement settlement) {
        int x = settlement.getX();
        int y = settlement.getY();
        if (x % 2 == y % 2) {
            return new[]
                {
                    roads[x-1,y*2],
                    roads[x,y*2],
                    roads[x,y*2+1]
                };
        }
        else {
            return new[]
            {
                    roads[x-1,y*2],
                    roads[x,y*2],
                    roads[x,y*2-1]
                };
        }
        //if (y % 2 == 0) {
        //    if (x % 2 == 0) {
        //        print("even x even y");
        //        return new[]
        //        {
        //            roads[x-1,y*2],
        //            roads[x,y*2],
        //            roads[x,y*2+1]
        //        };  
        //    }
        //    else {
        //        print("odd x even y");
        //        return new[]
        //            {
        //            roads[x-1,y*2],
        //            roads[x,y*2],
        //            roads[x,y*2-1]
        //            };
        //    }
        //}
        //else {
        //    if (x % 2 == 0) {
        //        print("even x odd y");
        //        return new[]
        //        {
        //            roads[x-1,y*2],
        //            roads[x,y*2],
        //            roads[x,y*2-1]
        //        };
        //    }
        //    else {
        //        print("odd x odd y");
        //        return new[]
        //            {
        //            roads[x-1,y*2],
        //            roads[x,y*2],
        //            roads[x,y*2+1]
        //            };
        //    }
        //}

        
    }

    public Tile[] GetHexes(Settlement settlement) {
        int x = settlement.getX();
        int y = settlement.getY();
        var xoffset = x % 2;
        var yoffset = y % 2;

        return new[]
        {
            tiles[(x+yoffset)/2-1,y-1],
            tiles[(x-1)/2,y-(1-xoffset)*yoffset],
            tiles[(x-yoffset)/2,y],
        };
    }

}