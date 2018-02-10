using UnityEngine;
using System.Collections;

public class ArmRotation : MonoBehaviour {

    public int rotationOffset = 90;

    void Update()
    {
    
    Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    difference.Normalize();//Normalizing the vector. Maning that all the sum of the vector will be equal to 1.

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
    //finding the angle: we are making the vector between (0,0) and (2,2), then we are finding the angle between the X axis and the vector, and the we are converting the angle from Radians to Degrees
    //Mathf.Atan2(difference.y, difference.x) - finding the angle
    //Mathf.Rad2Deg - converting to degrees

    //we apply the rotation
    transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rotationOffset);
        //Quaternion.Euler - we pass it as degrees not radians
   
    }  
}
