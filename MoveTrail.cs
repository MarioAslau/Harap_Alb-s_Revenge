using UnityEngine;
using System.Collections;

public class MoveTrail : MonoBehaviour {

    public int moveSpeed = 230;

	// Update is called once per frame
	void Update ()
    {

        transform.Translate(Vector3.right * Time.deltaTime * moveSpeed); 
        //used for moving object over time. it's a way of moving things without using a RigidBody. Used when we want something in a straigh line without complicated stuff
        //deltaTime so it won't be affected by frameRate;

        //end timer so we don't have too many objects not visible but just moving
        Destroy(gameObject, 1);
	}
}
