using UnityEngine;
using System.Collections;


[RequireComponent(typeof(SpriteRenderer))]



public class Tiling : MonoBehaviour {

    public int offsetX = 2; 

    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;

    public bool reverseScale = false;

    private float spriteWidth = 0f; 
    private Camera cam; 
    private Transform myTransform; 

    void Awake() 
    {
        cam = Camera.main;
        myTransform = transform;
       
    }

    void Start ()
    {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x; 

    }
	
	void Update ()
    {

        if (hasALeftBuddy == false || hasARightBuddy == false)
        {
            //calculate the camera's extend( half the width) of what the camera can see in world coordinates 
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

            //we are going to calculate the x position, where the camera can see the edge of the sprite(element)
            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend; //factoring in our own positon ,adding half the sprite widht(spriteextend) and subtracting the camhorizontal extend to figure out the position where we would intersect
            float edgeVisiblePositionLeft = (myTransform.position.x + spriteWidth / 2) + camHorizontalExtend;

            if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false)
            {
                MakeNewbuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft - offsetX && hasALeftBuddy == false)
            {
                MakeNewbuddy(-1);
                hasALeftBuddy = true;
            }
        }

	}

    void MakeNewbuddy(int rightOrLeft)
    {
       
        Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform; 

        if (reverseScale == true) 
        {
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = myTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiling>().hasALeftBuddy = true; 
        }
        else
        {
            newBuddy.GetComponent<Tiling>().hasARightBuddy = true;
        }

    }
}
