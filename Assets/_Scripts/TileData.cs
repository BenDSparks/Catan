using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour {


    private int tokenNumber;
    private int x, y;
    private ResourceType resourceType;

    public void setTokenNumber(int number){
        tokenNumber = number;
    }

    public void setPosition(int x, int y){
        this.x = x;
        this.y = y;
    }

    public int getX(){
        return x;
    }

    public int getY(){
        return y;
    }

    public int getTokenNumber(){
        return tokenNumber;
    }

    public void setResourceType(ResourceType resourceType){
        this.resourceType = resourceType;
    }

    public ResourceType getResourceType(){
        return resourceType;
    }
	//// Use this for initialization
	//void Start () {
		
	//}
	
	//// Update is called once per frame
	//void Update () {
		
	//}
}
