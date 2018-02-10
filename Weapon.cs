using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{

    public float fireRate = 0; //fire rate; float to hold down the mouse button to fire in intervals, instead of making it boolean, we check for fire rate to see if it's singe burst
    public int damage = 10;
    public LayerMask whatToHit; 
    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;
    public Transform HitPrefab;
    float timeToSpawnEffect = 0;
    public float effectSpawnRate = 10;

    public float camShakeAmt = 0.05f;//handle camera shaking
    public float camShakeLength = 0.1f;
    CameraShake camShake;//a refference

    float timeToFire = 0;
    Transform firePoint; //it will store our fire point
	
	void Awake ()
    {
        firePoint = transform.FindChild("FirePoint");
        if (firePoint == null)
        {
            Debug.LogError("No firepoint? But why? Check your code...");
        }
	}

    void Start()
    {
       camShake = GameMaster.gm.GetComponent<CameraShake>();//we set gamemaster so we could always get the local instance by doing .gm and then .getcomponent to get the camerashake on the object
        if (camShake == null)
        {
            Debug.LogError("No Camera script found on GM object.");
        }
    }

	// Update is called once per frame
	void Update ()
    {
      
        //if our gun is single fire(pressing the mouse button will only fire once)
        if (fireRate == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }

        else //if it's an automatic weapon
        {
            if (Input.GetButton("Fire1") && Time.time > timeToFire)//first we check if we are holding down the button(check for the button call) and then also we check if timeToFire(used as the place in time where we will have our next shot
            {
                timeToFire = Time.time + 1 / fireRate; //timeToFire = current time + fireRate(for delay)
                Shoot();
            }
        }
  	}

    void Shoot()
    {
        // Debug.Log("Test");
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        //we are creeatin a new vector2, assigning it the screen to world point of mouse pos x and y. we will translate the positon of the mouse from screen coordinates to coordinates in the world. to make a raycast
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);//we are taking the firepoint(the point at the tip of the gun) and we are storing that position as a Vector2
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mousePosition - firePointPosition, 100, whatToHit);//raycast(origin, direction, distance, layer mask)

     
        Debug.DrawLine(firePointPosition, (mousePosition - firePointPosition) * 100, Color.cyan);

        if (hit.collider != null)
        {
            Debug.DrawLine(firePointPosition, hit.point, Color.red);
            Enemy enemy = hit.collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DamageEnemy(damage);
              //  Debug.Log("We hit" + hit.collider.name + " and did " + damage + " damage. ");
            }
        }

        if (Time.time >= timeToSpawnEffect)
        {
            //pass the hitPos
            Vector3 hitPos;
            Vector3 hitNormal;

            if (hit.collider == null)
            {
                hitPos = (mousePosition - firePointPosition) * 30;//if we don't hit anything contiune into space
                hitNormal = new Vector3(9999, 9999, 9999);
            }
            else
            {
                hitPos = hit.point;
                hitNormal = hit.normal;
            }

            //StartCoroutine("Effect");--for IEnumerator
            Effect(hitPos,hitNormal);
            timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
        }

    }

    void Effect(Vector3 hitPos, Vector3 hitNormal)
    {
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform; // we call instatiate to spawn something. 3 args: first what we want to spawn, the position of where we want to spawn it, the rotation
        LineRenderer lr = trail.GetComponent<LineRenderer>();

        if (lr != null)
        {
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, hitPos);
        }
        Destroy(trail.gameObject, 0.1f);

        if (hitNormal != new Vector3(9999, 9999, 9999))
        {
            Instantiate(HitPrefab, hitPos, Quaternion.FromToRotation(Vector3.right, hitNormal));
        }

        Transform clone = (Transform)Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation); 
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        //yield return 0;//make sure it's displayed for one frame ,then time is not needed in Destroy
        Destroy(clone.gameObject, 0.2f); //whenever we need to destroy transform we need to do gameObject

        //shake the camera
        camShake.Shake(camShakeAmt, camShakeLength);
    }

}
