using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    //Declare variables
    //Initializes the timer and counter
    [SerializeField] Counter_Timer count;

    //Initializes GameBehaviour
    [SerializeField] GameBehaviour gameManager;

    //Initializes a place to hold the direction the player is moving
    [SerializeField] Vector3 direction;

    //Controls the faces on the slime
    [SerializeField] Face faces;

    //The slime's body
    [SerializeField] GameObject slimeBody;

    //The material for the face
    Material faceMaterial;

    //Allows the player to be moved or not
    [SerializeField] bool disabled;

    //Holds whether the player touched the flowers or not
    [SerializeField] bool touchedFlowers = false;

    //Holds whether or not the controls will be backwards
    [SerializeField] bool backward = false;

    //Holds whether script-controlled gravity is to be used
    [SerializeField] bool useGravity = true;

    //Used to make the turning angle transition smoothly
    private float smoothTime = 0.05f;
    private float currentVelocity;

    //Initializes the rigidbody and animator on the player
    private Rigidbody rb;
    private Animator anim;

    //Holds all of the illusion flowers
    GameObject[] flowers;

    //Shares the player's position to other scripts/classes
    public Vector3 slimePosition;

    //Start is called before the first frame update
    void Start()
    {
        //Get the rigidbody
        rb = GetComponent<Rigidbody>();

        //Get the animator
        anim = GetComponent<Animator>();

        //Allows the player to move
        disabled = false;

        //Gets the GameBehaviour script
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehaviour>();

        //Holds the position of the player
        slimePosition = transform.position;

        //Gets the face of the slime
        faceMaterial = slimeBody.GetComponent<Renderer>().materials[1];

        //Get all of the illusion flowers
        flowers = GameObject.FindGameObjectsWithTag("Illusion Flowers");

        //Enable all colliders of flowers
        for (int i = 0; i < flowers.Length; i++)
        {
            flowers[i].GetComponent<Collider>().enabled = true;
        }
    }

    //Update is called once per frame
    void Update()
    {
        //Make slimePosition the same as the player's position
        slimePosition = transform.position;

        //Set backward
        backward = inSeason("Spring", transform.position) && touchedFlowers;

        //If the player controls are not disabled ... 
        if (!disabled)
        {
            //Make the player jump
            if (Input.GetButtonDown("Jump"))
            {
                //Make animation for jumping
                anim.SetTrigger("Jump");
                rb.AddForce(transform.forward * 100);
            }

            //Get the horizontal component of movement
            float horizontalInput = Input.GetAxis("Horizontal");

            //Get the vertical component of movement
            float verticalInput = Input.GetAxis("Vertical");

            //If no input is given ...
            if (horizontalInput == 0 && verticalInput == 0)
            {
                //Keep player as is
                anim.SetFloat("Speed", 0);
                return;
            }

            //Declare the direction the player is moving in
            direction = new Vector3(horizontalInput, 0f, verticalInput);

            //Declare the angle the player will be facing in
            float targetAngle;
            
            //Makes the player's controls backwards
            if (backward)
            {
                //Make the angle the player is facing the opposite of the normal angle
                targetAngle = Mathf.Atan2(-direction.x, -direction.z) * Mathf.Rad2Deg;
            }

            //Makes the player's controls normal
            else
            {
                //Make the angle the player is facing the normal angle
                targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            }

            //Make the angle have a smooth transition
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
            
            //Make the character rotate
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            
            //Make the character move
            anim.SetFloat("Speed", Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
        }
    }

    //Called for physics updates
    void FixedUpdate()
    {
        //If gravity is being used ... 
        if (useGravity)
        {
            //Create own gravity
            rb.AddForce(Vector3.down * 100);
        }
    }

    //Collision detection using tags
    void OnCollisionEnter(Collision other)
    {
        //If player is touching water ...
        if (other.gameObject.tag == "Water")
        {
            //Make player swim
            anim.SetBool("Swim", true);
            rb.AddForce(transform.forward * 1000f);
        }

        //If player is not touching water ...
        else
        {
            //Make player not swim
            anim.SetBool("Swim", false);
        }

        //If player is touching winter charm ...
        if (other.gameObject.tag == "Winter Collect")
        {
            //Deactivate the charm
            other.gameObject.SetActive(false);

            //Add to charm counter
            count.winterItem++;

            //Play sound effects
            collectSFX();

            //Add item was found in winter
            gameManager.winterArrList.Add("");

            //Start collection animation
            StartCoroutine(collectionAnimation());
        }

        //If player is touching spring charm ...
        if (other.gameObject.tag == "Spring Collect")
        {
            //Deactivate the charm
            other.gameObject.SetActive(false);

            //Add to charm counter
            count.springItem++;

            //Play sound effects
            collectSFX();

            //Add item was found in spring
            gameManager.springArrList.Add("");

            //Start collection animation
            StartCoroutine(collectionAnimation());
        }

        //If player is touching summer charm ...
        if (other.gameObject.tag == "Summer Collect")
        {
            //Deactivate the charm
            other.gameObject.SetActive(false);

            //Add to charm counter
            count.summerItem++;

            //Play sound effects
            collectSFX();

            //Add item was found in summer
            gameManager.summerArrList.Add("");

            //Start collection animation
            StartCoroutine(collectionAnimation());
        }

        //If player is touching autumn charm ...
        if (other.gameObject.tag == "Autumn Collect")
        {
            //Deactivate the charm
            other.gameObject.SetActive(false);

            //Add to charm counter
            count.autumnItem++;

            //Play sound effects
            collectSFX();

            //Add item was found in autumn
            gameManager.autumnArrList.Add("");

            //Start collection animation
            StartCoroutine(collectionAnimation());
        }

        //If player is touching pestle and mortar ...
        if (other.gameObject.tag == "Mix Collect")
        {
            //Deactivate the charm
            other.gameObject.SetActive(false);

            //Add to charm counter
            count.mixItem++;

            //Play sound effects
            collectSFX();

            //If player is in winter ...
            if (inSeason("Winter", transform.position))
            {
                //Add item was found in winter
                gameManager.winterArrList.Add("");
            }

            //If player is in spring ...
            else if (inSeason("Spring", transform.position))
            {
                //Add item was found in spring
                gameManager.springArrList.Add("");
            }

            //If player is in summer
            else if (inSeason("Summer", transform.position))
            {
                //Add item was found in summer
                gameManager.summerArrList.Add("");
            }

            //If player is in autumn
            else if (inSeason("Autumn", transform.position))
            {
                //Add item was found in autumn
                gameManager.autumnArrList.Add("");
            }

            //Start collection animation
            StartCoroutine(collectionAnimation());
        }

        //If player is touching potion bottle ...
        if (other.gameObject.tag == "Bottle Collect")
        {
            //Deactivate the charm
            other.gameObject.SetActive(false);

            //Add to charm counter
            count.bottleItem++;

            //Play sound effects
            collectSFX();

            //If player is in winter ...
            if (inSeason("Winter", transform.position))
            {
                //Add item was found in winter
                gameManager.winterArrList.Add("");
            }

            //If player is in spring ...
            else if (inSeason("Spring", transform.position))
            {
                //Add item was found in spring
                gameManager.springArrList.Add("");
            }

            //If player is in summer ...
            else if (inSeason("Summer", transform.position))
            {
                //Add item was found in summer
                gameManager.summerArrList.Add("");
            }

            //If player is in autumn ...
            else if (inSeason("Autumn", transform.position))
            {
                //Add item was found in autumn
                gameManager.autumnArrList.Add("");
            }

            //Start collection animation
            StartCoroutine(collectionAnimation());
        }

        //If player is touching illusion flowers ...
        if (other.gameObject.tag == "Illusion Flowers")
        {
            //Player has touched the flowers
            touchedFlowers = true;

            //Disable colliders on all illusion flowers
            for (int i = 0; i < flowers.Length; i++)
            {
                flowers[i].GetComponent<Collider>().enabled = false;
            }
        }

        //If player is touching winter to spring portal ...
        if (other.gameObject.tag == "Winter2Spring Portal")
        {
            //teleports the player in front of the spring to winter portal
            StartCoroutine(Teleport("Winter2Spring"));
        }

        //If player is touching winter to autumn portal ...
        else if (other.gameObject.tag == "Winter2Autumn Portal")
        {
            //teleports the player in front of the autumn to winter portal
            StartCoroutine(Teleport("Winter2Autumn"));
        }

        //If player is touching spring to winter portal ... 
        else if (other.gameObject.tag == "Spring2Winter Portal")
        {
            //teleports the player in front of the winter to spring portal
            StartCoroutine(Teleport("Spring2Winter"));
        }

        //If player is touching spring to summer portal ...
        else if (other.gameObject.tag == "Spring2Summer Portal")
        {
            //teleports player in front of the summer to spring portal
            StartCoroutine(Teleport("Spring2Summer"));
        }

        //If player is touching summer to spring portal ...
        else if (other.gameObject.tag == "Summer2Spring Portal")
        {
            //teleports player in front of the the spring to summer portal
            StartCoroutine(Teleport("Summer2Spring"));
        }

        //If player is touching summer to autumn portal ...
        else if (other.gameObject.tag == "Summer2Autumn Portal")
        {
            //teleports the player in front of the autumn to summmer portal
            StartCoroutine(Teleport("Summer2Autumn"));
        }

        //If player is touching autumn to winter portal ... 
        else if (other.gameObject.tag == "Autumn2Winter Portal")
        {
            //teleports the player in front of the winter to autumn portal
            StartCoroutine(Teleport("Autumn2Winter"));
        }

        //If player is touching autumn to summer portal ...
        else if (other.gameObject.tag == "Autumn2Summer Portal")
        {
            //teleports the player in front of the summer to autumn portal
            StartCoroutine(Teleport("Autumn2Summer"));
        }
    }

    //Particle collision detection using tags
    void OnParticleCollision(GameObject other)
    {
        //If player is touching fire ...
        if (other.tag == "Fire")
        {
            //Teleport player to random location
            StartCoroutine(Teleport("Random"));
        }
    }

    //Change slime's face
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    //Plays the sound of the charms being collected
    void collectSFX()
    {
        GetComponent<AudioSource>().Play();
    }

    //Teleports the player in front of one of the portals
    private IEnumerator Teleport(string location)
    {
        Vector3[] pos = { new Vector3(505f, 144.5f, 85f), new Vector3(1065f, 144.5f, -452f), new Vector3(505f, 144.5f, -85f), new Vector3(1058f, 144.5f, 460f), new Vector3(932f, 144.5f, 463.1f), new Vector3(1426f, 144.5f, -70f), new Vector3(890f, 144.5f, -452.2f), new Vector3(1426f, 144.5f, 100f)};
        int num;

        //Assigns each string the corresponding element in pos
        switch(location)
        {
            case "Winter2Spring":
                num = 0;
                break;
            case "Winter2Autumn":
                num = 1;
                break;
            case "Spring2Winter":
                num = 2;
                break;
            case "Spring2Summer":
                num = 3;
                break;
            case "Summer2Spring":
                num = 4;
                break;
            case "Summer2Autumn":
                num = 5;
                break;
            case "Autumn2Winter":
                num = 6;
                break;
            case "Autumn2Summer":
                num = 7;
                break;
            default:
                num = getRandI(0, pos.Length-1);
                break;

        }

        //Disable player movements
        disabled = true;

        //Wait for 0.01 seconds
        yield return new WaitForSeconds(0.01f);

        //Teleports the player to the chosen position
        gameObject.transform.position = pos[num];

        //Wait for 0.01 seconds
        yield return new WaitForSeconds(0.01f);

        //Re-enable player movements
        disabled = false;
    }

    //Player celebrates finding a charm
    private IEnumerator collectionAnimation()
    {
        //Disable player movements
        disabled = true;

        //Make 
        bool look = true;

        if (look)
        {
            transform.LookAt(Camera.main.transform);
        }

        /*float yRotation;
        if (Mathf.Abs(transform.rotation.y) == 180)
        {
            yRotation = 180;
        }
        else
        {
            yRotation = 360-Mathf.Abs(transform.rotation.y);
        }

        transform.Rotate(0, yRotation, 0);*/
        yield return new WaitForSeconds(0.1f);

        SetFace(faces.WalkFace);
        anim.SetTrigger("Jump");

        yield return new WaitForSeconds(1f);
        SetFace(faces.Idleface);
        disabled = false;

        look = false;
    }

    //Returns a Random Number as a float in the Range 
    float getRandF(float min, float max)
    {
        //Return a random float
        return Random.Range(min, max);
    }

    //Returns a Random Number as an int in the Range
    int getRandI(float min, float max)
    {
        //Return a random int
        return (int)getRandF(min, max);
    }

    //Returns whether the position is in a season
    private bool inSeason(string season, Vector3 pos)
    {
        switch (season)
        {
            //Returns if position is in winter
            case "Winter": case "winter":
                return inRange(0, 990, pos.x) && inRange(-1000, -90, pos.z);

            //Returns if position is in spring
            case "Spring": case "spring":
                return inRange(0, 990, pos.x) && inRange(90, 1000, pos.z);

            //Returns if position is in summer
            case "Summer": case "summer":
                return inRange(1090, 2000, pos.x) && inRange(90, 1000, pos.z);

            //Returns if position is in autumn
            case "Autumn": case "autumn":
                return inRange(1090, 2000, pos.x) && inRange(-1000, -90, pos.z);
        }

        return false;
    }

    //Returns whether is not position in in a square of coordinates
    private bool inSquare(float minX, float maxX, float minZ, float maxZ, Vector3 pos)
    {
        return inRange(minX, maxX, pos.x) && inRange(minZ, maxZ, pos.z);
    }

    //Return whether the number is within the minimum and maximum values
    private bool inRange(float min, float max, float num)
    {
        return num <= max && num >= min;
    }
}