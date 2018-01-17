using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class gameMapScript : MonoBehaviour
{
    public Transform attackCanvas; //stores canvas, which is passed to units and buildings upon instantiation
    public Transform buyUnitMenu; //stores canvas, which is passed to units and buildings upon instantiation
    public Transform sector; //prefab for sector (neccessary to point unity to the correct prefab)
    public GameObject unit; //prefab for units (neccessary to point unity to the correct prefab)
    public GameObject building; //prefab for buildings (neccessary to point unity to the correct prefab)
    private List<GameObject> sectors = new List<GameObject>(); //list of all instances of the sector prefab
    private GameObject _selectedUnit; //needed to store intermediate values during getter and setter use
    public GameObject selectedUnit //this variable stores the last unit that has been clicked on by a player
    {
        get { return this._selectedUnit; } //return selectedUnit value when requested

        set //when a new unit is selected, this code highlights that unit and un-highlights the previously selected unit
        {
            if (value != null && _selectedUnit != null) //check if a new unit has been selected whilst another unit is selected
            {
                //if a unit has been selected, and an opponents unit is then selected then they will engage in battle (if they are in range of eachother)
                if (_selectedUnit.GetComponent<unitScript>().owner != value.GetComponent<unitScript>().owner)
                {
                    //give that the selected unit can move to a newly clickd on enemy unit, battle commences 
                    if ( _selectedUnit.GetComponent<unitScript>().canMoveTo().Contains(value.transform.parent.gameObject))
                    {
                        _selectedUnit.GetComponent<unitScript>().attackUnit(value); //previously selected unit attacks newly selected unit
                    }
                    else //if a unit has chosen to attack an out of range enemy unit, unselect that unit
                    {
                        selectedUnit = null; //unselect current unit
                    }

                    value = null;

                }
            }

            if (_selectedUnit != null) //if a unit has previously been selected, remove its borders before changing selectedUnit's value
            {
                _selectedUnit.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 0; //remove old selected unit border

                foreach (GameObject sect in _selectedUnit.GetComponent<unitScript>().canMoveTo()) //remove old selected unit's "can move to" indicators
                {
                    sect.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 0;
                }
            }

            _selectedUnit = value; //set new selectedUnit value

            if (selectedUnit != null) //if the new unit is not null, highlight it and the sectors to which it can move
            {
                selectedUnit.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 3; //draw border around unit
                selectedUnit.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowColor = selectedUnit.gameObject.GetComponent<unitScript>().owner.teamColour; //color unit's border using it's team's colour

                foreach (GameObject sect in selectedUnit.GetComponent<unitScript>().canMoveTo()) //add new selected unit's "can move to" indicators
                {
                    sect.gameObject.GetComponent<SpriteGlow.SpriteGlow>().OutlineWidth = 3;
                    sect.gameObject.GetComponent<SpriteGlow.SpriteGlow>().GlowColor = selectedUnit.gameObject.GetComponent<unitScript>().owner.teamColour;
                }
            }
        }
    }

    void Start()
    {

        for (int i = 0; i <= 30; i++) //instantiate 30 sector prefabs, set relevant x and y positions on the map and assign appropriate sectorID's
        {
            Transform createdSector = Instantiate(sector, getSectorCoordinates(i), Quaternion.identity); //instantiate new sector and set its position according to
                                                                                                         //"getSectorCoordinates(i)" which stores sector positions
                                                                                                         //according to sectorID's

            //scale sectors depending on their sector ID (sectors on east and west are scaled differently than in original artwork)
            if (i == 0 || i == 1 || i == 2 || i == 4 || i == 5 || i == 6 || i == 8 || i == 11 || i == 12 || i == 14 || i == 15 || i == 16 || i == 17 || i == 20 || i == 24 || i == 25 || i == 26)
            {
                //scale west sectors
                float scaleFactor = 0.99373699113f;
                createdSector.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            }
            else
            {
                //scale east sectors
                createdSector.localScale = new Vector3(0.73697657572f, 0.66770329348f, 0.71634995268f);
                createdSector.Rotate(new Vector3(-12.388f, -12.388f, -12.388f));
            }

            createdSector.transform.SetParent(this.gameObject.transform); //set gameMap as the parent to all sectors

            createdSector.GetComponent<sectorScript>().init(i); //perform additional initilization of sector (pass sectorID, choose sector sprite etc...)

            sectors.Add(createdSector.gameObject); //add this sector to list of sectors stored in gameMapCode
        }

        this.selectedUnit = null; //at the start of the game, no units are selected

        for (int i = 0; i <= 30; i++) //now all sectors have been created, loop through them again and add a list of neighbours to each of them
        {
            sectors[i].GetComponent<sectorScript>().neighbours = getSectorNeighbours(i); //set sector neighbours according to "getSectorCoordinates(i)"
                                                                                         //which returns a list of neighbours according to sectorID's 
        }

        //instantiate and place buildings
        
        Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<buildingScript>().Init("CentralHall", sectors[15], buyUnitMenu.gameObject, attackCanvas.gameObject );
        Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<buildingScript>().Init("HesHall", sectors[20], buyUnitMenu.gameObject, attackCanvas.gameObject); 
        Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<buildingScript>().Init("JBMorrell", sectors[17], buyUnitMenu.gameObject, attackCanvas.gameObject); 
        Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<buildingScript>().Init("Nisa", sectors[6], buyUnitMenu.gameObject, attackCanvas.gameObject);
        Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<buildingScript>().Init("RCH", sectors[10], buyUnitMenu.gameObject, attackCanvas.gameObject);
        Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<buildingScript>().Init("SportsVillage", sectors[13], buyUnitMenu.gameObject, attackCanvas.gameObject);
        Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<buildingScript>().Init("track", sectors[24], buyUnitMenu.gameObject, attackCanvas.gameObject);
        Instantiate(building, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<buildingScript>().Init("Nisa", sectors[25], buyUnitMenu.gameObject, attackCanvas.gameObject);

        //create 3 basic units for testing
        Instantiate(unit, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<unitScript>().Init("Basic", GetComponentInParent<gameMainScript>().playerList[1], sectors[1], attackCanvas.gameObject);
        Instantiate(unit, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<unitScript>().Init("Basic", GetComponentInParent<gameMainScript>().playerList[0], sectors[3], attackCanvas.gameObject);
        Instantiate(unit, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<unitScript>().Init("Basic", GetComponentInParent<gameMainScript>().playerList[0], sectors[4], attackCanvas.gameObject);
        Instantiate(unit, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<unitScript>().Init("Basic", GetComponentInParent<gameMainScript>().playerList[0], sectors[17], attackCanvas.gameObject);

        attackCanvas.gameObject.SetActive(false); //make canvas not visible (will be made visible during battles)
        buyUnitMenu.gameObject.SetActive(false);
    }


    List<GameObject> getSectorNeighbours(int sectorID) //returns a list of all neighbour sectors of a given sector
    {
        List<GameObject> neighbouringSectors = new List<GameObject>(); //use list to store neighbouring sectors

        switch (sectorID) //hardcoded neighbour sectors
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
