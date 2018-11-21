using UnityEngine;
 
public class CameraDrag : MonoBehaviour
{
     private Vector3 ResetCamera;
     private Vector3 Origin;
     private Vector3 Diference;

     void Start(){
         ResetCamera = Camera.main.transform.position;
     }
     void LateUpdate(){
         if(Input.GetMouseButtonDown(1)){
             Origin = MousePos();
         }
         if (Input.GetMouseButton(1)){
             Diference = MousePos() - transform.position;
             transform.position = Origin - Diference;
         }
         if (Input.GetMouseButton(2)){
             transform.position = ResetCamera;
         }
     }
     // return the position of the mouse in world coordinates (helper method)
     Vector3 MousePos(){
         return Camera.main.ScreenToWorldPoint(Input.mousePosition);
     }
 
}