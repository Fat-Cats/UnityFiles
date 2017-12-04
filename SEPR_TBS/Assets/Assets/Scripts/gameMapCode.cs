using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameMapCode : MonoBehaviour {

    public Transform sector; //prefab for sector
    public GameObject unit; //prefab for units
    private List<GameObject> sectors = new List<GameObject>();

    private GameObject _selectedUnit; //used for maintaining current value of selectedUnit (needed for getter and setter use)
    public GameObject selectedUnit //used for selecting and interacting with units
    {
        get { return this._selectedUnit; }

        set
        {
            if (selectedUnit != null) //if a unit has previously been selected, remove its borders before changing its value
            {
                selectedUnit.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 0; //remove old selected unit border

                foreach (GameObject sect in selectedUnit.GetComponent<unitScript>().canMoveTo()) //remove old selected unit "can move to" indicators
                {
                    sect.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 0;
                }
            }

            _selectedUnit = value; //set new value

            if (selectedUnit != null)
            {
                selectedUnit.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 3; //add new selected unit border
                selectedUnit.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowColor = Color.red;

                foreach (GameObject sect in selectedUnit.GetComponent<unitScript>().canMoveTo()) //add new selected unit "can move to" indicators
                {
                    sect.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 3;
                    sect.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowColor = Color.red;
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
        
		for (int i = 0; i <= 30; i++) //instantiate 30 sector prefabs, set relevant x and y positions on the map and assign them appropriate sectorID's
        {
            Transform createdSector = Instantiate(sector, getSectorCoordinates(i), Quaternion.identity); //instantiate sector prefab and

            //scale sectors depending on their sector ID (sectors on east and west are scaled differently than originally sized in artwork)
            if (i == 0 || i == 1 || i == 2 || i == 4 || i == 5 || i == 6 || i == 8 || i == 11 || i == 12 || i == 14 || i == 15  || i == 16 || i == 17 || i == 20 || i == 24 || i == 25 || i == 26)
            {
                //scale west sectors
                float scaleFactor = 0.9183525f;
                createdSector.localScale = new Vector3(scaleFactor , scaleFactor, scaleFactor);
            }
            else
            {
                //scale east sectors
                float scaleFactor = 0.8262805f;
                createdSector.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }

            if (i == 4 || i == 7)
            {
                GameObject createUnit = Instantiate(unit, new Vector3(0,0,0), Quaternion.identity);
                createUnit.GetComponent<unitScript>().Init("Basic", createdSector.gameObject);
            }

            createdSector.transform.SetParent(this.gameObject.transform); //set gameMap as sector's parent

            createdSector.GetComponent<sectorScript>().init(i); //perform additional initilization of sector (pass sectorID, choose sector sprite etc...)

            createdSector.gameObject.name = "mapSector" + i.ToString();

            sectors.Add(createdSector.gameObject);
        }
            this.selectedUnit = null; //at the start of the game, not units are selected

        for (int i = 0; i <= 30; i++) //now that the sectors have been created, we can loop through them again and add a list of neighbours to each of them
        {
            sectors[i].GetComponent<sectorScript>().neighbours = getSectorNeighbours(i); //used "getSectorNeighbours" to return neighbours of given sector
        }
    }


    List<GameObject> getSectorNeighbours (int sectorID) //returns a list of all neighbour sectors of a given sector
    {
        List<GameObject> neighbouringSectors = new List<GameObject>();

        switch(sectorID)
        {
            case 0:
                neighbouringSectors.Add(sectors[14]);
                neighbouringSectors.Add(sectors[6]);
                break;
            case 1:
                neighbouringSectors.Add(sectors[17]);
                break;
            case 2:
                neighbouringSectors.Add(sectors[17]);
                neighbouringSectors.Add(sectors[16]);
                break;
            case 3:
                neighbouringSectors.Add(sectors[19]);
                neighbouringSectors.Add(sectors[28]);
                neighbouringSectors.Add(sectors[30]);
                neighbouringSectors.Add(sectors[22]);
                neighbouringSectors.Add(sectors[21]);
                break;
            case 4:
                neighbouringSectors.Add(sectors[14]);
                neighbouringSectors.Add(sectors[8]);
                break;
            case 5:
                neighbouringSectors.Add(sectors[15]);
                neighbouringSectors.Add(sectors[20]);
                break;
            case 6:
                neighbouringSectors.Add(sectors[0]);
                neighbouringSectors.Add(sectors[14]);
                neighbouringSectors.Add(sectors[26]);
                neighbouringSectors.Add(sectors[15]);
                break;
            case 7:
                neighbouringSectors.Add(sectors[30]);
                neighbouringSectors.Add(sectors[22]);
                neighbouringSectors.Add(sectors[13]);
                neighbouringSectors.Add(sectors[23]);
                break;
            case 8:
                neighbouringSectors.Add(sectors[4]);
                neighbouringSectors.Add(sectors[26]);
                neighbouringSectors.Add(sectors[24]);
                break;
            case 9:
                neighbouringSectors.Add(sectors[27]);
                neighbouringSectors.Add(sectors[29]);
                neighbouringSectors.Add(sectors[18]);
                break;
            case 10:
                neighbouringSectors.Add(sectors[29]);
                neighbouringSectors.Add(sectors[18]);
                neighbouringSectors.Add(sectors[19]);
                neighbouringSectors.Add(sectors[16]);
                neighbouringSectors.Add(sectors[20]);
                neighbouringSectors.Add(sectors[28]);
                neighbouringSectors.Add(sectors[13]);
                break;
            case 11:
                neighbouringSectors.Add(sectors[24]);
                neighbouringSectors.Add(sectors[12]);
                neighbouringSectors.Add(sectors[25]);
                break;
            case 12:
                neighbouringSectors.Add(sectors[25]);
                neighbouringSectors.Add(sectors[11]);
                neighbouringSectors.Add(sectors[20]);
                break;
            case 13:
                neighbouringSectors.Add(sectors[16]);
                neighbouringSectors.Add(sectors[20]);
                neighbouringSectors.Add(sectors[10]);
                neighbouringSectors.Add(sectors[28]);
                neighbouringSectors.Add(sectors[21]);
                neighbouringSectors.Add(sectors[7]);
                neighbouringSectors.Add(sectors[23]);
                break;
            case 14:
                neighbouringSectors.Add(sectors[0]);
                neighbouringSectors.Add(sectors[6]);
                neighbouringSectors.Add(sectors[4]);
                break;
            case 15:
                neighbouringSectors.Add(sectors[6]);
                neighbouringSectors.Add(sectors[5]);
                neighbouringSectors.Add(sectors[17]);
                break;
            case 16:
                neighbouringSectors.Add(sectors[17]);
                neighbouringSectors.Add(sectors[2]);
                neighbouringSectors.Add(sectors[20]);
                neighbouringSectors.Add(sectors[10]);
                neighbouringSectors.Add(sectors[28]);
                neighbouringSectors.Add(sectors[13]);
                break;
            case 17:
                neighbouringSectors.Add(sectors[1]);
                neighbouringSectors.Add(sectors[2]);
                neighbouringSectors.Add(sectors[16]);
                neighbouringSectors.Add(sectors[15]);
                break;
            case 18:
                neighbouringSectors.Add(sectors[9]);
                neighbouringSectors.Add(sectors[29]);
                neighbouringSectors.Add(sectors[10]);
                neighbouringSectors.Add(sectors[19]);
                break;
            case 19:
                neighbouringSectors.Add(sectors[18]);
                neighbouringSectors.Add(sectors[10]);
                neighbouringSectors.Add(sectors[28]);
                neighbouringSectors.Add(sectors[3]);
                break;
            case 20:
                neighbouringSectors.Add(sectors[16]);
                neighbouringSectors.Add(sectors[10]);
                neighbouringSectors.Add(sectors[28]);
                neighbouringSectors.Add(sectors[13]);
                neighbouringSectors.Add(sectors[24]);
                neighbouringSectors.Add(sectors[26]);
                neighbouringSectors.Add(sectors[5]);
                neighbouringSectors.Add(sectors[12]);
                break;
            case 21:
                neighbouringSectors.Add(sectors[3]);
                neighbouringSectors.Add(sectors[22]);
                neighbouringSectors.Add(sectors[13]);
                break;
            case 22:
                neighbouringSectors.Add(sectors[3]);
                neighbouringSectors.Add(sectors[21]);
                neighbouringSectors.Add(sectors[7]);
                neighbouringSectors.Add(sectors[30]);
                break;
            case 23:
                neighbouringSectors.Add(sectors[7]);
                neighbouringSectors.Add(sectors[13]);
                break;
            case 24:
                neighbouringSectors.Add(sectors[8]);
                neighbouringSectors.Add(sectors[26]);
                neighbouringSectors.Add(sectors[20]);
                neighbouringSectors.Add(sectors[11]);
                break;
            case 25:
                neighbouringSectors.Add(sectors[12]);
                neighbouringSectors.Add(sectors[11]);
                break;
            case 26:
                neighbouringSectors.Add(sectors[8]);
                neighbouringSectors.Add(sectors[24]);
                neighbouringSectors.Add(sectors[20]);
                neighbouringSectors.Add(sectors[6]);
                break;
            case 27:
                neighbouringSectors.Add(sectors[9]);
                neighbouringSectors.Add(sectors[29]);
                break;
            case 28:
                neighbouringSectors.Add(sectors[16]);
                neighbouringSectors.Add(sectors[20]);
                neighbouringSectors.Add(sectors[13]);
                neighbouringSectors.Add(sectors[10]);
                neighbouringSectors.Add(sectors[19]);
                neighbouringSectors.Add(sectors[3]);
                neighbouringSectors.Add(sectors[30]);
                break;
            case 29:
                neighbouringSectors.Add(sectors[27]);
                neighbouringSectors.Add(sectors[9]);
                neighbouringSectors.Add(sectors[18]);
                neighbouringSectors.Add(sectors[10]);
                break;
            case 30:
                neighbouringSectors.Add(sectors[28]);
                neighbouringSectors.Add(sectors[3]);
                neighbouringSectors.Add(sectors[22]);
                neighbouringSectors.Add(sectors[7]);
                break;
        }

        return neighbouringSectors;
    }

    Vector3 getSectorCoordinates(int sectorID) //returns a vector representing the location of a sector specified by sectorID
    {
        Vector3 coordsToReturn = new Vector3(0, 0, 0);
        
        switch (sectorID) //The locations for the sectors were originally hand placed and then their positions were recorded below
        {
            case 1: coordsToReturn = new Vector3(-10.235f, 4.892f, 0); break;
            case 2: coordsToReturn = new Vector3(-6.789f, 5.195f, 0); break;
            case 3: coordsToReturn = new Vector3(7.785001f, -0.362f, 0); break;
            case 4: coordsToReturn = new Vector3(-13.635f, -0.548f, 0); break;
            case 5: coordsToReturn = new Vector3(-7.599f, 1.705f, 0); break;
            case 6: coordsToReturn = new Vector3(-11.214f, 2.909f, 0); break;
            case 7: coordsToReturn = new Vector3(12.364f, -1.072f, 0); break;
            case 8: coordsToReturn = new Vector3(-12.251f, -1.065f, 0); break;
            case 9: coordsToReturn = new Vector3(1.036f, -2.44f, 0); break;
            case 10: coordsToReturn = new Vector3(4.886f, -3.818f, 0); break;
            case 11: coordsToReturn = new Vector3(-9.39f, -4.264f, 0); break;
            case 12: coordsToReturn = new Vector3(-7.858f, -2.541f, 0); break;
            case 13: coordsToReturn = new Vector3(12.841f, 1.158f, 0); break;
            case 14: coordsToReturn = new Vector3(-13.252f, 1.896f, 0); break;
            case 15: coordsToReturn = new Vector3(-9.423f, 2.043f, 0); break;
            case 16: coordsToReturn = new Vector3(-6.203f, 3.529f, 0); break;
            case 17: coordsToReturn = new Vector3(-8.782f, 4.734f, 0); break;
            case 18: coordsToReturn = new Vector3(3.428f, -1.822f, 0); break;
            case 19: coordsToReturn = new Vector3(5.13f, -0.9400001f, 0); break;
            case 20: coordsToReturn = new Vector3(-9.446f, -1.043f, 0); break;
            case 21: coordsToReturn = new Vector3(10.175f, 0.985f, 0); break;
            case 22: coordsToReturn = new Vector3(10.398f, -0.6570001f, 0); break;
            case 23: coordsToReturn = new Vector3(13.61f, -0.6670001f, 0); break;
            case 24: coordsToReturn = new Vector3(-11.541f, -2.473f, 0); break;
            case 25: coordsToReturn = new Vector3(-6.609f, -4.489f, 0); break;
            case 26: coordsToReturn = new Vector3(-11.428f, 0.01499999f, 0); break;
            case 27: coordsToReturn = new Vector3(1.452f, -4.629f, 0); break;
            case 28: coordsToReturn = new Vector3(7.065001f, -3.139f, 0); break;
            case 29: coordsToReturn = new Vector3(2.505f, -3.605f, 0); break;
            case 30: coordsToReturn = new Vector3(10.064f, -2.673f, 0); break;
            case 0: coordsToReturn = new Vector3(-13.613f, 3.799f, 0); break;
        }

        return coordsToReturn;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
