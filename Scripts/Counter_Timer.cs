using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; //Import Text Mesh Pro

public class Counter_Timer : MonoBehaviour
{
    //Declare variables

    //Initializes GameBehaviour
    [SerializeField] GameBehaviour gameManager;

    //Initializes MoveCharacter
    [SerializeField] MoveCharacter player;

    //Initializes SoundControl
    [SerializeField] SoundControl sound;

    //For the timer
    //Initializes how much time the player has at the start of the game and keeps track of how much time (in seconds) the player has left
    [SerializeField] static float startTime = 600f, timeRemaining = startTime;

    //Initializes the timer to not running
    bool timerIsRunning = false;

    //Holds whether or not the player has won
    bool won = false;

    //Holds whether the game is over or not
    bool gameOver = false;

    //For the counter
    //Holds the maximum amount of items that can be collected for each charm
    [SerializeField] int[] maxItems = { 2, 2, 2, 2, 1, 1 };

    //Holds the amount of collected items for each charm
    [SerializeField] int[] itemsCollected = { 0, 0, 0, 0, 0, 0 };

    //Initalizes the space where the text is going to be put
    [SerializeField] TextMeshProUGUI counterTimerText, announcementText, endMessageText, timeTakenText, foundStatsText, destroyedStatsText;

    //Initializes the main and summary canvases
    [SerializeField] Canvas mainCanvas, endCanvas;

    //Sets and returns the number of winter charms collected
    public int winterItem
    {
        get { return itemsCollected[0]; }
        set { itemsCollected[0] = value; }
    }

    //Sets and returns the number of spring charms collected
    public int springItem
    {
        get { return itemsCollected[1]; }
        set { itemsCollected[1] = value; }
    }

    //Sets and returns the number of summer charms collected
    public int summerItem
    {
        get { return itemsCollected[2]; }
        set { itemsCollected[2] = value; }
    }

    //Sets and returns the number of autumn charms collected
    public int autumnItem
    {
        get { return itemsCollected[3]; }
        set { itemsCollected[3] = value; }
    }

    //Sets and returns the number of pestle and mortar charms collected
    public int mixItem
    {
        get { return itemsCollected[4]; }
        set { itemsCollected[4] = value; }
    }

    //Sets and returns the number of bottle charms collected
    public int bottleItem
    {
        get { return itemsCollected[5]; }
        set { itemsCollected[5] = value; }
    }

    //Start is called before the first frame update
    void Start()
    {
        //Starts the timer automatically
        timerIsRunning = true;

        //Gets the GameBehaviour script
        gameManager = GameObject.Find("GameManager").GetComponent<GameBehaviour>();

        //Gets the MoveCharacter script
        player = GameObject.Find("Slime").GetComponent<MoveCharacter>();

        //Gets the SoundControl script
        sound = GameObject.Find("Main Canvas").GetComponent<SoundControl>();

        //Enable mainCanvas
        mainCanvas.gameObject.SetActive(true);

        //Disable endCanvas
        endCanvas.gameObject.SetActive(false);
    }

    //Update is called once per frame
    void Update()
    {
        //If in autumn ...
        if (inSeason("Autumn", player.slimePosition)) {
            //Change text color to white
            counterTimerText.color = Color.white;
            announcementText.color = Color.white;
        }

        //If not in autumn ...
        else
        {
            //Change text color to black
            counterTimerText.color = Color.black;
            announcementText.color = Color.black;
        }

        
        //Checks to see if the timer is still running
        if (timerIsRunning)
        {
            //If time is still left ... 
            if (timeRemaining > 0)
            {
                //Subtract how much time has passed from timeRemaining
                timeRemaining -= Time.deltaTime;
            }

            //If time has run out ...
            else
            {
                //Set timeRemaining to 0, so there isn't negative time
                timeRemaining = 0;

                //Stop the timer from running
                timerIsRunning = false;

                //Game has ended
                gameOver = true;

                //Start end of game procedures
                GameEnd(timeRemaining);
            }
        }

        //Displays the counters and the time
        DisplayText(timeRemaining);

        //Checks to see if the game is over
        if (haveAllItems() && !gameOver)
        {
            //Game has ended
            gameOver = true;

            //Player won game
            won = true;

            //Start end of game procedures
            GameEnd(timeRemaining);
        }
    }

    void DisplayText(float timeToDisplay)
    {
        //Makes sure that the time is not negative
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        //Gets the number of minutes
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);

        //Gets the number of seconds
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        
        //Formats and puts the counters and timer into the text
        counterTimerText.text = string.Format("Snowflakes Found: {0:0}/{1:0}\nFlowers Found: {2:0}/{3:0}\nCrystals Found: {4:0}/{5:0}\nFruit: {6:0}/{7:0}\nPestle & Mortar: {8:0}/{9:0}\nBottle: {10:0}/{11:0}\n\nTime Remaining: {12:00}:{13:00}", itemsCollected[0], maxItems[0], itemsCollected[1], maxItems[1], itemsCollected[2], maxItems[2], itemsCollected[3], maxItems[3], itemsCollected[4], maxItems[4], itemsCollected[5], maxItems[5], minutes, seconds);
    
        //Display how many items are to be found if player is in winter
        if (inSeason("Winter", player.slimePosition))
        {
            if (gameManager.foundWinter == 0)
            {
                announcementText.text = string.Format("You have found all items in Winter!");
            }

            else if (gameManager.foundWinter == 1)
            {
                announcementText.text = string.Format("You have {0:0} item left to find in Winter!", gameManager.foundWinter);
            }

            else
            {
                announcementText.text = string.Format("You have {0:0} items left to find in Winter!", gameManager.foundWinter);
            }
        }

        //Display how many items are to be found if player is in spring
        else if (inSeason("Spring", player.slimePosition))
        {
            if (gameManager.foundSpring == 0)
            {
                announcementText.text = string.Format("You have found all items in Spring!");
            }

            else if (gameManager.foundSpring == 1)
            {
                announcementText.text = string.Format("You have {0:0} item left to find in Spring!", gameManager.foundSpring);
            }

            else
            {
                announcementText.text = string.Format("You have {0:0} items left to find in Spring!", gameManager.foundSpring);
            }
        }

        //Display how many items are to be found if player is in summer
        else if (inSeason("Summer", player.slimePosition))
        {
            if (gameManager.foundSummer == 0)
            {
                announcementText.text = string.Format("You have found all items in Summer!");
            }

            else if (gameManager.foundSummer == 1)
            {
                announcementText.text = string.Format("You have {0:0} item left to find in Summer!", gameManager.foundSummer);
            }

            else
            {
                announcementText.text = string.Format("You have {0:0} items left to find in Summer!", gameManager.foundSummer);
            }
        }

        //Display how many items are to be found if player is in autumn
        else if (inSeason("Autumn", player.slimePosition))
        {
            if (gameManager.foundAutumn == 0)
            {
                announcementText.text = string.Format("You have found all items in Autumn!");
            }

            else if (gameManager.foundAutumn == 1)
            {
                announcementText.text = string.Format("You have {0:0} item left to find in Autumn!", gameManager.foundAutumn);
            }

            else
            {
                announcementText.text = string.Format("You have {0:0} items left to find in Autumn!", gameManager.foundAutumn);
            }
        }
    }

    //End of game procedures
    void GameEnd(float timeToDisplay)
    {
        //Disable main canvas and enable the ending canvas
        mainCanvas.gameObject.SetActive(false);
        endCanvas.gameObject.SetActive(true);

        //Mute background music and sound effects
        sound.mute();

        //If player won ... 
        if (won)
        {
            //Play celebration music
            GameObject.Find("Win Music").GetComponent<AudioSource>().Play();

            //Display ending message
            endMessageText.text = string.Format("Congratulations! You have found all the charms and helped Serenade save the seasons!");
        }

        //If player did not win ...
        else
        {
            //Play sad music
            GameObject.Find("Lose Music").GetComponent<AudioSource>().Play();

            //Display ending message
            endMessageText.text = string.Format("Time's Up! Unfortunately, Roxie was able to destroy some of the charms.");
        }

        //Makes sure that the time is not negative
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        //Gets the number of minutes
        float minutes = Mathf.FloorToInt((startTime - timeToDisplay) / 60);

        //Gets the number of seconds
        float seconds = Mathf.FloorToInt((startTime - timeToDisplay) % 60);

        //Display the time taken
        timeTakenText.text = string.Format("Time Taken: {0:00}:{1:00}", minutes, seconds);

        //Display the ending stats
        EndingStats();
    }

    //Displays the ending statistics
    void EndingStats()
    {
        //Display the number of charms that were found
        foundStatsText.text = string.Format("Charms Found:\nSnowflakes: {0:0}/2\nFlowers: {1:0}/2\nCrystals: {2:0}/2\nFruits: {3:0}/2\nPestle & Mortar: {4:0}/1\nPotion Bottle: {5:0}/1", itemsCollected[0], itemsCollected[1], itemsCollected[2], itemsCollected[3], itemsCollected[4], itemsCollected[5]);

        //Display the number of charms that were not found
        destroyedStatsText.text = string.Format("Charms Destroyed:\nSnowflakes: {0:0}/2\nFlowers: {1:0}/2\nCrystals: {2:0}/2\nFruits: {3:0}/2\nPestle & Mortar: {4:0}/1\nPotion Bottle: {5:0}/1", 2-itemsCollected[0], 2-itemsCollected[1], 2-itemsCollected[2], 2-itemsCollected[3], 1-itemsCollected[4], 1-itemsCollected[5]);

    }

    //Returns whether or not all the items were found
    bool haveAllItems()
    {
        for (int i = 0; i < itemsCollected.Length; i++)
        {
            if (itemsCollected[i] != maxItems[i])
                return false;
        }

        return true;
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

    //Return whether the number is within the minimum and maximum values
    private bool inRange(float min, float max, float num)
    {
        return num <= max && num >= min;
    }
}