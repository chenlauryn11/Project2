using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviour : MonoBehaviour
{
    //Declare variables
    //Initialize the y coorinates for the charms
    [SerializeField]
    float winterYCoor = 160.2f, springYCoor = 143.85f, summerYCoor = 144.77f, autumnYCoor = 143.82f, miscYCoor = 143.9297f;

    //Get the prefabs that will be used for charms
    [SerializeField]
    GameObject winter, spring, summer, autumn, mix, bottle;

    //Declare the arrays that will hold the charms to be distributed in each season
    [SerializeField]
    GameObject[] winterList, springList, summerList, autumnList;

    //Holds all possible ranges for charms to be in each season.
    //Vector3's x coordinate holds the mininum vale for each range of coordinates.
    //Vector3's z coordinate holds the maximum value for each range of coordinates.
    Vector3[] winterPosX, winterPosZ;
    Vector3[] springPosX, springPosZ;
    Vector3[] summerPosX, summerPosZ;
    Vector3[] autumnPosX, autumnPosZ;

    //Holds all indicies for ranges that have already been chosen for each season
    ArrayList chosenWinter = new ArrayList();
    ArrayList chosenSpring = new ArrayList();
    ArrayList chosenSummer = new ArrayList();
    ArrayList chosenAutumn = new ArrayList();

    //Holds the lengths for each seasonal list of prefabs
    int[] lengths = { 2, 2, 2, 2};

    //Holds the number of objects that were found in each season
    public ArrayList winterArrList = new ArrayList();
    public ArrayList springArrList = new ArrayList();
    public ArrayList summerArrList = new ArrayList();
    public ArrayList autumnArrList = new ArrayList();


    //Shares the number of items found in each season with other scripts/classes
    public int foundWinter = 2;
    public int foundSpring = 2;
    public int foundSummer = 2;
    public int foundAutumn = 2;

    // Start is called before the first frame update
    void Start()
    {
        //Initialize the range of positions that each charm can be found in
        InstantiateXZPos();

        //Get the number where pestle & mortar and bottle will go into
        int num1 = Mix();
        int num2 = Bottle();

        if (num1 == num2)
        {
            //Change the lengths of the prefab lists to incorporate the pestle & mortar and bottle prefabs
            switch(num1)
            {
                case 1:
                    lengths[0] = 4;
                    break;
                case 2:
                    lengths[1] = 4;
                    break;
                case 3:
                    lengths[2] = 4;
                    break;
                case 4:
                    lengths[3] = 4;
                    break;
            }
        }
        else
        {
            //Change the lengths of the prefab lists to incorporate the pestle & mortar prefab
            switch (num1)
            {
                case 1:
                    lengths[0] = 3;
                    break;
                case 2:
                    lengths[1] = 3;
                    break;
                case 3:
                    lengths[2] = 3;
                    break;
                case 4:
                    lengths[3] = 3;
                    break;
            }

            //Change the lengths of the prefab lists to incorporate the bottle prefab
            switch (num2)
            {
                case 1:
                    lengths[0] = 3;
                    break;
                case 2:
                    lengths[1] = 3;
                    break;
                case 3:
                    lengths[2] = 3;
                    break;
                case 4:
                    lengths[3] = 3;
                    break;
            }
        }

        //Initialize the prefab lists
        winterList = new GameObject[lengths[0]];
        springList = new GameObject[lengths[1]];
        summerList = new GameObject[lengths[2]];
        autumnList = new GameObject[lengths[3]];

        //Instantiate winter charms and add them to winterList
        winterList[0] = Instantiate(winter);
        winterList[1] = Instantiate(winter);

        //Instantiate spring charms and add them to springList
        springList[0] = Instantiate(spring);
        springList[1] = Instantiate(spring);

        //Instantiate summer charms and add them to summerList
        summerList[0] = Instantiate(summer);
        summerList[1] = Instantiate(summer);

        //Instantiate autumn charms and add them to autumnList
        autumnList[0] = Instantiate(autumn);
        autumnList[1] = Instantiate(autumn);

        //Instantiate pestle & mortar and bottle charms and add them to the list of one of the four seasons
        if (num1 == num2)
        {
            //Add both pestle & mortar and bottle into the seasonal prefab lists
            switch (num1)
            {
                case 1:
                    winterList[2] = Instantiate(mix);
                    winterList[3] = Instantiate(bottle);
                    break;
                case 2:
                    springList[2] = Instantiate(mix);
                    springList[3] = Instantiate(bottle);
                    break;
                case 3:
                    summerList[2] = Instantiate(mix);
                    summerList[3] = Instantiate(bottle);
                    break;
                case 4:
                    autumnList[2] = Instantiate(mix);
                    autumnList[3] = Instantiate(bottle);
                    break;
            }
        }
        else
        {
            //Add  pestle & mortar into the seasonal prefab lists
            switch (num1)
            {
                case 1:
                    winterList[2] = Instantiate(mix);
                    break;
                case 2:
                    springList[2] = Instantiate(mix);
                    break;
                case 3:
                    summerList[2] = Instantiate(mix);
                    break;
                case 4:
                    autumnList[2] = Instantiate(mix);
                    break;
            }

            //Add  bottle into the seasonal prefab lists
            switch (num2)
            {
                case 1:
                    winterList[2] = Instantiate(bottle);
                    break;
                case 2:
                    springList[2] = Instantiate(bottle);
                    break;
                case 3:
                    summerList[2] = Instantiate(bottle);
                    break;
                case 4:
                    autumnList[2] = Instantiate(bottle);
                    break;
            }
        }

        //Randomly distribute the charms within their seasons
        Winter();
        Spring();
        Summer();
        Autumn();
    }

    //Update is called once per frame
    void Update()
    {
        //Update the number of items found in each season
        foundWinter = winterList.Length - winterArrList.Count;
        foundSpring = springList.Length - springArrList.Count;
        foundSummer = summerList.Length - summerArrList.Count;
        foundAutumn = autumnList.Length - autumnArrList.Count;
    }

    //Distributes Winter charms
    void Winter()
    {
        for (int i = 0; i < winterList.Length; i++)
        {
            //Chooses the random ranges where the charm to be placed
            int index = chooseIndex(winterPosX, chosenWinter);

            //Seperate the x and z coordinates
            Vector3 x = winterPosX[index];
            Vector3 z = winterPosZ[index];

            //Specify the x and z coordinates
            float xCoor = getRandF(x.x, x.z);
            float zCoor = getRandF(z.x, z.z);

            //Place the charm on the coordinates
            if (i == 2 || i == 3)
            {
                winterList[i].transform.position = new Vector3(xCoor, miscYCoor, zCoor); 
            }
            else
            {
                winterList[i].transform.position = new Vector3(xCoor, winterYCoor, zCoor);
            }

            //Add the chosen index into the used coordinates arraylist
            chosenWinter.Add(index);
        }
    }

    //Distributes Spring charms
    void Spring()
    {
        for (int i = 0; i < springList.Length; i++)
        {
            //Chooses the random ranges where the charm to be placed
            int index = chooseIndex(springPosX, chosenSpring);

            //Seperate the x and z coordinates
            Vector3 x = springPosX[index];
            Vector3 z = springPosZ[index];

            //Specify the x and z coordinates
            float xCoor = getRandF(x.x, x.z);
            float zCoor = getRandF(z.x, z.z);

            //Place the charm on the coordinates
            if (i == 2 || i == 3)
            {
                springList[i].transform.position = new Vector3(xCoor, miscYCoor, zCoor);
            }
            else
            {
                springList[i].transform.position = new Vector3(xCoor, springYCoor, zCoor);
            }

            //Add the chosen index into the used coordinates arraylist
            chosenSpring.Add(index);
        }
    }

    //Distributes Summer charms
    void Summer()
    {
        for (int i = 0; i < summerList.Length; i++)
        {
            //Chooses the random ranges where the charm to be placed
            int index = chooseIndex(summerPosX, chosenSummer);

            //Seperate the x and z coordinates
            Vector3 x = summerPosX[index];
            Vector3 z = summerPosZ[index];

            //Specify the x and z coordinates
            float xCoor = getRandF(x.x, x.z);
            float zCoor = getRandF(z.x, z.z);

            //Place the charm on the coordinates
            if (i == 2 || i == 3)
            {
                summerList[i].transform.position = new Vector3(xCoor, miscYCoor, zCoor);
            }
            else
            {
                summerList[i].transform.position = new Vector3(xCoor, summerYCoor, zCoor);
            }

            //Add the chosen index into the used coordinates arraylist
            chosenSummer.Add(index);
        }
    }

    //Distributes Autumn charms
    void Autumn()
    {
        for (int i = 0; i < autumnList.Length; i++)
        {
            //Chooses the random ranges where the charm to be placed
            int index = chooseIndex(autumnPosX, chosenAutumn);

            //Seperate the x and z coordinates
            Vector3 x = autumnPosX[index];
            Vector3 z = autumnPosZ[index];

            //Specify the x and z coordinates
            float xCoor = getRandF(x.x, x.z);
            float zCoor = getRandF(z.x, z.z);

            //Place the charm on the coordinates
            if (i == 2 || i == 3)
            {
                autumnList[i].transform.position = new Vector3(xCoor, miscYCoor, zCoor);
            }
            else
            {
                autumnList[i].transform.position = new Vector3(xCoor, autumnYCoor, zCoor);
            }

            //Add the chosen index into the used coordinates arraylist
            chosenAutumn.Add(index);
        }
    }

    //Distributes Pestle & Mortar charm
    int Mix()
    {
        return getRandI(1, 4);
    }

    //Distributes Bottle charm
    int Bottle()
    {
        return getRandI(1, 4);
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
        return (int) getRandF(min, max);
    }

    //Chooses a random index that has not already been chosen
    int chooseIndex(Vector3[] arr, ArrayList arrList)
    {
        int rand = 0;

        //Get a new number if the old number has already been used
        do
        {
            rand = getRandI(0, arr.Length - 1);
        }
        while (inList(rand, arrList));

        //Return the new number that hasn't been used before
        return rand;
    }

    //Returns whether or not the number is in the arraylist
    bool inList(int num, ArrayList arr)
    {
        return arr.Contains(num);
    }

    //Instantiates all the possible x and z values for each charm to be distributed in each season
    void InstantiateXZPos()
    {
        XZWinter();
        XZSpring();
        XZSummer();
        XZAutumn();
    }

    //Instantiates all the possible x and z values for each charm to be distributed in winter
    void XZWinter()
    {
        int len = 12;
        winterPosX = new Vector3[len];
        winterPosZ = new Vector3[len];

        winterPosX[0] = new Vector3(385f, 0f, 755f);
        winterPosZ[0] = new Vector3(-745f, 0f, -700f);

        winterPosX[1] = new Vector3(405f, 0f, 460f);
        winterPosZ[1] = new Vector3(-645f, 0f, -515f);

        winterPosX[2] = new Vector3(220f, 0f, 460f);
        winterPosZ[2] = new Vector3(-500f, 0f, -475f);

        winterPosX[3] = new Vector3(230f, 0f, 290f);
        winterPosZ[3] = new Vector3(-615f, 0f, -505f);

        winterPosX[4] = new Vector3(660f, 0f, 810f);
        winterPosZ[4] = new Vector3(-610f, 0f, -550f);

        winterPosX[5] = new Vector3(620f, 0f, 635f);
        winterPosZ[5] = new Vector3(-595f, 0f, -475f);

        winterPosX[6] = new Vector3(510f, 0f, 625f);
        winterPosZ[6] = new Vector3(-460f, 0f, -250f);

        winterPosX[7] = new Vector3(750f, 0f, 790f);
        winterPosZ[7] = new Vector3(-360f, 0f, -205f);

        winterPosX[8] = new Vector3(585f, 0f, 730f);
        winterPosZ[8] = new Vector3(-220f, 0f, -190f);

        winterPosX[9] = new Vector3(210f, 0f, 410f);
        winterPosZ[9] = new Vector3(-225f, 0f, -175f);

        winterPosX[10] = new Vector3(200f, 0f, 415f);
        winterPosZ[10] = new Vector3(-365f, 0f, -360f);

        winterPosX[11] = new Vector3(185f, 0f, 220f);
        winterPosZ[11] = new Vector3(-350f, 0f, -240f);
    }

    //Instantiates all the possible x and z values for each charm to be distributed in spring
    void XZSpring()
    {
        int len = 6;
        springPosX = new Vector3[len];
        springPosZ = new Vector3[len];

        springPosX[0] = new Vector3(425f, 0f, 555f);
        springPosZ[0] = new Vector3(370f, 0f, 480f);

        springPosX[1] = new Vector3(745f, 0f, 825f);
        springPosZ[1] = new Vector3(405f, 0f, 545f);

        springPosX[2] = new Vector3(450f, 0f, 570f);
        springPosZ[2] = new Vector3(210f, 0f, 315f);

        springPosX[3] = new Vector3(750f, 0f, 830f);
        springPosZ[3] = new Vector3(675f, 0f, 720f);

        springPosX[4] = new Vector3(560f, 0f, 645f);
        springPosZ[4] = new Vector3(630f, 0f, 720f);

        springPosX[5] = new Vector3(325f, 0f, 385f);
        springPosZ[5] = new Vector3(570f, 0f, 680f);
    }

    //Instantiates all the possible x and z values for each charm to be distributed in summer
    void XZSummer()
    {
        int len = 4;
        summerPosX = new Vector3[len];
        summerPosZ = new Vector3[len];
        
        summerPosX[0] = new Vector3(1650f, 0f, 1780f);
        summerPosZ[0] = new Vector3(380f, 0f, 500f);

        summerPosX[1] = new Vector3(1165f, 0f, 1310f);
        summerPosZ[1] = new Vector3(390f, 0f, 450f);

        summerPosX[2] = new Vector3(1125f, 0f, 1295f);
        summerPosZ[2] = new Vector3(640f, 0f, 785f);

        summerPosX[3] = new Vector3(1325f, 0f, 1650f);
        summerPosZ[3] = new Vector3(415f, 0f, 800f);
    }

    //Instantiates all the possible x and z values for each charm to be distributed in autumn
    void XZAutumn()
    {
        int len = 4;
        autumnPosX = new Vector3[len];
        autumnPosZ = new Vector3[len];

        autumnPosX[0] = new Vector3(1255f, 0f, 1355f);
        autumnPosZ[0] = new Vector3(-745f, 0f, -255f);

        autumnPosX[1] = new Vector3(1605f, 0f, 1680f);
        autumnPosZ[1] = new Vector3(-745f, 0f, -255f);

        autumnPosX[2] = new Vector3(1360f, 0f, 1600f);
        autumnPosZ[2] = new Vector3(-325f, 0f, -230f);

        autumnPosX[3] = new Vector3(1230f, 0f, 1600f);
        autumnPosZ[3] = new Vector3(-725f, 0f, -620f);
    }
}