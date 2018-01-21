using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class sectorScript : MonoBehaviour {

    public GameObject gameMap; //references the "gameMap", which is used to display the map 
    public List<GameObject> neighbours = new List<GameObject>(); //stores a list of neighbouring sectors
    public GameObject[] unitsContained = new GameObject[3]; //stores all units currently placed in this sector [maximum: 3]
    public Vector3[] standingPoints = new Vector3[3]; //stores vectors representing locations for 3 units to stand in, on this sector
    public int sectorID; //this stores this sectors sectorID
    public bool isBusStop; //indicates whether this sector is a bus stop (2 players on distant bus stops should not be able to battle)

    public player owner; //this shows which player currently owns this sector. a sector is owned by a player if one of their unit's enters the sector.
                         //a player loses control of a sector if another player's unit enters a sector. each turn, each player earns money for the number of sectors
                         //that they control

    public void init(int sectorID, GameObject gameMap) //this function is used to initialize each sector, after it is instantiated
    {
        this.sectorID = sectorID; //set sectorID
        this.gameMap = gameMap; //set a reference to the gameMap
        this.transform.parent = gameMap.transform; //make this sector a child of gameMap object

        this.transform.position = getSectorCoordinates(sectorID); //set position of sector according to the function "getSectorCoordinates" which returns 
                                                                  //a hardcoded vector representing the position of a sector, given it's sectorID

        //scale sectors depending on their sector ID (sectors on east and west are scaled differently than in original artwork)
        if (new int[] { 0, 1, 2, 4, 5, 6, 8, 11, 12, 14, 15, 16, 17, 20, 24, 25, 26 }.Contains(sectorID)) //if this sector is part of west campus
        {
            //scale west sectors
            float scaleFactor = 4.6f;
            this.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
        else //if this sector is part of east campus
        {
            //scale east sectors
            float scaleFactorX = 3.4f;
            float scaleFactorY = 3f;
            this.transform.localScale = new Vector3(scaleFactorX, scaleFactorY, scaleFactorX);
            this.transform.Rotate(new Vector3(-12.388f, -12.388f, -12.388f));
        }
        if (new int[] { 13, 28, 10, 16, 20 }.Contains(sectorID)) //if this sector contains a bus stop
        {
            this.isBusStop = true;
        }
        else //if this sector does not contain a bus stop
        {
            this.isBusStop = false;
        }

        Object[] sectorSprites = Resources.LoadAll("mapSectors"); //load all map sector sprites 
        this.gameObject.GetComponent<SpriteRenderer>().sprite = (Sprite)sectorSprites[sectorID + 1]; //set current sprite to appropriate sector as indicated by sectorID 

        this.gameObject.AddComponent<PolygonCollider2D>(); //add polygoncollider component to this sector so that it can be clicked by the user
        this.gameObject.AddComponent<SpriteGlow.SpriteGlow>(); //add a SpriteGlow script to this sector so that a border can be drawn around it
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 0; //do not draw border around sector when it is created
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowBrightness = 5; //SpriteGlow settings, used to ensure borders can be drawn correctly
        this.gameObject.GetComponent<SpriteGlow.SpriteGlow>().AlphaThreshold = 0.5f; //SpriteGlow settings, used to assure borders can be drawn correctly


        this.gameObject.name = "mapSector" + sectorID.ToString(); //set this sectors name to "mapSector" followed by it's sectorID (E.G: "mapSector3")

        //set positions in which units standing in this sector will be placed. Some sectors (4, 26 and 7) have finely tuned standing points because of their awkward shapes
        switch (sectorID)
        {
            case 4: //fine tune standing points for sector 4
                standingPoints[0] = this.gameObject.transform.position;
                standingPoints[0].y += 0.67f;
                standingPoints[0].z = -1;

                standingPoints[1] = this.gameObject.transform.position;
                standingPoints[1].y += 0.17f;
                standingPoints[1].x += -0.2999993f;
                standingPoints[1].z = -2;

                standingPoints[2] = this.gameObject.transform.position;
                standingPoints[2].x += -0.69f;
                standingPoints[2].y += 1.32f;
                standingPoints[2].z = -2;
                break;
            case 26: //fine tune standing points for sector 26
                standingPoints[0] = this.gameObject.transform.position;
                standingPoints[0].y += 0.67f;
                standingPoints[0].z = -1;

                standingPoints[1] = this.gameObject.transform.position;
                standingPoints[1].x += -0.6f;
                standingPoints[1].y += 1.06f;
                standingPoints[1].z = -2;

                standingPoints[2] = this.gameObject.transform.position;
                standingPoints[2].y += 0.16f;
                standingPoints[2].x += 0.34f;
                standingPoints[2].z = -2;
                break;
            case 7: //fine tune standing points for sector 7
                standingPoints[0] = this.gameObject.transform.position;
                standingPoints[0].y += 0.67f;
                standingPoints[0].z = -1;

                standingPoints[1] = this.gameObject.transform.position;
                standingPoints[1].y += 0.17f;
                standingPoints[1].x += -0.2999993f;
                standingPoints[1].z = -2;

                standingPoints[2] = this.gameObject.transform.position;
                standingPoints[2].x += 0.14f;
                standingPoints[2].y += -0.27f;
                standingPoints[2].z = -2;
                break;
            default: //standing points for all other sectors
                standingPoints[0] = this.gameObject.transform.position;
                standingPoints[0].y += 0.67f;
                standingPoints[0].z = -1;

                standingPoints[1] = this.gameObject.transform.position;
                standingPoints[1].y += 0.17f;
                standingPoints[1].x += -0.2999993f;
                standingPoints[1].z = -2;

                standingPoints[2] = this.gameObject.transform.position;
                standingPoints[2].y += 0.16f;
                standingPoints[2].x += 0.34f;
                standingPoints[2].z = -2;
                break;
        }

        for (int i = 0; i < 3; i++) //set all unitsContained values to null because (no units should be placed until after a sector is created)
        {
            unitsContained[i] = null;
        }
    }

    void OnMouseDown() //when the user clicks on this sector
    {
        gameMapScript theGameMapScript = this.gameMap.GetComponent<gameMapScript>(); //set a reference to the gameMapScript script of the gameMap
        GameObject theSelectedUnit = theGameMapScript.selectedUnit; //set a reference to the gameMap's selected unit to see if a unit was selected before this sector was clicked on

        theGameMapScript.selectedUnit = null; //a sector has now been selected, so set the selectedUnit to null

        if (theSelectedUnit != null && theSelectedUnit.GetComponent<unitScript>().canMoveTo().Contains(this.gameObject)) //if a unit has been selected, and this sector has been clicked, move 
        {                                                                                                                //that unit to this sector, if it is within range
            unitScript theSelectedUnitScript = theSelectedUnit.GetComponent<unitScript>(); //set a reference to the selected unit's unitScript script
            theSelectedUnitScript.moveUnit(this.gameObject); //move unit to this sector
        }

    }

    public void setSectorNeighbours() //sets the list of all neighbour sectors ("neighbours") of a given sector
    {
        List<GameObject> sectors = this.gameMap.GetComponent<gameMapScript>().sectors; //reference to all sectors in the game map

        List<GameObject> neighbouringSectors = new List<GameObject>(); //list to store neighbouring sectors

        switch (this.sectorID) //neighbours are hardcoded depending on sectorID
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

        this.neighbours = neighbouringSectors; //set neighbours
    }

    Vector3 getSectorCoordinates(int sectorID) //returns a vector representing the location of a sector specified by sectorID
    {
        Vector3 coordsToReturn = new Vector3(0, 0, 0);

        switch (sectorID) //The locations for the sectors were originally hand placed and then their positions were recorded below
        {
            case 0: coordsToReturn = new Vector3(-11.93744f, 4.767906f, 0); break;
            case 1: coordsToReturn = new Vector3(-8.186342f, 5.209211f, 0); break;
            case 2: coordsToReturn = new Vector3(-4.391114f, 5.54019f, 0); break;
            case 3: coordsToReturn = new Vector3(6.886451f, 2.285561f, 0); break;
            case 4: coordsToReturn = new Vector3(-11.91537f, 0.06800067f, 0); break;
            case 5: coordsToReturn = new Vector3(-5.414943f, 2.411334f, 0); break;
            case 6: coordsToReturn = new Vector3(-9.444064f, 3.797033f, 0); break;
            case 7: coordsToReturn = new Vector3(10.75229f, 0.851318f, 0); break;
            case 8: coordsToReturn = new Vector3(-10.437f, -0.5277619f, 0); break;
            case 9: coordsToReturn = new Vector3(0.6463891f, 1.93693f, 0); break;
            case 10: coordsToReturn = new Vector3(3.662713f, 0.1562618f, 0); break;
            case 11: coordsToReturn = new Vector3(-8.387136f, -4.375947f, 0); break;
            case 12: coordsToReturn = new Vector3(-6.730034f, -2.513637f, 0); break;
            case 13: coordsToReturn = new Vector3(11.55326f, 2.519453f, 0); break;
            case 14: coordsToReturn = new Vector3(-11.60646f, 2.715834f, 0); break;
            case 15: coordsToReturn = new Vector3(-7.480254f, 2.870291f, 0); break;
            case 16: coordsToReturn = new Vector3(-3.662961f, 3.752903f, 0); break;
            case 17: coordsToReturn = new Vector3(-6.531446f, 4.966493f, 0); break;
            case 18: coordsToReturn = new Vector3(2.837471f, 1.967821f, 0); break;
            case 19: coordsToReturn = new Vector3(4.474716f, 2.338518f, 0); break;
            case 20: coordsToReturn = new Vector3(-7.403026f, -0.4174355f, 0); break;
            case 21: coordsToReturn = new Vector3(9.201098f, 2.892356f, 0); break;
            case 22: coordsToReturn = new Vector3(9.11063f, 1.552994f, 0); break;
            case 23: coordsToReturn = new Vector3(11.9063f, 0.930753f, 0); break;
            case 24: coordsToReturn = new Vector3(-9.669129f, -2.050266f, 0); break;
            case 25: coordsToReturn = new Vector3(-5.377432f, -4.620871f, 0); break;
            case 26: coordsToReturn = new Vector3(-9.547771f, 0.6416981f, 0); break;
            case 27: coordsToReturn = new Vector3(0.624324f, 0.1275768f, 0); break;
            case 28: coordsToReturn = new Vector3(5.776567f, 0.2290771f, 0); break;
            case 29: coordsToReturn = new Vector3(1.676838f, 0.7740896f, 0); break;
            case 30: coordsToReturn = new Vector3(8.470737f, 0.0260765f, 0); break;
        }

        return coordsToReturn;
    }
}
