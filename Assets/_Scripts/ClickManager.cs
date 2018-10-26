using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour {

    public Camera camera;
    public GameObject GameBoard;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0)){
            RaycastHit[] hits;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            hits = Physics.RaycastAll(ray);

            for (int i = 0; i < hits.Length; i++){
                RaycastHit hit = hits[i];
                if (hit.transform.tag == "Tile")
                {
                    Transform objectHit = hit.transform;

                    TileData tileData = objectHit.GetComponent<TileData>();

                    print(string.Concat("Clicked Tile", tileData.getX(), ",", tileData.getY()));
                    break;
                }
            }

           
        }
       
    }
}
