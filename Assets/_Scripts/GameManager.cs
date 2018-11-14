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


    // Use this for initialization
    void Start () {

        //create players
        players = new Player[playerCount];
        turnOrder = new int[playerCount];

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
}
