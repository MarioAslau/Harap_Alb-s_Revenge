using UnityEngine;
using System.Collections;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;
    private float[] parallaxScales; //store all of the parallax scales, the proportion of the camera's movement to move the objects by
    public float smoothing = 1f;// How smooth the parallax is going to be(parralaxing amount). Make sure to set this above 0
    
    private Transform cam; 
    private Vector3 previousCamPos;//x,y,z value; will store the position of the camera in the previous frame

   
    void Awake()
    {
        cam = Camera.main.transform;   
    }


    
   
	void Start ()
    {
        //The previous  frame had the current frame's camera position
        previousCamPos = cam.position;

        //the paralax scales will be as long as the backgrounds, loop through backgrounds and assign the z values to the scales
        parallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;  
        }
	}
	
	void Update ()
    {

        for (int i = 0; i < backgrounds.Length; i++)
        {

            //the parallax is the opposite of the  camera movement, the previous frame multiplied by the scale
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];
            //the parallax in effect should be the different between what our camera's position is now and what it was before

            float backgroundTargetPosX = backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, backgrounds[i].position.y, backgrounds[i].position.z);

            //fade between current position and the target position using lerp
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime); //Time.deltaTime converts frames to seconds
 
        }

        //set the previousCamPos to the camera's positon at the end of the frame
        previousCamPos = cam.position;
	}
}
