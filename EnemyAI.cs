using UnityEngine;
using System.Collections;
using Pathfinding; //link between script and A* Pathfinding - how to import it

[RequireComponent(typeof(Rigidbody2D))] 
[RequireComponent(typeof(Seeker))] //the component that allows the enemy to seek, the scrip from A* pathfinding algorithm

public class EnemyAI : MonoBehaviour {

    //What to chase
    public Transform target;

    //How many times each second we will update our path
    public float updateRate = 2f;

    //Caching
    private Seeker seeker;
    private Rigidbody2D rb;

    //store the calculated path
    public Path path;

    //The AI's speed per second
    public float speed = 300f;
    public ForceMode2D fMode;//force mode =  a way to change between force and impluse; how to control the way our force is applied to the rigidbody. Gives us control in the editor as how we want our enemy to behave

    [HideInInspector]
    public bool pathIsEnded = false;

    public float nextWaypointDistance = 3;//how close do we need to get to a waypoint befre it can continue to the next waypoint. max distance from the AI to a waypoint for it to continue to the next waypoint

    //The waypoint we are currently moving towards to
    private int currentWaypoint = 0;

    private bool searchingForPlayer = false;

    void Start()
    {
        seeker = GetComponent<Seeker>(); //asigning seeker
        rb = GetComponent<Rigidbody2D>(); //asigning the rigidboy

        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            Debug.LogError("No player found! Not good.");
            return;
        }

        //start a new path to the target position, return the result to the OnPathCOmplete method
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine(UpdatePath());//so the enemy will update their path when the player moves

    }

    IEnumerator SearchForPlayer()
    {
        GameObject sResult = GameObject.FindGameObjectWithTag("Player");
        if (sResult == null)
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(SearchForPlayer());
        }
        else
        {
            target = sResult.transform;
            Debug.Log("Searching for " + target.name);
            searchingForPlayer = false;
            StartCoroutine(UpdatePath());
            yield return false;
        }
    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            yield return false;
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds(1f / updateRate);
        StartCoroutine(UpdatePath());
        Debug.Log("Searching for " + target.name);
    }

    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path. DId it have an error?" + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        if (target == null)
        {
           if (!searchingForPlayer)
            {
                searchingForPlayer = true;
                StartCoroutine(SearchForPlayer());
            }
            return;
        }

        if (path == null)
        {
            return ;
        }

        if (currentWaypoint >= path.vectorPath.Count)
         //if we reached our final waypoint vector path returns all the waypoints. we are checkick if current way point is bigger than the ammount of waypoints in the list(Count). if it is it means we reached the end.
        {
            if (pathIsEnded)
            {
                return;
            }

            Debug.Log("End of path reached.");
            pathIsEnded = true;
            return;

        }

        pathIsEnded = false;

        //Direction to the next waypoint.vectorpath - list of waypoints
        Vector3 dir = ( path.vectorPath[currentWaypoint] - transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime; //speed bound to update steps

        //Moving the AI
        rb.AddForce(dir, fMode);
        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        if (dist < nextWaypointDistance)//if we are close to the next waypoint, and then it will follow it
        {
            currentWaypoint++;
            return;
        }
    }

 

}
