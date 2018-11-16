﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour {

    public Camera camera;
    public GameObject gameBoard;
    private GridLogic gridLogic;

	// Use this for initialization
	void Start () {
        gridLogic = gameBoard.GetComponent<GridLogic>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)){
            RaycastHit[] hits;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray);

            for (int i = 0; i < hits.Length; i++){
                RaycastHit hit = hits[i];

                if (hit.transform.tag == "Settlement") {
                    Transform settlementTransform = hit.transform.parent;
                    SettlementData settlementData = settlementTransform.GetComponent<SettlementData>();
                    int x = settlementData.getX();
                    int y = settlementData.getY();
                    print("Settlement: (" + x + "," + y + ")");
                    gridLogic.highlightSurroundingRoads(settlementData.getX(), settlementData.getY());
                    break;
                }
                if (hit.transform.tag == "Tile"){
                    Transform tileTransform = hit.transform.parent;

                    TileData tileData = tileTransform.GetComponent<TileData>();
                    int x = tileData.getX();
                    int y = tileData.getY();
                    print(string.Concat("Center Tile: (", x, ",", y, ") ",tileData.getTokenNumber(), " ", tileData.getResourceType()));


                    gridLogic.checkIfNextToWater(x, y);
                    //deletes 
                    //gridLogic.testDelete(x, y);


                    break;
                }
                
            }

           
        }
       
    }
}
