using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject gameBoard;
    private GridLogic gridLogic;
    public int playerCount = 4;
    private Player[] players;
    private int[] turnOrder;

    int settlementMax = 5;
    int cityMax = 4;
    int roadMax = 15;

    // Use this for initialization
    void Start () {

        //create players
        players = new Player[playerCount];
        turnOrder = new int[playerCount];

        gridLogic = gameBoard.GetComponent<GridLogic>();

        print(gridLogic);

        for (int i = 0; i < turnOrder.Length; i++) {
            turnOrder[i] = i;
        }

        for (int i = 0; i < playerCount; i++) {
            GameObject playerGameObject = (GameObject)Instantiate(playerPrefab);
            playerGameObject.transform.name = "Player " + (i + 1);
            players[i] = new Player(playerPrefab, i);

            //settlement.transform.parent = settlementSpotsGameObject.transform;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void settlementClicked(int x, int y) {
        buySettlementIfPossible(x, y, players[0]);
    }
    
    public void backgroundClicked() {
        gridLogic.resetSettlementColors();
    }

    public void buySettlementIfPossible(int x, int y, Player player) {
        //check if player has enough settlements
        if (player.settlments > 0) {
            if (gridLogic.settlements[x,y].isAvailable) {
                print("(" + x + "," + y + ")");
                gridLogic.buySettlement(x, y);
            }
            
        }
        
    }
}
