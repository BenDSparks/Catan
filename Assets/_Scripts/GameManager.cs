using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject gameBoard;
    private GridLogic gridLogic;
    public int playerCount = 4;
    private Player[] players;
    //private int[] turnOrder;
    private int turnIndex = 0;

    int settlementMax = 5;
    int cityMax = 4;
    int roadMax = 15;

    bool isStartingPhase = false;
    bool settlementPlaced = false;
    bool roadPlaced = false;

    bool isPlaceingSettlement = false;
    bool isPlaceingRoad = false;

    // Use this for initialization
    void Start () {

        //create players
        players = new Player[playerCount];
        //turnOrder = new int[playerCount];

        gridLogic = gameBoard.GetComponent<GridLogic>();

        print(gridLogic);

        //for (int i = 0; i < turnOrder.Length; i++) {
        //    turnOrder[i] = i;
        //}

        for (int i = 0; i < playerCount; i++) {
            GameObject playerGameObject = (GameObject)Instantiate(playerPrefab);
            playerGameObject.transform.name = "Player " + (i + 1);
            players[i] = new Player(playerPrefab, i);

            //settlement.transform.parent = settlementSpotsGameObject.transform;
        }

        //highlight spots that the player can place his first settlement
        isStartingPhase = true;
        StartCoroutine(StartingPhase());
	}

    private IEnumerator GameLoop() {
        print("In game loop");
        

        yield return new WaitForSeconds(0);

        StartCoroutine(GameLoop());
    }

    IEnumerator StartingPhase() {
        gridLogic.resetRoadColors();
        //go through the players and set first settlement spot. Goes in order 0-4
        for (int i = 0; i < players.Length; i++) {
            //set the player turn to player i
            turnIndex = i;


            //player is placing settlement
            isPlaceingSettlement = true;
            settlementPlaced = false;
            gridLogic.highlightAvailableStartingSettlementSpots();
            print( "Waiting for player " + i + " first settlement to be Placed.");
            yield return new WaitUntil(() => settlementPlaced);
            Debug.Log("Settlement Placed");
            isPlaceingSettlement = false;
            roadPlaced = false;
            isPlaceingRoad = true;
            print("Waiting for player " + i + " first road to be placed");
            
            
            yield return new WaitUntil(() => roadPlaced);
            isPlaceingRoad = false;
            Debug.Log("Road Placed");

        }

        for (int i = players.Length - 1; i >= 0; i--) {
            turnIndex = i;
            Debug.Log("Waiting for player " + i + " second settlement to be Placed.");
            isPlaceingSettlement = true;
            settlementPlaced = false;
            gridLogic.highlightAvailableStartingSettlementSpots();
            yield return new WaitUntil(() => settlementPlaced);
            Debug.Log("Settlement Placed");
            isPlaceingSettlement = false;
            roadPlaced = false;
            isPlaceingRoad = true;
            print("Waiting for player " + i + " second road to be placed");


            yield return new WaitUntil(() => roadPlaced);
            settlementPlaced = false;
            roadPlaced = false;
            print("Road Placed");
        }

        //go to starting player
        turnIndex = 0;
        isStartingPhase = false;
        StartCoroutine(GameLoop());
    }

    // Update is called once per frame
    void Update () {
        //if (isStartingPhase) {
        //    if (settlementPlaced) {
        //        //highlight roads around it
                
        //        //check if road was placed
        //    }
        //}
	}

    public void settlementClicked(int x, int y) {
        print("settlement (" + x + "," + y + ")");

        if (isStartingPhase) {
            if (isPlaceingSettlement) {
                //buy settlement in available spaces
                buySettlementIfPossible(x, y, players[turnIndex]);

                if (settlementPlaced) {
                    gridLogic.highlightStartingRoads(x,y);
                }
               
            }
        }  
    }

    public void roadClicked(int x, int y) {
        print("Road clicked (" + x + "," + y + ")");
        if (isStartingPhase) {
            if (isPlaceingRoad) {
                //buy settlement in available spaces
                buyRoadIfPossible(x, y, players[turnIndex]);
                   
                
                
            }
        }
    }

    public void backgroundClicked() {
        //gridLogic.resetSettlementColors();
    }

    public void buySettlementIfPossible(int x, int y, Player player) {
        //check if player has enough settlements
        if (player.settlments > 0) {
            if (gridLogic.settlements[x,y].isAvailable) {
                print("(" + x + "," + y + ") settlement bought by player " + player.playerNumber);
                gridLogic.buySettlement(x, y,player.playerNumber);
                player.settlments--;
                settlementPlaced = true;
                
            }
            
        }
        
    }
    public void buyRoadIfPossible(int x, int y, Player player) {
        //check if player has enough settlements
        if (player.roads > 0) {
            if (gridLogic.roads[x, y].isAvailable) {
                print("(" + x + "," + y + ") road bought by player " + player.playerNumber);
                gridLogic.buyRoad(x, y, player.playerNumber);
                player.roads--;
                roadPlaced = true;
            }

        }

    }
}
