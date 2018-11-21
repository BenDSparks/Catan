using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject playerPrefab;
    public GameObject gameBoard;
    private GridLogic gridLogic;
    private int playerCount = 4;
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
    bool isRollingDice = false;
    bool isDistributingCards = false;

    private DiceRoller diceRoller;
    private ResourceCardUI resourceCardUI;
   
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

        diceRoller = GetComponent<DiceRoller>();
        resourceCardUI = GameObject.Find("ResourcePanel").GetComponent<ResourceCardUI>();
        diceRoller.disableAll();

        //highlight spots that the player can place his first settlement
        isStartingPhase = true;
        StartCoroutine(StartingPhase());
        StartCoroutine(AutoPickSettlements());

	}

    public static class WaitFor {
        public static IEnumerator Frames(int frameCount) {
            while (frameCount > 0) {
                frameCount--;
                yield return null;
            }
        }
    }



    IEnumerator AutoPickSettlements() {
        yield return new WaitForSeconds(.5f);

        settlementClicked(6, 2);
        yield return StartCoroutine(WaitFor.Frames(1));
        roadClicked(6, 5);
        yield return StartCoroutine(WaitFor.Frames(1));

        settlementClicked(10, 2);
        yield return StartCoroutine(WaitFor.Frames(1));

        roadClicked(10, 5);
        yield return StartCoroutine(WaitFor.Frames(1));

        settlementClicked(10, 5);
        yield return StartCoroutine(WaitFor.Frames(1));

        roadClicked(9, 10);
        yield return StartCoroutine(WaitFor.Frames(1));


        settlementClicked(8, 5);
        yield return StartCoroutine(WaitFor.Frames(1));


        roadClicked(8, 10);
        yield return StartCoroutine(WaitFor.Frames(1));

        //second group
        settlementClicked(5, 3);
        yield return StartCoroutine(WaitFor.Frames(1));


        roadClicked(5, 7);
        yield return StartCoroutine(WaitFor.Frames(1));


        settlementClicked(11, 3);
        yield return StartCoroutine(WaitFor.Frames(1));


        roadClicked(11, 7);
        yield return StartCoroutine(WaitFor.Frames(1));


        settlementClicked(7, 4);
        yield return StartCoroutine(WaitFor.Frames(1));

        roadClicked(6, 8);
        yield return StartCoroutine(WaitFor.Frames(1));


        settlementClicked(9, 4);
        yield return StartCoroutine(WaitFor.Frames(1));


        roadClicked(9, 7);
        yield return StartCoroutine(WaitFor.Frames(1));

    }

  
    private IEnumerator GameLoop() {

        //if has knight card, ask if you want to use it
        bool hasKnight = false;
        for (int i = 0; i < players[turnIndex].developmentCards.Count; i++) {
            if (players[turnIndex].developmentCards[i] == DevelopmentCard.Knight) {
                //ask player 1 if they want to use a knight card
                yield return StartCoroutine(AskToPlayKnight());

                break;
            }
        }
        //ask them to roll the dice and wait for the roll
        //diceRoller.gameObject.SetActive(false);
        isRollingDice = true;
        diceRoller.enableAll();

        yield return new WaitUntil(() => !isRollingDice);



        //resolve robber if it is rolled
        if (diceRoller.diceTotal == 7) {
            for (int i = 0; i < players.Length; i++) {
                //go through all players on a 7, if one has more then 7 cards, they get robbed
                if (players[i].cardCount > 7) {
                    yield return StartCoroutine(RobPlayers());
                    break;
                }
                
                
            }
        }
        else {
            //give resources to everyone remebering to not give any robbed space resources
            yield return StartCoroutine(GiveOutResources());
        }
        //building trading


        //increment turn and change ui to new person
        turnIndex++;
        if(turnIndex == players.Length) {
            turnIndex = 0;
        }
        resourceCardUI.setResources(players[turnIndex]);
        print("Players turn: " + turnIndex);
        StartCoroutine(GameLoop());


    }

    public void RollTheDice() {
        //print("Rolling Dice!");
        diceRoller.RollTheDice();
        print(diceRoller.diceValues[0] + " " + diceRoller.diceValues[1] + " = " + diceRoller.diceTotal);
        diceRoller.disableRollDiceButton();
        isRollingDice = false;
    }

    IEnumerator RobPlayers() {
        print("Robbing Players");
        yield return new WaitForSeconds(0);
    }

    IEnumerator AskToPlayKnight() {
        print("Would you like to play a knight?");

        yield return new WaitForSeconds(0);
    }

    void GiveResources(int x, int y) {
        Tile[] surroundingTiles = gridLogic.GetTilesAroundSettlement(gridLogic.settlements[x, y]);

        for (int i = 0; i < surroundingTiles.Length; i++) {
            if (surroundingTiles[i] != null) {

                switch (surroundingTiles[i].resourceType) {
                    case ResourceType.Brick:
                        players[turnIndex].brickCount++;
                        break;
                    case ResourceType.Wood:
                        players[turnIndex].woodCount++;
                        break;
                    case ResourceType.Sheep:
                        players[turnIndex].sheepCount++;
                        break;
                    case ResourceType.Wheat:
                        players[turnIndex].wheatCount++;
                        break;
                    case ResourceType.Ore:
                        players[turnIndex].oreCount++;
                        break;
                    case ResourceType.Desert:
                        break;
                    case ResourceType.Water:
                        break;
                    default:
                        break;
                }
            }
        }
    }

    IEnumerator GiveOutResources() {

        //go through all the players settlements and check if they are next to the number rolled
        for (int i = 0; i < players.Length; i++) {
            for (int j = 0; j < players[i].settlements.Count; j++) {
                int amount = 1;
                if (players[i].settlements[j].isCity) {
                    amount++;
                }

                Tile[] surroundingTiles = gridLogic.GetTilesAroundSettlement(players[i].settlements[j]);

                for (int k = 0; k < surroundingTiles.Length; k++) {
                    if (surroundingTiles[k] != null) {
                        if (surroundingTiles[k].resourceNumber == diceRoller.diceTotal) {
                            if (surroundingTiles[k].isRobbed == false) {
                                switch (surroundingTiles[k].resourceType) {
                                    case ResourceType.Brick:
                                        players[i].brickCount += amount;
                                        break;
                                    case ResourceType.Wood:
                                        players[i].woodCount += amount;
                                        break;
                                    case ResourceType.Sheep:
                                        players[i].sheepCount += amount;
                                        break;
                                    case ResourceType.Wheat:
                                        players[i].brickCount += amount;

                                        break;
                                    case ResourceType.Ore:
                                        players[i].oreCount += amount;
                                        break;
                                    case ResourceType.Desert:
                                        break;
                                    case ResourceType.Water:
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }

                players[i].cardCount += amount;
            }

            testResourceAmount(i);
        }

        //reset the ui for new cards
        resourceCardUI.setResources(players[turnIndex]);
        yield return new WaitForSeconds(0);

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

            print("Giving player " + i + " starting resources");
            GiveResources(players[turnIndex].settlements[1].getX(), players[turnIndex].settlements[1].getY());


            testResourceAmount(turnIndex);
        }



        //go to starting player
        turnIndex = 0;
        resourceCardUI.setResources(players[turnIndex]);
        isStartingPhase = false;
        StartCoroutine(GameLoop());
    }

    void testResourceAmount(int playerNumber) {
        print("Player " + playerNumber + "'s Cards:");
        print("Bricks: " + players[playerNumber].brickCount);
        print("Wood: " + players[playerNumber].woodCount);
        print("Sheep: " + players[playerNumber].sheepCount);
        print("Wheat: " + players[playerNumber].wheatCount);
        print("Ore: " + players[playerNumber].oreCount);
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
        if (player.settlmentCount > 0) {
            if (gridLogic.settlements[x,y].isAvailable) {
                print("(" + x + "," + y + ") settlement bought by player " + player.playerNumber);
                gridLogic.buySettlement(x, y,player.playerNumber);
                player.settlmentCount--;
                player.settlements.Add(gridLogic.settlements[x, y]);
                settlementPlaced = true;
            }
            
        }
        
    }

    public void buyRoadIfPossible(int x, int y, Player player) {
        //check if player has enough settlements
        if (player.roadCount > 0) {
            if (gridLogic.roads[x, y].isAvailable) {
                print("(" + x + "," + y + ") road bought by player " + player.playerNumber);
                gridLogic.buyRoad(x, y, player.playerNumber);
                player.roadCount--;
                player.roads.Add(gridLogic.roads[x, y]);
                roadPlaced = true;
            }

        }

    }
}
