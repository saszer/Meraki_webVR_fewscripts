using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerazoomWX : MonoBehaviour
{
 float curZoomPos, zoomTo; // curZoomPos will be the value
     float zoomFrom = 20f; //Midway point between nearest and farthest zoom values (a "starting position")
     
 
     void Update ()
     {
         // Attaches the float y to scrollwheel up or down
         float y = Input.mouseScrollDelta.y;
 
         // If the wheel goes up it, decrement 5 from "zoomTo"
         if (y >= 1)
         {
             zoomTo -= 5f;
             Debug.Log ("Zoomed In");
         }
 
         // If the wheel goes down, increment 5 to "zoomTo"
         else if (y >= -1) {
             zoomTo += 5f;
             Debug.Log ("Zoomed Out");
         }
 
         // creates a value to raise and lower the camera's field of view
         curZoomPos =  zoomFrom + zoomTo;
 
         curZoomPos = Mathf.Clamp (curZoomPos, 5f, 35f);
 
         // Stops "zoomTo" value at the nearest and farthest zoom value you desire
         zoomTo = Mathf.Clamp (zoomTo, -15f, 30f);
 
         // Makes the actual change to Field Of View
         GetComponent<Camera>().fieldOfView = curZoomPos;
}
}