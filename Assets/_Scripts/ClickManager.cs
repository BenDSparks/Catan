using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ClickManager : MonoBehaviour {

    public Camera camera;
    public GameObject gameBoard;
    private GridLogic gridLogic;
    private GameManager gameManager;

    public GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    public EventSystem m_EventSystem;

    // Use this for initialization
    void Start () {
        gridLogic = gameBoard.GetComponent<GridLogic>();
        gameManager = GetComponent<GameManager>();

        
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)){

            //Set up the new Pointer Event
            m_PointerEventData = new PointerEventData(m_EventSystem);
            //Set the Pointer Event Position to that of the mouse position
            m_PointerEventData.position = Input.mousePosition;

            //Create a list of Raycast Results
            List<RaycastResult> results = new List<RaycastResult>();

            //Raycast using the Graphics Raycaster and mouse click position
            m_Raycaster.Raycast(m_PointerEventData, results);

            //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
            foreach (RaycastResult result in results) {
                Debug.Log("Hit UI " + result.gameObject.name);
            }


            if(results.Count > 0) {
                return;
            }









            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits;
            Vector3 forward = transform.TransformDirection(Vector3.forward) * 100;

            //Ray ray = new Ray(camera.ScreenToWorldPoint(Input.mousePosition), forward);



            //Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            //Debug.DrawRay(camera.ScreenToWorldPoint(Input.mousePosition), forward,Color.red, 1);
            Debug.DrawRay(ray.origin,ray.direction);

            //get all the raycast hits and order them by which was hit first
            hits = Physics.RaycastAll(ray).OrderBy(h => h.distance).ToArray(); ;
            
            //print("Length " + hits.Length);
            for (int i = 0; i < hits.Length; i++){
                //print(i);
                RaycastHit hit = hits[i];
                //print("i: " + i + " " + hit.transform.tag);
                
                if (hit.transform.tag == "Settlement") {
                    print("Settlment clicked");
                    Transform settlementTransform = hit.transform.parent;
                    SettlementData settlementData = settlementTransform.GetComponent<SettlementData>();
                    int x = settlementData.getX();
                    int y = settlementData.getY();

                    gameManager.settlementClicked(x, y);
                    //print("Settlement: (" + x + "," + y + ")");
                    //gridLogic.highlightSurroundingRoads(settlementData.getX(), settlementData.getY());
                    //gridLogic.highlightSurroundingSettlements(settlementData.getX(), settlementData.getY());
                    //gridLogic.highlightSurroundingTiles(settlementData.getX(), settlementData.getY());
                    //break;
                }
                if(hit.transform.tag == "Road") {
                    print("Road clicked");
                    Transform roadTransform = hit.transform.parent;
                    RoadData settlementData = roadTransform.GetComponent<RoadData>();
                    int x = settlementData.getX();
                    int y = settlementData.getY();

                    gameManager.roadClicked(x, y);
                    //break;
                }
                if (hit.transform.tag == "Tile"){
                    print("tile clicked");
                    //Transform tileTransform = hit.transform.parent;

                    //TileData tileData = tileTransform.GetComponent<TileData>();
                    //int x = tileData.getX();
                    //int y = tileData.getY();
                    //print(string.Concat("Center Tile: (", x, ",", y, ") ",tileData.getTokenNumber(), " ", tileData.getResourceType()));


                    ////gridLogic.checkIfNextToWater(x, y);
                    ////deletes 
                    ////gridLogic.testDelete(x, y);


                    //break;
                }
                if (hit.transform.tag == "Background") {
                    print("Background clicked");
                    gameManager.backgroundClicked();
                    break;
                }
                
            }

           
        }
       
    }
}
