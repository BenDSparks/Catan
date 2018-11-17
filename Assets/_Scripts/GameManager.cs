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
	}
	
	// Update is called once per frame
	void Update () {
        if (isStartingPhase) {
            if (settlementPlaced) {
                //highlight roads around it
                
                //check if road was placed
            }
        }
	}

    public void settlementClicked(int x, int y) {

        if (isStartingPhase) {

            //buy settlement in available spaces
            buySettlementIfPossible(x, y, players[turnIndex]);
            


        }



        
    }
    
    public void backgroundClicked() {
        gridLogic.resetSettlementColors();
    }

    public void buySettlementIfPossible(int x, int y, Player player) {
        //check if player has enough settlements
        if (player.settlments > 0) {
            if (gridLogic.settlements[x,y].isAvailable) {
                print("(" + x + "," + y + ")");
                gridLogic.buySettlement(x, y,player.playerNumber);
            }
            
        }
        
    }
}
